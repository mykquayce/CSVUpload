using CSVUpload.Website.Models;
using CSVUpload.Website.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CSVUpload.Website.Controllers;
public class HomeController(ILogger<HomeController> logger, ICsvParser parser, IRepository repository) : Controller
{
	public IActionResult Index() => base.View();

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}

	[HttpPost]
	public async Task<IActionResult> FileUpload(IReadOnlyCollection<IFormFile> files)
	{
		using var cts = new CancellationTokenSource();
		var tasks = files.Where(f => f?.Length > 0).Select(f => UploadAsync(f, cts.Token));
		var tempFiles = await Task.WhenAll(tasks);
		var campaigns = tempFiles.Select(fi => parser.GetCampaignDetailsAsync(fi, cts.Token)).GetManyAsync();
		await repository.SaveCampaignsAsync(campaigns, cts.Token);
		var o = from fi in tempFiles
				select new { fi.Length, fi.FullName, };
		return Ok(o);
	}

	private async Task<FileInfo> UploadAsync(IFormFile formFile, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(formFile);
		ArgumentOutOfRangeException.ThrowIfZero(formFile.Length);
		logger.LogInformation("uploading {count} file(s)", formFile.Length);
		var tempFile = new FileInfo(fileName: Path.GetTempFileName());
		await using var stream = tempFile.OpenWrite();
		await formFile.CopyToAsync(stream, cancellationToken);
		return tempFile;
	}
}
