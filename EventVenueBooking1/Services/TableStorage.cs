using Azure;
using Azure.Data.Tables;
using EventVenueBooking1.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventVenueBooking1.Services
{
    public class TableStorageService
    {
        private readonly TableClient _eventTableClient;
        private readonly TableClient _venueTableClient;
        private readonly TableClient _bookingTableClient;

        public TableStorageService(string connectionString)
        {
            _eventTableClient = new TableClient(connectionString, "Events");
            _venueTableClient = new TableClient(connectionString, "Venues");
            _bookingTableClient = new TableClient(connectionString, "Bookings");

            EnsureTableExistsAsync(_eventTableClient).GetAwaiter().GetResult();
            EnsureTableExistsAsync(_venueTableClient).GetAwaiter().GetResult();
            EnsureTableExistsAsync(_bookingTableClient).GetAwaiter().GetResult();
        }

        private async Task EnsureTableExistsAsync(TableClient tableClient)
        {
            await tableClient.CreateIfNotExistsAsync();
        }

        // EVENTS
        public async Task AddEventAsync(Event entity)
        {
            await _eventTableClient.AddEntityAsync(entity);
        }

        public async Task<List<Event>> GetAllEventsAsync()
        {
            var events = new List<Event>();
            await foreach (var e in _eventTableClient.QueryAsync<Event >())
            {
                events.Add(e);
            }
            return events;
        }

        public async Task<Event> GetEventAsync(string partitionKey, string rowKey)
        {
            return await _eventTableClient.GetEntityAsync<Event>(partitionKey, rowKey);
        }

        public async Task DeleteEventAsync(string partitionKey, string rowKey)
        {
            await _eventTableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        // VENUES
        public async Task AddVenueAsync(Venue entity)
        {
            await _venueTableClient.AddEntityAsync(entity);
        }

        public async Task<List<Venue>> GetAllVenuesAsync()
        {
            var venues = new List<Venue>();
            await foreach (var v in _venueTableClient.QueryAsync<Venue>())
            {
                venues.Add(v);
            }
            return venues;
        }

        public async Task<Venue> GetVenueAsync(string partitionKey, string rowKey)
        {
            return await _venueTableClient.GetEntityAsync<Venue>(partitionKey, rowKey);
        }

        public async Task DeleteVenueAsync(string partitionKey, string rowKey)
        {
            await _venueTableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        // BOOKINGS
        public async Task AddBookingAsync(Booking entity)
        {
            await _bookingTableClient.AddEntityAsync(entity);
        }

        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            var bookings = new List<Booking>();
            await foreach (var b in _bookingTableClient.QueryAsync<Booking>())
            {
                bookings.Add(b);
            }
            return bookings;
        }

        public async Task<Booking> GetBookingAsync(string partitionKey, string rowKey)
        {
            return await _bookingTableClient.GetEntityAsync<Booking>(partitionKey, rowKey);
        }

        public async Task DeleteBookingAsync(string partitionKey, string rowKey)
        {
            await _bookingTableClient.DeleteEntityAsync(partitionKey, rowKey);
        }
    }
}
