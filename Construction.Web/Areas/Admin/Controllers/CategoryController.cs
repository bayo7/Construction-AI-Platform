using Construction.Business.Abstract;
using Construction.Entity.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]   // ← Editor de erişebilir
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IValidator<Category> _categoryValidator;

        public CategoryController(ICategoryService categoryService, IValidator<Category> categoryValidator)
        {
            _categoryService = categoryService;
            _categoryValidator = categoryValidator;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.TGetListAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult AddCategory() => View();

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            var result = await _categoryValidator.ValidateAsync(category);
            if (result.IsValid)
            {
                category.CreatedDate = DateTime.Now;
                await _categoryService.TInsertAsync(category);
                return RedirectToAction("Index", "Category", new { area = "Admin" });
            }
            foreach (var item in result.Errors)
                ModelState.AddModelError(item.PropertyName, item.ErrorMessage);

            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var value = await _categoryService.TGetByIdAsync(id);
            if (value == null) return RedirectToAction("Index");
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            var result = await _categoryValidator.ValidateAsync(category);
            if (result.IsValid)
            {
                await _categoryService.TUpdateAsync(category);
                return RedirectToAction("Index", "Category", new { area = "Admin" });
            }
            foreach (var item in result.Errors)
                ModelState.AddModelError(item.PropertyName, item.ErrorMessage);

            return View(category);
        }

        // Silme — SADECE Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var value = await _categoryService.TGetByIdAsync(id);
            if (value != null)
                await _categoryService.TDeleteAsync(value);

            return RedirectToAction("Index", "Category", new { area = "Admin" });
        }
    }
}
