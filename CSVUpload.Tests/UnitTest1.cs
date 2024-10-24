using CsvHelper;
using CsvHelper.Configuration;
using CSVUpload.Website.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Globalization;

namespace CSVUpload.Tests;

public class UnitTest1(WebApplicationFactory<Program> factory)
	: IClassFixture<WebApplicationFactory<Program>>
{
	private readonly HttpClient _httpClient = factory.CreateClient();

	[Fact]
	public async Task Test1()
	{
		var request = new HttpRequestMessage(HttpMethod.Post, "home/fileupload")
		{
			Content = new MultipartFormDataContent
			{
				{
					new StreamContent(File.OpenRead(path: Path.Combine(".", "Data", "campaigns.csv"))),
					"files",
					"campaigns.csv"
				},
				{
					new StreamContent(File.OpenRead(path: Path.Combine(".", "Data", "campaigns2.csv"))),
					"files",
					"campaigns2.csv"
				},
			}
		};

		var response = await _httpClient.SendAsync(request);
		var content = await response.Content.ReadAsStringAsync();

		// Assert
		Assert.True(response.IsSuccessStatusCode, userMessage: response.ReasonPhrase + " " + content);
		Assert.NotEmpty(content);
		Assert.NotEqual("[]", content);
	}
}

public class DeserializationTests
{
	[Theory, InlineData(".", "Data", "campaigns.csv")]
	public async Task Test1(params string[] paths)
	{
		var config = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = true,
			MissingFieldFound = null,
		};
		var reader = new StreamReader(path: Path.Combine(paths));
		using var csv = new CsvReader(reader, config);
		{
			csv.Context.RegisterClassMap<CampaignDetailsMap>();
		}

		var campaigns = csv.GetRecordsAsync<CampaignDetails>();
		await foreach (var (sector, uprn, name, date) in campaigns)
		{
			var ok = sector is null || uprn is null;
			Assert.True(ok);
			Assert.True(sector is not null || uprn is not null);
			Assert.NotNull(name);
			Assert.NotEmpty(name);
			Assert.NotEqual(default, date);
		}
	}
}
