using Construction.Entity.Entities;

namespace Construction.DataAccess.Abstract
{
    public interface IProjectImageDal : IGenericDal<ProjectImage>
    {
        Task<List<ProjectImage>> GetByProjectIdAsync(int projectId);
        Task DeleteByProjectIdAsync(int projectId);
    }
}
