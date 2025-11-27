using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSP.SPASv2.Domain.Tables;
using OSP.Common.Domain.Tables;
using OSP.SPASv2.Domain.View;
using OSP.SPASv2.Repository.Repository.MainRepository;
using OSP.SPASv2.Repository.Utility;
using SPASv2.Context;
using OSP.Common.Domain.View;
using OSP.SPASv2.Domain.Params;
using SPASv2.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Security.Policy;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Microsoft.Reporting.NETCore;
using System.Data;
using System.Linq;
using System.Xml;
using OSP.SPASv2.Domain.References;
using System.Text;
using System;
using OSP.Common.Domain.References;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Diagnostics;
using DocumentFormat.OpenXml.Bibliography;
using System.Globalization;
using OSP.Common.Domain.Params;
using NuGet.Packaging;
using OSP.SPASv2.Repository.Repository.ServiceUnit;

namespace OSP.SPASv2.Repository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RonController : ControllerBase
    {
        private readonly JWTAuthenticationManager jwtAuthenticationManager;
        private IConfiguration configuration;
        RepositoryUnit _RepositoryUnit;
        private ILogger<RonController> logger;
        private SPASv2Context context;
        TblResponse _response = new TblResponse();
        string _validmessage;
        string BaseUrlRepo;
        string BaseUrlService;
        string OSPUrlRepo;
        string OSPUrlService;
        string BaseUI;
        private IConfiguration _configuration;
        string _ReportFilespath;
        string DevelopmentType;
        string BKPTemplatePath;
        string CMSDeliveryTemplate;

        private readonly IWebHostEnvironment env;
        JonController _jonController;

        ServiceUnit _ServiceUnit;


        //RudyController _rudyController;

        //Hello
        public RonController(ILogger<RonController> _logger, SPASv2Context _context, JWTAuthenticationManager _jwt, IConfiguration configuration,
            JonController jonController, IWebHostEnvironment _env)
        {
            _configuration = configuration;
            logger = _logger;
            this.context = _context;
            _ServiceUnit = new ServiceUnit(_context);
            _RepositoryUnit = new RepositoryUnit(_context);
            this.jwtAuthenticationManager = _jwt;
            BaseUrlRepo = _configuration.GetSection("APIBaseURL")["SPASv2.Repository"];
            BaseUrlService = _configuration.GetSection("APIBaseURL")["SPASv2.Service"];
            OSPUrlRepo = _configuration.GetSection("APIBaseURLCommon")["Common.Repository"];
            OSPUrlService = _configuration.GetSection("APIBaseURLCommon")["Common.Service"];
            _ReportFilespath = _configuration.GetSection("UploadingPath")["ReqFiles"];
            DevelopmentType = _configuration.GetSection("Development")["Type"];
            BaseUI = _configuration.GetSection("UIBaseURL")["SPASv2.UI"];
            BKPTemplatePath = _configuration.GetSection("UploadingPath")["BKPPathTemplate"];
            CMSDeliveryTemplate = _configuration.GetSection("UploadingPath")["CMSDeliveryPathTemplate"];
            env = _env;

            _jonController = jonController;
            //_rudyController = rudyController;

            //_rudyController = rudycontroller;`
        }

        [HttpGet("GetVendorItems")]
        public async Task<IActionResult> GetVendorItems(string vendorcode, string paymentclasscode)
        {
            try
            {
                logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");
                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.VendorItemsRepository.GetVendorItems(vendorcode, paymentclasscode);

                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }

        }

        [HttpGet("GetVendorItemsList")]
        public async Task<IActionResult> GetVendorItemsList(string vendorcode)
        {
            try
            {
                logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");

                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.VendorItemsRepository.GetVendorItemsList(vendorcode);

                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }

        }

        [HttpGet("GetPRAuthorizationList")]
        public async Task<IActionResult> GetPRAuthorizationList(string empcode)
        {
            try
            {
                logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");

                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationList(empcode);

                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }

        }

        [HttpGet("GetPRAuthorizationLists")]
        public async Task<ActionResult<IList<qryPRAuthorizationList>>> GetPRAuthorizationLists(string personid)
        {
            try
            {

                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists(personid);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return new List<qryPRAuthorizationList>(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;




            }
        }

        [HttpGet("GetPRAuthorizationLists_Batch")]
        public async Task<IActionResult> GetPRAuthorizationLists_Batch(string personid, string BatchPRNo)
        {
            try
            {

                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                IList<qryPRAuthorizationList> vlist = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists_Batch(personid, BatchPRNo);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return Ok(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;




            }
        }

        [HttpGet("GetPRAuthorizationLists_BatchByPayclassCode")]
        public async Task<ActionResult<IList<qryPRAuthorizationList>>> GetPRAuthorizationLists_BatchByPayclassCode(string personid, string payclasscode)
        {
            try
            {

                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists_BatchByPayClassCode(personid, payclasscode);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return new List<qryPRAuthorizationList>(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;




            }
        }

        [HttpGet("GetAuthorizerPayClassLists")]
        public async Task<ActionResult<IList<qryListOfAuthorizerPayclass>>> GetAuthorizerPayClassLists(string companytype, string personid, string payclass)
        {
            try
            {

                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizerPayclassLists(companytype, payclass);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return new List<qryListOfAuthorizerPayclass>(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetAuthorizeClass")]
        public async Task<IActionResult> GetAuthorizeClass(string prno, string personid)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                string vlist = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizeClass(prno, personid);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return Ok(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return BadRequest(error);

            }
        }

        [HttpGet("GetAuthorizeClassByPersonId")]
        public async Task<ActionResult<string>> GetAuthorizeClassByPersonId(string personid)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                string vlist = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizeClass(personid);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetLatestAuthoriztionByAuthorizeLevel")]
        public async Task<ActionResult<TblPaymentRequestAuth>> GetLatestAuthoriztionByAuthorizeLevel(string prno)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                TblPaymentRequestAuth vlist = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel(prno);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetLatestAuthoriztionByAuthorizeLevel_ALL")]
        public async Task<IList<TblPaymentRequestAuth>> GetLatestAuthoriztionByAuthorizeLevel_ALL()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                IList<TblPaymentRequestAuth> vlist = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetALLTblPaymentRequestAuthByAuthorizeLevel")]
        public async Task<IList<TblPaymentRequestAuth>> GetALLTblPaymentRequestAuthByAuthorizeLevel(string prno)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                IList<TblPaymentRequestAuth> vlist = await _RepositoryUnit.PRAuthorizationRepository.GetALLTblPaymentRequestAuthByAuthorizeLevel(prno);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }
        [HttpGet("GetALLTblPaymentRequestAuthByPersonId")]
        public async Task<TblPaymentRequestAuth> GetALLTblPaymentRequestAuthByPersonId(string prno, string personid)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                TblPaymentRequestAuth vlist = await _RepositoryUnit.PRAuthorizationRepository.GetALLTblPaymentRequestAuthByPersonId(prno, personid);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpPost("CreatePRAuthorization")]
        public async Task<TblResponse> CreatePRAuthorization(string prno, string reqtype)
        {
            try
            {
                logger.LogInformation("Create PRAuthorization - " + Utilities.GetmethodName() + "");
                await _RepositoryUnit.PRAuthorizationRepository.CreatePRAuthorization(prno, reqtype);
                await _RepositoryUnit.TblRequisitionHdrRepository.UpdateRequestQtySummary(prno);
                logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(prno, "SUCCESS", "Successfully Save.", Utilities.GetmethodName());
                return await Task.FromResult(_response);

            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(prno, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }

        }

        [HttpPost("UpdatePRAuthorization")]
        public async Task<TblResponse> UpdatePRAuthorization(string prno, string personid, string statustype)
        {
            try
            {
                logger.LogInformation("Update PRAuthorization - " + Utilities.GetmethodName() + "");
                await _RepositoryUnit.PRAuthorizationRepository.UpdatePRAuthorization(prno, personid, statustype);
                logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(prno, "SUCCESS", "Successfully Updated.", Utilities.GetmethodName());
                return await Task.FromResult(_response);

            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(prno, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }

        }

        [HttpPost("ApprovePRAuthorization")]
        public async Task<TblResponse> ApprovePRAuthorization(qryUpdateStatusAuth _qryUpdateStatusAuth)
        {
            try
            {
                logger.LogInformation("Approve PRAuthorization - " + Utilities.GetmethodName() + "");

                //if (_qryUpdateStatusAuth.PersonID.Equals("PISPLPI06141") && _qryUpdateStatusAuth.ReqType.Equals("PY"))
                //{

                //}
                //else
                //{
                _response = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qryUpdateStatusAuth);
                if (!String.IsNullOrEmpty(_qryUpdateStatusAuth.TransType))
                {
                    _response = await _RepositoryUnit.TblRequisitionHdrRepository.UpdateTransType(_qryUpdateStatusAuth.TransType, _qryUpdateStatusAuth.PRRefNo);
                }
                //}

                logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_qryUpdateStatusAuth.PRRefNo, "SUCCESS", "Process Completed", Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(_qryUpdateStatusAuth.PRRefNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }

        }
        [HttpPost("ApprovePRAuthorizationByBANo")]
        public async Task<TblResponse> ApprovePRAuthorizationByBANo(qryUpdateStatusAuth _qryUpdateStatusAuth)
        {
            try
            {

                logger.LogInformation("Approve PRAuthorization - " + Utilities.GetmethodName() + "");


                List<string> _ReqnoList = new List<string>();

                _ReqnoList = await _RepositoryUnit.BatchApprovalRepository.GetReqnoListByBano(_qryUpdateStatusAuth.PRRefNo);


                foreach (var item in _ReqnoList)
                {
                    _qryUpdateStatusAuth.PRRefNo = item;
                    _response = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qryUpdateStatusAuth);
                }

                if (!String.IsNullOrEmpty(_qryUpdateStatusAuth.TransType))
                {
                    _response = await _RepositoryUnit.TblRequisitionHdrRepository.UpdateTransType(_qryUpdateStatusAuth.TransType, _qryUpdateStatusAuth.PRRefNo);
                }
                //}

                logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_qryUpdateStatusAuth.PRRefNo, "SUCCESS", "Process Completed", Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(_qryUpdateStatusAuth.PRRefNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }

        }

        //[HttpPost("ApprovePRAuthorizationRush")]
        //public async Task<TblResponse> ApprovePRAuthorizationRush(qryUpdateStatusAuth _qryUpdateStatusAuth)
        //{
        //    try
        //    {
        //        logger.LogInformation("Approve PRAuthorization - " + Utilities.GetmethodName() + "");
        //        await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qryUpdateStatusAuth);
        //        logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
        //        _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_qryUpdateStatusAuth.PRRefNo, "SUCCESS", "Process Completed", Utilities.GetmethodName());
        //        return await Task.FromResult(_response);

        //    }
        //    catch (Exception ex)
        //    {
        //        string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
        //        logger.LogError(error);

        //        await _RepositoryUnit.ResponseRepository.CreateResponse(_qryUpdateStatusAuth.PRRefNo, "FAILED", ex.Message, Utilities.GetmethodName());
        //        return await Task.FromResult(_response);
        //    }

        //}

        [HttpPost("UpdateReadPRAuthorization")]
        public async Task<TblResponse> UpdateReadPRAuthorization(qryUpdateStatusAuth _qryUpdateStatusAuth)
        {
            try
            {
                logger.LogInformation("Approve PRAuthorization - " + Utilities.GetmethodName() + "");
                await _RepositoryUnit.PRAuthorizationRepository.UpdateReadPRAuthorization(_qryUpdateStatusAuth);
                logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_qryUpdateStatusAuth.PRRefNo, "SUCCESS", "Process Completed", Utilities.GetmethodName());
                return await Task.FromResult(_response);

            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(_qryUpdateStatusAuth.PRRefNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }

        }

        [HttpPost("DisapprovePRAuthorization")]
        public async Task<TblResponse> DisapprovePRAuthorization(qryUpdateStatusAuth _qryUpdateStatusAuth)
        {
            try
            {
                logger.LogInformation("Disapprove PRAuthorization - " + Utilities.GetmethodName() + "");
                await _RepositoryUnit.PRAuthorizationRepository.DisapprovePRAuthorization(_qryUpdateStatusAuth);
                logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_qryUpdateStatusAuth.PRRefNo, "SUCCESS", "Process Completed.", Utilities.GetmethodName());
                return await Task.FromResult(_response);

            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(_qryUpdateStatusAuth.PRRefNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }

        }

        [HttpPost("InsertReqReason")]
        public async Task<TblResponse> InsertReqReason(TblRequisitionReason _TblRequisitionReason)
        {
            try
            {
                logger.LogInformation("InsertReqReason - " + Utilities.GetmethodName() + "");

                await _RepositoryUnit.TblRequisitionReasonRepository.Create(_TblRequisitionReason);
                logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_TblRequisitionReason.ReqNo, "SUCCESS", "Process Completed.", Utilities.GetmethodName());
                return await Task.FromResult(_response);

            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(_TblRequisitionReason.ReqNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }

        }

        [HttpPost("UpdatePaymentRequestAuthRemarks")]
        public async Task<TblResponse> UpdatePaymentRequestAuthRemarks(string prno, string personid)
        {
            try
            {
                logger.LogInformation("Approve PRAuthorization - " + Utilities.GetmethodName() + "");
                await _RepositoryUnit.PRAuthorizationRepository.UpdatePaymentRequestAuthRemarks(prno, personid);
                logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(prno, "SUCCESS", "Successfully Updated.", Utilities.GetmethodName());
                return await Task.FromResult(_response);

            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(prno, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }

        }

        [HttpPost("ApprovePRAuthorization_SPASv1")]
        public async Task<TblResponse> ApprovePRAuthorization_SPASv1(qryUpdateStatusAuth _qryUpdateStatusAuth)
        {
            try
            {
                logger.LogInformation("Approve PRAuthorization - " + Utilities.GetmethodName() + "");
                //string personid = empid;//repos getpersonid
                await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qryUpdateStatusAuth);
                logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_qryUpdateStatusAuth.PRRefNo, "SUCCESS", "Successfully Updated.", Utilities.GetmethodName());
                return await Task.FromResult(_response);

            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(_qryUpdateStatusAuth.PRRefNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }

        }

        [HttpPost("CreateTblBatchApproval")]
        public async Task<TblResponse> CreateTblBatchApproval(TblBatchApproval _TblBatchApproval)
        {
            try
            {
                logger.LogInformation("CreateBatchApproval - " + Utilities.GetmethodName() + "");
                //string personid = empid;//repos getpersonid
                await _RepositoryUnit.BatchApprovalRepository.CreateTblBatchApproval(_TblBatchApproval);
                logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_TblBatchApproval.BANo, "SUCCESS", "Successfully Updated.", Utilities.GetmethodName());
                return await Task.FromResult(_response);

            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(_TblBatchApproval.BANo, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }

        }

        [HttpGet("GetLastestBANo")]
        public async Task<string> GetLastestBANo(string reqtype, string payclasscode)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                string vlist = await _RepositoryUnit.BatchApprovalRepository.GetLastestBANo(reqtype, payclasscode);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetPaymentRequestAuthListByPRNO")]
        public async Task<IList<qryPaymentRequestAuthDtl>> GetPaymentRequestAuthListByPRNO(string prno)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                IList<qryPaymentRequestAuthDtl> vlist = await _RepositoryUnit.PRAuthorizationRepository.GetPaymentRequestAuthListByPRNO(prno);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetPositionCode")]
        public async Task<string> GetPositionCode(string personid)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                string vlist = await _RepositoryUnit.PRAuthorizationRepository.GetPositionCode(personid);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetRequestPaymentRequestbyStatus")]
        public async Task<IList<qryRequestPaymentRequestbyStatus>> GetRequestPaymentRequestbyStatus(string status, string personid)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                IList<qryRequestPaymentRequestbyStatus> vlist = await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus(status, personid);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return new List<qryRequestPaymentRequestbyStatus>();

            }
        }



        [HttpGet("GetNameofAuthorizer")]
        public async Task<string> GetNameofAuthorizer(string personid)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                string vlist = await _RepositoryUnit.PRAuthorizationRepository.GetNameofAuthorizer(personid);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetRequestDate")]
        public async Task<DateTime> GetRequestDate(string prno)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                DateTime vlist = await _RepositoryUnit.PRAuthorizationRepository.GetRequestDate(prno);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return Convert.ToDateTime("1900/01/01");

            }
        }

        [HttpGet("GetBatchPRNo")]
        public async Task<IList<TblRequisitionhdr>> GetBatchPRNo(string personid)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                IList<TblRequisitionhdr> vlist = await _RepositoryUnit.PRAuthorizationRepository.GetBatchPRNo(personid);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetDeclineReason")]
        public async Task<IList<qryDeclineReason>> GetDeclineReason()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                IList<qryDeclineReason> vlist = await _RepositoryUnit.PRAuthorizationRepository.GetDeclineReason();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetDeniedReason")]
        public async Task<IList<qryDeclineReason>> GetDeniedReason()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                IList<qryDeclineReason> vlist = await _RepositoryUnit.PRAuthorizationRepository.GetDeclineReason();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetBatchPRNoByPayclassCode")]
        public async Task<IList<TblRequisitionhdr>> GetBatchPRNoByPayclassCode(string personid, string payclasscode)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                IList<TblRequisitionhdr> vlist = await _RepositoryUnit.PRAuthorizationRepository.GetBatchPRNoByPayclassCode(personid, payclasscode);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetPaymentClassAuthorization")]
        public async Task<IList<qryPaymentClassAuthorization>> GetPaymentClassAuthorization(string personid)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                IList<qryPaymentClassAuthorization> vlist = await _RepositoryUnit.PRAuthorizationRepository.GetPaymentClassAuthorization(personid);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetRptPODtl")]
        public async Task<IList<qryRptPurchaseOrderDetails>> GetRptPODtl(string pono)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                IList<qryRptPurchaseOrderDetails> vlist = await _RepositoryUnit.PRAuthorizationRepository.GetRptPODtl(pono);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetRptPurchaseOrderConsolidated")]
        public async Task<IList<qryRptPurchaseOrderConsolidated>> GetRptPurchaseOrderConsolidated(string pono)
        {
            try
            {



                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                IList<qryRptPurchaseOrderConsolidated> vlist = await _RepositoryUnit.PRAuthorizationRepository.GetRptPurchaseOrderConsolidated(pono);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        //[HttpGet("GetRptPurchaseOrderConsolidated")]
        //public async Task<IList<qryRptPurchaseOrderConsolidated>> GetRptPurchaseOrderConsolidated(string reqno,string vendorname)
        //{
        //    try
        //    {

        //        IList<string> _listreqno = JsonSerializer.Deserialize<IList<string>>(reqno);

        //        logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
        //        IList<qryRptPurchaseOrderConsolidated> vlist = await _RepositoryUnit.PRAuthorizationRepository.GetRptPurchaseOrderConsolidated(pono);
        //        logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
        //        return vlist;
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
        //        logger.LogError(ex, error);
        //        return null;

        //    }
        //}

        [HttpGet("GetPOSignatories")]
        public async Task<qryPOSignatories> GetPOSignatories(string pono)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");



                qryPOSignatories vlist = new qryPOSignatories();



                vlist = await _RepositoryUnit.PRAuthorizationRepository.GetPOSignatories(pono);






                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");






                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetRptPOHdr")]
        public async Task<qryRptPurchaseOrderHdr> GetRptPOHdr(string pono)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                qryRptPurchaseOrderHdr vlist = await _RepositoryUnit.PRAuthorizationRepository.GetRptPOHdr(pono);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetBatchNoByPRNo")]
        public async Task<string> GetBatchNoByPRNo(string prno)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                string vlist = await _RepositoryUnit.PRAuthorizationRepository.GetBatchNoByPRNo(prno);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetRequestDateByBatchNo")]
        public async Task<DateTime> GetRequestDateByBatchNo(string batchprno)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                DateTime vlist = await _RepositoryUnit.PRAuthorizationRepository.GetRequestDateByBatchNo(batchprno);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return Convert.ToDateTime("1900/01/01");

            }
        }

        [HttpGet("GetEmailByPersonID")]
        public async Task<string> GetEmailByPersonID(string personid)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                string vlist = await _RepositoryUnit.PRAuthorizationRepository.GetEmailByPersonID(personid);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return string.Empty;

            }
        }

        [HttpGet("GetAuthorizerGroup")]
        public async Task<IList<TblAuthorizerGroup>> GetAuthorizerGroup(string groupid)
        {

            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizerGroup(groupid);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetGenderByPersonID")]
        public async Task<string> GetGenderByPersonID(string personid)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                string vlist = await _RepositoryUnit.PRAuthorizationRepository.GetGenderByPersonID(personid);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return string.Empty;

            }
        }


        [HttpPost("ProcessAuthorization")]
        public async Task<IActionResult> ProcessAuthorization(AuthorizationParams _AuthorizationParams)
        {
            _response = new TblResponse();
            var debug = "Start your engine";
            try
            {
                string reqnofornextauth = string.Empty;
                string batchPRNo = string.Empty;

                IList<string> batchPRNoList = new List<string>();
                string _personid = _AuthorizationParams.UserCode;
                string authpayclass = string.Empty;
                IList<string> _personidlist = new List<string>();
                IList<string> _prnolist = new List<string>();
                IList<string> _prnolisttoscctg = new List<string>();
                IList<string> _batchPRNolist = new List<string>();
                IList<string> _SinglePRNolist = new List<string>();
                IList<string> _CreatePOReqnolist = new List<string>();
                IList<string> _EndorseToAcctglist = new List<string>();

                IList<string> _NextAuthorization_Single = new List<string>();
                IList<string> _NextAuthorization_Batch = new List<string>();

                TblPaymentRequestAuth _TblPaymentRequestAuth_PRNO;
                TblSendEmail _tblsendemail = new TblSendEmail();

                PRAuthorizationModel _PRAuthorizationModel = new PRAuthorizationModel();

                OSPParams oSPParams = new OSPParams();
                oSPParams.PersonIdList = new List<string>();


                //var query = new Dictionary<string, string>()
                //{
                //    ["systemcode"] = "SPASv2",

                //};
                // requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);



                TblRequisitionhdr reqhdr = await _RepositoryUnit.TblRequisitionHdrRepository.ReadRequisitionHdr(_AuthorizationParams.ReqNo.FirstOrDefault());




                foreach (var itemreqno in _AuthorizationParams.ReqNo)
                {
                    debug = " GetBatchNoByPRNo";
                    batchPRNo = await _RepositoryUnit.PRAuthorizationRepository.GetBatchNoByPRNo(itemreqno);
                    if (!String.IsNullOrEmpty(batchPRNo))
                    {
                        _batchPRNolist.Add(itemreqno);
                    }
                    else
                    {
                        _SinglePRNolist.Add(itemreqno);
                    }
                }
                _personidlist.Clear();

                debug = " FOR SINGLE";
                //FOR SINGLE
                foreach (var item_singlereqno in _SinglePRNolist)
                {
                    _TblPaymentRequestAuth_PRNO = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel(item_singlereqno);
                    if (_TblPaymentRequestAuth_PRNO.Remarks == "FOR PAYMENT")
                    {
                        _CreatePOReqnolist.Add(item_singlereqno);
                    }
                    if (_TblPaymentRequestAuth_PRNO.PersonID == "ACCTG-APV")
                    {
                        _EndorseToAcctglist.Add(item_singlereqno);
                    }

                    if (_TblPaymentRequestAuth_PRNO.PersonID != "ACCTG-APV" && _TblPaymentRequestAuth_PRNO.Remarks != "FOR PAYMENT")
                    {
                        _NextAuthorization_Single.Add(item_singlereqno);
                    }
                    oSPParams.PersonIdList.Add(_TblPaymentRequestAuth_PRNO.PersonID);
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                debug = " FOR BATCH";
                //FOR BATCH
                foreach (var item_batchreqno in _batchPRNolist)
                {
                    _TblPaymentRequestAuth_PRNO = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel(item_batchreqno);
                    oSPParams.PersonIdList.Add(_TblPaymentRequestAuth_PRNO.PersonID);
                    if (_TblPaymentRequestAuth_PRNO.Remarks == "FOR PAYMENT")
                    {
                        _CreatePOReqnolist.Add(item_batchreqno);

                    }
                    if (_TblPaymentRequestAuth_PRNO.PersonID == "ACCTG-APV")
                    {
                        _EndorseToAcctglist.Add(item_batchreqno);
                    }
                    if (_TblPaymentRequestAuth_PRNO.PersonID != "ACCTG-APV" && _TblPaymentRequestAuth_PRNO.Remarks != "FOR PAYMENT")
                    {
                        _NextAuthorization_Batch.Add(item_batchreqno);
                        _personidlist.Add(_TblPaymentRequestAuth_PRNO.PersonID);
                        reqnofornextauth = item_batchreqno;

                    }

                }
                /////////////////
                ///
                string requestAddress = OSPUrlRepo + "/CommonRepository/GetEPPSDetails";
                oSPParams = await UtilitiesHttpClient<OSPParams>.PostAsyncT<OSPParams>(oSPParams, requestAddress);
                //if (oSPParams.TblResponse == null)
                //{
                //    oSPParams.TblResponse.ErrorMessage = "ERROR GG";
                //    goto Finish;
                //}
                //else if (oSPParams.TblResponse.Status == "FAILED")
                //{
                //    oSPParams.TblResponse.ErrorMessage = "ERROR GGWP";
                //    goto Finish;
                //}

                if (_NextAuthorization_Single.Count > 0)
                {

                    //await EmailAdvisory("NextAuth_Single", item, authpayclass, "", _NextAuthorization_Single, oSPParams);
                    oSPParams.listNo = _NextAuthorization_Single;
                    _response = await EmailAdvisory("NextAuth_Single", "", authpayclass, "", oSPParams);


                }
                ////////////////
                if (_NextAuthorization_Batch.Count > 0)
                {
                    foreach (var item in _personidlist)
                    {
                        var _positioncode = await GetPositionCode(item);
                        batchPRNo = await _RepositoryUnit.PRAuthorizationRepository.GetBatchNoByPRNo(reqnofornextauth);
                        batchPRNoList.Add(batchPRNo);


                        _PRAuthorizationModel.qryPRAuthorizationList = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists_Batch(item, batchPRNo);

                        _prnolist.Clear();
                        foreach (string prno in _PRAuthorizationModel.qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                        {
                            _prnolist.Add(prno);
                            authpayclass = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizeClass(prno, item);
                        }

                        //await EmailAdvisory("NextAuth_Batch", item, authpayclass, "", _NextAuthorization_Batch, OSPParams);
                        oSPParams.listNo = _NextAuthorization_Batch;
                        _response = await EmailAdvisory("NextAuth_Batch", item, authpayclass, "", oSPParams);
                    }
                }

                ///////////////
                if (_EndorseToAcctglist.Count > 0)
                {
                    AuthorizationParams _endorsetoaccountingAuthParams = new AuthorizationParams();
                    _endorsetoaccountingAuthParams.ReqNo = _EndorseToAcctglist;


                    IList<qryRequisitionInfo> _qryRequisitionInfo = new List<qryRequisitionInfo>();
                    IList<string> _vendor = new List<string>();
                    string vendorgrp = string.Empty;

                    foreach (var reqno in _EndorseToAcctglist)
                    {
                        qryRequisitionInfo _reqinfoawait = await _RepositoryUnit.TblRequisitionHdrRepository.GetRequisitionInfo(reqno);
                        _qryRequisitionInfo.Add(_reqinfoawait);
                    }

                    var result = _qryRequisitionInfo
                    .GroupBy(x => new { x.VendorCode, x.ItemCompany })
                    .Select(grp => grp.ToList())
                    .ToList();


                    string _BANo;
                    foreach (var reqnogroup in result)
                    {
                        //_BANo = await _RepositoryUnit.BatchApprovalRepository.GetLastestBANo("PY", "12345");
                        _BANo = await _RepositoryUnit.BatchApprovalRepository.GetLastestBANo("PY", reqhdr.PayClassCode);



                        foreach (var itemgrpup in reqnogroup)
                        {
                            TblBatchApproval _TblBatchApproval = new TblBatchApproval();
                            _TblBatchApproval.BANo = _BANo;
                            _TblBatchApproval.ReqNo = itemgrpup.ReqNo;
                            _TblBatchApproval.ReqType = "PY";
                            _TblBatchApproval.Active = false;
                            _TblBatchApproval.AuditUser = _personid;
                            _TblBatchApproval.AuditDate = DateTime.Now;

                            try
                            {
                                _response = await _RepositoryUnit.BatchApprovalRepository.CreateTblBatchApproval(_TblBatchApproval);
                            }
                            catch (Exception e)
                            {
                                _response.Status = "FAILED";
                                _response.ErrorMessage = e.Message + "ERROR batch approval insert";
                                return BadRequest(_response);
                            }
                            vendorgrp = itemgrpup.VendorCode;
                        }
                        //_endorsetoaccountingAuthParams.ReqNo.Clear();
                        _endorsetoaccountingAuthParams.BANo = _BANo;
                        _endorsetoaccountingAuthParams.UserCode = _AuthorizationParams.UserCode;


                        _endorsetoaccountingAuthParams.ReqNo.Clear();
                        foreach (var itemgrpup in reqnogroup)
                        {
                            _endorsetoaccountingAuthParams.ReqNo.Add(itemgrpup.ReqNo);
                        }


                        //_endorsetoaccountingAuthParams.BatchReqno = ;

                        requestAddress = BaseUrlRepo + "/Rudy/EndorseToAccounting";
                        _response = await UtilitiesHttpClient<AuthorizationParams>.PostAsync(_endorsetoaccountingAuthParams, requestAddress);

                        IList<qryRptTransmittalFO> _qryRptTransmittalFO = new List<qryRptTransmittalFO>();
                        _qryRptTransmittalFO = await _RepositoryUnit.PRAuthorizationRepository.GetRptTransmittalLO(_BANo);
                        IList<string> _PYCompanyCode = new List<string>();

                        foreach (var item in _qryRptTransmittalFO)
                        {
                            _PYCompanyCode.Add(item.CompanyCode);

                        }

                        foreach (var item in _PYCompanyCode.Distinct())
                        {
                            await this.ReportFileSPASv1(_BANo, item, vendorgrp);
                        }
                    }
                }

                debug = " itemCreatePOReqnolist";
                foreach (var itemCreatePOReqnolist in _CreatePOReqnolist)
                {


                    TblPurchaseorderhdr _tblPurchaseorderhdr = new TblPurchaseorderhdr();
                    _tblPurchaseorderhdr.PONo = "1";
                    _tblPurchaseorderhdr.Reqno = itemCreatePOReqnolist;
                    _tblPurchaseorderhdr.PODate = DateTime.Now;
                    _tblPurchaseorderhdr.Active = false;
                    _tblPurchaseorderhdr.Remarks = "PO as of " + DateTime.Now.ToShortDateString();
                    _tblPurchaseorderhdr.Printed = false;
                    _tblPurchaseorderhdr.AuditUser = _personid;
                    _tblPurchaseorderhdr.AuditDate = DateTime.Now;
                    _tblPurchaseorderhdr.TrxMonth = "JAN24";
                    _tblPurchaseorderhdr.TrxWeek = 0;

                    try
                    {
                        //debug = "Jon/CreateTblPurchaseOrderHdr";
                        //requestAddress = BaseUrlRepo + "/Jon/CreateTblPurchaseOrderHdr";
                        //_response = await UtilitiesHttpClient<TblPurchaseorderhdr>.PostAsync(_tblPurchaseorderhdr, requestAddress);

                        _response = await _jonController.CreateTblPurchaseOrderHdr(_tblPurchaseorderhdr);



                    }
                    catch (Exception e)
                    {
                        _response.Status = "FAILED";
                        _response.ErrorMessage = e.Message + "ERROR PO insert";
                        return BadRequest(_response);
                    }
                }

                //Creating Batch Approval
                if (_CreatePOReqnolist.Count > 0)
                {


                    debug = "GetLastestBANo";


                    string _BANo = await _RepositoryUnit.BatchApprovalRepository.GetLastestBANo("PO", reqhdr.PayClassCode);
                    foreach (var reqno in _CreatePOReqnolist)
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
                            _response = await _RepositoryUnit.BatchApprovalRepository.CreateTblBatchApproval(_TblBatchApproval);
                        }
                        catch (Exception e)
                        {
                            _response.Status = "FAILED";
                            _response.ErrorMessage = e.Message + "ERROR batch approval insert";
                            return BadRequest(_response);
                        }

                    }

                    //await this.Report("MC24000006");
                    debug = "GetRptPurchaseorderByBANo";
                    IList<RptPurchaseorder> _RptPurchaseorder = new List<RptPurchaseorder>();
                    _RptPurchaseorder = await _RepositoryUnit.PRAuthorizationRepository.GetRptPurchaseorderByBANo(_BANo);

                    IList<string> SendToVendorList = new List<string>();
                    IList<string> SendToChapelList = new List<string>();
                    IList<string> SendToGCMList = new List<string>();
                    IList<string> reqnolist = new List<string>();

                    qryChapelBranchDetails _qryChapelBranchDetails = new qryChapelBranchDetails();

                    foreach (var item in _RptPurchaseorder.DistinctBy(a => a.VendorName))
                    {
                        //await _rudyController.CreateExcelForVendor(item.PONo);

                        SendToVendorList.Add(item.VendorName);
                    }

                    if (await _RepositoryUnit.TblAssignedtoVendor_CMSRepository.ReadByReqNo(_RptPurchaseorder.Select(a => a.ReqNo).FirstOrDefault()) != null)
                    {
                        await Task.Run(async () =>
                        {
                            RequisitionParams _RequisitionParams = new RequisitionParams();
                            ServiceParams _ServiceParams = new ServiceParams();

                            _ServiceParams.qryCMSPOHdrList = new List<qryCMSPOHdr>();
                            _ServiceParams.qryCMSPODtlList = new List<qryCMSPODtl>();
                            _ServiceParams.qryCMSRefChapelList = new List<qryCMSRefChapel>();
                            TblVendorPayClass _TblVendorPayClass = new TblVendorPayClass();
                            _ServiceParams.SystemCode = await _RepositoryUnit.RefSystemsRepository.GetSystemCode();
                            foreach (var item in _RptPurchaseorder.Select(a => a.PONo).Distinct())
                            {
                                //await _rudyController.CreateExcelForVendor(item.PONo);
                                _RequisitionParams = new RequisitionParams();
                                string BatchApprovalNo = string.Empty;
                                _RequisitionParams.PONo = item;
                                _RequisitionParams.ServerPOPath = _ReportFilespath;
                                _RequisitionParams.UserID = _personid;
                                var _ReqHdr = await _RepositoryUnit.TblRequisitionHdrRepository.ReadRequisitionHdr(_RptPurchaseorder.Select(a => a.ReqNo).FirstOrDefault());
                                _TblVendorPayClass = await _RepositoryUnit.TblVendorPayClassRepository.ReadPayClass(_ReqHdr.VendorCode, _ReqHdr.PayClassCode);
                                await _RepositoryUnit.TblAssignedtoVendor_CMSRepository.UpdatePONo(item, _RptPurchaseorder.Select(a => a.ReqNo).FirstOrDefault());
                                // List<RptPurchaseorder> rpt =await  _RepositoryUnit.rptPurchaseorderRepository.GetListByPONo(item);



                                switch (_TblVendorPayClass.POType)
                                {
                                    case 1:
                                        await _ServiceUnit.VendorService.CreateExcelForVendor(_RequisitionParams, Path.Combine(CMSDeliveryTemplate, "PO Delivery Template 2.xlsx"));
                                        break;

                                    case 2:
                                        List<TblAssignedtoVendor_CMS> _TblAssignedtoVendor_CMSList = new List<TblAssignedtoVendor_CMS>();
                                        TblAssignedtoVendor_CMS _TblAssignedtoVendor_CMS = new TblAssignedtoVendor_CMS();
                                        List<TblRequisitiondtl> _TblRequisitiondtlList = new List<TblRequisitiondtl>();
                                        List<RefChapel> _RefChapelList = new List<RefChapel>();
                                        List<RefChapelBranch> _RefChapelBranchList = new List<RefChapelBranch>();
                                        List<RefChapelEmail> _RefChapelEmail = new List<RefChapelEmail>();
                                        List<string> _DistinctVendor = new List<string>();

                                        _TblAssignedtoVendor_CMSList = await _RepositoryUnit.TblAssignedtoVendor_CMSRepository.ReadByReqNoList(_ReqHdr.Reqno);
                                        _TblAssignedtoVendor_CMS = await _RepositoryUnit.TblAssignedtoVendor_CMSRepository.ReadByReqNo(_ReqHdr.Reqno);
                                        _TblRequisitiondtlList = await _RepositoryUnit.TblRequisitionDtlRepository.Read(_ReqHdr.Reqno);
                                        _RefChapelList = await _RepositoryUnit.RefChapelRepository.GetAllChapels();
                                        _RefChapelBranchList = await _RepositoryUnit.RefChapelBranchRepository.GetRefChapelBranches();
                                        _RefChapelEmail = await _RepositoryUnit.RefChapelEmailRepository.GetRefChapelEmailList();
                                        _DistinctVendor = _TblAssignedtoVendor_CMSList.Select(a => a.VendorCode).Distinct().ToList();

                                        //DateTime dt = DateTime.Now;
                                        foreach (string VendorCode in _DistinctVendor)
                                        {
                                            foreach (var item1 in _TblAssignedtoVendor_CMSList)
                                            {
                                                if (item1.VendorCode.Equals(VendorCode))
                                                {
                                                    foreach (var dtl in _TblRequisitiondtlList)
                                                    {
                                                        if (_ServiceParams.qryCMSPOHdrList.Where(a => a.FactoryCode.Equals(item1.VendorCode) && a.ChapelCode.Equals(dtl.DeptCode)).ToList().Count == 0)
                                                        {
                                                            qryCMSPOHdr qryCMSPOHdr = new qryCMSPOHdr()
                                                            {
                                                                PONo = item,
                                                                FactoryCode = item1.VendorCode,
                                                                CompanyCode = _ReqHdr.DtlCompanyCode,
                                                                ChapelCode = dtl.DeptCode,
                                                                PODate = _ReqHdr.ReqDate,
                                                                POReceivedDate = Convert.ToDateTime("1900-01-01"),
                                                                Terms = _TblVendorPayClass.Terms,
                                                                Remarks = _ReqHdr.Remarks,
                                                                POAmount = _ReqHdr.AmountDue,
                                                                AuditUser = _ReqHdr.AuditUser,
                                                                AuditDate = _ReqHdr.AuditDate,
                                                                EditUser = _ReqHdr.AuditUser,
                                                                EditDate = _ReqHdr.EditDate,
                                                                Void = false,
                                                                VoidUser = string.Empty,
                                                                VoidDate = Convert.ToDateTime("1900-01-01")
                                                            };

                                                            qryCMSRefChapel qryCMSRefChapel = new qryCMSRefChapel()
                                                            {
                                                                ChapelCode = dtl.DeptCode,
                                                                ChapelDesc = _RefChapelList.Where(a => a.ChapelCode.Equals(dtl.DeptCode)).Select(a => a.ChapelDesc).FirstOrDefault(),
                                                                CompanyCode = _RefChapelList.Where(a => a.ChapelCode.Equals(dtl.DeptCode)).Select(a => a.CompanyCode).FirstOrDefault(),
                                                                Address = _RefChapelList.Where(a => a.ChapelCode.Equals(dtl.DeptCode)).Select(a => a.Address).FirstOrDefault(),
                                                                ChapelMngr = _RefChapelBranchList.Where(a => a.ChapelCode.Equals(dtl.DeptCode)).Select(a => a.ChapelMngr).FirstOrDefault(),
                                                                ContactNo = _RefChapelBranchList.Where(a => a.ChapelCode.Equals(dtl.DeptCode)).Select(a => a.ContactNo).FirstOrDefault(),
                                                                Email = _RefChapelEmail.Where(a => a.ChapelCode.Equals(dtl.DeptCode)).Select(a => a.Email).FirstOrDefault(),
                                                                ChapelType = _RefChapelList.Where(a => a.ChapelCode.Equals(dtl.DeptCode)).Select(a => a.ChapelTypeCode).FirstOrDefault(),
                                                                Active = true,
                                                                Class = _RefChapelList.Where(a => a.ChapelCode.Equals(dtl.DeptCode)).Select(a => a.ChapelClass).FirstOrDefault(),
                                                                RegionCode = _RefChapelList.Where(a => a.ChapelCode.Equals(dtl.DeptCode)).Select(a => a.RegionCode).FirstOrDefault(),
                                                                TerritoryCode = _RefChapelList.Where(a => a.ChapelCode.Equals(dtl.DeptCode)).Select(a => a.TerritoryCode).FirstOrDefault(),
                                                                AuditUser = _ReqHdr.AuditUser,
                                                                AuditDate = _ReqHdr.EditDate,
                                                                UploadStat = false,
                                                                EditUser = _ReqHdr.AuditUser,
                                                                EditDate = _ReqHdr.EditDate
                                                            };



                                                            _ServiceParams.qryCMSPOHdrList.Add(qryCMSPOHdr);
                                                            _ServiceParams.qryCMSRefChapelList.Add(qryCMSRefChapel);

                                                            qryCMSPODtl qryCMSPODtl = new qryCMSPODtl()
                                                            {
                                                                FactoryCode = item1.VendorCode,
                                                                PONo = item,
                                                                CasketCode = dtl.ItemCode,
                                                                OrderQty = dtl.Quantity,
                                                                POAmount = dtl.TotalAmount,
                                                                AuditUser = _ReqHdr.AuditUser,
                                                                AuditDate = dtl.AuditDate,
                                                                EditUser = _ReqHdr.AuditUser,
                                                                EditDate = dtl.EditDate
                                                            };

                                                            if (_ServiceParams.qryCMSPODtlList.Where(a => a.FactoryCode.Equals(qryCMSPODtl.FactoryCode) && a.CasketCode.Equals(qryCMSPODtl.CasketCode)).ToList().Count == 0) //&& a.ChapelCode.Equals(qryCMSPODtl.ChapelCode)
                                                            {
                                                                _ServiceParams.qryCMSPODtlList.Add(qryCMSPODtl);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (_ServiceParams.qryCMSPOHdrList.Count > 0)
                                            {
                                                //CREATE BKP
                                                BatchApprovalNo = await _RepositoryUnit.BatchApprovalRepository.GetBatchNoByReqNo(_RptPurchaseorder.Select(a => a.ReqNo).FirstOrDefault());
                                                // _ServiceParams.qryCMSPOHdrList.Add(qryCMSPOHdr);

                                                _ServiceParams.BKPTemplatePath = Path.Combine(BKPTemplatePath, "Template.MDB");
                                                _ServiceParams.BKPSavingPath = Path.Combine(_ReportFilespath, "CIS PO", BatchApprovalNo, item + "-" + _RptPurchaseorder.Select(a => a.VendorName.Replace(".", "").Replace(",", "")).FirstOrDefault());

                                                if (_TblVendorPayClass.POType == 2)
                                                {
                                                    requestAddress = BaseUrlService + "/Requisition/CreateCMSBKP";
                                                    _response = await UtilitiesHttpClient<ServiceParams>.PostAsync(_ServiceParams, requestAddress);
                                                }

                                                //CLEAR LIST
                                                _ServiceParams.qryCMSPOHdrList = new List<qryCMSPOHdr>();
                                                _ServiceParams.qryCMSRefChapelList = new List<qryCMSRefChapel>();
                                                _ServiceParams.qryCMSPODtlList = new List<qryCMSPODtl>();

                                            }
                                        }

                                        break;
                                }
                            }

                        });
                    }



                    SendToVendorList = SendToVendorList.Distinct().ToList();

                    foreach (var item in SendToVendorList)
                    {
                        var str = await this.ReportPOVendor(_BANo, item, reqhdr.PayClassCode);
                        debug = str.ToString();
                        debug = debug + " VendorPO";
                        oSPParams.listNo = null;
                        //_response = await EmailAdvisory("VendorPO", _BANo, item, "",oSPParams);

                        //if (_response.Status == "FAILED")
                        //{
                        //    throw new Exception(_response.ErrorMessage);
                        //}
                    }

                    foreach (var item in _RptPurchaseorder.DistinctBy(a => a.ReqNo))
                    {
                        reqnolist.Add(item.ReqNo);
                    }
                    IList<TblRequisitiondtl> _reqdtl = new List<TblRequisitiondtl>();

                    foreach (var item in reqnolist.Distinct())
                    {
                        _reqdtl = await _RepositoryUnit.TblRequisitionDtlRepository.Read(item);
                        foreach (var item2 in _reqdtl.DistinctBy(a => a.DeptCode))
                        {
                            SendToChapelList.Add(item2.DeptCode);
                        }

                    }
                    string gcmid = string.Empty;
                    foreach (var item in SendToChapelList.Distinct())
                    {
                        var str = await this.ReportChapelAdvisory(_BANo, item, reqhdr.PayClassCode);
                        debug = str.ToString();
                        debug = debug + " ChapelAdvisoryForCM";
                        //_response = await EmailAdvisory("ChapelAdvisoryForCM", _BANo, item, item, oSPParams);
                        //if (_response.Status == "FAILED")
                        //{
                        //    throw new Exception(_response.ErrorMessage);
                        //}

                        oSPParams.listNo = null;
                        _qryChapelBranchDetails = await _RepositoryUnit.RefRegionRepository.GetChapelDetails(item);

                        gcmid = _qryChapelBranchDetails.GCMID;
                        SendToGCMList.Add(gcmid);
                    }

                    foreach (var item in SendToGCMList.Distinct())
                    {
                        var str = await this.ReportChapelAdvisory_GCM(_BANo, item, reqhdr.PayClassCode);
                        debug = str.ToString();
                        _qryChapelBranchDetails = await _RepositoryUnit.RefRegionRepository.GetChapelDetailsbygcmname(item);
                        debug = debug + " EmailAdvisory";
                        oSPParams.listNo = null;
                        //_response = await EmailAdvisory("ChapelAdvisoryForGCM", _BANo, item, _qryChapelBranchDetails.GCMName, oSPParams);
                        //if (_response.Status == "FAILED")
                        //{
                        //    throw new Exception(_response.ErrorMessage);
                        //}
                    }

                    ////SENDEMAIL

                    foreach (var item in SendToVendorList)
                    {
                        _response = await EmailAdvisory("VendorPO", _BANo, item, "", oSPParams);
                        if (_response.Status == "FAILED")
                        {
                            throw new Exception(_response.ErrorMessage);
                        }
                    }

                    foreach (var item in SendToChapelList.Distinct())
                    {

                        _response = await EmailAdvisory("ChapelAdvisoryForCM", _BANo, item, item, oSPParams);
                        if (_response.Status == "FAILED")
                        {
                            throw new Exception(_response.ErrorMessage);
                        }
                    }
                    foreach (var item in SendToGCMList.Distinct())
                    {

                        _qryChapelBranchDetails = await _RepositoryUnit.RefRegionRepository.GetChapelDetailsbygcmname(item);
                        _response = await EmailAdvisory("ChapelAdvisoryForGCM", _BANo, item, _qryChapelBranchDetails.GCMName, oSPParams);
                        if (_response.Status == "FAILED")
                        {
                            throw new Exception(_response.ErrorMessage);
                        }
                    }



                }


            //  _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_AuthorizationParams.ReqNo.FirstOrDefault(), "SUCCESS", "Process Completed.", Utilities.GetmethodName());
            Finish:

                return Ok(_response);
            }
            catch (Exception ex)
            {


                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_AuthorizationParams.ReqNo.FirstOrDefault(), "FAILED", "WAAAAAAAAAAAAAAAAAAAAA" + ex.Message + debug, Utilities.GetmethodName());
                _response.ErrorMessage = debug;


                return BadRequest(_response);
            }





        }

        [HttpPost("ReprocessResendPDF_PO")]
        public async Task<IActionResult> ReprocessResendPDF_PO(string bano)
        {

            OSPParams oSPParams = new OSPParams();

            string requestAddress = OSPUrlRepo + "/CommonRepository/GetEPPSDetails";
            oSPParams = await UtilitiesHttpClient<OSPParams>.PostAsyncT<OSPParams>(oSPParams, requestAddress);


            try
            {

                IList<RptPurchaseorder> _RptPurchaseorder = new List<RptPurchaseorder>();
                _RptPurchaseorder = await _RepositoryUnit.PRAuthorizationRepository.GetRptPurchaseorderByBANo(bano);
                IList<string> SendToVendorList = new List<string>();
                IList<string> SendToChapelList = new List<string>();
                IList<string> SendToGCMList = new List<string>();
                IList<string> reqnolist = new List<string>();



                TblRequisitionhdr reqhdr = await _RepositoryUnit.TblRequisitionHdrRepository.ReadRequisitionHdr(_RptPurchaseorder.Select(a => a.ReqNo).FirstOrDefault());

                qryChapelBranchDetails _qryChapelBranchDetails = new qryChapelBranchDetails();

                foreach (var item in _RptPurchaseorder)
                {
                    SendToVendorList.Add(item.VendorName);
                }

                SendToVendorList = SendToVendorList.Distinct().ToList();

                foreach (var item in SendToVendorList)
                {
                    var str = await this.ReportPOVendor(bano, item, reqhdr.PayClassCode);
                }

                foreach (var item in _RptPurchaseorder)
                {
                    reqnolist.Add(item.ReqNo);
                }

                IList<TblRequisitiondtl> _reqdtl = new List<TblRequisitiondtl>();

                foreach (var item in reqnolist.Distinct())
                {
                    _reqdtl = await _RepositoryUnit.TblRequisitionDtlRepository.Read(item);
                    foreach (var item2 in _reqdtl)
                    {
                        SendToChapelList.Add(item2.DeptCode);
                    }
                }

                string gcmid;
                foreach (var item in SendToChapelList.Distinct())
                {
                    var str = await this.ReportChapelAdvisory(bano, item, reqhdr.PayClassCode);
                    _qryChapelBranchDetails = await _RepositoryUnit.RefRegionRepository.GetChapelDetails(item);
                    gcmid = _qryChapelBranchDetails.GCMID;
                    SendToGCMList.Add(gcmid);
                }

                foreach (var item in SendToGCMList.Distinct())
                {
                    var str = await this.ReportChapelAdvisory_GCM(bano, item, reqhdr.PayClassCode);
                    _qryChapelBranchDetails = await _RepositoryUnit.RefRegionRepository.GetChapelDetailsbygcmname(item);

                }
                ///////EMAIL
                ///
                foreach (var item in SendToVendorList)
                {
                    _response = await EmailAdvisory("VendorPO", bano, item, "", oSPParams);
                    if (_response.Status == "FAILED")
                    {
                        throw new Exception(_response.ErrorMessage);
                    }
                }
                foreach (var item in SendToChapelList.Distinct())
                {

                    _response = await EmailAdvisory("ChapelAdvisoryForCM", bano, item, item, oSPParams);
                    if (_response.Status == "FAILED")
                    {
                        throw new Exception(_response.ErrorMessage);
                    }
                }
                foreach (var item in SendToGCMList.Distinct())
                {

                    _qryChapelBranchDetails = await _RepositoryUnit.RefRegionRepository.GetChapelDetailsbygcmname(item);
                    _response = await EmailAdvisory("ChapelAdvisoryForGCM", bano, item, _qryChapelBranchDetails.GCMName, oSPParams);
                    if (_response.Status == "FAILED")
                    {
                        throw new Exception(_response.ErrorMessage);
                    }
                }

            }
            catch (Exception ex)
            {

                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(bano, "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);
            }


            return Ok(_response);
        }





        [HttpPost("Report")]
        public async Task Report(string PONo)
        {
            //var test = _context.Table.ToList();
            //var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Report2.rdlc";

            var dtPO = new DataTable();
            IList<qryRptPurchaseOrderDetails> _qryRptPurchaseOrderDetails = new List<qryRptPurchaseOrderDetails>();
            IList<RptPurchaseorder> _qryRptPurchaseorder = new List<RptPurchaseorder>();

            IList<qryRptPurchaseOrderConsolidated> _qryRptPurchaseOrderConsolidated = new List<qryRptPurchaseOrderConsolidated>();
            _qryRptPurchaseOrderConsolidated = await _RepositoryUnit.PRAuthorizationRepository.GetRptPurchaseOrderConsolidated("MC24000006");
            qryRptPurchaseOrderHdr _qryRptPurchaseOrderHdr = await _RepositoryUnit.PRAuthorizationRepository.GetRptPOHdr("MC24000006");

            dtPO = ConvertIListToDataTable(_qryRptPurchaseOrderConsolidated);

            //dt = GetEmployeeList();

            //string mimetype = "";
            //int extension = 1;

            //var path = @"\\SPLPDEVSERVER\Spasv2$\Reports\Template\rptChapelAdvisory.rdlc";
            //var path = @"C:\SIS\SPASv2\wwwroot\Reports\Template\rptChapelAdvisory.rdlc";
            var path = Path.Combine(_ReportFilespath, "Reports", "Template", "rptPurchaseOrderConsolidated.rdlc");

            using var report = new LocalReport();
            report.ReportPath = path;

            report.DataSources.Add(new ReportDataSource("DataSetPurchaseOrder", dtPO));

            report.SetParameters(new[] { new ReportParameter("prmCompany", _qryRptPurchaseOrderHdr.CompanyDesc) });
            report.SetParameters(new[] { new ReportParameter("prmCompanyAddress", _qryRptPurchaseOrderHdr.Address) });
            report.SetParameters(new[] { new ReportParameter("prmVendor", _qryRptPurchaseOrderHdr.VendorName) });
            report.SetParameters(new[] { new ReportParameter("prmPayeeName", _qryRptPurchaseOrderHdr.PayeeName) });
            report.SetParameters(new[] { new ReportParameter("prmPODate", DateTime.Now.ToString("MM/dd/yyyy")) });
            //report.SetParameters(new[] { new ReportParameter("prmDeduction", 0.ToString()) });

            qryPOSignatories _qryPOSignatories = new qryPOSignatories();


            _qryPOSignatories = await _RepositoryUnit.PRAuthorizationRepository.GetPOSignatories(_qryRptPurchaseOrderHdr.ReqNo);


            report.SetParameters(new[] { new ReportParameter("prmPreparedByName", _qryPOSignatories.PreparedByName) });
            report.SetParameters(new[] { new ReportParameter("prmPreparedByPosition", _qryPOSignatories.PreparedByPosition) });

            report.SetParameters(new[] { new ReportParameter("prmReviewedByName", _qryPOSignatories.ReviewedByName) });
            report.SetParameters(new[] { new ReportParameter("prmReviewedByPosition", _qryPOSignatories.ReviewedByPosition) });

            report.SetParameters(new[] { new ReportParameter("prmApprovedByName", _qryPOSignatories.ApprovedByName) });
            report.SetParameters(new[] { new ReportParameter("prmApprovedByPosition", _qryPOSignatories.ApprovedByPosition) });





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
                string filePath = Path.Combine(_ReportFilespath, "Reports", "POReport", PONo + ".pdf");



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

        [HttpPost("ReportPOVendor")]
        public async Task<IActionResult> ReportPOVendor(string bano, string vendornae, string payclasscode)
        {
            //var test = _context.Table.ToList();
            //var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Report2.rdlc";
            var str = "";
            try
            {

                var dtPO = new DataTable();

                IList<RptPurchaseorder> _qryRptPurchaseorder = new List<RptPurchaseorder>();
                IList<qryRptPurchaseOrderConsolidated> _qryRptPurchaseOrderConsolidated = new List<qryRptPurchaseOrderConsolidated>();

                str = "Stage 1";
                _qryRptPurchaseOrderConsolidated = await _RepositoryUnit.PRAuthorizationRepository.GetRptPurchaseOrderConsolidated(bano, vendornae);

                str = "Stage 2";
                qryRptPurchaseOrderHdr _qryRptPurchaseOrderHdr = await _RepositoryUnit.PRAuthorizationRepository.GetRptPOHdr(bano, vendornae);

                str = "Stage 3";
                dtPO = ConvertIListToDataTable(_qryRptPurchaseOrderConsolidated);

                str = "Stage 4";
                var path = Path.Combine(_ReportFilespath, "Reports", "Template", "rptPurchaseOrderConsolidated.rdlc");

                using var report = new LocalReport();
                report.ReportPath = path;

                report.DataSources.Add(new ReportDataSource("DataSetPurchaseOrder", dtPO));
                report.SetParameters(new[] { new ReportParameter("prmBatchNo", bano) });
                report.SetParameters(new[] { new ReportParameter("prmCompany", "PURCHASE ORDER") });
                report.SetParameters(new[] { new ReportParameter("prmCompanyAddress", " ") });
                report.SetParameters(new[] { new ReportParameter("prmVendor", vendornae) });
                report.SetParameters(new[] { new ReportParameter("prmPayeeName", _qryRptPurchaseOrderHdr.PayeeName) });
                report.SetParameters(new[] { new ReportParameter("prmPODate", DateTime.Now.ToString("MM/dd/yyyy")) });
                //report.SetParameters(new[] { new ReportParameter("prmDeduction", 0.ToString()) });

                List<qrySignatoriesChapelAdvisory> _qrySignatoriesChapelAdvisory = new List<qrySignatoriesChapelAdvisory>();

                str = "Stage 5";




                _qrySignatoriesChapelAdvisory = await _RepositoryUnit.PRAuthorizationRepository.GetSignatoriesChapelAdvisory(bano, payclasscode);


                string _preparedby = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 0).Select(grp => grp.PersonName)
                       .FirstOrDefault();
                string _preparedbyposition = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 0).Select(grp => grp.PositionDesc)
                       .FirstOrDefault();

                string _reviewedby = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 1).Select(grp => grp.PersonName)
                        .FirstOrDefault();
                string _reviewedbyposition = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 1).Select(grp => grp.PositionDesc)
                       .FirstOrDefault();

                string _approver1 = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 2).Select(grp => grp.PersonName)
                       .FirstOrDefault();
                string _approver1byposition = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 2).Select(grp => grp.PositionDesc)
                       .FirstOrDefault();

                string _approver2 = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 3).Select(grp => grp.PersonName)
                        .FirstOrDefault();
                string _approver2byposition = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 3).Select(grp => grp.PositionDesc)
                       .FirstOrDefault();

                report.SetParameters(new[] { new ReportParameter("prmPreparedBy", _preparedby) });
                report.SetParameters(new[] { new ReportParameter("prmPreparedByPosition", _preparedbyposition) });

                report.SetParameters(new[] { new ReportParameter("prmReviewedBy", _reviewedby) });
                report.SetParameters(new[] { new ReportParameter("prmReviewedByPosition", _reviewedbyposition) });

                report.SetParameters(new[] { new ReportParameter("prmApprover1", _approver1) });
                report.SetParameters(new[] { new ReportParameter("prmApprover1Position", _approver1byposition) });

                report.SetParameters(new[] { new ReportParameter("prmApprover2", _approver2) });
                report.SetParameters(new[] { new ReportParameter("prmApprover2Position", _approver2byposition) });

                int ext = (int)(DateTime.Now.Ticks >> 10);
                //byte[] pdf = report.Render("PDF");
                str = "Stage 6";
                string format1 = "PDF";
                string ext1 = "pdf";
                string mimetype1 = "application/pdf";

                try
                {
                    var pdf = report.Render(format1);
                    var file = File(pdf, mimetype1, "report." + ext1);

                    // Specify the path where you want to save the PDF file
                    //string filePath = @"\\SPLPDEVSERVER\Spasv2$\Reports\ChapelAdvisory\" + PONo + ".pdf";

                    string filePathCreateFolderPO;

                    byte[] fileContent = file.FileContents;
                    str = "Stage 7";
                    foreach (var item in _qryRptPurchaseOrderConsolidated)
                    {

                        TblPurchaseorderhdr tblPurchaseorderhdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(item.PONo);
                        filePathCreateFolderPO = Path.Combine(_ReportFilespath, "Requisition", tblPurchaseorderhdr.Reqno, bano + ".pdf");
                        if (!Directory.Exists(Path.Combine(Path.Combine(_ReportFilespath, "Requisition", tblPurchaseorderhdr.Reqno))))
                        {
                            Directory.CreateDirectory(Path.Combine(_ReportFilespath, "Requisition", tblPurchaseorderhdr.Reqno));
                        }

                        using (FileStream fs = new FileStream(filePathCreateFolderPO, FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(fileContent, 0, fileContent.Length);
                        }
                    }

                    //filePath = "\\\\192.168.23.25:88\\SPASv2\\wwwroot\\Reports\\ChapelAdvisory\\" + PONo + ".pdf";

                    // Access the actual file content from the FileContentResult
                    str = "Stage 8";
                    string filePath = Path.Combine(_ReportFilespath, "Reports", "POReport", bano + "-" + vendornae + ".pdf");

                    //Use FileStream to write the PDF content to the file



                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        fs.Write(fileContent, 0, fileContent.Length);
                        //fs.Close();
                    }


                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message + str);
                }


                return Ok(str);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return Ok(str);
        }

        [HttpPost("ReportChapelAdvisory")]
        public async Task<IActionResult> ReportChapelAdvisory(string bano, string ChapelCode, string payclasscode)
        {
            var str = "";
            var dtPO = new DataTable();

            IList<qryRptChapelAdvisory> _qryRptChapelAdvisory = new List<qryRptChapelAdvisory>();
            _qryRptChapelAdvisory = await _RepositoryUnit.PRAuthorizationRepository.GetRptChapelAdvisory(bano, ChapelCode);
            str = "Stage10";
            qryRptPurchaseOrderHdr _qryRptPurchaseOrderHdr = await _RepositoryUnit.PRAuthorizationRepository.GetRptPOHdr(bano);

            dtPO = ConvertIListToDataTable(_qryRptChapelAdvisory);
            str = "Stage11";
            var path = Path.Combine(_ReportFilespath, "Reports", "Template", "rptChapelAdvisory.rdlc");

            using var report = new LocalReport();
            report.ReportPath = path;
            str = "Stage12";
            report.DataSources.Add(new ReportDataSource("DataSet2", dtPO));
            report.SetParameters(new[] { new ReportParameter("prmBANo", bano) });
            report.SetParameters(new[] { new ReportParameter("prmCompany", _qryRptPurchaseOrderHdr.CompanyDesc) });
            report.SetParameters(new[] { new ReportParameter("prmCompanyAddress", _qryRptPurchaseOrderHdr.Address) });
            report.SetParameters(new[] { new ReportParameter("prmPODate", DateTime.Now.ToString("MM/dd/yyyy")) });
            //report.SetParameters(new[] { new ReportParameter("prmDeduction", 0.ToString()) });

            RefChapel _refchapel = await _RepositoryUnit.RefChapelRepository.GetChapelsDetails(ChapelCode);
            str = "Stage13";

            List<qrySignatoriesChapelAdvisory> _qrySignatoriesChapelAdvisory = new List<qrySignatoriesChapelAdvisory>();

            str = "Stage14";
            _qrySignatoriesChapelAdvisory = await _RepositoryUnit.PRAuthorizationRepository.GetSignatoriesChapelAdvisory(bano, payclasscode);


            string _preparedby = _qrySignatoriesChapelAdvisory.Where(x => x.AuthorizeClass == "REQUESTER" && x.Authorizelevel == 0).Select(grp => grp.PersonName)
                   .FirstOrDefault();
            string _preparedbyposition = _qrySignatoriesChapelAdvisory.Where(x => x.AuthorizeClass == "REQUESTER" && x.Authorizelevel == 0).Select(grp => grp.PositionDesc)
                   .FirstOrDefault();

            string _reviewedby = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 1).Select(grp => grp.PersonName)
                    .FirstOrDefault();
            string _reviewedbyposition = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 1).Select(grp => grp.PositionDesc)
                   .FirstOrDefault();

            string _approver1 = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 2).Select(grp => grp.PersonName)
                   .FirstOrDefault();
            string _approver1byposition = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 2).Select(grp => grp.PositionDesc)
                   .FirstOrDefault();

            string _approver2 = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 3).Select(grp => grp.PersonName)
                    .FirstOrDefault();
            string _approver2byposition = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 3).Select(grp => grp.PositionDesc)
                   .FirstOrDefault();

            


            
            str = "Stage15";
            report.SetParameters(new[] { new ReportParameter("prmChapel", _refchapel.ChapelDesc) });
            report.SetParameters(new[] { new ReportParameter("prmAddress", _refchapel.Address) });





            report.SetParameters(new[] { new ReportParameter("prmPreparedBy", _preparedby) });
            report.SetParameters(new[] { new ReportParameter("prmPreparedByPosition", _preparedbyposition) });

            report.SetParameters(new[] { new ReportParameter("prmReviewBy", _reviewedby) });
            report.SetParameters(new[] { new ReportParameter("prmReviewByPosition", _reviewedbyposition) });

            report.SetParameters(new[] { new ReportParameter("prmApprover1", _approver1) });
            report.SetParameters(new[] { new ReportParameter("prmApprover1Position", _approver1byposition) });

            report.SetParameters(new[] { new ReportParameter("prmApprover2", _approver2) });
            report.SetParameters(new[] { new ReportParameter("prmApprover2Position", _approver2byposition) });







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
                string filePath = Path.Combine(_ReportFilespath, "Reports", "ChapelAdvisory", bano + "-" + ChapelCode + ".pdf");

                byte[] fileContent = file.FileContents;

                //filePath = "\\\\192.168.23.25:88\\SPASv2\\wwwroot\\Reports\\ChapelAdvisory\\" + PONo + ".pdf";

                // Access the actual file content from the FileContentResult

                str = "Stage16";
                // Use FileStream to write the PDF content to the file
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(fileContent, 0, fileContent.Length);
                    // No need to call fs.Dispose(), the using statement takes care of it
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message + "Chapel advisory");
            }


            return Ok(str);
        }

        [HttpPost("ReportChapelAdvisory_GCM")]
        public async Task<IActionResult> ReportChapelAdvisory_GCM(string bano, string GCMID, string payclasscode)
        {
            var str = "";
            var dtPO = new DataTable();

            IList<qryRptChapelAdvisory_GCM> _qryRptChapelAdvisory_GCM = new List<qryRptChapelAdvisory_GCM>();
            _qryRptChapelAdvisory_GCM = await _RepositoryUnit.PRAuthorizationRepository.GetRptChapelAdvisory_GCM(bano, GCMID);
            str = "Stage 21";
            qryRptPurchaseOrderHdr _qryRptPurchaseOrderHdr = await _RepositoryUnit.PRAuthorizationRepository.GetRptPOHdr(bano);

            dtPO = ConvertIListToDataTable(_qryRptChapelAdvisory_GCM);
            str = "Stage 22";
            var path = Path.Combine(_ReportFilespath, "Reports", "Template", "rptChapelAdvisory_GCM.rdlc");

            using var report = new LocalReport();
            report.ReportPath = path;
            str = "Stage 23";
            report.DataSources.Add(new ReportDataSource("DataSet3", dtPO));
            report.SetParameters(new[] { new ReportParameter("prmBANo", bano) });
            report.SetParameters(new[] { new ReportParameter("prmCompany", _qryRptPurchaseOrderHdr.CompanyDesc) });
            report.SetParameters(new[] { new ReportParameter("prmCompanyAddress", _qryRptPurchaseOrderHdr.Address) });
            report.SetParameters(new[] { new ReportParameter("prmPODate", DateTime.Now.ToString("MM/dd/yyyy")) });
            //report.SetParameters(new[] { new ReportParameter("prmDeduction", 0.ToString()) });
            List<qrySignatoriesChapelAdvisory> _qrySignatoriesChapelAdvisory = new List<qrySignatoriesChapelAdvisory>();

            _qrySignatoriesChapelAdvisory = await _RepositoryUnit.PRAuthorizationRepository.GetSignatoriesChapelAdvisory(bano, payclasscode);
            string _preparedby = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 0).Select(grp => grp.PersonName)
                   .FirstOrDefault();
            string _preparedbyposition = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 0).Select(grp => grp.PositionDesc)
                   .FirstOrDefault();

            string _reviewedby = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 1).Select(grp => grp.PersonName)
                    .FirstOrDefault();
            string _reviewedbyposition = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 1).Select(grp => grp.PositionDesc)
                   .FirstOrDefault();

            string _approver1 = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 2).Select(grp => grp.PersonName)
                   .FirstOrDefault();
            string _approver1byposition = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 2).Select(grp => grp.PositionDesc)
                   .FirstOrDefault();

            string _approver2 = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 3).Select(grp => grp.PersonName)
                    .FirstOrDefault();
            string _approver2byposition = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 3).Select(grp => grp.PositionDesc)
                   .FirstOrDefault();

            report.SetParameters(new[] { new ReportParameter("prmPreparedBy", _preparedby) });
            report.SetParameters(new[] { new ReportParameter("prmPreparedByPosition", _preparedbyposition) });

            report.SetParameters(new[] { new ReportParameter("prmReviewBy", _reviewedby) });
            report.SetParameters(new[] { new ReportParameter("prmReviewByPosition", _reviewedbyposition) });

            report.SetParameters(new[] { new ReportParameter("prmApprover1", _approver1) });
            report.SetParameters(new[] { new ReportParameter("prmApprover1Position", _approver1byposition) });

            report.SetParameters(new[] { new ReportParameter("prmApprover2", _approver2) });
            report.SetParameters(new[] { new ReportParameter("prmApprover2Position", _approver2byposition) });


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
                string filePath = Path.Combine(_ReportFilespath, "Reports", "ChapelAdvisory", bano + "-" + GCMID + ".pdf");
                byte[] fileContent = file.FileContents;
                //filePath = "\\\\192.168.23.25:88\\SPASv2\\wwwroot\\Reports\\ChapelAdvisory\\" + PONo + ".pdf";
                // Access the actual file content from the FileContentResult
                str = "Stage 24";
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


            return Ok(str);
        }

        [HttpPost("ReportFileSPASv1")]
        public async Task ReportFileSPASv1(string bano, string CompanyCode, string VendorCode)
        {
            //var test = _context.Table.ToList();
            //var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Report2.rdlc";

            var dtPO = new DataTable();

            IList<RptPurchaseorder> _qryRptPurchaseorder = new List<RptPurchaseorder>();
            IList<qryRptTransmittalFO> _qryRptTransmittalFO = new List<qryRptTransmittalFO>();

            _qryRptTransmittalFO = await _RepositoryUnit.PRAuthorizationRepository.GetRptTransmittalLO(bano, CompanyCode, VendorCode);

            // qryRptPurchaseOrderHdr _qryRptPurchaseOrderHdr = await _RepositoryUnit.PRAuthorizationRepository.GetRptPOHdr(bano);

            dtPO = ConvertIListToDataTable(_qryRptTransmittalFO);

            var path = Path.Combine(_ReportFilespath, "Reports", "Template", "rptTransmittalLO.rdlc");

            using var report = new LocalReport();
            report.ReportPath = path;


            //string companydesc = await _RepositoryUnit.RefCompanyRepository.GetCompanyDescByCompanyCode(CompanyCode);

            //string companydesc;
            string requestAddress = OSPUrlRepo + "/CommonRepository/GetCompanyDescByCode";
            var query = new Dictionary<string, string>()
            {
                ["CompanyCode"] = CompanyCode,

            };
            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            var companydesc = await UtilitiesHttpClient<string>.GetJsonstring(requestAddress);


            string _payee = await _RepositoryUnit.VendorRepository.GetVendorNameByVendorCode(VendorCode);


            TblVendorTIN _TblVendorTIN = await _RepositoryUnit.VendorRepository.GetVendorTIN(VendorCode);
            qryVendorDetails _tblVendor = await _RepositoryUnit.VendorRepository.GetVendorDetails(VendorCode, "12345");




            report.DataSources.Add(new ReportDataSource("DataSetTransmittalLO", dtPO));
            report.SetParameters(new[] { new ReportParameter("prmBANo", bano) });
            report.SetParameters(new[] { new ReportParameter("prmCompany", companydesc) });
            report.SetParameters(new[] { new ReportParameter("prmCompanyAddress", " ") });
            report.SetParameters(new[] { new ReportParameter("prmPayeeName", _tblVendor.PayeeName) });
            report.SetParameters(new[] { new ReportParameter("prmMOP", "CHEQUE") });
            report.SetParameters(new[] { new ReportParameter("prmTerms", "30 Days") });
            report.SetParameters(new[] { new ReportParameter("prmPayClass", "LOCAL OUTSOURCED CASKETS") });
            // report.SetParameters(new[] { new ReportParameter("prmVendor", "SPASv1") });
            report.SetParameters(new[] { new ReportParameter("prmPODate", DateTime.Now.ToString("MM/dd/yyyy")) });

            report.SetParameters(new[] { new ReportParameter("prmTIN", _TblVendorTIN.TIN) });

            if (_tblVendor.isVat == true)
            {
                report.SetParameters(new[] { new ReportParameter("prmTaxType", "VAT") });
            }
            else
            {
                report.SetParameters(new[] { new ReportParameter("prmTaxType", "NON-VAT") });
            }
            report.SetParameters(new[] { new ReportParameter("prmVendorName", _tblVendor.VendorName) });


            //report.SetParameters(new[] { new ReportParameter("prmDeduction", 0.ToString()) });

            //qryPOSignatories _qryPOSignatories = new qryPOSignatories();

            //qryRptPurchaseOrderHdr _qryRptPurchaseOrderHdr = await _RepositoryUnit.PRAuthorizationRepository.GetRptPOHdr(bano);


            List<qrySignatoriesChapelAdvisory> _qrySignatoriesChapelAdvisory = new List<qrySignatoriesChapelAdvisory>();


            _qrySignatoriesChapelAdvisory = await _RepositoryUnit.PRAuthorizationRepository.GetSignatoriesChapelAdvisory(bano, "12345");


            string _preparedby = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 0).Select(grp => grp.PersonName)
                   .FirstOrDefault();
            string _preparedbyposition = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 0).Select(grp => grp.PositionDesc)
                   .FirstOrDefault();

            string _reviewedby = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 1).Select(grp => grp.PersonName)
                    .FirstOrDefault();
            string _reviewedbyposition = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 1).Select(grp => grp.PositionDesc)
                   .FirstOrDefault();

            string _approver1 = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 2).Select(grp => grp.PersonName)
                   .FirstOrDefault();
            string _approver1byposition = _qrySignatoriesChapelAdvisory.Where(x => x.Authorizelevel == 2).Select(grp => grp.PositionDesc)
                   .FirstOrDefault();

            report.SetParameters(new[] { new ReportParameter("prmPreparedByName", _preparedby) });
            report.SetParameters(new[] { new ReportParameter("prmPreparedByPosition", _preparedbyposition) });

            report.SetParameters(new[] { new ReportParameter("prmReviewedByName", _reviewedby) });
            report.SetParameters(new[] { new ReportParameter("prmReviewedByPosition", _reviewedbyposition) });

            report.SetParameters(new[] { new ReportParameter("prmApprovedByName", _approver1) });
            report.SetParameters(new[] { new ReportParameter("prmApprovedByPosition", _approver1byposition) });

            int ext = (int)(DateTime.Now.Ticks >> 10);
            //byte[] pdf = report.Render("PDF");

            string format1 = "PDF";
            string ext1 = "pdf";
            string mimetype1 = "application/pdf";

            try
            {
                var pdf = report.Render(format1);
                var file = File(pdf, mimetype1, "report." + ext1);


                string filePath = Path.Combine(_ReportFilespath, "SPASv1 Transmittal", "Local Outsourced Casket", DateTime.Now.ToString("MM-dd-yyyy"));

                string filePathRequisition = Path.Combine(_ReportFilespath, "Requisition", DateTime.Now.ToString("MM-dd-yyyy"));

                if (!Directory.Exists(Path.Combine(filePath)))
                {
                    Directory.CreateDirectory(filePath);
                }
                if (!Directory.Exists(Path.Combine(filePathRequisition)))
                {
                    Directory.CreateDirectory(filePathRequisition);
                }

                // Specify the path where you want to save the PDF file
                //string filePath = @"\\SPLPDEVSERVER\Spasv2$\Reports\ChapelAdvisory\" + PONo + ".pdf";
                filePath = Path.Combine(filePath, bano + "-" + CompanyCode + VendorCode + ".pdf");
                filePathRequisition = Path.Combine(filePathRequisition, bano + "-" + CompanyCode + VendorCode + ".pdf");

                //filePath = "\\\\192.168.23.25:88\\SPASv2\\wwwroot\\Reports\\ChapelAdvisory\\" + PONo + ".pdf";

                // Access the actual file content from the FileContentResult
                byte[] fileContent = file.FileContents;

                // Use FileStream to write the PDF content to the file
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(fileContent, 0, fileContent.Length);
                    // No need to call fs.Dispose(), the using statement takes care of it
                }

                using (FileStream fs = new FileStream(filePathRequisition, FileMode.Create, FileAccess.Write))
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



        //public async Task<IActionResult> EmailAdvisory(string emailtype,string bano,string chapelcode ,string gcmname,IList<string> listprno,OSPParams oSPParams)
        //{
        //    var vlist = oSPParams.TblRecipientList;
        //    TblSendEmail _tblsendemail = new TblSendEmail();
        //    string EmailTo;
        //    _tblsendemail.ReferenceNo = DateTime.Now.ToString("MM/dd/yyyy hhmmss");
        //    _tblsendemail.SystemCode = "SPASv2";
        //    _tblsendemail.From = "noreplynotification@stpeter.com.ph";
        //    _tblsendemail.To = "ronom@stpeter.com.ph";

        //    string POReport;
        //    IList<string> strList = new List<string>();

        //    IList<string> CCs = new List<string>();
        //    IList<string> _bcc = new List<string>();

        //    //string requestAddress = OSPUrlRepo + "/CommonRepository/GetRecipient";
        //    //var query = new Dictionary<string, string>()
        //    //{
        //    //    ["systemcode"] = "SPASv2",

        //    //};
        //    //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
        //    //IList<TblRecipient> vlist = await UtilitiesHttpClient<TblRecipient>.GetJsonlist(requestAddress);

        //    //RECEIPTIENT
        //    switch (emailtype)
        //    {



        //        case "ChapelAdvisoryForCM":
        //        case "ChapelAdvisoryForGCM":
        //            var result1 = vlist.Where(x => x.ReportName == "CasketOrder" && x.EmailType == "CC").ToList();
        //            foreach (var x in result1)
        //            {
        //                CCs.Add(x.Email);
        //            }
        //            result1 = vlist.Where(x => x.ReportName == "CasketOrder" && x.EmailType == "BCC").ToList();
        //            foreach (var x in result1)
        //            {
        //                _bcc.Add(x.Email);
        //            }

        //            result1 = vlist.Where(x => x.ReportName == "CasketOrder" && x.EmailType == "TO").ToList();
        //            _tblsendemail.To = vlist.Where(x => x.ReportName == "CasketOrder" && x.EmailType == "TO").Select(x => x.Email).FirstOrDefault();
        //            break;
        //        case "VendorPO":
        //            var result2 = vlist
        //           .Where(x => x.ReportName == "PurchaseOrder" && x.EmailType == "CC")
        //           .ToList();
        //            foreach (var x in result2)
        //            { 
        //                CCs.Add(x.Email);
        //            }
        //            result2 = vlist.Where(x => x.ReportName == "PurchaseOrder" && x.EmailType == "BCC").ToList();
        //            foreach (var x in result2)
        //            {
        //                _bcc.Add(x.Email);
        //            }
        //            _tblsendemail.To = vlist.Where(x => x.ReportName == "PurchaseOrder" && x.EmailType == "TO").Select(x=>x.Email).FirstOrDefault();
        //            break;
        //        default:
        //            break;
        //    }

        //    switch (emailtype)
        //    {

        //        case "NextAuth_Single":
        //             EmailTo = string.Empty;
        //            qryEmployee _qryEmployee = new qryEmployee();
        //            _tblsendemail.ReferenceNo = DateTime.Now.ToString("MM/dd/yyyy hhmmss");
        //            _tblsendemail.SystemCode = "SPASv2";
        //            _tblsendemail.From = "ronom@stpeter.com.ph";


        //            var requestAddress = OSPUrlRepo + "/CommonRepository/GetTblEmployee";
        //            var query = new Dictionary<string, string>()
        //            {
        //                ["personid"] = bano,

        //            };

        //            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);//requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
        //            _qryEmployee = await UtilitiesHttpClient<qryEmployee>.GetJsonlist1(requestAddress);


        //            _tblsendemail.To = _qryEmployee.Emailaddress;

        //            _tblsendemail.Subject = "Request Authorization";
        //            if (chapelcode == "APPROVER")
        //            {
        //                _tblsendemail.Subject = "SPASv2 For Approval - " + DateTime.Now.ToString();
        //            }
        //            else
        //            {
        //                _tblsendemail.Subject = "SPASv2 For Verification  - " + DateTime.Now.ToString();
        //            }

        //            _tblsendemail.Body = await this.BodyEMAIL_Authorizationv2(listprno, bano, chapelcode, listprno.Count());

        //            break;
        //        case "NextAuth_Batch":
        //             EmailTo = string.Empty;
        //             _qryEmployee = new qryEmployee();
        //            IList<string> batchprnolist = new List<string>();


        //            _tblsendemail.ReferenceNo = DateTime.Now.ToString("MM/dd/yyyy hhmmss");
        //            _tblsendemail.SystemCode = "SPASv2";
        //            _tblsendemail.From = "ronom@stpeter.com.ph";


        //            requestAddress = OSPUrlRepo + "/CommonRepository/GetTblEmployee";
        //                query = new Dictionary<string, string>()
        //            {
        //                ["personid"] = bano,

        //            };


        //            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);//requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
        //            _qryEmployee = await UtilitiesHttpClient<qryEmployee>.GetJsonlist1(requestAddress);


        //            if (_qryEmployee == null)
        //            {
        //                IList<qryGroupEmails> _Emails = await _RepositoryUnit.PRAuthorizationRepository.GetEmailsByGroupId(bano);


        //                _tblsendemail.Subject = "Request Authorization";
        //                foreach (var item in listprno.Distinct())
        //                {
        //                    string batchPRNo = await _RepositoryUnit.PRAuthorizationRepository.GetBatchNoByPRNo(item);
        //                    batchprnolist.Add(batchPRNo);
        //                }

        //                foreach (var item in _Emails)
        //                {
        //                    if (chapelcode == "APPROVER")
        //                    {
        //                        _tblsendemail.Subject = "SPASv2 For Approval - " + DateTime.Now.ToString();
        //                    }
        //                    else
        //                    {
        //                        _tblsendemail.Subject = "SPASv2 For Verification  - " + DateTime.Now.ToString();
        //                    }
        //                    _tblsendemail.To = item.Emails;
        //                    _tblsendemail.Body = await this.BodyEMAIL_Authorization_Batchv2(listprno, item.PersonId, chapelcode, listprno.Count(), batchprnolist);
        //                    _tblsendemail.CCemails = CCs;
        //                    _tblsendemail.BCemails = _bcc;
        //                    await this.SendEmail(_tblsendemail);


        //                }
        //                return Ok();
        //            }




        //            _tblsendemail.To = _qryEmployee.Emailaddress;

        //            _tblsendemail.Subject = "Request Authorization";
        //            if (chapelcode == "APPROVER")
        //            {
        //                _tblsendemail.Subject = "SPASv2 For Approval - " + DateTime.Now.ToString();
        //            }
        //            else
        //            {
        //                _tblsendemail.Subject = "SPASv2 For Verification  - " + DateTime.Now.ToString();
        //            }

        //            foreach (var item in listprno.Distinct())
        //            {
        //                string batchPRNo = await _RepositoryUnit.PRAuthorizationRepository.GetBatchNoByPRNo(item);
        //                batchprnolist.Add(batchPRNo);
        //            }
        //            _tblsendemail.Body = await this.BodyEMAIL_Authorization_Batchv2(listprno, bano,chapelcode, listprno.Count(), batchprnolist);
        //            break;


        //        case "ChapelAdvisoryForCM":
        //            _tblsendemail.Subject = "SPASv2 Advisory - Casket Order - "+ chapelcode + " " + DateTime.Now.ToString();
        //            strList = new List<string>();
        //            POReport = Path.Combine(_ReportFilespath, "Reports", "ChapelAdvisory", bano +"-"+ chapelcode + ".pdf");
        //            strList.Add(POReport);
        //            _tblsendemail.Body = await this.BodyEMAIL_CM_ChapelAdvisory();
        //            _tblsendemail.Attachment = strList;


        //            requestAddress = OSPUrlRepo + "/CommonRepository/GetEmailPerChapel";
        //            query = new Dictionary<string, string>()
        //            {
        //                ["chapelcode"] = chapelcode,

        //            };
        //            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);//requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
        //            RefChapelEmail _refChapelEmail  = await UtilitiesHttpClient<RefChapelEmail>.GetJsonlist1(requestAddress);
        //            _tblsendemail.To = _refChapelEmail.Email;



        //            break;
        //        case "ChapelAdvisoryForGCM":
        //            _tblsendemail.Subject = "SPASv2 Advisory - Casket Order (GCM "+ gcmname + ") " + DateTime.Now.ToString();
        //            strList = new List<string>();
        //            POReport = Path.Combine(_ReportFilespath, "Reports", "ChapelAdvisory", bano +"-"+ chapelcode+ ".pdf");
        //            strList.Add(POReport);
        //            _tblsendemail.Body = await this.BodyEMAIL_GCM_ChapelAdvisory();
        //            _tblsendemail.Attachment = strList;
        //            break;

        //        case "VendorPO":
        //            _tblsendemail.Subject = "SPASv2 Advisory - Purchase Order - "+ bano + " " + DateTime.Now.ToString();
        //            strList = new List<string>();
        //            POReport = Path.Combine(_ReportFilespath, "Reports", "POReport", bano + "-" + chapelcode + ".pdf");
        //            strList.Add(POReport);
        //            _tblsendemail.Body = await this.BodyEMAIL_Vendor_PO(chapelcode,bano);
        //            _tblsendemail.Attachment = strList;
        //            break;
        //        default:
        //            break;
        //    }


        //    _tblsendemail.CCemails = CCs;
        //    _tblsendemail.BCemails = _bcc;


        //    await this.SendEmail(_tblsendemail);

        //    return Ok();



        //}

        [HttpPost("EmailAdvisory")]
        public async Task<TblResponse> EmailAdvisory(string emailtype, string bano, string chapelcode, string gcmname, OSPParams oSPParams)
        {
            _response = new TblResponse();

            try
            {
                IList<string> listprno = new List<string>();
                listprno = oSPParams.listNo;


                var vlist = oSPParams.TblRecipientList;
                TblSendEmail _tblsendemail = new TblSendEmail();
                string EmailTo;
                _tblsendemail.ReferenceNo = DateTime.Now.ToString("MM/dd/yyyy hhmmss");
                _tblsendemail.SystemCode = "SPASv2";
                _tblsendemail.From = "noreplynotification@stpeter.com.ph";
                _tblsendemail.To = "ronom@stpeter.com.ph";

                string POReport;
                IList<string> strList = new List<string>();

                IList<string> CCs = new List<string>();
                IList<string> _bcc = new List<string>();

                //string requestAddress = OSPUrlRepo + "/CommonRepository/GetRecipient";
                //var query = new Dictionary<string, string>()
                //{
                //    ["systemcode"] = "SPASv2",

                //};
                //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                //IList<TblRecipient> vlist = await UtilitiesHttpClient<TblRecipient>.GetJsonlist(requestAddress);

                //RECEIPTIENT
                switch (emailtype)
                {

                    case "ChapelAdvisoryForCM":
                    case "ChapelAdvisoryForGCM":
                        var result1 = vlist.Where(x => x.ReportName == "CasketOrder" && x.EmailType == "CC").ToList();
                        foreach (var x in result1)
                        {
                            CCs.Add(x.Email);
                        }
                        result1 = vlist.Where(x => x.ReportName == "CasketOrder" && x.EmailType == "BCC").ToList();
                        foreach (var x in result1)
                        {
                            _bcc.Add(x.Email);
                        }

                        result1 = vlist.Where(x => x.ReportName == "CasketOrder" && x.EmailType == "TO").ToList();
                        _tblsendemail.To = vlist.Where(x => x.ReportName == "CasketOrder" && x.EmailType == "TO").Select(x => x.Email).FirstOrDefault();
                        break;
                    case "VendorPO":
                        var result2 = vlist
                       .Where(x => x.ReportName == "PurchaseOrder" && x.EmailType == "CC")
                       .ToList();
                        foreach (var x in result2)
                        {
                            CCs.Add(x.Email);
                        }
                        result2 = vlist.Where(x => x.ReportName == "PurchaseOrder" && x.EmailType == "BCC").ToList();
                        //_bcc.AddRange(vlist.Select(a=> a.Email).Where(x => x.ReportName == "PurchaseOrder" && x.EmailType == "BCC").ToList());
                        foreach (var x in result2)
                        {
                            _bcc.Add(x.Email);
                        }

                        _tblsendemail.To = vlist.Where(x => x.ReportName == "PurchaseOrder" && x.EmailType == "TO").Select(x => x.Email).FirstOrDefault();
                        break;
                    default:
                        break;
                }
                qryEmployee _qryEmployee;
                switch (emailtype)
                {

                    case "NextAuth_Single":
                        var _prnolist = new List<string>();
                        foreach (var item in oSPParams.PersonIdList)
                        {
                            var _positioncode = await GetPositionCode(item);
                            var prlist = await _RepositoryUnit.PRAuthorizationRepository.GetPRAuthorizationLists(item);
                            _prnolist.Clear();
                            foreach (string prno in prlist.Select(i => i.Reqno).Distinct())
                            {
                                _prnolist.Add(prno);
                                var authpayclass = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizeClass(prno, item);
                            }
                            EmailTo = string.Empty;
                            _qryEmployee = new qryEmployee();
                            _tblsendemail.ReferenceNo = DateTime.Now.ToString("MM/dd/yyyy hhmmss");
                            _tblsendemail.SystemCode = "SPASv2";
                            _tblsendemail.From = "noreplynotification@stpeter.com.ph";

                            _qryEmployee = oSPParams.qryEmployeeList.Where(a => a.PersonID == bano).FirstOrDefault();

                            _tblsendemail.To = _qryEmployee.Emailaddress;

                            _tblsendemail.Subject = "Request Authorization";
                            if (chapelcode == "APPROVER")
                            {
                                _tblsendemail.Subject = "ePPS Advisory For Approval - " + DateTime.Now.ToString();
                            }
                            else
                            {
                                _tblsendemail.Subject = "ePPS Advisory For Verification  - " + DateTime.Now.ToString();
                            }

                        }

                        break;
                    case "NextAuth_Batch":
                        EmailTo = string.Empty;
                        _qryEmployee = new qryEmployee();
                        IList<string> batchprnolist = new List<string>();


                        _tblsendemail.ReferenceNo = DateTime.Now.ToString("MM/dd/yyyy hhmmss");
                        _tblsendemail.SystemCode = "SPASv2";
                        _tblsendemail.From = "noreplynotification@stpeter.com.ph";


                        var requestAddress = OSPUrlRepo + "/CommonRepository/GetTblEmployee";
                        var query = new Dictionary<string, string>()
                        {
                            ["personid"] = bano,

                        };

                        requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);//requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                        _qryEmployee = await UtilitiesHttpClient<qryEmployee>.GetJsonlist1(requestAddress);

                        if (_qryEmployee == null)
                        {
                            IList<qryGroupEmails> _Emails = await _RepositoryUnit.PRAuthorizationRepository.GetEmailsByGroupId(bano);


                            _tblsendemail.Subject = "Request Authorization";
                            foreach (var item in listprno.Distinct())
                            {
                                string batchPRNo = await _RepositoryUnit.PRAuthorizationRepository.GetBatchNoByPRNo(item);
                                batchprnolist.Add(batchPRNo);
                            }

                            foreach (var item in _Emails)
                            {
                                if (chapelcode == "APPROVER")
                                {
                                    _tblsendemail.Subject = "ePPS Advisory For Approval - " + DateTime.Now.ToString();
                                }
                                else
                                {
                                    _tblsendemail.Subject = "ePPS Advisory For Verification  - " + DateTime.Now.ToString();
                                }
                                _tblsendemail.To = item.Emails;
                                _tblsendemail.Body = await this.BodyEMAIL_Authorization_Batchv2(listprno, item.PersonId, chapelcode, listprno.Count(), batchprnolist);
                                _tblsendemail.CCemails = CCs;
                                _tblsendemail.BCemails = _bcc;


                                _response = await this.SendEmail(_tblsendemail);


                            }
                            _response.Status = "SUCCESS";
                            _response.ErrorMessage = "EMAIL SENT";

                            return _response;
                        }




                        _tblsendemail.To = _qryEmployee.Emailaddress;

                        _tblsendemail.Subject = "Request Authorization";
                        if (chapelcode == "APPROVER")
                        {
                            _tblsendemail.Subject = "ePPS Advisory For Approval - " + DateTime.Now.ToString();
                        }
                        else
                        {
                            _tblsendemail.Subject = "ePPS Advisory Verification  - " + DateTime.Now.ToString();
                        }

                        foreach (var item in listprno.Distinct())
                        {
                            string batchPRNo = await _RepositoryUnit.PRAuthorizationRepository.GetBatchNoByPRNo(item);
                            batchprnolist.Add(batchPRNo);
                        }
                        _tblsendemail.Body = await this.BodyEMAIL_Authorization_Batchv2(listprno, bano, chapelcode, listprno.Count(), batchprnolist);
                        break;


                    case "ChapelAdvisoryForCM":
                        _tblsendemail.Subject = "ePPS Advisory - Casket Order - " + chapelcode + " " + DateTime.Now.ToString();
                        strList = new List<string>();
                        POReport = Path.Combine(_ReportFilespath, "Reports", "ChapelAdvisory", bano + "-" + chapelcode + ".pdf");
                        strList.Add(POReport);
                        _tblsendemail.Body = await this.BodyEMAIL_CM_ChapelAdvisory();
                        _tblsendemail.Attachment = strList;


                        requestAddress = OSPUrlRepo + "/CommonRepository/GetEmailPerChapel";
                        query = new Dictionary<string, string>()
                        {
                            ["chapelcode"] = chapelcode,

                        };
                        requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);//requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                        RefChapelEmail _refChapelEmail = await UtilitiesHttpClient<RefChapelEmail>.GetJsonlist1(requestAddress);
                        _tblsendemail.To = _refChapelEmail.Email;
                        //_tblsendemail.To = "ronom@stpeter.com.ph";


                        break;
                    case "ChapelAdvisoryForGCM":
                        _tblsendemail.Subject = "ePPS Advisory - Casket Order (GCM " + gcmname + ") " + DateTime.Now.ToString();
                        strList = new List<string>();
                        POReport = Path.Combine(_ReportFilespath, "Reports", "ChapelAdvisory", bano + "-" + chapelcode + ".pdf");
                        strList.Add(POReport);
                        _tblsendemail.Body = await this.BodyEMAIL_GCM_ChapelAdvisory();
                        qryChapelBranchDetails _qryChapelBranchDetails = await _RepositoryUnit.RefRegionRepository.GetChapelDetailsbygcmname(chapelcode);
                        _tblsendemail.To = _qryChapelBranchDetails.Email;
                        _tblsendemail.Attachment = strList;
                        break;

                    case "VendorPO":


                        _tblsendemail.Subject = "ePPS Advisory - Purchase Order - " + bano + " " + DateTime.Now.ToString();
                        strList = new List<string>();
                        POReport = Path.Combine(_ReportFilespath, "Reports", "POReport", bano + "-" + chapelcode + ".pdf");
                        strList.Add(POReport);

                        //palagyan pagdevelopment RUDYBOY
                        POReport = Path.Combine(_ReportFilespath, "CIS PO", bano);

                        if (System.IO.Directory.Exists(POReport))
                        {
                            string[] fileEntries = Directory.GetFiles(POReport, "*", SearchOption.AllDirectories);

                            strList.AddRange(fileEntries);
                        }

                        _tblsendemail.Body = await this.BodyEMAIL_Vendor_PO(chapelcode, bano);
                        IList<qryVendorContact> qryVendorContact = await _RepositoryUnit.TblVendorContactPersonRepository.GetVendorContactEMAILByName(chapelcode);

                        //if (!env.IsDevelopment())
                        //{
                            foreach (var item in qryVendorContact)
                            {
                                //_tblsendemail.To = string.Join(";", _tblsendemail.To, item.ContactDetails);
                                _tblsendemail.To = item.ContactDetails;
                            }
                        //}


                        _tblsendemail.Attachment = strList;
                        break;


                    default:
                        break;
                }


                _tblsendemail.CCemails = CCs;
                _tblsendemail.BCemails = _bcc;


                await Task.Run(async () =>
                {
                    _response = await this.SendEmail(_tblsendemail);
                });
                //if (_response.Status == "SUCCESS")
                //{
                //    _response.Status = "SUCCESS";
                //    _response.ErrorMessage = "EMAIL SENT";
                //}




                return _response;
            }
            catch (Exception ex)
            {
                _response.Status = "FAILED";
                _response.ErrorMessage = ex.Message + "EMAIL FAILED";
                return _response;
            }

            //this.SendEmailAuthorization(_tblsendemail, OSPUrlService);


        }

        private async Task<string> BodyEMAIL_Authorizationv2(IList<string> listprno, string personid, string AuthorizeClass, int cntPR)
        {

            string Name = await _RepositoryUnit.PRAuthorizationRepository.GetNameofAuthorizer(personid);
            string Gender = await _RepositoryUnit.PRAuthorizationRepository.GetGenderByPersonID(personid);
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

                RequestDate = await _RepositoryUnit.PRAuthorizationRepository.GetRequestDate(item);


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

            string Name = await _RepositoryUnit.PRAuthorizationRepository.GetNameofEmployee(personid);
            string Gender = await _RepositoryUnit.PRAuthorizationRepository.GetGenderByPersonID(personid);
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

                RequestDate = await _RepositoryUnit.PRAuthorizationRepository.GetRequestDateByBatchNo(item);


                sb.Append(@"<td valign=""middle"" width=""80%"" style=""text-align:left; padding: 0 2.5em;"" class=""esd-text"">
                            <h3 style=""text-align: center;"">" + RequestDate + @"<span style=""font-size:13px;""></span></h3>
                        </td>
                    </tr>");

            }


            sb.Append(@"<tr>
                        <td valign=""middle"" style=""text-align:left; padding: 1em 2.5em;"" class=""esd-text"" align=""left"" esd-links-underline=""none"">
                            <p><a href=""" + BaseUI + @"/Authorization/BatchPRAuthorization"" class=""btn btn-primary"" style=""text-decoration: none;"" target=""_blank"">Click here to " + classword + @"</a></p>
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

        [HttpPost("BodyEMAIL_CM_ChapelAdvisory")]
        private async Task<string> BodyEMAIL_CM_ChapelAdvisory()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div>Dear CM,  </div>");

            sb.Append("<div class = 'clearfix'></div><br>");

            sb.Append("<div> Your order has been placed with the suppliers, and delivery is anticipated on or before the next cut-off.  ");

            sb.Append("<div class = 'clearfix'></div><br>");

            sb.Append("<div> If we do not receive a response to this email within two (2) days, it will be assumed that you approve the ");
            sb.Append("<div> delivery without any holds or cancellations on your part. Should you have any modifications or specific  ");
            sb.Append("<div> instructions, kindly inform us promptly. ");

            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div> Reminders: ");
            sb.Append("<div> 1. Upon receipt of the casket/s, kindly send a confirmation email to owensd@stpeter.com.ph, aprilnb@stpeter.com.ph, and markae@stpeter.com.ph. This immediate notification is essential for the timely release of payment to the supplier.  ");
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
            sb.Append("<div> Thank you.");
            sb.Append("<br><div class = 'clearfix'></div>");
            sb.Append("<div> LOGISTICS.");
            sb.Append("<br><div class = 'clearfix'></div>");
            sb.Append("<div> ** This is a system generated email. Please do not reply**");

            return sb.ToString();
        }
        [HttpPost("BodyEMAIL_GCM_ChapelAdvisory")]
        private async Task<string> BodyEMAIL_GCM_ChapelAdvisory()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div>Dear GCM,  </div>");

            sb.Append("<div class = 'clearfix'></div><br>");

            sb.Append("<div>The casket orders under your region have already been ordered to the respective outsource vendors below. ");
            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div> Delivery shall be on or before 15 days from the receipt of order by the vendor/s. ");
            sb.Append("<div class = 'clearfix'></div><br>");

            sb.Append("<div> Reminders: ");
            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div> 1. Ensure that chapel managers practice the standard procedure in receiving the caskets (physical inspection and proper documentation).  ");
            sb.Append("<div> 2. Establish proper controls to regularly monitor caskets ordered and delivered. ");
            sb.Append("<div> 3. Group chapel manager (GCM) is jointly responsible in case of loss of caskets or damaged caskets. ");
            sb.Append("<div> 4. For queries, please email owensd@stpeter.com.ph, aprilnb@stpeter.com.ph, and markae@stpeter.com.ph. ");

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

        [HttpPost("BodyEMAIL_Vendor_PO")]
        private async Task<string> BodyEMAIL_Vendor_PO(string VendorName, string PONo)
        {
            try
            {
                await Task.Yield();
                StringBuilder sb = new StringBuilder();
                sb.Append("<div>To: " + VendorName + ",  </div>");

                sb.Append("<div class = 'clearfix'></div><br>");

                sb.Append("<div>Dear Sir/Ma'am,  </div>");

                sb.Append("<div class = 'clearfix'></div><br>");
                sb.Append("<div class = 'clearfix'></div><br>");
                sb.Append("<div class = 'clearfix'></div><br>");


                sb.Append("<div>Please see the attached purchase order/s " + PONo + " to your company, and observe the following guidelines:  </div>");
                sb.Append("<div class = 'clearfix'></div><br>");
                sb.Append("<div class = 'clearfix'></div><br>");
                sb.Append("<div>1. For any questions or clarifications on the Purchase Order/s, please inform us via email/viber.  </div>");
                sb.Append("<div>2. Caskets must be delivered within 15 days from the receipt of the Purchase Order/s.  </div>");
                sb.Append("<div>3. Any casket delivery without any prior approval or Purchase Order will not be accepted or paid.  </div>");
                sb.Append("<div>4. Chapels are not allowed to make direct orders to vendors/suppliers. If the chapels ask for additional orders of caskets, please inform us via email or viber.  </div>");
                sb.Append("<div>5. All caskets must be in good condition with complete accessories and the deliveries must be in the correct quantity, casket types and sizes.  </div>");
                sb.Append("<div>6. Please inform us of each delivery completion via email or viber.  </div>");
                sb.Append("<div>7. Submit your invoice and delivery receipts promptly after each delivery to the Head Office or the casket factory to expedite payment processing.  </div>");
                sb.Append("<div>8. Sales invoices and delivery receipts must not contain any erasures or alterations.   </div>");


                sb.Append("<div class = 'clearfix'></div><br>");


                sb.Append("<div class = 'clearfix'></div><br>");
                sb.Append("<div class = 'clearfix'></div><br>");


                sb.Append("<style type='text/css'>");
                sb.Append("table { border-collapse:collapse; }");
                sb.Append("table,th, td {border: 1px solid black;padding:5px;}");
                sb.Append(".clearfix:after { visibility: hidden; display: block; font-size: 0; content: ' '; clear: both; height: 0; }");
                sb.Append(".clearfix { display: inline-block; }");
                sb.Append(".clearfix { display: block; zoom: 1; ");
                sb.Append("</style>");
                sb.Append("<div> Thank you.");
                sb.Append("<br><div class = 'clearfix'></div>");
                sb.Append("<div> St. Peter Casket – Head Office.");


                sb.Append("<br><div class = 'clearfix'></div>");
                sb.Append("<div> ** This is a system generated email. Please do not reply**");
                return sb.ToString();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        [HttpPost("SendEmail")]
        public async Task<TblResponse> SendEmail(TblSendEmail _TblSendEmail)
        {
            try
            {

                if (DevelopmentType == "TEST")
                {
                    _TblSendEmail.From = "noreplynotification@stpeter.com.ph";
                    _TblSendEmail.To = "ronom@stpeter.com.ph";
                    _TblSendEmail.Subject = " TEST " + _TblSendEmail.Subject + " TEST ";
                    if (_TblSendEmail.CCemails != null)
                    {
                        _TblSendEmail.CCemails.Clear();
                    }

                    IList<string> CCs = new List<string>();
                    //CCs.Add("ronom@stpeter.com.ph");
                    //CCs.Add("davidga@stpeter.com.ph");
                    CCs.Add("warrenlb@stpeter.com.ph");
                    CCs.Add("rudyab@stpeter.com.ph");
                    CCs.Add("jonab@stpeter.com.ph");
                    _TblSendEmail.CCemails = CCs;
                }


                _TblSendEmail.Host = "smtp-relay.gmail.com";
                _TblSendEmail.Port = "587";
                _TblSendEmail.Username = null;
                _TblSendEmail.Password = null;



                for (int i = 0; i < 5; i++)
                {
                    string requestAddress2 = OSPUrlService + "/SendEmail/SendEmail";
                    _response = await UtilitiesHttpClient<TblSendEmail>.PostAsync(_TblSendEmail, requestAddress2);

                    if (_response.Status == "SUCCESS")
                    {
                        break;
                    }
                }

                return _response;

            }
            catch (Exception ex)
            {
                _response.Status = "FAILED";
                _response.ErrorMessage = ex.Message + "EMAIL FAILED";
                return _response;
            }

        }
        //[HttpGet("SendEmailAuthorization_PRNO_Scheduled")]
        //public async Task<IActionResult> SendEmailAuthorization_PRNO_Scheduled()
        //{
        //    try
        //    {
        //        IList<TblPaymentRequestAuth> _TblPaymentRequestAuth = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel();
        //        IList<string> _personidlist = new List<string>();
        //        IList<string> _prnolist = new List<string>();



        //        return vlist;
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
        //        logger.LogError(ex, error);
        //        return null;

        //    }
        //}

    }
}
