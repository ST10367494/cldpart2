using Azure.Storage.Files.Shares;
using EventVenueBooking1.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Azure.Storage.Blobs;
namespace EventVenueBooking1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Access the configuration object
            var configuration = builder.Configuration;

            // Get the connection string from the nested AzureStorage section
var connectionString = configuration["AzureStorage:ConnectionString"];

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Azure Storage connection string is not configured.");
}

// Add MVC services to the container
builder.Services.AddControllersWithViews();

// ✅ Register BlobServiceClient so VenueController can use it
builder.Services.AddSingleton<BlobServiceClient>(sp =>
{
    return new BlobServiceClient(connectionString);
});

// Register BlobService if you're using it elsewhere
builder.Services.AddSingleton(new BlobService(connectionString));

// Register TableStorageService
builder.Services.AddSingleton(new TableStorageService(connectionString));

// Register ShareServiceClient for File Share access
builder.Services.AddSingleton(new ShareServiceClient(connectionString));

// Register FileShareService (for "venues" share)
builder.Services.AddSingleton<FileShareService>(sp =>
{
    var shareServiceClient = sp.GetRequiredService<ShareServiceClient>();
    return new FileShareService(shareServiceClient, "venue");
});

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
