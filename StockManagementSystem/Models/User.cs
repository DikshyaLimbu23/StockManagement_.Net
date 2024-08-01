using System;
using System.Collections.Generic;

namespace StockManagementSystem.Models;

public partial class User
{
    public int Userid { get; set; }

    public string Username { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string Locations { get; set; } = null!;

    public string? UserRole { get; set; }

    public bool? Isactive { get; set; }

    public DateOnly? RegisterDate { get; set; }

    public string? ResonforDelete { get; set; }

    public DateOnly? DeleteDate { get; set; }

    public string? Userfile { get; set; }

    public string? UserPassword { get; set; }

    public virtual ICollection<BillRecord> BillRecordCanceledusers { get; set; } = new List<BillRecord>();

    public virtual ICollection<BillRecord> BillRecordUsers { get; set; } = new List<BillRecord>();
}
