using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore.SqlServer;
using Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//add services into IoC Container
builder.Services.AddSingleton<ICountriesService, CountriesService>();
builder.Services.AddSingleton<IPersonsService, PersonsService>();

builder.Services.AddDbContext<PersonsDbContext>(options =>
{
    options.UseSqlServer();
});

//Data Source=(localdb)\MSSQLLocalDB;
//Initial Catalog=PersonsDatabase;
//Integrated Security=True;
//Connect Timeout=30;Encrypt=False;
//Trust Server Certificate=False;
//Application Intent=ReadWrite;
//Multi Subnet Failover=False

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
