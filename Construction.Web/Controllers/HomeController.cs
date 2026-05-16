using Construction.Business.Abstract;
using Construction.Entity.Entities;
using Construction.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Construction.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProjectService _projectService;
        private readonly ICategoryService _categoryService;
        private readonly ITestimonialService _testimonialService;
        private readonly IAIRecommendationService _aiService;

        public HomeController(
            ILogger<HomeController> logger,
            IProjectService projectService,
            ICategoryService categoryService,
            ITestimonialService testimonialService,
            IAIRecommendationService aiService)
        {
            _logger = logger;
            _projectService = projectService;
            _categoryService = categoryService;
            _testimonialService = testimonialService;
            _aiService = aiService;
        }

        // GET: /  →  Ana Sayfa
        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                FeaturedProjects = (await _projectService.GetProjectsWithCategory())
                                        .Where(p => p.IsActive)
                                        .OrderByDescending(p => p.CreatedDate)
                                        .Take(6)
                                        .ToList(),
                Categories = (await _categoryService.TGetListAsync())
                                        .Where(c => c.IsActive)
                                        .ToList(),
                Testimonials = (await _testimonialService.GetTestimonialsWithProjectAsync())
                                        .Where(t => t.IsActive)
                                        .OrderByDescending(t => t.Rating)
                                        .Take(6)
                                        .ToList()
            };
            return View(model);
        }

        // GET: /Home/Projects?categoryId=&search=&page=
        public async Task<IActionResult> Projects(int? categoryId, string? search, int page = 1)
        {
            const int pageSize = 9;

            var all = (await _projectService.GetProjectsWithCategory())
                          .Where(p => p.IsActive)
                          .ToList();

            if (categoryId.HasValue)
                all = all.Where(p => p.CategoryId == categoryId.Value).ToList();

            if (!string.IsNullOrWhiteSpace(search))
                all = all.Where(p =>
                    p.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    (p.Description ?? "").Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    (p.Location ?? "").Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            var model = new ProjectsViewModel
            {
                Projects = all.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                Categories = (await _categoryService.TGetListAsync()).Where(c => c.IsActive).ToList(),
                SelectedCategory = categoryId,
                SearchQuery = search,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(all.Count / (double)pageSize),
                TotalCount = all.Count
            };

            return View(model);
        }

        // GET: /Home/ProjectDetail/5
        public async Task<IActionResult> ProjectDetail(int id)
        {
            var project = await _projectService.TGetByIdAsync(id);
            if (project == null || !project.IsActive)
                return RedirectToAction("Projects");

            // İlişkili projeye ait yorumlar
            var testimonials = (await _testimonialService.GetTestimonialsWithProjectAsync())
                                   .Where(t => t.ProjectId == id && t.IsActive)
                                   .ToList();

            // AI ile benzer projeler (embedding varsa)
            List<Project> similarProjects = new();
            if (!string.IsNullOrEmpty(project.DetailsEmbedding))
            {
                try
                {
                    var recs = await _aiService.GetRecommendationsAsync(
                        $"{project.Title} {project.Description} {project.Location}", topN: 4);
                    similarProjects = recs
                        .Where(r => r.Project.Id != id)
                        .Select(r => r.Project)
                        .Take(3)
                        .ToList();
                }
                catch { /* AI servis yoksa sessizce atla */ }
            }

            var model = new ProjectDetailViewModel
            {
                Project = project,
                Testimonials = testimonials,
                SimilarProjects = similarProjects
            };

            return View(model);
        }

        // GET: /Home/Contact
        public IActionResult Contact()
        {
            return View();
        }

        // POST: /Home/Contact  (AJAX)
        [HttpPost]
        public IActionResult SendContact([FromBody] ContactFormModel form)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Lütfen tüm alanları doldurun." });

            // TODO: E-posta veya DB kaydı buraya gelecek
            _logger.LogInformation("İletişim formu: {Name} / {Email}", form.Name, form.Email);

            return Ok(new { success = true, message = "Mesajınız alındı. En kısa sürede dönüş yapacağız." });
        }

        // POST: /Home/AISearch  (AJAX — frontend arama kutusu)
        [HttpPost]
        public async Task<IActionResult> AISearch([FromBody] AISearchRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Query))
                return BadRequest(new { error = "Sorgu boş olamaz." });

            try
            {
                var results = await _aiService.GetRecommendationsAsync(request.Query, 6);
                var mapped = results.Select(r => new
                {
                    id = r.Project.Id,
                    title = r.Project.Title,
                    location = r.Project.Location,
                    category = r.Project.Category?.CategoryName,
                    minPrice = r.Project.MinPrice,
                    coverImageUrl = r.Project.CoverImageUrl,
                    projectStatus = r.Project.ProjectStatus,
                    score = Math.Round(r.SimilarityScore * 100, 1),
                    matchReason = r.MatchReason
                });
                return Ok(new { success = true, results = mapped });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    // ─── Request/Form modelleri ───────────────────────────────────────────────
    public class AISearchRequest
    {
        public string Query { get; set; }
    }

    public class ContactFormModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
