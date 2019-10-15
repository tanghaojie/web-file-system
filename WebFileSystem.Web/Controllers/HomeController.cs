using System.Web.Mvc;

namespace WebFileSystem.Web.Controllers
{
    public class HomeController : WebFileSystemControllerBase
    {
        public ActionResult Index()
        {
            //return View();
            return Redirect("/swagger");
        }
	}
}