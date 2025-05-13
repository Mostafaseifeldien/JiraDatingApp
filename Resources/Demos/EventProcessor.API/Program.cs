using EventProcessor.Application.Services;
using EventProcessor.Core.Interfaces;
using EventProcessor.Infrastructure.ApiClients;
using EventProcessor.Infrastructure.FileParsers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();  // ✅ Add Swagger generator

builder.Services.AddHttpClient<ISystemYApiClient, SystemYApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["SystemYApi:BaseUrl"]);
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddScoped<IFileParser, EventFileParser>();
builder.Services.AddScoped<IEventDispatcher, EventDispatcherService>();
builder.Services.AddScoped<EventProcessingService>();

builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// ✅ Enable Swagger middleware
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    //app.UseSwagger();
    //app.UseSwaggerUI(options =>
    //{
    //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Event Processor API v1");
    //    options.RoutePrefix = string.Empty; // Serve Swagger at root: https://localhost:<port>/
    //});
}

//app.UseAuthorization();
//app.MapControllers();
app.Run();
