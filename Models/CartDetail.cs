﻿using System;
using System.Collections.Generic;

namespace WatchShop2.Models;

public partial class CartDetail
{
    public int CartDetailId { get; set; }

    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
