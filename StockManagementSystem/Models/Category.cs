using System;
using System.Collections.Generic;

namespace StockManagementSystem.Models;

public partial class Category
{
    public short CateId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Plant> Plants { get; set; } = new List<Plant>();
}
