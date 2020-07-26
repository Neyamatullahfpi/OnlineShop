using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagListController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TagListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/TagList
        public async Task<IActionResult> Index()
        {
            return View(await _context.tagLists.ToListAsync());
        }

        // GET: Admin/TagList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tagList = await _context.tagLists
                .FirstOrDefaultAsync(m => m.TagID == id);
            if (tagList == null)
            {
                return NotFound();
            }

            return View(tagList);
        }

        // GET: Admin/TagList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/TagList/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TagID,TagName")] TagList tagList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tagList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tagList);
        }

        // GET: Admin/TagList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tagList = await _context.tagLists.FindAsync(id);
            if (tagList == null)
            {
                return NotFound();
            }
            return View(tagList);
        }

        // POST: Admin/TagList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TagID,TagName")] TagList tagList)
        {
            if (id != tagList.TagID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tagList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagListExists(tagList.TagID))
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
            return View(tagList);
        }

        // GET: Admin/TagList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tagList = await _context.tagLists
                .FirstOrDefaultAsync(m => m.TagID == id);
            if (tagList == null)
            {
                return NotFound();
            }

            return View(tagList);
        }

        // POST: Admin/TagList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tagList = await _context.tagLists.FindAsync(id);
            _context.tagLists.Remove(tagList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TagListExists(int id)
        {
            return _context.tagLists.Any(e => e.TagID == id);
        }
    }
}
