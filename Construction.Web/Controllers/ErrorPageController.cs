using Microsoft.AspNetCore.Mvc;

namespace Construction.Web.Controllers
{
    public class ErrorPageController : Controller
    {
        [Route("ErrorPage/Error404")]
        public IActionResult Error404()
        {
            return View();
        }
    }
}
