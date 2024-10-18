using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteTechSchoolERP.Models.Topic
{
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public Guid SectionId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid LessonId { get; set; }
        public string TopicName { get; set; }
    }
}
