using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.ViewModels
{
    public class AddTopicViewModel
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public Guid SectionId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid LessonId { get; set; }
        public string? Topic { get; set; }
    }
}
