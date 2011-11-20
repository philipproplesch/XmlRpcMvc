using System.Web.Mvc;

namespace XmlRpcMvc.Sample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Rsd()
        {
            Response.ContentType = "text/xml";
            return View();
        }
    }
}
