using Construction.Business.Abstract;
using Construction.DataAccess.Abstract;
using Construction.Entity.Entities;

namespace Construction.Business.Concrete
{
    public class ProjectManager : IProjectService
    {
        private readonly IProjectDal _projectDal;
        private readonly IAIRecommendationService _aiService;

        public ProjectManager(IProjectDal projectDal, IAIRecommendationService aiService)
        {
            _projectDal = projectDal;
            _aiService = aiService;
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

        /// <summary>
        /// Proje ekleme: kaydedildikten sonra otomatik olarak AI embedding oluşturulur.
        /// </summary>
        public async Task TInsertAsync(Project entity)
        {
            // 1) Önce projeyi kaydet (Id alabilmek için)
            await _projectDal.InsertAsync(entity);

            // 2) Embedding oluştur ve DetailsEmbedding alanını güncelle
            try
            {
                await _aiService.GenerateAndSaveEmbeddingAsync(entity);
                await _projectDal.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                // Embedding başarısız olursa proje yine de kaydedildi, sadece log bırak.
                // İleride Serilog/NLog entegre edildiğinde buraya logger eklenebilir.
                Console.WriteLine($"[AI Embedding] Hata oluştu, proje kaydedildi ama embedding boş kaldı: {ex.Message}");
            }
        }

        /// <summary>
        /// Proje güncelleme: içerik değiştiğinde embedding de yenilenir.
        /// </summary>
        public async Task TUpdateAsync(Project entity)
        {
            // Embedding'i yenile
            try
            {
                await _aiService.GenerateAndSaveEmbeddingAsync(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AI Embedding] Güncelleme sırasında hata: {ex.Message}");
            }

            await _projectDal.UpdateAsync(entity);
        }

        public async Task<List<Project>> GetProjectsWithCategory()
        {
            return await _projectDal.GetProjectsWithCategory();
        }
    }
}
