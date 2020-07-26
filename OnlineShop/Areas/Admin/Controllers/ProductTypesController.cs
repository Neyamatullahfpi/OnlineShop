using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        ApplicationDbContext _db;

        public  ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.productTypes.ToList() );
        }

        //Get Create Action Method
        public ActionResult Create()
        {
            return View();
        }

        //Post Create Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.productTypes.Add(productTypes);
                await _db.SaveChangesAsync();
                TempData["msg"] = "Product Save Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);

        }


        //Get Edit Action Method
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ProdutType = _db.productTypes.Find(id);
            if (ProdutType == null)
            {
                return NotFound();
            }
            return View(ProdutType);
        }

        //Post Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _db.Update(productTypes);
                await _db.SaveChangesAsync();
                TempData["msg"] = "Product Update Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);

        }

        //Get Details Action Method
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ProdutType = _db.productTypes.Find(id);
            if (ProdutType == null)
            {
                return NotFound();
            }
            return View(ProdutType);
        }

        //Post Details Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ProductTypes productTypes)
        {
            
               return RedirectToAction(nameof(Index));
         
        }

        //Get Delete Action Method
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ProdutType = _db.productTypes.Find(id);
            if (ProdutType == null)
            {
                return NotFound();
            }
            return View(ProdutType);
        }

        //Post Delete Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, ProductTypes productTypes)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != productTypes.Id)
            {
                return NotFound();
            }

            var Productt = _db.productTypes.Find(id);

            _db.productTypes.Remove(Productt);
            await _db.SaveChangesAsync();
            TempData["msg"] = "Product Delete Successfully";
            return RedirectToAction(nameof(Index));
            

        }
    }
}