using System;
using System.Collections.Generic;

namespace LanguageClasses.Models
{
    public partial class Position
    {
        //public Position()
        //{
        //    Employees = new HashSet<Employee>();
        //}

        public int Id { get; set; }
        public string? Name { get; set; }
        public override string ToString()
        {
            return (new { Код_должности = Id, Название = Name }).ToString();
        }

        //public virtual ICollection<Employee> Employees { get; set; }
    }


}
