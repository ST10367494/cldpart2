using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EventVenueBooking1.Models.ViewModels
{
    public class AddVenueViewModel
    {
        [Required(ErrorMessage = "Venue name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Venue Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be at least 1")]
        public int Capacity { get; set; }

        [Display(Name = "Venue Image")]
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }

        public string ExistingImageFileName { get; set; }

        [Display(Name = "Is Booked?")]
        public bool Booked { get; set; }

        [Display(Name = "Booking Date")]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Booking date must be in the future")]
        public DateTime? BookingDate { get; set; }

        public class FutureDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                if (value is DateTime date)
                {
                    return date > DateTime.Now;
                }
                return true;
            }
        }
    }
}
