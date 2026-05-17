using Construction.DataAccess.Abstract;
using Construction.DataAccess.Context;
using Construction.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace Construction.DataAccess.Concrete
{
    public class EfProjectImageDal : GenericDal<ProjectImage>, IProjectImageDal
    {
        private readonly ConstructionDbContext _context;

        public EfProjectImageDal(ConstructionDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ProjectImage>> GetByProjectIdAsync(int projectId)
        {
            return await _context.ProjectImages
                .Where(pi => pi.ProjectId == projectId)
                .OrderBy(pi => pi.SortOrder)
                .ToListAsync();
        }

        public async Task DeleteByProjectIdAsync(int projectId)
        {
            var images = await _context.ProjectImages
                .Where(pi => pi.ProjectId == projectId)
                .ToListAsync();

            _context.ProjectImages.RemoveRange(images);
            await _context.SaveChangesAsync();
        }
    }
}
