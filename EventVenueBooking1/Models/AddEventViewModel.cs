using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EventVenueBooking1.Models.ViewModels
{
    public class AddEventViewModel
    {
        [Required(ErrorMessage = "Event name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Event date is required.")]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Event description is required.")]
        public string Description { get; set; }

        public bool Booked { get; set; }

        [Required(ErrorMessage = "Please select a venue.")]
        [Display(Name = "Venue")]
      public string VenueId { get; set; }  // Keep only one VenueId property

        public List<SelectListItem> Venues { get; set; } = new(); // Initialize the list
    }
}