using ServiceContracts;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using Repositories;
using Serilog;
using CRUDExample.Filters.ActionFilters;

var builder = WebApplication.CreateBuilder(args);

//Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services,
    LoggerConfiguration loggerConfiguration) =>
    {
        loggerConfiguration.
        //read configuration settings from built-in IConfiguration
        ReadFrom.Configuration(context.Configuration).
        //read out current application services and make them available to serilog
        ReadFrom.Services(services);
    });

//it adds controllers and views as services
builder.Services.AddControllersWithViews(options =>
{
    //options.Filters.Add<ResponseHeaderActionFilter>();
    var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
    options.Filters.Add(new ResponseHeaderActionFilter(logger, "My-Key-From-Global", "My-Value-From-Global"));
});

//add services into IoC Container
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();

builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields =
    Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
});

var app = builder.Build();

app.UseSerilogRequestLogging();

//create application pipeline
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpLogging();

//app.Logger.LogDebug("debug-message");
//app.Logger.LogInformation("information-message");
//app.Logger.LogWarning("warning-message");
//app.Logger.LogError("error-message");
//app.Logger.LogCritical("critical-message");

if (builder.Environment.IsEnvironment("Test") == false)
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

//make the auto-generated Program accessible programmatically

//public partial class Program { } //make the auto-generated Program accessible programmatically
