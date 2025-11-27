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

//using ServiceReference1;

namespace SPASv2.Controllers
{
    public class IDCard
    {

        public string Type { get; set; }
        public string IDNo { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public string Sex { get; set; }
        public string DOB { get; set; }
        public string POB { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string Address { get; set; }
        public string Validity { get; set; }
        public string CardText { get; set; }

    }

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
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

        public HomeController(ILogger<HomeController> logger, UserManager<OSPSPASv2ApplicationUser> userManager, IConfiguration configuration
            ,SampleController sampleController)
           //)
        {
            _logger = logger;
            this._userManager = userManager;
            //var client = new SampleWaServiceClient();

            //var foo = client.GetBranchAsync();
            _ServiceUnit = new ServiceUnit();
            _SampleController = sampleController;

            string Username = "wa";
            string password = "123412";
            _RepositoryUnit = new RepositoryUnit();
            _configuration = configuration;
            BaseUrlRepo = _configuration.GetSection("APIBaseURL")["SPASv2.Repository"];
            BaseUrlService = _configuration.GetSection("APIBaseURL")["SPASv2.Service"];
            OSPUrlRepo = _configuration.GetSection("APIBaseURLCommon")["Common.Repository"];
            OSPUrlService = _configuration.GetSection("APIBaseURLCommon")["Common.Service"];
        }

        public async Task<IActionResult> ReadImageSample()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage()
        {
            IFormFileCollection formFiles = HttpContext.Request.Form.Files;

            IFormFile iFile = formFiles[0];

            //List<IFormFile>  FormFileList = new List<IFormFile>();

            //foreach (FormFile fromfile in formFiles)
            //{
            //    reqVM.FormFileList.Add(fromfile);
            //}



            //TblResponse resp = await _ServiceUnit.RequisitionService.ReadImageText(OSPUrlService, iFile);

            TblResponse resp = await _ServiceUnit.RequisitionService.ReadImageTextFromUrl(OSPUrlService, "https://online.stpeter.com.ph/Images/eCommerceBanner1.webp");

            return View("ReadImageSample", new IDCard() { CardText= "test"});
        }
        public IActionResult Index()
        {

            //  string _str = _ServiceUnit.SystemsService.GetChapelCode();

            //string connString = this.Configuration.GetConnectionString("spasv2context");

            return View();
        }

        public IActionResult Home()
        {

            //  string _str = _ServiceUnit.SystemsService.GetChapelCode();

            //string connString = this.Configuration.GetConnectionString("spasv2context");

            return View();
        }


        

        public async Task<IActionResult> DashBoard(string id = "REQUESTER")
        {
            
            //_SampleController.Index();

            ViewBag.ToggleSide = true;

            dbvm = new DashBoardViewModel();
            dbvm.ListofPR = new List<PaymentRequestModel>();
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            dbvm.PersonID = _userManager.GetUserId(this.User);
            Task<DashBoardViewModel> _CtrModel;
            dbvm.BarChartRequestData = this.GetRandomTop10(1, 1000000);
            dbvm.BarChartCreditedData = dbvm.BarChartRequestData;
            dbvm.BarCharRequestList = this.GetTop10List("Vendor", "RequestAmount");
            dbvm.DonutChartValue = this.GenerateRandomStatusCount();

            dbvm.DonutChartCtr = new List<decimal>();
            dbvm.DonutChartLabel = new List<string>();

            //dbvm.imgSrc = Utilities.GenerateBitMap("C:\\Image\\id.jpg");

            foreach (var item in dbvm.DonutChartValue)
            {
                dbvm.DonutChartLabel.Add(item.name);
                dbvm.DonutChartCtr.Add(item.value);
            }

            string personid = _userManager.GetUserId(this.User);

            string authorizeClass = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizeClasswURL(personid, BaseUrlRepo);
            dbvm.Paymenttypelist = await _RepositoryUnit.RefPaymentTypeRepository.GetPaymenttypeList(BaseUrlRepo);

            dbvm.DashboardType = authorizeClass;
            //dbvm.VendorRunningBalanceList = await _RepositoryUnit.TblRequisitionRepository.GetVendorRunningBalance(BaseUrlRepo);
            switch (authorizeClass)
            {
                case "APPROVER":
                    {
                        dbvm = await SetDashboardCounter("PD-APPROVER", dbvm);
                        return View("_DashboardApprover", dbvm);
                    }
                case "VERIFIER":
                    {
                        dbvm = await SetDashboardCounter("PD-VERIFIER", dbvm);
                        return View("_DashboardVerifier", dbvm);
                    }
                default:
                    {
                        dbvm = await SetDashboardCounter("PD-REQUESTER", dbvm);
                        return View("_DashboardRequester", dbvm);
                    }
            }
        }


        private void SetDashboardViewValue(ref DashBoardViewModel dbvm, Task<DashBoardViewModel> _CtrModel)
        {
            dbvm.ActiveCtr = _CtrModel.Result.ActiveCtr;
            dbvm.DeniedCtr = _CtrModel.Result.DeniedCtr;
            dbvm.PaymentValidationCtr = _CtrModel.Result.PaymentValidationCtr;


            dbvm.PDVerifierCtr = _CtrModel.Result.PDVerifierCtr;
            dbvm.OverdueCtr = _CtrModel.Result.OverdueCtr;

            dbvm.PDApproverCtr = _CtrModel.Result.PDApproverCtr;
            dbvm.SpecialCaseCtr = _CtrModel.Result.SpecialCaseCtr;
            dbvm.RushCtr = _CtrModel.Result.RushCtr;

            dbvm.RequestList = _CtrModel.Result.RequestList;
        }

        [HttpGet]
        public async Task<JsonResult> GetDashboardCtr(string requeststatus, DateTime DashboardDate)
        {
            dbvm = new DashBoardViewModel();
            string userid = _userManager.GetUserId(this.User);

            var resp = await _RepositoryUnit.TblRequisitionRepository.GetMaxEditDate(BaseUrlRepo);

            if (resp.MaxDateTime <= DashboardDate)
            {
                dbvm.DashboardDate = resp.MaxDateTime;
                dbvm.DashboardIsUptodate = true;

                return Json(
               dbvm, new JsonSerializerOptions());
            }
            
            return await GetRequesterDashboardCtr(userid, requeststatus);

        }

        [HttpGet]
        public async Task<IActionResult> SetSelectedList(string requeststatus)
        {
            DashBoardViewModel model  = new DashBoardViewModel();
            model.PersonID = _userManager.GetUserId(this.User);

            model = await SetDashboardCounter(requeststatus, model);

            //SetDashboardViewValue(ref dbvm, _CtrModel);

            return View("_SelectedList", model);

        }

        //[HttpGet]
        //public async Task<IActionResult> GetVendorRunningBalance()
        //{
        //    DashBoardViewModel model = new DashBoardViewModel();
        //    model.VendorRunningBalanceList = await _RepositoryUnit.TblRequisitionRepository.GetVendorRunningBalance(BaseUrlRepo);

        //    return View("_VendorRunningBalanceList", model);

        //}

        [HttpGet]
        public async Task<IActionResult> GetVendorRunningBalance(string AsOfMode,string PayClassCode)
        {
            DashBoardViewModel model = new DashBoardViewModel();
            model.PersonID = _userManager.GetUserId(this.User);

            model.VendorRunningBalanceList = await _RepositoryUnit.TblRequisitionRepository.GetVendorRunningBalance(BaseUrlRepo, PayClassCode, AsOfMode);
            return View("_VendorRunningBalanceList", model);

        }

        [HttpGet]
        public IActionResult GetChartBar()
        {
            dbvm = new DashBoardViewModel();

            return View("_ChartBar", dbvm);

        }

        [HttpGet]
        public IActionResult GetDashboardTop10Data(string filtertype, string sorttype)
        {
            return QueryDashboardTop10Data(filtertype, sorttype);
        }

        [HttpGet]
        public IActionResult GetRequestStatusCount(string filtertype)
        {
            return QueryRequestStatusCount(filtertype);
        }

        //public IActionResult GetDashboardTop10List(string filtertype, string sorttype)
        //{
        //    dbvm = new DashBoardViewModel();
        //    dbvm.BarCharRequestList =  GetTop10List(filtertype, sorttype);

        //    switch (sorttype)
        //    {
        //        case "RequestAmount":
        //            {
        //                dbvm.BarChartRequestData = GetRandomTop10(1, 1000000);
        //                break;
        //            }
        //        case "RequestUnits":
        //            {
        //                dbvm.BarChartRequestData = GetRandomTop10(1, 100);
        //                break;
        //            }
        //        case "CreditedAmount":
        //            {
        //                dbvm.BarChartRequestData = GetRandomTop10(1, 1000000);
        //                break;
        //            }
        //    };
        //    return View("_ChartBarRequestList", dbvm);
        //}

        public IActionResult GetDashboardTop10List(string RequestList)
        {
            dbvm = new DashBoardViewModel();
            dbvm.BarCharRequestList = new List<string>();
            dbvm.BarChartRequestData = new List<decimal>();

            return View("_ChartBarRequestList", dbvm);
        }
        private JsonResult QueryRequestStatusCount(string filtertype)
        {
            dbvm = new DashBoardViewModel();
            dbvm.DonutChartValue = GenerateRandomStatusCount();

            dbvm.DonutChartCtr = new List<decimal>();
            dbvm.DonutChartLabel = new List<string>();

            foreach (var item in dbvm.DonutChartValue)
            {
                dbvm.DonutChartLabel.Add(item.name);
                dbvm.DonutChartCtr.Add(item.value);
            }

            return Json(
               dbvm, new JsonSerializerOptions());
        }

        private List<DonutChartProperties> GenerateRandomStatusCount()
        {
            List<DonutChartProperties> _StatusList = new List<DonutChartProperties>();

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            string[] stages = { "FOR VERIFICATION", "FOR APPROVAL", "APV PREPARATION", "ACCT. POSTING", "FOR PAYMENT", "TREAS. POSTING", "FOR SENDING ADVICE", "FOR VALIDATION", "DENIED" };

            foreach (var item in stages)
            {
                Random rnd = new Random();

                decimal num = rnd.Next(1, 100);

                _StatusList.Add(new DonutChartProperties(@textInfo.ToTitleCase(item.Replace("FOR", "").ToLower()), num));
            }

            return _StatusList;
        }

        private JsonResult QueryDashboardTop10Data(string filtertype, string sorttype)
        {
            dbvm = new DashBoardViewModel();

            dbvm.BarChartRequestData = GetRandomTop10(1, 1000000);
            dbvm.BarChartCreditedData = dbvm.BarChartRequestData;
            dbvm.BarCharRequestList = GetTop10List(filtertype, sorttype);

            return Json(
               dbvm, new JsonSerializerOptions());
        }



        private IList<string> GetTop10List(string filtertype, string sorttype)
        {
            List<string> Templist = new List<string>();

            List<string> vendorlist = new List<string>{"CREATIVE FASHION, INTERIORS, LANDSCAPE, EVENTS AND SET DESIGN (C-FILES). INC.",
            "PHILIPPINE FEDERATION OF MEMORIAL, PENSION AND EDUCATION PLAN COMPANIES INC.",
            "INTERNET AND MOBILE MARKETING ASSOCIATION OF THE PHILIPPINES, INC.",
            "PHILIPPINE CENTER FOR ENTREPRENEURSHIP FOUNDATION INC. / KAPATID",
            "TROPICAL AIRCONDITIONING AND REFRIGERATION PRODUCTS CORPORATION",
            "BMC REFRIGERATION AND AIRCONDITIONING GEN. REPAIR AND SERVICES",
            "LANDBANK - EAST AVENUE BRANCH FAO BUREAU OF INTERNAL REVENUE",
            "RAC S ENGINEERING REFRIGERATION AND AIRCONDITIONING SERVICES",
            "BROADFIELD ENGINEERING TECHNOLOGIES AND INTEGRATED SERVICES",
            "BASALLOTES REFRIGERATION & AIR CONDITIONING SERVICE CENTER" };

            List<string> companylist = new List<string>{"ST. PETER CASKET MANUFACTURING AND TRADING LUZON, INC.",
            "ST. PETER CASKET FACTORY AND TRADING VISAYAS, INC.",
            "ST. PETER CASKET MANUFACTURING MINDANAO, INC.",
            "ST. PETERLIFE MEMORIAL HOMES (MINDANAO), INC.",
            "ST. PETERLIFE MEMORIAL HOMES (VISAYAS), INC.",
            "GOLDEN GATE MEMORIAL CHAPELS VISAYAS, INC.",
            "ST. PETERLIFE MEMORIAL HOMES (LUZON), INC.",
            "FOREST HILL FUNERAL HOMES & SERVICES INC.",
            "ST. PETER CHAPELS SOUTHERN LUZON, INC.",
            "ST. PETER CHAPELS NORTHERN LUZON, INC." };
            List<string> PayclassList = new List<string>
            { "MARKETING & SELLING EXPENSE (I) – HEAD OFFICE",
            "CONTRACTED SERVICES (DIRECT COST) - EMBALMER",
            "REPAIR & MAINTENANCE -OTHER FIXED ASSET",
            "MEMORIAL EXPENSE (ST. PETER TRIBUTE)",
            "CONTRACTED SERVICES-MEAL ALLOWANCE",
            "CONTRACTED SERVICES(VIDEO/IMAGE)",
            "CONTRACTED SERVICES (JANITORIAL)",
            "OTHER EXPENSE-CALLER`S GATHERING",
            "REPAIRS & MAINTENANCE (VEHICLE)",
            "LABORATORY/EMBALMING EQUIPMENT"};

            //List<string> ItemsPaidList = new List<string>
            //{ };
            switch (filtertype)
            {
                case "Vendor":
                    {
                        Templist = vendorlist;
                        break;
                    }
                case "Company":
                    {
                        Templist = companylist;
                        break;
                    }
                case "Payclass":
                    {
                        Templist = PayclassList;
                        break;
                    }
                case "ItemsPaid":
                    {
                        Templist = PayclassList;
                        break;
                    }
            };


            return Templist;
        }

        private List<decimal> GetRandomTop10(int min, int max)
        {
            List<decimal> RandomNum = new List<decimal>();
            for (int j = 0; j < 12; j++)
            {
                Random rnd = new Random();

                decimal num = rnd.Next(min, max);

                RandomNum.Add(num);
            }
            return RandomNum.OrderByDescending(t => t).ToList();
        }



        private async Task<JsonResult> GetRequesterDashboardCtr(string userid, string requeststatus)
        {
            dbvm = new DashBoardViewModel();
            dbvm.PersonID = userid;

            dbvm = await SetDashboardCounter(requeststatus, dbvm);
            //Task<DashBoardViewModel> _CtrModel = SetDashboardCtr(requeststatus);
            //SetDashboardViewValue(ref dbvm, _CtrModel);

            var resp = await _RepositoryUnit.TblRequisitionRepository.GetMaxEditDate(BaseUrlRepo);

            dbvm.DashboardDate = resp.MaxDateTime;
            dbvm.DashboardIsUptodate = false;

            return Json(
               dbvm, new JsonSerializerOptions());
        }

        private async Task<DashBoardViewModel> SetDashboardCtr(string Status)
        {
            IList<qryRequestPaymentRequestbyStatus> _tempactive =
                await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-REQUESTER", dbvm.PersonID, BaseUrlRepo);
            IList<qryRequestPaymentRequestbyStatus> _denied =
                await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("DN-APPROVER", dbvm.PersonID, BaseUrlRepo);
            IList<qryRequestPaymentRequestbyStatus> _pdVerifier =
                await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-VERIFIER", dbvm.PersonID, BaseUrlRepo);
            IList<qryRequestPaymentRequestbyStatus> _pdApprover =
                await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-APPROVER", dbvm.PersonID, BaseUrlRepo);
            IList<qryRequestPaymentRequestbyStatus> _pdPaymentVal =
                await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("CR-REQUESTER", dbvm.PersonID, BaseUrlRepo);
            IList<qryRequestPaymentRequestbyStatus> _pdVerifierDue =
                await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-VERIFIERDUE", dbvm.PersonID, BaseUrlRepo);
            IList<qryRequestPaymentRequestbyStatus> _pdApproverRush =
                await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-APPROVER-RUSH", dbvm.PersonID, BaseUrlRepo);

            IList<qryRequestPaymentRequestbyStatus> _active = _tempactive.Where(a => a.RequestID != a.MainReqNo).ToList();
            IList<qryRequestPaymentRequestbyStatus> _forpayment = new List<qryRequestPaymentRequestbyStatus>();

            foreach (var mainreqitem in _tempactive.Where(a=> a.RequestID == a.MainReqNo))
            {
                bool hasBalance = false;
                IList<qryRequisitionItem> poItem = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionItemList(BaseUrlRepo, mainreqitem.MainReqNo);

                IList<qryRequisitionInfo> reqPYInfo = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionListInfoByMainReqNo(BaseUrlRepo, mainreqitem.MainReqNo);
                IList < qryRequisitionItem> pyItem = new List<qryRequisitionItem>();
                foreach (var item in reqPYInfo)
                {
                    pyItem.AddRange(await _RepositoryUnit.TblRequisitionRepository.GetRequisitionItemList(BaseUrlRepo, item.ReqNo));
                }

                foreach (var _poitem in poItem)
                {
                    int approved = pyItem.Where(a => _poitem.Item == a.Item && _poitem.DeptCode == a.DeptCode).Sum(a => a.Quantity);

                    if (_poitem.Quantity > approved)
                    {
                        hasBalance = true;
                        break;
                    }
                }

                if (hasBalance)
                {
                    if (mainreqitem.Status.ToLower().Contains("for payment"))
                    {
                        _forpayment.Add(mainreqitem);
                    }

                    _active.Add(mainreqitem);
                }
                else
                {

                }
            }


            dbvm.ActiveCtr = _active.Count.ToString();
            dbvm.DeniedCtr = _denied.Count.ToString();
            dbvm.ForPaymentCtr = _forpayment.Count.ToString();
            dbvm.PaymentValidationCtr = _pdPaymentVal.Count.ToString();

            dbvm.PDVerifierCtr = _pdVerifier.Count.ToString();
            dbvm.OverdueCtr = _pdVerifierDue.Count.ToString();

            dbvm.PDApproverCtr = _pdApprover.Count.ToString();
            dbvm.SpecialCaseCtr = "0";
            dbvm.RushCtr = _pdApproverRush.Count.ToString();


            switch (Status)
            {
                case "PD-REQUESTER":
                    {
                        dbvm.RequestListTitle = "List of Pending Requisition";
                        dbvm.RequestList = _active;
                        break;
                    }
                case "PD-FORPAYMENT":
                    {
                        dbvm.RequestListTitle = "List of For Payment";
                        dbvm.RequestList = _forpayment;
                        break;
                    }
                case "DN-APPROVER":
                    {
                        dbvm.RequestListTitle = "List of Denied Requisition";
                        dbvm.RequestList = _denied;
                        break;
                    }
                case "PD-VERIFIER":
                    {
                        dbvm.RequestListTitle = "List of Requisition for Verification";
                        dbvm.RequestList = _pdVerifier;
                        break;
                    }
                case "PD-APPROVER":
                    {
                        dbvm.RequestListTitle = "List of Requisition for Approval";
                        dbvm.RequestList = _pdApprover;
                        break;
                    }
                case "PD-APPROVER-RUSH":
                    {
                        dbvm.RequestListTitle = "List of Requisition for Approval - RUSH";
                        dbvm.RequestList = _pdApproverRush;
                        break;
                    }
                case "CR-REQUESTER":
                    {
                        dbvm.RequestListTitle = "List of Requisition for Payment Validation";
                        dbvm.RequestList = _pdPaymentVal;
                        break;
                    }
                case "PD-VERIFIERDUE":
                    {
                        dbvm.RequestListTitle = "List of Overdue Requisition for Verification";
                        dbvm.RequestList = _pdVerifierDue;
                        break;
                    }
            }

            return dbvm;
        }

        private async Task<DashBoardViewModel> SetDashboardCounter(string Status, DashBoardViewModel model)
        {

            IList<qryRequestPaymentRequestbyStatus> _allActive = new List<qryRequestPaymentRequestbyStatus>();
            IList<qryRequestPaymentRequestbyStatus> _active = new List<qryRequestPaymentRequestbyStatus>();
            IList<qryRequestPaymentRequestbyStatus> _forpayment = new List<qryRequestPaymentRequestbyStatus>();
            IList<qryRequestPaymentRequestbyStatus> _powithbal = new List<qryRequestPaymentRequestbyStatus>();
            IList<qryRequestPaymentRequestbyStatus> _requesterRush = new List<qryRequestPaymentRequestbyStatus>();

            IList<qryRequestPaymentRequestbyStatus> _denied = new List<qryRequestPaymentRequestbyStatus>();
            IList<qryRequestPaymentRequestbyStatus> _pdVerifier = new List<qryRequestPaymentRequestbyStatus>();
            IList<qryRequestPaymentRequestbyStatus> _pdApprover = new List<qryRequestPaymentRequestbyStatus>();
            IList<qryRequestPaymentRequestbyStatus> _pdPaymentVal = new List<qryRequestPaymentRequestbyStatus>();
            IList<qryRequestPaymentRequestbyStatus> _pdVerifierDue = new List<qryRequestPaymentRequestbyStatus>();
            IList<qryRequestPaymentRequestbyStatus> _pdApproverRush = new List<qryRequestPaymentRequestbyStatus>();
            

            switch (Status)
            {
                case "PD-REQUESTER":
                case "DN-APPROVER":
                case "CR-REQUESTER":
                case "PD-FORPAYMENT":
                case "PD-REQRUSH":
                case "PD-POWITHBAL":
                    {
                        _allActive =
                            await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-REQUESTER", model.PersonID, BaseUrlRepo);
                        _denied =
                            await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("DN-APPROVER", model.PersonID, BaseUrlRepo);
                        _pdPaymentVal =
                            await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("CR-REQUESTER", model.PersonID, BaseUrlRepo);

                        _active = _allActive.Where(a=>a.Status != "FOR PAYMENT" && a.Status != "PO WITH BALANCE").ToList();

                        _forpayment = _allActive.Where(a=> a.MainReqNo == a.RequestID && a.TempBalanceQty == a.OrigQty && a.BalanceQty > 0 && a.Status == "FOR PAYMENT" ).ToList();
                        
                        _powithbal = _allActive.Where(a => a.MainReqNo == a.RequestID && a.BalanceQty > 0 && a.Status == "PO WITH BALANCE" && (a.ApprovedQty + a.PendingQty) > 0 ) .ToList();
                        

                        _requesterRush = _active.Where(a=> a.TransType == "RSH").ToList();

                        break;
                    }
                case "PD-VERIFIER":
                case "PD-VERIFIERDUE":
                    {
                        _pdVerifier =
                            await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-VERIFIER", model.PersonID, BaseUrlRepo);
                        _pdVerifierDue =
                            await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-VERIFIERDUE", model.PersonID, BaseUrlRepo);
                        
                        break;
                    }
                case "PD-APPROVER":
                case "PD-APPROVER-RUSH":
                    {
                        _pdApproverRush =
                            await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-APPROVER-RUSH", model.PersonID, BaseUrlRepo);
                        _pdApprover =
                            await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-APPROVER", model.PersonID, BaseUrlRepo);
                        break;
                    }
            }

            model.ActiveCtr = _active.Count.ToString();
            model.DeniedCtr = _denied.Count.ToString();
            model.ForPaymentCtr = _forpayment.Count.ToString();
            model.PaymentValidationCtr = _pdPaymentVal.Count.ToString();
            model.POWithBalanceCtr = _powithbal.Count.ToString();
            model.RequesterRushCtr = _requesterRush.Count.ToString();

            model.PDVerifierCtr = _pdVerifier.Count.ToString();
            model.OverdueCtr = _pdVerifierDue.Count.ToString();

            model.PDApproverCtr = _pdApprover.Count.ToString();
            model.SpecialCaseCtr = "0";
            model.RushCtr = _pdApproverRush.Count.ToString();


            switch (Status)
            {
                case "PD-REQUESTER":
                    {
                        model.RequestListTitle = "List of Pending Requisition";
                        model.RequestList = _active;
                        break;
                    }
                case "PD-FORPAYMENT":
                    {
                        model.RequestListTitle = "List of For Payment";
                        model.RequestList = _forpayment;
                        break;
                    }
                case "PD-REQRUSH":
                    {
                        model.RequestListTitle = "List of Active - RUSH";
                        model.RequestList = _requesterRush;
                        break;
                    }
                case "PD-POWITHBAL":
                    {
                        model.RequestListTitle = "List of PO w/ Balance";
                        model.RequestList = _powithbal;
                        break;
                    }
                case "DN-APPROVER":
                    {
                        model.RequestListTitle = "List of Denied Requisition";
                        model.RequestList = _denied;
                        break;
                    }
                case "PD-VERIFIER":
                    {
                        model.RequestListTitle = "List of Requisition for Verification";
                        model.RequestList = _pdVerifier;
                        break;
                    }
                case "PD-APPROVER":
                    {
                        model.RequestListTitle = "List of Requisition for Approval";
                        model.RequestList = _pdApprover;
                        break;
                    }
                case "PD-APPROVER-RUSH":
                    {
                        model.RequestListTitle = "List of Requisition for Approval - RUSH";
                        model.RequestList = _pdApproverRush;
                        break;
                    }
                case "CR-REQUESTER":
                    {
                        model.RequestListTitle = "List of Requisition for Payment Validation";
                        model.RequestList = _pdPaymentVal;
                        break;
                    }
                case "PD-VERIFIERDUE":
                    {
                        model.RequestListTitle = "List of Overdue Requisition for Verification";
                        model.RequestList = _pdVerifierDue;
                        break;
                    }
            }

            return model;
        }

        public IActionResult Privacy()
        {

            //TblEmployee sampleEmployee = new TblEmployee()
            //{
            //    EmpID = "6",
            //    FirstName = "davee1ee",
            //    LastName = "daveee1ee",
            //    Age = 10
            //};
            //_ServiceUnit.EmployeeService.Create(sampleEmployee);
            //_IRepositoryUnit.ISubjectRepository.Delete(sampleEmployee.EmpID);
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Login_(LoginModel Login)
        {
            string b = string.Empty;
            string c = string.Empty;
            DashBoardViewModel vm = new DashBoardViewModel();
            if (ModelState.IsValid)
            {
                var pw = await _ServiceUnit.AccountService.EncryptPW(Login.Password);

                //connect in controller to check the username and password

                string a = pw.ToString();

                var viewString = await UtilityRenderView.RenderViewToStringAsync(this, "Login", Login);
                var viewString2 = await UtilityRenderView.RenderViewToStringAsync(this, "Dashboard2", vm);
                //return Content(viewString);
                b = viewString;
                c = viewString2;
            }

            return Json(new { v1 = b, v2 = c },
                new System.Text.Json.JsonSerializerOptions());

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel Login)
        {
            string b = string.Empty;
            string c = string.Empty;
            DashBoardViewModel vm = new DashBoardViewModel();
            if (ModelState.IsValid)
            {
                var pw = await _ServiceUnit.AccountService.EncryptPW(Login.Password);

                //connect in controller to check the username and password

                string a = pw.ToString();

                var viewString = await UtilityRenderView.RenderViewToStringAsync(this, "Login", Login);
                var viewString2 = await UtilityRenderView.RenderViewToStringAsync(this, "Dashboard2", vm);
                //return Content(viewString);
                b = viewString;
                c = viewString2;
            }

            return View("Dashboard");

        }


        public async Task<IActionResult> PostLogin()
        {
            string b = string.Empty;
            string c = string.Empty;
            DashBoardViewModel vm = new DashBoardViewModel();
            if (ModelState.IsValid)
            {
                var pw = await _ServiceUnit.AccountService.EncryptPW("123456");

                //connect in controller to check the username and password

                string a = pw.ToString(); ;
            }

            return View("Dashboard");

        }
        [HttpGet]
        public async Task<IActionResult> TestResponse()
        {
            ViewBag.ErrorMessage = "error pala to eh!";
            ViewBag.Status = "error";

            DashBoardViewModel vm = new DashBoardViewModel();

            var pw = await _ServiceUnit.AccountService.EncryptPW("123456");

            return View("Login");

        }
        public IActionResult Login()
        {

            if (ModelState.IsValid)
            {

                //var User = from m in _context.Login select m;
                //User = User.Where(s => s.username.Contains(model.username));
                //if (User.Count() != 0)
                //{
                //    if (User.First().password == model.password)
                //    {
                //        return RedirectToAction("Success");
                //    }
                //}s
            }
            return View("Login");
        }

        public ActionResult ListofPR()
        {
            return View("ListofPR");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> SearchRequest(string RequestNo)
        {
            DashBoardViewModel model = new DashBoardViewModel();

            string[] SearchMode = { "REQNO","PYNO","REFNO" };

            model.isStartUp = false;
            model.isSearch = true;
            model.RequestNo = RequestNo;

            if (!string.IsNullOrEmpty(RequestNo))
            foreach (string mode in SearchMode )
            {
                DashBoardViewModel modelsearch =  await GetJourney(mode, RequestNo);
                model.RequestJourney = modelsearch.RequestJourney;
                
                if (model.RequestJourney.Count > 0)
                {
                    model.RequestNo = modelsearch.RequestNo;
                    model.isPaymentRequest = modelsearch.isPaymentRequest;
                    model.PaymentRequestHdr = modelsearch.PaymentRequestHdr;
                    model.PORequestJourney = modelsearch.PORequestJourney;

                    break;
                }

            }

            return View("_RequestJourney", model);
        }

        private async Task<DashBoardViewModel> GetJourney(string Mode, string RequestNo)
        {
            DashBoardViewModel model = new DashBoardViewModel();
            qryRequisitionInfo _reqinfo = null;

            switch (Mode)
            {
                case "REQNO":
                    {
                        _reqinfo = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionInfo(BaseUrlRepo, RequestNo);
                        break;
                    }
                case "PYNO":
                    {
                        _reqinfo = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionInfoByApprovalNo(BaseUrlRepo, RequestNo);
                        break;
                    }
                case "REFNO":
                    {
                        _reqinfo = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionInfoByRefNo(BaseUrlRepo, RequestNo);
                        break;
                    }
            }

            if (_reqinfo == null)
            {

                model.RequestJourney = new List<qryPaymentRequestAuthDtl>();
            }
            else
            {
                RequestNo = Convert.ToString(_reqinfo.ReqNo);
                model.RequestJourney = await _RepositoryUnit.PRAuthorizationRepository.GetPaymentRequestAuthListByPRNO(RequestNo, BaseUrlRepo);
                model.RequestNo = RequestNo;

                if (model.RequestJourney.Count > 0)
                {
                    model.PaymentRequestHdr = await _RepositoryUnit.PaymentRequestRepository.GetPaymentRequestHdr(RequestNo, BaseUrlRepo);

                    if (_reqinfo.ReqNo != _reqinfo.MainReqNo)
                    {
                        model.PORequestJourney = await _RepositoryUnit.PRAuthorizationRepository.GetPaymentRequestAuthListByPRNO(_reqinfo.MainReqNo, BaseUrlRepo);
                        model.isPaymentRequest = true;
                    }
                }
            }

            return model;
        }
    }
}