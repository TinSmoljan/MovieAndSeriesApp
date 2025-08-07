using MovieAndSeriesScanner.Api.Middleware;
using MovieAndSeriesScanner.Api.Middleware.ExceptionHandling;
using MovieAndSeriesScanner.Services.Api.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Configuration
builder.Configuration.AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true);
#endregion

#region Logging
Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.CreateLogger();

builder.Host.UseSerilog();
#endregion

#region HttpClients
builder.Services.ConfigureHttpClients(builder.Configuration);
#endregion

#region Services
builder.Services.AddApiServices(builder.Configuration);
#endregion

#region Exception handling
builder.Services.AddExceptionHandler<TmdbApiExceptionHandler>();
builder.Services.AddProblemDetails();
#endregion

#region Caching
builder.Services.AddDistributedMemoryCache();
#endregion

#region Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

builder.Services.AddControllers();
var app = builder.Build();

#region Middleware
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseExceptionHandler();
#endregion

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
