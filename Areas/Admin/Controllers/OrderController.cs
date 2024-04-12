using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WatchShop2.Helpers;
using WatchShop2.Models;
using X.PagedList;

namespace WatchShop2.Areas.Admin.Controllers;

[Area("admin")]
[Route("admin/orders")]
public class OrderController : Controller
{
  private WatchShop2Context _entityContext { get; }

  public OrderController(WatchShop2Context entityContext)
  {
    _entityContext = entityContext;
  }

  [Route("")]
  public IActionResult Index(int page = 1)
  {
    int pageSize = 10;
    var orders = _entityContext.GetOrders().ToPagedList(page, pageSize);
    return View(orders);
  }

  [Route("{OrderId}")]
  public IActionResult Detail(int OrderId)
  {
    var order = _entityContext.GetOrderById(OrderId);

    if (order != null)
    {
      ViewBag.OrderDetail = _entityContext.GetOrderDetail(OrderId);
      return View(order);
    }
    else
    {
      return RedirectToAction("PageNotFound", "Home");
    }
  }

}