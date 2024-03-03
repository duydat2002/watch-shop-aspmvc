using System;
using System.Collections.Generic;

namespace WatchShop2.Models;

public partial class Color
{
    public int ColorId { get; set; }

    public string ColorName { get; set; } = null!;

    public string ColorValue { get; set; } = null!;

    public int? Quantity { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
