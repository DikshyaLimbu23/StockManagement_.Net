using System;
using System.Collections.Generic;

namespace StockManagementSystem.Models;

public partial class CompleteBillView
{
    public int Bpid { get; set; }

    public int Brid { get; set; }

    public string PrintedBy { get; set; } = null!;

    public DateOnly PrintDate { get; set; }

    public TimeOnly PrintTime { get; set; }

    public int Bid { get; set; }

    public int Userid { get; set; }

    public int BillNo { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateOnly? BillDate { get; set; }

    public string? TransactionType { get; set; }

    public string? ReasonforCancel { get; set; }

    public int? Canceleduserid { get; set; }

    public DateOnly? CancelDate { get; set; }

    public bool? Status { get; set; }

    public int BillDetId { get; set; }

    public int Productid { get; set; }

    public decimal Rate { get; set; }

    public int Quantity { get; set; }

    public int? BillRid { get; set; }

    public int PlantId { get; set; }

    public string PlantName { get; set; } = null!;
}
