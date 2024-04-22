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
    public class ApliedsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApliedsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Aplieds
        public async Task<IActionResult> Index()
        {
            return View(await _context.Aplieds.ToListAsync());
        }

        // GET: Aplieds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aplied = await _context.Aplieds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aplied == null)
            {
                return NotFound();
            }

            return View(aplied);
        }

        // GET: Aplieds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aplieds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,DateUpdate")] Aplied aplied)
        {
            aplied.DateUpdate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Aplieds.Add(aplied);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aplied);
        }

        // GET: Aplieds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aplied = await _context.Aplieds.FindAsync(id);
            if (aplied == null)
            {
                return NotFound();
            }
            return View(aplied);
        }

        // POST: Aplieds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateUpdate")] Aplied aplied)
        {
            if (id != aplied.Id)
            {
                return NotFound();
            }

            aplied.DateUpdate = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aplied);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApliedExists(aplied.Id))
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
            return View(aplied);
        }

        // GET: Aplieds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aplied = await _context.Aplieds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aplied == null)
            {
                return NotFound();
            }

            return View(aplied);
        }

        // POST: Aplieds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aplied = await _context.Aplieds.FindAsync(id);
            if (aplied != null)
            {
                _context.Aplieds.Remove(aplied);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApliedExists(int id)
        {
            return _context.Aplieds.Any(e => e.Id == id);
        }
    }
}
