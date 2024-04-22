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
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Please enter your lastname.")]
    public string LastName { get; set; } = null!;

    [NotMapped]
    public string? FullName { get { return $"{FirstName} {LastName}"; } }

    public bool Gender { get; set; }

    [ValidateBirthdate]
    public DateOnly? Birthdate { get; set; }

    public bool Active { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

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