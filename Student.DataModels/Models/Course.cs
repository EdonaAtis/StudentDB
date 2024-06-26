﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Student.DataModels.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string FieldOfStudy { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }
    }

   
}
