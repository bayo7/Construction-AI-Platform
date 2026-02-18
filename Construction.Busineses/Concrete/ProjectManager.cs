using Construction.Business.Abstract;
using Construction.DataAccess.Abstract;
using Construction.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Business.Concrete
{
    public class ProjectManager : IProjectService
    {
        private readonly IProjectDal _projectDal;

        public ProjectManager(IProjectDal projectDal)
        {
            _projectDal = projectDal;
        }

        public async Task TDeleteAsync(Project entity)
        {
            await _projectDal.DeleteAsync(entity);
        }

        public async Task<List<Project>> TGetListAsync()
        {
            return await _projectDal.GetListAsync();
        }

        public async Task<Project> TGetByIdAsync(int id)
        {
            return await _projectDal.GetByIdAsync(id);
        }

        public async Task TInsertAsync(Project entity)
        {
            // Burası gelecekte yapılacak AI kısmı için ayrıldı. Şimdilik direkt olarak veritabanına ekleme işlemi yapılıyor.

            await _projectDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(Project entity)
        {
            await _projectDal.UpdateAsync(entity);
        }

        public async Task<List<Project>> GetProjectsWithCategory()
        {
            return await _projectDal.GetProjectsWithCategory();
        }
    }
}
