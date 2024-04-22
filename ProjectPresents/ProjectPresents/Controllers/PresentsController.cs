using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectPresents.Data;

namespace ProjectPresents.Controllers
{
    public class PresentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PresentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Presents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Presents.Include(p => p.Aplieds).Include(p => p.Categories);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Presents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var present = await _context.Presents
                .Include(p => p.Aplieds)
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (present == null)
            {
                return NotFound();
            }

            return View(present);
        }

        // GET: Presents/Create
        public IActionResult Create()
        {
            ViewData["ApliedsId"] = new SelectList(_context.Aplieds, "Id", "Name");
            ViewData["CategoriesId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Presents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatalogNum,Name,CategoriesId,ApliedsId,Description,Image,Price,DateUpdate")] Present present)
        {
            present.DateUpdate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(present);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApliedsId"] = new SelectList(_context.Aplieds, "Id", "Name", present.ApliedsId);
            ViewData["CategoriesId"] = new SelectList(_context.Categories, "Id", "Name", present.CategoriesId);
            return View(present);
        }

        // GET: Presents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var present = await _context.Presents.FindAsync(id);
            if (present == null)
            {
                return NotFound();
            }
            ViewData["ApliedsId"] = new SelectList(_context.Aplieds, "Id", "Name", present.ApliedsId);
            ViewData["CategoriesId"] = new SelectList(_context.Categories, "Id", "Name", present.CategoriesId);
            return View(present);
        }

        // POST: Presents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CatalogNum,Name,CategoriesId,ApliedsId,Description,Image,Price,DateUpdate")] Present present)
        {
            if (id != present.Id)
            {
                return NotFound();
            }

            present.DateUpdate = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(present);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PresentExists(present.Id))
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
            ViewData["ApliedsId"] = new SelectList(_context.Aplieds, "Id", "Name", present.ApliedsId);
            ViewData["CategoriesId"] = new SelectList(_context.Categories, "Id", "Name", present.CategoriesId);
            return View(present);
        }

        // GET: Presents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var present = await _context.Presents
                .Include(p => p.Aplieds)
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (present == null)
            {
                return NotFound();
            }

            return View(present);
        }

        // POST: Presents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var present = await _context.Presents.FindAsync(id);
            if (present != null)
            {
                _context.Presents.Remove(present);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PresentExists(int id)
        {
            return _context.Presents.Any(e => e.Id == id);
        }
    }
}
