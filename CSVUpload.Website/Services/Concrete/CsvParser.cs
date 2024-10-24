using CsvHelper;
using CsvHelper.Configuration;
using CSVUpload.Website.Models;
using System.Runtime.CompilerServices;

namespace CSVUpload.Website.Services.Concrete;

public class CsvParser(IReaderConfiguration config) : ICsvParser
{
	public async IAsyncEnumerable<CampaignDetails> GetCampaignDetailsAsync(FileInfo file, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var reader = file.OpenText();
		using var csv = new CsvReader(reader, config);
		{
			csv.Context.RegisterClassMap<CampaignDetailsMap>();
		}

		var campaigns = csv.GetRecordsAsync<CampaignDetails>(cancellationToken);
		await foreach (var campaign in campaigns) { yield return campaign; }
	}
}
