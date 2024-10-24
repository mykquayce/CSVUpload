using CSVUpload.Website.Models;

namespace CSVUpload.Website.Services;
public interface ICsvParser
{
	IAsyncEnumerable<CampaignDetails> GetCampaignDetailsAsync(FileInfo file, CancellationToken cancellationToken = default);
}
