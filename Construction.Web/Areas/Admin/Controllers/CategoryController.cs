using Construction.Business.Abstract;
using Construction.Entity.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Construction.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {

            var result = await _categoryValidator.ValidateAsync(category);

            if (result.IsValid)
            {

                //category.IsActive = true;
                category.CreatedDate = DateTime.Now;

                await _categoryService.TInsertAsync(category);
                return RedirectToAction("Index", "Category", new { area = "Admin" });
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View(category);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var value = await _categoryService.TGetByIdAsync(id);

            if(value == null)
            {
                return RedirectToAction("Index", "Category", new { area = "Admin" });
            }
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            var result = await _categoryValidator.ValidateAsync(category);
            if(result.IsValid)
            {
                var existingCategory = await _categoryService.TGetByIdAsync(category.Id);

                if(existingCategory == null)
                {
                    return RedirectToAction("Index", "Category", new { area = "Admin" });
                }

                existingCategory.CategoryName = category.CategoryName;
                existingCategory.IsActive = category.IsActive;

                await _categoryService.TUpdateAsync(existingCategory);
                return RedirectToAction("Index", "Category", new { area = "Admin" });
            }
            else
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View(category);
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryService.TGetByIdAsync(id);
            if(category != null) 
            {
                await _categoryService.TDeleteAsync(category);
            }
            
            return RedirectToAction("Index", "Category", new { area = "Admin" });
        }
    }
}
