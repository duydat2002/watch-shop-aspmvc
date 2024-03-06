using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchShop2.Models;

public partial class User
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    [Required(ErrorMessage = "Please enter your email.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Please enter your password.")]
    [StringLength(maximumLength: 255, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
    public string Password { get; set; } = null!;

    [NotMapped]
    [Compare("Password", ErrorMessage = "The password and confirm password do not match.")]
    public string? ConfirmPassword { get; set; } = null!;


    [Required(ErrorMessage = "Please enter your firstname.")]
    [RegularExpression(@"^[\p{L}0-9_-]+$", ErrorMessage = "Firstname can only contain letters, numbers, underscores, and hyphens.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Please enter your lastname.")]
    [RegularExpression(@"^[\p{L}0-9_-]+$", ErrorMessage = "Lastname can only contain letters, numbers, underscores, and hyphens.")]
    public string LastName { get; set; } = null!;

    [NotMapped]
    public string? FullName { get { return $"{FirstName} {LastName}"; } }

    public bool Gender { get; set; }

    [ValidateBirthdate]
    public DateOnly? Birthdate { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();

    public virtual Role? Role { get; set; } = null!;

    public virtual ICollection<UserContact> UserContacts { get; set; } = new List<UserContact>();
}

public class ValidateBirthdateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null)
        {
            DateOnly birthdate = (DateOnly)value;

            if (birthdate.AddYears(18) >= DateOnly.FromDateTime(DateTime.Now))
            {
                return new ValidationResult("You must be at least 18 years old.");
            }
        }

        return ValidationResult.Success;
    }
}