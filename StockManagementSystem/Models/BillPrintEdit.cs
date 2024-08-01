namespace StockManagementSystem.Models
{
    public class BillPrintEdit
    {
        public int Bpid { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }

        public string PrintedBy { get; set; } = null!;

        public DateOnly PrintDate { get; set; }

        public TimeOnly PrintTime { get; set; }
    }
}
