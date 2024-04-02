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

  [Route("{ProductId}")]
  public IActionResult ProductDetail(int ProductId)
  {
    var categories = _entityContext.GetCategories();
    var productCategories = _entityContext.GetProductCategories(ProductId);
    var colors = _entityContext.GetColors();
    var sizes = _entityContext.GetSizes();
    var product = _entityContext.GetProductById(ProductId);

    ViewBag.ProductCategories = productCategories;
    ViewBag.Categories = categories;
    ViewBag.Colors = colors;
    ViewBag.Sizes = sizes;
    return View(product);
  }

  [Route("/admin/api/products/get-products")]
  public IActionResult GetProducts(int CategoryId = 0, int SizeId = 0, int ColorId = 0, string search = "", int page = 1)
  {
    int pageSize = 5;
    var products = _entityContext
      .GetProducts(CategoryId, SizeId, ColorId)
      .Where(p => p.ProductName.ToLower().Contains(search.ToLower()))
      .ToList().ToPagedList(page, pageSize);
    // return Json(new { products });
    return PartialView("_ProductListCard", products);
  }

  [Route("/admin/api/products/get-products-without")]
  public IActionResult GetProductsWithOut(int CategoryId = 0, int SizeId = 0, int ColorId = 0, string search = "")
  {
    var products = _entityContext
      .GetProductsWithOut(CategoryId, SizeId, ColorId)
      .Where(p => p.ProductName.ToLower().Contains(search.ToLower()))
      .ToList();
    return PartialView("_ProductList", products);
  }

  [HttpPost]
  [Route("/admin/api/products/update-product")]
  public IActionResult GetProductsWithOut(IFormFile[] images, [FromBody] AddProductModel product)
  {

    // var update = _entityContext.UpdateProduct(product);
    return Json(new { images, product });
  }
}