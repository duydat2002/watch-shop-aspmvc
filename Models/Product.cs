﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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

    public bool Active { get; set; }

    public string? ColorName { get; set; } = null!;

    public string? ColorValue { get; set; } = null!;

    public string? SizeName { get; set; } = null!;

    [NotMapped]
    public double PriceSale { get { return Price * (100 - Discount) / 100; } }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Color Color { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Size Size { get; set; } = null!;

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
