using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.DataModels.Models
{
	public class StudentInfo
	{

		[Key]
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public string PhoneNumber { get; set; }
		public string FieldOfStudy { get; set; }

        public ICollection<StudentCourse> StudentCourses { get; set; }

    }
}
