using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WatchShop2.Models;

namespace WatchShop2.Controllers;

[Route("account")]
public class AccountController : Controller
{
  private WatchShop2Context _entityContext { get; }

  public AccountController(WatchShop2Context entityContext)
  {
    _entityContext = entityContext;
  }

  [Route("")]
  public IActionResult Index()
  {
    int? UserId = HttpContext.Session.GetInt32("UserId");
    if (UserId != null)
    {
      User user = _entityContext.GetUserInfo((int)UserId);
      List<UserContact> userContact = _entityContext.GetUserContact((int)UserId);
      ViewBag.UserContact = userContact;
      return View(user);
    }
    else
      return Redirect("/account/signin");
  }

  [Route("signup")]
  public IActionResult SignUp()
  {
    return View();
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  [Route("signup")]
  public IActionResult SignUp(User user)
  {
    if (ModelState.IsValid)
    {
      try
      {
        string hashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
        int signUp = _entityContext.SignUp(user.Email, hashPassword, user.FirstName, user.LastName, user.Gender, user.Birthdate);

        return RedirectToAction("SignIn", "Account");
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

  [Route("signin")]
  public IActionResult SignIn()
  {
    return View();
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  [Route("signin")]
  public IActionResult SignIn(SignInModel signInData)
  {
    if (ModelState.IsValid)
    {
      try
      {
        User user = _entityContext.SignIn(signInData.Email);

        // return Json(new { up = user, sp = signInData, p = BCrypt.Net.BCrypt.Verify(signInData.Password, user.Password) });
        if (user == null)
        {
          ModelState.AddModelError("Email", "The email is not registered.");
        }
        else if (user.Active == false)
        {
          ViewBag.ErrorMessage = "This account has been banned.";
        }
        else if (!BCrypt.Net.BCrypt.Verify(signInData.Password, user.Password))
        {
          ModelState.AddModelError("Password", "The password is wrong.");
        }
        else
        {
          HttpContext.Session.SetInt32("UserId", user.UserId);
          HttpContext.Session.SetInt32("RoleId", user.RoleId);
          HttpContext.Session.SetString("Email", user.Email);
          HttpContext.Session.SetString("FullName", user.FullName);

          return RedirectToAction("Index", "Home");
        }
        return View(signInData);
      }
      catch (System.Exception ex)
      {
        // return Json(new { cac = ex.Message, sign = signInData, hash = BCrypt.Net.BCrypt.HashPassword(signInData.Password) });
        ViewBag.ErrorMessage = "An error has occurred.";
        return View(signInData);
      }
    }
    else
    {
      return View(signInData);
    }
  }

  [HttpPost]
  [Route("signout")]
  public IActionResult SignOut()
  {
    HttpContext.Session.Clear();
    return RedirectToAction("Index", "Home");
  }

  [HttpPost]
  [Route("/api/account/update-user-password")]
  public IActionResult UpdateUserPassword([FromBody] UpdateUserPasswordModel userPasswordModel)
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

  [HttpPost]
  [Route("/api/account/update-user-info")]
  public IActionResult UpdateUserInfo([FromBody] User user)
  {
    int update = _entityContext.UpdateUserInfo(user);
    if (update > 0)
    {
      HttpContext.Session.SetString("FullName", $"{user.FirstName} {user.LastName}");
    }
    return Json(new { success = update > 0 });
  }

  [Route("/api/account/get-user-contact-raw")]
  public IActionResult GetUserContactRaw(int UserId)
  {
    List<UserContact> userContacts = _entityContext.GetUserContact(UserId);
    return Json(new { userContacts });
  }

  [Route("/api/account/get-user-contact")]
  public IActionResult GetUserContact(int UserId)
  {
    List<UserContact> userContacts = _entityContext.GetUserContact(UserId);
    return PartialView("_UserContactItems", userContacts);
  }

  [HttpPost]
  [Route("/api/account/add-user-contact")]
  public IActionResult AddUserContact([FromBody] UserContact userContact)
  {
    try
    {
      int add = _entityContext.AddUserContact(userContact);
      return Json(new { success = add > 0 });
    }
    catch (SqlException e)
    {
      return Json(new { success = false, message = e.Message });
    }
  }

  [HttpPost]
  [Route("/api/account/update-user-contact")]
  public IActionResult UpdateUserContact([FromBody] UserContact userContact)
  {
    int update = _entityContext.UpdateUserContact(userContact);
    return Json(new { success = update > 0 });
  }

  [HttpPost]
  [Route("/api/account/set-default-user-contact")]
  public IActionResult SetDefaultUserContact([FromBody] UserContact userContact)
  {
    int set = _entityContext.SetDefaultUserContact(userContact);
    return Json(new { success = set > 0 });
  }

  [HttpPost]
  [Route("/api/account/delete-user-contact")]
  public IActionResult DeleteUserContact([FromBody] UserContact userContact)
  {
    int delete = _entityContext.DeleteUserContact(userContact);
    return Json(new { success = delete > 0 });
  }
}