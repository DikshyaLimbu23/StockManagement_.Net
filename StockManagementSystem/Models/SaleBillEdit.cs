namespace StockManagementSystem.Models
{
    public class SaleBillEdit
    {
        public int Bid { get; set; }

        public int Userid { get; set; }

        public int BillNo { get; set; }

        public decimal? TotalAmount { get; set; }

        public DateOnly? BillDate { get; set; }

        public string? TransactionType { get; set; }

        public int Productid { get; set; }

        public string? ReasonforCancel { get; set; }

        public int? Canceleduserid { get; set; }

        public DateOnly? CancelDate { get; set; }

        public bool? Status { get; set; }

        public int Quantity { get; set; }

        public int Amount { get; set; }

        public List<BillDetail>? BillDetails { get; set; }

    }
}
