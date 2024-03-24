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
    int? UserId = HttpContext.Session.GetInt32("UserId");
    if (UserId == null)
      return Redirect("/account/signin");
    else
      return View();
  }

  [HttpPost]
  [Route("/api/cart/add-to-cart")]
  public IActionResult AddToCart([FromBody] Cart addToCartParams)
  {
    int? UserId = HttpContext.Session.GetInt32("UserId");

    int add = -1;
    if (UserId != null)
      add = _entityContext.AddCart((int)UserId, addToCartParams.ProductId, addToCartParams.Quantity);

    return Json(new { success = add > 0 ? true : false });

    // return Json(new { a = add, u = UserId, p = addToCartParams.ProductId, cac = addToCartParams.Quantity });
  }

  [Route("/api/cart/get-cart")]
  public IActionResult GetCart()
  {
    int? UserId = HttpContext.Session.GetInt32("UserId");
    List<CartModel> cart = new List<CartModel>();
    if (UserId != null)
      cart = _entityContext.GetCart((int)UserId);

    return PartialView("_CartItems", cart);
  }

  [Route("/api/cart/get-header-cart")]
  public IActionResult GetHeaderCart()
  {
    int? UserId = HttpContext.Session.GetInt32("UserId");
    List<CartModel> cart = new List<CartModel>();
    if (UserId != null)
      cart = _entityContext.GetCart((int)UserId);

    return PartialView("_HeaderCartItems", cart);
  }

  [HttpPost]
  [Route("/api/cart/update-cart")]
  public IActionResult UpdateCart([FromBody] Cart updateCartParam)
  {

    int update = _entityContext.UpdateCart(updateCartParam.CartId, updateCartParam.Quantity);
    return Json(new { success = update > 0 });
  }

  [HttpPost]
  [Route("/api/cart/delete-cart")]
  public IActionResult DeleteCart([FromBody] Cart deleteCartParam)
  {
    int delete = _entityContext.DeleteCart(deleteCartParam.CartId);
    return Json(new { success = delete > 0 });
  }

  [HttpPost]
  [Route("/api/cart/add-order")]
  public IActionResult AddOrder([FromBody] AddOrderModel addOrder)
  {
    int add = _entityContext.AddOrder(addOrder);
    return Json(new { success = add > 0, add });
  }
}