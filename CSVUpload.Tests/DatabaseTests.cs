using CSVUpload.Website.Services;

namespace CSVUpload.Tests;

public class DatabaseTests(Fixture fixture) : IClassFixture<Fixture>
{
	private readonly IRepository _sut = fixture.Repository;

	[Fact]
	public async Task ConnectionTests()
	{
		var now = DateTime.UtcNow;
		var timestamp = await _sut.GetTimeStampAsync();

		// Assert
		Assert.NotEqual(default, timestamp);
		Assert.InRange(timestamp, now.AddMinutes(-1), now.AddMinutes(1));
	}
}
