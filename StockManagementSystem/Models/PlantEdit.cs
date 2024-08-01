using System.ComponentModel.DataAnnotations;

namespace StockManagementSystem.Models
{
    public class PlantEdit
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

        [DataType(DataType.Upload)]
        public IFormFile? PlantFile { get; set; }

    }
}
