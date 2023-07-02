using DotnetTest.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotnetTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly DataContext _context;

        public PatientController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Patient>>> GetPatients()
        {
            return Ok(await _context.Patients.ToListAsync());
        }

        [HttpGet]
        [Route("Patient-Doctor")]
        public async Task<ActionResult<List<Patient>>> GetPatientsWithDoctors()
        {
            return Ok(_context.Patients.Include(a => a.Doctor).ToList());
        }

        [HttpGet("SearchPatientByDoctorName")]
        public async Task<ActionResult<List<Patient>>> SearchPatientByDoctorName(string? searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                return await GetPatientsWithDoctors();
            }
            return Ok(_context.Patients.Include(a => a.Doctor).Where(a => a.Doctor.Name.Contains(searchValue)).ToList());
        }

        [HttpPost]
        public async Task<ActionResult<List<Patient>>> CreatePatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return Ok(await _context.Patients.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Patient>>> DeletePatient(int id)
        {
            var dbPatient = await _context.Patients.FindAsync(id);
            if (dbPatient == null)
                return BadRequest("Patient not found");

            _context.Patients.Remove(dbPatient);
            await _context.SaveChangesAsync();

            return Ok(await _context.Patients.ToListAsync());
        }
    }
}
