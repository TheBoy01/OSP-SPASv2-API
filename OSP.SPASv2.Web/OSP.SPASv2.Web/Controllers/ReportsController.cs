using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OSP.SPASv2.Web.APIServices.Services;
using OSP.SPASv2.Web.Models;
using OSP.SPASv2.Web.Utility;
//using Repository.IRepository;
using SPASv2.Models;
using System.Data;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Web.Helpers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Identity;
using OSP.SPASv2.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Build.Tasks;
using System.Text.Json;
using OSP.SPASv2.Web.APIServices;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using Microsoft.Data.SqlClient.Server;
using System.Security.Cryptography;
using System.Drawing.Imaging;
using System.Drawing;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using System.Collections.Generic;
using NuGet.Packaging;
using OSP.SPASv2.Web.Controllers;
using NuGet.Configuration;

namespace OSP.SPASv2.Web.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ILogger<ReportsController> _logger;
        private readonly UserManager<OSPSPASv2ApplicationUser> _userManager;
        private IConfiguration _configuration;


        ServiceUnit _ServiceUnit;
        private RepositoryUnit _RepositoryUnit;
        // private ServiceUnit _ServiceUnit;

        //private readonly IRepositoryUnit _IRepositoryUnit;

        DashBoardViewModel dbvm;

        string BaseUrlRepo;
        string BaseUrlService;
        string OSPUrlRepo;
        string OSPUrlService;
        private SampleController _SampleController;

        public ReportsController(ILogger<ReportsController> logger, UserManager<OSPSPASv2ApplicationUser> userManager, IConfiguration configuration
            , SampleController sampleController)
        //)
        {
            _logger = logger;
            this._userManager = userManager;
            _ServiceUnit = new ServiceUnit();

            _RepositoryUnit = new RepositoryUnit();
            _configuration = configuration;
            BaseUrlRepo = _configuration.GetSection("APIBaseURL")["SPASv2.Repository"];
            BaseUrlService = _configuration.GetSection("APIBaseURL")["SPASv2.Service"];
            OSPUrlRepo = _configuration.GetSection("APIBaseURLCommon")["Common.Repository"];
            OSPUrlService = _configuration.GetSection("APIBaseURLCommon")["Common.Service"];
        }

        public async Task<IActionResult> Index()
        {
            ReportViewModel model = new ReportViewModel();
            string personid = _userManager.GetUserId(this.User);
            model.Params = new ReportParams() {  PersonId = personid };
            model.Params = await _RepositoryUnit.ReportRepository.GetReportListByPersonID(BaseUrlRepo, model.Params);
            return View(model);
        }

        public IActionResult GenerateReport(string ReportParams)
        {
            try
            {
                string personid = _userManager.GetUserId(this.User);
                ReportParamsModel model = JsonConvert.DeserializeObject<ReportParamsModel>(ReportParams);
                model.PersonId = personid;

                throw new Exception("The report is currently undergoing maintenance.");
                return View(model);
                //return Json(new { vendordtl = vendordtl, itemList = itemList }, new JsonSerializerOptions());
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }

        }

    }
}
