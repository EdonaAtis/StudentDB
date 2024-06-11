using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.DataModels.Models
{
    public class StudentCourse
    {
        public int StudentId { get; set; }
        public StudentInfo Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
