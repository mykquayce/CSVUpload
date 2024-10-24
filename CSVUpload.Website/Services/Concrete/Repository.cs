using CSVUpload.Website.Models;
using Dapper;
using System.Data;

namespace CSVUpload.Website.Services.Concrete;

public class Repository(IDbConnection connection) : IRepository
{
	public Task<DateTime> GetTimeStampAsync(CancellationToken cancellationToken = default)
	{
		var command = new CommandDefinition(commandText: "select utc_timestamp();", cancellationToken: cancellationToken);
		return connection.ExecuteScalarAsync<DateTime>(command);
	}

	public async Task SaveCampaignsAsync(IAsyncEnumerable<CampaignDetails> campaigns, CancellationToken cancellationToken = default)
	{
		await foreach (var (postCodeSector, uprn, campaign, date) in campaigns)
		{
			var parameters = new { postCodeSector, uprn, campaign, date, };
			var command = new CommandDefinition(commandText: "Campaigns_Insert", parameters: parameters, cancellationToken: cancellationToken);
			await connection.ExecuteAsync(command);
		}
	}
}
