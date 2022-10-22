using System;
using System.Collections.Generic;

namespace LanguageClasses.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public double? Amount { get; set; }
        public int? ListenerId { get; set; }
        public int? PurposeId { get; set; }

        public virtual Listener? Listener { get; set; }
        public virtual Purpose? Purpose { get; set; }
    }
}
