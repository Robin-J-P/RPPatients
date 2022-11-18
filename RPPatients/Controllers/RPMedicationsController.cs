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
    public class RPMedicationsController : Controller
    {
        private readonly PatientsContext _context;

        public RPMedicationsController(PatientsContext context)
        {
            _context = context;
        }

        // GET: RPMedications
        public async Task<IActionResult> Index(string MedicationTypeID)
        {
            //Storing the Query value into Cookie
            //Retriving the value when necessry
            if (MedicationTypeID != null)
            {
                Response.Cookies.Append("MedicationTypeID", MedicationTypeID);
            }
            else if (Request.Query["MedicationTypeID"].Any())
            {
                Request.Query["MedicationTypeID"].ToString();
                Response.Cookies.Append("MedicationTypeID", MedicationTypeID);
            }
            else if (Request.Cookies["MedicationTypeID"] != null)
            {
                MedicationTypeID = Request.Cookies["MedicationTypeID"].ToString();
            }
            else
            {
                TempData["message"] = "Please select any Medication Type ID";
                return RedirectToAction("Index", "RPSMedicationTypes");
            }

            //Naming the title
            var medType = _context.MedicationTypes.Where(a => a.MedicationTypeId == Convert.ToInt16(MedicationTypeID)).FirstOrDefault();
            ViewData["MTName"] = medType.Name;

            //Ordring and Showing the value according to the Medication Type ID
            var patientsContext = _context.Medications
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .Where(m => m.MedicationTypeId == Convert.ToInt16(MedicationTypeID))
                .OrderBy(m => m.Name).ThenBy(m => m.Concentration);
            return View(await patientsContext.ToListAsync());
        }
        //Action used to view details
        // GET: RPMedications/Details/5
        public async Task<IActionResult> Details(string id)
        {
            //Retriving the value from the cookies
            string medTypeID = string.Empty;
            if (Request.Cookies["MedicationTypeID"] != null)
            {
                medTypeID = Request.Cookies["MedicationTypeID"].ToString();
            }

            //Naming the title
            var medType = _context.MedicationTypes.Where(a => a.MedicationTypeId == Convert.ToInt32(medTypeID)).FirstOrDefault();
            ViewData["MTName"] = medType.Name;

            if (id == null || _context.Medications == null)
            {
                return NotFound();
            }

            var medication = await _context.Medications
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .FirstOrDefaultAsync(m => m.Din == id);
            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }
        //Action used to create data
        // GET: RPMedications/Create
        public IActionResult Create()
        {
            //Retriving the value from the cookies
            int medTypeID = 0;
            if (Request.Cookies["MedicationTypeID"] != null)
            {
                medTypeID = Convert.ToInt32(Request.Cookies["MedicationTypeID"]);
            }

            //Naming the title
            var medType = _context.MedicationTypes.Where(a => a.MedicationTypeId == Convert.ToInt32(medTypeID)).FirstOrDefault();
            ViewData["MTName"] = medType.Name;
            //Console.Write(medType);
            //int bool = await DisplayAlert("Alert", "You have been alerted", "OK");
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnits.OrderBy(a => a.ConcentrationCode), "ConcentrationCode", "ConcentrationCode");
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnits.OrderBy(a => a.DispensingCode), "DispensingCode", "DispensingCode");
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationTypes, "MedicationTypeId", "MedicationTypeId", medTypeID);
            return View();
        }

        // POST: RPMedications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Action used to create data in database
        public async Task<IActionResult> Create([Bind("Din,Name,Image,MedicationTypeId,DispensingCode,Concentration,ConcentrationCode")] Medication medication)
        {
            //Retriving the value from the cookies
            int medTypeID = 0;
            if (Request.Cookies["MedicationTypeID"] != null)
            {
                medTypeID = Convert.ToInt32(Request.Cookies["MedicationTypeID"]);
            }

            //Naming the title
            var medType = _context.MedicationTypes.Where(a => a.MedicationTypeId == Convert.ToInt32(medTypeID)).FirstOrDefault();
            ViewData["MTName"] = medType.Name;

            //Checking if it is duplicate value then show error
            var isDuplicate = _context.Medications.Where(a => a.Name == medication.Name && a.Concentration == medication.Concentration && a.ConcentrationCode == a.ConcentrationCode);
            if (isDuplicate.Any())
            {
                ModelState.AddModelError("", "Already exists in the database");
            }

            if (ModelState.IsValid)
            {
                _context.Add(medication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //Showing the value in accending order
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnits.OrderBy(a => a.ConcentrationCode), "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnits.OrderBy(a => a.DispensingCode), "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationTypes, "MedicationTypeId", "MedicationTypeId", medTypeID);
            return View(medication);
        }
        //Action used to send data to be edit page
        // GET: RPMedications/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            //Retriving the value from the cookies
            string medTypeID = string.Empty;
            if (Request.Cookies["MedicationTypeID"] != null)
            {
                medTypeID = Request.Cookies["MedicationTypeID"].ToString();
            }

            //Naming the title
            var medType = _context.MedicationTypes.Where(a => a.MedicationTypeId == Convert.ToInt32(medTypeID)).FirstOrDefault();
            ViewData["MTName"] = medType.Name;

            if (id == null || _context.Medications == null)
            {
                return NotFound();
            }

            var medication = await _context.Medications.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }

            //Showing the value in accending order
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnits.OrderBy(a => a.ConcentrationCode), "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnits.OrderBy(a => a.DispensingCode), "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationTypes, "MedicationTypeId", "MedicationTypeId", medication.MedicationTypeId);
            return View(medication);
        }
        //Action used to edit data in database
        // POST: RPMedications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Din,Name,Image,MedicationTypeId,DispensingCode,Concentration,ConcentrationCode")] Medication medication)
        {
            //Retriving the value from the cookies
            string medTypeID = string.Empty;
            if (Request.Cookies["MedicationTypeID"] != null)
            {
                medTypeID = Request.Cookies["MedicationTypeID"].ToString();
            }

            //Naming the title
            var medType = _context.MedicationTypes.Where(a => a.MedicationTypeId == Convert.ToInt32(medTypeID)).FirstOrDefault();
            ViewData["MTName"] = medType.Name;

            if (id != medication.Din)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationExists(medication.Din))
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

            //Showing the value in accending order
            ViewData["ConcentrationCode"] = new SelectList(_context.ConcentrationUnits.OrderBy(a => a.ConcentrationCode), "ConcentrationCode", "ConcentrationCode", medication.ConcentrationCode);
            ViewData["DispensingCode"] = new SelectList(_context.DispensingUnits.OrderBy(a => a.DispensingCode), "DispensingCode", "DispensingCode", medication.DispensingCode);
            ViewData["MedicationTypeId"] = new SelectList(_context.MedicationTypes, "MedicationTypeId", "MedicationTypeId", medication.MedicationTypeId);
            return View(medication);
        }
        //Action used to send data to be deleted to view page
        // GET: RPMedications/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            //Retriving the value from the cookies
            string medTypeID = string.Empty;
            if (Request.Cookies["MedicationTypeID"] != null)
            {
                medTypeID = Request.Cookies["MedicationTypeID"].ToString();
            }

            //Naming the title
            var medType = _context.MedicationTypes.Where(a => a.MedicationTypeId == Convert.ToInt32(medTypeID)).FirstOrDefault();
            ViewData["MTName"] = medType.Name;

            if (id == null || _context.Medications == null)
            {
                return NotFound();
            }

            var medication = await _context.Medications
                .Include(m => m.ConcentrationCodeNavigation)
                .Include(m => m.DispensingCodeNavigation)
                .Include(m => m.MedicationType)
                .FirstOrDefaultAsync(m => m.Din == id);
            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }
        //Action used to delete data in database
        // POST: RPMedications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            //Retriving the value from the cookies
            string medTypeID = string.Empty;
            if (Request.Cookies["MedicationTypeID"] != null)
            {
                medTypeID = Request.Cookies["MedicationTypeID"].ToString();
            }

            //Naming the title
            var medType = _context.MedicationTypes.Where(a => a.MedicationTypeId == Convert.ToInt32(medTypeID)).FirstOrDefault();
            ViewData["MTName"] = medType.Name;

            if (_context.Medications == null)
            {
                return Problem("Entity set 'PatientsContext.Medications'  is null.");
            }
            var medication = await _context.Medications.FindAsync(id);
            if (medication != null)
            {
                _context.Medications.Remove(medication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationExists(string id)
        {
            return _context.Medications.Any(e => e.Din == id);
        }
    }
}
