using Microsoft.EntityFrameworkCore;
using Construction.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Construction.DataAccess.Context
{
    public class ConstructionDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public ConstructionDbContext(DbContextOptions<ConstructionDbContext> options)
            : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }   // ← YENİ

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ProjectImage → Project  (cascade delete: proje silinince görseller de silinir)
            builder.Entity<ProjectImage>()
                .HasOne(pi => pi.Project)
                .WithMany(p => p.ProjectImages)
                .HasForeignKey(pi => pi.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
