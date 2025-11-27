using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OSP.SPASv2.Web.Models;
using SPASv2.Models;

namespace OSP.SPASv2.Web.Controllers
{
    public class UtilityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowAlert(string alertval)
        {
            UtilityViewModel model = new UtilityViewModel();
            model.Alert = JsonConvert.DeserializeObject<AlertProperties>(alertval);
            return View("_AlertBody",model);
        }

    }
}
