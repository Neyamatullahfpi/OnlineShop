using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _HE;

        public ProductsController(ApplicationDbContext context, IHostingEnvironment HE)
        {
            _context = context;
            _HE = HE;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.products.Include(p => p.ProductTypes).Include(p => p.TagList);
            return View(await applicationDbContext.ToListAsync());
        }

        //Post Index action method
        [HttpPost]
        public IActionResult index(decimal? LowAmount, decimal? LargeAmount)
        {
            var Productss = _context.products.Include(p => p.ProductTypes).Include(p => p.TagList).Where(p => p.Price >= LowAmount && p.Price <= LargeAmount).ToList() ;
            
            if (LowAmount==null || LargeAmount ==null)
            {
                Productss = _context.products.Include(p => p.ProductTypes).Include(p => p.TagList).ToList();
                
            }
            return View(Productss);
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.products
                .Include(p => p.ProductTypes)
                .Include(p => p.TagList)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["ProductTypeId"] = new SelectList(_context.productTypes, "Id", "ProductType");
            ViewData["Tagid"] = new SelectList(_context.tagLists, "TagID", "TagName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Price,Image,ProductColor,IsAvailable,ProductTypeId,Tagid")] Products products,IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _context.products.FirstOrDefault(c=>c.Name==products.Name);
                if (searchProduct!=null)
                {
                    ViewBag.message = "This Product already Exists";
                    ViewData["ProductTypeId"] = new SelectList(_context.productTypes, "Id", "ProductType");
                    ViewData["Tagid"] = new SelectList(_context.tagLists, "TagID", "TagName");
                    return View(products);
                }

                if (image!=null)
                {
                    var name = Path.Combine(_HE.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "images/"+image.FileName;
                }
                if (image==null)
                {
                    products.Image = "images/index.png";

                }
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypeId"] = new SelectList(_context.productTypes, "Id", "ProductType", products.ProductTypeId);
            ViewData["Tagid"] = new SelectList(_context.tagLists, "TagID", "TagName", products.Tagid);
            return View(products);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeId"] = new SelectList(_context.productTypes, "Id", "ProductType", products.ProductTypeId);
            ViewData["Tagid"] = new SelectList(_context.tagLists, "TagID", "TagName", products.Tagid);
            return View(products);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Price,Image,ProductColor,IsAvailable,ProductTypeId,Tagid")] Products products, IFormFile image)
        {
            if (id != products.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_HE.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "images/" + image.FileName;
                }
                if (image == null)
                {
                    products.Image = "images/index.png";

                }
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypeId"] = new SelectList(_context.productTypes, "Id", "ProductType", products.ProductTypeId);
            ViewData["Tagid"] = new SelectList(_context.tagLists, "TagID", "TagName", products.Tagid);
            return View(products);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.products
                .Include(p => p.ProductTypes)
                .Include(p => p.TagList)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.products.FindAsync(id);
            _context.products.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.products.Any(e => e.ID == id);
        }
    }
}
