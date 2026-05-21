using Construction.Business.Abstract;
using Construction.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Construction.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]   // ← Editor de erişebilir
    public class TestimonialController : Controller
    {
        private readonly ITestimonialService _testimonialService;
        private readonly IProjectService _projectService;

        public TestimonialController(ITestimonialService testimonialService, IProjectService projectService)
        {
            _testimonialService = testimonialService;
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _testimonialService.GetTestimonialsWithProjectAsync();
            return View(values);
        }

        // Silme — SADECE Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var value = await _testimonialService.TGetByIdAsync(id);
            if (value != null)
                await _testimonialService.TDeleteAsync(value);

            return RedirectToAction("Index", "Testimonial", new { area = "Admin" });
        }

        // Durum değiştirme — Admin + Editor
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var value = await _testimonialService.TGetByIdAsync(id);
            if (value != null)
            {
                value.IsActive = !value.IsActive;
                await _testimonialService.TUpdateAsync(value);
            }
            return RedirectToAction("Index", "Testimonial", new { area = "Admin" });
        }
    }
}
