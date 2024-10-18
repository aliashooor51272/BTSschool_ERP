using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Filters;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IHostel;
using ByteTechSchoolERP.DataAccess.Repository.IRepository.UnitOfWork;
using ByteTechSchoolERP.Models.Hostel;
using ByteTechSchoolERP.Models.Subjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ByteTechSchoolERP.Areas.Admin.Controllers
{
    [Area("Admin")]
     public class HostelController : Controller
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public HostelController(IHostelRoomTypeService hostelRoomTypeService, ByteTechSchoolERPContext context, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
             _context = context;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> HostelRoom()
        {
            return View();
        }
        public async Task<IActionResult> RoomType()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoomType(HostelRoomType hostelRoomType)
        {
            if (ModelState.IsValid)
            {
                var response = await _unitOfWork.HostelRoomTypeService.AddHostelRoomType(hostelRoomType);
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

            return View("RoomType", hostelRoomType);
        }

        [HttpGet]
        public JsonResult GetHotelRoomTypeData()
        {
            var currentUserId = _httpContextAccessor.HttpContext.Session.GetString("UserId");

            var data = _context.HostelRoomTypes
                               .Where(h => h.UserId == currentUserId) // Filter by UserId
                               .ToList();

            var HotelRoomTypeData = data.Select(l => new
            {
                l.Id,
                l.HostelRoomTypeName,
                l.HostelRoomTypeDescription
            });

            return Json(new { data = HotelRoomTypeData });
        }

        public async Task<IActionResult> Hostel()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRoomType(Guid id)
        {
            var response = await _unitOfWork.HostelRoomTypeService.DeleteRoomTypeById(id);
            return Json(response);
        }

    }
}
