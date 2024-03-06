using Microsoft.AspNetCore.Mvc;
using WatchShop2.Models;

namespace WatchShop2.Controllers;

[Route("cart")]
public class CartController : Controller
{
  private WatchShop2Context _entityContext { get; }

  public CartController(WatchShop2Context entityContext)
  {
    _entityContext = entityContext;
  }

  public IActionResult Index()
  {
    return View();
  }

  [HttpPost]
  [Route("add-to-cart")]
  public IActionResult AddToCart()
  {
    return View();
  }

}