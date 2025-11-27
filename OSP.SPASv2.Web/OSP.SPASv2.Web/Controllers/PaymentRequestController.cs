using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
//using OSP.Common.Domain.View;
using OSP.SPASv2.Web.APIServices;

using OSP.SPASv2.Web.APIServices.Services;
using OSP.SPASv2.Web.Models;
using OSP.SPASv2.Web.Utility;
//using Repository.IRepository;
using SPASv2.Models;
using System.Diagnostics;

using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.IO;
using Microsoft.AspNetCore.Identity;
using OSP.SPASv2.Web.Areas.Identity.Data;
using Microsoft.VisualBasic;
using OSP.SPASv2.Web.Controllers;
using OSP.SPASv2.Domain.View;
//using AspNetCore;

namespace SPASv2.Controllers
{
    //[ValidateAntiForgeryToken]
    public class PaymentRequestController : Controller
    {

        //private readonly IRepositoryUnit _IRepositoryUnit;
        private ILogger<PaymentRequestController> _logger;
        private ServiceUnit _ServiceUnit;
        private UserManager<OSPSPASv2ApplicationUser> _userManager;
        private RepositoryUnit _RepositoryUnit;
        public string errorMessage = "";



        private IConfiguration _configuration;
        private IHostingEnvironment _environment;

        string personid;
        string UploadingPathPR;
        ////string UploadingPathPR;
        string BaseUrlRepo;
        string BaseUrlService;
        string OSPUrlRepo;
        string OSPUrlService;

        public PaymentRequestController(ILogger<PaymentRequestController> logger, IConfiguration configuration
            , IHostingEnvironment environment, UserManager<OSPSPASv2ApplicationUser> userManager)
        {
            _logger = logger;
            _RepositoryUnit = new RepositoryUnit();

            _configuration = configuration;
            _environment = environment;
            _ServiceUnit = new ServiceUnit();
            this._userManager = userManager;
          UploadingPathPR = _configuration.GetSection("UploadingPath")["PaymentRequest"];

            BaseUrlRepo = _configuration.GetSection("APIBaseURL")["SPASv2.Repository"];
            BaseUrlService = _configuration.GetSection("APIBaseURL")["SPASv2.Service"];
            OSPUrlRepo = _configuration.GetSection("APIBaseURLCommon")["Common.Repository"];
            OSPUrlService = _configuration.GetSection("APIBaseURLCommon")["Common.Service"];
        }


        // GET: PaymentRequest
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult CreatePRBatch()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyPaymentRequest(string prno, string verify)
        {
            TblResponse response = new TblResponse();
            response.Status = "SUCCESS";
            response.ErrorMessage = "Process complete.";
            response.TrxNo = "12";
            response.MethodName = "POST";
            response.UniqueInfo = "12";

            personid = _userManager.GetUserId(this.User);
            qryUpdateStatusAuth _qry = new qryUpdateStatusAuth();
            _qry.StatusType = "AP";
            _qry.PRRefNo = prno;

            _qry.PersonID = personid;


            response = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qry, BaseUrlRepo );
            //return Json(new { success = response.Status, errormsg = response.ErrorMessage });
            return Json(response, new JsonSerializerOptions());
        }

        public async Task<IActionResult> ViewPR(string prno)
        {
            personid = _userManager.GetUserId(this.User);
            qryUpdateStatusAuth _qry = new qryUpdateStatusAuth();
            TblResponse _response = new TblResponse();
            _qry.PRRefNo = prno;
            _qry.PersonID = personid;
            _qry.StatusType = "PD";
            TblPaymentRequestAuth _auth = await _RepositoryUnit.PRAuthorizationRepository.GetALLTblPaymentRequestAuthByPersonId("",prno, personid);

            string baseUri = $"{Request.Scheme}://{Request.Host}";
            TempData["baseUri"] = baseUri;

            PaymentRequestModel _PaymentRequestModel = new PaymentRequestModel();
            var prhdr = await _RepositoryUnit.PaymentRequestRepository.GetPaymentRequestHdr(prno, BaseUrlRepo);
            _PaymentRequestModel.isverify = false;


            if (_auth != null)
            {
                _PaymentRequestModel.forapproval = _auth.AuthorizeClass;
                if (!_auth.IsRead)
                {
                    _response = await _RepositoryUnit.PRAuthorizationRepository.UpdateReadPRAuthorization(_qry,"");
                }
                if (_auth.PersonID == personid)
                {

                    if (_auth.StatusType == "PD")
                    {
                        _PaymentRequestModel.isverify = true;
                    }

                    if (_auth.AuthorizeClass == "VERIFIER")
                    {
                        _PaymentRequestModel.lblAuth = "Verify";
                    }
                    else
                    {
                        _PaymentRequestModel.lblAuth = "Approve";
                    }
                }


            }
            else
            {
                _auth = await _RepositoryUnit.PRAuthorizationRepository.GetALLTblPaymentRequestAuthByPersonId("",prno, "REQUESTER-VAL");
                if (_auth != null)
                {
                    _PaymentRequestModel.forapproval = _auth.AuthorizeClass;

                    _PaymentRequestModel.isverify = true;
                }


            }

            _PaymentRequestModel.dashboardViewModel = new DashBoardViewModel();

            _PaymentRequestModel.dashboardViewModel.RequestJourney = await _RepositoryUnit.PRAuthorizationRepository.GetPaymentRequestAuthListByPRNO(prhdr.PRNo,"");
            _PaymentRequestModel.dashboardViewModel.RequestNo = prhdr.PRNo;

            if (_PaymentRequestModel.dashboardViewModel.RequestJourney.Count > 0)
            {
                _PaymentRequestModel.dashboardViewModel.PaymentRequestHdr = await _RepositoryUnit.PaymentRequestRepository.GetPaymentRequestHdr(prhdr.PRNo, BaseUrlRepo);
            }



            _PaymentRequestModel.PRNo = prhdr.PRNo;
            _PaymentRequestModel.Requestdate = Convert.ToString(prhdr.RequestDate);
            _PaymentRequestModel.Company = prhdr.CompanyType;
            _PaymentRequestModel.Code = prhdr.DeptDesc;
            _PaymentRequestModel.Address = "";
            _PaymentRequestModel.PaymentClass = prhdr.PayDesc;
            _PaymentRequestModel.TIN = "";
            _PaymentRequestModel.PCVName = prhdr.PayeeName;
            _PaymentRequestModel.Supplier = prhdr.DisplayName;
            _PaymentRequestModel.TAmount = prhdr.TotalAmount;
            _PaymentRequestModel.PaymentMethod = prhdr.PayMethodType;
            _PaymentRequestModel.Destination = "";
            _PaymentRequestModel.Name = "JUAN DELA CRUZ";
            _PaymentRequestModel.Remarks = prhdr.Remarks;
            //_PaymentRequestModel.PRNo = "SPLPI-MAYON-230400001";
            //_PaymentRequestModel.Requestdate = "4-19-2023 09:17:03";
            //_PaymentRequestModel.Company = "LifePlan";
            //_PaymentRequestModel.Code = "MAYON";
            _PaymentRequestModel.Address = "RM 230 BRB BLDG. A BONIFACIO ST. MAYON QUEZON CITY"; ;

            //_PaymentRequestModel.PaymentClass = "REPAIRS & MAINTENANCE (OTHERS)";
            _PaymentRequestModel.TIN = "123-321-123-000";
            //_PaymentRequestModel.PCVName = "BLACKHOUNDS SECURITY AND INVESTIGATION AGENCY INC.";

            //_PaymentRequestModel.Supplier = "BLACKHOUNDS SECURITY AND INVESTIGATION AGENCY INC.";
            //_PaymentRequestModel.ItemCategory = "INKS AND TONERS";
            //_PaymentRequestModel.ItemDesc = "INKS AND TONERS";
            //_PaymentRequestModel.UOM = "PCS";
            //_PaymentRequestModel.Quantity = 500;
            //_PaymentRequestModel.TAmount = 82000;

            //_PaymentRequestModel.PaymentMethod = "CHEQUE";
            _PaymentRequestModel.Destination = "";
            _PaymentRequestModel.PaymentNetwork = "BDO";
            //_PaymentRequestModel.Name = "JUAN DELA CRUZ";

            _PaymentRequestModel.InvoiceDate = "" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "";

            //_PaymentRequestModel.Remarks = "PAYMENT FOR SECURITY GUARD";


            //_PaymentRequestModel.PaymentMethod = "CASH";



            var StatusList = new List<RefStatus>();
            StatusList.Add(new RefStatus() { Statuscode = "1", StatusDesc = "WRONG ENCODING OF PRODUCT" });
            StatusList.Add(new RefStatus() { Statuscode = "2", StatusDesc = "WRONG ENCODING OF SERVICE" });
            StatusList.Add(new RefStatus() { Statuscode = "3", StatusDesc = "WRONG ENCODING OF VENDOR" });
            _PaymentRequestModel.StatusList = StatusList;


            //var model = new FilesViewModel();
            string dir = Path.Combine(UploadingPathPR + "\\" + prhdr.PRNo);
            var files = new List<FileDetails>();

            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            // Get an array of file paths within the specified directory
            string[] filePaths = Directory.GetFiles(dir);

            if (filePaths.Length > 0)
            {
                DirectoryInfo directory = new DirectoryInfo(dir);
                List<FileInfo> FileList = new List<FileInfo>();
                //FileList = directory.GetFiles(".", SearchOption.AllDirectories).ToList();

                FileList = directory.GetFiles("*", SearchOption.AllDirectories).ToList();


                for (int i = 0; i < FileList.Count; i++)
                {
                    //FileList[i].Attributes.HasFlag(FileAttributes.Hidden);
                    if (FileList[i].Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        continue;
                    }


                    if (System.IO.Path.GetFileName(FileList[i].Name).Contains(".db"))
                    {
                        continue;
                    }

                    _PaymentRequestModel.Files.Add(
                        new FileDetails { Name = System.IO.Path.GetFileName(FileList[i].Name), Path = FileList[i].FullName, Src = Utilities.GenerateBitMap(FileList[i].FullName) });

                }
            }

            _PaymentRequestModel.tmpPaymentRequestInventory = await _RepositoryUnit.PaymentRequestRepository.GettmpPaymentRequestInventoryA(personid, "Auto...", BaseUrlRepo);

            ViewData["tmp"] = _PaymentRequestModel.tmpPaymentRequestInventory;

            //var rcthist = await _RepositoryUnit.PRAuthorizationRepository.GetPaymentRequestAuthListByPRNO("SPLPIBALING2308-000001");

            return View(_PaymentRequestModel);
        }


        public async Task<IActionResult> ListofPR()
        {
            personid = _userManager.GetUserId(this.User);
            PaymentRequestModel _PaymentRequestModel = new PaymentRequestModel();
            _PaymentRequestModel.dashboardViewModel = new DashBoardViewModel();
            //_PaymentRequestModel.dashboardViewModel.RequestList = await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-REQUESTER", personid);

            return View(_PaymentRequestModel);
        }



        // GET: PaymentRequest/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public async Task<IActionResult> GetRefPaymentTypesAsync()
        {
            ViewData["RefPaymentTypes"] = await _RepositoryUnit.RefPaymentTypeRepository.GetPaymenttypes(BaseUrlRepo);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GettmpPaymentRequestInventory(tmpPaymentRequestInventory _tmp, string disccode)
        {


            IList<tmpPaymentRequestInventory> _tmpPaymentRequestInventory = new List<tmpPaymentRequestInventory>();

            qryComputeBreakdown _qry = new qryComputeBreakdown();
            _qry.Qty = _tmp.Qty;
            _qry.Gross = _tmp.Price;
            _qry.VatRate = Convert.ToDecimal(1.12);
            _qry.Discount = _tmp.Discount;
            _qry.Disccode = " ";
            _qry.AmountDue = 0;
            _qry.Vat = 0;
            _qry.NetOfVAT = 0;
            _qry.isVAT = true;
           
            _qry = await _ServiceUnit.PaymentRequestService.ComputeBreakDown(_qry,BaseUrlService);

            _tmp.Gross = _qry.Gross;
            _tmp.VatRate = _qry.VatRate;
            _tmp.NetofVat = _qry.NetOfVAT;
            _tmp.Discount = _qry.Discount;
            _tmp.TotalAmt = _qry.AmountDue;


            TblResponse _resp = await _RepositoryUnit.PaymentRequestRepository.PosttmpPaymentRequestInventory(_tmp, BaseUrlRepo);

            _tmpPaymentRequestInventory = await _RepositoryUnit.PaymentRequestRepository.GettmpPaymentRequestInventoryA(_tmp.AuditUser, _tmp.PRNo,BaseUrlRepo);

            //var viewString = await UtilityRenderView.RenderViewToStringAsync(this, "_tmpPaymentRequestInventory", _tmpPaymentRequestInventory);
            //_resp.ErrorMessage = "Error churva";
            //_resp.Status = "error";

            ViewData["resp"] = _resp.ErrorMessage;

            ViewData["tmp"] = _tmpPaymentRequestInventory;



            var partialView = new PartialViewResult
            {
                ViewName = "_tmpPaymentRequestInventory",
                ViewData = ViewData,
                TempData = TempData,
                //Model = _tmpPaymentRequestInventory

            };


            //if (string.IsNullOrEmpty(_resp.ErrorMessage))
            //{
            // return PartialView("_tmpPaymentRequestInventory", _tmpPaymentRequestInventory);
            return partialView;
            //}
            //else
            //{
            //    return Json(new { error = true, errormsg = _resp.ErrorMessage }, new JsonSerializerOptions());
            //}

            //return Json(new { v1 = viewString },
            //    new System.Text.Json.JsonSerializerOptions());
            //return Json(_tmpPaymentRequestInventory, new JsonSerializerOptions());
            //return PartialView("_tmpPaymentRequestInventory", _tmpPaymentRequestInventory);

            //return PartialViewResult(_resp);


        }

        // GET: PaymentRequest/Create
        // GET: PaymentRequest/Create
        //public async Task<IActionResult> Create(string companycode)
        //{
        //    ViewBag.msg = new TblResponse()
        //    {
        //        ErrorMessage = "",
        //        Status = "success"
        //    };


        //    PaymentRequestModel _PaymentRequestModel = new PaymentRequestModel();
        //    _PaymentRequestModel.PRNo = "1234";
        //    _PaymentRequestModel.AuditUser = "MAIN OBOLIVWL";
        //    _PaymentRequestModel.RequestDatetime = DateTime.Now;
        //    _PaymentRequestModel.Requestdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

        //    //_PaymentRequestModel.Company = _RepositoryUnit.CompanyRepository.GetCompanies();
        //    _PaymentRequestModel.Paymenttypelist = await _RepositoryUnit.RefPaymentTypeRepository.GetPaymenttypeList();
        //    _PaymentRequestModel.tmpPaymentRequestInventory = await _RepositoryUnit.PaymentRequestRepository.GettmpPaymentRequestInventoryA(_PaymentRequestModel.AuditUser);

        //    ViewData["tmp"] = _PaymentRequestModel.tmpPaymentRequestInventory;
        //    _PaymentRequestModel.Discountlist = await _RepositoryUnit.RefDiscountRepository.GetRefDiscount();

        //    List<TblVendorpaymethod> bankdetails = new List<TblVendorpaymethod>();
        //    bankdetails.Add(new TblVendorpaymethod { BankCode = "-1", AcctNo = "" });
        //    //var PaymodeList = new List<qryPaymode>();
        //    //PaymodeList.Add(new qryPaymode() { PaymodeCode = "1", Paymode = "CASH" });
        //    //PaymodeList.Add(new qryPaymode() { PaymodeCode = "2", Paymode = "CHEQUE" });
        //    //PaymodeList.Add(new qryPaymode() { PaymodeCode = "3", Paymode = "DIGITAL WALLET" });
        //    //_PaymentRequestModel.PaymodeList = PaymodeList;



        //    //  _PaymentRequestModel.Company = "St. Peter LifePlan Inc.";
        //    //  _PaymentRequestModel.Code = "HO1 Office";
        //    //_PaymentRequestModel.Address = "St. Peter Corporate Center, 999 EDSA Veterans Village, Quezon City";

        //    //_PaymentRequestModel.PaymentType = "Payment to Suppliers";
        //    //_PaymentRequestModel.TIN = "000-388-433-000";

        //    //_PaymentRequestModel.Supplier = "NEED INK SALES AND SERVICES";
        //    //_PaymentRequestModel.PCVName = "JUAN DELA CRUZ";



        //    _PaymentRequestModel.BankAccountList = bankdetails;
        //    //_PaymentRequestModel.ItemCategory = "INKS AND TONERS";
        //    //_PaymentRequestModel.Item = "INKS AND TONERS";
        //    //_PaymentRequestModel.UnitofMeasurement = "PCS";
        //    //_PaymentRequestModel.Quantity = 100;
        //    //_PaymentRequestModel.ItemPrice = 200;

        //    //_PaymentRequestModel.PaymentMethod = "CHEQUE";
        //    //_PaymentRequestModel.Destination = "5210 6988 8182 2136";
        //    //_PaymentRequestModel.PaymentNetwork = "BDO";
        //    //_PaymentRequestModel.Name = "JUAN DELA CRUZ";

        //    //_PaymentRequestModel.CompanyList = _IRepositoryUnit.IRefCompanytypeRepository.GetAllObjects();
        //    //_PaymentRequestModel.Branchlist = _IRepositoryUnit.IRefBranchRepository.GetAllObjects1(_PaymentRequestModel.Company);

        //    //_PaymentRequestModel.PaymentRequestdtl = _IRepositoryUnit.IRefCompanytypeRepository.qryPaymentRequestDtl();


        //    //PaymodeList.Add(new qryPaymode() { PaymodeCode = "3", Paymode = "CREDIT" });


        //    return View(_PaymentRequestModel);
        //}

        public async Task<IActionResult> Create1()
        {
            ViewBag.msg = new TblResponse()
            {
                ErrorMessage = "",
                Status = "success"
            };

            personid = _userManager.GetUserId(this.User);
            PaymentRequestModel _PaymentRequestModel = new PaymentRequestModel();

            _PaymentRequestModel.VendorList = new List<qryVendorList>();
            //_PaymentRequestModel.VendorList.Add(new qryVendorList() { VendorCode = "3", VendorName = "ron" });
            //_PaymentRequestModel.VendorList.Add(new qryVendorList() { VendorCode = "2", VendorName = "wa" });

            _PaymentRequestModel.PRNo = "Auto...";
            _PaymentRequestModel.AuditUser = personid;
            _PaymentRequestModel.RequestDatetime = DateTime.Now;
            _PaymentRequestModel.Requestdate = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss");

            //_PaymentRequestModel.CompanyList = await _RepositoryUnit.CompanyRepository.GetCompanyTypes("");
            _PaymentRequestModel.CompanyList = await _RepositoryUnit.CompanyRepository.GetCompanyTypesAccess(personid, OSPUrlRepo);
            _PaymentRequestModel.Company = _PaymentRequestModel.CompanyList.Select(m => m.CompanyType).FirstOrDefault();

            _PaymentRequestModel.DeptList = await _RepositoryUnit.RefDepartmentRepository.GetDeptByPersonID(personid, _PaymentRequestModel.Company,OSPUrlRepo);
            _PaymentRequestModel.Code = _PaymentRequestModel.DeptList.Select(m => m.DeptCode).FirstOrDefault();
            _PaymentRequestModel.qryBranch = await _RepositoryUnit.BranchRepository.GetBranchdetails(_PaymentRequestModel.Company, _PaymentRequestModel.Code, OSPUrlRepo);

            if (_PaymentRequestModel.qryBranch == null)
            {
                _PaymentRequestModel.Address = "";
            }
            else
            {
                _PaymentRequestModel.Address = _PaymentRequestModel.qryBranch.Address;
            }

            _PaymentRequestModel.Paymenttypelist = await _RepositoryUnit.RefPaymentTypeRepository.GetPaymenttypeList(BaseUrlRepo);
            _PaymentRequestModel.tmpPaymentRequestInventory = await _RepositoryUnit.PaymentRequestRepository.GettmpPaymentRequestInventoryA(personid, "Auto...", BaseUrlRepo);

            ViewData["tmp"] = _PaymentRequestModel.tmpPaymentRequestInventory;
            _PaymentRequestModel.Discountlist = await _RepositoryUnit.RefDiscountRepository.GetRefDiscount(BaseUrlRepo);

            List<RefBranch> branch = new List<RefBranch>();
            branch.Add(new RefBranch { BranchCode = "-1", BranchDesc = "" });
            _PaymentRequestModel.Branchlist = branch;



            return View(_PaymentRequestModel);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.msg = new TblResponse()
            {
                ErrorMessage = "",
                Status = "success"
            };

            personid = _userManager.GetUserId(this.User);
            PaymentRequestModel _PaymentRequestModel = new PaymentRequestModel();
            _PaymentRequestModel.isCreate = true;
            _PaymentRequestModel.VendorList = new List<qryVendorList>();
            //_PaymentRequestModel.VendorList.Add(new qryVendorList() { VendorCode = "3", VendorName = "ron" });
            //_PaymentRequestModel.VendorList.Add(new qryVendorList() { VendorCode = "2", VendorName = "wa" });

            _PaymentRequestModel.PRNo = "Auto...";
            _PaymentRequestModel.AuditUser = personid;
            _PaymentRequestModel.RequestDatetime = DateTime.Now;
            _PaymentRequestModel.Requestdate = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss");

            //_PaymentRequestModel.CompanyList = await _RepositoryUnit.CompanyRepository.GetCompanyTypes("");
            _PaymentRequestModel.CompanyList = await _RepositoryUnit.CompanyRepository.GetCompanyTypesAccess(personid, OSPUrlRepo);
            _PaymentRequestModel.Company = _PaymentRequestModel.CompanyList.Select(m => m.CompanyType).FirstOrDefault();

            _PaymentRequestModel.DeptList = await _RepositoryUnit.RefDepartmentRepository.GetDeptByPersonID(personid, _PaymentRequestModel.Company, OSPUrlRepo);
            _PaymentRequestModel.Code = _PaymentRequestModel.DeptList.Select(m => m.DeptCode).FirstOrDefault();
            _PaymentRequestModel.qryBranch = await _RepositoryUnit.BranchRepository.GetBranchdetails(_PaymentRequestModel.Company, _PaymentRequestModel.Code, OSPUrlRepo);

            if (_PaymentRequestModel.qryBranch == null)
            {
                _PaymentRequestModel.Address = "";
            }
            else
            {
                _PaymentRequestModel.Address = _PaymentRequestModel.qryBranch.Address;
            }

            _PaymentRequestModel.Paymenttypelist = await _RepositoryUnit.RefPaymentTypeRepository.GetPaymenttypeList(BaseUrlRepo);
            _PaymentRequestModel.tmpPaymentRequestInventory = await _RepositoryUnit.PaymentRequestRepository.GettmpPaymentRequestInventoryA(personid, "Auto...", BaseUrlRepo);

            ViewData["tmp"] = _PaymentRequestModel.tmpPaymentRequestInventory;
            _PaymentRequestModel.Discountlist = await _RepositoryUnit.RefDiscountRepository.GetRefDiscount(BaseUrlRepo);

            List<RefBranch> branch = new List<RefBranch>();
            branch.Add(new RefBranch { BranchCode = "-1", BranchDesc = "" });
            _PaymentRequestModel.Branchlist = branch;


            //List<TblVendorpaymethod> bankdetails = new List<TblVendorpaymethod>();
            //bankdetails.Add(new TblVendorpaymethod { BankCode = "-1", AcctNo = "" });
            ////_PaymentRequestModel.BankAccountList = bankdetails;
            //var PaymodeList = new List<qryPaymode>();
            //PaymodeList.Add(new qryPaymode() { PaymodeCode = "1", Paymode = "CASH" });
            //PaymodeList.Add(new qryPaymode() { PaymodeCode = "2", Paymode = "CHEQUE" });
            //PaymodeList.Add(new qryPaymode() { PaymodeCode = "3", Paymode = "DIGITAL WALLET" });
            //_PaymentRequestModel.PaymodeList = PaymodeList;



            //  _PaymentRequestModel.Company = "St. Peter LifePlan Inc.";
            //  _PaymentRequestModel.Code = "HO1 Office";
            //_PaymentRequestModel.Address = "St. Peter Corporate Center, 999 EDSA Veterans Village, Quezon City";

            //_PaymentRequestModel.PaymentType = "Payment to Suppliers";
            //_PaymentRequestModel.TIN = "000-388-433-000";

            //_PaymentRequestModel.Supplier = "NEED INK SALES AND SERVICES";
            //_PaymentRequestModel.PCVName = "JUAN DELA CRUZ";




            //_PaymentRequestModel.ItemCategory = "INKS AND TONERS";
            //_PaymentRequestModel.Item = "INKS AND TONERS";
            //_PaymentRequestModel.UnitofMeasurement = "PCS";
            //_PaymentRequestModel.Quantity = 100;
            //_PaymentRequestModel.ItemPrice = 200;

            //_PaymentRequestModel.PaymentMethod = "CHEQUE";
            //_PaymentRequestModel.Destination = "5210 6988 8182 2136";
            //_PaymentRequestModel.PaymentNetwork = "BDO";
            //_PaymentRequestModel.Name = "JUAN DELA CRUZ";

            //_PaymentRequestModel.CompanyList = _IRepositoryUnit.IRefCompanytypeRepository.GetAllObjects();
            //_PaymentRequestModel.Branchlist = _IRepositoryUnit.IRefBranchRepository.GetAllObjects1(_PaymentRequestModel.Company);

            //_PaymentRequestModel.PaymentRequestdtl = _IRepositoryUnit.IRefCompanytypeRepository.qryPaymentRequestDtl();


            //PaymodeList.Add(new qryPaymode() { PaymodeCode = "3", Paymode = "CREDIT" });


            return View(_PaymentRequestModel);
        }

        [HttpPost]
        public async Task<IActionResult> ValidatePayment(List<IFormFile> files)
        {
            try
            {
                TblResponse _resp = new TblResponse();
                string prno = Request.Form["PRNoID"];
                await UploadFiles(files, prno);

                personid = _userManager.GetUserId(this.User);
                qryUpdateStatusAuth _qry = new qryUpdateStatusAuth();
                _qry.StatusType = "AP";
                _qry.PRRefNo = prno;
                _qry.PersonID = "REQUESTER-VAL";
                //var _auth = new AuthorizationController(_logger, _userManager);
                //_auth.ControllerContext = new ControllerContext(ApprovePRAuthorization(_qry));
                //var result = _auth.ApprovePRAuthorization(_qry);
                //return result();
                TblPaymentrequesthdr prhdr = await _RepositoryUnit.PaymentRequestRepository.ReadRequestByPRNo(prno,BaseUrlRepo);
                prhdr.Active = false;
                TblResponse response = await _RepositoryUnit.PaymentRequestRepository.UpdatePaymentRequestHdr(prhdr, BaseUrlRepo);
                response = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qry, BaseUrlRepo);

                //return RedirectToAction("ApprovePRAuthorization", "Authorization", new { prno = _qry.PRRefNo });
                return Json(new { success = response.Status, errormsg = response.ErrorMessage });
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<IActionResult> UploadFile(IFormFile files, string prno)
        {
            //long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            //foreach (var formFile in files)
            //{

            var fileName = System.IO.Path.GetFileName(files.FileName);

            // Get file path to be uploaded
            //var path = Path.Combine(Directory.GetCurrentDirectory(), prno);
            var path = Path.Combine(UploadingPathPR, prno);
            var filePath = Path.Combine(UploadingPathPR, prno, fileName);
            //var filePath = "C:\\Wa\\sample\\" + fileName;

            // Check If file with same name exists and delete it
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            else
            {
                Directory.CreateDirectory(path);
            }

            // Create a new local file and copy contents of uploaded file
            using (var localFile = System.IO.File.OpenWrite(filePath))
            using (var uploadedFile = files.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }
            //}

            return Ok(new { count = files });
        }

        public async Task<IActionResult> UploadFiles(List<IFormFile> files, string prno)
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {

                var fileName = System.IO.Path.GetFileName(formFile.FileName);

                // Get file path to be uploaded
                //var path = Path.Combine(Directory.GetCurrentDirectory(), prno);
                var path = Path.Combine(UploadingPathPR, prno);
                var filePath = Path.Combine(UploadingPathPR, prno, fileName);
                //var filePath = "C:\\Wa\\sample\\" + fileName;

                // Check If file with same name exists and delete it
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                else
                {
                    Directory.CreateDirectory(path);
                }

                // Create a new local file and copy contents of uploaded file
                using (var localFile = System.IO.File.OpenWrite(filePath))
                using (var uploadedFile = formFile.OpenReadStream())
                {
                    uploadedFile.CopyTo(localFile);
                }
            }

            return Ok(new { count = files.Count, size });
        }

        // POST: PaymentRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentRequestModel _PaymentRequestModel, List<IFormFile> Files)
        {
            try
            {
                personid = _userManager.GetUserId(this.User);
                _PaymentRequestModel.Paymenttypelist = await _RepositoryUnit.RefPaymentTypeRepository.GetPaymenttypeList(BaseUrlRepo);
                _PaymentRequestModel.tmpPaymentRequestInventory = await _RepositoryUnit.PaymentRequestRepository.GettmpPaymentRequestInventory(BaseUrlRepo);
                _PaymentRequestModel.CompanyList = await _RepositoryUnit.CompanyRepository.GetCompanyTypesAccess(personid,OSPUrlRepo);

                _PaymentRequestModel.DeptList = await _RepositoryUnit.RefDepartmentRepository.GetDeptByPersonID(personid, _PaymentRequestModel.Company, OSPUrlRepo);
                _PaymentRequestModel.Discountlist = await _RepositoryUnit.RefDiscountRepository.GetRefDiscount(BaseUrlRepo);
                _PaymentRequestModel.tmpPaymentRequestInventory = await _RepositoryUnit.PaymentRequestRepository.GettmpPaymentRequestInventoryA(personid, "Auto...", BaseUrlRepo);
                
                ViewData["tmp"] = _PaymentRequestModel.tmpPaymentRequestInventory;
                if (!ModelState.IsValid)
                {

                    return View(_PaymentRequestModel);
                }

                
            

                List<TblVendorpaymethod> bankdetails = new List<TblVendorpaymethod>();
                bankdetails.Add(new TblVendorpaymethod { BankCode = "-1", AcctNo = "" });


                //ServiceUnit _ServiceUnit = new ServiceUnit();
                //_ServiceUnit.UserService.SendEmail();
                //if (!ModelState.IsValid)
                //{


                //    return View(_PaymentRequestModel);
                //}

                TblPaymentrequesthdr _tblprhdr = new TblPaymentrequesthdr();
                // _tblprhdr.PRNo = _PaymentRequestModel.PRNo;
                string companytype = _PaymentRequestModel.Company;
                string code = _PaymentRequestModel.Code;
                string bcode = _PaymentRequestModel.PaymentNetwork;
                _tblprhdr.CompanyCode = _PaymentRequestModel.Companyid;
                _tblprhdr.DeptCode = _PaymentRequestModel.Code;
                _tblprhdr.RequestDate = DateTime.Now;
                _tblprhdr.PayClassCode = _PaymentRequestModel.PaymentClass;
                _tblprhdr.Active = true;
                _tblprhdr.VendorCode = _PaymentRequestModel.Vendorcode;
                _tblprhdr.PayeeName = _PaymentRequestModel.PCVName;
                _tblprhdr.PayMethodType = _PaymentRequestModel.PaymentMethod;
                //_tblprhdr.AcctTypeCode = _PaymentRequestModel.PaymentNetwork;
                _tblprhdr.BankCode = _PaymentRequestModel.PaymentNetwork;
                _tblprhdr.Destination = _PaymentRequestModel.Destination;
                //_tblprhdr.TotalAmount = 10000;

                _tblprhdr.Remarks = _PaymentRequestModel.Remarks;
                _tblprhdr.Void = false;
                _tblprhdr.VoidDate = Convert.ToDateTime("1900-01-01");
                _tblprhdr.VoidUser = " ";
                _tblprhdr.Printed = false;
                _tblprhdr.AuditUser = _PaymentRequestModel.AuditUser;
                _tblprhdr.AuditDate = DateTime.Now;
                _tblprhdr.UploadStat = false;
                _tblprhdr.EditUser = _PaymentRequestModel.AuditUser;
                _tblprhdr.EditDate = DateTime.Now;
                _tblprhdr.TrxMonth = "FEB22";
                _tblprhdr.TrxWeek = 4;
                _tblprhdr.PRNo = "Auto...";
                _tblprhdr.RefNo = _PaymentRequestModel.RefNo;
                TblPaymentrequesthdr tblprhdr = await _RepositoryUnit.PaymentRequestRepository.GetLatestPRRow(_tblprhdr.CompanyCode, _tblprhdr.DeptCode, BaseUrlRepo);
                //string prno = await _RepositoryUnit.PaymentRequestRepository.GetLatestPRRow(_tblprhdr.CompanyCode, _tblprhdr.DeptCode);

                if (tblprhdr != null)
                {
                    var newprno = await _ServiceUnit.PaymentRequestService.GenerateNewPRNo(tblprhdr.PRNo, tblprhdr.CompanyCode, tblprhdr.DeptCode, tblprhdr.AuditDate,BaseUrlService);
                    _tblprhdr.PRNo = newprno;
                    var _trxweek = await _RepositoryUnit.ReftrxweekRepository.Getreftrxweek(tblprhdr.AuditDate,BaseUrlRepo);
                    _tblprhdr.TrxWeek = _trxweek.WeekNo;
                    _tblprhdr.TrxMonth = _trxweek.TrxMonth;
                }
                else
                {
                    var newprno = await _ServiceUnit.PaymentRequestService.GenerateNewPRNo("0", _tblprhdr.CompanyCode, _tblprhdr.DeptCode, _tblprhdr.AuditDate, BaseUrlService);
                    _tblprhdr.PRNo = newprno;
                }


                IList<tmpPaymentRequestInventory> tmp = await _RepositoryUnit.PaymentRequestRepository.GettmpPaymentRequestInventoryA(_tblprhdr.AuditUser, "Auto...", BaseUrlRepo);
                IList<TblPaymentrequestdtl> dtl = new List<TblPaymentrequestdtl>();

                for (int i = 0; i < tmp.Count; i++)
                {
                    _tblprhdr.TotalAmount = tmp[i].TotalAmt + _tblprhdr.TotalAmount;

                    dtl.Add(new TblPaymentrequestdtl
                    {
                   
                        PRNo = _tblprhdr.PRNo,
                        ProductServiceCode = tmp[i].ItemCode,
                        Unit = tmp[i].UOM,
                        Price = tmp[i].Price,
                        Quantity = tmp[i].Qty,
                        Gross = tmp[i].Gross,
                        Discount = tmp[i].Discount,
                        VatRate = tmp[i].VatRate,
                        Vat = tmp[i].Vat,
                        NetofVat = tmp[i].NetofVat,
                        TotalTax = tmp[i].ATC,
                        TotalAmount = tmp[i].TotalAmt,
                        Void = false,
                        AuditUser = _tblprhdr.AuditUser,
                        AuditDate = _tblprhdr.AuditDate,
                        UploadStat = false,
                        EditUser = _tblprhdr.EditUser,
                        EditDate = _tblprhdr.EditDate
                    });

                }

                //  var totamt = await _ServiceUnit.PaymentRequestService.ComputeBreakDown(tmp);
                

                var resp = await _RepositoryUnit.PaymentRequestRepository.PostCreatePaymentRequestHdr(_tblprhdr, BaseUrlRepo);
                resp = await _RepositoryUnit.PaymentRequestRepository.CreatePaymentRequestDtl(dtl, BaseUrlRepo);
                await UploadFiles(Files, _tblprhdr.PRNo);

                //TblResponse _resp = await _RepositoryUnit.PRAuthorizationRepository.CreatePRAuthorization("", _tblprhdr.PRNo);

                return RedirectToAction("CreatePRAuthorization", "Authorization", new { prno = _tblprhdr.PRNo });
                //return RedirectToAction("ViewPR", "PaymentRequest", new { prno = _tblprhdr.PRNo });
                // return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage });
            }
            catch (Exception ex)
            {
                string exs = ex.Message;
                return View(_PaymentRequestModel);
            }
        }

        // GET: PaymentRequest/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PaymentRequest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PaymentRequest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PaymentRequest/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        //[HttpGet]
        //public async Task<ActionResult<IList<RefBranch>>> GetBranchlist(string branch,string company)
        //{
        //    try
        //    {
        //        //string company = HttpContext.Request.Query["company"].ToString();
        //        //string branch = HttpContext.Request.Query["branch"].ToString();
        //        var config1 = configuration["TestRepository"];

        //        _logger.LogInformation("Fetching - " + Utilities.GetmethodName() + " ");
        //        IList<RefBranch> _RefBranch = await _RepositoryUnit.BranchRepository.GetBranches(config1,company,branch);

        //        _logger.LogInformation("Success - " + Utilities.GetmethodName() + "");

        //        return Json(new SelectList(_RefBranch, "BranchDesc", "BranchCode"));
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage = "Fetching Failed - " + Utilities.GetmethodName() + "";

        //        _logger.LogError(ex, errorMessage);
        //        TempData["Msgbox"] = ex.Message;
        //        return View();
        //    }

        //}

        [HttpGet]
        public async Task<ActionResult<IList<RefPaymentClass>>> GetPaymentTypes(string paydesc)
        {
            try
            {
                //string company = HttpContext.Request.Query["company"].ToString();
                //string branch = HttpContext.Request.Query["branch"].ToString();
                var config1 = _configuration["TestRepository"];

                _logger.LogInformation("Fetching - " + Utilities.GetmethodName() + " ");
                IList<RefPaymentClass> _RefPaymenttype = await _RepositoryUnit.RefPaymentTypeRepository.GetPaymentTypes(config1, paydesc);

                _logger.LogInformation("Success - " + Utilities.GetmethodName() + "");

                return Json(new SelectList(_RefPaymenttype, "PayDesc", "PayTypeCode"));
                //return Json(_RefBranch);
            }
            catch (Exception ex)
            {
                errorMessage = "Fetching Failed - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return View();
            }

        }

        [HttpGet]
        public async Task<ActionResult<IList<RefBranch>>> GetBranches(string company)
        {
            try
            {
                //string company = HttpContext.Request.Query["company"].ToString();
                //string branch = HttpContext.Request.Query["branch"].ToString();
                var config1 = _configuration["TestRepository"];

                _logger.LogInformation("Fetching - " + Utilities.GetmethodName() + " ");
                IList<RefBranch> _RefBranch = await _RepositoryUnit.BranchRepository.GetBranches(config1, company, "");

                _logger.LogInformation("Success - " + Utilities.GetmethodName() + "");

                //return Json(new SelectList(_RefBranch, "BranchDesc", "BranchCode"));
                return Json(_RefBranch, new JsonSerializerOptions());
                //return Json(_RefBranch);
            }
            catch (Exception ex)
            {
                errorMessage = "Fetching Failed - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return View();
            }

        }

        [HttpGet]
        public async Task<ActionResult<IList<RefDepartment>>> GetDeptByPersonID(string personid, string companytype)
        {
            try
            {
                //string company = HttpContext.Request.Query["company"].ToString();
                //string branch = HttpContext.Request.Query["branch"].ToString();
                var config1 = _configuration["TestRepository"];

                _logger.LogInformation("Fetching - " + Utilities.GetmethodName() + " ");
                IList<RefDepartment> _RefBranch = await _RepositoryUnit.RefDepartmentRepository.GetDeptByPersonID(personid, companytype,OSPUrlRepo);

                _logger.LogInformation("Success - " + Utilities.GetmethodName() + "");

                //return Json(new SelectList(_RefBranch, "BranchDesc", "BranchCode"));
                return Json(_RefBranch, new JsonSerializerOptions());
                //return Json(_RefBranch);
            }
            catch (Exception ex)
            {
                errorMessage = "Fetching Failed - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return View();
            }

        }


        [HttpGet]
        public async Task<ActionResult<IList<RefBranch>>> GetBranchesbyPersonID(string personid)
        {
            try
            {
                //string company = HttpContext.Request.Query["company"].ToString();
                //string branch = HttpContext.Request.Query["branch"].ToString();
                var config1 = _configuration["TestRepository"];

                _logger.LogInformation("Fetching - " + Utilities.GetmethodName() + " ");
                IList<RefBranch> _RefBranch = await _RepositoryUnit.BranchRepository.GetBranchesbyPersonID(config1, personid);

                _logger.LogInformation("Success - " + Utilities.GetmethodName() + "");

                //return Json(new SelectList(_RefBranch, "BranchDesc", "BranchCode"));
                return Json(_RefBranch, new JsonSerializerOptions());
                //return Json(_RefBranch);
            }
            catch (Exception ex)
            {
                errorMessage = "Fetching Failed - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return View();
            }

        }


        [HttpGet]
        public async Task<ActionResult<IList<RefChapel>>> GetChapels(string branch, string company)
        {
            try
            {
                //string company = HttpContext.Request.Query["company"].ToString();
                //string branch = HttpContext.Request.Query["branch"].ToString();
                var config1 = _configuration["TestRepository"];

                _logger.LogInformation("Fetching - " + Utilities.GetmethodName() + " ");
                IList<RefChapel> refChapels = await _RepositoryUnit.ChapelRepository.GetChapels(config1, company, branch);

                _logger.LogInformation("Success - " + Utilities.GetmethodName() + "");

                return Json(new SelectList(refChapels, "ChapelDesc", "ChapelCode"));
                //return Json(refChapels);

            }
            catch (Exception ex)
            {
                errorMessage = "Fetching Failed - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return View();
            }

        }

        [HttpGet]
        public async Task<ActionResult<IList<RefChapel>>> GetChapelsbyPersonID(string personid)
        {
            try
            {
                //string company = HttpContext.Request.Query["company"].ToString();
                //string branch = HttpContext.Request.Query["branch"].ToString();
                var config1 = _configuration["TestRepository"];

                _logger.LogInformation("Fetching - " + Utilities.GetmethodName() + " ");
                IList<RefChapel> refChapels = await _RepositoryUnit.ChapelRepository.GetChapelsbyPersonID(config1, personid);

                _logger.LogInformation("Success - " + Utilities.GetmethodName() + "");

                //return Json(new SelectList(refChapels, "ChapelDesc", "ChapelCode"));
                return Json(refChapels, new JsonSerializerOptions());
                //return Json(refChapels);

            }
            catch (Exception ex)
            {
                errorMessage = "Fetching Failed - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return View();
            }

        }

        //public  JsonResult GetBranchDetails(string branch,string company)
        //{
        //    try
        //    {
        //        var refe= "";
        //        if (company=="CHAPELS")
        //        {
        //            refe =  "b1 l1";
        //        }
        //        else
        //        {
        //            refe =  "b2 l2";
        //        }
        //        return Json(refe);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }


        //}

        public async Task<string> Get1234()
        {
            var s = "1234";
            return await Task.FromResult(s);
        }

        [HttpGet]
        public async Task<JsonResult> GetBranchdetails(string company, string branchcode)
        {
            try
            {
                _logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                qryBranch vlist = await _RepositoryUnit.BranchRepository.GetBranchdetails(company, branchcode, OSPUrlRepo);



                //return vlist;
                return Json(vlist, new JsonSerializerOptions());
                // return 
            }
            catch (Exception ex)
            {

                errorMessage = "Fetching Failed - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return Json(new { result = false, error = ex.Message });
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetVendorAcctNo(string vendorcode)
        {
            try
            {
                _logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                IList<TblVendorpaymethod> vlist = await _RepositoryUnit.VendorRepository.GetVendorAcctNo("", vendorcode);



                //return vlist;
                return Json(vlist, new JsonSerializerOptions());
                // return 
            }
            catch (Exception ex)
            {

                errorMessage = "Fetching Failed - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return Json(new { result = false, error = ex.Message });
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetVendorAcctNo1(string vendorcode)
        {
            try
            {
                _logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                TblVendorpaymethod vlist = await _RepositoryUnit.VendorRepository.GetVendorAcctNo1("", vendorcode);



                //return vlist;
                return Json(vlist, new JsonSerializerOptions());
                // return 
            }
            catch (Exception ex)
            {

                errorMessage = "Fetching Failed - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return Json(new { result = false, error = ex.Message });
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetVendorAcctNowithAcctno(string vendorcode, string acctno)
        {
            try
            {
                _logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                IList<TblVendorpaymethod> vlist = await _RepositoryUnit.VendorRepository.GetVendorAcctNo("", vendorcode);

                //var list = vlist.Where(f => f.AcctNo == acctno && f.VendorCode== vendorcode).Select(n);
                // var list = from c in vlist where c.VendorCode==vendorcode && c.AcctNo==acctno select c ;
                var resultsvlist = vlist.Where(c => c.VendorCode == vendorcode && c.AcctNo == acctno).FirstOrDefault();
                //return vlist;
                return Json(resultsvlist, new JsonSerializerOptions());
                // return 
            }
            catch (Exception ex)
            {

                errorMessage = "Fetching Failed - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return Json(new { result = false, error = ex.Message });
            }

        }

        [HttpGet]
        public async Task<ActionResult<IList<qryVendorList>>> GetVendorList()
        {
            try
            {
                _logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                IList<qryVendorList> vlist = await _RepositoryUnit.VendorRepository.GetVendorLists("");



                //return vlist;
                //return Json(vlist, new JsonSerializerOptions());
                return Json(new SelectList(vlist, "VendorName", "VendorCode"));
                // return 
            }
            catch (Exception ex)
            {

                errorMessage = "Fetching Failed - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return Json(new { result = false, error = ex.Message });
            }

        }

        [HttpGet]
        public async Task<ActionResult<IList<qryVendorList>>> GetVendorLists1(string vendordesc, string paymentclass)
        {
            try
            {
                _logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                IList<qryVendorList> vlist = await _RepositoryUnit.VendorRepository.GetVendorLists1("", vendordesc, paymentclass);



                //return vlist;
                return Json(vlist, new JsonSerializerOptions());
                ////return Json(new SelectList(vlist, "VendorName", "VendorCode"));
                // return 
            }
            catch (Exception ex)
            {

                errorMessage = "Fetching Failed - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                //return Json(new { result = false, error = ex.Message });
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public async Task<ActionResult<IList<TblVendorItems>>> GetVendorItemsList(string vendorcode)
        {
            try
            {
                _logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                IList<TblVendorItems> vlist = await _RepositoryUnit.VendorRepository.GetVendorItemsList("", vendorcode);



                //return vlist;
                //return Json(vlist, new JsonSerializerOptions());
                return Json(new SelectList(vlist, "ItemDesc", "ItemCode"));
                // return 
            }
            catch (Exception ex)
            {

                errorMessage = "Fetching Failed - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return Json(new { result = false, error = ex.Message });
            }

        }


        public async Task<JsonResult> GetVendorItemsList1(string vendorcode, string itemdesc)
        {
            try
            {
                if (itemdesc is null)
                {
                    itemdesc = "";
                }
                _logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.VendorRepository.GetVendorItemsList1("", vendorcode, itemdesc);


                var resultsvlist = vlist.Where(c => c.ItemDesc == itemdesc).FirstOrDefault();
                //return vlist;
                return Json(vlist, new JsonSerializerOptions());
                //return Json(new SelectList(vlist, "ItemDesc", "ItemCode"));
                // return 
            }
            catch (Exception ex)
            {

                errorMessage = "Fetching Failed - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return Json(new { result = false, error = ex.Message });
            }

        }

        public async Task<JsonResult> GetVendorItemsDetails(string vendorcode, string itemdesc)
        {


            try
            {
                if (itemdesc is null)
                {
                    itemdesc = "";
                }

                _logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.VendorRepository.GetVendorItemsDetails("", vendorcode, itemdesc);


                //var resultsvlist = vlist.Where(c => c.ItemDesc == itemdesc).FirstOrDefault();
                //return vlist;
                return Json(vlist, new JsonSerializerOptions());
                //return Json(new SelectList(vlist, "ItemDesc", "ItemCode"));
                // return 
            }
            catch (Exception ex)
            {

                errorMessage = "Fetching Failed - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return Json(new { result = false, error = ex.Message });
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetTin(string company)
        {
            // string company = HttpContext.Request.Query["term"].ToString();
            var result = await Get1234();

            //return Json(new SelectList(result));
            return Json(result);
        }


        [HttpGet]
        public async Task<ActionResult> GetCompanyTypes(string company)
        {
            try
            {
                _logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var config1 = _configuration["TestRepository"];
                //string company = HttpContext.Request.Query["term"].ToString();
                IList<qryCompanyType> result = await _RepositoryUnit.CompanyRepository.GetCompanyTypes(company, OSPUrlRepo);
                if (!result.Any())
                {
                    return NotFound(company);
                }
                //return Json(new SelectList(result));
                // return Json(new SelectList(result, "CompanyType", "CompanyType"));
                return Json(result, new JsonSerializerOptions());
                //return Json(result);
            }
            catch (Exception ex)
            {

                errorMessage = "Fetching Failed - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return Json(new { result = false, error = ex.Message });
            }

        }

        [HttpGet]
        public async Task<ActionResult> SearchCompany()
        {
            try
            {
                _logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var config1 = _configuration["TestRepository"];
                string company = HttpContext.Request.Query["term"].ToString();
                IList<RefCompany> result = await _RepositoryUnit.CompanyRepository.SearchCompany(company, OSPUrlRepo);
                if (!result.Any())
                {
                    return NotFound(company);
                }
                //return Json(new SelectList(result));
                return Json(new SelectList(result, "CompanyDesc", "CompanyType"));
                //return Json(result);
            }
            catch (Exception ex)
            {

                errorMessage = "Fetching Failed - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return Json(new { result = false, error = ex.Message });
            }

        }

        [HttpGet]
        public async Task<ActionResult<IList<RefCompany>>> GetCompanylist()
        {
            try
            {
                string company = HttpContext.Request.Query["term"].ToString();
                var config1 = _configuration["TestRepository"];

                _logger.LogInformation("Fetching - " + Utilities.GetmethodName() + " ");

                IList<RefCompany> _refComp = await _RepositoryUnit.CompanyRepository.GetCompanylist(config1, company);

                _logger.LogInformation("Success - " + Utilities.GetmethodName() + "");

                return Json(new SelectList(_refComp, "CompanyDesc", "CompanyCode"));
            }
            catch (Exception ex)
            {
                errorMessage = "Fetching Failed - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return View();
            }

        }

        [HttpGet]
        public async Task<ActionResult<IList<RefCompany>>> GetCompanylist1()
        {
            try
            {
                string company = HttpContext.Request.Query["term"].ToString();
                var config1 = _configuration["TestRepository"];

                _logger.LogInformation("Fetching - " + Utilities.GetmethodName() + " ");

                IList<RefCompany> _refComp = await _RepositoryUnit.CompanyRepository.GetCompanylist1(OSPUrlRepo);

                _logger.LogInformation("Success - " + Utilities.GetmethodName() + "");

                //return Json(new SelectList(_refComp, "CompanyDesc", "CompanyCode"));

                //var Name = (from N in _refComp
                //            where N.CompanyDesc.StartsWith(company)
                //            select N).ToList();

                var filteredList = _refComp.Where(s => s.CompanyDesc.Contains(company)).ToList();
                // string json = serieli(filteredList);


                //return Ok(json);
                return Json(new SelectList(_refComp, "CompanyDesc", "CompanyCode"));
            }
            catch (Exception ex)
            {
                errorMessage = "Fetching Failed - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return View();
            }

        }

        public async Task<IList<SelectListItem>> GetCompanies()
        {


            IList<RefCompany> comp = await _RepositoryUnit.CompanyRepository.GetCompanies(OSPUrlRepo);

            List<SelectListItem> companies = new List<SelectListItem>();
            foreach (var item in comp)
            {
                companies.Add(new SelectListItem { Text = item.CompanyDesc.ToString(), Value = item.CompanyCode.ToString() });
            }
            //return Json(new SelectList(_vendor, "BranchDesc", "Branchcode"));


            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "Select Company"
            };
            companies.Insert(0, defItem);
            return companies;
        }

        [HttpPost]
        public async Task<ActionResult> CreatetmpPaymentRequestInventory(tmpPaymentRequestInventory _tmp)
        {
            try
            {
                _logger.LogInformation("Posting - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                tmpPaymentRequestInventory _tmpPaymentRequestInventory = new tmpPaymentRequestInventory();
                _tmpPaymentRequestInventory.ItemCode = _tmp.ItemCode;
                _tmpPaymentRequestInventory.ItemDesc = _tmp.ItemDesc;
                //_tmpPaymentRequestInventory.Qty = qty;
                //_tmpPaymentRequestInventory.Price = price;

                //var response = await UtilitiesHttpClient<TblResponse>.PostAsync<tmpPaymentRequestInventory>("", _tmpPaymentRequestInventory);
                //TblResponse _resp = await _RepositoryUnit.PaymentRequestRepository.PosttmpPaymentRequestInventory(_tmp);
                //if (response.IsSuccessStatusCode)
                //{
                //    // Handle success
                //    return Ok();
                //}
                //else
                //{
                //    // Handle error
                //    return BadRequest();
                //}
                return Ok();
            }
            catch (Exception ex)
            {
                errorMessage = "Posting Failed -  " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetVendorDetails(string vendorcode, string payclass)
        {
            try
            {
                _logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                qryVendorDetails vlist = await _RepositoryUnit.VendorRepository.GetVendorDetails("", vendorcode, payclass);

                //return vlist;
                return Json(vlist, new JsonSerializerOptions());
                // return 
            }
            catch (Exception ex)
            {

                errorMessage = "Fetching Failed - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "";

                _logger.LogError(ex, errorMessage);
                TempData["Msgbox"] = ex.Message;
                return Json(new { result = false, error = ex.Message });
            }







        }



        public IActionResult ImageView(string filename, string prno)
        {


            try
            {
                var baseUri = $"{Request.Scheme}://{Request.Host}";

                // Replace this with your actual image URL
                //string imageUrl = "https://upload.wikimedia.org/wikipedia/commons/4/4f/Black_hole_-_Messier_87_crop_max_res.jpg";
                // imageUrl = @"\\splpdevserver\spasv2$\PaymentRequest\SPLPICUBAO2309-000018\1123.png";
                //string filename = "1123.png";
                //string imageUrl = Path.Combine(UploadingPathPR + "\\SPLPICUBAO2309-000018\\" + filename);
                //return View("ImageView", imageUrl);

                string imageUrl = @"\\splpdevserver\spasv2$\PaymentRequest\" + prno + @"\" + filename;

                // Read the file from the local drive
                var imageBytes = System.IO.File.ReadAllBytes(imageUrl);

                // Determine the content type (e.g., image/jpeg for a JPEG image)
                string contentType = "image/jpeg";

                // Return the image as a FileResult
                return File(imageBytes, contentType);
            }
            catch (Exception ex)
            {
                // Handle exceptions if the file cannot be found or read
                // You can return an error view or redirect to an error page
                return Content($"Error: {ex.Message}");
            }


        }

        public async Task<IActionResult> DownloadImg(string filename, string prno)
        {
            if (filename == null)
                return Content("filename is not availble");



            // return RedirectToAction("ImageView", "PaymentRequest", new { imageUrl = imageUrl });

            //var path = Path.Combine(Directory.GetCurrentDirectory(), "upload", filename);
            string path = Path.Combine(UploadingPathPR + "\\" + prno, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }


    }


}
