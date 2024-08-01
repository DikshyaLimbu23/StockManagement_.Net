using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockManagementSystem.Models;

namespace StockManagementSystem.Controllers
{
    public class BillController : Controller
    {
        private readonly InventoryManagementContext _context;
        public BillController(InventoryManagementContext context) {
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.Plants.ToList();
            ViewBag.Products = products;
            var sales = new SaleBillEdit
            {
                BillDetails = new List<BillDetail>()
            };

            foreach (var p in products)
            {
                sales.BillDetails.Add(new BillDetail
                {
                    Product = new Plant // Initialize the Product object if needed
                    {
                        Price = p.Price,
                        PlantName = p.PlantName,
                        Quantity = p.Quantity,
                        PlantId = p.PlantId,

                    }
                });
            }

            return View(sales);
        }

        [HttpPost]
        public IActionResult Index(SaleBillEdit edit)
        {
         
            // Validate BillDetails collection
            if (edit.BillDetails == null || !edit.BillDetails.Any())
            {
                ModelState.AddModelError("", "No items in the bill details.");
                return View(edit);
            }

            // Ensure that Productid exists in the database
            var productIds = edit.BillDetails.Select(bd => bd.Productid).Distinct();
            var products = _context.Plants.Where(p => productIds.Contains(p.PlantId)).ToList();
           

            if (products.Count != productIds.Count())
            {
                ModelState.AddModelError("", "Some products are not found.");
                return View(edit);
            }

            int maxid = _context.BillRecords.Any() ? _context.BillRecords.Max(x => x.BillNo) + 1 : 1;
            edit.BillNo = maxid;
            edit.Userid = Convert.ToInt32(User.Identity?.Name);

            // Create a new BillRecord
            var record = new BillRecord
            {
                TransactionType = edit.TransactionType,
                BillDate = DateTime.Now, // Use current date if BillDate is null
                BillNo = edit.BillNo,
                Userid = edit.Userid,
                TotalAmount = edit.TotalAmount
            };

    
            // Add the BillRecord to the database
            _context.BillRecords.Add(record);
            _context.SaveChanges();

            // Process each BillDetail
            foreach (var billDetail in edit.BillDetails)
            {
                if (billDetail.Quantity <= 0)
                {
                    ModelState.AddModelError("", "Quantity must be greater than zero.");
                    continue; // Skip this detail and process the next one
                }

                var product = products.FirstOrDefault(p => p.PlantId == billDetail.Productid);
                if (product == null)
                {
                    ModelState.AddModelError("", $"Product with ID {billDetail.Productid} not found.");
                    continue;
                }

                var detail = new BillDetail
                {
                    Rate = billDetail.Rate,
                    Quantity = billDetail.Quantity,
                    Productid = billDetail.Productid,
                    BillRid = record.Bid
                };

                _context.BillDetails.Add(detail);

                // Update the product quantity
                product.Quantity -= billDetail.Quantity;
                product.SaleQuantity += billDetail.Quantity;
                product.UpdatedAt = DateTime.Now;
                _context.Plants.Update(product);
            }

            _context.SaveChanges();

            // Create a BillPrint record
            var billPrint = new BillPrint
            {
                Brid = record.Bid,
                PrintDate = DateOnly.FromDateTime(DateTime.Now), // Set current date
                PrintTime = TimeOnly.FromDateTime(DateTime.Now), // Set current time
                PrintedBy = User.Identity?.Name!
            };

            _context.BillPrints.Add(billPrint);
            _context.SaveChanges();

            return RedirectToAction("GetBillPrint", new { id = billPrint.Bpid });
        }

        [HttpGet]
        public async Task<IActionResult> GetBillPrint(int id)
        {
            var billPrint =await _context.CompleteBillViews.Where(p => p.Bpid == id).FirstAsync();

            if (billPrint == null)
            {
                return NotFound();
            }

            return View(billPrint);
        }



    }
}
