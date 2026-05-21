using Construction.DataAccess.Abstract;
using Construction.DataAccess.Context;
using Construction.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Construction.DataAccess.Concrete
{
    public class EFProjectDal : GenericDal<Project>, IProjectDal
    {
        private readonly ConstructionDbContext _context;

        public EFProjectDal(ConstructionDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetProjectsWithCategory()
        {
            return await _context.Projects
                .Include(p => p.Category)
                .Include(p => p.ProjectImages)   // ← Galeri görselleri
                .ToListAsync();
        }
    }
}
