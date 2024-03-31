using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WatchShop2.Helpers;
using WatchShop2.Models;
using X.PagedList;

namespace WatchShop2.Areas.Admin.Controllers;

[Area("admin")]
[Route("admin/attributes")]
public class AttributesController : Controller
{
  private WatchShop2Context _entityContext { get; }


  public AttributesController(WatchShop2Context entityContext)
  {
    _entityContext = entityContext;
  }

  [Route("categories")]
  public IActionResult Categories(int page = 1)
  {
    int pageSize = 10;
    var categories = _entityContext.GetCategories().ToPagedList(page, pageSize);
    return View(categories);
  }

  [Route("colors")]
  public IActionResult Colors(int page = 1)
  {
    int pageSize = 10;
    var colors = _entityContext.GetColors().ToPagedList(page, pageSize);
    return View(colors);
  }

  [Route("sizes")]
  public IActionResult Sizes(int page = 1)
  {
    int pageSize = 10;
    var sizes = _entityContext.GetSizes().ToPagedList(page, pageSize);
    return View(sizes);
  }
}