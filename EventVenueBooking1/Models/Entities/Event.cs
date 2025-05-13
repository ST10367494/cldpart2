using Azure;
using Azure.Data.Tables;

namespace EventVenueBooking1.Models.Entities
{
    public class Event : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }  // Guid string for EventId

        public string Name { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public bool Booked { get; set; }
        public string VenueId { get; set; }

        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}
