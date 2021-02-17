using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ionici_Floriana_Proiect.Data;
using Ionici_Floriana_Proiect.Models;
using Ionici_Floriana_Proiect.Models.BakeryViewModels;

namespace Ionici_Floriana_Proiect.Controllers
{
    public class OwnersController : Controller
    {
        private readonly BakeryContext _context;

        public OwnersController(BakeryContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(int? id, int? cakeID)
        {
            var viewModel = new OwnersIndexData();
            viewModel.Owners = await _context.Owners
            .Include(i => i.OwnedCake)
            .ThenInclude(i => i.Cake)
            .ThenInclude(i => i.Orders)
            .ThenInclude(i => i.Customer)
            .AsNoTracking()
            .OrderBy(i => i.OwnerName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["OwnerID"] = id.Value;
                Owners owners = viewModel.Owners.Where(
                i => i.ID == id.Value).Single();
                viewModel.Cakes = owners.OwnedCake.Select(s => s.Cake);
            }
            if (cakeID != null)
            {
                ViewData["CakeID"] = cakeID.Value;
                viewModel.Orders = viewModel.Cakes.Where(
                x => x.ID == cakeID).Single().Orders;
            }
            return View(viewModel);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owners = await _context.Owners
                .FirstOrDefaultAsync(m => m.ID == id);
            if (owners == null)
            {
                return NotFound();
            }

            return View(owners);
        }

        // GET: Owners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Owners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,OwnerName,Adress")] Owners owners)
        {
            if (ModelState.IsValid)
            {
                _context.Add(owners);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(owners);
        }

        // GET: Owners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var owner = await _context.Owners
            .Include(i => i.OwnedCake).ThenInclude(i => i.Cake)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ID == id);
            if (owner == null)
            {
                return NotFound();
            }
            PopulateOwnedCakeData(owner);
            return View(owner);

        }
        private void PopulateOwnedCakeData(Owners owners)
        {
            var allCakes = _context.Cake;
            var ownedCake = new HashSet<int>(owners.OwnedCake.Select(c => c.CakeID));
            var viewModel = new List<OwnedCakeData>();
            foreach (var cake in allCakes)
            {
                viewModel.Add(new OwnedCakeData
                {
                    CakeID = cake.ID,
                    Name = cake.Name,
                    IsOwned = ownedCake.Contains(cake.ID)
                });
            }
            ViewData["Cake"] = viewModel;
        }
        // POST: Owners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedCakes)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ownersToUpdate = await _context.Owners
            .Include(i => i.OwnedCake)
            .ThenInclude(i => i.Cake)
            .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Owners>(
            ownersToUpdate,
            "",
            i => i.OwnerName, i => i.Adress))
            {
                UpdateOwnedCakes(selectedCakes, ownersToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateOwnedCakes(selectedCakes, ownersToUpdate);
            PopulateOwnedCakeData(ownersToUpdate);
            return View(ownersToUpdate);
        }
        private void UpdateOwnedCakes(string[] selectedCakes, Owners ownersToUpdate)
        {
            if (selectedCakes == null)
            {
                ownersToUpdate.OwnedCake = new List<OwnedCake>();
                return;
            }
            var selectedCakesHS = new HashSet<string>(selectedCakes);
            var ownedCakes = new HashSet<int>
            (ownersToUpdate.OwnedCake.Select(c => c.Cake.ID));
            foreach (var cake in _context.Cake)
            {
                if (selectedCakesHS.Contains(cake.ID.ToString()))
                {
                    if (!ownedCakes.Contains(cake.ID))
                    {
                        ownersToUpdate.OwnedCake.Add(new OwnedCake
                        {
                            OwnerID =
                       ownersToUpdate.ID,
                            CakeID = cake.ID
                        });
                    }
                }
                else
                {
                    if (ownedCakes.Contains(cake.ID))
                    {
                        OwnedCake cakeToRemove = ownersToUpdate.OwnedCake.FirstOrDefault(i
                       => i.CakeID == cake.ID);
                        _context.Remove(cakeToRemove);
                    }
                }
            }
        }
        // GET: Owners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owners = await _context.Owners
                .FirstOrDefaultAsync(m => m.ID == id);
            if (owners == null)
            {
                return NotFound();
            }

            return View(owners);
        }

        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var owners = await _context.Owners.FindAsync(id);
            _context.Owners.Remove(owners);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OwnersExists(int id)
        {
            return _context.Owners.Any(e => e.ID == id);
        }
    }
}
