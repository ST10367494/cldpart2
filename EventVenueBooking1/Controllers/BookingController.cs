using EventVenueBooking1.Models.Entities;
using EventVenueBooking1.Models.ViewModels;
using EventVenueBooking1.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventVenueBooking1.Controllers
{
    public class BookingController : Controller
    {
        private readonly TableStorageService _tableService;

        public BookingController(TableStorageService tableService)
        {
            _tableService = tableService;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var bookings = await _tableService.GetAllBookingsAsync();
            var events = await _tableService.GetAllEventsAsync();
            var venues = await _tableService.GetAllVenuesAsync();

            var result = bookings.Select(b => new BookingViewModel
            {
                VenueId = Guid.Parse(b.VenueId),
                EventId = events.FirstOrDefault(e => e.VenueId == b.VenueId)?.RowKey,
                EventDate = b.Date,
                VenueName = venues.FirstOrDefault(v => v.RowKey == b.VenueId)?.Name,
                VenueLocation = venues.FirstOrDefault(v => v.RowKey == b.VenueId)?.Location,
                VenueCapacity = venues.FirstOrDefault(v => v.RowKey == b.VenueId)?.Capacity ?? 0,
                Booked = venues.FirstOrDefault(v => v.RowKey == b.VenueId)?.Booked ?? false,
                ImageFileName = venues.FirstOrDefault(v => v.RowKey == b.VenueId)?.ImageFileName
            }).ToList();

            return View(result);
        }
    }
}
