using Construction.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.DataAccess.Abstract
{
    public interface IProjectDal : IGenericDal<Project>
    {
        Task<List<Project>> GetProjectsWithCategory();
    }
}
