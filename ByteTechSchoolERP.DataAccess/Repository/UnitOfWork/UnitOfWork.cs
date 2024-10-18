using ByteTechSchoolERP.DataAccess.Data;

using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IClass;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IHostel;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IInstituite;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.ILesson;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.ISection;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IStudent;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IStudentPromotion;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.ISubject;
using ByteTechSchoolERP.DataAccess.Repository.IRepository.UnitOfWork;
using ByteTechSchoolERP.DataAccess.Repository.Services.ClassService;
using ByteTechSchoolERP.DataAccess.Repository.Services.SectionService;
using ByteTechSchoolERP.DataAccess.Repository.Services.HostelService;
using ByteTechSchoolERP.DataAccess.Repository.Services.InstituteService;
using ByteTechSchoolERP.DataAccess.Repository.Services.LessonService;
using ByteTechSchoolERP.DataAccess.Repository.Services.StudentPromotionService;
using ByteTechSchoolERP.DataAccess.Repository.Services.StudentService;
using ByteTechSchoolERP.DataAccess.Repository.Services.SubjectService;
using ByteTechSchoolERP.Models.ClassAndSection;
using ByteTechSchoolERP.Models.Hostel;
using ByteTechSchoolERP.Models.Students;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.ITopic;
using ByteTechSchoolERP.DataAccess.Repository.Services.TopicService;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IExam;
using ByteTechSchoolERP.DataAccess.Repository.Services.ExamService;
using ByteTechSchoolERP.DataAccess.Repository.Interfaces.IClasstimetable;


namespace ByteTechSchoolERP.DataAccess.Repository.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		public ISubjectAdd Subject { get; }
		public IClass Class { get; }
		public ISection Section { get; }
		public ILesson Lessons { get; }
		public ITopic Topic { get; }
		public ITerm Term { get; }
		public IPromoteStudent Student { get; }
		public IStudentAttendance StudentAttendance { get; }
		public IStudentAdmit StudentAdmit { get; }
		public IInstituteProfile Institute { get; }
		public IHostelRoomTypeService HostelRoomTypeService { get; }
		public IExam Exam { get; }
		public IClasstimetable Classtimetable { get; }
		public IGrade Grade { get; }

		public UnitOfWork(ByteTechSchoolERPContext db, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostingEnvironment)
		{
			Subject = new SubjectRepo(db, httpContextAccessor);
			Class = new ClassRepo(db, httpContextAccessor);
			Section = new SectionRepo(db, httpContextAccessor);
			Lessons = new Lessonrepo(db, httpContextAccessor);
			Topic = new TopicRepo(db, httpContextAccessor);
			Student = new StudentPromotionRepo(db);
			StudentAttendance = new StudentAttendanceRepo(db);
			StudentAdmit = new StudentAdmitRapo(db, hostingEnvironment, httpContextAccessor);
			Institute = new IntituiteRepo(db, hostingEnvironment);
			HostelRoomTypeService = new HostelRoomTypeService(db, httpContextAccessor);
			Term = new TermRepo(db, httpContextAccessor);
			// Exam = new ExamRepo(db, httpContextAccessor);
			Classtimetable = new ClassTimetableRepo(db, httpContextAccessor);
			Grade = new GradeRepo(db, httpContextAccessor);
			Exam = new ExamRepo(db, httpContextAccessor);// Corrected here
		}


	}
}
