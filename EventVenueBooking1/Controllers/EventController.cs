using EventVenueBooking1.Models.Entities;
using EventVenueBooking1.Models.ViewModels;
using EventVenueBooking1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventVenueBooking.Controllers
{
    public class EventController : Controller
    {
        private readonly TableStorageService _tableService;

        public EventController(TableStorageService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var venues = await _tableService.GetAllVenuesAsync();

            var viewModel = new AddEventViewModel
            {
                Venues = venues.Select(v => new SelectListItem
                {
                    Value = v.RowKey,
                    Text = v.Name
                }).ToList(),
                EventDate = DateTime.Now
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEventViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Venues = (await _tableService.GetAllVenuesAsync())
                    .Select(v => new SelectListItem
                    {
                        Value = v.RowKey,
                        Text = v.Name
                    }).ToList();
                return View(viewModel);
            }

            var entity = new Event
            {
                PartitionKey = "Events",
                RowKey = Guid.NewGuid().ToString(),
                Name = viewModel.Name,
                EventDate = viewModel.EventDate,
                Description = viewModel.Description,
                Booked = viewModel.Booked,
                VenueId = viewModel.VenueId
            };

            await _tableService.AddEventAsync(entity);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var events = await _tableService.GetAllEventsAsync();
            var venues = await _tableService.GetAllVenuesAsync();

            var eventViews = events.Select(e => new
            {
                Event = e,
                Venue = venues.FirstOrDefault(v => v.RowKey == e.VenueId)
            });

            return View(eventViews); // Adjust view to accept this tuple or flat model
        }
    }
}
