using System;
using System.Collections.Generic;

namespace StockManagementSystem.Models;

public partial class Order
{
    public int Orderid { get; set; }

    public DateOnly? OrderDate { get; set; }

    public int? TotalAmount { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
