using System;
using System.Collections.Generic;

namespace LanguageClasses.Models
{
    public partial class Employee
    {
        public Employee()
        {
            EmployeesCourses = new HashSet<EmployeesCourse>();
        }

        public int Id { get; set; }
        public string? Surname { get; set; }
        public string? FirstName { get; set; }
        public string? Patronymic { get; set; }
        public string? Education { get; set; }
        public int? PositionId { get; set; }

        public virtual Position? Position { get; set; }
        public virtual ICollection<EmployeesCourse> EmployeesCourses { get; set; }
    }
}
