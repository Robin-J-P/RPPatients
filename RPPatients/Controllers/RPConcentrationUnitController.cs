using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPPatients.Models;

namespace RPPatients.Controllers
{
    public class RPConcentrationUnitController : Controller
    {
        private readonly PatientsContext _context;

        public RPConcentrationUnitController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RPConcentrationUnit
        // Action to view contents of ConcentrationUnit
        public async Task<IActionResult> Index()
        {
              return View(await _context.ConcentrationUnits.ToListAsync());
        }

        // GET: RPConcentrationUnit/Details/5
        //Action used to get details from ConcentrationUnit
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.ConcentrationUnits == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnits
                .FirstOrDefaultAsync(m => m.ConcentrationCode == id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }

            return View(concentrationUnit);
        }

        // GET: RPConcentrationUnit/Create
        //Action used to create setup,to display a blank input page 
        public IActionResult Create()
        {
            return View();
        }
        //Action to create data and check if the data is valid
        // POST: RPConcentrationUnit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConcentrationCode")] ConcentrationUnit concentrationUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concentrationUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(concentrationUnit);
        }
        //Action used to update data
        // GET: RPConcentrationUnit/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.ConcentrationUnits == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnits.FindAsync(id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }
            return View(concentrationUnit);
        }
        //Action used to send data to be edited
        // POST: RPConcentrationUnit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ConcentrationCode")] ConcentrationUnit concentrationUnit)
        {
            if (id != concentrationUnit.ConcentrationCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concentrationUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcentrationUnitExists(concentrationUnit.ConcentrationCode))
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
            return View(concentrationUnit);
        }
        //Action used ensure data to be deleted
        // GET: RPConcentrationUnit/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.ConcentrationUnits == null)
            {
                return NotFound();
            }

            var concentrationUnit = await _context.ConcentrationUnits
                .FirstOrDefaultAsync(m => m.ConcentrationCode == id);
            if (concentrationUnit == null)
            {
                return NotFound();
            }

            return View(concentrationUnit);
        }
        //Action used to delete data to be deleted
        // POST: RPConcentrationUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ConcentrationUnits == null)
            {
                return Problem("Entity set 'PatientsContext.ConcentrationUnits'  is null.");
            }
            var concentrationUnit = await _context.ConcentrationUnits.FindAsync(id);
            if (concentrationUnit != null)
            {
                _context.ConcentrationUnits.Remove(concentrationUnit);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConcentrationUnitExists(string id)
        {
          return _context.ConcentrationUnits.Any(e => e.ConcentrationCode == id);
        }
    }
}
