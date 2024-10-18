using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.GenericRepository;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IExam;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IHostel;
using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.Exam;
using ByteTechSchoolERP.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ByteTechSchoolERP.DataAccess.Repository.Services.ExamService
{
	public class ExamRepo : GenericRepository<ExamList>, IExam
	{
		private readonly ByteTechSchoolERPContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public ExamRepo(ByteTechSchoolERPContext context, IHttpContextAccessor httpContextAccessor = null) : base(context)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
		}
		public async Task<ResponseModel> AddOrUpdateExam(ExamList examList)
		{
			var responseModel = new ResponseModel();

			try
			{
				var existingClass = await _context.ExamLists.FirstOrDefaultAsync(c => c.Id == examList.Id);
				if (existingClass != null)
				{

					existingClass.Id = examList.Id;
					existingClass.ExamName = examList.ExamName;
					existingClass.Description = examList.Description;
					existingClass.TermId = examList.TermId;
					existingClass.ClassId = examList.ClassId;
					existingClass.SeactionId = examList.SeactionId;
					existingClass.GradeId = examList.GradeId;
					existingClass.Session = examList.Session;
					existingClass.AddExamDate = examList.AddExamDate;
					existingClass.IsPublished = examList.IsPublished;

					_context.ExamLists.Update(existingClass);
					responseModel.Message = "Exam updated successfully.";
				}
				else
				{

					await _context.ExamLists.AddAsync(examList);
					responseModel.Message = "Exam added successfully.";
				}

				await _context.SaveChangesAsync();
				responseModel.isSuccess = true;
			}
			catch (Exception ex)
			{
				responseModel.isSuccess = false;
				responseModel.Message = $"Error: {ex.Message}";
			}

			return responseModel;
		}
		public List<ExamViewModel> GetExamList()
		{
			return _context.ExamLists
				.Select(e => new ExamViewModel
				{
					Id = e.Id,
					ExamName = e.ExamName,
					TermId = e.TermId,
					ClassId = e.ClassId,
					SeactionId = e.SeactionId,
					GradeId = e.GradeId,
					Session = e.Session,
					AddExamDate = e.AddExamDate,
					Description = e.Description,
					IsPublished = e.IsPublished

				})
				.ToList();
		}
		public async Task<ResponseModel> DeleteExamById(Guid id)
		{
			var responseModel = new ResponseModel();

			try
			{
				var classToDelete = await _context.ExamLists.FindAsync(id);
				if (classToDelete == null)
				{
					responseModel.isSuccess = false;
					responseModel.Message = "Class not found.";
					return responseModel;
				}

				_context.ExamLists.Remove(classToDelete);
				await _context.SaveChangesAsync();

				responseModel.isSuccess = true;
				responseModel.Message = "Class deleted successfully.";
			}
			catch (Exception ex)
			{
				responseModel.isSuccess = false;
				responseModel.Message = $"Error: {ex.Message}";
			}

			return responseModel;
		}

	}
}
