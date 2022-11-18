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
    public class RPDiagnosisController : Controller
    {
        private readonly PatientsContext _context;

        public RPDiagnosisController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RPDiagnosis
        public async Task<IActionResult> Index()
        {
            var patientsContext = _context.Diagnoses.Include(d => d.DiagnosisCategory);
            return View(await patientsContext.ToListAsync());
        }

        // GET: RPDiagnosis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Diagnoses == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .Include(d => d.DiagnosisCategory)
                .FirstOrDefaultAsync(m => m.DiagnosisId == id);
            if (diagnosis == null)
            {
                return NotFound();
            }

            return View(diagnosis);
        }

        // GET: RPDiagnosis/Create
        public IActionResult Create()
        {
            ViewData["DiagnosisCategoryId"] = new SelectList(_context.DiagnosisCategories, "Id", "Id");
            return View();
        }

        // POST: RPDiagnosis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiagnosisId,Name,DiagnosisCategoryId")] Diagnosis diagnosis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(diagnosis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiagnosisCategoryId"] = new SelectList(_context.DiagnosisCategories, "Id", "Id", diagnosis.DiagnosisCategoryId);
            return View(diagnosis);
        }

        // GET: RPDiagnosis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Diagnoses == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses.FindAsync(id);
            if (diagnosis == null)
            {
                return NotFound();
            }
            ViewData["DiagnosisCategoryId"] = new SelectList(_context.DiagnosisCategories, "Id", "Id", diagnosis.DiagnosisCategoryId);
            return View(diagnosis);
        }

        // POST: RPDiagnosis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiagnosisId,Name,DiagnosisCategoryId")] Diagnosis diagnosis)
        {
            if (id != diagnosis.DiagnosisId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diagnosis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnosisExists(diagnosis.DiagnosisId))
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
            ViewData["DiagnosisCategoryId"] = new SelectList(_context.DiagnosisCategories, "Id", "Id", diagnosis.DiagnosisCategoryId);
            return View(diagnosis);
        }

        // GET: RPDiagnosis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Diagnoses == null)
            {
                return NotFound();
            }

            var diagnosis = await _context.Diagnoses
                .Include(d => d.DiagnosisCategory)
                .FirstOrDefaultAsync(m => m.DiagnosisId == id);
            if (diagnosis == null)
            {
                return NotFound();
            }

            return View(diagnosis);
        }

        // POST: RPDiagnosis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Diagnoses == null)
            {
                return Problem("Entity set 'PatientsContext.Diagnoses'  is null.");
            }
            var diagnosis = await _context.Diagnoses.FindAsync(id);
            if (diagnosis != null)
            {
                _context.Diagnoses.Remove(diagnosis);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiagnosisExists(int id)
        {
          return _context.Diagnoses.Any(e => e.DiagnosisId == id);
        }
    }
}
