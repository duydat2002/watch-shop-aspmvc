using System;
using System.Collections.Generic;

namespace WatchShop2.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string? BankName { get; set; }

    public string? CardNumber { get; set; }

    public virtual Order Order { get; set; } = null!;
}
