using clinic_reservation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using clinic_reservation.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using clinic_reservation.Hubs;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = ServerVersion.AutoDetect(connectionString);
var broker = new RabbitMq("Test");


// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Json serialiation
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
).AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

// Database
builder.Services.AddDbContext<ClinicContext>(options => options.UseMySql(connectionString, serverVersion));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();