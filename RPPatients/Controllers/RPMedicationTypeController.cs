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
    public class RPMedicationTypeController : Controller
    {
        private readonly PatientsContext _context;

        public RPMedicationTypeController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RPMedicationType
        // Action to view contents from MedicationType table
        public async Task<IActionResult> Index()
        {
            return View(await _context.MedicationTypes.OrderBy(m => m.Name).ToListAsync());
        }

        // GET: RPMedicationType/Details/5
        //Action used to get details from MedicationType table
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MedicationTypes == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationTypes
                .FirstOrDefaultAsync(m => m.MedicationTypeId == id);
            if (medicationType == null)
            {
                return NotFound();
            }

            return View(medicationType);
        }

        // GET: RPMedicationType/Create
        //Action used to create setup,to display a blank input page 
        public IActionResult Create()
        {
            return View();
        }

        // POST: RPMedicationType/Create
        //Action to create data and check if the data is valid
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicationTypeId,Name")] MedicationType medicationType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicationType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicationType);
        }

        // GET: RPMedicationType/Edit/5
        //Action used to update data
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MedicationTypes == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationTypes.FindAsync(id);
            if (medicationType == null)
            {
                return NotFound();
            }
            return View(medicationType);
        }

        // POST: RPMedicationType/Edit/5
        //Action used to send data to be edited
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicationTypeId,Name")] MedicationType medicationType)
        {
            if (id != medicationType.MedicationTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicationType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationTypeExists(medicationType.MedicationTypeId))
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
            return View(medicationType);
        }

        // GET: RPMedicationType/Delete/5
        //Action used ensure data to be deleted
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MedicationTypes == null)
            {
                return NotFound();
            }

            var medicationType = await _context.MedicationTypes
                .FirstOrDefaultAsync(m => m.MedicationTypeId == id);
            if (medicationType == null)
            {
                return NotFound();
            }

            return View(medicationType);
        }

        // POST: RPMedicationType/Delete/5
        //Action used ensure data to be deleted
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MedicationTypes == null)
            {
                return Problem("Entity set 'PatientsContext.MedicationTypes'  is null.");
            }
            var medicationType = await _context.MedicationTypes.FindAsync(id);
            if (medicationType != null)
            {
                _context.MedicationTypes.Remove(medicationType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationTypeExists(int id)
        {
          return _context.MedicationTypes.Any(e => e.MedicationTypeId == id);
        }
    }
}
