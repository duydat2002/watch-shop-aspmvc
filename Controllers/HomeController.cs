using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WatchShop2.Helpers;
using WatchShop2.Models;

namespace WatchShop2.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;

  private WatchShop2Context _entityContext { get; }

  private IWebHostEnvironment _environment;

  public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment, WatchShop2Context entityContext)
  {
    _logger = logger;
    _entityContext = entityContext;
    _environment = environment;
  }

  public IActionResult Index()
  {
    ViewBag.NewProducts = _entityContext.FilterProducts("", "New Watches");
    ViewBag.SpecialProducts = _entityContext.FilterProducts("", "Special Watches");
    return View();
  }

  public IActionResult Privacy()
  {
    return View();
  }

  [Route("PageNotFound")]
  public IActionResult PageNotFound()
  {
    return View();
  }

  [Route("upload")]
  public IActionResult Upload()
  {
    return View();
  }

  [HttpPost]
  [Route("upload")]
  public async Task<IActionResult> Upload(IFormFile productImage)
  {
    try
    {
      string cac = await UploadHelper.UploadOne(_environment, productImage, "products");
      ViewBag.FileStatus = cac;
    }
    catch (System.Exception)
    {
      ViewBag.FileStatus = "Error while file uploading.";
    }
    return View("Upload");
  }

  [HttpPost]
  [Route("upload-multi")]
  public async Task<IActionResult> Upload(IFormFile[] productImages)
  {
    try
    {
      List<string> cac = await UploadHelper.UploadMulti(_environment, productImages, "products");
      ViewBag.FileStatus = string.Join(", ", cac);
    }
    catch (System.Exception)
    {
      ViewBag.FileStatus = "Error while file uploading.";
    }
    return View("Upload");
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}