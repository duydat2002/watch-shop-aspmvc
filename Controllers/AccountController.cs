using Microsoft.AspNetCore.Mvc;
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
        return Json(new { cac = ex.Message, sign = signInData });
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
}