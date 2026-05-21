using Construction.Business.Abstract;
using Construction.DataAccess.Abstract;
using Construction.Entity.Entities;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace Construction.Business.Concrete
{
    public class AIRecommendationManager : IAIRecommendationService
    {
        private readonly IProjectDal _projectDal;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        // Gemini Embedding Modeli URL'si
        private const string GeminiEmbeddingModel = "text-embedding-004";

        public AIRecommendationManager(IProjectDal projectDal, IConfiguration configuration)
        {
            _projectDal = projectDal;
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Gemini API ile metin → float[] vektör üretir.
        /// API key tanımlı değilse veya istek başarısız olursa null döner (uygulama çökmez).
        /// </summary>
        public async Task<float[]?> GetEmbeddingAsync(string text)
        {
            var apiKey = _configuration["GeminiAI:ApiKey"];

            // Key tanımlı değil veya şablon değer içeriyor — sessizce atla
            if (string.IsNullOrWhiteSpace(apiKey) ||
                apiKey.StartsWith("BURAYA_", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            try
            {
                // Gemini API URL'si (Query string olarak API key alıyor)
                string requestUrl = $"https://generativelanguage.googleapis.com/v1beta/models/{GeminiEmbeddingModel}:embedContent?key={apiKey}";

                // Gemini'nin beklediği JSON formatı
                var requestBody = new
                {
                    model = $"models/{GeminiEmbeddingModel}",
                    content = new
                    {
                        parts = new[]
                        {
                            new { text = text }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(requestUrl, content);

                if (!response.IsSuccessStatusCode)
                    return null;  // API hatası — sessizce atla

                var responseJson = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseJson);

                // Gemini Yanıt Formatı: { "embedding": { "values": [0.1, 0.2, ...] } }
                var embedding = doc.RootElement
                    .GetProperty("embedding")
                    .GetProperty("values")
                    .EnumerateArray()
                    .Select(e => e.GetSingle())
                    .ToArray();

                return embedding;
            }
            catch
            {
                return null;  // Beklenmedik hata — sessizce atla
            }
        }

        /// <summary>
        /// Proje kaydedilirken/güncellenirken çağrılır.
        /// API key yoksa veya hata oluşursa embedding sessizce atlanır, proje kaydı engellenmez.
        /// </summary>
        public async Task GenerateAndSaveEmbeddingAsync(Project project)
        {
            var textToEmbed = BuildProjectText(project);
            var embedding = await GetEmbeddingAsync(textToEmbed);
            if (embedding != null)
                project.DetailsEmbedding = JsonSerializer.Serialize(embedding);
            // embedding null ise DetailsEmbedding alanı mevcut değerini korur
        }

        /// <summary>
        /// Kullanıcının doğal dil sorgusunu vektöre çevirip eşleştirir.
        /// </summary>
        public async Task<List<ProjectRecommendationResult>> GetRecommendationsAsync(string userQuery, int topN = 5)
        {
            var queryEmbedding = await GetEmbeddingAsync(userQuery);

            // API key yoksa veya embedding alınamadıysa boş liste döndür
            if (queryEmbedding == null)
                return new List<ProjectRecommendationResult>();

            var allProjects = await _projectDal.GetProjectsWithCategory();
            var results = new List<ProjectRecommendationResult>();

            foreach (var project in allProjects)
            {
                if (string.IsNullOrEmpty(project.DetailsEmbedding))
                    continue;

                float[] projectEmbedding;
                try
                {
                    projectEmbedding = JsonSerializer.Deserialize<float[]>(project.DetailsEmbedding)!;
                }
                catch
                {
                    continue;
                }

                var score = CosineSimilarity(queryEmbedding, projectEmbedding);

                results.Add(new ProjectRecommendationResult
                {
                    Project = project,
                    SimilarityScore = score,
                    MatchReason = BuildMatchReason(project, score)
                });
            }

            return results
                .OrderByDescending(r => r.SimilarityScore)
                .Take(topN)
                .ToList();
        }

        // ─── Yardımcı Metodlar (Claude'un yazdığı kısımlar, aynı kaldı) ───

        private static string BuildProjectText(Project project)
        {
            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(project.Title)) parts.Add($"Proje adı: {project.Title}");
            if (!string.IsNullOrWhiteSpace(project.Description)) parts.Add($"Açıklama: {project.Description}");
            if (!string.IsNullOrWhiteSpace(project.Location)) parts.Add($"Konum: {project.Location}");
            if (!string.IsNullOrWhiteSpace(project.ProjectStatus)) parts.Add($"Durum: {project.ProjectStatus}");
            if (project.Category != null && !string.IsNullOrWhiteSpace(project.Category.CategoryName))
                parts.Add($"Kategori: {project.Category.CategoryName}");
            if (project.MinPrice > 0) parts.Add($"Başlangıç fiyatı: {project.MinPrice:C0}");

            return string.Join(". ", parts);
        }

        private static string BuildMatchReason(Project project, float score)
        {
            if (score >= 0.85f) return $"Çok yüksek eşleşme — {project.Category?.CategoryName ?? "bu proje"} profilinize tam uyan bir seçenek.";
            if (score >= 0.70f) return $"Yüksek eşleşme — {project.Location} konumundaki bu proje aradığınız özelliklere sahip.";
            if (score >= 0.55f) return $"Orta eşleşme — Bazı özellikler örtüşüyor, incelemeye değer.";
            return "Düşük eşleşme — İlginizi çekebilecek farklı bir alternatif.";
        }

        private static float CosineSimilarity(float[] a, float[] b)
        {
            if (a.Length != b.Length) throw new ArgumentException("Vektör boyutları eşit değil.");
            double dot = 0, normA = 0, normB = 0;
            for (int i = 0; i < a.Length; i++)
            {
                dot += a[i] * b[i];
                normA += a[i] * a[i];
                normB += b[i] * b[i];
            }
            if (normA == 0 || normB == 0) return 0f;
            return (float)(dot / (Math.Sqrt(normA) * Math.Sqrt(normB)));
        }
    }
}