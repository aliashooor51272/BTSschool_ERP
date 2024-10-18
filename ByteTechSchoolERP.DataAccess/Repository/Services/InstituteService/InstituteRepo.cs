using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.GenericRepository;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IInstituite;
using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.InstitudesProfile;
using ByteTechSchoolERP.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.DataAccess.Repository.Services.InstituteService
{
    public class IntituiteRepo : GenericRepository<Instituite_VM>, IInstituteProfile
    {
        private readonly ByteTechSchoolERPContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public IntituiteRepo(ByteTechSchoolERPContext db, IWebHostEnvironment hostingEnvironment) : base(db)
        {
            _context = db;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<ResponseModel> AddIntitute(Instituite_VM institute)
        {
            var responseModel = new ResponseModel();

            try
            {
                Institute inst = new Institute
                {
                    PhoneNumber = institute.PhoneNumber,
                    Address = institute.Address,
                    WebsiteUrl = institute.WebsiteUrl,
                    Name = institute.Name,
                    City = institute.City,
                    Country = institute.Country
                };

                if (institute.ImageFileVM != null)
                {
                    var imageFolder = Path.Combine(_hostingEnvironment.WebRootPath, "InstitudeImage");
                    if (!Directory.Exists(imageFolder))
                    {
                        Directory.CreateDirectory(imageFolder);
                    }

                    var uniqueFileName = Guid.NewGuid() + Path.GetExtension(institute.ImageFileVM.FileName);
                    var imagePathOnDisk = Path.Combine(imageFolder, uniqueFileName);

                    using (var stream = new FileStream(imagePathOnDisk, FileMode.Create))
                    {
                        await institute.ImageFileVM.CopyToAsync(stream);
                    }

                    inst.ImagePath = uniqueFileName; // Save only the unique filename
                }

                await _context.Institutes.AddAsync(inst);
                await _context.SaveChangesAsync();

                responseModel.isSuccess = true;
                responseModel.Message = "/InstitudeImage/" + inst.ImagePath;
            }
            catch (Exception ex)
            {
                responseModel.isSuccess = false;
                responseModel.Message = $"An error occurred: {ex.Message}";
            }

            return responseModel;
        }
    }
}
