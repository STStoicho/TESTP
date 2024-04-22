using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectPresents.Data;

namespace ProjectPresents.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Client> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<Client> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var applicationDbContext = _context.Orders
                                    .Include(o => o.Clietns)
                                    .Include(o => o.Presents);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.Orders
                                    .Include(o => o.Clietns)
                                    .Include(o => o.Presents)
                                    .Where(x => x.ClientId == _userManager.GetUserId(User));
                return View(await applicationDbContext.ToListAsync());
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Clietns)
                .Include(o => o.Presents)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["PresentsId"] = new SelectList(_context.Presents, "Id", "Name");
            return View();
        }
        public async Task<IActionResult> CreateWithPresentId(int presentId, int countP)
        {
            //int c = int.Parse( ViewBag.counter);
            //return View();
            var currentPresent = await _context.Presents.FirstOrDefaultAsync(z => z.Id == presentId);
            Order order = new Order();
            order.PresentsId = presentId;
            order.Quantity = countP;
            order.ClientId = _userManager.GetUserId(User);
            order.DateUpdate = DateTime.Now;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PresentsId,Quantity")] Order order)
        {
            order.DateUpdate = DateTime.Now;

            if (ModelState.IsValid)
            {
                order.ClientId = _userManager.GetUserId(User);
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", order.ClientId);
            ViewData["PresentsId"] = new SelectList(_context.Presents, "Id", "Name", order.PresentsId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            //ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", order.ClientId);
            ViewData["PresentsId"] = new SelectList(_context.Presents, "Id", "Name", order.PresentsId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClientId,PresentsId,Quantity,DateUpdate")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            order.DateUpdate = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    order.ClientId = _userManager.GetUserId(User);
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            //ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", order.ClientId);
            ViewData["PresentsId"] = new SelectList(_context.Presents, "Id", "Name", order.PresentsId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Clietns)
                .Include(o => o.Presents)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
