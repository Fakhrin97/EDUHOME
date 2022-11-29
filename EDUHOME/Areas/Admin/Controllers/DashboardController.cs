using Microsoft.AspNetCore.Mvc;

namespace EDUHOME.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
