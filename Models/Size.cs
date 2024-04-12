using System;
using System.Collections.Generic;

namespace WatchShop2.Models;

public partial class Size
{
    public int SizeId { get; set; }

    public string SizeName { get; set; } = null!;

    public int? Quantity { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
