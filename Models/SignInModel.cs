using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WatchShop2.Models;

public class SignInModel
{
  [Required(ErrorMessage = "Please enter your email.")]
  [EmailAddress(ErrorMessage = "Invalid email address.")]
  public string Email { get; set; } = null!;

  [Required(ErrorMessage = "Please enter your password.")]
  [StringLength(maximumLength: 255, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
  public string Password { get; set; } = null!;

}