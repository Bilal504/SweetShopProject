using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SweetShopProject.Models;
namespace SweetShop.Controllers
{
    public class InventoriesController : Controller
    {
        private readonly SweetContext _context;

        public InventoriesController(SweetContext context)
        {
            _context = context;
        }

        // GET: Inventories
        public async Task<IActionResult> Index()
        {
            var sweetContext = _context.inventory.Include(i => i.cat).Include(i => i.prod);
            return View(await sweetContext.ToListAsync());
        }

        // GET: Inventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.inventory
                .Include(i => i.cat)
                .Include(i => i.prod)
                .FirstOrDefaultAsync(m => m.id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventories/Create
        public IActionResult Create()
        {
            ViewData["catID"] = new SelectList(_context.category, "id", "id");
            ViewData["prodID"] = new SelectList(_context.product, "id", "id");
            return View();
        }

        // POST: Inventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,quantityAvail,totalQuantity,totalSold,prodID,catID")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["catID"] = new SelectList(_context.category, "id", "id", inventory.catID);
            ViewData["prodID"] = new SelectList(_context.product, "id", "id", inventory.prodID);
            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.inventory.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            ViewData["catID"] = new SelectList(_context.category, "id", "id", inventory.catID);
            ViewData["prodID"] = new SelectList(_context.product, "id", "id", inventory.prodID);
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,quantityAvail,totalQuantity,totalSold,prodID,catID")] Inventory inventory)
        {
            if (id != inventory.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.id))
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
            ViewData["catID"] = new SelectList(_context.category, "id", "id", inventory.catID);
            ViewData["prodID"] = new SelectList(_context.product, "id", "id", inventory.prodID);
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.inventory
                .Include(i => i.cat)
                .Include(i => i.prod)
                .FirstOrDefaultAsync(m => m.id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.inventory == null)
            {
                return Problem("Entity set 'SweetContext.inventory'  is null.");
            }
            var inventory = await _context.inventory.FindAsync(id);
            if (inventory != null)
            {
                _context.inventory.Remove(inventory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
          return (_context.inventory?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
