using ITSpark.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc().AddViewLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-US", "ar-EG" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});





builder.Services.AddDbContext<ITSparkDbContext>(a =>{

    a.UseSqlServer("Data Source=.;Initial Catalog=AUE;Encrypt=False;Integrated Security=True");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

var supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("ar-EG"),
    // Add more cultures as needed
};

                    //TEST 
//foreach (var culture in supportedCultures)
//{
//    Console.WriteLine($"{culture.NativeName} ({culture.Name})");
//}


app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=student}/{action=Index}/{id?}");

app.Run();
