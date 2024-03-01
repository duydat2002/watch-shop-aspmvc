using System;
using System.Collections.Generic;

namespace WatchShop2.Models;

public partial class Reply
{
    public int ReplyId { get; set; }

    public int RatingId { get; set; }

    public int UserId { get; set; }

    public string? Comment { get; set; }

    public virtual Rating Rating { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
