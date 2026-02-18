using Construction.Business.Abstract;
using Construction.Entity.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Construction.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ICategoryService _categoryService;
        private readonly IValidator<Project> _projectValidator;

        public ProjectController(IProjectService projectService, ICategoryService categoryService, IValidator<Project> projectValidator)
        {
            _projectService = projectService;
            _categoryService = categoryService;
            _projectValidator = projectValidator;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _projectService.GetProjectsWithCategory();
            return View(values);
        }
        [HttpGet]
        public async Task<IActionResult> CreateProject()
        {
            var values = await _categoryService.TGetListAsync();

            List<SelectListItem> categoryValues = (from x in values
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.Id.ToString()
                                                   }).ToList();
            ViewBag.v = categoryValues;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateProject(Project project, IFormFile imageFile)
        {
            var result = await _projectValidator.ValidateAsync(project);
            if (result.IsValid)
            {
                if(imageFile != null)
                {
                    // Resmin Uzantısını Alıyoruz (jpg,png)
                    var extension = Path.GetExtension(imageFile.FileName);

                    // Resmin Yeni İsmini Oluşturuyoruz (benzersiz bir isim)
                    var newImageName = Guid.NewGuid() + extension;

                    // Resmin Kaydedileceği Yolu Belirliyoruz
                    var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", newImageName);

                    // Resmi Kaydediyoruz
                    var streamInfo = new FileStream(location, FileMode.Create);
                    await imageFile.CopyToAsync(streamInfo);

                    project.CoverImageUrl = newImageName;
                }
                else
                {
                    project.CoverImageUrl = "no-image.jpg"; // Varsayılan resim
                }

                project.CreatedDate = DateTime.Now;
                //project.IsActive = true;

                await _projectService.TInsertAsync(project);
                return RedirectToAction("Index", "Project", new {area = "Admin"});
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
            }


            var values = await _categoryService.TGetListAsync();
            List<SelectListItem> categoryValues = (from x in values
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.Id.ToString()
                                                   }).ToList();
            ViewBag.v = categoryValues;
            return View(project);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateProject(int id)
        {
            var value = await _projectService.TGetByIdAsync(id);

            if(value == null)
            {
                return RedirectToAction("Index");
            }
            var categories = await _categoryService.TGetListAsync();

            List<SelectListItem> categoryValues = (from x in categories
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.Id.ToString()
                                                   }).ToList();
            ViewBag.v = categoryValues;
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProject(Project project, IFormFile? imageFile)
        {
            var categories = await _categoryService.TGetListAsync();
            ViewBag.v = categories.Select(x => new SelectListItem
            {
                Text = x.CategoryName,
                Value = x.Id.ToString()
            }).ToList();

            if(imageFile != null)
            {
                var extension = Path.GetExtension(imageFile.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", newImageName);

                using(var stream = new FileStream(location, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                project.CoverImageUrl = newImageName;
            }

            if(ModelState.IsValid)
            {
                await _projectService.TUpdateAsync(project);
                return RedirectToAction("Index", "Project", new { area = "Admin" });
            }

            return View(project);
        }
        public async Task<IActionResult> DeleteProject(int id)
        {
            var value = await _projectService.TGetByIdAsync(id);

            if(value == null)
            {
                return RedirectToAction("Index");
            }

            // Burada kritik detay eklenen resmi de silmektir.
            if(!string.IsNullOrEmpty(value.CoverImageUrl) && value.CoverImageUrl != "no-image.jpg")
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", value.CoverImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            await _projectService.TDeleteAsync(value);
            return RedirectToAction("Index", "Project", new { area = "Admin" });
        }
    }
}
