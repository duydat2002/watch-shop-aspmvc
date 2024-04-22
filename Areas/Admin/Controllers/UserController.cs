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

  [Route("change-password")]
  public IActionResult ChangePassword()
  {
    return View();
  }

  [Route("administrators")]
  public IActionResult Administrators(int page = 1)
  {
    int pageSize = 10;
    var administrators = _entityContext.GetAdministrators().ToPagedList(page, pageSize);
    return View(administrators);
  }

  [Route("administrators/create")]
  public IActionResult CreateAdmin()
  {
    return View();
  }

  [HttpPost]
  [Route("administrators/create")]
  public IActionResult CreateAdmin(User user)
  {
    if (ModelState.IsValid)
    {
      try
      {
        string hashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
        int addAdmin = _entityContext.AddAdmin(user);

        if (addAdmin > 0)
        {
          return Redirect("/admin/user/administrators");
        }
        else
        {
          ViewBag.ErrorMessage = "An error has occurred.";
          return View(user);
        }
      }
      catch (System.Exception ex)
      {
        if (ex.Message.IndexOf("UC_User_Email") != -1)
        {
          ModelState.AddModelError("Email", "The email is already used.");
        }
        else
        {
          ViewBag.ErrorMessage = "An error has occurred.";
        }
        return View(user);
      }
    }
    else
    {
      return View(user);
    }
  }

  [Route("administrators/{UserId}")]
  public IActionResult AdminDetail(int UserId)
  {
    var user = _entityContext.GetUserInfo(UserId);
    // return Json(new { user });
    if (user != null && user.RoleId == 1)
    {
      return View(user);
    }
    else
    {
      return RedirectToAction("PageNotFound", "Home");
    }
  }

  [Route("customers")]
  public IActionResult Customers(int page = 1)
  {
    int pageSize = 10;
    var customers = _entityContext.GetCustomers().ToPagedList(page, pageSize);
    return View(customers);
  }

  [Route("customers/{UserId}")]
  public IActionResult CustomerDetail(int UserId)
  {
    var user = _entityContext.GetUserInfo(UserId);
    // return Json(new { user });
    if (user != null && user.RoleId == 2)
    {
      ViewBag.Contacts = _entityContext.GetUserContact(user.UserId);
      return View(user);
    }
    else
    {
      return RedirectToAction("PageNotFound", "Home");
    }
  }

  [Route("/admin/api/user/get-administrators")]
  public IActionResult GetAdministrators(int page = 1)
  {
    int pageSize = 10;
    var users = _entityContext.GetAdministrators().ToPagedList(page, pageSize);
    return PartialView("_AdminListCard", users);
  }

  [Route("/admin/api/user/get-customers")]
  public IActionResult GetCustomers(int page = 1)
  {
    int pageSize = 10;
    var users = _entityContext.GetCustomers().ToPagedList(page, pageSize);
    return PartialView("_CustomerListCard", users);
  }

  [Route("/admin/api/user/get-users-by-role-without")]
  public IActionResult GetUserByRoleWithOut(int RoleId, string search = "")
  {
    search = search.Trim().ToLower();
    var users = _entityContext
      .GetUserByRoleWithOut(RoleId)
      .Where(u => u.FullName.ToLower().Contains(search) || u.Email.ToLower().Contains(search))
      .ToList();
    return PartialView("_UserList", users);
  }

  [HttpPost]
  [Route("/admin/api/user/change-user-role")]
  public IActionResult ChangeUserRole([FromBody] ChangeUserRoleModel role)
  {
    var update = _entityContext.ChangeUserRole(role.UserId, role.RoleId);
    return Json(new { success = update > 0 });
  }

  [HttpPost]
  [Route("/admin/api/user/ban-unban")]
  public IActionResult BanOrUnbanUser([FromBody] User user)
  {
    var update = _entityContext.BanOrUnbanUser(user.UserId, user.Active);
    return Json(new { success = update > 0 });
  }

  [HttpPost]
  [Route("/admin/api/user/change-password")]
  public IActionResult ChangePassword([FromBody] UpdateUserPasswordModel userPasswordModel)
  {
    User userInfo = _entityContext.GetUserById(userPasswordModel.UserId);
    if (userInfo == null)
    {
      return Json(new { success = false });
    }

    if (!BCrypt.Net.BCrypt.Verify(userPasswordModel.OldPassword, userInfo.Password))
    {
      return Json(new { success = false, message = "Wrong password." });
    }

    string NewPassword = BCrypt.Net.BCrypt.HashPassword(userPasswordModel.NewPassword);
    int update = _entityContext.UpdateUserPassword(userPasswordModel.UserId, NewPassword);
    return Json(new { success = update > 0 });
  }
}