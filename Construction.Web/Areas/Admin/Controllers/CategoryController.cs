using Construction.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.TGetListAsync();
            return View(categories);
        }
    }
}
