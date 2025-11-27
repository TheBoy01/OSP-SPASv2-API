    using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Execution;
using OSP.SPASv2.Web.APIServices;
using OSP.SPASv2.Web.APIServices.Services;
using OSP.SPASv2.Web.Utility;
using Serilog;
//using OSP.SPASv2.Domain;
//using OSP.SPASv2.DomainDummy.Models;
//using OSP.SPASv2.RepositoryUnit;
using SPASv2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Web.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
//using OSP.SPASv2.ServiceUnit;
//using OSP.SPASv2.RepositoryUnit;
using Microsoft.AspNetCore.Identity;
using OSP.SPASv2.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using OSP.SPASv2.Domain.View;
using Humanizer;
using Microsoft.CodeAnalysis.Scripting;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using System.Web.Razor.Parser.SyntaxTree;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;
using Microsoft.Build.Tasks;
using System.Net;
using SPASv2.Controllers;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Reflection;
//using AspNetCore.Reporting;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Reporting.NETCore;
using OSP.Common.Domain.Msgbox;
using OSP.SPASv2.Domain.Msgbox;
using System.Data.Entity.Migrations.Model;
using Microsoft.AspNetCore.Mvc.Formatters;
using DocumentFormat.OpenXml.EMMA;
using static System.Data.Odbc.ODBC32;

namespace OSP.SPASv2.Web.Controllers
{
    [Authorize]
    public class AuthorizationController : Controller
    {
        //

        private readonly ILogger<AuthorizationController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<OSPSPASv2ApplicationUser> _userManager;
        private RepositoryUnit _RepositoryUnit;
        private ServiceUnit _ServiceUnit;
        public string errorMessage = "";



        private IConfiguration configuration;
        //string GlobalPersonid ;
        private string UploadingPathPR;
        private string PRBatchFilePath;

        string BaseUrl;
        string BaseUrlRepo;
        string BaseUrlService;
        string OSPUrlRepo;
        string OSPUrlService;
        string DevelopmentType;
        string _ReportFilespath;
        private IConfiguration _configuration;

        TblResponse _resp;
        public AuthorizationController(ILogger<AuthorizationController> logger, UserManager<OSPSPASv2ApplicationUser> userManager, IConfiguration configuration, IHostEnvironment env)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _logger = logger;
            _RepositoryUnit = new RepositoryUnit();
            _ServiceUnit = new ServiceUnit();
            _resp = new TblResponse();
            //configuration = _configuration;
            //GlobalPersonid = configuration["GlobalPersonID"];

            this._userManager = userManager;
            //ViewData["UserID"] = _userManager.GetUserId(this.User);

            //GlobalPersonid = _userManager.GetUserId(this.User);
            _configuration = configuration;

            UploadingPathPR = _configuration.GetSection("UploadingPath")["PaymentRequest"];
            PRBatchFilePath = _configuration.GetSection("UploadingPath")["PRBatchPath"];

            BaseUrlRepo = _configuration.GetSection("APIBaseURL")["SPASv2.Repository"];
            BaseUrlService = _configuration.GetSection("APIBaseURL")["SPASv2.Service"];
            OSPUrlRepo = _configuration.GetSection("APIBaseURLCommon")["Common.Repository"];
            OSPUrlService = _configuration.GetSection("APIBaseURLCommon")["Common.Service"];
            BaseUrl = _configuration.GetSection("BaseURL").Value;

            _ReportFilespath = _configuration.GetSection("UploadingPath")["ReqFiles"];

            DevelopmentType = env.EnvironmentName;

        }
        [HttpGet]
        public async Task<IActionResult> PRAuthorizationLists(string personid)
        {
            personid = _userManager.GetUserId(this.User);
            //var _sample = await _RepositoryUnit.PRAuthorizationRepository.GetPaymentRequestAuthListByPRNO("SPLPIBALING2308-000001");
            PRAuthorizationModel _PRAuthorizationModel = new PRAuthorizationModel();
            //_VendorMaintenanceModel.VendorList = await _RepositoryUnitClient.GetVendorListAsync();
            //_VendorMaintenanceModel.qryVendorList = await _RepositoryUnit.VendorRepository.GetVendorList("");
            _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists(personid, BaseUrlRepo);

            string authorizeClass = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizeClass(personid, BaseUrlRepo);

            switch (authorizeClass)
            {
                case "APPROVER":
                    _PRAuthorizationModel.AuthorizationBatchTitle = "Pay Class.";
                    _PRAuthorizationModel.AuthorizeClassButton = "Approve Request";
                    break;
                case "VERIFIER":
                    _PRAuthorizationModel.AuthorizationBatchTitle = "Batch No.";
                    _PRAuthorizationModel.AuthorizeClassButton = "Verify Request";
                    _PRAuthorizationModel.RushClassButton = "Verify Request as RUSH";
                    break;
                default:
                    _PRAuthorizationModel.AuthorizeClassButton = "";
                    break;
            }

            return View(_PRAuthorizationModel);
            //return View();
        }
        [HttpGet]
        public async Task<IActionResult> BatchPRAuthorization(string personid, string BatchPRNo)
        {
            //BatchPRNo = "BN2309000001";
            personid = _userManager.GetUserId(this.User);
            //var _sample = await _RepositoryUnit.PRAuthorizationRepository.GetPaymentRequestAuthListByPRNO("SPLPIBALING2308-000001");
            PRAuthorizationModel _PRAuthorizationModel = new PRAuthorizationModel();
            RequisitionViewModel model = new RequisitionViewModel();
            //_VendorMaintenanceModel.VendorList = await _RepositoryUnitClient.GetVendorListAsync();
            //_VendorMaintenanceModel.qryVendorList = await _RepositoryUnit.VendorRepository.GetVendorList("");
            //model.DeclineReasonViewModal.lstReason = await _RepositoryUnit.PRAuthorizationRepository.GetDeclineReason(BaseUrlRepo);

            string authorizeClass = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizeClass(personid, BaseUrlRepo);
            string payclasscode = string.Empty;
            switch (authorizeClass)
            {
                case "APPROVER":

                    _PRAuthorizationModel.TblRequisitionhdr = await _RepositoryUnit.PRAuthorizationRepository.GetBatchPRNoList(personid, BaseUrlRepo);
                    _PRAuthorizationModel.qryPaymentClassAuthorization = await _RepositoryUnit.PRAuthorizationRepository.GetPaymentClassAuthorization(personid, BaseUrlRepo);
                    _PRAuthorizationModel.batchno = _PRAuthorizationModel.qryPaymentClassAuthorization.Select(a => a.PayClassDesc).Distinct().ToList();
                    
                    if (!String.IsNullOrEmpty(BatchPRNo))
                    {
                        payclasscode = _PRAuthorizationModel.qryPaymentClassAuthorization.Where(a=> a.PayClassDesc.Equals(BatchPRNo)).Select(a => a.PayClassCode).Distinct().FirstOrDefault();
                        _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists_BatchByPayclassCode(personid, payclasscode, BaseUrlRepo);
                    }
                    else
                    {
                        _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists_Batch(personid, BatchPRNo, BaseUrlRepo);
                    }

                    _PRAuthorizationModel.AuthorizationBatchTitle = "Pay Class.";
                    _PRAuthorizationModel.AuthorizeClassButton = "Approve Request";
                    
                    _PRAuthorizationModel.BatchPRNoCombobox = "Select 1 out of " + _PRAuthorizationModel.batchno.Count.ToString() + " Pay Class";
                    break;
                case "VERIFIER":
                    _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists_Batch(personid, BatchPRNo, BaseUrlRepo);
                    
                    
                    
                    _PRAuthorizationModel.AuthorizationBatchTitle = "Batch No.";
                    _PRAuthorizationModel.AuthorizeClassButton = "Verify Request";
                    _PRAuthorizationModel.RushClassButton = "Verify Request as RUSH";
                    _PRAuthorizationModel.TblRequisitionhdr = await _RepositoryUnit.PRAuthorizationRepository.GetBatchPRNoList(personid, BaseUrlRepo);
                    _PRAuthorizationModel.batchno = _PRAuthorizationModel.TblRequisitionhdr.Select(a => a.BatchNo).Distinct().ToList();
                    _PRAuthorizationModel.BatchPRNoCombobox = "Select 1 out of " + _PRAuthorizationModel.batchno.Count.ToString() + " Pending Batch";
                    break;
                default:
                    _PRAuthorizationModel.AuthorizeClassButton = "";
                    break;
            }

            //_PRAuthorizationModel.TblRequisitionhdr = await _RepositoryUnit.PRAuthorizationRepository.GetBatchPRNoList(personid, BaseUrlRepo);
            //_PRAuthorizationModel.batchno = _PRAuthorizationModel.TblRequisitionhdr.Select(a => a.PayeeName).Distinct().ToList();
            //_PRAuthorizationModel.BatchPRNoCombobox = "Select 1 out of " + _PRAuthorizationModel.batchno.Count.ToString() + " Pending Batch";

            if (_PRAuthorizationModel.TblRequisitionhdr.Count == 0)
            {
                _PRAuthorizationModel.BatchPRNoCombobox = "No Pending Batch";
            }

            _PRAuthorizationModel.lstReason = await _RepositoryUnit.PRAuthorizationRepository.GetDeclineReason(BaseUrlRepo);

            return View(_PRAuthorizationModel);
            //return View();
            //return RedirectToAction("BatchPRAuthorization", "Authorization", new { batchprno = BatchPRNo });
            //return Json(new { success = "SUCCESS", errormsg = "SUCCESS" });
        }
        [HttpGet]
        public async Task<IActionResult> BatchPRAuthorization_Select(string personid, string batchprno)
        {

            personid = _userManager.GetUserId(this.User);
            //var _sample = await _RepositoryUnit.PRAuthorizationRepository.GetPaymentRequestAuthListByPRNO("SPLPIBALING2308-000001");
            PRAuthorizationModel _PRAuthorizationModel = new PRAuthorizationModel();
            //_VendorMaintenanceModel.VendorList = await _RepositoryUnitClient.GetVendorListAsync();
            //_VendorMaintenanceModel.qryVendorList = await _RepositoryUnit.VendorRepository.GetVendorList("");
            _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists_Batch(personid, batchprno, BaseUrlRepo);

            string authorizeClass = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizeClass(personid, BaseUrlRepo);

            switch (authorizeClass)
            {
                case "APPROVER":
                    _PRAuthorizationModel.AuthorizeClassButton = "Approve Request";
                    break;
                case "VERIFIER":
                    _PRAuthorizationModel.AuthorizeClassButton = "Verify Request";
                    _PRAuthorizationModel.RushClassButton = "Verify Request as RUSH";
                    break;
                default:
                    _PRAuthorizationModel.AuthorizeClassButton = "";
                    break;
            }
            _PRAuthorizationModel.TblRequisitionhdr = await _RepositoryUnit.PRAuthorizationRepository.GetBatchPRNoList(personid, BaseUrlRepo);

            //List<string> batchnolist = _PRAuthorizationModel.TblRequisitionhdr.Select(a => a.BatchNo).Distinct().ToList();

            _PRAuthorizationModel.batchno = _PRAuthorizationModel.TblRequisitionhdr.Select(a => a.BatchNo).Distinct().ToList();

            _PRAuthorizationModel.lstReason = await _RepositoryUnit.PRAuthorizationRepository.GetDeclineReason(BaseUrlRepo);

            return View(_PRAuthorizationModel);
            //RedirectToAction("BatchPRAuthorization", "Authorization", new { BatchPRNo = BatchPRNo });
            //return Json(new { success = "SUCCESS", errormsg = "SUCCESS" });

            //return View();
        }
        [HttpGet]
        public async Task<IActionResult> AuthorizerPayclassLists()
        {

            PRAuthorizationModel _PRAuthorizationModel = new PRAuthorizationModel();
            //_VendorMaintenanceModel.VendorList = await _RepositoryUnitClient.GetVendorListAsync();
            //_VendorMaintenanceModel.qryVendorList = await _RepositoryUnit.VendorRepository.GetVendorList("");
            _PRAuthorizationModel.qryListOfAuthorizerPayclass = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizerPayclassLists("CHAPEL", "6412-00");
            return View(_PRAuthorizationModel);
            //return View();
        }
        [HttpGet]
        public async Task<IActionResult> AuthorizerPayclassListsByPayclass(string payclass)
        {
            PRAuthorizationModel _PaymentRequestModel = new PRAuthorizationModel();
            //_PaymentRequestModel.Paymenttypelist = await _RepositoryUnit.RefPaymentTypeRepository.GetPaymenttypeList();
            _PaymentRequestModel.qryListOfAuthorizerPayclass = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizerPayclassLists("CHAPEL", payclass);
            //ViewData["tmp"] = _PaymentRequestModel.qryListOfAuthorizerPayclass;
            //return View(_PaymentRequestModel);
            return PartialView("tmpListOfAuthorizerPayClass", _PaymentRequestModel);

        }
        [HttpGet]
        public async Task<IActionResult> AuthorizerMaintenance()
        {
            PRAuthorizationModel _PaymentRequestModel = new PRAuthorizationModel();
            _PaymentRequestModel.Paymenttypelist = await _RepositoryUnit.RefPaymentTypeRepository.GetPaymenttypeList(BaseUrlRepo);
            _PaymentRequestModel.qryListOfAuthorizerPayclass = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizerPayclassLists("CHAPEL", "6412-00");
            ViewData["tmp"] = _PaymentRequestModel.qryListOfAuthorizerPayclass;

            return View(_PaymentRequestModel);
        }
        [HttpGet]
        public async Task<IActionResult> BatchPRNoList()
        {
            PRAuthorizationModel _PaymentRequestModel = new PRAuthorizationModel();
            _PaymentRequestModel.TblRequisitionhdr = await _RepositoryUnit.PRAuthorizationRepository.GetBatchPRNoList("", BaseUrlRepo);
            //ViewData["tmp"] = _PaymentRequestModel.qryListOfAuthorizerPayclass;
            return View(_PaymentRequestModel);
        }
        [HttpGet]
        public async Task<IActionResult> CreatePRAuthorization(string URL, string prno, string reqtype)
        {
            TblResponse _resp = await _RepositoryUnit.PRAuthorizationRepository.CreatePRAuthorization(BaseUrlRepo, "", prno, reqtype);

            IList<string> listprno = new List<string>();

            listprno.Add(prno);

           

            this.SendEmailAuthorization_PRNO(prno);

            //return Json(new { msgType = _resp.Status, msg = _resp.ErrorMessage });
            //return RedirectToAction("PRAuthorizationLists", "Authorization");
            //return Json(new { msgType = true, msg = _resp.ErrorMessage }, new JsonSerializerOptions());
            return RedirectToAction("ViewPR", "PaymentRequest", new { prno = prno });
            //return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage });
            //return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> UpdatePRAuthorization(string prno)
        {
            TblResponse _resp = await _RepositoryUnit.PRAuthorizationRepository.UpdatePRAuthorization("", prno, BaseUrlRepo);
            return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage });
            //return RedirectToAction("PRAuthorizationLists", "Authorization");
            //return Json(new { error = true, errormsg = _resp.ErrorMessage }, new JsonSerializerOptions());
            //return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> ApprovePRAuthorization(qryUpdateStatusAuth _qryUpdateStatusAuth)
        {
            _qryUpdateStatusAuth.PersonID = _userManager.GetUserId(this.User);
            //_qryUpdateStatusAuth.StatusType = "AP";
            TblResponse _resp = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qryUpdateStatusAuth, BaseUrlRepo);
            IList<string> listprno = new List<string>();
            listprno.Add(_qryUpdateStatusAuth.PRRefNo);
            this.SendEmailAuthorization_PRNO();

            return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage });
            //return RedirectToAction("PRAuthorizationLists");
            //return RedirectToAction("PRAuthorizationLists", "Authorization");

            //return Json(new { error = true, errormsg = _resp.ErrorMessage }, new JsonSerializerOptions());
            //return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> ApprovePRAuthorizationRush(qryUpdateStatusAuth _qryUpdateStatusAuth)
        {

            _qryUpdateStatusAuth.PersonID = _userManager.GetUserId(this.User);
            //_qryUpdateStatusAuth.StatusType = "AP";
            TblResponse _resp = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorizationRush(_qryUpdateStatusAuth, BaseUrlRepo);

            IList<string> listprno = new List<string>();
            listprno.Add(_qryUpdateStatusAuth.PRRefNo);

            this.SendEmailAuthorization_PRNO();


            return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage });
            //return RedirectToAction("PRAuthorizationLists");
            //return RedirectToAction("PRAuthorizationLists", "Authorization");

            //return Json(new { error = true, errormsg = _resp.ErrorMessage }, new JsonSerializerOptions());
            //return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> ApprovePRAuthorizationALL(string prno)
        {

            //PrintPO("99999999999");

            AuthorizationParams authorizationParams = new AuthorizationParams();

            qryUpdateStatusAuth _qryUpdateStatusAuth = new qryUpdateStatusAuth();
            _qryUpdateStatusAuth.PersonID = _userManager.GetUserId(this.User);
            _qryUpdateStatusAuth.StatusType = "AP";


            IList<string> _prno = JsonSerializer.Deserialize<IList<string>>(prno);

            try
            {
                foreach (var item in _prno)
                {
                    _qryUpdateStatusAuth.PRRefNo = item;
                    //_resp = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(item, "SPLPI0002", statustype);
                    //this.SendEmailAuthorization_PRNO(item);
                    _resp = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qryUpdateStatusAuth, BaseUrlRepo);

                    authorizationParams.ReqNo.Add(item);
                }


                this.SendEmailAuthorization_PRNO(authorizationParams);

                //this.SendEmailAuthorization_PRNO();

            }
            catch (Exception ex)
            {

                _resp.Status = ex.Message;
                _resp.ErrorMessage = ex.Message;

            }

            return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage });
            //return RedirectToAction("PRAuthorizationLists");
            //return RedirectToAction("PRAuthorizationLists", "Authorization");

            //return Json(new { error = true, errormsg = _resp.ErrorMessage }, new JsonSerializerOptions());
            //return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> ApprovePRAuthorizationALL_Batch(string prno)
        {

            var debug = "Starting..";
            //await PrintPO("198399930");
            AuthorizationParams authorizationParams = new AuthorizationParams();
            qryUpdateStatusAuth _qryUpdateStatusAuth = new qryUpdateStatusAuth();
            _qryUpdateStatusAuth.PersonID = _userManager.GetUserId(this.User);
            _qryUpdateStatusAuth.StatusType = "AP";
            _qryUpdateStatusAuth.TransType = "REG";
            IList<string> _prno = JsonSerializer.Deserialize<IList<string>>(prno);

            try
            {
                foreach (var item in _prno)
                {
                    _qryUpdateStatusAuth.PRRefNo = item;
                    //_resp = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(item, "SPLPI0002", statustype);
                    //this.SendEmailAuthorization_PRNO(item);
                    debug = "GetReqPOPY..";
                    string _isPOPY = await _RepositoryUnit.TblRequisitionRepository.GetReqPOPY(BaseUrlRepo, item);
                    _qryUpdateStatusAuth.ReqType = _isPOPY;

                    debug = "ApprovePRAuthorization..";
                    _resp = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qryUpdateStatusAuth, BaseUrlRepo);

                    debug = "PostUpdateRequestQtySummary..";
                    authorizationParams.ReqNo.Add(item);
                    _resp = await _RepositoryUnit.TblRequisitionRepository.PostUpdateRequestQtySummary(BaseUrlRepo, authorizationParams);
                    
                }
                if (_qryUpdateStatusAuth.PersonID.Equals("PISPLPI06141") && _qryUpdateStatusAuth.ReqType.Equals("PY"))
                {
                    //_resp = await _RepositoryUnit.PRAuthorizationRepository.EndorseToAccounting(authorizationParams.ReqNo, _qryUpdateStatusAuth.PersonID, BaseUrlRepo);
                }

                authorizationParams.UserCode = _qryUpdateStatusAuth.PersonID;
                debug = "ProcessAuthorization..";

                _resp = await _RepositoryUnit.PRAuthorizationRepository.ProcessAuthorization(authorizationParams, BaseUrlRepo);
               

            }
            catch (Exception ex)
            {
                _resp.Status = ex.Message;
                _resp.ErrorMessage = "WEB" + ex.Message + debug;
            }

            return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage  });
        }
        [HttpGet]
        public async Task<IActionResult> ApprovePRAuthorizationALL_BatchRush(string prno)
        {
            //await PrintPO("198399930");

            AuthorizationParams authorizationParams = new AuthorizationParams();

            qryUpdateStatusAuth _qryUpdateStatusAuth = new qryUpdateStatusAuth();
            _qryUpdateStatusAuth.PersonID = _userManager.GetUserId(this.User);
            _qryUpdateStatusAuth.StatusType = "AP";
            _qryUpdateStatusAuth.TransType = "RSH";
            IList<string> _prno = JsonSerializer.Deserialize<IList<string>>(prno);


            try
            {
                foreach (var item in _prno)
                {
                    _qryUpdateStatusAuth.PRRefNo = item;
                    //_resp = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(item, "SPLPI0002", statustype);
                    //this.SendEmailAuthorization_PRNO(item);
                    _resp = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qryUpdateStatusAuth, BaseUrlRepo);

                    authorizationParams.ReqNo.Add(item);

                }


                _resp = await this.SendEmailAuthorization_PRNO_Batch(authorizationParams);
                //await this.PrintChapelAdvisoryPO("POSPLPI2402-000001");
                //this.SendEmailAuthorization_PRNO();

            }
            catch (Exception ex)
            {

                _resp.Status = ex.Message;
                _resp.ErrorMessage = ex.Message;

            }

            return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage });
            //return RedirectToAction("PRAuthorizationLists");
            //return RedirectToAction("PRAuthorizationLists", "Authorization");

            //return Json(new { error = true, errormsg = _resp.ErrorMessage }, new JsonSerializerOptions());
            //return Ok();
        }
        public async Task<IActionResult> DisapprovePRAuthorizationALL_OLD05312024(string prno,string reason)
        {

            qryUpdateStatusAuth _qryUpdateStatusAuth = new qryUpdateStatusAuth();
            _qryUpdateStatusAuth.PersonID = _userManager.GetUserId(this.User);
            _qryUpdateStatusAuth.StatusType = "DN";


            IList<string> _prno = JsonSerializer.Deserialize<IList<string>>(prno);

            try
            {
                foreach (var item in _prno)
                {
                    _qryUpdateStatusAuth.PRRefNo = item;
                    //_resp = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(item, "SPLPI0002", statustype);
                    //this.SendEmailAuthorization_PRNO(item);
                    _qryUpdateStatusAuth.ReqReason = reason;
                    
                    

                    

                    TblRequisitionReason _tblRequisitionReason = new TblRequisitionReason();
                    _tblRequisitionReason.ReqNo = item;
                    _tblRequisitionReason.ReasonCode = reason;
                    _tblRequisitionReason.Remarks = "NA";
                    _tblRequisitionReason.AuditUser = _userManager.GetUserId(this.User);
                    _tblRequisitionReason.AuditDate = DateTime.Now;


                    await _RepositoryUnit.PRAuthorizationRepository.InsertReqReason(_tblRequisitionReason, BaseUrlRepo);





                    _resp = await _RepositoryUnit.PRAuthorizationRepository.DisapprovePRAuthorization(_qryUpdateStatusAuth, BaseUrlRepo);
                }
                //this.SendEmailAuthorization_PRNO(_prno);
                //this.SendEmailAuthorization_PRNO();

            }
            catch (Exception ex)
            {

                _resp.Status = ex.Message;

            }

            return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage });
            //return RedirectToAction("PRAuthorizationLists");
            //return RedirectToAction("PRAuthorizationLists", "Authorization");

            //return Json(new { error = true, errormsg = _resp.ErrorMessage }, new JsonSerializerOptions());
            //return Ok();
        }

        public async Task<IActionResult> DisapprovePRAuthorizationALL(string prno, string reason,string Remarks)
        {

        qryUpdateStatusAuth _qryUpdateStatusAuth = new qryUpdateStatusAuth();
            _qryUpdateStatusAuth.PersonID = _userManager.GetUserId(this.User);
            _qryUpdateStatusAuth.StatusType = "DN";


            IList<string> _prno = JsonSerializer.Deserialize<IList<string>>(prno);

            try
            {
                foreach (var item in _prno)
                {
                    _qryUpdateStatusAuth.PRRefNo = item;
                    //_resp = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(item, "SPLPI0002", statustype);
                    //this.SendEmailAuthorization_PRNO(item);
                    _qryUpdateStatusAuth.ReqReason = reason;





                    TblRequisitionReason _tblRequisitionReason = new TblRequisitionReason();
                    _tblRequisitionReason.ReqNo = item;
                    _tblRequisitionReason.ReasonCode = reason;
                    _tblRequisitionReason.Remarks = Remarks;
                    _tblRequisitionReason.AuditUser = _userManager.GetUserId(this.User);
                    _tblRequisitionReason.AuditDate = DateTime.Now;


                    await _RepositoryUnit.PRAuthorizationRepository.InsertReqReason(_tblRequisitionReason, BaseUrlRepo);





                    _resp = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qryUpdateStatusAuth, BaseUrlRepo);
                }
                //this.SendEmailAuthorization_PRNO(_prno);
                //this.SendEmailAuthorization_PRNO();

            }
            catch (Exception ex)
            {

                _resp.Status = ex.Message;
                return Json(new { success = false, errormsg = _resp.ErrorMessage });

            }

            return Json(new { success = true, errormsg = _resp.ErrorMessage });
            //return RedirectToAction("PRAuthorizationLists");
            //return RedirectToAction("PRAuthorizationLists", "Authorization");

            //return Json(new { error = true, errormsg = _resp.ErrorMessage }, new JsonSerializerOptions());
            //return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> PopulateReason()
        {
            var reasons = await _RepositoryUnit.PRAuthorizationRepository.GetDeclineReason(BaseUrlRepo);
            return Json(reasons); // Return data as JSON
        }

        public async Task<IActionResult> ACCTG_ApprovePRAuthorizationALL(qryUpdateStatusAuth _qryUpdateStatusAuth)
        {
            _qryUpdateStatusAuth.PersonID = _userManager.GetUserId(this.User);
            _qryUpdateStatusAuth.StatusType = "AP";

            IList<string> _prno = JsonSerializer.Deserialize<IList<string>>(_qryUpdateStatusAuth.PRRefNo);
            try
            {
                foreach (var item in _prno)
                {
                    //_resp = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(item, "SPLPI0002", statustype);
                    //this.SendEmailAuthorization_PRNO(item);
                    _resp = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qryUpdateStatusAuth, BaseUrlRepo);
                }
                this.SendEmailAuthorization_PRNO();
            }
            catch (Exception ex)
            {

                _resp.Status = ex.Message;

            }

            return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage });
            //return RedirectToAction("PRAuthorizationLists");
            //return RedirectToAction("PRAuthorizationLists", "Authorization");

            //return Json(new { error = true, errormsg = _resp.ErrorMessage }, new JsonSerializerOptions());
            //return Ok();
        }
        [HttpGet]
        public async Task<TblResponse> SendEmailAuthorization(TblSendEmail _tblsendemail, string url)
        {

            if (DevelopmentType == "Development")
            {
                _tblsendemail.From = "ronom@stpeter.com.ph";
                _tblsendemail.To = "ronom@stpeter.com.ph";
                if (_tblsendemail.CCemails != null)
                {
                    _tblsendemail.CCemails.Clear();
                }

                IList<string> CCs = new List<string>();
                //CCs.Add("ronom@stpeter.com.ph");
                //CCs.Add("davidga@stpeter.com.ph");
                CCs.Add("warrenlb@stpeter.com.ph");
                CCs.Add("rudyab@stpeter.com.ph");
                CCs.Add("jonab@stpeter.com.ph");
                _tblsendemail.CCemails = CCs;
            }

            IList<string> _bcc = new List<string>();
            _bcc.Add("ronom@stpeter.com.ph");
            _bcc.Add("warrenlb@stpeter.com.ph");
            _bcc.Add("rudyab@stpeter.com.ph");
            _bcc.Add("jonab@stpeter.com.ph");
            _tblsendemail.BCemails = _bcc;

            _resp = await _RepositoryUnit.PRAuthorizationRepository.SendEmailPRAuthorization(_tblsendemail, url);

            //return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage });
            //return RedirectToAction("PRAuthorizationLists");
            //return RedirectToAction("PRAuthorizationLists", "Authorization");
            //return Json(new { error = true, errormsg = _resp.ErrorMessage }, new JsonSerializerOptions());

            return _resp;
        }
        [HttpGet]
        public async Task<IActionResult> SendEmailAuthorization_PRNO_Scheduled()
        {
            string _personid = _userManager.GetUserId(this.User);

            IList<TblPaymentRequestAuth> _TblPaymentRequestAuth = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel(BaseUrlRepo);
            IList<string> _personidlist = new List<string>();
            IList<string> _prnolist = new List<string>();

            IList<string> _prnolisttoscctg = new List<string>();

            string authpayclass = string.Empty;
            if (_TblPaymentRequestAuth == null)
            {
                return Ok();
            }


            foreach (string id in _TblPaymentRequestAuth.Select(i => i.PersonID).Distinct())
            {

                _personidlist.Add(id);
            }

            PRAuthorizationModel _PRAuthorizationModel = new PRAuthorizationModel();
            foreach (var item in _personidlist)
            {

                var _positioncode = await _RepositoryUnit.PRAuthorizationRepository.GetPositioncode(item, BaseUrlRepo);


                if (_positioncode == "SYSTEM")
                {
                    _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists(item, BaseUrlRepo);
                    foreach (string prno in _PRAuthorizationModel.qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                    {
                        _prnolisttoscctg.Add(prno);
                    }
                }
                else
                {

                    _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists(item, BaseUrlRepo);

                    foreach (string prno in _PRAuthorizationModel.qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                    {
                        _prnolist.Add(prno);
                        authpayclass = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizeClass(prno, item, BaseUrlRepo);
                    }

                    strBodyEmail = await this.BodyEMAIL_Authorizationv2(_prnolist, item, authpayclass, _prnolist.Count());

                    //await _RepositoryUnit.PRAuthorizationRepository.SendEmailPRAuthorization(_tblsendemail);
                    //return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage });
                    //return RedirectToAction("PRAuthorizationLists");
                    //return RedirectToAction("PRAuthorizationLists", "Authorization");

                    TblSendEmail _tblsendemail = new TblSendEmail();

                    _tblsendemail.ReferenceNo = DateTime.Now.ToString("MM/dd/yyyy hhmmss");
                    _tblsendemail.SystemCode = "SPASv2";
                    _tblsendemail.From = "ronom@stpeter.com.ph";
                    _tblsendemail.To = "ronom@stpeter.com.ph";

                    _tblsendemail.Subject = "Payment Request Authorization";



                    _tblsendemail.Body = strBodyEmail;
                    _tblsendemail.Attachment = null;

                    IList<string> CCs = new List<string>();
                    //CCs.Add("ronom@stpeter.com.ph");
                    //CCs.Add("davidga@stpeter.com.ph");
                    CCs.Add("warrenlb@stpeter.com.ph");
                    CCs.Add("rudyab@stpeter.com.ph");
                    CCs.Add("jonab@stpeter.com.ph");
                    _tblsendemail.CCemails = CCs;
                    _tblsendemail.BCemails = null;
                    _tblsendemail.Host = "smtp-relay.gmail.com";
                    _tblsendemail.Port = "587";
                    _tblsendemail.Username = null;
                    _tblsendemail.Password = null;

                    this.SendEmailAuthorization(_tblsendemail, OSPUrlService);
                }

            }

            if (_prnolisttoscctg.Count > 0)
            {

                await _RepositoryUnit.PRAuthorizationRepository.EndorseToAccounting(_prnolisttoscctg, _personid, BaseUrlRepo);
            }

            return Ok();
        }
        public async Task<IActionResult> SendEmailAuthorization_PRNO()
        {
            string _personid = _userManager.GetUserId(this.User);

            IList<TblPaymentRequestAuth> _TblPaymentRequestAuth = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel(BaseUrlRepo);
            IList<string> _personidlist = new List<string>();
            IList<string> _prnolist = new List<string>();

            IList<string> _prnolisttoscctg = new List<string>();

            string authpayclass = string.Empty;

            if (_TblPaymentRequestAuth == null)
            {
                return Ok();
            }


            foreach (string id in _TblPaymentRequestAuth.Select(i => i.PersonID).Distinct())
            {

                _personidlist.Add(id);
            }

            PRAuthorizationModel _PRAuthorizationModel = new PRAuthorizationModel();
            foreach (var item in _personidlist)
            {

                var _positioncode = await _RepositoryUnit.PRAuthorizationRepository.GetPositioncode(item, BaseUrlRepo);


                if (_positioncode == "SYSTEM")
                {
                    _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists(item, BaseUrlRepo);
                    foreach (string prno in _PRAuthorizationModel.qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                    {
                        _prnolisttoscctg.Add(prno);
                    }
                }
                else
                {

                    _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists(item, BaseUrlRepo);

                    foreach (string prno in _PRAuthorizationModel.qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                    {
                        _prnolist.Add(prno);
                        authpayclass = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizeClass(prno, item, BaseUrlRepo);
                    }

                    strBodyEmail = await this.BodyEMAIL_Authorizationv2(_prnolist, item, authpayclass, _prnolist.Count());

                    //await _RepositoryUnit.PRAuthorizationRepository.SendEmailPRAuthorization(_tblsendemail);
                    //return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage });
                    //return RedirectToAction("PRAuthorizationLists");
                    //return RedirectToAction("PRAuthorizationLists", "Authorization");

                    TblSendEmail _tblsendemail = new TblSendEmail();

                    _tblsendemail.ReferenceNo = DateTime.Now.ToString("MM/dd/yyyy hhmmss");
                    _tblsendemail.SystemCode = "SPASv2";
                    _tblsendemail.From = "ronom@stpeter.com.ph";
                    _tblsendemail.To = "ronom@stpeter.com.ph";
                    _tblsendemail.Subject = "Payment Request Authorization";



                    _tblsendemail.Body = strBodyEmail;
                    _tblsendemail.Attachment = null;

                    IList<string> CCs = new List<string>();
                    //CCs.Add("ronom@stpeter.com.ph");
                    //CCs.Add("davidga@stpeter.com.ph");
                    CCs.Add("warrenlb@stpeter.com.ph");
                    CCs.Add("rudyab@stpeter.com.ph");
                    CCs.Add("jonab@stpeter.com.ph");
                    _tblsendemail.CCemails = CCs;
                    _tblsendemail.BCemails = null;
                    _tblsendemail.Host = "smtp-relay.gmail.com";
                    _tblsendemail.Port = "587";
                    _tblsendemail.Username = null;
                    _tblsendemail.Password = null;

                    this.SendEmailAuthorization(_tblsendemail, OSPUrlService);
                }

            }

            if (_prnolisttoscctg.Count > 0)
            {

                await _RepositoryUnit.PRAuthorizationRepository.EndorseToAccounting(_prnolisttoscctg, _personid, BaseUrlRepo);
            }

            return Ok();
        }
        private async Task CreatePurchaseOrder(string prno, string personid)
        {
            TblPaymentRequestAuth _TblPaymentRequestAuth_PRNO = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel(prno, BaseUrlRepo);

            if (_TblPaymentRequestAuth_PRNO.Remarks == "FOR PAYMENT")
            {
                string ReqType = "PO";

                if (ReqType == "PO")
                {
                    TblPurchaseorderhdr _tblPurchaseorderhdr = new TblPurchaseorderhdr();
                    _tblPurchaseorderhdr.PONo = "PO" + prno;
                    _tblPurchaseorderhdr.Reqno = prno;
                    _tblPurchaseorderhdr.PODate = DateTime.Now;
                    _tblPurchaseorderhdr.Active = false;
                    _tblPurchaseorderhdr.Remarks = "PO as of " + DateTime.Now.Date;
                    _tblPurchaseorderhdr.Printed = false;
                    _tblPurchaseorderhdr.AuditUser = personid;
                    _tblPurchaseorderhdr.AuditDate = DateTime.Now;
                    _tblPurchaseorderhdr.TrxMonth = "JAN24";
                    _tblPurchaseorderhdr.TrxWeek = 0;
                    try
                    {
                        _resp = await _RepositoryUnit.TblPurchaseorderhdrRepository.PostCreatePurchaseOrderHdr(BaseUrlRepo, _tblPurchaseorderhdr);
                    }
                    catch (Exception e)
                    {

                    }

                    try
                    {
                        //response doc
                        await this.Report("PO" + prno);
                        await this.ReportPO("PO" + prno, 0);

                    }
                    catch (Exception e)
                    {
                    }

                    qryRptPurchaseOrderHdr _qryRptPurchaseOrderHdr = await _RepositoryUnit.PRAuthorizationRepository.GetRptPOHdr("PO" + prno, BaseUrlRepo);

                    TblSendEmail _tblsendemail = new TblSendEmail();
                    _tblsendemail.ReferenceNo = DateTime.Now.ToString("MM/dd/yyyy hhmmss");
                    _tblsendemail.SystemCode = "SPASv2";
                    _tblsendemail.From = "ronom@stpeter.com.ph";
                    _tblsendemail.To = "ronom@stpeter.com.ph";
                    _tblsendemail.Subject = "SPASv2 Chapel Advisory - " + "PO" + prno + " - " + DateTime.Now.ToString();
                    //for testing
                    IList<string> strList = new List<string>();
                    string POReport = Path.Combine(_ReportFilespath, "Reports", "ChapelAdvisory", "PO" + prno + ".pdf");
                    strList.Add(POReport);
                    _tblsendemail.Body = await this.BodyEMAIL_ChapelAdvisory();
                    //for testing
                    _tblsendemail.Attachment = strList;

                    IList<string> CCs = new List<string>();
                    CCs.Add("warrenlb@stpeter.com.ph");
                    CCs.Add("rudyab@stpeter.com.ph");
                    CCs.Add("jonab@stpeter.com.ph");

                    _tblsendemail.CCemails = CCs;
                    _tblsendemail.BCemails = null;
                    _tblsendemail.Host = "smtp-relay.gmail.com";
                    _tblsendemail.Port = "587";
                    _tblsendemail.Username = null;
                    _tblsendemail.Password = null;

                    this.SendEmailAuthorization(_tblsendemail, OSPUrlService);

                    ///////Sending to Vendor
                    //await this.PrintPO("PO" + item);

                    _tblsendemail.Subject = "SPASv2 Purchase Order - " + "PO" + prno + " - " + DateTime.Now.ToString();
                    //for testing
                    strList = new List<string>();
                    //POReport = Convert.ToString(@"\\SPLPDEVSERVER\Spasv2$\Reports\POReport\" + "PO" + item + ".pdf");
                    POReport = Path.Combine(_ReportFilespath, "Reports", "POReport", "PO" + prno + ".pdf");
                    strList.Add(POReport);
                    _tblsendemail.Attachment = strList;
                    _tblsendemail.Body = await this.BodyEMAIL_ChapelAdvisory_Vendor(_qryRptPurchaseOrderHdr.VendorName, "PO" + prno);
                    this.SendEmailAuthorization(_tblsendemail, OSPUrlService);


                    ///////Sending to GCM

                    _tblsendemail.Subject = "SPASv2 ADVISORY: CASKET (OUTSOURCE) ORDER" + DateTime.Now.ToString();
                    //for testing
                    strList = new List<string>();
                    //POReport = Convert.ToString(@"\\SPLPDEVSERVER\Spasv2$\Reports\ChapelAdvisory\" + "PO" + item + ".pdf");
                    POReport = Path.Combine(_ReportFilespath, "Reports", "ChapelAdvisory", "PO" + prno + ".pdf");
                    strList.Add(POReport);
                    _tblsendemail.Attachment = strList;
                    _tblsendemail.Body = await this.BodyEMAIL_ChapelAdvisory_GCM();

                    this.SendEmailAuthorization(_tblsendemail, OSPUrlService);

                }
            }


        }
        public async Task<IActionResult> SendEmailAuthorization_PRNO(string Prno, string _personid)
        {
            //string _personid = _userManager.GetUserId(this.User);

            IList<TblPaymentRequestAuth> _TblPaymentRequestAuth = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel(BaseUrlRepo);
            IList<string> _personidlist = new List<string>();
            IList<string> _prnolist = new List<string>();

            IList<string> _prnolisttoscctg = new List<string>();

            string authpayclass = string.Empty;

            if (_TblPaymentRequestAuth == null)
            {
                return Ok();
            }


            foreach (string id in _TblPaymentRequestAuth.Select(i => i.PersonID).Distinct())
            {
                _personidlist.Add(id);
            }


            await CreatePurchaseOrder(Prno, _personid);


            PRAuthorizationModel _PRAuthorizationModel = new PRAuthorizationModel();
            foreach (var item in _personidlist)
            {

                var _positioncode = await _RepositoryUnit.PRAuthorizationRepository.GetPositioncode(item, BaseUrlRepo);


                if (_positioncode == "SYSTEM")
                {
                    _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists(item, BaseUrlRepo);
                    foreach (string prno in _PRAuthorizationModel.qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                    {
                        _prnolisttoscctg.Add(prno);
                    }
                }
                else
                {

                    _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists(item, BaseUrlRepo);

                    foreach (string prno in _PRAuthorizationModel.qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                    {
                        _prnolist.Add(prno);
                        authpayclass = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizeClass(prno, item, BaseUrlRepo);
                    }


                    strBodyEmail = await this.BodyEMAIL_Authorizationv2(_prnolist, item, authpayclass, _prnolist.Count());


                    //await _RepositoryUnit.PRAuthorizationRepository.SendEmailPRAuthorization(_tblsendemail);
                    //return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage });
                    //return RedirectToAction("PRAuthorizationLists");
                    //return RedirectToAction("PRAuthorizationLists", "Authorization");

                    TblSendEmail _tblsendemail = new TblSendEmail();

                    _tblsendemail.ReferenceNo = DateTime.Now.ToString("MM/dd/yyyy hhmmss");
                    _tblsendemail.SystemCode = "SPASv2";
                    _tblsendemail.From = "ronom@stpeter.com.ph";
                    _tblsendemail.Subject = "Payment Request Authorization";
                    if (authpayclass == "APPROVER")
                    {
                        _tblsendemail.Subject = "SPASv2 For Approval - " + DateTime.Now.ToString();
                    }
                    else
                    {
                        _tblsendemail.Subject = "SPASv2 For Verification  - " + DateTime.Now.ToString();
                    }


                    _tblsendemail.Body = strBodyEmail;
                    _tblsendemail.Attachment = null;

                    IList<string> CCs = new List<string>();
                    //CCs.Add("ronom@stpeter.com.ph");
                    //CCs.Add("davidga@stpeter.com.ph");
                    CCs.Add("warrenlb@stpeter.com.ph");
                    CCs.Add("rudyab@stpeter.com.ph");
                    CCs.Add("jonab@stpeter.com.ph");
                    _tblsendemail.CCemails = CCs;
                    _tblsendemail.BCemails = null;
                    _tblsendemail.Host = "smtp-relay.gmail.com";
                    _tblsendemail.Port = "587";
                    _tblsendemail.Username = null;
                    _tblsendemail.Password = null;

                    var EmailTo = await _RepositoryUnit.PRAuthorizationRepository.GetEmailByPersonID(item, BaseUrlRepo);
                    _tblsendemail.To = EmailTo;


                    if (String.IsNullOrEmpty(EmailTo))
                    {
                        IList<qryGroupEmails> _Emails = await _RepositoryUnit.PRAuthorizationRepository.GetEmailsByGroupId(item, BaseUrlRepo);

                        foreach (var itemEmails in _Emails)
                        {

                            strBodyEmail = await this.BodyEMAIL_Authorization_Batchv2(_prnolist, item, authpayclass, _prnolist.Count(), _prnolist);
                            _tblsendemail.To = itemEmails.Emails;
                            if (authpayclass == "APPROVER")
                            {
                                _tblsendemail.Subject = "SPASv2 For Approval - " + DateTime.Now.ToString();
                            }
                            else
                            {
                                _tblsendemail.Subject = "SPASv2 For Verification  - " + DateTime.Now.ToString();
                            }
                            _resp = await this.SendEmailAuthorization(_tblsendemail, OSPUrlService);
                        }
                        return Ok(_resp);

                    }
                    else
                    {
                        _resp = await this.SendEmailAuthorization(_tblsendemail, OSPUrlService);
                    }
                }

            }

            if (_prnolisttoscctg.Count > 0)
            {

                await _RepositoryUnit.PRAuthorizationRepository.EndorseToAccounting(_prnolisttoscctg, _personid, BaseUrlRepo);
            }

            return Ok();
        }
        public async Task<IActionResult> SendEmailAuthorization_PRNO(string Prno)
        {
            string _personid = _userManager.GetUserId(this.User);

            IList<TblPaymentRequestAuth> _TblPaymentRequestAuth = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel(BaseUrlRepo);
            IList<string> _personidlist = new List<string>();
            IList<string> _prnolist = new List<string>();

            IList<string> _prnolisttoscctg = new List<string>();

            string authpayclass = string.Empty;

            if (_TblPaymentRequestAuth == null)
            {
                return Ok();
            }


            foreach (string id in _TblPaymentRequestAuth.Select(i => i.PersonID).Distinct())
            {

                _personidlist.Add(id);
            }

            PRAuthorizationModel _PRAuthorizationModel = new PRAuthorizationModel();
            foreach (var item in _personidlist)
            {

                var _positioncode = await _RepositoryUnit.PRAuthorizationRepository.GetPositioncode(item, BaseUrlRepo);


                if (_positioncode == "SYSTEM")
                {
                    _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists(item, BaseUrlRepo);
                    foreach (string prno in _PRAuthorizationModel.qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                    {
                        _prnolisttoscctg.Add(prno);
                    }
                }
                else
                {

                    _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists(item, BaseUrlRepo);

                    foreach (string prno in _PRAuthorizationModel.qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                    {
                        _prnolist.Add(prno);
                        authpayclass = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizeClass(prno, item, BaseUrlRepo);
                    }


                    strBodyEmail = await this.BodyEMAIL_Authorizationv2(_prnolist, item, authpayclass, _prnolist.Count());

                    //await _RepositoryUnit.PRAuthorizationRepository.SendEmailPRAuthorization(_tblsendemail);
                    //return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage });
                    //return RedirectToAction("PRAuthorizationLists");
                    //return RedirectToAction("PRAuthorizationLists", "Authorization");

                    TblSendEmail _tblsendemail = new TblSendEmail();

                    _tblsendemail.ReferenceNo = DateTime.Now.ToString("MM/dd/yyyy hhmmss");
                    _tblsendemail.SystemCode = "SPASv2";
                    _tblsendemail.From = "ronom@stpeter.com.ph";

                    var EmailTo = await _RepositoryUnit.PRAuthorizationRepository.GetEmailByPersonID(item, BaseUrlRepo);
                    _tblsendemail.To = EmailTo;
                    _tblsendemail.To = "ronom@stpeter.com.ph;jonab@stpeter.com.ph";
                    //_tblsendemail.To = "ronom@stpeter.com.ph";
                    _tblsendemail.Subject = "Payment Request Authorization";
                    if (authpayclass == "APPROVER")
                    {
                        _tblsendemail.Subject = "SPASv2 For Approval - " + DateTime.Now.ToString();
                    }
                    else
                    {
                        _tblsendemail.Subject = "SPASv2 For Verification  - " + DateTime.Now.ToString();
                    }

                    _tblsendemail.Body = strBodyEmail;
                    _tblsendemail.Attachment = null;

                    IList<string> CCs = new List<string>();
                    //CCs.Add("ronom@stpeter.com.ph");
                    //CCs.Add("davidga@stpeter.com.ph");
                    CCs.Add("warrenlb@stpeter.com.ph");
                    CCs.Add("rudyab@stpeter.com.ph");
                    CCs.Add("jonab@stpeter.com.ph");
                    _tblsendemail.CCemails = CCs;
                    _tblsendemail.BCemails = null;
                    _tblsendemail.Host = "smtp-relay.gmail.com";
                    _tblsendemail.Port = "587";
                    _tblsendemail.Username = null;
                    _tblsendemail.Password = null;

                    this.SendEmailAuthorization(_tblsendemail, OSPUrlService);
                }

            }

            if (_prnolisttoscctg.Count > 0)
            {

                await _RepositoryUnit.PRAuthorizationRepository.EndorseToAccounting(_prnolisttoscctg, _personid, BaseUrlRepo);
            }

            return Ok();
        }
        //public async Task PrintPO(string PONo)
        //{
        //    var dt = new DataTable();

        //    IList<qryRptPurchaseOrderDetails> _qryRptPurchaseOrderDetails = await _RepositoryUnit.PRAuthorizationRepository.GetRptPODtl(PONo, BaseUrlRepo);

        //    qryRptPurchaseOrderHdr _qryRptPurchaseOrderHdr = await _RepositoryUnit.PRAuthorizationRepository.GetRptPOHdr(PONo, BaseUrlRepo);

        //    dt = ConvertIListToDataTable(_qryRptPurchaseOrderDetails);

        //    //dt = GetEmployeeList();

        //    string mimetype = "";
        //    int extension = new int();
        //    extension = 1;


        //    var path = @"\\SPLPDEVSERVER\Spasv2$\Reports\Template\rptPOVendor.rdlc";

        //    Dictionary<string, string> parameters = new Dictionary<string, string>();
        //    parameters.Add("prmCompany", _qryRptPurchaseOrderHdr.CompanyDesc);
        //    parameters.Add("prmCompanyAddress", _qryRptPurchaseOrderHdr.Address);
        //    parameters.Add("prmVendor", _qryRptPurchaseOrderHdr.VendorName);
        //    parameters.Add("prmPayeeName", _qryRptPurchaseOrderHdr.PayeeName);
        //    parameters.Add("prmTin", _qryRptPurchaseOrderHdr.TIN);
        //    parameters.Add("prmModeOfPayments", _qryRptPurchaseOrderHdr.PayMethod);
        //    parameters.Add("prmRemarks", "NA");
        //    parameters.Add("prmPONo", PONo);
        //    parameters.Add("prmTerms", _qryRptPurchaseOrderHdr.Terms);
        //    parameters.Add("prmPayClass", _qryRptPurchaseOrderHdr.PayClass);
        //    parameters.Add("prmPODate", DateTime.Now.ToString("MM/dd/yyyy"));
        //    //parameters.Add("prmPreparedBy", "APRIL ROSE N. BAZAR");
        //    //parameters.Add("prmManagerLogistic", "OLGA R. PADERNAL");
        //    //parameters.Add("prmChapel", _qryRptPurchaseOrderHdr.Department);

        //    LocalReport localReport = new LocalReport(path);
        //    localReport.AddDataSource("DataSetPOVendor", dt);

        //    try
        //    {
        //        int ext = (int)(DateTime.Now.Ticks >> 10);
        //        var result = localReport.Execute(RenderType.Pdf, ext, parameters);
        //        using (FileStream fs = new FileStream(@"\\SPLPDEVSERVER\Spasv2$\Reports\POReport\" + PONo + ".pdf", FileMode.Create, FileAccess.Write))
        //        {
        //            fs.Write(result.MainStream, 0, result.MainStream.Length);
        //            fs.Dispose();
        //            fs.Close();
        //        }

        //        result = null;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }
        //    localReport = null;
        //    parameters.Clear();
        //    return;
        //}
        public async Task ReportPO(string PONo, decimal Deduction)
        {
            //var test = _context.Table.ToList();
            //var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Report2.rdlc";

            var dt = new DataTable();

            IList<qryRptPurchaseOrderDetails> _qryRptPurchaseOrderDetails = await _RepositoryUnit.PRAuthorizationRepository.GetRptPODtl(PONo, BaseUrlRepo);

            qryRptPurchaseOrderHdr _qryRptPurchaseOrderHdr = await _RepositoryUnit.PRAuthorizationRepository.GetRptPOHdr(PONo, BaseUrlRepo);

            dt = ConvertIListToDataTable(_qryRptPurchaseOrderDetails);

            //dt = GetEmployeeList();

            string mimetype = "";
            int extension = new int();
            extension = 1;


            //var path = @"\\SPLPDEVSERVER\Spasv2$\Reports\Template\rptPOVendor.rdlc";
            var path = Path.Combine(_ReportFilespath, "Reports", "Template", "rptPOVendor.rdlc");

            LocalReport report = new LocalReport();
            report.ReportPath = path;
            report.DataSources.Add(new ReportDataSource("DataSetPOVendor", dt));
            report.SetParameters(new[] { new ReportParameter("prmCompany", _qryRptPurchaseOrderHdr.CompanyDesc) });
            report.SetParameters(new[] { new ReportParameter("prmCompanyAddress", _qryRptPurchaseOrderHdr.Address) });
            report.SetParameters(new[] { new ReportParameter("prmVendor", _qryRptPurchaseOrderHdr.VendorName) });
            report.SetParameters(new[] { new ReportParameter("prmPayeeName", _qryRptPurchaseOrderHdr.PayeeName) });
            report.SetParameters(new[] { new ReportParameter("prmTin", _qryRptPurchaseOrderHdr.TIN) });
            report.SetParameters(new[] { new ReportParameter("prmModeOfPayments", _qryRptPurchaseOrderHdr.PayMethod) });
            report.SetParameters(new[] { new ReportParameter("prmRemarks", "NA") });
            report.SetParameters(new[] { new ReportParameter("prmPONo", PONo) });
            report.SetParameters(new[] { new ReportParameter("prmTerms", _qryRptPurchaseOrderHdr.Terms) });
            report.SetParameters(new[] { new ReportParameter("prmPayClass", _qryRptPurchaseOrderHdr.PayClass) });
            report.SetParameters(new[] { new ReportParameter("prmPODate", DateTime.Now.ToString("MM/dd/yyyy")) });
            report.SetParameters(new[] { new ReportParameter("prmDeduction", Deduction.ToString()) });




            qryPOSignatories _qryPOSignatories = new qryPOSignatories();


            _qryPOSignatories = await _RepositoryUnit.PRAuthorizationRepository.GetPOSignatories(_qryRptPurchaseOrderHdr.ReqNo, BaseUrlRepo);


            report.SetParameters(new[] { new ReportParameter("prmPreparedByName", _qryPOSignatories.PreparedByName) });
            report.SetParameters(new[] { new ReportParameter("prmPreparedByPosition", _qryPOSignatories.PreparedByPosition) });

            report.SetParameters(new[] { new ReportParameter("prmReviewedByName", _qryPOSignatories.ReviewedByName) });
            report.SetParameters(new[] { new ReportParameter("prmReviewedByPosition", _qryPOSignatories.ReviewedByPosition) });

            report.SetParameters(new[] { new ReportParameter("prmApprovedByName", _qryPOSignatories.ApprovedByName) });
            report.SetParameters(new[] { new ReportParameter("prmApprovedByPosition", _qryPOSignatories.ApprovedByPosition) });




            //byte[] pdf = report.Render("PDF");
            //return File(pdf, "application/pdf");
            string format1 = "PDF";
            string ext1 = "pdf";
            string mimetype1 = "application/pdf";

            try
            {
                var pdf = report.Render(format1);
                var file = File(pdf, mimetype1, "report." + ext1);

                // Specify the path where you want to save the PDF file
                //string filePath = @"\\SPLPDEVSERVER\Spasv2$\Reports\POReport\" + PONo + ".pdf";
                string filePath = Path.Combine(_ReportFilespath, "Reports", "POReport", PONo + ".pdf");



                // Access the actual file content from the FileContentResult
                byte[] fileContent = file.FileContents;

                // Use FileStream to write the PDF content to the file
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(fileContent, 0, fileContent.Length);
                    // No need to call fs.Dispose(), the using statement takes care of it
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return;

        }
        public async Task ReportPurchaseOrderConsolidated(IList<string> reqno,string vendorname, decimal Deduction)
        {
            //var test = _context.Table.ToList();
            //var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Report2.rdlc";
            qryRptPurchaseOrderHdr _qryRptPurchaseOrderHdr = new qryRptPurchaseOrderHdr();
            var dt = new DataTable();

            IList<qryRptPurchaseOrderConsolidated> _qryRptPurchaseOrderDetails = await _RepositoryUnit.PRAuthorizationRepository.GetRptPurchaseOrderConsolidated(reqno, vendorname, BaseUrlRepo);

             _qryRptPurchaseOrderHdr = await _RepositoryUnit.PRAuthorizationRepository.GetRptPOHdr(_qryRptPurchaseOrderDetails.FirstOrDefault().PONo, BaseUrlRepo);

            TblPurchaseorderhdr _TblPurchaseorderhdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(BaseUrlRepo, _qryRptPurchaseOrderDetails.FirstOrDefault().PONo);


            dt = ConvertIListToDataTable(_qryRptPurchaseOrderDetails);

            //dt = GetEmployeeList();

            string mimetype = "";
            int extension = new int();
            extension = 1;


            //var path = @"\\SPLPDEVSERVER\Spasv2$\Reports\Template\rptPOVendor.rdlc";
            var path = Path.Combine(_ReportFilespath, "Reports", "Template", "rptPurchaseOrderConsolidated.rdlc");

            LocalReport report = new LocalReport();
            report.ReportPath = path;
            report.DataSources.Add(new ReportDataSource("DataSetPurchaseOrder", dt));
            report.SetParameters(new[] { new ReportParameter("prmCompany", _qryRptPurchaseOrderHdr.CompanyDesc) });
            report.SetParameters(new[] { new ReportParameter("prmCompanyAddress", _qryRptPurchaseOrderHdr.Address) });
            report.SetParameters(new[] { new ReportParameter("prmVendor", _qryRptPurchaseOrderHdr.VendorName) });
            report.SetParameters(new[] { new ReportParameter("prmPayeeName", _qryRptPurchaseOrderHdr.PayeeName) });
            report.SetParameters(new[] { new ReportParameter("prmTin", _qryRptPurchaseOrderHdr.TIN) });
            report.SetParameters(new[] { new ReportParameter("prmModeOfPayments", _qryRptPurchaseOrderHdr.PayMethod) });
            report.SetParameters(new[] { new ReportParameter("prmRemarks", "NA") });
            //report.SetParameters(new[] { new ReportParameter("prmPONo", PONo) });
            report.SetParameters(new[] { new ReportParameter("prmTerms", _qryRptPurchaseOrderHdr.Terms) });
            report.SetParameters(new[] { new ReportParameter("prmPayClass", _qryRptPurchaseOrderHdr.PayClass) });
            report.SetParameters(new[] { new ReportParameter("prmPODate", DateTime.Now.ToString("MM/dd/yyyy")) });
            report.SetParameters(new[] { new ReportParameter("prmDeduction", Deduction.ToString()) });




            qryPOSignatories _qryPOSignatories = new qryPOSignatories();


            _qryPOSignatories = await _RepositoryUnit.PRAuthorizationRepository.GetPOSignatories(_qryRptPurchaseOrderHdr.ReqNo, BaseUrlRepo);


            report.SetParameters(new[] { new ReportParameter("prmPreparedByName", _qryPOSignatories.PreparedByName) });
            report.SetParameters(new[] { new ReportParameter("prmPreparedByPosition", _qryPOSignatories.PreparedByPosition) });

            report.SetParameters(new[] { new ReportParameter("prmReviewedByName", _qryPOSignatories.ReviewedByName) });
            report.SetParameters(new[] { new ReportParameter("prmReviewedByPosition", _qryPOSignatories.ReviewedByPosition) });

            report.SetParameters(new[] { new ReportParameter("prmApprovedByName", _qryPOSignatories.ApprovedByName) });
            report.SetParameters(new[] { new ReportParameter("prmApprovedByPosition", _qryPOSignatories.ApprovedByPosition) });




            //byte[] pdf = report.Render("PDF");
            //return File(pdf, "application/pdf");
            string format1 = "PDF";
            string ext1 = "pdf";
            string mimetype1 = "application/pdf";

            try
            {
                var pdf = report.Render(format1);
                var file = File(pdf, mimetype1, "report." + ext1);

                // Specify the path where you want to save the PDF file
                //string filePath = @"\\SPLPDEVSERVER\Spasv2$\Reports\POReport\" + PONo + ".pdf";


                string errString = _TblPurchaseorderhdr.Remarks;
                string correctString = errString.Replace("/", "-").Replace(" ", "");

                string filePath = Path.Combine(_ReportFilespath, "Reports", "POReport", "PO"+ DateTime.Now.ToString("MMddyyyyhhmmss") + ".pdf");




                // Access the actual file content from the FileContentResult
                byte[] fileContent = file.FileContents;

                // Use FileStream to write the PDF content to the file
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(fileContent, 0, fileContent.Length);
                    // No need to call fs.Dispose(), the using statement takes care of it
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return;

        }
        public async Task PrintChapelAdvisoryPO(string PONo)
        {
            var dtPO = new DataTable();
            IList<qryRptPurchaseOrderDetails> _qryRptPurchaseOrderDetails = new List<qryRptPurchaseOrderDetails>();
            _qryRptPurchaseOrderDetails = await _RepositoryUnit.PRAuthorizationRepository.GetRptPODtl(PONo, BaseUrlRepo);

            qryRptPurchaseOrderHdr _qryRptPurchaseOrderHdr = await _RepositoryUnit.PRAuthorizationRepository.GetRptPOHdr(PONo, BaseUrlRepo);

            dtPO = ConvertIListToDataTable(_qryRptPurchaseOrderDetails);

            //dt = GetEmployeeList();

            string mimetype = "";
            int extension = 1;

            //var path = @"\\SPLPDEVSERVER\Spasv2$\Reports\Template\rptChapelAdvisory.rdlc";
            string filePath = Path.Combine(_ReportFilespath, "Template", "rptChapelAdvisory.rdlc");






            Dictionary<string, string> parametersPO = new Dictionary<string, string>();
            parametersPO.Add("prmCompany", _qryRptPurchaseOrderHdr.CompanyDesc);
            parametersPO.Add("prmCompanyAddress", _qryRptPurchaseOrderHdr.Address);
            parametersPO.Add("prmVendor", _qryRptPurchaseOrderHdr.VendorName);
            parametersPO.Add("prmPayeeName", _qryRptPurchaseOrderHdr.PayeeName);
            parametersPO.Add("prmTin", _qryRptPurchaseOrderHdr.TIN);
            parametersPO.Add("prmModeOfPayments", _qryRptPurchaseOrderHdr.PayMethod);
            parametersPO.Add("prmRemarks", "NA");
            parametersPO.Add("prmPONo", PONo);
            parametersPO.Add("prmTerms", _qryRptPurchaseOrderHdr.Terms);
            parametersPO.Add("prmPayClass", _qryRptPurchaseOrderHdr.PayClass);
            parametersPO.Add("prmPODate", DateTime.Now.ToString("MM/dd/yyyy"));
            parametersPO.Add("prmPreparedBy", "APRIL ROSE N. BAZAR");
            parametersPO.Add("prmManagerLogistic", "OLGA R. PADERNAL");
            parametersPO.Add("prmChapel", _qryRptPurchaseOrderHdr.Department);


            //LocalReport localReportPO = new LocalReport(path);
            //localReportPO.AddDataSource("DataSet1", dtPO);

            //localReport.SetParameters(new[] { new ReportParameter("prmCompany", _qryRptPurchaseOrderHdr.CompanyDesc) });
            //localReport.SetParameters(new[] { new ReportParameter("prmCompanyAddress", _qryRptPurchaseOrderHdr.Address) });
            //localReport.SetParameters(new[] { new ReportParameter("prmVendor", _qryRptPurchaseOrderHdr.VendorName) });
            //localReport.SetParameters(new[] { new ReportParameter("prmPayeeName", _qryRptPurchaseOrderHdr.PayeeName) });
            //localReport.SetParameters(new[] { new ReportParameter("prmTin", _qryRptPurchaseOrderHdr.TIN) });
            //localReport.SetParameters(new[] { new ReportParameter("prmModeOfPayments", _qryRptPurchaseOrderHdr.PayMethod) });
            //localReport.SetParameters(new[] { new ReportParameter("prmRemarks", "NA") });
            //localReport.SetParameters(new[] { new ReportParameter("prmPONo", PONo) });
            //localReport.SetParameters(new[] { new ReportParameter("prmTerms", _qryRptPurchaseOrderHdr.Terms) });
            //localReport.SetParameters(new[] { new ReportParameter("prmPayClass", _qryRptPurchaseOrderHdr.PayClass) });
            //localReport.SetParameters(new[] { new ReportParameter("prmPODate", DateTime.Now.ToString("MM/dd/yyyy")) });
            try
            {
                int ext = (int)(DateTime.Now.Ticks >> 10);
                //var result = localReportPO.Execute(RenderType.Word, ext, parametersPO);

                //using (FileStream fs = new FileStream(@"\\SPLPDEVSERVER\Spasv2$\Reports\ChapelAdvisory\" + PONo + ".pdf", FileMode.Create,FileAccess.Write))
                //{
                //    fs.Write(result.MainStream, 0, result.MainStream.Length);
                //    fs.Dispose();
                //}

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            //localReportPO = null;
            return;
        }
        public async Task Report(string PONo)
        {
            //var test = _context.Table.ToList();
            //var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Report2.rdlc";

            var dtPO = new DataTable();
            IList<qryRptPurchaseOrderDetails> _qryRptPurchaseOrderDetails = new List<qryRptPurchaseOrderDetails>();
            _qryRptPurchaseOrderDetails = await _RepositoryUnit.PRAuthorizationRepository.GetRptPODtl(PONo, BaseUrlRepo);

            qryRptPurchaseOrderHdr _qryRptPurchaseOrderHdr = await _RepositoryUnit.PRAuthorizationRepository.GetRptPOHdr(PONo, BaseUrlRepo);

            dtPO = ConvertIListToDataTable(_qryRptPurchaseOrderDetails);

            //dt = GetEmployeeList();

            //string mimetype = "";
            //int extension = 1;

            //var path = @"\\SPLPDEVSERVER\Spasv2$\Reports\Template\rptChapelAdvisory.rdlc";
            //var path = @"C:\SIS\SPASv2\wwwroot\Reports\Template\rptChapelAdvisory.rdlc";
            var path = Path.Combine(_ReportFilespath, "Reports", "Template", "rptChapelAdvisory.rdlc");

            using var report = new LocalReport();
            report.ReportPath = path;

            report.DataSources.Add(new ReportDataSource("DataSet1", dtPO));

            report.SetParameters(new[] { new ReportParameter("prmCompany", _qryRptPurchaseOrderHdr.CompanyDesc) });
            report.SetParameters(new[] { new ReportParameter("prmCompanyAddress", _qryRptPurchaseOrderHdr.Address) });
            report.SetParameters(new[] { new ReportParameter("prmVendor", _qryRptPurchaseOrderHdr.VendorName) });
            report.SetParameters(new[] { new ReportParameter("prmPayeeName", _qryRptPurchaseOrderHdr.PayeeName) });
            report.SetParameters(new[] { new ReportParameter("prmTin", _qryRptPurchaseOrderHdr.TIN) });
            report.SetParameters(new[] { new ReportParameter("prmModeOfPayments", _qryRptPurchaseOrderHdr.PayMethod) });
            report.SetParameters(new[] { new ReportParameter("prmRemarks", "NA") });
            report.SetParameters(new[] { new ReportParameter("prmPONo", PONo) });
            report.SetParameters(new[] { new ReportParameter("prmTerms", _qryRptPurchaseOrderHdr.Terms) });
            report.SetParameters(new[] { new ReportParameter("prmPayClass", _qryRptPurchaseOrderHdr.PayClass) });
            report.SetParameters(new[] { new ReportParameter("prmPODate", DateTime.Now.ToString("MM / dd / yyyy")) });
            report.SetParameters(new[] { new ReportParameter("prmPreparedBy", "APRIL ROSE N. BAZAR") });
            report.SetParameters(new[] { new ReportParameter("prmManagerLogistic", "OLGA R. PADERNAL") });
            report.SetParameters(new[] { new ReportParameter("prmChapel", _qryRptPurchaseOrderHdr.Department) });

            int ext = (int)(DateTime.Now.Ticks >> 10);
            //byte[] pdf = report.Render("PDF");

            string format1 = "PDF";
            string ext1 = "pdf";
            string mimetype1 = "application/pdf";

            try
            {
                var pdf = report.Render(format1);
                var file = File(pdf, mimetype1, "report." + ext1);

                // Specify the path where you want to save the PDF file
                //string filePath = @"\\SPLPDEVSERVER\Spasv2$\Reports\ChapelAdvisory\" + PONo + ".pdf";
                string filePath = Path.Combine(_ReportFilespath, "Reports", "ChapelAdvisory", PONo + ".pdf");



                //filePath = "\\\\192.168.23.25:88\\SPASv2\\wwwroot\\Reports\\ChapelAdvisory\\" + PONo + ".pdf";

                // Access the actual file content from the FileContentResult
                byte[] fileContent = file.FileContents;

                // Use FileStream to write the PDF content to the file
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(fileContent, 0, fileContent.Length);
                    // No need to call fs.Dispose(), the using statement takes care of it
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            //using (FileStream fs = new FileStream(@"\\SPLPDEVSERVER\Spasv2$\Reports\ChapelAdvisory\" + PONo + ".pdf", FileMode.Create, FileAccess.Write))
            //{
            //    fs.Write(file.MainStream, 0, file.MainStream.Length);
            //    fs.Dispose();
            //}

            // localReportPO.Execute(RenderType.Word, ext, parametersPO);
            return;
        }
        public async Task ReportCasketOrder(string PONo)
        {
            //var test = _context.Table.ToList();
            //var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Report2.rdlc";

            var dtPO = new DataTable();
            IList<qryRptPurchaseOrderDetails> _qryRptPurchaseOrderDetails = new List<qryRptPurchaseOrderDetails>();
            _qryRptPurchaseOrderDetails = await _RepositoryUnit.PRAuthorizationRepository.GetRptPODtl(PONo, BaseUrlRepo);

            qryRptPurchaseOrderHdr _qryRptPurchaseOrderHdr = await _RepositoryUnit.PRAuthorizationRepository.GetRptPOHdr(PONo, BaseUrlRepo);

            dtPO = ConvertIListToDataTable(_qryRptPurchaseOrderDetails);

            //dt = GetEmployeeList();

            //string mimetype = "";
            //int extension = 1;

            //var path = @"\\SPLPDEVSERVER\Spasv2$\Reports\Template\rptChapelAdvisory.rdlc";
            //var path = @"C:\SIS\SPASv2\wwwroot\Reports\Template\rptChapelAdvisory.rdlc";
            var path = Path.Combine(_ReportFilespath, "Reports", "Template", "rptChapelAdvisory.rdlc");

            using var report = new LocalReport();
            report.ReportPath = path;

            report.DataSources.Add(new ReportDataSource("DataSet1", dtPO));

            report.SetParameters(new[] { new ReportParameter("prmCompany", _qryRptPurchaseOrderHdr.CompanyDesc) });
            report.SetParameters(new[] { new ReportParameter("prmCompanyAddress", _qryRptPurchaseOrderHdr.Address) });
            report.SetParameters(new[] { new ReportParameter("prmVendor", _qryRptPurchaseOrderHdr.VendorName) });
            report.SetParameters(new[] { new ReportParameter("prmPayeeName", _qryRptPurchaseOrderHdr.PayeeName) });
            report.SetParameters(new[] { new ReportParameter("prmTin", _qryRptPurchaseOrderHdr.TIN) });
            report.SetParameters(new[] { new ReportParameter("prmModeOfPayments", _qryRptPurchaseOrderHdr.PayMethod) });
            report.SetParameters(new[] { new ReportParameter("prmRemarks", "NA") });
            report.SetParameters(new[] { new ReportParameter("prmPONo", PONo) });
            report.SetParameters(new[] { new ReportParameter("prmTerms", _qryRptPurchaseOrderHdr.Terms) });
            report.SetParameters(new[] { new ReportParameter("prmPayClass", _qryRptPurchaseOrderHdr.PayClass) });
            report.SetParameters(new[] { new ReportParameter("prmPODate", DateTime.Now.ToString("MM / dd / yyyy")) });
            report.SetParameters(new[] { new ReportParameter("prmPreparedBy", "APRIL ROSE N. BAZAR") });
            report.SetParameters(new[] { new ReportParameter("prmManagerLogistic", "OLGA R. PADERNAL") });
            report.SetParameters(new[] { new ReportParameter("prmChapel", _qryRptPurchaseOrderHdr.Department) });

            int ext = (int)(DateTime.Now.Ticks >> 10);
            //byte[] pdf = report.Render("PDF");

            string format1 = "PDF";
            string ext1 = "pdf";
            string mimetype1 = "application/pdf";

            try
            {
                var pdf = report.Render(format1);
                var file = File(pdf, mimetype1, "report." + ext1);

                // Specify the path where you want to save the PDF file
                //string filePath = @"\\SPLPDEVSERVER\Spasv2$\Reports\ChapelAdvisory\" + PONo + ".pdf";
                string filePath = Path.Combine(_ReportFilespath, "Reports", "ChapelAdvisory", PONo + ".pdf");



                //filePath = "\\\\192.168.23.25:88\\SPASv2\\wwwroot\\Reports\\ChapelAdvisory\\" + PONo + ".pdf";

                // Access the actual file content from the FileContentResult
                byte[] fileContent = file.FileContents;

                // Use FileStream to write the PDF content to the file
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(fileContent, 0, fileContent.Length);
                    // No need to call fs.Dispose(), the using statement takes care of it
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            //using (FileStream fs = new FileStream(@"\\SPLPDEVSERVER\Spasv2$\Reports\ChapelAdvisory\" + PONo + ".pdf", FileMode.Create, FileAccess.Write))
            //{
            //    fs.Write(file.MainStream, 0, file.MainStream.Length);
            //    fs.Dispose();
            //}

            // localReportPO.Execute(RenderType.Word, ext, parametersPO);
            return;
        }
        public async Task ReportCasketOrderConsolidated(string PONo)
        {
            //var test = _context.Table.ToList();
            //var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Report2.rdlc";

            var dtPO = new DataTable();
            IList<qryRptPurchaseOrderDetails> _qryRptPurchaseOrderDetails = new List<qryRptPurchaseOrderDetails>();
            _qryRptPurchaseOrderDetails = await _RepositoryUnit.PRAuthorizationRepository.GetRptPODtl(PONo, BaseUrlRepo);

            qryRptPurchaseOrderHdr _qryRptPurchaseOrderHdr = await _RepositoryUnit.PRAuthorizationRepository.GetRptPOHdr(PONo, BaseUrlRepo);

            dtPO = ConvertIListToDataTable(_qryRptPurchaseOrderDetails);

            //dt = GetEmployeeList();

            //string mimetype = "";
            //int extension = 1;

            //var path = @"\\SPLPDEVSERVER\Spasv2$\Reports\Template\rptChapelAdvisory.rdlc";
            //var path = @"C:\SIS\SPASv2\wwwroot\Reports\Template\rptChapelAdvisory.rdlc";
            var path = Path.Combine(_ReportFilespath, "Reports", "Template", "rptChapelAdvisory.rdlc");

            using var report = new LocalReport();
            report.ReportPath = path;

            report.DataSources.Add(new ReportDataSource("DataSet1", dtPO));

            report.SetParameters(new[] { new ReportParameter("prmCompany", _qryRptPurchaseOrderHdr.CompanyDesc) });
            report.SetParameters(new[] { new ReportParameter("prmCompanyAddress", _qryRptPurchaseOrderHdr.Address) });
            report.SetParameters(new[] { new ReportParameter("prmVendor", _qryRptPurchaseOrderHdr.VendorName) });
            report.SetParameters(new[] { new ReportParameter("prmPayeeName", _qryRptPurchaseOrderHdr.PayeeName) });
            report.SetParameters(new[] { new ReportParameter("prmTin", _qryRptPurchaseOrderHdr.TIN) });
            report.SetParameters(new[] { new ReportParameter("prmModeOfPayments", _qryRptPurchaseOrderHdr.PayMethod) });
            report.SetParameters(new[] { new ReportParameter("prmRemarks", "NA") });
            report.SetParameters(new[] { new ReportParameter("prmPONo", PONo) });
            report.SetParameters(new[] { new ReportParameter("prmTerms", _qryRptPurchaseOrderHdr.Terms) });
            report.SetParameters(new[] { new ReportParameter("prmPayClass", _qryRptPurchaseOrderHdr.PayClass) });
            report.SetParameters(new[] { new ReportParameter("prmPODate", DateTime.Now.ToString("MM / dd / yyyy")) });
            report.SetParameters(new[] { new ReportParameter("prmPreparedBy", "APRIL ROSE N. BAZAR") });
            report.SetParameters(new[] { new ReportParameter("prmManagerLogistic", "OLGA R. PADERNAL") });
            report.SetParameters(new[] { new ReportParameter("prmChapel", _qryRptPurchaseOrderHdr.Department) });

            int ext = (int)(DateTime.Now.Ticks >> 10);
            //byte[] pdf = report.Render("PDF");

            string format1 = "PDF";
            string ext1 = "pdf";
            string mimetype1 = "application/pdf";

            try
            {
                var pdf = report.Render(format1);
                var file = File(pdf, mimetype1, "report." + ext1);

                // Specify the path where you want to save the PDF file
                //string filePath = @"\\SPLPDEVSERVER\Spasv2$\Reports\ChapelAdvisory\" + PONo + ".pdf";
                string filePath = Path.Combine(_ReportFilespath, "Reports", "ChapelAdvisory", PONo + ".pdf");



                //filePath = "\\\\192.168.23.25:88\\SPASv2\\wwwroot\\Reports\\ChapelAdvisory\\" + PONo + ".pdf";

                // Access the actual file content from the FileContentResult
                byte[] fileContent = file.FileContents;

                // Use FileStream to write the PDF content to the file
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(fileContent, 0, fileContent.Length);
                    // No need to call fs.Dispose(), the using statement takes care of it
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            //using (FileStream fs = new FileStream(@"\\SPLPDEVSERVER\Spasv2$\Reports\ChapelAdvisory\" + PONo + ".pdf", FileMode.Create, FileAccess.Write))
            //{
            //    fs.Write(file.MainStream, 0, file.MainStream.Length);
            //    fs.Dispose();
            //}

            // localReportPO.Execute(RenderType.Word, ext, parametersPO);
            return;
        }
        public static DataTable ConvertIListToDataTable<T>(IList<T> dataList)
        {
            DataTable dataTable = new DataTable();

            // Get the properties of the type
            var properties = typeof(T).GetProperties();

            // Create columns in DataTable based on properties of the type
            foreach (var property in properties)
            {
                dataTable.Columns.Add(property.Name, property.PropertyType);
            }

            // Populate DataTable with data from IList
            foreach (var data in dataList)
            {
                DataRow row = dataTable.NewRow();
                foreach (var property in properties)
                {
                    row[property.Name] = property.GetValue(data);
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
        public async Task<IActionResult> SendEmailAuthorization_PRNO(AuthorizationParams authorizationParams)
        {
            string _personid = _userManager.GetUserId(this.User);
            IList<string> _personidlist = new List<string>();
            IList<string> _prnolist = new List<string>();
            IList<string> _prnolisttoscctg = new List<string>();

            foreach (var itemprno in authorizationParams.ReqNo)
            {

                TblPaymentRequestAuth _TblPaymentRequestAuth_PRNO = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel(itemprno, BaseUrlRepo);
                _personidlist.Clear();

                string authpayclass = string.Empty;

                if (_TblPaymentRequestAuth_PRNO == null)
                {

                    authorizationParams.ReqType = "PO";


                    if (authorizationParams.ReqType == "PO")
                    {
                        TblPurchaseorderhdr _tblPurchaseorderhdr = new TblPurchaseorderhdr();
                        _tblPurchaseorderhdr.PONo = "PO" + itemprno;
                        _tblPurchaseorderhdr.Reqno = itemprno;
                        _tblPurchaseorderhdr.PODate = DateTime.Now;
                        _tblPurchaseorderhdr.Active = false;
                        _tblPurchaseorderhdr.Remarks = "PO as of " + DateTime.Now.Date;
                        _tblPurchaseorderhdr.Printed = false;
                        _tblPurchaseorderhdr.AuditUser = _personid;
                        _tblPurchaseorderhdr.AuditDate = DateTime.Now;
                        _tblPurchaseorderhdr.TrxMonth = "JAN24";
                        _tblPurchaseorderhdr.TrxWeek = 0;

                        await _RepositoryUnit.TblPurchaseorderhdrRepository.PostCreatePurchaseOrderHdr(BaseUrlRepo, _tblPurchaseorderhdr);
                    }




                    return Ok();
                }

                _personidlist.Add(_TblPaymentRequestAuth_PRNO.PersonID);

                PRAuthorizationModel _PRAuthorizationModel = new PRAuthorizationModel();
                foreach (var item in _personidlist)
                {

                    var _positioncode = await _RepositoryUnit.PRAuthorizationRepository.GetPositioncode(item, BaseUrlRepo);


                    if (_positioncode == "SYSTEM")
                    {
                        _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists(item, BaseUrlRepo);
                        foreach (string prno in _PRAuthorizationModel.qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                        {
                            _prnolisttoscctg.Add(prno);
                        }
                    }
                    else
                    {
                        //SINGLE
                        _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists(item, BaseUrlRepo);

                        _prnolist.Clear();
                        foreach (string prno in _PRAuthorizationModel.qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                        {
                            _prnolist.Add(prno);
                            authpayclass = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizeClass(prno, item, BaseUrlRepo);
                        }

                        strBodyEmail = await this.BodyEMAIL_Authorizationv2(_prnolist, item, authpayclass, _prnolist.Count());

                        //await _RepositoryUnit.PRAuthorizationRepository.SendEmailPRAuthorization(_tblsendemail);
                        //return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage });
                        //return RedirectToAction("PRAuthorizationLists");
                        //return RedirectToAction("PRAuthorizationLists", "Authorization");

                        TblSendEmail _tblsendemail = new TblSendEmail();

                        _tblsendemail.ReferenceNo = DateTime.Now.ToString("MM/dd/yyyy hhmmss");
                        _tblsendemail.SystemCode = "SPASv2";
                        _tblsendemail.From = "ronom@stpeter.com.ph";
                        _tblsendemail.To = "ronom@stpeter.com.ph";

                        _tblsendemail.Subject = "Payment Request Authorization";
                        if (authpayclass == "APPROVER")
                        {
                            _tblsendemail.Subject = "SPASv2 For Approval - " + DateTime.Now.ToString();
                        }
                        else
                        {
                            _tblsendemail.Subject = "SPASv2 For Verification  - " + DateTime.Now.ToString();
                        }


                        _tblsendemail.Body = strBodyEmail;
                        _tblsendemail.Attachment = null;

                        IList<string> CCs = new List<string>();
                        //CCs.Add("ronom@stpeter.com.ph");
                        //CCs.Add("davidga@stpeter.com.ph");
                        CCs.Add("warrenlb@stpeter.com.ph");
                        CCs.Add("rudyab@stpeter.com.ph");
                        CCs.Add("jonab@stpeter.com.ph");
                        _tblsendemail.CCemails = CCs;
                        _tblsendemail.BCemails = null;
                        _tblsendemail.Host = "smtp-relay.gmail.com";
                        _tblsendemail.Port = "587";
                        _tblsendemail.Username = null;
                        _tblsendemail.Password = null;

                        this.SendEmailAuthorization(_tblsendemail, OSPUrlService);
                    }

                }



            }

            if (_prnolisttoscctg.Count > 0)
            {
                await _RepositoryUnit.PRAuthorizationRepository.EndorseToAccounting(_prnolisttoscctg, _personid, BaseUrlRepo);
            }
            // IList<TblPaymentRequestAuth> _TblPaymentRequestAuth = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel();

            return Ok();
        }
        public async Task<TblResponse> SendEmailAuthorization_PRNO_Batch(AuthorizationParams authorizationParams)
        {
            _resp = new TblResponse();
            try
            {
                string batchPRNo = string.Empty;
                string _personid = _userManager.GetUserId(this.User);
                string authpayclass = string.Empty;
                IList<string> _personidlist = new List<string>();
                IList<string> _prnolist = new List<string>();
                IList<string> _prnolisttoscctg = new List<string>();
                IList<string> _batchPRNolist = new List<string>();
                TblSendEmail _tblsendemail = new TblSendEmail();

                foreach (var itemprno in authorizationParams.ReqNo)
                {
                    batchPRNo = await _RepositoryUnit.PRAuthorizationRepository.GetBatchNoByPRNo(itemprno, BaseUrlRepo);

                    if (!_batchPRNolist.Contains(batchPRNo))
                    {
                        _batchPRNolist.Add(batchPRNo);
                    }
                }

                TblPaymentRequestAuth _TblPaymentRequestAuth_PRNO;
                PRAuthorizationModel _PRAuthorizationModel = new PRAuthorizationModel();
                _personidlist.Clear();

                foreach (var itemprno in authorizationParams.ReqNo)
                {
                    _TblPaymentRequestAuth_PRNO = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel(itemprno, BaseUrlRepo);

                    if (_TblPaymentRequestAuth_PRNO.Remarks == "FOR PAYMENT")
                    {
                        authorizationParams.ReqType = "PO";
                        if (authorizationParams.ReqType == "PO")
                        {
                            //string CompanyCode = await _RepositoryUnit.TblRequisitionRepository.GetCompanyCode(BaseUrlRepo, itemprno);
                            //string NewPONo = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetLatestPONo(BaseUrlRepo, CompanyCode); 
                            foreach (var reqno in authorizationParams.ReqNo)
                            {
                                TblPurchaseorderhdr _tblPurchaseorderhdr = new TblPurchaseorderhdr();
                                _tblPurchaseorderhdr.PONo = "1";
                                _tblPurchaseorderhdr.Reqno = reqno;
                                _tblPurchaseorderhdr.PODate = DateTime.Now;
                                _tblPurchaseorderhdr.Active = false;
                                _tblPurchaseorderhdr.Remarks = "PO as of " + DateTime.Now.Date;
                                _tblPurchaseorderhdr.Printed = false;
                                _tblPurchaseorderhdr.AuditUser = _personid;
                                _tblPurchaseorderhdr.AuditDate = DateTime.Now;
                                _tblPurchaseorderhdr.TrxMonth = "JAN24";
                                _tblPurchaseorderhdr.TrxWeek = 0;

                                try
                                {
                                    _resp = await _RepositoryUnit.TblPurchaseorderhdrRepository.PostCreatePurchaseOrderHdr(BaseUrlRepo, _tblPurchaseorderhdr);
                                }
                                catch (Exception e)
                                {
                                    _resp.Status = "FAILED";
                                    _resp.ErrorMessage = e.Message + "ERROR PO insert";
                                    return _resp;
                                }

                            }

                            string _BANo = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetLastestBANo(BaseUrlRepo,"PO", "12345");


                            foreach (var reqno in authorizationParams.ReqNo)
                            {
                                TblBatchApproval _TblBatchApproval = new TblBatchApproval();

                                _TblBatchApproval.BANo = _BANo;
                                _TblBatchApproval.ReqNo = reqno;
                                _TblBatchApproval.ReqType = "PO";
                                _TblBatchApproval.Active = false;
                                _TblBatchApproval.AuditUser = _personid;
                                _TblBatchApproval.AuditDate = DateTime.Now;


                                try
                                {
                                    _resp = await _RepositoryUnit.TblPurchaseorderhdrRepository.CreateBatchApproval(BaseUrlRepo, _TblBatchApproval);
                                }
                                catch (Exception e)
                                {
                                    _resp.Status = "FAILED";
                                    _resp.ErrorMessage = e.Message + "ERROR PO insert";
                                    return _resp;
                                }

                            }

                            var _qryreqinfo = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionInfo(BaseUrlRepo, itemprno);



                            try
                            {
                                await this.Report("PONO");
                                await this.ReportPO("PONO", _qryreqinfo.Deduction);
                                /////start generate PO PDF
                                //await this.ReportCasketOrder(_qryreqinfo.PONo);
                                //await this.ReportCasketOrderConsolidated(_qryreqinfo.PONo);
                                //await this.ReportPurchaseOrderConsolidated(authorizationParams.ReqNo, _qryreqinfo.Vendor, 0);

                            }
                            catch (Exception e)
                            {

                                _resp.Status = "FAILED";
                                _resp.ErrorMessage = e.Message + "ERROR PO PDF";
                                return _resp;

                            }

                            qryRptPurchaseOrderHdr _qryRptPurchaseOrderHdr = await _RepositoryUnit.PRAuthorizationRepository.GetRptPOHdr("PONO", BaseUrlRepo);

                            _tblsendemail = new TblSendEmail();
                            _tblsendemail.ReferenceNo = DateTime.Now.ToString("MM/dd/yyyy hhmmss");
                            _tblsendemail.SystemCode = "SPASv2";
                            _tblsendemail.From = "ronom@stpeter.com.ph";
                            _tblsendemail.To = "ronom@stpeter.com.ph";


                            _tblsendemail.Subject = "SPASv2 Advisory - Casket Order - " + DateTime.Now.ToString();
                            IList<string> strList = new List<string>();
                            string POReport = Path.Combine(_ReportFilespath, "Reports", "ChapelAdvisory", "" + ".pdf");
                            strList.Add(POReport);
                            _tblsendemail.Body = await this.BodyEMAIL_ChapelAdvisory();
                            _tblsendemail.Attachment = strList;
                            IList<string> CCs = new List<string>();
                            CCs.Add("warrenlb@stpeter.com.ph");
                            CCs.Add("rudyab@stpeter.com.ph");
                            CCs.Add("jonab@stpeter.com.ph");
                            CCs.Add("olgabr@stpeter.com.ph");
                            CCs.Add("aprilnb@stpeter.com.ph");
                            CCs.Add("owensd@stpeter.com.ph");
                            CCs.Add("markae@stpeter.com.ph");

                            _tblsendemail.CCemails = CCs;
                            //_tblsendemail.BCemails = null;
                            IList<string> _bcc = new List<string>();
                            _bcc.Add("ronom@stpeter.com.ph");
                            _bcc.Add("warrenlb@stpeter.com.ph");
                            _bcc.Add("rudyab@stpeter.com.ph");
                            _bcc.Add("jonab@stpeter.com.ph");
                            _tblsendemail.BCemails = _bcc;
                            _tblsendemail.Host = "smtp-relay.gmail.com";
                            _tblsendemail.Port = "587";
                            _tblsendemail.Username = null;
                            _tblsendemail.Password = null;
                            this.SendEmailAuthorization(_tblsendemail, OSPUrlService);


                            ///////Sending to Vendor
                            _tblsendemail.Subject = "SPASv2 Advisory - Purchase Order - " + DateTime.Now.ToString();
                            strList = new List<string>();
                            POReport = Path.Combine(_ReportFilespath, "Reports", "POReport", "PONO" + ".pdf");
                            strList.Add(POReport);
                            _tblsendemail.Attachment = strList;
                            _tblsendemail.Body = await this.BodyEMAIL_ChapelAdvisory_Vendor(_qryRptPurchaseOrderHdr.VendorName, _qryreqinfo.Remarks);
                            this.SendEmailAuthorization(_tblsendemail, OSPUrlService);

                            ///////Sending to GCM
                            _tblsendemail.Subject = "SPASv2 Advisory - Casket Order - " + DateTime.Now.ToString();
                            strList = new List<string>();
                            POReport = Path.Combine(_ReportFilespath, "Reports", "ChapelAdvisory", "PONO" + ".pdf");
                            strList.Add(POReport);
                            _tblsendemail.Attachment = strList;
                            _tblsendemail.Body = await this.BodyEMAIL_ChapelAdvisory_GCM();
                            this.SendEmailAuthorization(_tblsendemail, OSPUrlService);

                            _resp.Status = "FAILED";
                            _resp.ErrorMessage = "PROCESS COMPLETED. SUCCESFULLY CREATED PURCHASE ORDER";

                        }

                        return _resp;
                    }

                    if (!_personidlist.Contains(_TblPaymentRequestAuth_PRNO.PersonID))
                    {
                        _personidlist.Add(_TblPaymentRequestAuth_PRNO.PersonID);
                    }
                }


                


                foreach (var item in _personidlist)
                {
                    var _positioncode = await _RepositoryUnit.PRAuthorizationRepository.GetPositioncode(item, BaseUrlRepo);
                    if (_positioncode == "SYSTEM")
                    {
                        _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists_Batch(item, batchPRNo, BaseUrlRepo);
                        foreach (string prno in _PRAuthorizationModel.qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                        {
                            _prnolisttoscctg.Add(prno);
                        }
                    }
                    if (_prnolisttoscctg.Count > 0)
                    {
                        //_resp = await _RepositoryUnit.PRAuthorizationRepository.EndorseToAccounting(_prnolisttoscctg, _personid, BaseUrlRepo);
                        if (_resp.Status.ToUpper() == "SUCCESS")
                        {
                            _resp.ErrorMessage = SPASv2Messagebox.EndtoAcctg;
                        }


                        return _resp;
                    }

                    _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists_Batch(item, batchPRNo, BaseUrlRepo);
                    _prnolist.Clear();
                    foreach (string prno in _PRAuthorizationModel.qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                    {
                        _prnolist.Add(prno);
                        authpayclass = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizeClass(prno, item, BaseUrlRepo);
                    }

                    strBodyEmail = await this.BodyEMAIL_Authorization_Batchv2(_prnolist, item, authpayclass, _prnolist.Count(), _batchPRNolist);
                    _tblsendemail = new TblSendEmail();


                    _tblsendemail.ReferenceNo = DateTime.Now.ToString("MM/dd/yyyy hhmmss");
                    _tblsendemail.SystemCode = "SPASv2";
                    _tblsendemail.From = "ronom@stpeter.com.ph";


                    var EmailTo = await _RepositoryUnit.PRAuthorizationRepository.GetEmailByPersonID(item, BaseUrlRepo);
                    _tblsendemail.To = EmailTo;



                    _tblsendemail.Subject = "Payment Request Authorization";
                    if (authpayclass == "APPROVER")
                    {
                        _tblsendemail.Subject = "SPASv2 For Approval - " + DateTime.Now.ToString();
                    }
                    else
                    {
                        _tblsendemail.Subject = "SPASv2 For Verification  - " + DateTime.Now.ToString();
                    }

                    _tblsendemail.Body = strBodyEmail;
                    _tblsendemail.Attachment = null;
                    IList<string> CCs = new List<string>();
                    _tblsendemail.CCemails = CCs;
                    _tblsendemail.BCemails = null;
                    _tblsendemail.Host = "smtp-relay.gmail.com";
                    _tblsendemail.Port = "587";
                    _tblsendemail.Username = null;
                    _tblsendemail.Password = null;

                    _resp = await this.SendEmailAuthorization(_tblsendemail, OSPUrlService);


                }

                // IList<TblPaymentRequestAuth> _TblPaymentRequestAuth = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel();
            }
            catch (Exception ex)
            {
                _resp.Status = "FAILED";
                _resp.ErrorMessage = ex.Message;
            }

            return _resp;
        }
        private async Task<string> BodyEMAIL_ChapelAdvisory()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div>Dear CM,  </div>");

            sb.Append("<div class = 'clearfix'></div><br>");

            sb.Append("<div> Below are the casket orders assigned and ordered to your local supplier/s. ");
            sb.Append("<div class = 'clearfix'></div><br>");

            sb.Append("<div> Please indicate your preferred delivery date to ensure casket storage availability and email to aprilnb@stpeter.com.ph. ");
            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div> Reminders: ");
            sb.Append("<div> 1. Sign with your signature over printed name and date on the Sales Invoice and Delivery Receipts.  ");
            sb.Append("<div> 2. Accept only the caskets mentioned in this advisory. ");
            sb.Append("<div> 3. Do not accept damaged, incorrect size, or wrong-colored caskets. ");
            sb.Append("<div class = 'clearfix'></div><br>");

            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div> Please see the attached file for details.");

            sb.Append("<style type='text/css'>");
            sb.Append("table { border-collapse:collapse; }");
            sb.Append("table,th, td {border: 1px solid black;padding:5px;}");
            sb.Append(".clearfix:after { visibility: hidden; display: block; font-size: 0; content: ' '; clear: both; height: 0; }");
            sb.Append(".clearfix { display: inline-block; }");
            sb.Append(".clearfix { display: block; zoom: 1; ");
            sb.Append("</style>");


            sb.Append("<br><div class = 'clearfix'></div>");

            return sb.ToString();
        }
        private async Task<string> BodyEMAIL_ChapelAdvisory_GCM()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div>Dear GCM,  </div>");

            sb.Append("<div class = 'clearfix'></div><br>");

            sb.Append("<div> Your order has been placed with the suppliers, and delivery is anticipated on or before the next cut-off.. ");
            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div> If we do not receive a response to this email within two (2) days, it will be assumed that you approve ");
            sb.Append("<div> the delivery without any holds or cancellations on your part. Should you have any modifications or specific ");
            sb.Append("<div> instructions, kindly inform us promptly. ");
            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div> Reminders: ");
            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div> 1. Upon receipt of the casket/s, kindly click the link (link) to notify us that the casket/s has been successfully received. This immediate notification is essential for the timely release of payment to the supplier.  ");
            sb.Append("<div> 2. Please indicate your preferred delivery date to guarantee casket storage availability. ");
            sb.Append("<div> 3. Attach only the scanned copy of delivery receipts signed with signature over printed name. ");
            sb.Append("<div> 4. Accept only the caskets mentioned in the advisory. ");
            sb.Append("<div> 5. Do not accept damaged, incorrect size/type, and wrong-colored caskets. ");
            sb.Append("<div> 6. Casket details from your respective factories shall be sent separately. ");
            sb.Append("<div class = 'clearfix'></div><br>");

            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div> Please see the attached file for details.");

            sb.Append("<style type='text/css'>");
            sb.Append("table { border-collapse:collapse; }");
            sb.Append("table,th, td {border: 1px solid black;padding:5px;}");
            sb.Append(".clearfix:after { visibility: hidden; display: block; font-size: 0; content: ' '; clear: both; height: 0; }");
            sb.Append(".clearfix { display: inline-block; }");
            sb.Append(".clearfix { display: block; zoom: 1; ");
            sb.Append("</style>");


            sb.Append("<br><div class = 'clearfix'></div>");
            sb.Append("<div> ** This is a system generated email. Please do not reply**");
            return sb.ToString();
        }
        private async Task<string> BodyEMAIL_ChapelAdvisory_Vendor(string VendorName, string PONo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div>To: " + VendorName + ",  </div>");

            sb.Append("<div class = 'clearfix'></div><br>");

            sb.Append("<div>Dear Sir/Ma'am,  </div>");

            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div class = 'clearfix'></div><br>");


            sb.Append("<div>Please see the attached purchase order " + PONo + ".  </div>");
            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div>For queries, please feel free to communicate on your respective Viber group and refer to the existing guidelines.</div>");
            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div>Any additional delivery that is not on the purchase order will not be accommodated.</div>");


            sb.Append("<div class = 'clearfix'></div><br>");


            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div> Please see the attached file for details.");

            sb.Append("<style type='text/css'>");
            sb.Append("table { border-collapse:collapse; }");
            sb.Append("table,th, td {border: 1px solid black;padding:5px;}");
            sb.Append(".clearfix:after { visibility: hidden; display: block; font-size: 0; content: ' '; clear: both; height: 0; }");
            sb.Append(".clearfix { display: inline-block; }");
            sb.Append(".clearfix { display: block; zoom: 1; ");
            sb.Append("</style>");
            sb.Append("<div> Thank you.");
            sb.Append("<br><div class = 'clearfix'></div>");
            sb.Append("<div> LOGISTICS.");


            sb.Append("<br><div class = 'clearfix'></div>");
            sb.Append("<div> ** This is a system generated email. Please do not reply**");
            return sb.ToString();
        }
        public async Task<IActionResult> SendEmailAuthorization_PRNO_Batch(IList<string> _prno, string _personid)
        {

            try
            {
                string batchPRNo = string.Empty;
                //string _personid = _userManager.GetUserId(this.User);
                string authpayclass = string.Empty;
                IList<string> _personidlist = new List<string>();
                IList<string> _prnolist = new List<string>();
                IList<string> _prnolisttoscctg = new List<string>();
                IList<string> _batchPRNolist = new List<string>();

                foreach (var itemprno in _prno)
                {
                    batchPRNo = await _RepositoryUnit.PRAuthorizationRepository.GetBatchNoByPRNo(itemprno, BaseUrlRepo);

                    if (!_batchPRNolist.Contains(batchPRNo))
                    {
                        _batchPRNolist.Add(batchPRNo);
                    }
                }

                TblPaymentRequestAuth _TblPaymentRequestAuth_PRNO;
                PRAuthorizationModel _PRAuthorizationModel = new PRAuthorizationModel();
                _personidlist.Clear();
                foreach (var itemprno in _prno)
                {

                    _TblPaymentRequestAuth_PRNO = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel(itemprno, BaseUrlRepo);
                    if (_TblPaymentRequestAuth_PRNO == null)
                    {
                        return Ok();
                    }

                    if (!_personidlist.Contains(_TblPaymentRequestAuth_PRNO.PersonID))
                    {
                        _personidlist.Add(_TblPaymentRequestAuth_PRNO.PersonID);
                    }



                }
                foreach (var item in _personidlist)
                {
                    var _positioncode = await _RepositoryUnit.PRAuthorizationRepository.GetPositioncode(item, BaseUrlRepo);
                    if (_positioncode == "SYSTEM")
                    {
                        _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists(item, BaseUrlRepo);
                        foreach (string prno in _PRAuthorizationModel.qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                        {
                            _prnolisttoscctg.Add(prno);
                        }
                    }
                    if (_prnolisttoscctg.Count > 0)
                    {
                        _resp = await _RepositoryUnit.PRAuthorizationRepository.EndorseToAccounting(_prnolisttoscctg, _personid, BaseUrlRepo);
                    }

                    _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists_Batch(item, batchPRNo, BaseUrlRepo);
                    _prnolist.Clear();
                    foreach (string prno in _PRAuthorizationModel.qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                    {
                        _prnolist.Add(prno);
                        authpayclass = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizeClass(prno, item, BaseUrlRepo);
                    }

                    strBodyEmail = await this.BodyEMAIL_Authorization_Batchv2(_prnolist, item, authpayclass, _prnolist.Count(), _batchPRNolist);

                    TblSendEmail _tblsendemail = new TblSendEmail();

                    _tblsendemail.ReferenceNo = DateTime.Now.ToString("MM/dd/yyyy hhmmss");
                    _tblsendemail.SystemCode = "SPASv2";

                    _tblsendemail.From = "ronom@stpeter.com.ph";

                    _tblsendemail.Subject = "Payment Request Authorization";
                    if (authpayclass == "APPROVER")
                    {
                        _tblsendemail.Subject = "SPASv2 For Approval - " + DateTime.Now.ToString();
                    }
                    else
                    {
                        _tblsendemail.Subject = "SPASv2 For Verification  - " + DateTime.Now.ToString();
                    }


                    _tblsendemail.Body = strBodyEmail;
                    _tblsendemail.Attachment = null;

                    IList<string> CCs = new List<string>();
                    //CCs.Add("ronom@stpeter.com.ph");
                    //CCs.Add("davidga@stpeter.com.ph");
                    CCs.Add("warrenlb@stpeter.com.ph");
                    CCs.Add("rudyab@stpeter.com.ph");
                    CCs.Add("jonab@stpeter.com.ph");
                    _tblsendemail.CCemails = CCs;
                    _tblsendemail.BCemails = null;
                    _tblsendemail.Host = "smtp-relay.gmail.com";
                    _tblsendemail.Port = "587";
                    _tblsendemail.Username = null;
                    _tblsendemail.Password = null;


                    var EmailTo = await _RepositoryUnit.PRAuthorizationRepository.GetEmailByPersonID(item, BaseUrlRepo);
                    _tblsendemail.To = EmailTo;


                    if (String.IsNullOrEmpty(EmailTo))
                    {
                        IList<qryGroupEmails> _Emails = await _RepositoryUnit.PRAuthorizationRepository.GetEmailsByGroupId(item, BaseUrlRepo);

                        foreach (var itemEmails in _Emails)
                        {

                            strBodyEmail = await this.BodyEMAIL_Authorization_Batchv2(_prnolist, item, authpayclass, _prnolist.Count(), _batchPRNolist);
                            _tblsendemail.To = itemEmails.Emails;
                            if (authpayclass == "APPROVER")
                            {
                                _tblsendemail.Subject = "SPASv2 For Approval - " + DateTime.Now.ToString();
                            }
                            else
                            {
                                _tblsendemail.Subject = "SPASv2 For Verification  - " + DateTime.Now.ToString();
                            }
                            _resp = await this.SendEmailAuthorization(_tblsendemail, OSPUrlService);
                        }
                        return Ok(_resp);

                    }
                    else
                    {
                        _resp = await this.SendEmailAuthorization(_tblsendemail, OSPUrlService);
                    }




                }
                // IList<TblPaymentRequestAuth> _TblPaymentRequestAuth = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel();
                //_resp.Status = "SUCCESS";
                //_resp.ErrorMessage = "Process Completed.";
                return Ok(_resp);
            }
            catch (Exception ex)
            {
                _resp.Status = "FAILED";
                _resp.ErrorMessage = ex.Message;
                return BadRequest(_resp);
            }
        }
        public IActionResult Index()
        {
            return View();
        }
        private string strBodyEmail;
        private async Task<string> BodyEMAIL_Authorizationv2(IList<string> listprno, string personid, string AuthorizeClass, int cntPR)
        {

            string Name = await _RepositoryUnit.PRAuthorizationRepository.GetNameofAuthorizer(personid, BaseUrlRepo);
            string Gender = await _RepositoryUnit.PRAuthorizationRepository.GetGenderByPersonID(personid, BaseUrlRepo);
            string anotation;

            if (Gender == "MALE")
            {
                anotation = "Mr.";
            }
            else
            {
                anotation = "Ms.";
            }

            if (personid == "LOGLOCVER")
            {
                anotation = "";
                Name = "Team Logistics Verifiers";
            }

            DateTime RequestDate = Convert.ToDateTime("1900/01/01");
            StringBuilder sb = new StringBuilder();


            string strbuttonclass = string.Empty;
            string classword = string.Empty;
            string strTitle = string.Empty;
            string strbuttonurl = string.Empty;

            CultureInfo culture_info = Thread.CurrentThread.CurrentCulture;
            TextInfo text_info = culture_info.TextInfo;
            Name = text_info.ToTitleCase(Name.ToLower());


            if (AuthorizeClass == "VERIFIER")
            {
                classword = "verify";

            }
            else
            {
                classword = "approve";

            }







            sb.Append(@"<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"">

<head>
    <meta charset=""utf-8""><!-- utf-8 works for most cases -->
    <meta name=""viewport"" content=""width=device-width""><!-- Forcing initial-scale shouldn't be necessary -->
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge""><!-- Use the latest (edge) version of IE rendering engine -->
    <meta name=""x-apple-disable-message-reformatting""><!-- Disable auto-scale in iOS 10 Mail entirely -->
    <title></title><!-- The title tag shows in email notifications, like Android 4.4. -->
    <link href=""https://fonts.googleapis.com/css?family=Work+Sans:200,300,400,500,600,700"" rel=""stylesheet""><!-- CSS Reset : BEGIN -->
    <style>
        /* What it does: Remove spaces around the email design added by some email clients. */
        /* Beware: It can remove the padding / margin and add a background color to the compose a reply window. */
        html,
        body {
            margin: 0 auto !important;
            padding: 0 !important;
            height: 100% !important;
            width: 100% !important;
            background: #cccccc;
        }

        /* What it does: Stops email clients resizing small text. */
        * {
            -ms-text-size-adjust: 100%;
            -webkit-text-size-adjust: 100%;
        }

        /* What it does: Centers email on Android 4.4 */
        div[style*=""margin: 16px 0""] {
            margin: 0 !important;
        }

        /* What it does: Stops Outlook from adding extra spacing to tables. */
        table,
        td {
            mso-table-lspace: 0pt !important;
            mso-table-rspace: 0pt !important;
        }

        /* What it does: Fixes webkit padding issue. */
        table {
            border-spacing: 0 !important;
            border-collapse: collapse !important;
            table-layout: fixed !important;
            margin: 0 auto !important;
        }

        /* What it does: Uses a better rendering method when resizing images in IE. */
        img {
            -ms-interpolation-mode: bicubic;
        }

        /* What it does: Prevents Windows 10 Mail from underlining links despite inline CSS. Styles for underlined links should be inline. */
        a {
            text-decoration: none;
        }

        /* What it does: A work-around for email clients meddling in triggered links. */
        *[x-apple-data-detectors],
        /* iOS */
        .unstyle-auto-detected-links *,
        .aBn {
            border-bottom: 0 !important;
            cursor: default !important;
            color: inherit !important;
            text-decoration: none !important;
            font-size: inherit !important;
            font-family: inherit !important;
            font-weight: inherit !important;
            line-height: inherit !important;
        }

        /* What it does: Prevents Gmail from displaying a download button on large, non-linked images. */
        .a6S {
            display: none !important;
            opacity: 0.01 !important;
        }

        /* What it does: Prevents Gmail from changing the text color in conversation threads. */
        .im {
            color: inherit !important;
        }

        /* If the above doesn't work, add a .g-img class to any image in question. */
        img.g-img+div {
            display: none !important;
        }

        /* What it does: Removes right gutter in Gmail iOS app: https://github.com/TedGoas/Cerberus/issues/89  */
        /* Create one of these media queries for each additional viewport size you'd like to fix */
        /* iPhone 4, 4S, 5, 5S, 5C, and 5SE */
        @media only screen and (min-device-width: 320px) and (max-device-width: 374px) {
            u~div .email-container {
                min-width: 320px !important;
            }
        }

        /* iPhone 6, 6S, 7, 8, and X */
        @media only screen and (min-device-width: 375px) and (max-device-width: 413px) {
            u~div .email-container {
                min-width: 375px !important;
            }
        }

        /* iPhone 6+, 7+, and 8+ */
        @media only screen and (min-device-width: 414px) {
            u~div .email-container {
                min-width: 414px !important;
            }
        }
    </style><!-- CSS Reset : END -->
    <!-- Progressive Enhancements : BEGIN -->
    <style>
        .primary {
            background: #17bebb;
        }

        .bg_white {
            background: #ffffff;
        }

        .bg_light {
            background: #f7fafa;
        }

        .bg_black {
            background: #000000;
        }

        .bg_dark {
            background: rgba(0, 0, 0, .8);
        }

        .email-section {
            padding: 2.5em;
        }

        /*BUTTON*/
        .btn {
            padding: 10px 15px;
            display: inline-block;
        }

        .btn.btn-primary {
            border-radius: 5px;
            background: #6c63ff;
            color: #ffffff;
        }

        .btn.btn-white {
            border-radius: 5px;
            background: #ffffff;
            color: #000000;
        }

        .btn.btn-white-outline {
            border-radius: 5px;
            background: transparent;
            border: 100px solid #fff;
            color: #fff;
        }

        .btn.btn-black-outline {
            border-radius: 0px;
            background: transparent;
            border: 2px solid #000;
            color: #000;
            font-weight: 700;
        }

        .btn-custom {
            color: rgba(0, 0, 0, .3);
            text-decoration: underline;
        }

        h1,
        h2,
        h3,
        h4,
        h5,
        h6 {
            font-family: 'Work Sans', sans-serif;
            color: #000000;
            margin-top: 0;
            font-weight: 400;
        }

        body {
            font-family: 'Work Sans', sans-serif;
            font-weight: 400;
            font-size: 15px;
            line-height: 1.8;
            color: rgba(0, 0, 0, .4);
        }

        a {
            color: #17bebb;
        }

        table {}

        /*LOGO*/
        .logo h1 {
            margin: 0;
        }

        .logo h1 a {
            color: #17bebb;
            font-size: 24px;
            font-weight: 700;
            font-family: 'Work Sans', sans-serif;
        }

        /*HERO*/
        .hero {
            position: relative;
            z-index: 0;
        }

        .hero .text {
            color: rgba(0, 0, 0, .3);
        }

        .hero .text h2 {
            color: #000;
            font-size: 34px;
            margin-bottom: 15px;
            font-weight: 300;
            line-height: 1.2;
        }

        .hero .text h3 {
            font-size: 24px;
            font-weight: 200;
        }

        .hero .text h2 span {
            font-weight: 600;
            color: #000;
        }

        /*PRODUCT*/
        .product-entry {
            display: block;
            position: relative;
            float: left;
            padding-top: 20px;
        }

        .product-entry .text {
            width: calc(100% - 125px);
            padding-left: 20px;
        }

        .product-entry .text h3 {
            margin-bottom: 0;
            padding-bottom: 0;
        }

        .product-entry .text p {
            margin-top: 0;
        }

        .product-entry img,
        .product-entry .text {
            float: left;
        }

        ul.social {
            padding: 0;
        }

        ul.social li {
            display: inline-block;
            margin-right: 10px;
        }

        /*FOOTER*/
        .footer {
            border-top: 1px solid rgba(0, 0, 0, .05);
            color: rgba(0, 0, 0, .5);
        }

        .footer .heading {
            color: #000;
            font-size: 20px;
        }

        .footer ul {
            margin: 0;
            padding: 0;
        }

        .footer ul li {
            list-style: none;
            margin-bottom: 10px;
        }

        .footer ul li a {
            color: rgba(0, 0, 0, 1);
        }

        @media screen and (max-width: 500px) {}
    </style>
</head>

<body width=""100%"" style=""margin: 0; padding: 0 !important; mso-line-height-rule: exactly; background-color: #f1f1f1;"">
    <center style=""width: 100%; background-color: #f1f1f1;"">
        <div style=""display: none; font-size: 1px;max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden; mso-hide: all; font-family: sans-serif;"" class=""esd-text""> ‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp; </div>
        <div style=""max-width: 600px; margin: 0 auto;"" class=""email-container"">
            <!-- BEGIN BODY -->
            <table align=""center"" role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""margin: auto;border-left:1px solid #cccccc;border-right:1px solid #cccccc;border-top:1px solid #cccccc;background-color: #ffffff;"">
                <tbody>
                    <tr>
                        <td valign=""top"" class=""bg_white"" style=""padding: 1em 2.5em 0 2.5em;""></td>
                    </tr><!-- end tr -->
                    <tr>
                        <td valign=""middle"" class=""hero bg_white"" style=""padding: 2em 0 2em 0;"">
                            <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tbody>
                                    <tr>
                                        <td style=""padding: 0 2.5em; text-align: left;"">
                                            <div class=""text esd-text"">
                                                <h4>Dear " + anotation + " " + Name + @",</h4>
                                                <h4>You have been requested to " + classword + @" <span style=""font-size: 24px; color: #0000ff;"">" + cntPR.ToString() + @"</span> requisition of the following Payment Request No(s):</h4>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr><!-- end tr -->
                    <tr></tr>
                </tbody>
            </table>
            <table class=""bg_white"" role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""margin: auto;border-left:1px solid #cccccc;border-bottom:1px solid #cccccc;border-right:1px solid #cccccc;background-color: #ffffff;"">
                <tbody>
                    <tr style=""border-bottom: 1px solid rgba(0,0,0,.05);"">
                        <th width=""80%"" style=""text-align:left; padding: 0 2.5em; color: #000; padding-bottom: 20px"" class=""esd-text"">Request No.</th>
                        <th width=""20%"" style=""text-align:right; padding: 0 2.5em; color: #000; padding-bottom: 20px"" class=""esd-text"">Request Date</th>
                    </tr> ");


            foreach (var item in listprno)
            {

                sb.Append(@"<tr style=""border-bottom: 1px solid rgba(0,0,0,.05);"">
                        <td valign=""middle"" width=""80%"" style=""text-align:left; padding: 0 2.5em;"">
                            <div class=""product-entry"">
                                <div class=""esd-text"">
                                    <h3 style=""text-align: center;"">" + item + @"</h3>
                                </div>
                            </div>
                        </td>");

                RequestDate = await _RepositoryUnit.PRAuthorizationRepository.GetRequestDate(item, BaseUrlRepo);


                sb.Append(@"<td valign=""middle"" width=""80%"" style=""text-align:left; padding: 0 2.5em;"" class=""esd-text"">
                            <h3 style=""text-align: center;"">" + RequestDate + @"<span style=""font-size:13px;""></span></h3>
                        </td>
                    </tr>");

            }


            sb.Append(@"<tr>
                        <td valign=""middle"" style=""text-align:left; padding: 1em 2.5em;"" class=""esd-text"" align=""left"" esd-links-underline=""none"">
                            <p><a href=""https://localhost:7137/Authorization/PRAuthorizationLists"" class=""btn btn-primary"" style=""text-decoration: none;"" target=""_blank"">Click here to " + classword + @"</a></p>
                        </td>
                    </tr>
                </tbody>
            </table><!-- end tr -->
            <!-- 1 Column Text + Button : END -->
            <table align=""center"" role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""margin: auto;""></table>
        </div>
    </center>
</body>

</html>");




            return sb.ToString();
        }
        private async Task<string> BodyEMAIL_Authorization_Batchv2(IList<string> listprno, string personid, string AuthorizeClass, int cntPR, IList<string> BatchPRNo)
        {

            string Name = await _RepositoryUnit.PRAuthorizationRepository.GetNameofAuthorizer(personid, BaseUrlRepo);
            string Gender = await _RepositoryUnit.PRAuthorizationRepository.GetGenderByPersonID(personid, BaseUrlRepo);
            string anotation;

            if (Gender == "MALE")
            {
                anotation = "Mr.";
            }
            else
            {
                anotation = "Ms.";
            }

            if (personid == "LOGLOCVER")
            {
                Name = "Logistics Verifier";
                anotation = "";
            }






            DateTime RequestDate = Convert.ToDateTime("1900/01/01");
            StringBuilder sb = new StringBuilder();


            string strbuttonclass = string.Empty;
            string classword = string.Empty;
            string strTitle = string.Empty;
            string strbuttonurl = string.Empty;

            CultureInfo culture_info = Thread.CurrentThread.CurrentCulture;
            TextInfo text_info = culture_info.TextInfo;
            Name = text_info.ToTitleCase(Name.ToLower());


            if (AuthorizeClass == "VERIFIER")
            {
                classword = "verify";

            }
            else
            {
                classword = "approve";

            }







            sb.Append(@"<html lang=""en"" xmlns=""http://www.w3.org/1999/xhtml"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"">

<head>
    <meta charset=""utf-8""><!-- utf-8 works for most cases -->
    <meta name=""viewport"" content=""width=device-width""><!-- Forcing initial-scale shouldn't be necessary -->
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge""><!-- Use the latest (edge) version of IE rendering engine -->
    <meta name=""x-apple-disable-message-reformatting""><!-- Disable auto-scale in iOS 10 Mail entirely -->
    <title></title><!-- The title tag shows in email notifications, like Android 4.4. -->
    <link href=""https://fonts.googleapis.com/css?family=Work+Sans:200,300,400,500,600,700"" rel=""stylesheet""><!-- CSS Reset : BEGIN -->
    <style>
        /* What it does: Remove spaces around the email design added by some email clients. */
        /* Beware: It can remove the padding / margin and add a background color to the compose a reply window. */
        html,
        body {
            margin: 0 auto !important;
            padding: 0 !important;
            height: 100% !important;
            width: 100% !important;
            background: #cccccc;
        }

        /* What it does: Stops email clients resizing small text. */
        * {
            -ms-text-size-adjust: 100%;
            -webkit-text-size-adjust: 100%;
        }

        /* What it does: Centers email on Android 4.4 */
        div[style*=""margin: 16px 0""] {
            margin: 0 !important;
        }

        /* What it does: Stops Outlook from adding extra spacing to tables. */
        table,
        td {
            mso-table-lspace: 0pt !important;
            mso-table-rspace: 0pt !important;
        }

        /* What it does: Fixes webkit padding issue. */
        table {
            border-spacing: 0 !important;
            border-collapse: collapse !important;
            table-layout: fixed !important;
            margin: 0 auto !important;
        }

        /* What it does: Uses a better rendering method when resizing images in IE. */
        img {
            -ms-interpolation-mode: bicubic;
        }

        /* What it does: Prevents Windows 10 Mail from underlining links despite inline CSS. Styles for underlined links should be inline. */
        a {
            text-decoration: none;
        }

        /* What it does: A work-around for email clients meddling in triggered links. */
        *[x-apple-data-detectors],
        /* iOS */
        .unstyle-auto-detected-links *,
        .aBn {
            border-bottom: 0 !important;
            cursor: default !important;
            color: inherit !important;
            text-decoration: none !important;
            font-size: inherit !important;
            font-family: inherit !important;
            font-weight: inherit !important;
            line-height: inherit !important;
        }

        /* What it does: Prevents Gmail from displaying a download button on large, non-linked images. */
        .a6S {
            display: none !important;
            opacity: 0.01 !important;
        }

        /* What it does: Prevents Gmail from changing the text color in conversation threads. */
        .im {
            color: inherit !important;
        }

        /* If the above doesn't work, add a .g-img class to any image in question. */
        img.g-img+div {
            display: none !important;
        }

        /* What it does: Removes right gutter in Gmail iOS app: https://github.com/TedGoas/Cerberus/issues/89  */
        /* Create one of these media queries for each additional viewport size you'd like to fix */
        /* iPhone 4, 4S, 5, 5S, 5C, and 5SE */
        @media only screen and (min-device-width: 320px) and (max-device-width: 374px) {
            u~div .email-container {
                min-width: 320px !important;
            }
        }

        /* iPhone 6, 6S, 7, 8, and X */
        @media only screen and (min-device-width: 375px) and (max-device-width: 413px) {
            u~div .email-container {
                min-width: 375px !important;
            }
        }

        /* iPhone 6+, 7+, and 8+ */
        @media only screen and (min-device-width: 414px) {
            u~div .email-container {
                min-width: 414px !important;
            }
        }
    </style><!-- CSS Reset : END -->
    <!-- Progressive Enhancements : BEGIN -->
    <style>
        .primary {
            background: #17bebb;
        }

        .bg_white {
            background: #ffffff;
        }

        .bg_light {
            background: #f7fafa;
        }

        .bg_black {
            background: #000000;
        }

        .bg_dark {
            background: rgba(0, 0, 0, .8);
        }

        .email-section {
            padding: 2.5em;
        }

        /*BUTTON*/
        .btn {
            padding: 10px 15px;
            display: inline-block;
        }

        .btn.btn-primary {
            border-radius: 5px;
            background: #6c63ff;
            color: #ffffff;
        }

        .btn.btn-white {
            border-radius: 5px;
            background: #ffffff;
            color: #000000;
        }

        .btn.btn-white-outline {
            border-radius: 5px;
            background: transparent;
            border: 100px solid #fff;
            color: #fff;
        }

        .btn.btn-black-outline {
            border-radius: 0px;
            background: transparent;
            border: 2px solid #000;
            color: #000;
            font-weight: 700;
        }

        .btn-custom {
            color: rgba(0, 0, 0, .3);
            text-decoration: underline;
        }

        h1,
        h2,
        h3,
        h4,
        h5,
        h6 {
            font-family: 'Work Sans', sans-serif;
            color: #000000;
            margin-top: 0;
            font-weight: 400;
        }

        body {
            font-family: 'Work Sans', sans-serif;
            font-weight: 400;
            font-size: 15px;
            line-height: 1.8;
            color: rgba(0, 0, 0, .4);
        }

        a {
            color: #17bebb;
        }

        table {}

        /*LOGO*/
        .logo h1 {
            margin: 0;
        }

        .logo h1 a {
            color: #17bebb;
            font-size: 24px;
            font-weight: 700;
            font-family: 'Work Sans', sans-serif;
        }

        /*HERO*/
        .hero {
            position: relative;
            z-index: 0;
        }

        .hero .text {
            color: rgba(0, 0, 0, .3);
        }

        .hero .text h2 {
            color: #000;
            font-size: 34px;
            margin-bottom: 15px;
            font-weight: 300;
            line-height: 1.2;
        }

        .hero .text h3 {
            font-size: 24px;
            font-weight: 200;
        }

        .hero .text h2 span {
            font-weight: 600;
            color: #000;
        }

        /*PRODUCT*/
        .product-entry {
            display: block;
            position: relative;
            float: left;
            padding-top: 20px;
        }

        .product-entry .text {
            width: calc(100% - 125px);
            padding-left: 20px;
        }

        .product-entry .text h3 {
            margin-bottom: 0;
            padding-bottom: 0;
        }

        .product-entry .text p {
            margin-top: 0;
        }

        .product-entry img,
        .product-entry .text {
            float: left;
        }

        ul.social {
            padding: 0;
        }

        ul.social li {
            display: inline-block;
            margin-right: 10px;
        }

        /*FOOTER*/
        .footer {
            border-top: 1px solid rgba(0, 0, 0, .05);
            color: rgba(0, 0, 0, .5);
        }

        .footer .heading {
            color: #000;
            font-size: 20px;
        }

        .footer ul {
            margin: 0;
            padding: 0;
        }

        .footer ul li {
            list-style: none;
            margin-bottom: 10px;
        }

        .footer ul li a {
            color: rgba(0, 0, 0, 1);
        }

        @media screen and (max-width: 500px) {}
    </style>
</head>

<body width=""100%"" style=""margin: 0; padding: 0 !important; mso-line-height-rule: exactly; background-color: #f1f1f1;"">
    <center style=""width: 100%; background-color: #f1f1f1;"">
        <div style=""display: none; font-size: 1px;max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden; mso-hide: all; font-family: sans-serif;"" class=""esd-text""> ‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp;‌&nbsp; </div>
        <div style=""max-width: 600px; margin: 0 auto;"" class=""email-container"">
            <!-- BEGIN BODY -->
            <table align=""center"" role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""margin: auto;border-left:1px solid #cccccc;border-right:1px solid #cccccc;border-top:1px solid #cccccc;background-color: #ffffff;"">
                <tbody>
                    <tr>
                        <td valign=""top"" class=""bg_white"" style=""padding: 1em 2.5em 0 2.5em;""></td>
                    </tr><!-- end tr -->
                    <tr>
                        <td valign=""middle"" class=""hero bg_white"" style=""padding: 2em 0 2em 0;"">
                            <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                <tbody>
                                    <tr>
                                        <td style=""padding: 0 2.5em; text-align: left;"">
                                            <div class=""text esd-text"">
                                                <h4>Dear " + anotation + " " + Name + @",</h4>
                                                <h4>You have been requested to " + classword + @" <span style=""font-size: 24px; color: #0000ff;"">" + cntPR.ToString() + @"</span> requisition of the following Batch No(s):</h4>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr><!-- end tr -->
                    <tr></tr>
                </tbody>
            </table>
            <table class=""bg_white"" role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""margin: auto;border-left:1px solid #cccccc;border-bottom:1px solid #cccccc;border-right:1px solid #cccccc;background-color: #ffffff;"">
                <tbody>
                    <tr style=""border-bottom: 1px solid rgba(0,0,0,.05);"">
                        <th width=""80%"" style=""text-align:left; padding: 0 2.5em; color: #000; padding-bottom: 20px"" class=""esd-text"">Batch No.</th>
                        <th width=""20%"" style=""text-align:right; padding: 0 2.5em; color: #000; padding-bottom: 20px"" class=""esd-text"">Request Date</th>
                    </tr> ");


            foreach (var item in BatchPRNo)
            {

                sb.Append(@"<tr style=""border-bottom: 1px solid rgba(0,0,0,.05);"">
                        <td valign=""middle"" width=""80%"" style=""text-align:left; padding: 0 2.5em;"">
                            <div class=""product-entry"">
                                <div class=""esd-text"">
                                    <h3 style=""text-align: center;"">" + item + @"</h3>
                                </div>
                            </div>
                        </td>");

                RequestDate = await _RepositoryUnit.PRAuthorizationRepository.GetRequestDateByBatchNo(item, BaseUrlRepo);


                sb.Append(@"<td valign=""middle"" width=""80%"" style=""text-align:left; padding: 0 2.5em;"" class=""esd-text"">
                            <h3 style=""text-align: center;"">" + RequestDate + @"<span style=""font-size:13px;""></span></h3>
                        </td>
                    </tr>");

            }


            sb.Append(@"<tr>
                        <td valign=""middle"" style=""text-align:left; padding: 1em 2.5em;"" class=""esd-text"" align=""left"" esd-links-underline=""none"">
                            <p><a href=""" + BaseUrlRepo + @"/Authorization/BatchPRAuthorization"" class=""btn btn-primary"" style=""text-decoration: none;"" target=""_blank"">Click here to " + classword + @"</a></p>
                        </td>
                    </tr>
                </tbody>
            </table><!-- end tr -->
            <!-- 1 Column Text + Button : END -->
            <table align=""center"" role=""presentation"" cellspacing=""0"" cellpadding=""0"" border=""0"" width=""100%"" style=""margin: auto;""></table>
        </div>
    </center>
</body>

</html>");




            return sb.ToString();
        }

    }
}
