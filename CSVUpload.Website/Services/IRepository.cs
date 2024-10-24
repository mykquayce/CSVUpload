using CSVUpload.Website.Models;

namespace CSVUpload.Website.Services;

public interface IRepository
{
	Task<DateTime> GetTimeStampAsync(CancellationToken cancellationToken = default);
	Task SaveCampaignsAsync(IAsyncEnumerable<CampaignDetails> campaigns, CancellationToken cancellationToken = default);
}
