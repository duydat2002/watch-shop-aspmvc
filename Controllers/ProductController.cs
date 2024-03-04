using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WatchShop2.Models;

namespace WatchShop2.Controllers;

public class ProductController : Controller
{
  private WatchShop2Context _entityContext { get; }

  public ProductController(WatchShop2Context entityContext)
  {
    _entityContext = entityContext;
  }

  [Route("products/{category?}")]
  public IActionResult Products(string category = "", string search = "")
  {
    string categoryName = "";

    switch (category)
    {
      case "men-watches":
        categoryName = "Men Watches";
        break;
      case "women-watches":
        categoryName = "Women Watches";
        break;
      case "sport-watches":
        categoryName = "Sports Watches";
        break;
      case "luxury-watches":
        categoryName = "Luxury Watches";
        break;
      case "":
        break;
      default:
        return RedirectToAction("PageNotFound", "Home");
    }

    ViewData["categoryName"] = categoryName;

    ViewBag.sizes = _entityContext.GetSizes(search, categoryName);
    ViewBag.colors = _entityContext.GetColors(search, categoryName);

    return View();
  }

  [Route("product/{ProductSlug}")]
  public IActionResult Detail(string ProductSlug)
  {
    Product? product = _entityContext.GetProductBySlug(ProductSlug);
    if (product == null)
      return RedirectToAction("PageNotFound", "Home");
    else
      return View(product);
  }

  [Route("products/filter")]
  public IActionResult Filter(string search = "", string categories = "", string colors = "", string sizes = "", string price = "", int pageNumber = 1, int pageSize = 10, string sort = "auto")
  {
    float priceStart = 0, priceEnd = float.MaxValue;

    if (price != "")
    {
      string[] prices = price.Split('-');
      priceStart = float.Parse(prices[0]);
      if (prices[1] != "inf")
      {
        priceEnd = float.Parse(prices[1]);
      }
    }

    List<Product> products = _entityContext.FilterProducts(search, categories, colors, sizes, priceStart, priceEnd, pageNumber, pageSize, sort);

    return PartialView("_ProductItems", products);
  }

  [Route("api/products/get-colors")]
  public IActionResult GetColors(string ProductName = "", string Categories = "")
  {
    List<Color> colors = _entityContext.GetColors(ProductName, Categories);
    return Json(colors);
  }

  [Route("api/products/get-sizes")]
  public IActionResult GetSizes(string ProductName = "", string Categories = "")
  {
    List<Size> sizes = _entityContext.GetSizes(ProductName, Categories);
    return Json(sizes);
  }

  [HttpPost]
  [Route("api/products/add-product")]
  public IActionResult AddProduct([FromBody] AddProductModel product)
  {
    int cac = _entityContext.AddProduct(product.Categories, product.ColorId, product.SizeId, product.ProductName, product.ProductSlug, product.ProductDesc, product.Price, product.Quantity, product.Discount, product.ProductImages);
    return Json(cac);
  }
}