using CsvHelper.Configuration;
using CSVUpload.Website.Models;
using CSVUpload.Website.Services;
using CSVUpload.Website.Services.Concrete;
using Microsoft.Extensions.Options;
using MySqlConnector;
using System.Data;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	.AddSingleton<IReaderConfiguration>(
		new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = true,
			MissingFieldFound = null,
		})
	.AddTransient<ICsvParser, CsvParser>();

builder.Services
	.Configure<ConnectionDetails>(builder.Configuration.GetSection("Database"))
	.AddSingleton(p =>
	{
		var config = p.GetRequiredService<IOptions<ConnectionDetails>>().Value;
		ArgumentException.ThrowIfNullOrEmpty(config.Server);
		ArgumentOutOfRangeException.ThrowIfGreaterThan(config.Port, ushort.MaxValue);
		ArgumentException.ThrowIfNullOrEmpty(config.UserId);
		ArgumentException.ThrowIfNullOrEmpty(config.Password);
		return new MySqlConnectionStringBuilder
		{
			Server = config.Server,
			Port = config.Port,
			UserID = config.UserId,
			Password = config.Password,
			Database = config.Database,
		};
	})
	.AddTransient<IDbConnection>(p =>
	{
		var builder = p.GetRequiredService<MySqlConnectionStringBuilder>();
		return new MySqlConnection(builder.ConnectionString);
	})
	.AddTransient<IRepository, Repository>();

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
