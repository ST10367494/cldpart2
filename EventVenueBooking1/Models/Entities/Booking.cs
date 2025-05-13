using Azure;
using Azure.Data.Tables;

namespace EventVenueBooking1.Models.Entities
{
    public class Booking : ITableEntity
    {
        public string PartitionKey { get; set; }  // e.g., "BookingGroup"
        public string RowKey { get; set; }        // Guid string for BookingId

        public string VenueId { get; set; }
        public DateTime Date { get; set; }

        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}
