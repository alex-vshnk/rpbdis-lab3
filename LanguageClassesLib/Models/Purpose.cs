using System;
using System.Collections.Generic;

namespace LanguageClasses.Models
{
    public partial class Purpose
    {
        public Purpose()
        {
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public string? PurposeName { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
