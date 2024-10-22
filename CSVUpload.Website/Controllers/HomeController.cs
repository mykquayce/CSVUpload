using CSVUpload.Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CSVUpload.Website.Controllers;
public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;

	public HomeController(ILogger<HomeController> logger)
	{
		_logger = logger;
	}

	public IActionResult Index()
	{
		return View();
	}

	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}

	[HttpPost]
	public async Task<IActionResult> FileUpload(IReadOnlyCollection<IFormFile> files)
	{
		var tasks = files.Where(f => f?.Length > 0).Select(f => UploadAsync(f));
		var tempFiles = await Task.WhenAll(tasks);

		try
		{
			return Ok(new { count = files.Count, size = tempFiles.Sum(fi => fi.Length), filePaths = tempFiles.Select(fi => fi.FullName), });
		}
		finally
		{
			base.Response.OnCompleted(async () =>
			{
			});
		}
	}

	private static async Task<FileInfo> UploadAsync(IFormFile formFile, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(formFile);
		ArgumentOutOfRangeException.ThrowIfZero(formFile.Length);
		var tempFile = new FileInfo(fileName: Path.GetTempFileName());
		await using var stream = tempFile.OpenWrite();
		await formFile.CopyToAsync(stream, cancellationToken);
		return tempFile;
	}
}
