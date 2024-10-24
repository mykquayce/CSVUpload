namespace CSVUpload.Website.Models;

public record class ConnectionDetails(string Server, uint Port, string Database, string UserId, string Password)
{
	public ConnectionDetails() : this(default!, default!, default!, default!, default!) { }
}
