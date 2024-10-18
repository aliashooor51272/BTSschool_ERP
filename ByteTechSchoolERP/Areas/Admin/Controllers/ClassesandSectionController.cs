using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.IRepository.UnitOfWork;
using ByteTechSchoolERP.Models.ClassAndSection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ClassesandSectionController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ClassesandSectionController(ByteTechSchoolERPContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public IActionResult AddNewClass()
        {
            return View();
        }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddNewClass(Class classes)
    {
        // Check if the class name already exists before proceeding
        if (_context.Classes.Any(c => c.ClassName == classes.ClassName))
        {
            return Json(new { isSuccess = false, message = "Class name already exists." });
        }

        // Proceed to add or update the class if the name is unique
        var response = await _unitOfWork.Class.AddOrUpdateClasses(classes);
        if (response.isSuccess)
        {
            TempData["SuccessMessage"] = response.Message;
            return Json(new { isSuccess = true, message = response.Message });
        }
        else
        {
            return Json(new { isSuccess = false, message = "Validation failed." });
        }
    }

    [HttpPost]
    public IActionResult CheckClassNameExists(string className)
    {
        var exists = _context.Classes.Any(c => c.ClassName == className);
        return Json(new { exists });
    }

        [HttpGet]
        public IActionResult ClassList()
        {
            var classes = _unitOfWork.Class.GetClassList();
            var classList = classes.Select(c => new
            {
                id = c.Id,
                className = c.ClassName,
                description = c.Description
            }).ToList();

            return Json(new { data = classList });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClass(Class model)
        {
            var response = await _unitOfWork.Class.AddOrUpdateClasses(model);
            return Json(response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClass(Guid id)
        {
            var response = await _unitOfWork.Class.DeleteClassById(id);
            return Json(new { isSuccess = response });
        }



        //public async Task<IActionResult> AddNewSection()
        //{
        //    var classes = await _context.Classes.ToListAsync();
        //    ViewBag.Classes = classes;
        //    return View();
        //}


        //[HttpPost]
        //public async Task<IActionResult> AddNewSection(Section section)
        //{
        //    // Check if the section name already exists for the selected class
        //    bool sectionExists = await _context.Sections
        //        .AnyAsync(s => s.Name == section.Name && s.ClassId == section.ClassId);

        //    if (sectionExists)
        //    {
        //        return Json(new { isSuccess = false, message = "Section name already exists." });
        //    }

        //    // Proceed with adding or updating the section
        //    var response = await _unitOfWork.Section.AddOrUpdateSection(section);
        //    if (response.isSuccess)
        //    {
        //        TempData["SuccessMessage"] = response.Message;

        //        // Redirect to the desired location after successful save
        //        return RedirectToAction("AddNewSection", "ClassesandSection");
        //    }
        //    else
        //    {
        //        return Json(new { isSuccess = false, message = "Validation failed." });
        //    }
        //}





        //[HttpGet]
        //public IActionResult SectionList()
        //{
        //    var sections = _unitOfWork.Section.GetSectionList();
        //    var sectionList = sections.Select(s => new
        //    {
        //        id = s.Id,
        //        className = s.Class.ClassName,
        //        name = s.Name,
        //        description = s.Description
        //    }).ToList();

        //    return Json(new { data = sectionList });
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateSection(Section section)
        //{
        //    var response = await _unitOfWork.Section.AddOrUpdateSection(section);
        //    if (response.isSuccess)
        //    {
        //        return Json(new { isSuccess = true, message = response.Message });
        //    }
        //    else
        //    {
        //        return Json(new { isSuccess = false, message = "Update failed." });
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> DeleteSection(Guid id)
        //{
        //    var response = await _unitOfWork.Section.DeleteSectionById(id);
        //    return Json(response);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetClasses()
        //{
        //    var classes = await _context.Classes.Select(c => new { c.Id, c.ClassName }).ToListAsync();
        //    return Json(classes);
        //}



        public async Task<IActionResult> AddNewSection()
        {
            var classes = await _context.Classes.ToListAsync();
            ViewBag.Classes = classes;
            return View();
        }

        // POST: Add New Section
        [HttpPost]
        public async Task<IActionResult> AddNewSection(Section section)
        {
            bool sectionExists = await _context.Sections
                .AnyAsync(s => s.Name == section.Name && s.ClassId == section.ClassId);

            if (sectionExists)
            {
                return Json(new { isSuccess = false, message = "Section name already exists." });
            }

            var response = await _unitOfWork.Section.AddOrUpdateSection(section);
            if (response.isSuccess)
            {
                TempData["SuccessMessage"] = response.Message;
                return Json(new { isSuccess = true, message = response.Message });
            }
            else
            {
                return Json(new { isSuccess = false, message = "Validation failed." });
            }
        }

        // GET: Section List for DataTable
        [HttpGet]
        public IActionResult SectionList()
        {
            var sections = _unitOfWork.Section.GetSectionList();
            var sectionList = sections.Select(s => new
            {
                id = s.Id,
                className = s.Class.ClassName,
                name = s.Name,
                description = s.Description
            }).ToList();

            return Json(new { data = sectionList });
        }

        // POST: Update Section
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSection(Section section)
        {
            var response = await _unitOfWork.Section.AddOrUpdateSection(section);
            if (response.isSuccess)
            {
                return Json(new { isSuccess = true, message = response.Message });
            }
            else
            {
                return Json(new { isSuccess = false, message = "Update failed." });
            }
        }

        // POST: Delete Section
        [HttpPost]
        public async Task<IActionResult> DeleteSection(Guid id)
        {
            var response = await _unitOfWork.Section.DeleteSectionById(id);
            return Json(new { isSuccess = response });
        }

        // GET: Get Classes for Dropdown
        [HttpGet]
        public async Task<IActionResult> GetClasses()
        {
            var classes = await _context.Classes.Select(c => new { c.Id, c.ClassName }).ToListAsync();
            return Json(classes);
        }






    }
}



