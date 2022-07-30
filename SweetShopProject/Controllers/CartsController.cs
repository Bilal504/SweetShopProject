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
    public class CartsController : Controller
    {
        private readonly SweetContext _context;

        public CartsController(SweetContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var sweetContext = _context.cart.Include(c => c.cat).Include(c => c.prod);
            return View(await sweetContext.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.cart == null)
            {
                return NotFound();
            }

            var cart = await _context.cart
                .Include(c => c.cat)
                .Include(c => c.prod)
                .FirstOrDefaultAsync(m => m.id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["catID"] = new SelectList(_context.category, "id", "id");
            ViewData["prodID"] = new SelectList(_context.product, "id", "id");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,totalPrice,quantity,timeStamp,Discount,finalAmount,prodID,catID")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["catID"] = new SelectList(_context.category, "id", "id", cart.catID);
            ViewData["prodID"] = new SelectList(_context.product, "id", "id", cart.prodID);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.cart == null)
            {
                return NotFound();
            }

            var cart = await _context.cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["catID"] = new SelectList(_context.category, "id", "id", cart.catID);
            ViewData["prodID"] = new SelectList(_context.product, "id", "id", cart.prodID);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,totalPrice,quantity,timeStamp,Discount,finalAmount,prodID,catID")] Cart cart)
        {
            if (id != cart.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.id))
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
            ViewData["catID"] = new SelectList(_context.category, "id", "id", cart.catID);
            ViewData["prodID"] = new SelectList(_context.product, "id", "id", cart.prodID);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.cart == null)
            {
                return NotFound();
            }

            var cart = await _context.cart
                .Include(c => c.cat)
                .Include(c => c.prod)
                .FirstOrDefaultAsync(m => m.id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.cart == null)
            {
                return Problem("Entity set 'SweetContext.cart'  is null.");
            }
            var cart = await _context.cart.FindAsync(id);
            if (cart != null)
            {
                _context.cart.Remove(cart);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
          return (_context.cart?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
