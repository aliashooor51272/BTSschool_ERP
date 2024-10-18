using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.IRepository.UnitOfWork;
using ByteTechSchoolERP.Models.Exam;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExamsController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ExamsController(ByteTechSchoolERPContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public IActionResult AddTerm()
        {
            return View();
        }
        public IActionResult AddGrade()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTerm(Term term)
        {
            // Check if the term name already exists before proceeding


            // Proceed to add or update the term if the name is unique
            var response = await _unitOfWork.Term.AddOrUpdateTerm(term);
            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = response.Message;
                return Json(new { isSuccess = true, message = response.Message });
            }
            else
            {
                return Json(new { isSuccess = false, message = "Failed to add term." });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGrade(Grade grade)
        {
            //// Check if a grade with the same GradeTitle already exists
            //var existingGrade = await _unitOfWork.Grade(
            //    .FirstOrDefaultAsync(g => g.GradeTitle == grade.GradeTitle && g.Id != grade.Id);

            //if (existingGrade != null)
            //{
            //    // Grade with the same title already exists
            //    return Json(new { isSuccess = false, message = "A grade with this title already exists." });
            //}

            // Proceed to add or update the grade
            var response = await _unitOfWork.Grade.AddOrUpdateGrade(grade);
            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = response.Message;
                return Json(new { isSuccess = true, message = response.Message });
            }
            else
            {
                return Json(new { isSuccess = false, message = "Failed to add or update grade." });
            }
        }



        [HttpGet]
        public IActionResult TermList()
        {
            var terms = _unitOfWork.Term.GetTermList(); // Assuming you have this method in your repository
            var termList = terms.Select(t => new
            {
                id = t.Id,
                name = t.Name, // Ensure this matches the key in the DataTable column configuration
                description = t.Description
            }).ToList();

            return Json(new { data = termList });
        }
        [HttpGet]
        public IActionResult GradeList()
        {
            // Retrieve the list of grades from the repository
            var grades = _unitOfWork.Grade.GetGradeList(); // Assuming this method exists and returns a list of Grade entities

            // Map the retrieved grades to a list of anonymous objects with the required properties
            var termList = grades.Select(g => new
            {
                id = g.Id, // Ensure the key matches the DataTable column configuration
                title = g.GradeTitle, // Mapping to GradeTitle
                description = g.Description,
                grades = g.Grades,
                maxPercentage = g.MaximumPercentage,
                minPercentage = g.MinimumPercentage,
                remark = g.Remark
            }).ToList();

            // Return the data as a JSON response
            return Json(new { data = termList });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTerm(Term model) // Renamed Editerm to EditTerm
        {
            var response = await _unitOfWork.Term.AddOrUpdateTerm(model); // Use Term repository method
            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGrade(Grade model) // Renamed Editerm to EditTerm
        {
            var response = await _unitOfWork.Grade.AddOrUpdateGrade(model); // Use Term repository method
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTerm(Guid id)
        {
            var response = await _unitOfWork.Term.DeleteTermById(id); // Use Term repository method
            return Json(new { isSuccess = response });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGrade(Guid id)
        {
            var response = await _unitOfWork.Grade.DeleteGradeById(id); // Use Term repository method
            return Json(new { isSuccess = response });
        }

    }

}