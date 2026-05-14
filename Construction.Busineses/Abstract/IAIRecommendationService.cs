using Construction.Entity.Entities;

namespace Construction.Business.Abstract
{
    public interface IAIRecommendationService
    {
        /// <summary>
        /// Verilen metin için Anthropic API üzerinden embedding vektörü üretir.
        /// </summary>
        Task<float[]> GetEmbeddingAsync(string text);

        /// <summary>
        /// Proje ekleme/güncellemede çağrılır; projenin DetailsEmbedding alanını doldurur.
        /// </summary>
        Task GenerateAndSaveEmbeddingAsync(Project project);

        /// <summary>
        /// Kullanıcının doğal dil sorgusuna (örn: "doğayla iç içe") en uygun projeleri döner.
        /// </summary>
        Task<List<ProjectRecommendationResult>> GetRecommendationsAsync(string userQuery, int topN = 5);
    }

    public class ProjectRecommendationResult
    {
        public Project Project { get; set; }
        public float SimilarityScore { get; set; }        // 0.0 - 1.0 arası cosine benzerliği
        public string MatchReason { get; set; }           // "Doğa manzarası, yeşil alan..." gibi
    }
}
