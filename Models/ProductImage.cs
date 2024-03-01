using System;
using System.Collections.Generic;

namespace WatchShop2.Models;

public partial class ProductImage
{
    public int ProductImageId { get; set; }

    public int ProductId { get; set; }

    public string ProductImagePath { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
