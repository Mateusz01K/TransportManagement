using Microsoft.EntityFrameworkCore;
using TransportManagement;
using TransportManagement.Services.AssignTrailer;
using TransportManagement.Services.AssignTruck;
using TransportManagement.Services.Driver;
using TransportManagement.Services.Trailer;
using TransportManagement.Services.Truck;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TransportManagementDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("TransportManagementDB"),
        new MySqlServerVersion(new Version(8, 0, 21))));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<ITruckService, TruckService>();
builder.Services.AddScoped<ITrailerService, TrailerService>();
builder.Services.AddScoped<IAssignTruckService, AssignTruckService>();
builder.Services.AddScoped<IAssignTrailerService, AssignTrailerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
