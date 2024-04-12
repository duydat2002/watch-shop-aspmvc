using System;
using System.Collections.Generic;

namespace WatchShop2.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
