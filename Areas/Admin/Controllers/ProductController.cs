using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WatchShop2.Helpers;
using WatchShop2.Models;
using X.PagedList;

namespace WatchShop2.Areas.Admin.Controllers;

[Area("admin")]
[Route("admin/products")]
public class ProductController : Controller
{
  private WatchShop2Context _entityContext { get; }


  public ProductController(WatchShop2Context entityContext)
  {
    _entityContext = entityContext;
  }

  [Route("")]
  public IActionResult Products(int page = 1)
  {
    int pageSize = 10;
    var products = _entityContext.GetProducts().ToPagedList(page, pageSize);
    return View(products);
  }

}