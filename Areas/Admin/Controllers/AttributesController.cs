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

  [Route("categories/{CategoryId}")]
  public IActionResult CategoryDetail(int CategoryId, int page = 1)
  {
    int pageSize = 5;
    var products = _entityContext.GetProducts(CategoryId).ToPagedList(page, pageSize);
    var category = _entityContext.GetCategoryById(CategoryId);

    ViewBag.Products = products;

    return View(category);
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

  [HttpPost]
  [Route("/admin/api/attributes/categories/update-category")]
  public IActionResult UpdateCategory([FromBody] Category category)
  {
    int update = _entityContext.UpdateCategory(category);
    return Json(new { success = update > 0 });
  }
}