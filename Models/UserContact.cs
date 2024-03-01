using System;
using System.Collections.Generic;

namespace WatchShop2.Models;

public partial class UserContact
{
    public int UserContactId { get; set; }

    public int UserId { get; set; }

    public string Address { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public bool? IsDefault { get; set; }

    public virtual User User { get; set; } = null!;
}
