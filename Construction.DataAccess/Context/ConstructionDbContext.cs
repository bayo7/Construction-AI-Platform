using Microsoft.EntityFrameworkCore;
using Construction.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.DataAccess.Context
{
    public class ConstructionDbContext : IdentityDbContext<AppUser>
    {
        public ConstructionDbContext(DbContextOptions<ConstructionDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
    }
}
