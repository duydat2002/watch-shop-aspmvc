using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

public class UpdateUserPasswordModel
{
  public int UserId { get; set; }

  public string OldPassword { get; set; } = null!;

  public string NewPassword { get; set; } = null!;
}

public class UserWithRoleName
{
  public int UserId { get; set; }

  public int RoleId { get; set; }

  public string RoleName { get; set; } = null!;

  public string Email { get; set; } = null!;

  [NotMapped]
  public string Password { get; set; } = null!;

  public string FirstName { get; set; } = null!;

  public string LastName { get; set; } = null!;

  [NotMapped]
  public string? FullName { get { return $"{FirstName} {LastName}"; } }

  public bool Gender { get; set; }

  public DateOnly? Birthdate { get; set; }

  public bool Active { get; set; }
}

public class ChangeUserRoleModel
{
  public int UserId { get; set; }

  public int RoleId { get; set; }
}

public class BanOrUnbanUserModel
{
  public int UserId { get; set; }

  public bool Active { get; set; }
}