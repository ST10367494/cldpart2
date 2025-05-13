using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventVenueBooking1.Models.ViewModels
{
    public class BookingViewModel
    {
        public Guid VenueId { get; set; } // Fixed to match Venue entity
        public string VenueName { get; set; }
        public string VenueLocation { get; set; }
        public int VenueCapacity { get; set; }
        public string EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; } // Ensure Event entity uses this name
        public string ImageFileName { get; set; }

        public bool Booked { get; set; }
        public IEnumerable<SelectListItem> Venues { get; set; }

    }


}

