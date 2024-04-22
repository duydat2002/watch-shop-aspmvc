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

  [Route("/admin/api/statitics/get-dashboard-info")]
  public IActionResult GetDashboardInfo(int month, int year)
  {
    var dashBoardInfo = _entityContext.GetDashboardInfo(month, year);
    return Json(new { dashBoardInfo });
  }

  [Route("/admin/api/statitics/get-order-status")]
  public IActionResult GetOrderStatus(int month, int year)
  {
    var orderStatus = _entityContext.GetOrderStatus(month, year);
    return Json(new { orderStatus });
  }

  [Route("/admin/api/statitics/get-sales-statitic")]
  public IActionResult SalesStatitic(int month, int year)
  {
    var salesStatitic = _entityContext.SalesStatitic(month, year);
    return Json(new { salesStatitic });
  }

  [Route("/admin/api/statitics/get-recent-orders")]
  public IActionResult GetRecentOrders()
  {
    var recentOrders = _entityContext.GetRecentOrders();
    return PartialView("_RecentOrderList", recentOrders);
  }
}