using CSVUpload.Website.Models;
using CSVUpload.Website.Services;
using CSVUpload.Website.Services.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MySqlConnector;
using System.Data;

namespace CSVUpload.Tests;

public sealed class Fixture : IDisposable
{
	private const uint _port = 3_306;
	private const string _server = "localhost", _database = "campaigns", _userId = "campaigns", _password = "b316f9c44f20f2fd2d3fa4aef10ee957";
	private readonly ServiceProvider _serviceProvider;

	public Fixture()
	{
		var initialData = new Dictionary<string, string?>
		{
			["Database:Server"] = _server,
			["Database:Port"] = _port.ToString("D"),
			["Database:Database"] = _database,
			["Database:UserId"] = _userId,
			["Database:Password"] = _password,
		};

		var configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(initialData)
			.Build();

		_serviceProvider = new ServiceCollection()
			.Configure<ConnectionDetails>(configuration.GetSection("Database"))
			.AddSingleton(p =>
			{
				var config = p.GetRequiredService<IOptions<ConnectionDetails>>().Value;
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
			.AddTransient<IRepository, Repository>()
			.BuildServiceProvider();

		Repository = _serviceProvider.GetRequiredService<IRepository>();
	}

	public IRepository Repository { get; }

	public void Dispose() => _serviceProvider.Dispose();
}
