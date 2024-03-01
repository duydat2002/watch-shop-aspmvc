using System;
using System.Collections.Generic;

namespace WatchShop2.Models;

public partial class Rating
{
    public int RatingId { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public double Rate { get; set; }

    public string? Comment { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();

    public virtual User User { get; set; } = null!;
}
