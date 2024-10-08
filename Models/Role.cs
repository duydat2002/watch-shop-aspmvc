﻿using System;
using System.Collections.Generic;

namespace WatchShop2.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public int? UserCount { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
