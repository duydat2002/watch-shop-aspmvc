using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WatchShop2.Helpers;
using WatchShop2.Models;
using X.PagedList;

namespace WatchShop2.Areas.Admin.Controllers;

[Area("admin")]
[Route("admin/user")]
public class UserController : Controller
{
  private WatchShop2Context _entityContext { get; }


  public UserController(WatchShop2Context entityContext)
  {
    _entityContext = entityContext;
  }

  [Route("administrators")]
  public IActionResult Administrators(int page = 1)
  {
    int pageSize = 10;
    var administrators = _entityContext.GetAdministrators().ToPagedList(page, pageSize);
    return View(administrators);
  }

  [Route("customers")]
  public IActionResult Customers(int page = 1)
  {
    int pageSize = 10;
    var customers = _entityContext.GetCustomers().ToPagedList(page, pageSize);
    return View(customers);
  }
}