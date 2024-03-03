using System;
using System.Collections.Generic;

namespace WatchShop2.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int ColorId { get; set; }

    public int SizeId { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductSlug { get; set; } = null!;

    public string ProductDesc { get; set; } = null!;

    public double Price { get; set; }

    public int Quantity { get; set; }

    public int Discount { get; set; }

    public string ProductImages { get; set; } = null!;

    public string? ColorName { get; set; } = null!;

    public string? ColorValue { get; set; } = null!;

    public string? SizeName { get; set; } = null!;

    public double PriceSale { get; set; }

    public virtual ICollection<CartDetail> CartDetails { get; set; } = new List<CartDetail>();

    public virtual Color Color { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual Size Size { get; set; } = null!;

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
