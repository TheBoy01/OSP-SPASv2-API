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
using Microsoft.Extensions.Options;
using NuGet.Packaging;
using System.Text;
using ClosedXML.Excel;
using System.Data;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Runtime.CompilerServices;
using OSP.SPASv2.Domain.Tables;
using System.Web.Helpers;
using DocumentFormat.OpenXml.Spreadsheet;
using Irony.Parsing.Construction;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Reflection;
using System.Linq;
using AspNetCore;

namespace OSP.SPASv2.Web.Controllers
{
    public class RequisitionController : Controller
    {
        private readonly ILogger<AuthorizationController> _AuthLogger;
        private ILogger<RequisitionController> _logger;
        private ServiceUnit _ServiceUnit;
        private UserManager<OSPSPASv2ApplicationUser> _userManager;
        private RepositoryUnit _RepositoryUnit;
        public string errorMessage = "";
        private Config _config;

        private IConfiguration configuration;
        private IHostEnvironment _environment;

        string personid;
        string UploadingPathPR;
        private string WebRootPath;
        string BaseUrlRepo;
        string BaseUrlService;
        string OSPUrlRepo;
        string OSPUrlService;
        decimal VatRate = 1.12m;
        private object _response;

        public string Req { get; private set; }

        public RequisitionController(ILogger<RequisitionController> logger, IConfiguration _configuration
            , IHostEnvironment environment, UserManager<OSPSPASv2ApplicationUser> userManager, IOptions<Config> config)
        {

            _logger = logger;
            _RepositoryUnit = new RepositoryUnit();

            configuration = _configuration;
            _environment = environment;
            _ServiceUnit = new ServiceUnit();
            this._userManager = userManager;
            _config = config.Value;


            BaseUrlRepo = _configuration.GetSection("APIBaseURL")["SPASv2.Repository"];
            BaseUrlService = _configuration.GetSection("APIBaseURL")["SPASv2.Service"];
            OSPUrlRepo = _configuration.GetSection("APIBaseURLCommon")["Common.Repository"];
            OSPUrlService = _configuration.GetSection("APIBaseURLCommon")["Common.Service"];
            //  UploadingPathPR = _config.Environment + / "UploadingPath:PaymentRequest";
            UploadingPathPR = _configuration.GetSection("UploadingPath")["PaymentRequest"];

            this.WebRootPath = _environment.ContentRootPath + "\\wwwroot";

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SetDocs(List<IFormFile> files)
        {
            RequisitionViewModel model = new RequisitionViewModel();

            model.Files = new List<FileDetails>();
            model.PRNo = "0";
            model.SupDocsListName = "PYDocsNew";
            foreach (var item in files)
            {
                string filename = Path.GetFileName(item.FileName);
                model.Files.Add(
                    new FileDetails()
                    { ReqNo = "0", Name = filename });
            }

            return View("_RequisitionDocs", model);
        }

        public async Task<IActionResult> GetCreatePaymentForm(string ReqNo)
        {
            personid = _userManager.GetUserId(this.User);

            RequisitionViewModel model = new RequisitionViewModel();
            model.dashboardViewModel = new DashBoardViewModel();
            model.dashboardViewModel.RequestNo = ReqNo;
            model.isCreatePayment = true;

            model.RequisitionInfo = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionInfo(BaseUrlRepo, ReqNo);

            model.dashboardViewModel.RequestJourney = await _RepositoryUnit.PRAuthorizationRepository.GetPaymentRequestAuthListByPRNO(model.dashboardViewModel.RequestNo, BaseUrlRepo);

            model.dashboardViewModel.isSearch = false;

            if (model.dashboardViewModel.RequestJourney.Count > 0)
            {
                model.PaymentRequestHdr = await _RepositoryUnit.PaymentRequestRepository.GetPaymentRequestHdr(model.dashboardViewModel.RequestNo, BaseUrlRepo);
            }

            model.CompanyList = await _RepositoryUnit.CompanyRepository.GetCompanyTypesAccess(personid, OSPUrlRepo);
            model.Company = model.CompanyList.Select(m => m.CompanyType).FirstOrDefault();

            model.DeptList = await _RepositoryUnit.RefDepartmentRepository.GetDeptByPersonID(personid, model.Company, OSPUrlRepo);
            model.Code = model.DeptList.Select(m => m.DeptCode).FirstOrDefault();
            model.Discountlist = await _RepositoryUnit.RefDiscountRepository.GetRefDiscount(BaseUrlRepo);

            model.RequisitionItemList = new List<qryRequisitionItem>();
            model.RequisitionItemList.AddRange(await _RepositoryUnit.TblRequisitionRepository.GetRequisitionItemList(BaseUrlRepo, ReqNo));

            model.VendorItemList = new List<TblVendorItems>();

            foreach (var item in model.RequisitionItemList.Where(a => a.ReqNo == model.RequisitionInfo.MainReqNo))
            {
                int MainQty = model.RequisitionItemList.Where(t => t.ReqNo == model.RequisitionInfo.MainReqNo && t.Item == item.Item).Sum(t => t.Quantity);
                int ItemQty = model.RequisitionItemList.Where(t => t.ReqNo != model.RequisitionInfo.MainReqNo && t.Item == item.Item).Sum(t => t.Quantity);
                int Bal = MainQty - ItemQty;

                if (Bal <= 0)
                {
                    continue;
                }

                TblVendorItems _vitem = new TblVendorItems()
                {
                    ItemCode = item.ItemCode,
                    ItemDesc = item.Item,
                    UOM = item.Unit,
                    Amount = item.Price
                };

                model.VendorItemList.Add(_vitem);
            }

            model.MainReqNo = model.RequisitionInfo.MainReqNo;


            return View("_CreatePaymentForm", model);
        }

        public async Task<IActionResult> SearchPO(string PONo)
        {
            personid = _userManager.GetUserId(this.User);
            RequisitionViewModel model = new RequisitionViewModel();
            model.dashboardViewModel = new DashBoardViewModel();
            model.dashboardViewModel.RequestList = await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-POREQUESTER", PONo, BaseUrlRepo);

            model.ListCheck = false;
            model.ListHidenColumns.Add("BatchNo");
            return View("_RequisitionList", model);
        }

        public async Task<IActionResult> GenerateItemList(string item, string vendorcode, decimal deduction, decimal freight, bool isCreatePayment, string payclass)
        {
            RequisitionViewModel model = new RequisitionViewModel();
            model.RequisitionInfo.Deduction = deduction;
            model.Freight = freight;
            model.isCreatePayment = isCreatePayment;
            model.PaymentClass = payclass;
            if (deduction > 0 || freight > 0)
            {
                model.isCreatePayment = true;
            }
            model = await setCreateItemList(item, vendorcode, model);
            
            return View("_RequisitionItemsList", model);
        }

        private async Task<RequisitionViewModel> setCreateItemList(string jsonItem, string vendorcode, RequisitionViewModel model)
        {
            model.RequisitionItemList = JsonConvert.DeserializeObject<List<qryRequisitionItem>>(jsonItem);
            foreach (var reqItem in model.RequisitionItemList)
            {
                await SetRequestItemBreakdown(reqItem, vendorcode, model.PaymentClass);
            }
            model.showAddItemButton = true;
            model.ItemListName = "newReq";

            return model;
        }

        public async Task<JsonResult> GetVendorDetails(string vendorcode, string payclass)
        {
            try
            {
                qryVendorDetails vendordtl = await _RepositoryUnit.VendorRepository.GetVendorDetails(BaseUrlRepo, vendorcode, payclass);
                IList<TblVendorItems> itemList = await _RepositoryUnit.VendorRepository.GetVendorItemsList(BaseUrlRepo, vendorcode);


                return Json(new { vendordtl = vendordtl, itemList = itemList }, new JsonSerializerOptions());
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IList<qryVendorList>>> GetVendorLists(string vendordesc, string paymentclass)
        {
            try
            {
                IList<qryVendorList> vlist = await _RepositoryUnit.VendorRepository.GetVendorLists1(BaseUrlRepo, vendordesc, paymentclass);

                RefPaymentClass refPaymentClass = await _RepositoryUnit.RefPaymentTypeRepository.PaymentClassDetails(BaseUrlRepo, paymentclass);

                return Json(new { data = vlist, ClassDesc = refPaymentClass.ReqDesc }, new JsonSerializerOptions());
            }
            catch (Exception ex)
            {
                return Json(new { errorMessage = ex.Message }, new JsonSerializerOptions());
            }

        }

        public async Task<IActionResult> ExportDataTabletoExcelAsync(string htmltable)
        {
            string[] lstPO = htmltable.Split(',');

            List<string> list = (new List<string>(lstPO.Cast<string>().Distinct()));

            // Create a new DataTable.    
            DataTable custTable = new DataTable("Request Payment Template");
            custTable.Columns.Add(new DataColumn("PONo", typeof(string)));
            custTable.Columns.Add(new DataColumn("VendorName", typeof(string)));
            custTable.Columns.Add(new DataColumn("Department", typeof(string)));
            custTable.Columns.Add(new DataColumn("Item", typeof(string)));
            custTable.Columns.Add(new DataColumn("Orig Order", typeof(int)));
            custTable.Columns.Add(new DataColumn("Paid Qty", typeof(int)));
            custTable.Columns.Add(new DataColumn("Balance", typeof(int)));
            custTable.Columns.Add(new DataColumn() { ColumnName = "S.I. Qty", DataType = typeof(int), AllowDBNull = true });
            custTable.Columns.Add(new DataColumn("Reference Reciept", typeof(string)));
            custTable.Columns.Add(new DataColumn("D.R. No.", typeof(string)));
            custTable.Columns.Add(new DataColumn("D.R. Date", typeof(string)));
            custTable.Columns.Add(new DataColumn("S.I. No.", typeof(string)));
            custTable.Columns.Add(new DataColumn("S.I. Date", typeof(string)));
            custTable.Columns.Add(new DataColumn() { ColumnName = "Balance Amount", DataType = typeof(decimal), AllowDBNull = true });
            custTable.Columns.Add(new DataColumn() { ColumnName = "HP Deduction", DataType = typeof(decimal), AllowDBNull = true });

            foreach (var pono in list)
            {
                if (string.IsNullOrEmpty(pono))
                {
                    continue;
                }

                TblPurchaseorderhdr _PO = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(BaseUrlRepo, pono);

                IList<qryRequisitionItem> _MainReqItem = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionItemList(BaseUrlRepo, _PO.Reqno);

                List<qryRequisitionItem> _ReqItemList = new List<qryRequisitionItem>();
                _ReqItemList.AddRange(_MainReqItem);

                IList<qryRequisitionInfo> _PyReqList = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionListInfoByMainReqNo(BaseUrlRepo, _PO.Reqno);
                qryRequisitionInfo _ReqInfo = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionInfo(BaseUrlRepo, _PO.Reqno);

                foreach (var pyReq in _PyReqList)
                {
                    _ReqItemList.AddRange(await _RepositoryUnit.TblRequisitionRepository.GetRequisitionItemList(BaseUrlRepo, pyReq.ReqNo));
                }


                foreach (var _reqitem in _MainReqItem)
                {

                    int ItemQty = _ReqItemList.Where(t => t.ReqNo != _PO.Reqno && t.Item == _reqitem.Item && _reqitem.isDeduct).Sum(t => t.Quantity);

                    string department = _reqitem.CompanyType + "-" + _reqitem.DeptCode;
                    string vendor = _ReqInfo.Vendor;
                    string item = _reqitem.Item;
                    string balance = (Convert.ToInt32(_reqitem.Quantity) - ItemQty).ToString();
                    object QtyOrdered = Convert.ToInt32(_reqitem.Quantity);
                    object QtyDelivered = Convert.ToInt32(ItemQty);
                    object quantity = null;
                    string refno = string.Empty;
                    string drno = string.Empty;
                    string drdate = string.Empty;
                    string sino = string.Empty;
                    string sidate = string.Empty;
                    object balanceAmt = ((Convert.ToInt32(_reqitem.Quantity) - ItemQty) * _reqitem.Price);
                    object hpDeduction = null;


                    custTable.Rows.Add(pono, vendor, department,
                     item,
                     QtyOrdered,
                     QtyDelivered,
                     balance,
                     quantity,
                     refno,
                     drno,
                     drdate,
                     sino,
                     sidate,
                     balanceAmt,
                     hpDeduction
                        );
                }


            }


            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(custTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Payment Request Template.xlsx");
                }
            }
        }
        public async Task<DataTable> CreateDataTable(string[] POList)
        {
            List<string> list = (new List<string>(POList.Cast<string>().Distinct()));

            // Create a new DataTable.    
            DataTable custTable = new DataTable("Request Payment Template");
            custTable.Columns.Add(new DataColumn("PONo", typeof(string)));
            custTable.Columns.Add(new DataColumn("Department", typeof(string)));
            custTable.Columns.Add(new DataColumn("Item", typeof(string)));
            custTable.Columns.Add(new DataColumn("Balance", typeof(string)));
            custTable.Columns.Add(new DataColumn("Quantity", typeof(string)));
            custTable.Columns.Add(new DataColumn("Reference Reciept", typeof(string)));
            custTable.Columns.Add(new DataColumn("D.R No.", typeof(string)));
            custTable.Columns.Add(new DataColumn("D.R Date", typeof(string)));
            custTable.Columns.Add(new DataColumn("S.I No.", typeof(string)));
            custTable.Columns.Add(new DataColumn("S.I Date", typeof(string)));


            foreach (var pono in list)
            {
                TblPurchaseorderhdr _PO = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(BaseUrlRepo, pono);

                List<qryRequisitionItem> RequisitionItemList = new List<qryRequisitionItem>();
                RequisitionItemList.AddRange(await _RepositoryUnit.TblRequisitionRepository.GetRequisitionItemList(BaseUrlRepo, _PO.Reqno));

                foreach (var _reqitem in RequisitionItemList)
                {
                    string department = _reqitem.CompanyType + "-" + _reqitem.DeptDesc;
                    string item = _reqitem.Item;
                    string balance = "10";
                    string quantity = _reqitem.Quantity.ToString();
                    string refno = string.Empty;
                    string drno = string.Empty;
                    string drdate = string.Empty;
                    string sino = string.Empty;
                    string sidate = string.Empty;


                    custTable.Rows.Add(pono, department,
                     item,
                     balance,
                     quantity,
                     refno,
                     drno,
                     drdate,
                     sino,
                     sidate
                        );
                }


            }

            return custTable;
        }

        public async Task<IActionResult> ConfirmPaymentRequest(List<IFormFile> files, string vm)
        {
            try
            {
                RequisitionViewModel _viewReq = JsonConvert.DeserializeObject<RequisitionViewModel>(vm);

                foreach (var item in _viewReq.dashboardViewModel.RequestList)
                {


                    TblResponse _resp = new TblResponse();
                    string prno = item.RequestID;

                    List<IFormFile> _reqAttachments = new List<IFormFile>();

                    foreach (var file in files)
                    {
                        string[] filenames = file.FileName.Split('/');
                        string fileReqNo = filenames[1];
                        if (filenames.Count() < 3)
                        {
                            continue;
                        }

                        if (fileReqNo.Equals(prno))
                        {
                            _reqAttachments.Add(file);
                        }
                    }

                    UploadFiles(_reqAttachments, prno, "Confirmation");

                    personid = _userManager.GetUserId(this.User);
                    qryUpdateStatusAuth _qry = new qryUpdateStatusAuth();
                    _qry.StatusType = "AP";
                    _qry.PRRefNo = prno;
                    _qry.PersonID = "REQUESTER-VAL";
                    TblResponse response = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qry, BaseUrlRepo);
                }

                return Json(new { success = "", errormsg = "" });
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public void UploadFiles(List<IFormFile> files, string prno, string folderName = "")
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {

                var fileName = System.IO.Path.GetFileName(formFile.FileName);

                // Get file path to be uploaded
                //var path = Path.Combine(Directory.GetCurrentDirectory(), prno);
                var path = Path.Combine(UploadingPathPR, prno, folderName);
                path = path + @"\";
                var filePath = Path.Combine(UploadingPathPR, prno, folderName, "Confirmation-" + fileName);

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
        }

        public async Task<IActionResult> ReadConfirmationAttachments(List<IFormFile> files)
        {
            RequisitionViewModel model = new RequisitionViewModel();

            model.RequestAttachments = new List<qryRequestAttachments>();
            model.dashboardViewModel = new DashBoardViewModel();
            model.dashboardViewModel.RequestList = new List<qryRequestPaymentRequestbyStatus>();
            model.RequestAttachments = new List<qryRequestAttachments>();
            //IList<string> list1 = JsonConvert.DeserializeObject<List<string>>(list);

            foreach (var file in files)
            {
                string[] filenames = file.FileName.Split('/');

                if (filenames.Length > 3)
                {
                    continue;
                }
                string RequestNo = filenames[1];

                qryRequisitionInfo _ReqInfo = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionInfo(BaseUrlRepo, RequestNo);

                if (model.dashboardViewModel.RequestList.Where(t => t.RequestID == RequestNo).FirstOrDefault() == null)
                {
                    if (_ReqInfo != null)
                    {
                        IList<qryRequisitionItem> _ReqItems = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionItemList(BaseUrlRepo, _ReqInfo.ReqNo);

                        qryRequestPaymentRequestbyStatus _req = new qryRequestPaymentRequestbyStatus()
                        {
                            RequestID = _ReqInfo.ReqNo,
                            PONo = _ReqInfo.PONo,
                            Amount = _ReqItems.Sum(t => t.TotalAmount),
                            Status = _ReqInfo.Status,
                            Vendor = _ReqInfo.Vendor,
                            RequestDate = DateTime.Now,
                            PayClass = _ReqInfo.PayClass,
                            CompanyType = _ReqInfo.RequesterCompanyType,
                            DeptDesc = _ReqInfo.RequesterDepartment,
                            PayMethodType = _ReqInfo.PayMethod,
                            BatchPRNo = ""
                        };

                        model.dashboardViewModel.RequestList.Add(_req);
                    }
                }

                string filename = Path.GetFileName(file.FileName);
                model.RequestAttachments.Add(new qryRequestAttachments() { RequestNo = _ReqInfo.ReqNo, FileName = filename });

            }
            model.ListHidenColumns = new List<string>()
                                { "BatchNo","Actions","PayClass","RequestDate","PONo","Status"};
            return View("_RequisitionList", model);
        }
        public IActionResult Confirmation()
        {
            return View();
        }
        public async Task<IActionResult> List()
        {

            personid = _userManager.GetUserId(this.User);
            RequisitionViewModel model = new RequisitionViewModel();
            model.dashboardViewModel = new DashBoardViewModel();
            model.value = BaseUrlRepo + "/Ron/GetRequestPaymentRequestbyStatus";
            model.dashboardViewModel.RequestList = await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-REQUESTER", personid, BaseUrlRepo);
            model.dashboardViewModel.RequestDepartmentList = await _RepositoryUnit.PRAuthorizationRepository.GetRequisitionDepartment(personid, BaseUrlRepo);
            //model.value = BaseUrlRepo;
            model.value1 = BaseUrlService;
            model.value2 = OSPUrlRepo;
            model.value3 = OSPUrlService;


            //await _RepositoryUnit.BranchRepository.GetPaymentTypeList_Jon();

            return View("SingleRequisitionList", model);
        }

        public async Task<IActionResult> VerifyRequest(string ReqNo, bool isRush, string RushReason, string RushRemarks)
        {
            string _UserID = _userManager.GetUserId(this.User);

            qryUpdateStatusAuth _qryUpdateStatusAuth = new qryUpdateStatusAuth()
            {
                PRRefNo = ReqNo,
                PersonID = _UserID,
                StatusType = "AP"
            };
            TblResponse _resp;
            AuthorizationParams authorizationParams = new AuthorizationParams(){};

            if (isRush)
            {
                _qryUpdateStatusAuth.TransType = "RSH";
            }

            _resp = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qryUpdateStatusAuth, BaseUrlRepo);

            authorizationParams.ReqNo.Add(ReqNo);


            //var result = await new AuthorizationController(_AuthLogger, _userManager, configuration, _environment).SendEmailAuthorization_PRNO(ReqNo, _UserID);

            authorizationParams.UserCode = _UserID;


            _resp = await _RepositoryUnit.PRAuthorizationRepository.ProcessAuthorization(authorizationParams, BaseUrlRepo);

            if (isRush)
            {
                TblRequisitionReason _reason = new TblRequisitionReason()
                {
                    ReqNo = ReqNo,
                    ReasonCode = RushReason,
                    Remarks = RushRemarks,
                    AuditUser = _UserID,
                    AuditDate = DateTime.Now,
                };

                await _RepositoryUnit.PRAuthorizationRepository.InsertReqReason(_reason, BaseUrlRepo);
            }

            return Json(new { isVerified = true }, new JsonSerializerOptions());
        }

        public async Task<IActionResult> VoidRequest(string ReqNo,string ReasonCode, string Remarks)
        {
            string _UserID = _userManager.GetUserId(this.User);

            TblRequisitionReason _reason = new TblRequisitionReason()
            {
                ReqNo = ReqNo,
                ReasonCode = ReasonCode,
                Remarks = Remarks,
                AuditUser = _UserID,
                AuditDate = DateTime.Now,
            };

            await _RepositoryUnit.TblRequisitionRepository.PostVoidRequisitionByReqNo(BaseUrlRepo,ReqNo, _UserID);
            await _RepositoryUnit.PRAuthorizationRepository.InsertReqReason(_reason, BaseUrlRepo);


            return Json(new { isVoid = true }, new JsonSerializerOptions());
        }

        public async Task<IActionResult> DisapprovePRAuthorizationALL(string prno, string reason, string Remarks)
        {

            qryUpdateStatusAuth _qryUpdateStatusAuth = new qryUpdateStatusAuth();
            _qryUpdateStatusAuth.PersonID = _userManager.GetUserId(this.User);
            _qryUpdateStatusAuth.StatusType = "DN";


            IList<string> _prno = System.Text.Json.JsonSerializer.Deserialize<IList<string>>(prno);

            try
            {
                foreach (var item in _prno)
                {
                    _qryUpdateStatusAuth.PRRefNo = item;
                    //_resp = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(item, "SPLPI0002", statustype);
                    //this.SendEmailAuthorization_PRNO(item);
                    _qryUpdateStatusAuth.ReqReason = reason;
                    _qryUpdateStatusAuth.ReasonRemarks = reason;

                    TblRequisitionReason _reason = new TblRequisitionReason()
                    {
                        ReqNo = item,
                        ReasonCode = reason,
                        Remarks = Remarks,
                        AuditUser = _qryUpdateStatusAuth.PersonID,
                        AuditDate = DateTime.Now,
                    };

                    await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qryUpdateStatusAuth, BaseUrlRepo);
                    await _RepositoryUnit.PRAuthorizationRepository.InsertReqReason(_reason, BaseUrlRepo);
                }

            }
            catch (Exception ex)
            {

                return Json(new { success = false, errormsg = ex.Message });

            }

            return Json(new { success = true });
        }

        public async Task<IActionResult> CanAddItem(string itemList, string item)
        {
            try
            {
                List<qryRequisitionItem> ItemList = JsonConvert.DeserializeObject<List<qryRequisitionItem>>(itemList);
                qryRequisitionItem Item = JsonConvert.DeserializeObject<qryRequisitionItem>(item);

                if (ItemList.Where(t=> t.ItemCode == Item.ItemCode).ToList().Count > 0)
                {
                    throw new Exception("Item/service is already added!");
                }

                if (Item.Quantity <= 0)
                {
                    throw new Exception("Please enter a valid quantity! Quantity must be greater than zero.");
                }
            }
            catch (Exception err)
            {
                return Json(new { success = false, errorMessage = err.Message }, new JsonSerializerOptions());
            }

            return Json(new { success = true }, new JsonSerializerOptions());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsertRequisitionModel insModel)
        {
            string _UserID = _userManager.GetUserId(this.User);
            personid = _UserID;
            RequisitionViewModel _reqvm = new RequisitionViewModel();
            try
            {
                insModel.RequisitionItemList =
                JsonConvert.DeserializeObject<List<qryRequisitionItem>>
                (HttpContext.Request.Form["RequisitionItemList"].ToString());
            }
            catch (Exception)
            {
                insModel.RequisitionItemList = new List<qryRequisitionItem>();
            }

            insModel.ModelStateError = this.CanCreateRequisition(insModel);

            if (!ModelState.IsValid || insModel.ModelStateError.Count > 0)
            {
                foreach (var item in insModel.ModelStateError)
                {
                    ModelState.AddModelError(item.Key, item.Message);
                }

                _reqvm.isModelValidated = true;
                _reqvm = await SetCreateNewRequisitionModel(_reqvm);

                _reqvm = await SetInputValuesForCreate(insModel, _reqvm);

                return View("CreateRequisition", _reqvm);
            }

            _reqvm = await  SetRequisition(insModel,_reqvm, _UserID);

            BatchUploadParams _params = new BatchUploadParams();

            _params.UserID = _UserID;
            _params.TblRequisitionhdrList = new List<TblRequisitionhdr>();
            _params.TblRequisitiondtlList = new List<TblRequisitiondtl>();

            _params.TblRequisitionhdrList.Add(_reqvm.TblRequisitionhdr);
            _params.TblRequisitiondtlList.AddRange(_reqvm.RequisitionDtlList);

            _params.qryEmployee = await _RepositoryUnit.EmployeeRepository.GetEmployeeDetails(OSPUrlRepo, _UserID);

            _params =  await _RepositoryUnit.TblRequisitionRepository.InsertRequisitionList(BaseUrlRepo,_params);

            //await _RepositoryUnit.TblRequisitionRepository.PostCreateRequisitionHdr(BaseUrlRepo, _reqvm.TblRequisitionhdr);

            //foreach (var _reqdtl in _reqvm.RequisitionDtlList)
            //{
            //    await _RepositoryUnit.TblRequisitionRepository.PostCreateRequisitionDtl(BaseUrlRepo, _reqdtl);
            //}


            //await _RepositoryUnit.PRAuthorizationRepository.CreatePRAuthorization(BaseUrlRepo, BaseUrlRepo, _reqvm.TblRequisitionhdr.Reqno, "PO");
            ////await _RepositoryUnit.TblRequisitionRepository.CreateLoanHdr(BaseUrlRepo, _requisitionParams);
            //var result = await new AuthorizationController(_AuthLogger, _userManager, configuration, _environment).SendEmailAuthorization_PRNO(_reqvm.TblRequisitionhdr.Reqno, _UserID);

            UploadFiles(_reqvm.FormFileList, _params.TblRequisitionhdrList[0].Reqno);

            return Redirect("~/Requisition/Details/" + _params.TblRequisitionhdrList[0].Reqno);
        }

        private IList<ModelStateError> CanCreateRequisition(InsertRequisitionModel insModel)
        {
            insModel.ModelStateError = new List<ModelStateError>();
            if (insModel.RequisitionItemList.Count() == 0)
            {
                insModel.ModelStateError.Add
                    (new ModelStateError()
                    {
                        Key = "RequisitionItemList",
                        Message = "Please add atleast 1 item/service!"
                    });             
            }

            if (HttpContext.Request.Form.Files.Count == 0)
            {
                insModel.ModelStateError.Add
                    (new ModelStateError()
                    {
                        Key = "Files",
                        Message = "Please add atleast 1 supporting docs!"
                    });
            }

            return insModel.ModelStateError;
        }

        private async Task<RequisitionViewModel> SetInputValuesForCreate(InsertRequisitionModel insModel, RequisitionViewModel model)
        {
            model.VendorList  = await _RepositoryUnit.VendorRepository.GetVendorLists1(BaseUrlRepo, "", insModel.PaymentClass);
            model.VendorItemList = await _RepositoryUnit.VendorRepository.GetVendorItemsList(BaseUrlRepo, insModel.SupplierCode);
            model.PaymentClass = insModel.PaymentClass;
            model = await setCreateItemList(JsonConvert.SerializeObject(insModel.RequisitionItemList), insModel.SupplierCode, model);
            return model;
        }

        private async Task<RequisitionViewModel> SetRequisition(InsertRequisitionModel insModel, RequisitionViewModel reqVM,string _UserID)
        {
            reqVM.ReqEmpDetails = await _RepositoryUnit.EmployeeRepository.GetEmployeeDetails(OSPUrlRepo, _UserID);
            string strItems = HttpContext.Request.Form["RequisitionItemList"].ToString();
            IFormFileCollection formFiles = HttpContext.Request.Form.Files;

            reqVM.FormFileList = new List<IFormFile>();

            foreach (FormFile fromfile in formFiles)
            {
                reqVM.FormFileList.Add(fromfile);
            }

            reqVM.RequisitionItemList = JsonConvert.DeserializeObject<List<qryRequisitionItem>>(strItems);

            qryVendorDetails _vendorDtl = await _RepositoryUnit.VendorRepository.GetVendorDetails(BaseUrlRepo, insModel.SupplierCode, insModel.PaymentClass);
            TblRequisitionhdr _ReqHdr = new TblRequisitionhdr()
            {
                Reqno = "0",
                MainReqNo = "0",
                BatchNo = string.Empty,
                CompanyCode = reqVM.ReqEmpDetails.CompanyCode,
                DeptCode = reqVM.ReqEmpDetails.DeptCode,
                ReqDate = DateTime.Now,
                PayClassCode = insModel.PaymentClass,
                Active = true,
                VendorCode = _vendorDtl.VendorCode,
                PayeeName = _vendorDtl.PayeeName,
                PayMethodCode = _vendorDtl.PaymethodCode,
                BankCode = _vendorDtl.BankCode,
                Destination = "12345",
                TotalAmount = 0,
                Remarks = insModel.Remarks,
                Void = false,
                VoidUser = "",
                VoidDate = DateTime.Now,
                Printed = false,
                AuditUser = _UserID,
                AuditDate = DateTime.Now,
                UploadStat = false,
                EditUser = _UserID,
                EditDate = DateTime.Now,
                TrxMonth = "JAN24",
                TrxWeek = 1,
                RefNo = insModel.RefNo
            };

            RequisitionParams _requisitionParams = new RequisitionParams();

            TblRequisitionhdr _oldReqhdr = await _RepositoryUnit.TblRequisitionRepository.GetLatestPRRow(BaseUrlRepo, _ReqHdr.CompanyCode, _ReqHdr.DeptCode);
            string _PRNo = string.Empty;
            _requisitionParams.LastNo = _oldReqhdr == null ? "" : _oldReqhdr.Reqno;
            _requisitionParams.CompanyCode = _ReqHdr.CompanyCode;
            _PRNo = await GenerateNewPRNo(_oldReqhdr, _requisitionParams);

            _ReqHdr.Reqno = _PRNo;
            _ReqHdr.MainReqNo = _PRNo;

            qryRequisitionHdrComputation _qryRequisitionHdrComputation = new qryRequisitionHdrComputation();
            List<qryRequisitionDtl> qryRequisitionDtl = new List<qryRequisitionDtl>();

            decimal totAmt = 0;
            foreach (var reqItem in reqVM.RequisitionItemList)
            {
                reqItem.ReqNo = _ReqHdr.Reqno;
                await SetRequestItemBreakdown(reqItem, _ReqHdr.VendorCode, _ReqHdr.PayClassCode);

                qryRequisitionDtl.Add(
                    new qryRequisitionDtl()
                    {
                        ReqNo = reqItem.ReqNo,
                        Gross = reqItem.Quantity * reqItem.Price,
                        VAT = reqItem.Vat,
                        NetOfVAT = reqItem.NetOfVat,
                        TotalTax = reqItem.Vat,
                        Discount = reqItem.Discount,
                        TotalAmount = reqItem.TotalAmount,
                        Deduction = 0,
                    });
                totAmt += reqItem.TotalAmount;
            }

            _qryRequisitionHdrComputation = await _ServiceUnit.PaymentRequestService.ComputeBreakDownHdr(BaseUrlService, qryRequisitionDtl);

            _ReqHdr.Vat = _qryRequisitionHdrComputation.Vat;
            _ReqHdr.NetofVat = _qryRequisitionHdrComputation.NetOfVat;
            _ReqHdr.TotalTax = _qryRequisitionHdrComputation.TotalTax;
            _ReqHdr.Deduction = _qryRequisitionHdrComputation.Deduction;
            _ReqHdr.Discount = _qryRequisitionHdrComputation.Discount;
            _ReqHdr.AmountDue = _qryRequisitionHdrComputation.AmountDue;
            _ReqHdr.TransType = "REG";
            _ReqHdr.TotalAmount = totAmt;

            foreach (var reqItem in reqVM.RequisitionItemList)
            {
                qryCompanyDetails _reqCompDtl = await _RepositoryUnit.CompanyRepository.GetCompanyDetails(OSPUrlRepo, reqItem.CompanyType, reqItem.DeptCode);

                TblRequisitiondtl _reqdtl = new TblRequisitiondtl()
                {
                    ReqItemNo = 0,
                    ReqNo = _ReqHdr.Reqno,
                    CompanyCode = _reqCompDtl.CompanyCode,
                    DeptCode = reqItem.DeptCode,
                    ItemCode = reqItem.ItemCode,
                    Unit = reqItem.Unit,
                    Price = reqItem.Price,
                    Quantity = reqItem.Quantity,
                    Gross = reqItem.Quantity * reqItem.Price,
                    VatRate = VatRate,
                    Vat = reqItem.Vat,
                    NetofVat = reqItem.NetOfVat,
                    TotalTax = reqItem.Vat,
                    Discount = reqItem.Discount,
                    TotalAmount = reqItem.TotalAmount,
                    Void = false,
                    AuditUser = _UserID,
                    AuditDate = DateTime.Now,
                    UploadStat = false,
                    EditUser = _UserID,
                    EditDate = DateTime.Now
                };

                reqVM.RequisitionDtlList.Add(_reqdtl);
            }

            reqVM.TblRequisitionhdr = _ReqHdr;
            return reqVM;
         }

        private async Task<RequisitionViewModel> SetCreateNewRequisitionModel(RequisitionViewModel model)
        {
            //ViewBag.msg = new TblResponse()
            //{
            //    ErrorMessage = "",
            //    Status = "success"
            //};

            personid = _userManager.GetUserId(this.User);
            model.dashboardViewModel = new DashBoardViewModel();
            model.dashboardViewModel.RequestNo = "";
            model.isCreate = true;
            model.VendorList = new List<qryVendorList>();
            model.isCreatePayment = false;
            model.isCreateRequisition = true;

            model.PRNo = null;
            model.AuditUser = personid;
            model.RequestDatetime = DateTime.Now;
            model.Requestdate = DateTime.Now.ToString("MM-dd-yyyy hh:mm:ss");

            //_PaymentRequestModel.CompanyList = await _RepositoryUnit.CompanyRepository.GetCompanyTypes("");
            model.CompanyList = await _RepositoryUnit.CompanyRepository.GetCompanyTypesAccess(personid, OSPUrlRepo);
            model.Company = model.CompanyList.Select(m => m.CompanyType).FirstOrDefault();

            model.DeptList = await _RepositoryUnit.RefDepartmentRepository.GetDeptByPersonID(personid, model.Company, OSPUrlRepo);
            model.Code = model.DeptList.Select(m => m.DeptCode).FirstOrDefault();
            model.qryBranch = await _RepositoryUnit.BranchRepository.GetBranchdetails(model.Company, model.Code, OSPUrlRepo);


            if (model.qryBranch == null)
            {
                model.Address = "";
            }
            else
            {
                model.Address = model.qryBranch.Address;
            }

            model.Paymenttypelist = await _RepositoryUnit.RefPaymentTypeRepository.GetPaymenttypeList(BaseUrlRepo);            
            model.Discountlist = await _RepositoryUnit.RefDiscountRepository.GetRefDiscount(BaseUrlRepo);

            List<RefBranch> branch = new List<RefBranch>();
            branch.Add(new RefBranch { BranchCode = "-1", BranchDesc = "" });
            model.Branchlist = branch;

            ViewData["pagetitle"] = "Requisition Entry";

            model.ReqEmpDetails = await _RepositoryUnit.EmployeeRepository.GetEmployeeDetails(OSPUrlRepo, personid);
            model.RequisitionItemList = new List<qryRequisitionItem>();
            model.RequisitionInfo.Deduction = 0;
            return model;
        }

        private async Task<qryRequisitionItem> SetRequestItemBreakdown(qryRequisitionItem reqItem, string vendorcode, string payclass)
        {
            qryVendorDetails _vdtl = await _RepositoryUnit.VendorRepository.GetVendorDetails(BaseUrlRepo, vendorcode, payclass);

            TblVendorItems _vitems = await _RepositoryUnit.VendorRepository.GetVendorItemsDetails(BaseUrlRepo, vendorcode, reqItem.ItemCode);
            reqItem.Price = _vitems.Amount;
            reqItem.Unit = _vitems.UOM;
            if (String.IsNullOrEmpty(reqItem.DiscountCode))
            {
                reqItem.DiscountCode = "002";
                reqItem.Discount = 0;
            }
            qryComputeBreakdown _criteria = new qryComputeBreakdown() { Gross = reqItem.Price, Qty = reqItem.Quantity, Disccode = reqItem.DiscountCode, VatRate = VatRate, Discount = reqItem.Discount, isVAT = _vdtl.isVat };
            _criteria = await _ServiceUnit.PaymentRequestService.ComputeBreakDown(_criteria, BaseUrlService);

            reqItem.Vat = _criteria.Vat;
            reqItem.NetOfVat = _criteria.NetOfVAT;
            reqItem.TotalAmount = _criteria.AmountDue;
            reqItem.Discount = _criteria.Discount;

            return reqItem;
        }

        public async Task<string> GenerateNewPRNo(TblRequisitionhdr OldRequisitionHdr, RequisitionParams _requisitionParams)
        {
            string _PRNo = string.Empty;
            if (OldRequisitionHdr != null)
            {
                var newprno = await _ServiceUnit.RequisitionService.GenerateNewPRNo(BaseUrlService, _requisitionParams);
                _PRNo = newprno;
            }
            else
            {
                _requisitionParams.LastNo = "0";
                var newprno = await _ServiceUnit.RequisitionService.GenerateNewPRNo(BaseUrlService, _requisitionParams);
                _PRNo = newprno;
            }

            return _PRNo;
        }

        public async Task<IActionResult> Create()
        {
            //ViewBag.msg = new TblResponse()
            //{
            //    ErrorMessage = "",
            //    Status = "success"
            //};

            personid = _userManager.GetUserId(this.User);
            RequisitionViewModel model = new RequisitionViewModel();

            model = await SetCreateNewRequisitionModel(model);
            return View("CreateRequisition", model);
        }

        public async Task<IActionResult>  GetRequisitionList( string Filters)
        {
            RequisitionViewModel model = new RequisitionViewModel();

            List<FilterOption> filter = JsonConvert.DeserializeObject<List<FilterOption>>(Filters);

            personid = _userManager.GetUserId(this.User);
            model.dashboardViewModel = new DashBoardViewModel();
            model.value = BaseUrlRepo + "/Ron/GetRequestPaymentRequestbyStatus";
            IList<qryRequestPaymentRequestbyStatus>  ReqList = await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-REQUESTER", personid, BaseUrlRepo);

            foreach (var item in filter.Select(a=>a.FilterBy).Distinct().ToList())
            {
                ReqList = FilterList(ReqList,filter,item);
            }
            model.dashboardViewModel.RequestList = ReqList;
            model.dashboardViewModel.RequestDepartmentList = await _RepositoryUnit.PRAuthorizationRepository.GetRequisitionDepartment(personid, BaseUrlRepo);
            //ReqList.Where(a => filterStatus.Contains(a.Status)).ToList();
            model.ListCheck = true;
            model.ListCheckID = "SelectedPO";
            return View("_RequisitionList", model);
        }

        private IList<qryRequestPaymentRequestbyStatus> FilterList(IList<qryRequestPaymentRequestbyStatus> lst, List<FilterOption> filterOption, string field)
        {

            List<string> filters = filterOption.Where(x => x.FilterBy == field).Select(xx => xx.FilterVal.ToUpper()).ToList();

            if (filters.Count > 0)
            {
                switch (field)
                {
                    case "Payclass":
                        {
                            return lst.Where(a => filters.Contains(a.PayClass.ToUpper())).ToList();
                        }
                    case "Company":
                        {
                            return lst.Where(a => filters.Contains(a.ItemCompany.ToUpper())).ToList();
                        }
                    case "Status":
                        {
                            return lst.Where(a => filters.Contains(a.Status.ToUpper())).ToList();
                        }
                    case "Vendor":
                        {
                            return lst.Where(a => filters.Contains(a.Vendor.ToUpper())).ToList();
                        }
                    default:
                        {
                            return lst;
                        }
                }
                
            }
            else
            { return lst; }
            
        }

        public ActionResult OpenFolderPath(string path)
        {
            var psi = new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            };
            Process.Start(psi);

            return Json(new { success = "", errormsg = "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePayment(RequisitionViewModel _reqVM)
        {
            string _UserID = _userManager.GetUserId(this.User);

            _reqVM.ReqEmpDetails = await _RepositoryUnit.EmployeeRepository.GetEmployeeDetails(OSPUrlRepo, _UserID);
            string strItems = HttpContext.Request.Form["RequisitionItemList"].ToString();
            _reqVM.MainReqNo = HttpContext.Request.Form["MainReqNo"].ToString();
            IFormFileCollection formFiles = HttpContext.Request.Form.Files;
            List<IFormFile> reqfiles = new List<IFormFile>();
            foreach (FormFile file in formFiles)
            { reqfiles.Add(file); }

            _reqVM.RequisitionItemList = JsonConvert.DeserializeObject<List<qryRequisitionItem>>(strItems); ;

            string MainReqNo = _reqVM.RequisitionInfo.MainReqNo;
            TblRequisitionhdr _MainReq = await _RepositoryUnit.TblRequisitionRepository.ReadRequestByPRNo(BaseUrlRepo, _reqVM.MainReqNo);

            decimal freightPerUnit = _reqVM.Freight > 0 ? _reqVM.Freight / _reqVM.RequisitionItemList.Sum(t=>t.Quantity): 0;

            TblRequisitionhdr _ReqHdr = new TblRequisitionhdr()
            {
                Reqno = "0",
                MainReqNo = _reqVM.MainReqNo,
                BatchNo = string.Empty,
                CompanyCode = _reqVM.ReqEmpDetails.CompanyCode,
                DeptCode = _reqVM.ReqEmpDetails.DeptCode,
                ReqDate = DateTime.Now,
                PayClassCode = _MainReq.PayClassCode,
                Active = true,
                VendorCode = _MainReq.VendorCode,
                PayeeName = _MainReq.PayeeName,
                PayMethodCode = _MainReq.PayMethodCode,
                BankCode = _MainReq.BankCode,
                Destination = _MainReq.Destination,
                TotalAmount = 0,
                Remarks = _reqVM.Remarks,
                Void = false,
                VoidUser = "",
                VoidDate = DateTime.Now,
                Printed = false,
                AuditUser = _UserID,
                AuditDate = DateTime.Now,
                UploadStat = false,
                EditUser = _UserID,
                EditDate = DateTime.Now,
                TrxMonth = "JAN24",
                TrxWeek = 1,
                RefNo = _reqVM.RefNo,
                Deduction = _reqVM.Deduction,
                TotalFreight = _reqVM.Freight
            };

            RequisitionParams _requisitionParams = new RequisitionParams();

            TblRequisitionhdr _oldReqhdr = await _RepositoryUnit.TblRequisitionRepository.GetLatestPRRow(BaseUrlRepo, _ReqHdr.CompanyCode, _ReqHdr.DeptCode);
            string _PRNo = string.Empty;
            _requisitionParams.LastNo = _oldReqhdr.Reqno;
            _requisitionParams.CompanyCode = _ReqHdr.CompanyCode;
            _PRNo = await GenerateNewPRNo(_oldReqhdr, _requisitionParams);

            _ReqHdr.Reqno = _PRNo;

            qryRequisitionHdrComputation _qryRequisitionHdrComputation = new qryRequisitionHdrComputation();
            List<qryRequisitionDtl> qryRequisitionDtl = new List<qryRequisitionDtl>();

            decimal totAmt = 0;
            foreach (var reqItem in _reqVM.RequisitionItemList)
            {
                reqItem.ReqNo = _ReqHdr.Reqno;
                await SetRequestItemBreakdown(reqItem, _ReqHdr.VendorCode, _ReqHdr.PayClassCode);

                qryRequisitionDtl.Add(
                    new qryRequisitionDtl()
                    {
                        ReqNo = reqItem.ReqNo,
                        Gross = reqItem.Quantity * reqItem.Price,
                        VAT = reqItem.Vat,
                        NetOfVAT = reqItem.NetOfVat,
                        TotalTax = reqItem.Vat,
                        Discount = reqItem.Discount,
                        TotalAmount = reqItem.TotalAmount,
                        Deduction = 0,
                    });
                totAmt += reqItem.TotalAmount;
            }

            _qryRequisitionHdrComputation = await _ServiceUnit.PaymentRequestService.ComputeBreakDownHdr(BaseUrlService, qryRequisitionDtl);

            _ReqHdr.Vat = _qryRequisitionHdrComputation.Vat;
            _ReqHdr.NetofVat = _qryRequisitionHdrComputation.NetOfVat;
            _ReqHdr.TotalTax = _qryRequisitionHdrComputation.TotalTax;
            _ReqHdr.Discount = _qryRequisitionHdrComputation.Discount;
            _ReqHdr.AmountDue = _qryRequisitionHdrComputation.AmountDue;
            _ReqHdr.TransType = "REG";
            _ReqHdr.TotalAmount = totAmt;

            await _RepositoryUnit.TblRequisitionRepository.PostCreateRequisitionHdr(BaseUrlRepo, _ReqHdr);

            foreach (var reqItem in _reqVM.RequisitionItemList)
            {
                qryCompanyDetails _reqCompDtl = await _RepositoryUnit.CompanyRepository.GetCompanyDetails(OSPUrlRepo, reqItem.CompanyType, reqItem.DeptCode);

                TblRequisitiondtl _reqdtl = new TblRequisitiondtl()
                {
                    ReqItemNo = 0,
                    ReqNo = _ReqHdr.Reqno,
                    CompanyCode = _reqCompDtl.CompanyCode,
                    DeptCode = reqItem.DeptCode,
                    ItemCode = reqItem.ItemCode,
                    Unit = reqItem.Unit,
                    Price = reqItem.Price,
                    Quantity = reqItem.Quantity,
                    Gross = reqItem.Quantity * reqItem.Price,
                    VatRate = VatRate,
                    Vat = reqItem.Vat,
                    NetofVat = reqItem.NetOfVat,
                    TotalTax = reqItem.Vat,
                    Discount = reqItem.Discount,
                    TotalAmount = reqItem.TotalAmount,
                    Void = false,
                    AuditUser = _UserID,
                    AuditDate = DateTime.Now,
                    UploadStat = false,
                    EditUser = _UserID,
                    EditDate = DateTime.Now,
                    Freight = freightPerUnit
                };

                await _RepositoryUnit.TblRequisitionRepository.PostCreateRequisitionDtl(BaseUrlRepo, _reqdtl);
            }

            TblPaymentrequisitionhdr _pyreqhdr = new TblPaymentrequisitionhdr()
            {
                Reqno = _ReqHdr.Reqno,
                PRno = _ReqHdr.Reqno,
                PRDate = _ReqHdr.ReqDate,
                Active = true,
                TotalAmount = _ReqHdr.TotalAmount,
                SalesInvoiceNo = _reqVM.SalesInvoiceNo,
                SalesInvoiceDate = _reqVM.SalesInvoiceDate,
                DeliveryNo = _reqVM.DeliveryNo,
                DeliveryDate = _reqVM.DeliveryDate,
                Printed = false,
                AuditUser = _ReqHdr.AuditUser,
                AuditDate = _ReqHdr.AuditDate,
                TrxMonth = _ReqHdr.TrxMonth,
                TrxWeek = _ReqHdr.TrxWeek
            };

            await _RepositoryUnit.TblRequisitionRepository.PostCreatePaymentRequisitionHdr(BaseUrlRepo,_pyreqhdr);
            UploadFiles(reqfiles, _ReqHdr.Reqno);

            await _RepositoryUnit.PRAuthorizationRepository.CreatePRAuthorization(BaseUrlRepo, BaseUrlRepo, _ReqHdr.Reqno, "PR");
            //await _RepositoryUnit.TblRequisitionRepository.CreateLoanHdr(BaseUrlRepo, _requisitionParams);

            var result = await new AuthorizationController(_AuthLogger, _userManager, configuration, _environment).SendEmailAuthorization_PRNO(_ReqHdr.Reqno, _UserID);


            return Redirect("~/Requisition/Details/" + _ReqHdr.Reqno);
        }

        public async Task<IActionResult> CreatePayment(string id = "")
        {
            ViewBag.msg = new TblResponse()
            {
                ErrorMessage = "",
                Status = "success"
            };

            string ReqNo = id;

            personid = _userManager.GetUserId(this.User);

            RequisitionViewModel model = new RequisitionViewModel();
            model.dashboardViewModel = new DashBoardViewModel();
            ReqNo = (await _RepositoryUnit.TblRequisitionRepository.GetRequisitionInfo(BaseUrlRepo, ReqNo)).MainReqNo;

            model.dashboardViewModel.RequestNo = ReqNo;
            model.PRNo = ReqNo;

            model.RequisitionInfo = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionInfo(BaseUrlRepo, ReqNo);

            model.RequisitionItemList = new List<qryRequisitionItem>();
            model.RequisitionItemList.AddRange(await _RepositoryUnit.TblRequisitionRepository.GetRequisitionItemList(BaseUrlRepo, ReqNo));


            ViewData["pagetitle"] = "Requisition Details";

            model.dashboardViewModel.RequestJourney = await _RepositoryUnit.PRAuthorizationRepository.GetPaymentRequestAuthListByPRNO(model.dashboardViewModel.RequestNo, BaseUrlRepo);

            model.dashboardViewModel.isSearch = false;

            if (model.dashboardViewModel.RequestJourney.Count > 0)
            {
                model.PaymentRequestHdr = await _RepositoryUnit.PaymentRequestRepository.GetPaymentRequestHdr(model.dashboardViewModel.RequestNo, BaseUrlRepo);
            }

            model.Files = new List<FileDetails>();
            model.Files.AddRange(GetRequisitionDocs(ReqNo));

            if (model.RequisitionInfo.ReqNo == model.RequisitionInfo.MainReqNo)
            {
                model.PaymentRequisitionInfoList = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionListInfoByMainReqNo(BaseUrlRepo, ReqNo);

                foreach (var item in model.PaymentRequisitionInfoList)
                {
                    model.Files.AddRange(GetRequisitionDocs(item.ReqNo));

                    model.RequisitionItemList.AddRange(await _RepositoryUnit.TblRequisitionRepository.GetRequisitionItemList(BaseUrlRepo, item.ReqNo));
                }

            }

            model.CompanyList = await _RepositoryUnit.CompanyRepository.GetCompanyTypesAccess(personid, OSPUrlRepo);
            model.Company = model.CompanyList.Select(m => m.CompanyType).FirstOrDefault();

            model.DeptList = await _RepositoryUnit.RefDepartmentRepository.GetDeptByPersonID(personid, model.Company, OSPUrlRepo);
            model.Code = model.DeptList.Select(m => m.DeptCode).FirstOrDefault();
            model.Discountlist = await _RepositoryUnit.RefDiscountRepository.GetRefDiscount(BaseUrlRepo);

            model.VendorItemList = new List<TblVendorItems>();

            foreach (var item in model.RequisitionItemList.Where(a => a.ReqNo == model.RequisitionInfo.MainReqNo))
            {
                int MainQty = model.RequisitionItemList.Where(t => t.ReqNo == model.RequisitionInfo.MainReqNo && t.Item == item.Item).Sum(t => t.Quantity);
                int ItemQty = model.RequisitionItemList.Where(t => t.ReqNo != model.RequisitionInfo.MainReqNo && t.Item == item.Item).Sum(t => t.Quantity);
                int Bal = MainQty - ItemQty;

                if (Bal <= 0)
                {
                    continue;
                }

                TblVendorItems _vitem = new TblVendorItems()
                {
                    ItemCode = item.ItemCode,
                    ItemDesc = item.Item,
                    UOM = item.Unit,
                    Amount = item.Price
                };

                model.VendorItemList.Add(_vitem);
            }

            model.MainReqNo = model.RequisitionInfo.MainReqNo;

            return View("CreatePayment", model);
        }

        public async Task<IActionResult> Details(string id = "")
        {
            string ReqNo = id.ToUpper();

            personid = _userManager.GetUserId(this.User);

            RequisitionViewModel model = new RequisitionViewModel();
            model.dashboardViewModel = new DashBoardViewModel();
            model.dashboardViewModel.RequestNo = ReqNo;
            model.PRNo = ReqNo;

            model.RequisitionInfo = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionInfo(BaseUrlRepo, ReqNo);

            var DRList = await _RepositoryUnit.TblRequisitionRepository.GetDRListByReqNo(BaseUrlRepo, ReqNo);
            if (DRList.DRList != null)
            {
                if (DRList.DRList.Count > 0)
                {
                    model.RequisitionInfo.DeliveryNo = string.Join(", ", DRList.DRList);
                }
            }

            if (model.RequisitionInfo == null)
            {
                return Redirect("/");
            }
            model.Freight = model.RequisitionInfo.TotalFreight;
            model.RequisitionItemList = new List<qryRequisitionItem>();
            model.RequisitionItemList.AddRange(await _RepositoryUnit.TblRequisitionRepository.GetRequisitionItemList(BaseUrlRepo, ReqNo));

            model.tmpPaymentRequestInventory = await _RepositoryUnit.PaymentRequestRepository.GettmpPaymentRequestInventoryA(personid, "Auto...", BaseUrlRepo);
            ViewData["tmp"] = model.tmpPaymentRequestInventory;

            ViewData["pagetitle"] = "Requisition Details";

            model.dashboardViewModel.RequestJourney = await _RepositoryUnit.PRAuthorizationRepository.GetPaymentRequestAuthListByPRNO(model.dashboardViewModel.RequestNo, BaseUrlRepo);


            model.dashboardViewModel.isSearch = false;

            if (model.dashboardViewModel.RequestJourney.Count > 0)
            {
                model.PaymentRequestHdr = await _RepositoryUnit.PaymentRequestRepository.GetPaymentRequestHdr(model.dashboardViewModel.RequestNo, BaseUrlRepo);
                RefPaymentClass pyclass = await _RepositoryUnit.RefPaymentTypeRepository.PaymentClassDetails(BaseUrlRepo, model.RequisitionInfo.PayClassCode);
                model.dashboardViewModel.RequestJourneyType = pyclass.ReqDesc;


                qryRequisitionInfo _reqinfo = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionInfo(BaseUrlRepo, model.dashboardViewModel.RequestNo);
                if (_reqinfo.ReqNo != _reqinfo.MainReqNo)
                {
                    model.dashboardViewModel.PORequestJourney = await _RepositoryUnit.PRAuthorizationRepository.GetPaymentRequestAuthListByPRNO(_reqinfo.MainReqNo, BaseUrlRepo);
                    model.dashboardViewModel.isPaymentRequest = true;
                }
            }

            model.Files = new List<FileDetails>();
            model.Files.AddRange(GetRequisitionDocs(ReqNo));

            if (model.RequisitionInfo.ReqNo == model.RequisitionInfo.MainReqNo)
            {
                model.PaymentRequisitionInfoList = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionListInfoByMainReqNo(BaseUrlRepo, ReqNo);

                foreach (var item in model.PaymentRequisitionInfoList)
                {
                    model.Files.AddRange(GetRequisitionDocs(item.ReqNo));

                    model.RequisitionItemList.AddRange(await _RepositoryUnit.TblRequisitionRepository.GetRequisitionItemList(BaseUrlRepo, item.ReqNo));
                }

            }

            TblPaymentRequestAuth _auth = await _RepositoryUnit.PRAuthorizationRepository.GetALLTblPaymentRequestAuthByPersonId(BaseUrlRepo, model.RequisitionInfo.ReqNo, personid);



            if (_auth != null)
            {
                model.forapproval = _auth.AuthorizeClass;
                model.CanVoid =  true;
                if (_auth.StatusType == "PD")
                {
                    model.isverify = true;
                    _response = await _RepositoryUnit.
                        PRAuthorizationRepository.
                        UpdateReadPRAuthorization
                        (new qryUpdateStatusAuth()
                        {
                            PersonID = personid,
                            PRRefNo = model.RequisitionInfo.ReqNo,
                            StatusType = "AP",
                            //ReadUser = _userManager.GetUserId(this.User)
                        }
                        , BaseUrlRepo);

                    TblResponse _resp = await _RepositoryUnit.TblRequisitionRepository.CanVoidRequisition(BaseUrlRepo, new RequisitionParams() { ReqNo = model.PRNo, UserID = personid, TblResponse = new TblResponse() });
                    model.CanVoid = _resp.Status == "FAILED" ? false : true;
                }
                
                if (_auth.AuthorizeClass == "VERIFIER")
                {
                    model.lblAuth = "Tag As Verified";
                }
                else
                {
                    model.lblAuth = "Tag As Approved";
                }


            }

            model.lstReason = await _RepositoryUnit.PRAuthorizationRepository.GetDeclineReason(BaseUrlRepo);

            
            

            IList<TblPaymentRequestAuth> prauthlist = await _RepositoryUnit.PRAuthorizationRepository.GetALLTblPaymentRequestAuthByAuthorizeLevel(BaseUrlRepo,model.PRNo);

            if (prauthlist.Where(t=>t.StatusType == "DN").ToList().Count > 0)
            {
                model.isDenied = true;
                model.CanVoid = false;
            }

            model.showDeleteDocs = true;

            return View("RequisitionInfo", model);
        }

        [HttpPost]
        public IActionResult DeleteDocs(string DocPath)
        {

            try
            {
                DocPath = Utilities.DecodeBase64(DocPath);
                System.IO.File.Delete(DocPath);

                return Json(new { success = true });
            }
            catch (Exception)
            {

                return Json(new { success = false });
            }

        }
        
        private List<FileDetails> GetRequisitionDocs(string ReqNo)
        {
            List<FileDetails> ReqFiles = new List<FileDetails>();

            //  return ReqFiles; // comment temporarily

            string dir = Path.Combine(UploadingPathPR + "\\" + ReqNo);
            var files = new List<FileDetails>();

            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);

            }

            // Get an array of file paths within the specified directory
            string[] filePaths = Directory.GetFiles(dir);

            string userid = _userManager.GetUserId(this.User);
            string SrcFilePath = "\\Files" +"\\" + userid + "\\" + ReqNo + "\\";

            string ReqPathDisplay = this.WebRootPath + SrcFilePath;

            if (System.IO.Directory.Exists(ReqPathDisplay))
            {
                try
                {
                    System.IO.Directory.Delete(ReqPathDisplay,true);
                }
                catch (Exception)
                {
                }
            }

            System.IO.Directory.CreateDirectory(ReqPathDisplay);


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

                    string copyDestination = ReqPathDisplay + System.IO.Path.GetFileName(FileList[i].Name);
                    string src = SrcFilePath + System.IO.Path.GetFileName(FileList[i].Name);

                    src = src.Replace("\\","/");
                    FileList[i].CopyTo(copyDestination,true);

                    //string src = FileList[i].FullName;

                    //if (IsImageExtension(System.IO.Path.GetExtension(FileList[i].Name)))
                    //{
                    //    src = Utilities.GenerateBitMap(FileList[i].FullName);
                    //}
                    src = "/Requisition/DocumentViewer?url=" + Utilities.EncodeBase64(src);
                    string path = Utilities.EncodeBase64(FileList[i].FullName);
                    ReqFiles.Add(new FileDetails { ReqNo = ReqNo, Name = System.IO.Path.GetFileName(FileList[i].Name), Path = path,
                        Src = src });

                }
            }

            return ReqFiles;
        }
        public IActionResult DocumentViewer(string url = "")
        {
            url = Utilities.DecodeBase64(url);
            string fname = Path.GetFileName(url);
            ViewBag.url = url;
            ViewBag.filename = fname;

            string[] _validExtensions = { ".jpeg", ".jpg", ".bmp", ".gif", ".png", ".tiff", ".tif", ".raw" };
            ViewBag.isImage = _validExtensions.Contains(System.IO.Path.GetExtension(fname));
            return View();
        }

        public async Task<IActionResult> BatchApprovalDetails(string id = "")
        {
            string BatchApprovalNo = id;

            BatchRequisitionViewModel model = new BatchRequisitionViewModel();
            RepositoryUnit _RepositoryUnit = new RepositoryUnit();

            model.DashBoardViewModel = new DashBoardViewModel();
            model.DashBoardViewModel.RequestJourney = await _RepositoryUnit.PRAuthorizationRepository.GetPaymentRequestAuthListByPRNO(BatchApprovalNo, BaseUrlRepo);
            model.DashBoardViewModel.RequestNo = BatchApprovalNo;

            model.BatchPRNo = BatchApprovalNo;
            model._BatchSummaryList = new List<qryPaymentRequestHdr>();

            model.RequisitionViewModel = new RequisitionViewModel();
            model.RequisitionViewModel.dashboardViewModel = new DashBoardViewModel();
            model.RequisitionViewModel.dashboardViewModel.RequestList = await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-BatchApprovalNo", BatchApprovalNo, BaseUrlRepo);


            if (string.IsNullOrEmpty(BatchApprovalNo))
            {
                RedirectToAction("/");
            }

            return View("BatchApprovalDetails", model);
        }

        public void UploadFiles(List<IFormFile> files, string prno)
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {

                var fileName = System.IO.Path.GetFileName(formFile.FileName);

                // Get file path to be uploaded
                //var path = Path.Combine(Directory.GetCurrentDirectory(), prno);
                var path = Path.Combine(UploadingPathPR, prno);
                path = path + @"\";
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
        }

        public async Task<IActionResult> UpdateReqDetails(List<IFormFile> files, string model)
        {
            string _UserID = _userManager.GetUserId(this.User);

            qryUpdateReqDetails _qryUpdateReqDetails = JsonConvert.DeserializeObject<qryUpdateReqDetails>(model);  
            _qryUpdateReqDetails.UserCode = _UserID;

            UploadFiles(files, _qryUpdateReqDetails.ReqNo);

            var _respo = await _RepositoryUnit.TblRequisitionRepository.UpdateReqDetails(BaseUrlRepo, _qryUpdateReqDetails);

            return Json(new { isSuccess = true }, new JsonSerializerOptions());
        }

        public async Task<IActionResult> CanUpdateReqDetails(List<IFormFile> files, string model)
        {

            string _UserID = _userManager.GetUserId(this.User);

            qryUpdateReqDetails _qryUpdateReqDetails = JsonConvert.DeserializeObject<qryUpdateReqDetails>(model);
            _qryUpdateReqDetails.UserCode = _UserID;

            //UploadFiles(files, _qryUpdateReqDetails.ReqNo);

            var _respo = await _RepositoryUnit.TblRequisitionRepository.CanUpdateReqDetails(BaseUrlRepo, _qryUpdateReqDetails);
             
            return Json(new { _respo }, new JsonSerializerOptions());
        }

    }


}
