using Microsoft.AspNetCore.Mvc.Testing;

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
