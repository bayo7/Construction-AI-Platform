using Construction.Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AIController : Controller
    {
        private readonly IAIRecommendationService _aiService;
        private readonly IProjectService _projectService;

        public AIController(IAIRecommendationService aiService, IProjectService projectService)
        {
            _aiService = aiService;
            _projectService = projectService;
        }

        // GET: /Admin/AI/Index
        // AI arama test paneli
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Admin/AI/Search  (AJAX)
        [HttpPost]
        public async Task<IActionResult> Search([FromBody] AISearchRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Query))
                return BadRequest(new { error = "Arama sorgusu boş olamaz." });

            try
            {
                var results = await _aiService.GetRecommendationsAsync(request.Query, request.TopN);

                var mapped = results.Select(r => new
                {
                    id = r.Project.Id,
                    title = r.Project.Title,
                    description = r.Project.Description,
                    location = r.Project.Location,
                    category = r.Project.Category?.CategoryName,
                    minPrice = r.Project.MinPrice,
                    coverImageUrl = r.Project.CoverImageUrl,
                    projectStatus = r.Project.ProjectStatus,
                    completionRate = r.Project.CompleetionRate,
                    similarityScore = Math.Round(r.SimilarityScore * 100, 1), // % olarak
                    matchReason = r.MatchReason
                });

                return Ok(new { success = true, results = mapped });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"AI servisi hatası: {ex.Message}" });
            }
        }

        // POST: /Admin/AI/RegenerateAll  (toplu embedding yenileme)
        [HttpPost]
        public async Task<IActionResult> RegenerateAll()
        {
            try
            {
                var projects = await _projectService.GetProjectsWithCategory();
                int success = 0, failed = 0;

                foreach (var project in projects)
                {
                    try
                    {
                        await _aiService.GenerateAndSaveEmbeddingAsync(project);
                        await _projectService.TUpdateAsync(project);
                        success++;
                    }
                    catch
                    {
                        failed++;
                    }
                }

                return Ok(new
                {
                    success = true,
                    message = $"{success} proje başarıyla güncellendi, {failed} proje atlandı."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class AISearchRequest
    {
        public string Query { get; set; }
        public int TopN { get; set; } = 5;
    }
}
