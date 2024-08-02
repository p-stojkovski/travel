using TravelInspiration.API;
using TravelInspiration.API.Features.Destinations;
using TravelInspiration.API.Features.Itineraries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddHttpClient();
   
builder.Services.RegisterApplicationServices();
builder.Services.RegisterPersistenceServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
}
app.UseStatusCodePages();

SearchDestinations.AddEndpoint(app);
GetItineraries.AddEndpoint(app);

app.Run();