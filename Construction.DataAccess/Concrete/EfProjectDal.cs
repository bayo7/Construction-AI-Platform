using Construction.DataAccess.Abstract;
using Construction.DataAccess.Context;
using Construction.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.DataAccess.Concrete
{
    public class EFProjectDal : GenericRepository<Project>, IProjectDal
    {
        public EFProjectDal(ConstructionDbContext context) : base(context)
        {
            
        }
    }
}
