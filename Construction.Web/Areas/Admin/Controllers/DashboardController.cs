using Construction.Business.Abstract;
using Construction.Entity.Entities;
using Construction.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
    public class DashboardController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ICategoryService _categoryService;
        private readonly ITestimonialService _testimonialService;
        private readonly UserManager<AppUser> _userManager;

        public DashboardController(
            IProjectService projectService,
            ICategoryService categoryService,
            ITestimonialService testimonialService,
            UserManager<AppUser> userManager)
        {
            _projectService = projectService;
            _categoryService = categoryService;
            _testimonialService = testimonialService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetProjectsWithCategory();
            var categories = await _categoryService.TGetListAsync();
            var testimonials = await _testimonialService.TGetListAsync();
            var users = _userManager.Users.ToList();

            var vm = new DashboardViewModel
            {
                // ── Özet Sayaçlar ──────────────────────────────────────
                TotalProjects = projects.Count,
                ActiveProjects = projects.Count(p => p.IsActive),
                TotalCategories = categories.Count,
                TotalTestimonials = testimonials.Count,
                TotalUsers = users.Count,
                TotalPortfolioValue = projects.Where(p => p.IsActive).Sum(p => p.MinPrice),

                // ── Kategoriye Göre Proje (Doughnut) ───────────────────
                ProjectsByCategory = projects
                    .Where(p => p.Category != null)
                    .GroupBy(p => p.Category!.CategoryName)
                    .Select(g => new ChartItem { Label = g.Key!, Value = g.Count() })
                    .OrderByDescending(x => x.Value)
                    .ToList(),

                // ── Duruma Göre Proje (Bar) ─────────────────────────────
                ProjectsByStatus = projects
                    .GroupBy(p => p.ProjectStatus ?? "Belirtilmemiş")
                    .Select(g => new ChartItem { Label = g.Key, Value = g.Count() })
                    .OrderByDescending(x => x.Value)
                    .ToList(),

                // ── Son 6 Ay (Line) ─────────────────────────────────────
                ProjectsMonthly = Enumerable.Range(0, 6)
                    .Select(i =>
                    {
                        var date = DateTime.Now.AddMonths(-5 + i);
                        return new ChartItem
                        {
                            Label = date.ToString("MMM yy"),
                            Value = projects.Count(p =>
                                p.CreatedDate.Year == date.Year &&
                                p.CreatedDate.Month == date.Month)
                        };
                    })
                    .ToList(),

                // ── Tamamlanma Dağılımı (Horizontal Bar) ───────────────
                CompletionDistribution = new List<ChartItem>
                {
                    new() { Label = "0–25%",   Value = projects.Count(p => p.CompleetionRate <= 25) },
                    new() { Label = "26–50%",  Value = projects.Count(p => p.CompleetionRate is > 25 and <= 50) },
                    new() { Label = "51–75%",  Value = projects.Count(p => p.CompleetionRate is > 50 and <= 75) },
                    new() { Label = "76–99%",  Value = projects.Count(p => p.CompleetionRate is > 75 and < 100) },
                    new() { Label = "100%",    Value = projects.Count(p => p.CompleetionRate == 100) },
                },

                // ── Son 5 Proje ─────────────────────────────────────────
                RecentProjects = projects
                    .OrderByDescending(p => p.CreatedDate)
                    .Take(5)
                    .ToList(),

                // ── Son 5 Yorum ─────────────────────────────────────────
                RecentTestimonials = testimonials
                    .OrderByDescending(t => t.CreatedDate)
                    .Take(5)
                    .ToList(),

                // ── Embedding Durumu ────────────────────────────────────
                EmbeddedProjectCount = projects.Count(p => !string.IsNullOrEmpty(p.DetailsEmbedding)),
                NotEmbeddedProjectCount = projects.Count(p => string.IsNullOrEmpty(p.DetailsEmbedding)),
            };

            return View(vm);
        }
    }
}
