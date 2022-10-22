using System;
using System.Collections.Generic;
using LanguageClasses.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace LanguageClasses.Data
{
    public partial class LanguageClassesContext : DbContext
    {
        public LanguageClassesContext()
        {
        }

        public LanguageClassesContext(DbContextOptions<LanguageClassesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<EmployeesCourse> EmployeesCourses { get; set; } = null!;
        public virtual DbSet<Listener> Listeners { get; set; } = null!;
        public virtual DbSet<ListenersCourse> ListenersCourses { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Position> Positions { get; set; } = null!;
        public virtual DbSet<Purpose> Purposes { get; set; } = null!;
    }
}
