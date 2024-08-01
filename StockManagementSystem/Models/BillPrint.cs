using System;
using System.Collections.Generic;

namespace StockManagementSystem.Models;

public partial class BillPrint
{
    public int Bpid { get; set; }

    public int Brid { get; set; }

    public string PrintedBy { get; set; } = null!;

    public DateOnly PrintDate { get; set; }

    public TimeOnly PrintTime { get; set; }

    public virtual BillRecord Br { get; set; } = null!;
}
