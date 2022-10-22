using System;
using System.Collections.Generic;

namespace LanguageClasses.Models
{
    public partial class Listener
    {
        public Listener()
        {
            ListenersCourses = new HashSet<ListenersCourse>();
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public string? Surname { get; set; }
        public string? FirstName { get; set; }
        public string? Patronymic { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? PassportNumber { get; set; }

        public virtual ICollection<ListenersCourse> ListenersCourses { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
