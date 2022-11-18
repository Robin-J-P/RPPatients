using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;
using NuGet.Frameworks;
using RPPatients.Models;

namespace RPPatients.Controllers
{
    public class RPPatientDiagnosisController : Controller
    {
        private readonly PatientsContext _context;

        public RPPatientDiagnosisController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RPPatientDiagnosis
        public async Task<IActionResult> Index(string PatientID, string FirstName, string LastName)
        {
            if (PatientID != null)
            {
                Response.Cookies.Append("PatientID", PatientID);
                Response.Cookies.Append("FirstName", FirstName);
                Response.Cookies.Append("LastName", LastName);
            }
            else if (Request.Query["PatientID"].Any())
            {
                Request.Query["PatientID"].ToString();
                Response.Cookies.Append("PatientID", PatientID);
                Request.Query["FirstName"].ToString();
                Response.Cookies.Append("FirstName", FirstName);
                Request.Query["LastName"].ToString();
                Response.Cookies.Append("LastName", LastName);
            }
            else if (Request.Cookies["PatientID"] != null)
            {
                PatientID = Request.Cookies["PatientID"].ToString();
                FirstName = Request.Cookies["FirstName"].ToString();
                LastName = Request.Cookies["LastName"].ToString();
            }
            else
            {
                TempData["message"] = "Please select any Patient ID";
                return RedirectToAction("Index", "RPPatients");
            }
            TempData["FirstName"] = FirstName;
            TempData["LastName"] = LastName;

            //var patientsContext = _context.PatientDiagnoses.Include(p => p.Diagnosis).Include(p => p.Patient);
            //return View(await patientsContext.ToListAsync());
            //var patients = _context.PatientDiagnosis.Include(a => a.DiagnosisId)
            //        .OrderBy(a => a.Diagnosis.Name).ThenBy(a => a.DiagnosisId);
            //var patientsConte = _context.Diagnoses
            //    .Include(m => m.Name)
            //    .Where(m => m.DiagnosisId == Convert.ToInt32(DiagnosisId))
            //    .OrderBy(m => m.Name).ThenBy(m => m.DiagnosisId);
            //return View(await patientsConte.ToListAsync());

            //var patient = from anPa in _context.PatientDiagnosis
            //              join anArt in _context.Diagnoses on anPa.DiagnosisId equals anArt.DiagnosisId
            //              where anPa.DiagnosisId == anArt.DiagnosisId ||  PatientID == anPa.PatientId
            //              select new PatientDiagnosis
            //              {
            //                  PatientDiagnosisId = anPa.PatientDiagnosisId,
            //                  PatientId = anPa.PatientId,
            //                  DiagnosisId = anPa.DiagnosisId,
            //                  Comments = anPa.Comments,
            //                  Name = anArt,
            //              };
            //return View(patient);
            var patientsCon = _context.PatientDiagnosis.Include(a => a.Diagnosis).Include( b => b.Patient).Include(c =>c.PatientTreatments).Where(m => m.PatientId == Convert.ToInt16(PatientID)).OrderBy(a => a.Patient.LastName).ToListAsync();
            return View(await patientsCon);


        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PatientDiagnosis == null)
            {
                return NotFound();
            }

            var patientDiagnosis = await _context.PatientDiagnosis
                .Include(p => p.Diagnosis)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.PatientDiagnosisId == id);
            if (patientDiagnosis == null)
            {
                return NotFound();
            }

            return View(patientDiagnosis);
        }

        // GET: RPPatientDiagnosis/Create
        public IActionResult Create()
        {
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "DiagnosisId", "DiagnosisId");
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "PatientId");
            return View();
        }

        // POST: RPPatientDiagnosis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientDiagnosisId,PatientId,DiagnosisId,Comments")] PatientDiagnosis patientDiagnosis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(patientDiagnosis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "DiagnosisId", "DiagnosisId", patientDiagnosis.DiagnosisId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "PatientId", patientDiagnosis.PatientId);
            return View(patientDiagnosis);
        }

        // GET: RPPatientDiagnosis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PatientDiagnosis == null)
            {
                return NotFound();
            }

            var patientDiagnosis = await _context.PatientDiagnosis.FindAsync(id);
            if (patientDiagnosis == null)
            {
                return NotFound();
            }
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "DiagnosisId", "DiagnosisId", patientDiagnosis.DiagnosisId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "PatientId", patientDiagnosis.PatientId);
            return View(patientDiagnosis);
        }

        // POST: RPPatientDiagnosis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientDiagnosisId,PatientId,DiagnosisId,Comments")] PatientDiagnosis patientDiagnosis)
        {
            if (id != patientDiagnosis.PatientDiagnosisId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientDiagnosis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientDiagnosisExists(patientDiagnosis.PatientDiagnosisId))
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
            ViewData["DiagnosisId"] = new SelectList(_context.Diagnoses, "DiagnosisId", "DiagnosisId", patientDiagnosis.DiagnosisId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "PatientId", patientDiagnosis.PatientId);
            return View(patientDiagnosis);
        }

        // GET: RPPatientDiagnosis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PatientDiagnosis == null)
            {
                return NotFound();
            }

            var patientDiagnosis = await _context.PatientDiagnosis
                //.Include(p => p.Diagnosis)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(m => m.PatientDiagnosisId == id);
            if (patientDiagnosis == null)
            {
                return NotFound();
            }

            return View(patientDiagnosis);
        }

        // POST: RPPatientDiagnosis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PatientDiagnosis == null)
            {
                return Problem("Entity set 'PatientsContext.PatientDiagnoses'  is null.");
            }
            var patientDiagnosis = await _context.PatientDiagnosis.FindAsync(id);
            if (patientDiagnosis != null)
            {
                _context.PatientDiagnosis.Remove(patientDiagnosis);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientDiagnosisExists(int id)
        {
          return _context.PatientDiagnosis.Any(e => e.PatientDiagnosisId == id);
        }
    }
}
