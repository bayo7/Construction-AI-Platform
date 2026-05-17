using Construction.Business.Abstract;
using Construction.Entity.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Construction.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]   // Editor de girebilir
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ICategoryService _categoryService;
        private readonly IProjectImageService _imageService;
        private readonly IValidator<Project> _projectValidator;
        private readonly IWebHostEnvironment _env;

        public ProjectController(
            IProjectService projectService,
            ICategoryService categoryService,
            IProjectImageService imageService,
            IValidator<Project> projectValidator,
            IWebHostEnvironment env)
        {
            _projectService = projectService;
            _categoryService = categoryService;
            _imageService = imageService;
            _projectValidator = projectValidator;
            _env = env;
        }

        // ── Liste ────────────────────────────────────────────────────────────
        public async Task<IActionResult> Index()
        {
            var values = await _projectService.GetProjectsWithCategory();
            return View(values);
        }

        // ── Oluştur GET ──────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> CreateProject()
        {
            await LoadCategoriesAsync();
            return View();
        }

        // ── Oluştur POST ─────────────────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> CreateProject(Project project,
            IFormFile? imageFile, IList<IFormFile>? galleryFiles)
        {
            var result = await _projectValidator.ValidateAsync(project);
            if (!result.IsValid)
            {
                result.Errors.ForEach(e => ModelState.AddModelError(e.PropertyName, e.ErrorMessage));
                await LoadCategoriesAsync();
                return View(project);
            }

            // Kapak görseli
            project.CoverImageUrl = imageFile != null
                ? await SaveImageAsync(imageFile)
                : "no-image.jpg";

            project.CreatedDate = DateTime.Now;
            await _projectService.TInsertAsync(project);

            // Galeri görselleri
            if (galleryFiles != null && galleryFiles.Any())
            {
                int order = 0;
                foreach (var file in galleryFiles)
                {
                    var fileName = await SaveImageAsync(file);
                    await _imageService.TInsertAsync(new ProjectImage
                    {
                        ProjectId = project.Id,
                        ImageUrl = fileName,
                        SortOrder = order++,
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    });
                }
            }

            return RedirectToAction("Index");
        }

        // ── Güncelle GET ─────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> UpdateProject(int id)
        {
            var value = await _projectService.TGetByIdAsync(id);
            if (value == null) return RedirectToAction("Index");

            await LoadCategoriesAsync();
            ViewBag.GalleryImages = await _imageService.GetByProjectIdAsync(id);
            return View(value);
        }

        // ── Güncelle POST ────────────────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> UpdateProject(Project project,
            IFormFile? imageFile, IList<IFormFile>? galleryFiles)
        {
            await LoadCategoriesAsync();
            ViewBag.GalleryImages = await _imageService.GetByProjectIdAsync(project.Id);

            // Kapak görseli güncelle (yüklendiyse)
            if (imageFile != null)
            {
                // Eski kapak görselini sil
                DeleteImageFile(project.CoverImageUrl);
                project.CoverImageUrl = await SaveImageAsync(imageFile);
            }

            if (ModelState.IsValid)
            {
                await _projectService.TUpdateAsync(project);

                // Yeni galeri görselleri ekle
                if (galleryFiles != null && galleryFiles.Any())
                {
                    var existing = await _imageService.GetByProjectIdAsync(project.Id);
                    int order = existing.Count;

                    foreach (var file in galleryFiles)
                    {
                        var fileName = await SaveImageAsync(file);
                        await _imageService.TInsertAsync(new ProjectImage
                        {
                            ProjectId = project.Id,
                            ImageUrl = fileName,
                            SortOrder = order++,
                            CreatedDate = DateTime.Now,
                            IsActive = true
                        });
                    }
                }

                return RedirectToAction("Index");
            }

            return View(project);
        }

        // ── Sil ──────────────────────────────────────────────────────────────
        [Authorize(Roles = "Admin")]    // Silme sadece Admin
        public async Task<IActionResult> DeleteProject(int id)
        {
            var value = await _projectService.TGetByIdAsync(id);
            if (value == null) return RedirectToAction("Index");

            // Kapak görselini sil
            DeleteImageFile(value.CoverImageUrl);

            // Galeri görsellerini sil (dosya + DB, cascade zaten var ama dosyaları da sil)
            var galleryImages = await _imageService.GetByProjectIdAsync(id);
            foreach (var img in galleryImages)
                DeleteImageFile(img.ImageUrl);

            await _projectService.TDeleteAsync(value);
            return RedirectToAction("Index");
        }

        // ── Galeri Görseli Sil (AJAX) ─────────────────────────────────────────
        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> DeleteGalleryImage(int imageId)
        {
            var image = await _imageService.TGetByIdAsync(imageId);
            if (image == null)
                return NotFound(new { success = false, message = "Görsel bulunamadı." });

            DeleteImageFile(image.ImageUrl);
            await _imageService.TDeleteAsync(image);

            return Ok(new { success = true });
        }

        // ── Galeri Sırasını Güncelle (AJAX) ──────────────────────────────────
        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> UpdateImageOrder([FromBody] List<ImageOrderItem> items)
        {
            foreach (var item in items)
            {
                var image = await _imageService.TGetByIdAsync(item.Id);
                if (image != null)
                {
                    image.SortOrder = item.Order;
                    await _imageService.TUpdateAsync(image);
                }
            }
            return Ok(new { success = true });
        }

        // ── Yardımcılar ───────────────────────────────────────────────────────
        private async Task LoadCategoriesAsync()
        {
            var cats = await _categoryService.TGetListAsync();
            ViewBag.v = cats.Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = x.Id.ToString()
            }).ToList();
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var newName = Guid.NewGuid().ToString() + ext;
            var folder = Path.Combine(_env.WebRootPath, "images");
            Directory.CreateDirectory(folder);
            var path = Path.Combine(folder, newName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return newName;
        }

        private void DeleteImageFile(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName) || fileName == "no-image.jpg") return;
            var path = Path.Combine(_env.WebRootPath, "images", fileName);
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }
    }

    public class ImageOrderItem
    {
        public int Id { get; set; }
        public int Order { get; set; }
    }
}
