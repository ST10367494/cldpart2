using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EventVenueBooking1.Models.Entities;
using EventVenueBooking1.Models.ViewModels;
using EventVenueBooking1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EventVenueBooking1.Controllers
{
    public class VenueController : Controller
    {
        private readonly TableStorageService _tableService;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "venueimages";
        private readonly ILogger<VenueController> _logger;

        public VenueController(TableStorageService tableService, BlobServiceClient blobServiceClient, ILogger<VenueController> logger)
        {
            _tableService = tableService;
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new AddVenueViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddVenueViewModel viewModel)
        {
            _logger.LogInformation("Entered Add POST method.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid.");
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        _logger.LogError($"ModelState error in '{key}': {error.ErrorMessage}");
                    }
                }
                return View(viewModel);
            }

            var existingVenues = await _tableService.GetAllVenuesAsync();
            if (existingVenues.Any(v => v.Name == viewModel.Name && v.Location == viewModel.Location))
            {
                ModelState.AddModelError("", "This venue already exists.");
                _logger.LogWarning("Duplicate venue detected.");
                return View(viewModel);
            }

            string imageUrl = null;
            if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
            {
                imageUrl = await UploadFileToBlobStorage(viewModel.ImageFile);
            }

            var venue = new Venue
            {
                PartitionKey = "Venues",
                RowKey = Guid.NewGuid().ToString(),
                Name = viewModel.Name,
                Location = viewModel.Location,
                Capacity = viewModel.Capacity,
                Booked = viewModel.Booked,
                BookingDate = viewModel.BookingDate,
                ImageFileName = imageUrl
            };

            await _tableService.AddVenueAsync(venue);
            _logger.LogInformation("Venue saved successfully.");
            return RedirectToAction("List");
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
            var venues = await _tableService.GetAllVenuesAsync();
            return View(venues);
        }

        private async Task<string> UploadFileToBlobStorage(IFormFile file)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var blobClient = containerClient.GetBlobClient(fileName);

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobHttpHeaders
                    {
                        ContentType = file.ContentType
                    });
                }

                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading to Azure Blob Storage");
                throw;
            }
        }
    }
}
