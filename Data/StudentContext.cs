using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.Data
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}
