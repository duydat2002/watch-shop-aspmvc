using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WatchShop2.Models;

namespace WatchShop2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private WatchShop2Context _entityContext { get; }

    public HomeController(ILogger<HomeController> logger, WatchShop2Context entityContext)
    {
        _logger = logger;
        _entityContext = entityContext;
    }

    public IActionResult Index()
    {
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
