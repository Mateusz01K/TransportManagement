using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransportManagement;
using TransportManagement.Models.User;
using TransportManagement.Services.AssignTrailer;
using TransportManagement.Services.AssignTruck;
using TransportManagement.Services.Driver;
using TransportManagement.Services.Order;
using TransportManagement.Services.Trailer;
using TransportManagement.Services.Truck;
using TransportManagement.Services.User;
using TransportManagement.Services.User.EmailSender;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TransportManagementDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("TransportManagementDB"),
        new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<TransportManagementDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}
);

builder.Services.AddControllersWithViews();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<ITruckService, TruckService>();
builder.Services.AddScoped<ITrailerService, TrailerService>();
builder.Services.AddScoped<IAssignTruckService, AssignTruckService>();
builder.Services.AddScoped<IAssignTrailerService, AssignTrailerService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
