using ByteTechSchoolERP.Models.Subjects;
using ByteTechSchoolERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByteTechSchoolERP.Models.Lesson;
using ByteTechSchoolERP.Models.ViewModels.LoginVM;

namespace ByteTechSchoolERP.DataAccess.Repository.Interfaces.ILesson
{
    public interface ILesson
    {
         Task<ResponseModel> AddOrUpdateLessons(Lesson  lesson);
         List<LessonListViewModel> GetLessonList(LessonListViewModel  lessonListViewModel);
        Task<ResponseModel> DeleteLessonById(Guid id);
    }
}
