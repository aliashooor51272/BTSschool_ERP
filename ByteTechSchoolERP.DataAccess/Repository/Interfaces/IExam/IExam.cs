using ByteTechSchoolERP.Models.Lesson;
using ByteTechSchoolERP.Models;
using ByteTechSchoolERP.Models.Exam;
using ByteTechSchoolERP.Models.ViewModels;

namespace ByteTechSchoolERP.DataAccess.Repository.Interfaces.IExam
{
	public interface IExam
	{
		Task<ResponseModel> AddOrUpdateExam(ExamList examList);
		List<ExamViewModel> GetExamList();
		Task<ResponseModel> DeleteExamById(Guid id);

	}
}
