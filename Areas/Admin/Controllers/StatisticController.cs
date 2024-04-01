using Microsoft.AspNetCore.Mvc;
using WatchShop2.Models;

namespace WatchShop2.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/statistic")]
    public class StatisticController : Controller
    {
        
        private WatchShop2Context _entityContext { get; }


        public StatisticController(WatchShop2Context entityContext)
        {
            _entityContext = entityContext;
        }
        public IActionResult Index()
        {
            // Thống kê số lượng sản phẩm trong từng danh mục
            var productCountByCategory = _entityContext.Categories
                .Select(c => new { CategoryName = c.CategoryName, ProductCount = c.Products.Count })
                .ToList();

            ViewBag.ProductCountByCategory = productCountByCategory;

            // Thống kê số lượng đơn hàng theo trạng thái
            var orderCountByStatus = _entityContext.Orders
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key, OrderCount = g.Count() })
                .ToList();

            ViewBag.OrderCountByStatus = orderCountByStatus;

            return View();
        }        

    }
}
