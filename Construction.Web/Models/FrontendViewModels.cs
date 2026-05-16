using Construction.Entity.Entities;

namespace Construction.Web.Models
{
    public class HomeViewModel
    {
        public List<Project> FeaturedProjects { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<Testimonial> Testimonials { get; set; } = new();
    }

    public class ProjectsViewModel
    {
        public List<Project> Projects { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public int? SelectedCategory { get; set; }
        public string? SearchQuery { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }

    public class ProjectDetailViewModel
    {
        public Project Project { get; set; }
        public List<Testimonial> Testimonials { get; set; } = new();
        public List<Project> SimilarProjects { get; set; } = new();
    }
}
