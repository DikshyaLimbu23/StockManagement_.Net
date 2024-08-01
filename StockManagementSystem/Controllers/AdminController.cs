using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockManagementSystem.Models;
using System.Diagnostics.Eventing.Reader;

namespace StockManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly IWebHostEnvironment _env;
        private readonly InventoryManagementContext _context;
        public AdminController(InventoryManagementContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public IActionResult Index()
        {
           
                return View();
        }

        [HttpPost]
        public IActionResult Index(PlantEdit plant)
        {
            if (ModelState.IsValid)
            {
                if (plant.PlantFile != null)
                {
                    string fileName = "PlantImage" + Guid.NewGuid() + Path.GetExtension(plant.PlantFile.FileName);
                    string filePath = Path.Combine(_env.WebRootPath, "PlantImage", fileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        plant.PlantFile.CopyTo(stream);
                    }
                    plant.Plantfile = fileName;
                }



                // Mapping to the database entity and saving
                var newPlant = new Plant
                {
                    PlantId = plant.PlantId,
                    CateId = plant.CateId,
                    PlantName = plant.PlantName,
                    Plantfile = plant.Plantfile,
                    Description = plant.Description,
                    Price = plant.Price,
                    Quantity = plant.Quantity,
                    TotalQuantity = plant.TotalQuantity,
                    SaleQuantity = plant.SaleQuantity,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Add(newPlant);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult PlantLists()
        {
            ViewData["cat"] = new SelectList(_context.Categories, nameof(Category.CateId), nameof(Category.CategoryName));
            return View();
        }

        public IActionResult GetCategoryList(int cateid)
        {
            var product = _context.Plants.Where(e => e.CateId == cateid).ToList();
            return PartialView("_GetPlantLists", product);
        }

        public IActionResult Delete(int id)
        {
            var plant = _context.Plants.FirstOrDefault(e => e.PlantId == id);
            if (plant != null)
            {
                _context.Plants.Remove(plant);
                _context.SaveChanges();
            }
            return RedirectToAction("PlantLists");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            Plant? plant = _context.Plants.Where(e => e.PlantId == id).FirstOrDefault();

            if (plant == null)
            {
                return NotFound();
            }

            PlantEdit pl = new()
            {
                PlantName = plant.PlantName,
                PlantId = plant.PlantId,
                Plantfile = plant.Plantfile,
                Description = plant.Description,
                Price = plant.Price,
                Quantity = plant.Quantity,
                TotalQuantity = plant.TotalQuantity,
                SaleQuantity = plant.SaleQuantity,
                CreatedAt = plant.CreatedAt,
                UpdatedAt = plant.UpdatedAt,
                CateId = plant.CateId
            };
            return View(pl);


        }

        [HttpPost]
        public async Task<IActionResult> Edit(PlantEdit edit)
        {
            var plant = await _context.Plants.Where(e => e.PlantId == edit.PlantId).FirstAsync();


            if (edit.PlantFile != null)
            {
                string fileName = "PlantImage" + Guid.NewGuid() + Path.GetExtension(edit.PlantFile.FileName);
                string filePath = Path.Combine(_env.WebRootPath, "PlantImage", fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    edit.PlantFile.CopyTo(stream);
                }
                edit.Plantfile = fileName;
            }

            plant.Plantfile = edit.Plantfile;
            plant.PlantName = edit.PlantName;
            plant.Description = edit.Description;
            plant.Price = edit.Price;
            plant.Quantity = edit.Quantity;
            plant.TotalQuantity = edit.TotalQuantity;
            plant.SaleQuantity = edit.SaleQuantity;
            plant.UpdatedAt = DateTime.Now;
            plant.CateId = edit.CateId;

            _context.Plants.Update(plant);
            _context.SaveChanges();

            return RedirectToAction("PlantLists");
        }


    }
}
