using System;
using System.Collections.Generic;

namespace LanguageClasses.Models
{
    public partial class EmployeesCourse
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public int? EmployeeId { get; set; }

        public virtual Course? Course { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
