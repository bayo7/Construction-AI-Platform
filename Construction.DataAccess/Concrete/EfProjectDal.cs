using Construction.DataAccess.Abstract;
using Construction.DataAccess.Context;
using Construction.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.DataAccess.Concrete
{
    public class EFProjectDal : GenericRepository<Project>, IProjectDal
    {
        private readonly ConstructionDbContext _context;
        public EFProjectDal(ConstructionDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetProjectsWithCategory()
        {
            return await _context.Projects.Include(p => p.Category).ToListAsync();
        }
    }
}
