using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WatchShop2.Helpers;
using WatchShop2.Models;

namespace WatchShop2.Areas.Admin.Controllers;

[Area("admin")]
[Route("admin")]
public class HomeController : Controller
{
  private WatchShop2Context _entityContext { get; }

  private IWebHostEnvironment _environment;

  public HomeController(IWebHostEnvironment environment, WatchShop2Context entityContext)
  {
    _entityContext = entityContext;
    _environment = environment;
  }

  public IActionResult Index()
  {
    return View();
  }

  [HttpPost]
  [Route("signout")]
  public IActionResult SignOutPost()
  {
    HttpContext.Session.Clear();
    return RedirectToAction("Index", "Home");
  }

  [Route("PageNotFound")]
  public IActionResult PageNotFound()
  {
    return View();
  }
}