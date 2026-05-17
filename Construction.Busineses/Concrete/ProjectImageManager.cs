using Construction.Business.Abstract;
using Construction.DataAccess.Abstract;
using Construction.Entity.Entities;

namespace Construction.Business.Concrete
{
    public class ProjectImageManager : IProjectImageService
    {
        private readonly IProjectImageDal _projectImageDal;

        public ProjectImageManager(IProjectImageDal projectImageDal)
        {
            _projectImageDal = projectImageDal;
        }

        public async Task TInsertAsync(ProjectImage entity) => await _projectImageDal.InsertAsync(entity);
        public async Task TDeleteAsync(ProjectImage entity) => await _projectImageDal.DeleteAsync(entity);
        public async Task TUpdateAsync(ProjectImage entity) => await _projectImageDal.UpdateAsync(entity);
        public async Task<ProjectImage> TGetByIdAsync(int id) => await _projectImageDal.GetByIdAsync(id);
        public async Task<List<ProjectImage>> TGetListAsync() => await _projectImageDal.GetListAsync();

        public async Task<List<ProjectImage>> GetByProjectIdAsync(int projectId)
            => await _projectImageDal.GetByProjectIdAsync(projectId);

        public async Task DeleteByProjectIdAsync(int projectId)
            => await _projectImageDal.DeleteByProjectIdAsync(projectId);
    }
}
