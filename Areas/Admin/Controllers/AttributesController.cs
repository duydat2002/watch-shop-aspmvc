using Microsoft.Data.SqlClient;
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

  [Route("categories/create")]
  public IActionResult CategoryCreate()
  {
    return View();
  }

  [Route("categories/{CategoryId}")]
  public IActionResult CategoryDetail(int CategoryId, int page = 1)
  {
    int pageSize = 5;
    var category = _entityContext.GetCategoryById(CategoryId);

    if (category == null)
      return RedirectToAction("PageNotFound", "Home");

    var products = _entityContext.GetProducts(CategoryId).ToPagedList(page, pageSize);

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

  [Route("colors/create")]
  public IActionResult ColorCreate()
  {
    return View();
  }

  [Route("colors/{ColorId}")]
  public IActionResult ColorDetail(int ColorId, int page = 1)
  {
    int pageSize = 5;
    var products = _entityContext.GetProducts(ColorId: ColorId).ToPagedList(page, pageSize);
    var color = _entityContext.GetColorById(ColorId);

    ViewBag.Products = products;

    return View(color);
  }

  [Route("sizes")]
  public IActionResult Sizes(int page = 1)
  {
    int pageSize = 10;
    var sizes = _entityContext.GetSizes().ToPagedList(page, pageSize);
    return View(sizes);
  }

  [Route("sizes/{SizeId}")]
  public IActionResult SizeDetail(int SizeId, int page = 1)
  {
    int pageSize = 5;
    var products = _entityContext.GetProducts(SizeId: SizeId).ToPagedList(page, pageSize);
    var size = _entityContext.GetSizeById(SizeId);

    ViewBag.Products = products;

    return View(size);
  }

  [HttpPost]
  [Route("/admin/api/attributes/colors/create-color")]
  public IActionResult CreateColor([FromBody] Color color)
  {
    try
    {
      int create = _entityContext.CreateColor(color);
      return Json(new { success = true, id = create });
    }
    catch (SqlException e)
    {
      return Json(new { success = false, message = e.Message });
    }
  }

  [HttpPost]
  [Route("/admin/api/attributes/categories/create-category")]
  public IActionResult CreateCategory([FromBody] Category category)
  {
    try
    {
      int create = _entityContext.CreateCategory(category);
      return Json(new { success = true, id = create });
    }
    catch (SqlException e)
    {
      return Json(new { success = false, message = e.Message });
    }
  }


  [HttpPost]
  [Route("/admin/api/attributes/categories/update-category")]
  public IActionResult UpdateCategory([FromBody] Category category)
  {
    int update = _entityContext.UpdateCategory(category);
    return Json(new { success = update > 0 });
  }

  [HttpPost]
  [Route("/admin/api/attributes/categories/add-product")]
  public IActionResult AddProductCategory([FromBody] ProductCategory productCategory)
  {
    int add = _entityContext.AddProductCategory(productCategory);
    return Json(new { success = add > 0 });
  }

  [HttpPost]
  [Route("/admin/api/attributes/categories/delete-product")]
  public IActionResult DeleteProductCategory([FromBody] ProductCategory productCategory)
  {
    int delete = _entityContext.DeleteProductCategory(productCategory);
    return Json(new { success = delete > 0 });
  }
}