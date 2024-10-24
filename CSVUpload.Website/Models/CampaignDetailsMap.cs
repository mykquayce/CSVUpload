using CsvHelper.Configuration;
using System.Globalization;

namespace CSVUpload.Website.Models;

public class CampaignDetailsMap : ClassMap<CampaignDetails>
{
	public CampaignDetailsMap()
	{
		Map(m => m.PostCodeSector)
			.Convert(args =>
			{
				var s = args.Row[0];
				if (string.IsNullOrWhiteSpace(s)) { return null; }
				return s.ToUpperInvariant();
			});

		Map(m => m.Uprn)
			.TypeConverterOption.NullValues("0")
			.Index(1);

		Map(m => m.Name)
			.Index(2);

		Map(m => m.Date)
			.TypeConverterOption.Format("yyyy-MM-dd")
			.TypeConverterOption.CultureInfo(CultureInfo.InvariantCulture)
			.Index(3);
	}
}
