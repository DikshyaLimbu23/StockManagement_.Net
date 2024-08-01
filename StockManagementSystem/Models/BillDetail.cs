using System;
using System.Collections.Generic;

namespace StockManagementSystem.Models;

public partial class BillDetail
{
    public int BillDetId { get; set; }

    public int Productid { get; set; }

    public decimal Rate { get; set; }

    public int Quantity { get; set; }

    public int? BillRid { get; set; }

    public virtual BillRecord? BillR { get; set; }

    public virtual Plant Product { get; set; } = null!;
}
