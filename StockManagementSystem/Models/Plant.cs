using System;
using System.Collections.Generic;

namespace StockManagementSystem.Models;

public partial class Plant
{
    public int PlantId { get; set; }

    public short CateId { get; set; }

    public string PlantName { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public int TotalQuantity { get; set; }

    public int SaleQuantity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Plantfile { get; set; }

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category Cate { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
