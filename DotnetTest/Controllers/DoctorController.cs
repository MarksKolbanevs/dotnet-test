using DotnetTest.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotnetTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DoctorController(DataContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<List<Doctor>>> GetDoctors()
        {
            var doctors = await _context.Doctors.ToListAsync();

            foreach (var doctor in doctors)
            {
                var imagePath = Path.Combine(_hostingEnvironment.ContentRootPath, "assets", doctor.PhotoName);


                if (!System.IO.File.Exists(imagePath))
                {
                    return NotFound();
                }

                // Read the image file as an array of bytes.
                var imageBytes = System.IO.File.ReadAllBytes(imagePath);

                // Encode the image bytes to a base64 string.
                doctor.PhotoBytes = Convert.ToBase64String(imageBytes);
            }

            return Ok(doctors);
        }

        [HttpGet("SearchDoctorByName")]
        public async Task<ActionResult<List<Doctor>>> SearchDoctorByName(string? searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                return await GetDoctors(); // Return all doctors if input is empty
            }

            var doctors = await _context.Doctors.Where(x => x.Name.Contains(searchValue)).ToListAsync();

            foreach (var doctor in doctors)
            {
                var imagePath = Path.Combine(_hostingEnvironment.ContentRootPath, "assets", doctor.PhotoName);

                if (!System.IO.File.Exists(imagePath))
                {
                    return NotFound();
                }

                // Read the image file as an array of bytes.
                var imageBytes = System.IO.File.ReadAllBytes(imagePath);

                // Encode the image bytes to a base64 string.
                doctor.PhotoBytes = Convert.ToBase64String(imageBytes);
            }

            return Ok(doctors);
        }


        [HttpPost]
        public async Task<ActionResult<List<Doctor>>> CreateDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();
            return Ok(await _context.Doctors.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Doctor>>> DeleteDoctor(int id)
        {
            var dbDoctor = await _context.Doctors.FindAsync(id);
            if (dbDoctor == null)
                return BadRequest("Doctor not found");

            _context.Doctors.Remove(dbDoctor);
            await _context.SaveChangesAsync();

            return Ok(await _context.Doctors.ToListAsync());
        }
    }
}