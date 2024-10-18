using ByteTechSchoolERP.DataAccess.Data;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IClass;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.ISection;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IHostel;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IInstituite;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.ILesson;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IStudent;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IStudentPromotion;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.ISubject;
using ByteTechSchoolERP.DataAccess.Repository.Services.SubjectService;
using ByteTechSchoolERP.Models.Students;
using Microsoft.AspNetCore.Http;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.ITopic;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IExam;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IClasstimetable;

namespace ByteTechSchoolERP.DataAccess.Repository.IRepository.UnitOfWork
{
	public interface IUnitOfWork
	{
		public ISubjectAdd Subject { get; }
		public IExam Exam { get; }
		public IGrade Grade { get; }
		public ITerm Term { get; }
		public IClasstimetable Classtimetable { get; }
		public ILesson Lessons { get; }
		public IClass Class { get; }
		public ISection Section { get; }
		public IPromoteStudent Student { get; }
		public IStudentAttendance StudentAttendance { get; }
		public IStudentAdmit StudentAdmit { get; }
		public IInstituteProfile Institute { get; }
		public IHostelRoomTypeService HostelRoomTypeService { get; }
		public ITopic Topic { get; }


	}
}
