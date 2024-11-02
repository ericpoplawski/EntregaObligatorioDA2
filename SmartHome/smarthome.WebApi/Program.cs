using Domain;
using Microsoft.EntityFrameworkCore;
using smarthome.BussinessLogic;
using smarthome.BussinessLogic.Services.HomeServices;
using smarthome.BussinessLogic.Services.Notifications;
using smarthome.BussinessLogic.Services.Sessions;
using smarthome.BussinessLogic.Services.System;
using smarthome.DataAccess;
using smarthome.DataImporter;
using smarthome.Services.System.BusinessLogic;
using smarthome.WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers(options =>
                {
                    options.Filters.Add<ExceptionFilter>();
                }).ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    options.JsonSerializerOptions.MaxDepth = 64;
                });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepository<Room>, Repository<Room>>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<ISystemService, SystemService>();
builder.Services.AddScoped<IRepository<Home>, Repository<Home>>();
builder.Services.AddScoped<IRepository<HardwareDevice>, Repository<HardwareDevice>>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IRepository<UserNotification>, Repository<UserNotification>>();
builder.Services.AddScoped<IRepository<Resident>, Repository<Resident>>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IRepository<Device>, Repository<Device>>();
builder.Services.AddScoped<ILoadAssembly<IDeviceImportService>, LoadAssembly<IDeviceImportService>>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IRepository<Company>, Repository<Company>>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IRepository<Session>, Repository<Session>>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IRepository<Notification>, Repository<Notification>>();
builder.Services.AddScoped<IRepository<SystemPermission>, Repository<SystemPermission>>();
builder.Services.AddScoped<IRepository<HomePermission>, Repository<HomePermission>>();
builder.Services.AddScoped<IRepository<Role>, Repository<Role>>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(options =>
{
    options.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin();
});

app.MapControllers();

app.Run();
