using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WatchShop2.Helpers;
using WatchShop2.Models;
using X.PagedList;

namespace WatchShop2.Areas.Admin.Controllers;

[Area("admin")]
[Route("admin/user")]
public class UserController : Controller
{
  private WatchShop2Context _entityContext { get; }


  public UserController(WatchShop2Context entityContext)
  {
    _entityContext = entityContext;
  }

  [Route("roles")]
  public IActionResult Roles(int page = 1)
  {
    int pageSize = 10;
    var roles = _entityContext.GetRoles().ToPagedList(page, pageSize);
    return View(roles);
  }

  [Route("roles/{RoleId}")]
  public IActionResult RoleDetail(int RoleId)
  {
    var role = _entityContext.GetRoleById(RoleId);
    return View(role);
  }

  [Route("administrators")]
  public IActionResult Administrators(int page = 1)
  {
    int pageSize = 10;
    var administrators = _entityContext.GetAdministrators().ToPagedList(page, pageSize);
    return View(administrators);
  }

  [Route("customers")]
  public IActionResult Customers(int page = 1)
  {
    int pageSize = 10;
    var customers = _entityContext.GetCustomers().ToPagedList(page, pageSize);
    return View(customers);
  }

  [Route("/admin/api/user/get-users-by-role")]
  public IActionResult GetCustomers(int roleId, int page = 1)
  {
    int pageSize = 10;
    var administrators = _entityContext.GetUserByRole(roleId).ToPagedList(page, pageSize);
    return PartialView("_UserListCard", administrators);
  }
}