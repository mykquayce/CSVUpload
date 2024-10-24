namespace CSVUpload.Website.Models;

public readonly record struct CampaignDetails(string? PostCodeSector, long? Uprn, string Name, DateTime Date);
