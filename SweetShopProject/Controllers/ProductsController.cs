using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using SweetShopProject.Models;
namespace SweetShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly SweetContext _context;
        public IHostingEnvironment _env;

        public ProductsController(SweetContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env; 
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var sweetContext = _context.product.Include(p => p.cat).Include(p => p.city);
            return View(await sweetContext.ToListAsync());
        }



        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.product == null)
            {
                return NotFound();
            }

            var product = await _context.product
                .Include(p => p.cat)
                .Include(p => p.city)
                .FirstOrDefaultAsync(m => m.id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["catID"] = new SelectList(_context.category, "id", "categoryName");
            ViewData["cityID"] = new SelectList(_context.cities, "id", "city");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            int qty = int.Parse(Request.Form["qty"].ToString());
            var nam = Path.Combine(_env.WebRootPath + "/Images", Path.GetFileName(product.formFile.FileName));
            product.formFile.CopyTo(new FileStream(nam, FileMode.Create));
            product.imgpath = "Images/" + product.formFile.FileName;
            _context.Add(product);
            await _context.SaveChangesAsync();
            Inventory inv = new Inventory()
            {
                prodID = product.id,
                catID = product.catID,
                quantityAvail=qty,
                 totalQuantity=qty
            };
            _context.inventory.Add(inv);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            //if (ModelState.IsValid)
            //{
            
            //_context.product.Add(product);

            
                return RedirectToAction(nameof(Index));
            //}
            //ViewData["catID"] = new SelectList(_context.category, "id", "categoryName", product.catID);
            //ViewData["cityID"] = new SelectList(_context.cities, "id", "city", product.cityID);
            //return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.product == null)
            {
                return NotFound();
            }

            var product = await _context.product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["catID"] = new SelectList(_context.category, "id", "categoryName", product.catID);
            ViewData["cityID"] = new SelectList(_context.cities, "id", "city", product.cityID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,prodName,price,description,quantity,imgpath,catID,cityID")] Product product)
        {
            if (id != product.id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            //ViewData["catID"] = new SelectList(_context.category, "id", "categoryName", product.catID);
            //ViewData["cityID"] = new SelectList(_context.cities, "id", "city", product.cityID);
            //return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.product == null)
            {
                return NotFound();
            }

            var product = await _context.product
                .Include(p => p.cat)
                .Include(p => p.city)
                .FirstOrDefaultAsync(m => m.id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.product == null)
            {
                return Problem("Entity set 'SweetContext.product'  is null.");
            }
            var product = await _context.product.FindAsync(id);
            if (product != null)
            {
                _context.product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.product?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
