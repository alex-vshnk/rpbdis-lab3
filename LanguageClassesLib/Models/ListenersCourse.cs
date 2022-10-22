using System;
using System.Collections.Generic;

namespace LanguageClasses.Models
{
    public partial class ListenersCourse
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public int? ListenerId { get; set; }

        public virtual Course? Course { get; set; }
        public virtual Listener? Listener { get; set; }
    }
}
