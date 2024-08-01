﻿using System;
using System.Collections.Generic;

namespace StockManagementSystem.Models;

public partial class OrderDetail
{
    public int OrderDetailsId { get; set; }

    public int OrderId { get; set; }

    public int? Plantid { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Plant? Plant { get; set; }
}