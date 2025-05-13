using Azure;
using Azure.Data.Tables;

namespace EventVenueBooking1.Models.Entities
{
    public class Venue : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }  // Guid string for VenueId

        public string Name { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string ImageFileName { get; set; }
        public bool Booked { get; set; }
        public DateTime? BookingDate { get; set; }

        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}
