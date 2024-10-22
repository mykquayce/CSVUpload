using CsvHelper.Configuration;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	.AddSingleton<IReaderConfiguration>(
		new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = false,
			MissingFieldFound = null,
		});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program;
