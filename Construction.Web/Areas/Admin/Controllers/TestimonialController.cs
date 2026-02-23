using Construction.Business.Abstract;
using Construction.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Construction.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        // 1. Yorum Silme İşlemi
        public async Task<IActionResult> Delete(int id)
        {
            // Silinecek yorumu buluyoruz
            var value = await _testimonialService.TGetByIdAsync(id);

            if (value != null)
            {
                await _testimonialService.TDeleteAsync(value);
            }

            // İşlem bitince listeye geri dönüyoruz
            return RedirectToAction("Index", "Testimonial", new { area = "Admin" });
        }

        // 2. Durum Değiştirme (Onayla / Yayından Kaldır) İşlemi
        public async Task<IActionResult> ChangeStatus(int id)
        {
            // Durumu değişecek yorumu buluyoruz
            var value = await _testimonialService.TGetByIdAsync(id);

            if (value != null)
            {
                // Mevcut durumun tam tersini atıyoruz (! operatörü ile)
                value.IsActive = !value.IsActive;

                // Güncelleme işlemini yapıyoruz
                await _testimonialService.TUpdateAsync(value);
            }

            // İşlem bitince listeye geri dönüyoruz
            return RedirectToAction("Index", "Testimonial", new { area = "Admin" });
        }
    }
}
