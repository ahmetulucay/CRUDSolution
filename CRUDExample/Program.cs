using ServiceContracts;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using Repositories;

public partial class Program {
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Logging
        builder.Host.ConfigureLogging(loggingProvider =>
        {
            loggingProvider.ClearProviders();
            loggingProvider.AddConsole();
            loggingProvider.AddDebug();
            loggingProvider.AddEventLog();
        });

        builder.Services.AddControllersWithViews();

        //add services into IoC Container
        builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
        builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();

        builder.Services.AddScoped<ICountriesService, CountriesService>();
        builder.Services.AddScoped<IPersonsService, PersonsService>();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        var app = builder.Build();

        //create application pipeline
        if (builder.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

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
    }
} //make the auto-generated Program accessible programmatically

public partial class Program { } //make the auto-generated Program accessible programmatically
