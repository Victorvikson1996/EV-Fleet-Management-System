// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast = Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast");

// app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }

using EVFleetManagement.ChargingStations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebSocketSharp.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddScoped<ServiceRegistry>();
builder.Services.AddScoped<EventBus>();
builder.Services.AddScoped<FsmService>();
builder.Services.AddScoped<VehicleNode>();
builder.Services.AddScoped<ChargingStation>();
builder.Services.AddScoped<FleetManager>();
builder.Services.AddScoped<GpsDriver>();
builder.Services.AddScoped<BatteryDriver>();
builder.Services.AddScoped<LocationSensor>();
builder.Services.AddScoped<BatterySensor>();
builder.Services.AddScoped<ChargingActuator>();
builder.Services.AddScoped<MovementActuator>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<VehicleService>();
builder.Services.AddScoped<ChargingStationService>();

// Add JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
#pragma warning disable CS8604 // Possible null reference argument.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
#pragma warning restore CS8604 // Possible null reference argument.
    });

builder.Services.AddAuthorization();

// Configure WebSocket for real-time communication (simulating UDP Multicast)
builder.Services.AddScoped<UdpDriver>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// Start the fleet manager to initialize nodes and simulate operations
using (var scope = app.Services.CreateScope())
{
    var fleetManager = scope.ServiceProvider.GetRequiredService<FleetManager>();
    // Simulate initial fleet operations (optional, for demo)
    _ = Task.Run(async () =>
    {
        await Task.Delay(1000); // Delay to ensure services are ready
        fleetManager.StartDriving("vehicle1"); // Hardcoded for demo
        await Task.Delay(4000);
        fleetManager.SendToCharge("vehicle1"); // Hardcoded for demo
    });
}

// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllers();
//     // Map WebSocket endpoints if needed for UdpDriver
//     endpoints.Map("/udp", async context =>
//     {
//         var ws = await context.WebSockets.AcceptWebSocketAsync();
//         // Handle the WebSocket connection here
//     });
// });


app.Map("/udp", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        // Handle the WebSocket connection here
    }
    else
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
    }
});

app.Run();