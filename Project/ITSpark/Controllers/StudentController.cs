using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITSpark.Models;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;


namespace ITSpark.Controllers
{

    public class StudentController : Controller
    {


        private readonly ITSparkDbContext _context;
        private readonly IStringLocalizer<StudentController> _localizer;

        public StudentController(ITSparkDbContext context, IStringLocalizer<StudentController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }



        public IActionResult Index(string nameSearch, string genderSearch, string citySearch, DateTime? dateOfBirthFrom, DateTime? dateOfBirthTo, int pageNumber = 1, int pageSize = 3)
        {
            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(nameSearch))
            {
                // Perform exact match search
                query = query.Where(s => s.Name.Equals(nameSearch));
            }

            if (!string.IsNullOrEmpty(genderSearch))
            {
                query = query.Where(s => s.Gender == genderSearch);
            }

            if (!string.IsNullOrEmpty(citySearch))
            {
                query = query.Where(s => s.City == citySearch);
            }

            if (dateOfBirthFrom.HasValue)
            {
                query = query.Where(s => s.DateOfBirth >= dateOfBirthFrom.Value);
            }

            if (dateOfBirthTo.HasValue)
            {
                query = query.Where(s => s.DateOfBirth <= dateOfBirthTo.Value);
            }

            // Add pagination
            int totalStudents = query.Count();
            var students = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Pass the pagination data to the view
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalStudents / pageSize);
            ViewData["CurrentPage"] = pageNumber;

            //var students = query.ToList();
            return View(students);
        }



        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var students = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (students == null)
            {
                return NotFound();
            }

            return View(students);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Gender,City,DateOfBirth,IsEnrolled")] Students students)
        {
            if (ModelState.IsValid)
            {
                _context.Add(students);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(students);
        }




        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var students = await _context.Students.FindAsync(id);
            if (students == null)
            {
                return NotFound();
            }
            return View(students);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Gender,City,DateOfBirth,IsEnrolled")] Students students)
        {
            if (id != students.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(students);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentsExists(students.Id))
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
            return View(students);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var students = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (students == null)
            {
                return NotFound();
            }

            return View(students);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'ITSparkDbContext.Students'  is null.");
            }
            var students = await _context.Students.FindAsync(id);
            if (students != null)
            {
                _context.Students.Remove(students);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentsExists(int id)
        {
          return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }


    }

}
