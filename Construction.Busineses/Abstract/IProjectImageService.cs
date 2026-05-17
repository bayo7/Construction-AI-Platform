using Construction.Entity.Entities;

namespace Construction.Business.Abstract
{
    public interface IProjectImageService : IGenericService<ProjectImage>
    {
        Task<List<ProjectImage>> GetByProjectIdAsync(int projectId);
        Task DeleteByProjectIdAsync(int projectId);
    }
}
