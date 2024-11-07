using Microsoft.EntityFrameworkCore;
using TransportManagement;
using TransportManagement.Services.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TransportManagementDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("TransportManagementDB"),
        new MySqlServerVersion(new Version(8, 0, 21))));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDriverService, DriverService>();

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
