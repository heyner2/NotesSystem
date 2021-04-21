using Microsoft.EntityFrameworkCore;
using NotesSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Student { get; set; }

        public DbSet<Professor> professor { get; set; }
        public DbSet<Grade> Grade { get; set; }

        public DbSet<Notes> Notes { get; set; }

    }
}
