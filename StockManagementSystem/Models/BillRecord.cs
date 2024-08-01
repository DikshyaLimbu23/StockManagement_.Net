using System;
using System.Collections.Generic;

namespace StockManagementSystem.Models;

public partial class BillRecord
{
    public int Bid { get; set; }

    public int Userid { get; set; }

    public int BillNo { get; set; }

    public decimal? TotalAmount { get; set; }

    public DateTime? BillDate { get; set; }

    public string? TransactionType { get; set; }

    public string? ReasonforCancel { get; set; }

    public int? Canceleduserid { get; set; }

    public DateOnly? CancelDate { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<BillDetail> BillDetails { get; set; } = new List<BillDetail>();

    public virtual ICollection<BillPrint> BillPrints { get; set; } = new List<BillPrint>();

    public virtual User? Canceleduser { get; set; }

    public virtual User User { get; set; } = null!;
}
