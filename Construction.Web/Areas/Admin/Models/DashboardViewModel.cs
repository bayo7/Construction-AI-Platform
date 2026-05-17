using Construction.Entity.Entities;

namespace Construction.Web.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        // ── Özet Sayaçlar ──────────────────────────────────────────────
        public int TotalProjects { get; set; }
        public int ActiveProjects { get; set; }
        public int TotalCategories { get; set; }
        public int TotalTestimonials { get; set; }
        public int TotalUsers { get; set; }
        public decimal TotalPortfolioValue { get; set; }

        // ── Grafik Verileri ────────────────────────────────────────────
        // Kategoriye göre proje sayısı  →  Doughnut
        public List<ChartItem> ProjectsByCategory { get; set; } = new();

        // Proje durumuna göre dağılım  →  Bar
        public List<ChartItem> ProjectsByStatus { get; set; } = new();

        // Son 6 aya göre eklenen proje sayısı  →  Line
        public List<ChartItem> ProjectsMonthly { get; set; } = new();

        // Tamamlanma oranı dağılımı  →  Bar yatay
        public List<ChartItem> CompletionDistribution { get; set; } = new();

        // ── Son Eklenenler ─────────────────────────────────────────────
        public List<Project> RecentProjects { get; set; } = new();
        public List<Testimonial> RecentTestimonials { get; set; } = new();

        // ── Embedding Durumu ───────────────────────────────────────────
        public int EmbeddedProjectCount { get; set; }
        public int NotEmbeddedProjectCount { get; set; }
    }

    public class ChartItem
    {
        public string Label { get; set; }
        public int Value { get; set; }
    }
}
