using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ionici_Floriana_Proiect.Data;
using Ionici_Floriana_Proiect.Models;

namespace Ionici_Floriana_Proiect.Controllers
{
    public class CakesController : Controller
    {
        private readonly BakeryContext _context;

        public CakesController(BakeryContext context)
        {
            _context = context;
        }

        // GET: Cakes
        public async Task<IActionResult> Index(string sortOrder,string currentFilter,string searchString,int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var cakes = from b in _context.Cake
                        select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                cakes = cakes.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    cakes = cakes.OrderByDescending(b => b.Name);
                    break;
                case "Price":
                    cakes = cakes.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    cakes = cakes.OrderByDescending(b => b.Price);
                    break;
                default:
                    cakes = cakes.OrderBy(b => b.Name);
                    break;

            }
            int pageSize = 2;
            return View(await PaginatedList<Cake>.CreateAsync(cakes.AsNoTracking(), pageNumber ??
1, pageSize));
        }

        // GET: Cakes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cake = await _context.Cake
            .Include(s => s.Orders)
            .ThenInclude(e => e.Customer)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);  
            
            if (cake == null)
            {
                return NotFound();
            }

            return View(cake);
        }

        // GET: Cakes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cakes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Name,Baker,Price")] Cake cake)
        {
            try
            {
                if (ModelState.IsValid)
            {
                _context.Add(cake);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            }
            catch (DbUpdateException /* ex*/)
            {
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists ");
            }
            
                return View(cake);
        }

        // GET: Cakes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cake = await _context.Cake.FindAsync(id);
            if (cake == null)
            {
                return NotFound();
            }
            return View(cake);
        }

        // POST: Cakes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var studentToUpdate = await _context.Cake.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Cake>(
            studentToUpdate,
            "",
            s => s.Baker, s => s.Name, s => s.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists");
                }
            }
            return View(studentToUpdate);
        }

        // GET: Cakes/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cake = await _context.Cake
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (cake == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again";
            }

            return View(cake);
        }

        // POST: Cakes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cake = await _context.Cake.FindAsync(id);
            if (cake == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Cake.Remove(cake);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {

                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool CakeExists(int id)
        {
            return _context.Cake.Any(e => e.ID == id);
        }
    }
}
