using Microsoft.AspNetCore.Mvc;
using OSP.SPASv2.Web.APIFactory.SPASv2Repo;

namespace OSP.SPASv2.Web.Controllers
{
    public class SampleController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HTTPSPASv2Repo _HTTPSPASv2Repo;
        private readonly IHTTPSPASv2Repo _IHTTPSPASv2Repo;
        public SampleController(IHTTPSPASv2Repo IHTTPSPASv2Repo)
        {
            // _httpClientFactory = httpClientFactory;
            _IHTTPSPASv2Repo = IHTTPSPASv2Repo;
        }

        public IActionResult Index()
        {

            var sample =  _IHTTPSPASv2Repo.GetSample();
            return View();
        }
    }
}
