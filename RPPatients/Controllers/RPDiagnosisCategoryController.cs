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
    public class RPDiagnosisCategoryController : Controller
    {
        private readonly PatientsContext _context;

        public RPDiagnosisCategoryController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RPDiagnosisCategory
        // Action to view contents from DiagnosisCategory table
        public async Task<IActionResult> Index()
        {
              return View(await _context.DiagnosisCategories.ToListAsync());
        }

        // GET: RPDiagnosisCategory/Details/5
        //Action used to get details from DiagnosisCategory table
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DiagnosisCategories == null)
            {
                return NotFound();
            }

            var diagnosisCategory = await _context.DiagnosisCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diagnosisCategory == null)
            {
                return NotFound();
            }

            return View(diagnosisCategory);
        }

        // GET: RPDiagnosisCategory/Create
        //Action used to create setup,to display a blank input page 
        public IActionResult Create()
        {
            return View();
        }

        // POST: RPDiagnosisCategory/Create
        //Action to create data and check if the data is valid
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] DiagnosisCategory diagnosisCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diagnosisCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(diagnosisCategory);
        }

        // GET: RPDiagnosisCategory/Edit/5
        //Action used to update data
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DiagnosisCategories == null)
            {
                return NotFound();
            }

            var diagnosisCategory = await _context.DiagnosisCategories.FindAsync(id);
            if (diagnosisCategory == null)
            {
                return NotFound();
            }
            return View(diagnosisCategory);
        }

        // POST: RPDiagnosisCategory/Edit/5
        //Action used to send data to be edited
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] DiagnosisCategory diagnosisCategory)
        {
            if (id != diagnosisCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diagnosisCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnosisCategoryExists(diagnosisCategory.Id))
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
            return View(diagnosisCategory);
        }

        // GET: RPDiagnosisCategory/Delete/5
        //Action used ensure data to be deleted
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DiagnosisCategories == null)
            {
                return NotFound();
            }

            var diagnosisCategory = await _context.DiagnosisCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diagnosisCategory == null)
            {
                return NotFound();
            }

            return View(diagnosisCategory);
        }

        // POST: RPDiagnosisCategory/Delete/5
        //Action used to delete data to be deleted
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DiagnosisCategories == null)
            {
                return Problem("Entity set 'PatientsContext.DiagnosisCategories'  is null.");
            }
            var diagnosisCategory = await _context.DiagnosisCategories.FindAsync(id);
            if (diagnosisCategory != null)
            {
                _context.DiagnosisCategories.Remove(diagnosisCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiagnosisCategoryExists(int id)
        {
          return _context.DiagnosisCategories.Any(e => e.Id == id);
        }
    }
}
