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
    public class RPPatientTreatmentsController : Controller
    {
        private readonly PatientsContext _context;

        public RPPatientTreatmentsController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RPPatientTreatments
        public async Task<IActionResult> Index(string PatientDiagnosisId)
        {
            if (PatientDiagnosisId != null)
            {
                Response.Cookies.Append("PatientDiagnosisId", PatientDiagnosisId);
            }
            else if (Request.Query["PatientDiagnosisId"].Any())
            {
                Request.Query["PatientDiagnosisId"].ToString();
                Response.Cookies.Append("PatientDiagnosisId", PatientDiagnosisId);
   
            }
            else if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PatientDiagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            
            else
            {
                TempData["message"] = "Please select any Patient DiagnosisId ID";
                return RedirectToAction("Index", "RPPatientDiagnosis");
            }
            var Filter = _context.PatientDiagnosis.Where(a => a.PatientDiagnosisId == Convert.ToInt32(PatientDiagnosisId)).FirstOrDefault();
            int DiagnosisId = Filter.DiagnosisId;
            int PatientId = Filter.PatientId;
            var Name = _context.Patients.Where(p => p.PatientId == PatientId).FirstOrDefault();
            var Filter1 = _context.Diagnoses.Where(p => p.DiagnosisId == DiagnosisId).FirstOrDefault();
            ViewData["DiagnosisName"] = Filter1.Name;
            ViewData["FirstName"] = Name.FirstName;
            ViewData["LastName"] = Name.LastName;
            var patientsContext = _context.PatientTreatments
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .Where(p => p.PatientDiagnosisId == Convert.ToInt32(PatientDiagnosisId))
                .OrderBy(p => p.DatePrescribed);
            return View(await patientsContext.ToListAsync());
        }

        // GET: RPPatientTreatments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string PatientDiagnosisId = String.Empty;

            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PatientDiagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            var Filter = _context.PatientDiagnosis.Where(a => a.PatientDiagnosisId == Convert.ToInt32(PatientDiagnosisId)).FirstOrDefault();
            int DiagnosisId = Filter.DiagnosisId;
            int PatientId = Filter.PatientId;
            var Name = _context.Patients.Where(p => p.PatientId == PatientId).FirstOrDefault();
            var Filter1 = _context.Diagnoses.Where(p => p.DiagnosisId == DiagnosisId).FirstOrDefault();
            ViewData["DiagnosisName"] = Filter1.Name;
            ViewData["FirstName"] = Name.FirstName;
            ViewData["LastName"] = Name.LastName;
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId");
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(p => p.DiagnosisId == DiagnosisId), "TreatmentId", "TreatmentId");
            if (id == null || _context.PatientTreatments == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatments
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .FirstOrDefaultAsync(m => m.PatientTreatmentId == id);
            if (patientTreatment == null)
            {
                return NotFound();
            }

            return View(patientTreatment);
        }

        // GET: RPPatientTreatments/Create
        public IActionResult Create()
        {
            string PatientDiagnosisId = String.Empty;

            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PatientDiagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            var Filter = _context.PatientDiagnosis.Where(a => a.PatientDiagnosisId == Convert.ToInt32(PatientDiagnosisId)).FirstOrDefault();
            int DiagnosisId = Filter.DiagnosisId;
            int PatientId = Filter.PatientId;
            var Name = _context.Patients.Where(p => p.PatientId == PatientId).FirstOrDefault();
            var Filter1 = _context.Diagnoses.Where(p => p.DiagnosisId == DiagnosisId).FirstOrDefault();
            ViewData["DiagnosisName"] = Filter1.Name;
            ViewData["FirstName"] = Name.FirstName;
            ViewData["LastName"] = Name.LastName;
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId");
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(p => p.DiagnosisId == DiagnosisId), "TreatmentId", "TreatmentId");
            return View();
        }

        // POST: RPPatientTreatments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientTreatmentId,TreatmentId,DatePrescribed,Comments,PatientDiagnosisId")] PatientTreatment patientTreatment)
        {
            string PatientDiagnosisId = String.Empty;

            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PatientDiagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            var Filter = _context.PatientDiagnosis.Where(a => a.PatientDiagnosisId == Convert.ToInt32(PatientDiagnosisId)).FirstOrDefault();
            int DiagnosisId = Filter.DiagnosisId;
            int PatientId = Filter.PatientId;
            var Name = _context.Patients.Where(p => p.PatientId == PatientId).FirstOrDefault();
            var Filter1 = _context.Diagnoses.Where(p => p.DiagnosisId == DiagnosisId).FirstOrDefault();
            ViewData["DiagnosisName"] = Filter1.Name;
            ViewData["FirstName"] = Name.FirstName;
            ViewData["LastName"] = Name.LastName;
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId");
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(p => p.DiagnosisId == DiagnosisId), "TreatmentId", "TreatmentId");
            if (ModelState.IsValid)
            {
                _context.Add(patientTreatment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            DateTime now = DateTime.Now;
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(p => p.DiagnosisId == DiagnosisId), "TreatmentId", "TreatmentId");
            return View(patientTreatment);
        }

        // GET: RPPatientTreatments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.PatientTreatments == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatments.FindAsync(id);
            if (patientTreatment == null)
            {
                return NotFound();
            }
            string PatientDiagnosisId = String.Empty;

            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PatientDiagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            var Filter = _context.PatientDiagnosis.Where(a => a.PatientDiagnosisId == Convert.ToInt32(PatientDiagnosisId)).FirstOrDefault();
            int DiagnosisId = Filter.DiagnosisId;
            int PatientId = Filter.PatientId;
            var Name = _context.Patients.Where(p => p.PatientId == PatientId).FirstOrDefault();
            var Filter1 = _context.Diagnoses.Where(p => p.DiagnosisId == DiagnosisId).FirstOrDefault();
            ViewData["DiagnosisName"] = Filter1.Name;
            ViewData["FirstName"] = Name.FirstName;
            ViewData["LastName"] = Name.LastName;
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId");
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(p => p.DiagnosisId == DiagnosisId), "TreatmentId", "TreatmentId");
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(p => p.DiagnosisId == DiagnosisId), "TreatmentId", "TreatmentId");
            //var date = _context.PatientTreatments.Where(p => p.DiagnosisId == DiagnosisId).FirstOrDefault();
            //return View(patient);
            return View(patientTreatment);

        }

        // POST: RPPatientTreatments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientTreatmentId,TreatmentId,DatePrescribed,Comments,PatientDiagnosisId")] PatientTreatment patientTreatment)
        {
            string PatientDiagnosisId = String.Empty;

            if (Request.Cookies["PatientDiagnosisId"] != null)
            {
                PatientDiagnosisId = Request.Cookies["PatientDiagnosisId"].ToString();
            }
            var Filter = _context.PatientDiagnosis.Where(a => a.PatientDiagnosisId == Convert.ToInt32(PatientDiagnosisId)).FirstOrDefault();
            int DiagnosisId = Filter.DiagnosisId;
            int PatientId = Filter.PatientId;
            var Name = _context.Patients.Where(p => p.PatientId == PatientId).FirstOrDefault();
            var Filter1 = _context.Diagnoses.Where(p => p.DiagnosisId == DiagnosisId).FirstOrDefault();
            ViewData["DiagnosisName"] = Filter1.Name;
            ViewData["FirstName"] = Name.FirstName;
            ViewData["LastName"] = Name.LastName;
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId");
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(p => p.DiagnosisId == DiagnosisId), "TreatmentId", "TreatmentId");

            if (id != patientTreatment.PatientTreatmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientTreatment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientTreatmentExists(patientTreatment.PatientTreatmentId))
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
            ViewData["PatientDiagnosisId"] = new SelectList(_context.PatientDiagnosis, "PatientDiagnosisId", "PatientDiagnosisId", patientTreatment.PatientDiagnosisId);
            ViewData["TreatmentId"] = new SelectList(_context.Treatments.Where(p => p.DiagnosisId == DiagnosisId), "TreatmentId", "TreatmentId");
            return View(patientTreatment);
        }

        // GET: RPPatientTreatments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PatientTreatments == null)
            {
                return NotFound();
            }

            var patientTreatment = await _context.PatientTreatments
                .Include(p => p.PatientDiagnosis)
                .Include(p => p.Treatment)
                .FirstOrDefaultAsync(m => m.PatientTreatmentId == id);
            if (patientTreatment == null)
            {
                return NotFound();
            }

            return View(patientTreatment);
        }

        // POST: RPPatientTreatments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PatientTreatments == null)
            {
                return Problem("Entity set 'PatientsContext.PatientTreatments'  is null.");
            }
            var patientTreatment = await _context.PatientTreatments.FindAsync(id);
            if (patientTreatment != null)
            {
                _context.PatientTreatments.Remove(patientTreatment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientTreatmentExists(int id)
        {
          return _context.PatientTreatments.Any(e => e.PatientTreatmentId == id);
        }
    }
}
