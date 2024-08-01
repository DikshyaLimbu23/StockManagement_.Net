using System;
using System.Collections.Generic;

namespace StockManagementSystem.Models;

public partial class Cart
{
    public int CartItemId { get; set; }

    public short CateId { get; set; }

    public int PlantId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Category Cate { get; set; } = null!;

    public virtual Plant Plant { get; set; } = null!;
}
