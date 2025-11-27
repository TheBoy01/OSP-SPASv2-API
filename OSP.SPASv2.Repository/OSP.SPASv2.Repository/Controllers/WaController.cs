using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using OSP.Common.Domain.View;
using OSP.SPASv2.Domain.Params;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Domain.View;
using OSP.SPASv2.Repository.Parameters;
using OSP.SPASv2.Repository.Repository.MainRepository;
using OSP.SPASv2.Repository.Rules;
using OSP.SPASv2.Repository.Utility;
using OSP.SPASv2.Web.APIServices.Services;
using OSP.SPASv2.Web.Areas.Identity.Data;
using OSP.SPASv2.Web.Controllers;
using SPASv2.Context;
using System;
using System.Formats.Asn1;
using System.Net;
using System.Security.Policy;
using Microsoft.Extensions.Hosting;
using SPASv2.Models;
using System.Diagnostics;
using System.Data.Entity.Core.Metadata.Edm;
using System.Text.Json;
using static System.Data.Odbc.ODBC32;
using System.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DocumentFormat.OpenXml.Office2016.Excel;
using Newtonsoft.Json;
using AspNetCore;

namespace OSP.SPASv2.Repository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaController : ControllerBase
    {
        private readonly JWTAuthenticationManager jwtAuthenticationManager;

        RepositoryUnit _RepositoryUnit;
        ServiceUnit _ServiceUnit;
        private ILogger<WaController> logger;
        private readonly ILogger<AuthorizationController> _AuthLogger;
        private readonly UserManager<OSPSPASv2ApplicationUser> _userManager;
        private IConfiguration _configuration;
        private IHostEnvironment _env;

        private SPASv2Context context;
        //TblResponse _response = new TblResponse();
        TblResponse _response;
        string _validmessage;
        PaymentRequestParams _PaymentRequestParams = new PaymentRequestParams();
        PaymentRequestRules _PRrules; 
        string BaseUrlRepo;
        string BaseUrlService;
        string OSPUrlRepo;
        string OSPUrlService;
        RonController _ronController;
        //PaymentRequestRules _PRrules;

        public WaController(ILogger<WaController> _logger, SPASv2Context _context, JWTAuthenticationManager _jwt,
            IConfiguration configuration, IHostEnvironment env, RonController ronController)
        {
            logger = _logger;
            this.context = _context;
            _RepositoryUnit = new RepositoryUnit(_context);
            this.jwtAuthenticationManager = _jwt;
            _PRrules = new PaymentRequestRules(_context);
            _configuration = configuration;
            BaseUrlRepo = _configuration.GetSection("APIBaseURL")["SPASv2.Repository"];
            BaseUrlService = _configuration.GetSection("APIBaseURL")["SPASv2.Service"];
            OSPUrlRepo = _configuration.GetSection("APIBaseURLCommon")["Common.Repository"];
            OSPUrlService = _configuration.GetSection("APIBaseURLCommon")["Common.Service"];
            _env = env;
            //_PRrules = new PaymentRequestRules(_context);
            _ronController = ronController;
        }

        [HttpGet("GetVendorItems")]
        public async Task<IActionResult> GetVendorItems(string vendorcode, string paymentclasscode)
        {
            try
            {
                // logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");


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
                //  logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");


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

        [HttpGet("GettmpPaymentRequestInventory")]
        public async Task<IActionResult> GettmpPaymentRequestInventory()
        {
            try
            {
                // logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");


                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.tmpPaymentRequestInventoryRepository.GettmpPaymentRequestInventory();

                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }

        }

        [HttpGet("GettmpPaymentRequestInventoryA")]
        public async Task<IActionResult> GettmpPaymentRequestInventoryA(string audituser, string prno)
        {
            try
            {

                // logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");


                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.tmpPaymentRequestInventoryRepository.GettmpPaymentRequestInventoryA(audituser, prno);

                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }

        }

        [HttpPost("PosttmpPaymentRequestInventory")]
        public async Task<TblResponse> PosttmpPaymentRequestInventory(tmpPaymentRequestInventory tmp)
        {
            try
            {
                _response = new TblResponse();
                _PaymentRequestParams = new PaymentRequestParams();
                _PaymentRequestParams.tmpPaymentRequestInventory = tmp;

                logger.LogInformation("Can Create - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                //_validmessage = prrules.CanCreatetmp(_PaymentRequestParams);
                if (String.IsNullOrEmpty(_validmessage))
                {
                    logger.LogInformation("Create tmpPaymentRequestInventory - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                    await _RepositoryUnit.tmpPaymentRequestInventoryRepository.PosttmpPaymentRequestInventory(tmp);

                    logger.LogInformation("Create Response - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                    _response = await _RepositoryUnit.ResponseRepository.CreateResponse(tmp.PRNo + '_' + tmp.ItemCode, "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());
                }
                else
                {
                    _response = await _RepositoryUnit.ResponseRepository.CreateResponse(tmp.PRNo + '_' + tmp.ItemCode, "FAILED", _validmessage, Utilities.GetmethodName());
                }
                return await Task.FromResult(_response);

            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(tmp.PRNo + '_' + tmp.ItemCode, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }

        }

        [HttpPost("CreatePaymentRequestHdr")]
        public async Task<TblResponse> CreatePaymentRequestHdr(TblPaymentrequesthdr tblprhdr)
        {
            try
            {
                _response = new TblResponse();
                _PaymentRequestParams = new PaymentRequestParams();
                _PaymentRequestParams.TblPaymentrequesthdr = tblprhdr;


                logger.LogInformation("Can Create - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                //_validmessage = prrules.CanCreate(_PaymentRequestParams);
                if (String.IsNullOrEmpty(_validmessage))
                {
                    logger.LogInformation("Create tmpPaymentRequestInventory - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                    await _RepositoryUnit.PaymentrequesthdrRepository.CreatePaymentRequest(tblprhdr);

                    logger.LogInformation("Create Response - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                    _response = await _RepositoryUnit.ResponseRepository.CreateResponse(tblprhdr.PRNo, "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());
                }
                else
                {
                    _response = await _RepositoryUnit.ResponseRepository.CreateResponse(tblprhdr.PRNo, "FAILED", _validmessage, Utilities.GetmethodName());
                }
                return await Task.FromResult(_response);




            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(tblprhdr.PRNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }
        }

        [HttpGet("GetLatestPRRow")]
        public async Task<IActionResult> GetLatestPRRow(string companycode, string branchcode)
        {
            try
            {
                // logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");


                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.PaymentrequesthdrRepository.GetLatestPRRow(companycode, branchcode);
                //var str = result.PRNo;
                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }

        }

        //[HttpGet("GetLatestPRNo")]
        //public async Task<IActionResult> GetLatestPRNo(string companycode, string branchcode)
        //{
        //    try
        //    {
        //        // logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");


        //        logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
        //        var result = await _RepositoryUnit.PaymentrequesthdrRepository.GetLatestPRNo(companycode, branchcode);
        //        //var str = result.PRNo;
        //        if (result == null)
        //        { 
        //            result = "";
        //        }
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {

        //        string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
        //        logger.LogError(ex, error);
        //        return NotFound();
        //    }

        //}

        [HttpGet("GetReftrxweek")]
        public async Task<IActionResult> GetReftrxweek(DateTime auditdate)
        {
            try
            {
                // logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");


                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.ReftrxweekRepository.GetReftrxweek(auditdate);
                //var str = result.PRNo;
                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }

        }

        [HttpGet("GetRefDiscount")]
        public async Task<IActionResult> GetRefDiscount()
        {
            try
            {



                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.RefDiscountRepository.GetRefDiscount();
                //var str = result.PRNo;
                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }

        }

        [HttpGet("GetRefVat")]
        public async Task<IActionResult> GetRefVat()
        {
            try
            {



                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.VatRepository.GetRefVat();
                //var str = result.PRNo;
                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }

        }

        [HttpGet]
        [Route("GetPaymentrequesthdr")]
        public async Task<IActionResult> GetPaymentrequesthdr(string prno)
        {
            //  return Ok(await _productRepository.GetByIdAsync(productId));

            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.PaymentrequesthdrRepository.GetPaymentrequesthdr(prno);
                //var str = result.PRNo;
                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }
        }

        [HttpGet]
        [Route("ReadPaymentrequesthdr")]
        public async Task<IActionResult> ReadPaymentrequesthdr(string prno)
        {
            //  return Ok(await _productRepository.GetByIdAsync(productId));

            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.PaymentrequesthdrRepository.ReadRequestByPRNo(prno);
                //var str = result.PRNo;
                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }
        }

        [HttpPost("UpdatePaymentRequestHdr")]
        public async Task<TblResponse> UpdatePaymentRequestHdr(TblPaymentrequesthdr tblprhdr)
        {
            try
            {
                _response = new TblResponse();
                _PaymentRequestParams = new PaymentRequestParams();
                _PaymentRequestParams.TblPaymentrequesthdr = tblprhdr;

                //_validmessage = "1";
                logger.LogInformation("Can Create - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                //_validmessage = prrules.CanCreate(_PaymentRequestParams);
                if (String.IsNullOrEmpty(_validmessage))
                {
                    logger.LogInformation("Create tmpPaymentRequestInventory - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                    await _RepositoryUnit.PaymentrequesthdrRepository.UpdateTblPaymentRequestHdr(tblprhdr);

                    logger.LogInformation("Create Response - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                    _response = await _RepositoryUnit.ResponseRepository.CreateResponse(tblprhdr.PRNo, "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());
                }
                else
                {
                    _response = await _RepositoryUnit.ResponseRepository.CreateResponse(tblprhdr.PRNo, "FAILED", _validmessage, Utilities.GetmethodName());
                }
                return await Task.FromResult(_response);




            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(tblprhdr.PRNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }

        }


        [HttpPost("CanConfirm")]
        public async Task<TblResponse> CanConfirm(TblPaymentrequesthdr tblprhdr)
        {
            try
            {
                _response = new TblResponse();
                _PaymentRequestParams = new PaymentRequestParams();
                _PaymentRequestParams.TblPaymentrequesthdr = tblprhdr;
                _PaymentRequestParams.TblPaymentRequestAuth = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel(tblprhdr.PRNo);

                logger.LogInformation("Can Create - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                //_validmessage = await prrules.CanConfirm(_PaymentRequestParams);
                //if (String.IsNullOrEmpty(_validmessage))
                //{

                //    logger.LogInformation("Create Response SUCCESS - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                //    _response = await _RepositoryUnit.ResponseRepository.CreateResponse(tblprhdr.PRNo, "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());
                //}
                //else
                //{
                //    logger.LogInformation("Create Response FAILED - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                //    _response = await _RepositoryUnit.ResponseRepository.CreateResponse(tblprhdr.PRNo, "FAILED", _validmessage, Utilities.GetmethodName());
                //}
                return await Task.FromResult(_response);




            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(tblprhdr.PRNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }
        }



        [HttpPost("CreatePaymentRequestDtl")]
        public async Task<TblResponse> CreatePaymentRequestDtl(IList<TblPaymentrequestdtl> tblprdtl)
        {
            string pscode = string.Empty;
            try
            {
                _response = new TblResponse();


                for (int i = 0; i < tblprdtl.Count; i++)
                {
                    pscode = string.Empty;
                    pscode = tblprdtl[i].ProductServiceCode;

                    logger.LogInformation("Can Create - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                    _response = await _RepositoryUnit.TblPaymentRequestDtlRepository.CreatePaymentRequest(tblprdtl[i]);

                    logger.LogInformation("Create Response SUCCESS - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                    _response = await _RepositoryUnit.ResponseRepository.CreateResponse(tblprdtl[i].ProductServiceCode, "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());
                }

                return await Task.FromResult(_response);


            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(pscode, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }
        }

        [HttpPost("ReceiveRequisition")]
        public async Task<TblResponse> ReceiveRequisition(DistributionParams DistributionParams)
        {
            //string json = System.Text.Json.JsonSerializer.Serialize(DistributionParams);
            ServiceParams _ServiceParams = new ServiceParams();
            List<TblRequisitionhdr> _TblRequisitionhdrList = new List<TblRequisitionhdr>();
            List<TblRequisitiondtl> _TblRequisitiondtlList = new List<TblRequisitiondtl>();
            RequisitionParams _RequisitionParams = new RequisitionParams();

            _RequisitionParams.tblLoanhdrs = new List<TblLoanhdr>();
            _RequisitionParams.TblResponse = new TblResponse();
            _RequisitionParams.TblRequisitionDtlSummary = new List<TblRequisitionDtlSummary>();
            _RequisitionParams.TblDataSourceDtl_List = new List<TblDataSourceDtl>();
            _RequisitionParams.RefAccountMap = new List<RefAccountMap>();
            _RequisitionParams.BatchReqNo = "";
            _RequisitionParams.CompanyCode = "";
            _RequisitionParams.LastNo = "";
            _RequisitionParams.PONo = "";
            _RequisitionParams.Payclass = "";
            _RequisitionParams.ReqNo = "";
            _RequisitionParams.ServerPOPath = "";
            _RequisitionParams.UserID = "";

            List<qryRequisition> _qryRequisitionList = new List<qryRequisition>();
            qryEmployee _qryEmployee = new qryEmployee();

            string payclass = "12345";
            string companytype = "CHAPELS";
            string requestAddress = "";
            string ReqBatch = string.Empty;
            string companycode = string.Empty;

            List<qryRequisitionCasket> qryRequisitionCasket = new List<qryRequisitionCasket>();
            List<TblAssignedtoVendor_CMS> TblAssignedtoVendor_CMSList = new List<TblAssignedtoVendor_CMS>();

            try
            {
                requestAddress = OSPUrlRepo + "/CommonRepository/GetAllCompanyDetails";
                IList<qryCompanyDetails> qryCompDtlList = await UtilitiesHttpClient<qryCompanyDetails>.GetJsonlist(requestAddress);
                
                foreach (var item in DistributionParams.tmpAssignedtoVendor_CMS)
                {
                    companycode = qryCompDtlList.Where(a => a.DeptCode.Equals(item.VendorCode) && a.CompanyType.Equals("FACTORY")).Select(a => a.CompanyCode).FirstOrDefault();
                    if (string.IsNullOrEmpty(companycode))
                    {
                        companycode = item.VendorCode;
                    }

                    TblAssignedtoVendor_CMS TblAssignTable = new TblAssignedtoVendor_CMS
                    {
                        CompanyCode = companycode,
                        OrderNo = item.OrderNo,
                        VendorCode = item.VendorCode,
                        DeptCode = item.DeptCode,
                        ItemCode = item.ItemCode,
                        Quantity = item.Quantity
                    };
                    TblAssignedtoVendor_CMSList.Add(TblAssignTable);
                }
                qryRequisitionCasket = DistributionParams.qryRequisitionCasket;
                 
                requestAddress = OSPUrlRepo + "/CommonRepository/GetEmployeeDetails";
                var qryEmployee = new Dictionary<string, string>()
                {
                    ["personid"] = qryRequisitionCasket.Select(a => a.AuditUser).FirstOrDefault(),
                };
                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, qryEmployee);
                qryEmployee empdetails = await UtilitiesHttpClient<qryEmployee>.GetJsonlist1(requestAddress);


                for (int i = 0; i < qryRequisitionCasket.Count; i++)
                {
                    companycode = qryCompDtlList.Where(a => a.DeptCode.Equals(qryRequisitionCasket[i].VendorCode) && a.CompanyType.Equals("FACTORY")).Select(a => a.CompanyCode).FirstOrDefault();
                    string deptcompanycode = qryCompDtlList.Where(a => a.DeptCode.Equals(qryRequisitionCasket[i].DeptCode.ToUpper().Trim()) && a.CompanyType.Equals("CHAPELS")).Select(a => a.CompanyCode).FirstOrDefault();
                    if (string.IsNullOrEmpty(companycode))
                    {
                        companycode = qryRequisitionCasket[i].VendorCode;
                    }
                    TblVendorItems TblVendorItems = await _RepositoryUnit.VendorItemsRepository.GetVendorItemsDetails(companycode, qryRequisitionCasket[i].CasketCode);
                    payclass = TblVendorItems.PaymentClassCode;
                    qryVendorDetails qryVendorDetails = await _RepositoryUnit.VendorRepository.GetVendorDetails(companycode, payclass);
                    RefItems refitems = await _RepositoryUnit.RefItemsRepository.Read(qryRequisitionCasket[i].CasketCode);

                    qryRequisition _qryRequisition = new qryRequisition()
                    {
                        //  audit = _qryRequisitionCasket[i].AuditDate;
                        UserCompanyCode = "SPLPI", //DAPAT SA OSP ITO KUNIN.
                        UserDeptCode = "MORTUA",// DAPAT SA OSP ITO KUNIN. 
                        RequestDate = qryRequisitionCasket[i].AuditDate,
                        PayClassCode = payclass,
                        VendorCode = companycode,
                        PayeeName = qryVendorDetails.PayeeName,
                        PayMethodCode = qryVendorDetails.PaymethodCode,
                        BankCode = qryVendorDetails.BankCode,
                        Destination = qryVendorDetails.AccountNo,
                        TotalAmount = 0,
                        Remarks = qryRequisitionCasket[i].Remarks,
                        RefNo = "1",
                        CompanyCode = deptcompanycode,
                        CompanyType = companytype,
                        DeptCode = qryRequisitionCasket[i].DeptCode,
                        ItemCode = qryRequisitionCasket[i].CasketCode,
                        ItemDesc = refitems.ItemDesc,
                        Unit = refitems.UOMCode,
                        Price = TblVendorItems.Amount,
                        Quantity = qryRequisitionCasket[i].Quantity,
                        Gross = 0,
                        VatRate = 0,
                        VAT = 0,
                        NetOfVAT = 0,
                        TotalTax = 0,
                        Discount = 0,
                        DtlTotalAmount = 0,
                        AuditUser = "PISPLPI19181" //qryRequisitionCasket[i].AuditUser////qryRequisitionCasket[i].AuditUser
                    };
                    if (_qryRequisitionList.Exists(q => q.Equals(_qryRequisition)))
                    {
                        throw new Exception("dup");
                    }
                    _qryRequisitionList.Add(_qryRequisition);
                }
                 
                logger.LogInformation("Can Create - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");

                //var CanUpload = await _PRrules.CanCreateDistribution(TblAssignedtoVendor_CMSList, qryCompDtlList, _qryRequisitionList);

                //if (!string.IsNullOrEmpty(CanUpload))
                //{
                //    DistributionParams.TblResponse = new TblResponse();
                //    DistributionParams.TblResponse.Status = "FAILED";
                //    DistributionParams.TblResponse.TrxNo = DateTime.Now.ToString("mm/dd/yyy");
                //    DistributionParams.TblResponse.UniqueInfo = DateTime.Now.ToString("mm/dd/yyy");
                //    DistributionParams.TblResponse.ErrorMessage = CanUpload;
                //    DistributionParams.TblResponse.MethodName = "ReceiveRequisition";
                //    DistributionParams.TblResponse.AuditDate = DateTime.Now;
                //    return await Task.FromResult(DistributionParams.TblResponse);
                //}

                _response = await _RepositoryUnit.TblAssignedtoVendor_CMSRepository.BulkInsert(TblAssignedtoVendor_CMSList.Distinct().ToList());

                _response = new TblResponse();
                {
                    _response.TrxNo = DateTime.Now.ToString("MMMYYYYhhmmss");
                    _response.UniqueInfo = "123";
                    _response.Status = "false";
                    _response.ErrorMessage = "";
                    _response.MethodName = "";
                    _response.AuditDate = DateTime.Now;
                }

                logger.LogInformation("Create Response SUCCESS - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");

                // return await Task.FromResult(_RequisitionParams);
                requestAddress = BaseUrlService + "/Requisition/GroupRequisitionHdr";
                RequisitionParams RequisitionParams = await UtilitiesHttpClient<List<qryRequisition>>.PostAsyncT<RequisitionParams>(_qryRequisitionList, requestAddress);

                _RequisitionParams.RequisitionHdrList = RequisitionParams.RequisitionHdrList;
                _RequisitionParams.RequisitionDtlList = RequisitionParams.RequisitionDtlList;


                _RequisitionParams._TblRequisitionhdr_old = await _RepositoryUnit.TblRequisitionHdrRepository.GetlatestPRBatchNo();

                TblRequisitionhdr reqhdr = await _RepositoryUnit.TblRequisitionHdrRepository.GetlatestPRBatchNo();

                if (_RequisitionParams.RequisitionHdrList.Where(a => string.IsNullOrEmpty(a.BatchNo)).Select(a => a.BatchNo).ToList().Count > 0)
                {
                    //if (_RequisitionParams._TblRequisitionhdr_old == null)
                    //{
                    //    _ServiceParams.LastNo = "0";
                    //}
                    //else
                    //{
                    //    _ServiceParams.LastNo = _RequisitionParams._TblRequisitionhdr_old.BatchNo;
                    //}
                    requestAddress = BaseUrlService + "/Requisition/GenerateBatchNo";
                    _RequisitionParams = await UtilitiesHttpClient<RequisitionParams>.PostAsyncT<RequisitionParams>(_RequisitionParams, requestAddress);
                }

                //RequisitionParams.LastNo = reqhdr?.BatchNo ?? "0";
                //requestAddress = BaseUrlService + "/Requisition/GenerateBatchNo";
                //RequisitionParams = await UtilitiesHttpClient<RequisitionParams>.PostAsyncT<RequisitionParams>(RequisitionParams, requestAddress);
                //string batchno = RequisitionParams.LastNo;
                ReqBatch = _RequisitionParams.BatchReqNo;

                RefTrxweek refTrxweek = await _RepositoryUnit.ReftrxweekRepository.GetReftrxweek(DateTime.Now);
                //RefTrxweek refTrxweek = await _RepositoryUnit.ReftrxweekRepository.GetReftrxweek(RequisitionParams.RequisitionHdrList.Select(a => a.AuditDate).FirstOrDefault());

                for (int i = 0; i < _RequisitionParams.RequisitionHdrList.Count; i++)
                {
                    //reqhdr = await _RepositoryUnit.TblRequisitionHdrRepository.GetLatestPRRow(RequisitionParams.RequisitionHdrList[i].CompanyCode);
                    //RequisitionParams.LastNo = reqhdr?.Reqno ?? "0";
                    //RequisitionParams.CompanyCode = RequisitionParams.RequisitionHdrList[i].CompanyCode;
                    //RequisitionParams.RequisitionHdrList[i].RefNo = "1";
                    //RequisitionParams.TblResponse = new TblResponse();
                    //requestAddress = BaseUrlService + "/Requisition/GenerateNewPRNo";
                    //RequisitionParams = await UtilitiesHttpClient<RequisitionParams>.PostAsyncT<RequisitionParams>(RequisitionParams, requestAddress);

                    //RequisitionParams.RequisitionHdrList[i].Reqno = RequisitionParams.LastNo;
                    //RequisitionParams.RequisitionHdrList[i].MainReqNo = RequisitionParams.LastNo;

                    //RequisitionParams.RequisitionHdrList[i].BatchNo = ReqBatch;
                    //RequisitionParams.RequisitionHdrList[i].TrxMonth = refTrxweek.TrxMonth;
                    //RequisitionParams.RequisitionHdrList[i].TrxWeek = refTrxweek.WeekNo;

                    //TblResponse tblResponse = await _RepositoryUnit.TblRequisitionHdrRepository.CreateTblRequisitionHdr(RequisitionParams.RequisitionHdrList[i]);
                    //string _reqtype = "PO";
                    //requestAddress = BaseUrlRepo + "/Ron/CreatePRAuthorization?prno=" + RequisitionParams.RequisitionHdrList[i].Reqno + "&reqtype=" + _reqtype + "";

                    //tblResponse = await UtilitiesHttpClient<string>.PostAsync(RequisitionParams.RequisitionHdrList[i].Reqno, requestAddress);

                    string _TempReqNo = string.Empty;
                    _TempReqNo = _RequisitionParams.RequisitionHdrList[i].Reqno;
                    TblRequisitionhdr _TblRequisitionhdr = new TblRequisitionhdr();
                    _TblRequisitionhdr = _RequisitionParams.RequisitionHdrList[i];

                    _TblRequisitionhdr.BatchNo = ReqBatch;
                    _TblRequisitionhdr.AuditUser = _TblRequisitionhdr.AuditUser;
                    _TblRequisitionhdr.AuditDate = DateTime.Now; //change to RefServerDate
                    _TblRequisitionhdr.EditUser = _TblRequisitionhdr.AuditUser;
                    _TblRequisitionhdr.EditDate = DateTime.Now; //change to RefServerDate
                    _TblRequisitionhdr.DeptCode = "MORTUA"; //empdetails.DeptCode; //BatchUploadParams.qryEmployee.DeptCode;
                    _TblRequisitionhdr.CompanyCode = "SPLPI";// empdetails.CompanyCode; //BatchUploadParams.qryEmployee.CompanyCode;
                    _TblRequisitionhdr.TrxMonth = refTrxweek.TrxMonth;
                    _TblRequisitionhdr.TrxWeek = refTrxweek.WeekNo;

                    _RequisitionParams._TblRequisitionhdr_old = await _RepositoryUnit.TblRequisitionHdrRepository.GetLatestPRRow(_TblRequisitionhdr.CompanyCode);
                    _RequisitionParams.CompanyCode = _TblRequisitionhdr.CompanyCode;


                    requestAddress = BaseUrlService + "/Requisition/GenerateNewPRNo";
                    _RequisitionParams = await UtilitiesHttpClient<RequisitionParams>.PostAsyncT<RequisitionParams>(_RequisitionParams, requestAddress);

                    _TblRequisitionhdr.Reqno = _RequisitionParams.ReqNo;
                    _TblRequisitionhdr.MainReqNo = _RequisitionParams.ReqNo;
                    _response = await _RepositoryUnit.TblRequisitionHdrRepository.Create(_TblRequisitionhdr);

                    //for (int ii = 0; ii < RequisitionParams.RequisitionDtlList.Count; ii++)
                    //{
                    //    if (RequisitionParams.RequisitionHdrList[i].CompanyCode == RequisitionParams.RequisitionDtlList[ii].CompanyCode)
                    //    {
                    //        RequisitionParams.RequisitionDtlList[ii].ReqNo = RequisitionParams.RequisitionHdrList[i].Reqno;
                    //        _response = await _RepositoryUnit.TblRequisitionDtlRepository.CreateRequisitionDtl(RequisitionParams.RequisitionDtlList[ii]);
                    //    }
                    //}

                    for (int dtl = 0; dtl < _RequisitionParams.RequisitionDtlList.Count; dtl++)
                    {
                        if (_TempReqNo.Equals(_RequisitionParams.RequisitionDtlList[dtl].ReqNo))
                        {
                            _RequisitionParams.RequisitionDtlList[dtl].ReqNo = _TblRequisitionhdr.Reqno;
                            _RequisitionParams.RequisitionDtlList[dtl].AuditUser = _RequisitionParams.RequisitionDtlList[i].AuditUser;
                            _RequisitionParams.RequisitionDtlList[dtl].AuditDate = DateTime.Now;
                            _RequisitionParams.RequisitionDtlList[dtl].EditUser = _RequisitionParams.RequisitionDtlList[i].EditUser;
                            _RequisitionParams.RequisitionDtlList[dtl].EditDate = DateTime.Now;
                            //Debug = "Starting to create Req Batch DTL";
                            //_requisitionParams.RequisitionDtlList.Add(BatchRequisitionViewModel.TblRequisitiondtl[dtl]);
                            _response = await _RepositoryUnit.TblRequisitionDtlRepository.CreateRequisitionDtl(_RequisitionParams.RequisitionDtlList[dtl]);

                            _response = await _RepositoryUnit.TblAssignedtoVendor_CMSRepository.UpdateReqNo(_TblRequisitionhdr.VendorCode, _TblRequisitionhdr.Reqno, _RequisitionParams.RequisitionDtlList[dtl].DeptCode);
                        }
                    }

                    _response = await _RepositoryUnit.TblRequisitionDtlSummaryRepository.Create(_TblRequisitionhdr.Reqno, _TblRequisitionhdr.AuditUser);

                    //requestAddress = BaseUrlRepo + "/Ron/CreatePRAuthorization?prno=" + _TblRequisitionhdr.Reqno + "&reqtype=PO";
                    _response = await _ronController.CreatePRAuthorization(_TblRequisitionhdr.Reqno, "PO"); //await UtilitesHttpClient<string>.PostAsync(_TblRequisitionhdr.Reqno, requestAddress);
                    //var PODetails = await _RepositoryUnit.TblPurchaseorderhdrRepository.GETPObyReqNo(_TblRequisitionhdr.Reqno);
                    //_response = await _RepositoryUnit.TblAssignedtoVendor_CMSRepository.UpdatePONo(PODetails.PONo, _TblRequisitionhdr.Reqno);
                    //_RequisitionParams.TblRequisitionDtlSummary = await _RepositoryUnit.TblRequisitionDtlSummaryRepository.ReadList(_TblRequisitionhdr.Reqno);
                    //BatchUploadParams.TblRequisitionhdrList[i] = _TblRequisitionhdr;
                }

                //var result = new AuthorizationController(_AuthLogger, _userManager, _configuration).SendEmailAuthorization_PRNO_Batch(RequisitionParams.RequisitionHdrList.Select(a => a.Reqno).ToList(), RequisitionParams.RequisitionHdrList.Select(a => a.AuditUser).FirstOrDefault());
                var result = await new AuthorizationController(_AuthLogger, _userManager, _configuration, _env).SendEmailAuthorization_PRNO_Batch(_RequisitionParams.RequisitionHdrList.Select(a => a.Reqno).ToList(), "PISPLPI19181");

                //requestAddress = BaseUrlRepo + "/Wa/CreaterptPurchaseOrder";
                //RequisitionParams = await UtilitesHttpClient<RequisitionParams>.PostAsyncT<RequisitionParams>(RequisitionParams, requestAddress);

                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(error);

                //await _RepositoryUnit.ResponseRepository.CreateResponse(pscode, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);

            }
        }

        //[HttpPost("ReceiveDistribution")]
        //public async Task<IActionResult> ReceiveDistribution(List<TblAssignedtoVendor_CMS> TblAssignedtoVendor_CMSList)
        //{
        //    try
        //    {
        //        var _result = await _RepositoryUnit.TblAssignedtoVendor_CMSRepository.BulkInsert(TblAssignedtoVendor_CMSList);

        //        return Ok(_result);
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
        //        logger.LogError(error);

        //        //await _RepositoryUnit.ResponseRepository.CreateResponse(pscode, "FAILED", ex.Message, Utilities.GetmethodName());
        //        return BadRequest(_response);

        //    }
        //}

        [HttpPost("CreaterptPurchaseOrder")]
        public async Task<IActionResult> CreaterptPurchaseOrder(RequisitionParams requisitionParams)
        {
            try
            {
                _response = new TblResponse();
                string requestAddress = "";
                RptPurchaseorder rptPurchaseorder = new RptPurchaseorder();

                {
                    //for (int h = 0; h < requisitionParams.RequisitionDtlList.Count; h++)
                    //{

                    for (int i = 0; i < requisitionParams.RequisitionHdrList.Count; i++)
                    {

                        qryRequisitionInfo qryRequisitionInfo = await _RepositoryUnit.TblRequisitionHdrRepository.GetRequisitionInfo(requisitionParams.RequisitionHdrList[i].Reqno);
                        var tblrequisitionhdr = await _RepositoryUnit.TblRequisitionHdrRepository.ReadRequisitionHdr(requisitionParams.RequisitionHdrList[i].Reqno);
                        var tblvendortin = await _RepositoryUnit.VendorRepository.GetVendorTIN(requisitionParams.RequisitionHdrList[i].VendorCode);

                        requestAddress = OSPUrlRepo + "/CommonRepository/GetCompanyDetails";
                        var query = new Dictionary<string, string>()
                        {
                            ["CompanyType"] = "LIFEPLAN",
                            ["DeptCode"] = requisitionParams.RequisitionHdrList[i].DeptCode,
                        };
                        requestAddress = Utility.Utilities.GetUrlWithQueryString(requestAddress, query);
                        qryCompanyDetails qryCompanyDetails = await UtilitiesHttpClient<qryCompanyDetails>.GetJsonlist1(requestAddress);

                        TblPurchaseorderhdr tblPurchaseorderhdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPObyMainReqno(requisitionParams.RequisitionHdrList[i].Reqno);

                        rptPurchaseorder.CompanyDesc = qryCompanyDetails.CompanyDesc;
                        rptPurchaseorder.Address = qryRequisitionInfo.RequesterAddress;

                        rptPurchaseorder.PONo = tblPurchaseorderhdr.PONo;
                        rptPurchaseorder.ReqNo = tblPurchaseorderhdr.Reqno;
                        rptPurchaseorder.BatchNo = tblrequisitionhdr.BatchNo;
                        rptPurchaseorder.VendorName = qryRequisitionInfo.Vendor;
                        rptPurchaseorder.PayeeName = qryRequisitionInfo.PayeeName;
                        rptPurchaseorder.TIN = tblvendortin.TIN;
                        rptPurchaseorder.PayMethod = qryRequisitionInfo.PayMethod;
                        rptPurchaseorder.PayClass = qryRequisitionInfo.PayClass;

                        rptPurchaseorder.AuditUser = requisitionParams.RequisitionHdrList[i].AuditUser;
                        rptPurchaseorder.AuditDate = DateTime.Now;
                        rptPurchaseorder.Terms = "30";
                        rptPurchaseorder.isPDF = false;
                        //if (rptPurchaseorder.PONo.Contains("MC"))
                        //{
                        //    rptPurchaseorder.PONo = rptPurchaseorder.PONo;
                        //}
                        IList<qryRequisitionItem> qryRequisitionItem = await _RepositoryUnit.TblRequisitionHdrRepository.GetRequisitionItemList(requisitionParams.RequisitionHdrList[i].Reqno);
                        for (int j = 0; j < qryRequisitionItem.Count; j++)
                        {
                            rptPurchaseorder.Department = qryRequisitionItem[j].DeptDesc;
                            rptPurchaseorder.Description = qryRequisitionItem[j].Item;
                            rptPurchaseorder.Qty = qryRequisitionItem[j].Quantity;
                            rptPurchaseorder.UOM = qryRequisitionItem[j].Unit;
                            rptPurchaseorder.UnitPrice = qryRequisitionItem[j].Price;
                            rptPurchaseorder.TotalPrice = qryRequisitionItem[j].TotalAmount;
                            rptPurchaseorder.Freight = qryRequisitionItem[j].Freight;
                            rptPurchaseorder.VAT = qryRequisitionItem[j].Vat;
                            rptPurchaseorder.NetofVAT = qryRequisitionItem[j].NetOfVat;
                            rptPurchaseorder.TotalTax = qryRequisitionItem[j].TotalTax;
                            rptPurchaseorder.TotalAmount = qryRequisitionItem[j].TotalAmount + qryRequisitionItem[j].Freight;
                            rptPurchaseorder.Discount = qryRequisitionItem[j].Discount;



                            _response = await _RepositoryUnit.rptPurchaseorderRepository.Create(rptPurchaseorder);
                        }



                    }


                };

                return Ok(_response);

            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(error);

                //await _RepositoryUnit.ResponseRepository.CreateResponse(pscode, "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);

            }

        }

        //[HttpGet("ReceiveRequisition")]
        //public async Task<IActionResult> GetReqtype(AuthorizationParams authorizationParams)
        //{
        //    try
        //    {
        //        _response = new TblResponse();

        //        authorizationParams.ReqType = "PAY";
        //        for (int i = 0; i < authorizationParams.ReqNo.Count; i++)
        //        {
        //            string hasValue = _RepositoryUnit.TblPurchaseorderhdrRepository.GetPObyMainReqno(authorizationParams.ReqNo[i]).Result.Reqno;
        //            //if (hasValue)
        //            //{

        //            //}
        //        }


        //        return  Ok(authorizationParams.ReqType);
        //    }
        //    catch (Exception ex)
        //    {

        //        string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
        //        logger.LogError(error);

        //        //await _RepositoryUnit.ResponseRepository.CreateResponse(pscode, "FAILED", ex.Message, Utilities.GetmethodName());
        //        return  Ok(_response);
        //    }
        //}

        [HttpGet("ComputeReqHdr")]
        public async Task<IActionResult> ComputeReqHdr()
        {
            try
            {
                qryRequisitionHdrComputation qryRequisitionHdrComputation = new qryRequisitionHdrComputation();

                //qryRequisitionHdrComputation = _ServiceUnit.RequisitionService.; 

                return Ok(qryRequisitionHdrComputation);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + Request.Path.Value + " - " + ex.Message;
                logger.LogError(error);

                //await _RepositoryUnit.ResponseRepository.CreateResponse(pscode, "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);

            }
        }

        [HttpGet("CanUploadBatch")]
        public async Task<IActionResult> CanUploadBatch(RequisitionParams requisitionParams)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + Request.Path.Value + " - " + ex.Message;
                logger.LogError(error);

                //await _RepositoryUnit.ResponseRepository.CreateResponse(pscode, "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);

            }
        }

        [HttpGet("GetCompanyCodeByReqNo")]
        public async Task<IActionResult> GetCompanyCodeByReqNo(string reqno)
        {
            try
            {
                string CompanyCode = await _RepositoryUnit.TblRequisitionDtlRepository.ReadCompanyCodeReqDtl(reqno);
                return Ok(CompanyCode);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + Request.Path.Value + " - " + ex.Message;
                logger.LogError(error);

                //await _RepositoryUnit.ResponseRepository.CreateResponse(pscode, "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);

            }
        }

        [HttpGet("ReadVendor")]
        public async Task<IActionResult> ReadVendor(string vendorcode)
        {
            try
            {
                var vendor = await _RepositoryUnit.VendorRepository.Read(vendorcode);
                return Ok(vendor);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message;
                logger.LogError(error);

                //await _RepositoryUnit.ResponseRepository.CreateResponse(pscode, "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);

            }
        }

        [HttpPost("DeleteRequisition")]
        public async Task<IActionResult> DeleteRequisition(RequisitionParams requisitionParams)
        {
            try
            {
                logger.LogInformation("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName());
                await _RepositoryUnit.TblRequisitionHdrRepository.DeleteRequisition(requisitionParams.ReqNo);
                _response = new TblResponse();
                _response.Status = "SUCESS";
                _response.ErrorMessage = "Requisition has been deleted.";
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(requisitionParams.ReqNo, _response.Status, _response.ErrorMessage, Utilities.GetmethodName());
                return Ok(_response);
            }
            catch (Exception ex)
            {

                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(requisitionParams.ReqNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);

            }
        }

        [HttpPost("CancelRequisition")]
        public async Task<IActionResult> CancelRequisition(RequisitionParams requisitionParams)
        {
            try
            {
                logger.LogInformation("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName());
                await _RepositoryUnit.TblRequisitionHdrRepository.CancelRequisition(requisitionParams.ReqNo, requisitionParams.UserID);
                _response = new TblResponse();
                _response.Status = "SUCESS";
                _response.ErrorMessage = "Requisition has been cancelled.";
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(requisitionParams.ReqNo, _response.Status, _response.ErrorMessage, Utilities.GetmethodName());
                return Ok(_response);
            }
            catch (Exception ex)
            {

                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);

                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(requisitionParams.ReqNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);

            }
        }

        [HttpPost("CanReadExcel")]
        public async Task<IActionResult> CanReadExcel(RequisitionParams requisitionParams)
        {
            try
            {
                logger.LogInformation("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName());
                var err = await _RepositoryUnit.TblRequisitionHdrRepository.CancelRequisition(requisitionParams.ReqNo, requisitionParams.UserID);
                _response = new TblResponse();
                _response.Status = "SUCESS";
                _response.ErrorMessage = "Requisition has been cancelled.";
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(requisitionParams.ReqNo, _response.Status, _response.ErrorMessage, Utilities.GetmethodName());
                return Ok(_response);
            }
            catch (Exception ex)
            {

                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);

                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(requisitionParams.ReqNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);

            }
        }

        [HttpPost("CanVoidRequisition")]
        public async Task<IActionResult> CanVoidRequisition(RequisitionParams RequisitionParams)

        {
            try
            {
                _response = new TblResponse();

                logger.LogInformation("CanVoidRequisition - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");

                var canread = await _PRrules.CanDeleteAsync(RequisitionParams);

                if (!string.IsNullOrEmpty(canread))
                {
                    RequisitionParams.TblResponse.Status = "FAILED";
                    RequisitionParams.TblResponse.ErrorMessage = canread;
                    return Ok(RequisitionParams);
                }



                return Ok(RequisitionParams);

            }
            catch (Exception ex)
            {
                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(RequisitionParams.ReqNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);
            }
        }

        [HttpPost("InsertRequisitionPO")]
        public async Task<IActionResult> InsertRequisitionPO(BatchUploadParams BatchUploadParams)
        {
            try
            {
                _response = new TblResponse();
                string _ReqBatch = string.Empty;
                string requestAddress = string.Empty;
                RequisitionParams _RequisitionParams = new RequisitionParams();
                _RequisitionParams._TblRequisitionhdr_old = await _RepositoryUnit.TblRequisitionHdrRepository.GetlatestPRBatchNo();
                _RequisitionParams.RequisitionHdrList = BatchUploadParams.TblRequisitionhdrList;
                BatchUploadParams.qryEmployee = new qryEmployee();

                requestAddress = OSPUrlRepo + "/CommonRepository/GetEmployeeDetails";
                var query = new Dictionary<string, string>()
                {
                    ["personid"] = BatchUploadParams.UserID,
                };
                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                qryEmployee empdetails = await UtilitiesHttpClient<qryEmployee>.GetJsonlist1(requestAddress);

                BatchUploadParams.qryEmployee = empdetails;

                if (BatchUploadParams.TblRequisitionhdrList.Where(a => string.IsNullOrEmpty(a.BatchNo)).Select(a => a.BatchNo).ToList().Count > 0)
                {
                    requestAddress = BaseUrlService + "/Requisition/GenerateBatchNo";
                    _RequisitionParams = await UtilitiesHttpClient<RequisitionParams>.PostAsyncT<RequisitionParams>(_RequisitionParams, requestAddress);
                }

                _ReqBatch = _RequisitionParams.BatchReqNo;

                RefTrxweek refTrxweek = await _RepositoryUnit.ReftrxweekRepository.GetReftrxweek(DateTime.Now);

                for (int i = 0; i < BatchUploadParams.TblRequisitionhdrList.Count; i++)
                {
                    //var cancreate = _RepositoryUnit.TblRequisitionRepository.

                    string _TempReqNo = string.Empty;
                    _TempReqNo = BatchUploadParams.TblRequisitionhdrList[i].Reqno;
                    TblRequisitionhdr _TblRequisitionhdr = new TblRequisitionhdr();
                    _TblRequisitionhdr = BatchUploadParams.TblRequisitionhdrList[i];

                    _TblRequisitionhdr.BatchNo = _ReqBatch;
                    _TblRequisitionhdr.AuditUser = BatchUploadParams.UserID;
                    _TblRequisitionhdr.AuditDate = DateTime.Now; //change to RefServerDate
                    _TblRequisitionhdr.EditUser = BatchUploadParams.UserID;
                    _TblRequisitionhdr.EditDate = DateTime.Now; //change to RefServerDate
                    _TblRequisitionhdr.DeptCode = BatchUploadParams.qryEmployee.DeptCode;
                    _TblRequisitionhdr.CompanyCode = BatchUploadParams.qryEmployee.CompanyCode;
                    _TblRequisitionhdr.TrxMonth = refTrxweek.TrxMonth;
                    _TblRequisitionhdr.TrxWeek = refTrxweek.WeekNo;

                    _RequisitionParams._TblRequisitionhdr_old = await _RepositoryUnit.TblRequisitionHdrRepository.GetLatestPRRow(BatchUploadParams.TblRequisitionhdrList[i].CompanyCode);

                    _RequisitionParams.CompanyCode = BatchUploadParams.TblRequisitionhdrList[i].CompanyCode;
                    requestAddress = BaseUrlService + "/Requisition/GenerateNewPRNo";
                    _RequisitionParams = await UtilitiesHttpClient<RequisitionParams>.PostAsyncT<RequisitionParams>(_RequisitionParams, requestAddress);

                    _TblRequisitionhdr.Reqno = _RequisitionParams.ReqNo;
                    _TblRequisitionhdr.MainReqNo = _RequisitionParams.ReqNo;
                    //_TblRequisitionhdr.RefNo = "1";
                    //Debug = "Starting to create PR HDR Batch" + System.Text.Json.JsonSerializer.Serialize(_TblRequisitionhdr);
                    //_requisitionParams.RequisitionHdrList.Add(_TblRequisitionhdr);
                    _response = await _RepositoryUnit.TblRequisitionHdrRepository.Create(_TblRequisitionhdr);

                    for (int dtl = 0; dtl < BatchUploadParams.TblRequisitiondtlList.Count; dtl++)
                    {
                        if (_TempReqNo.Equals(BatchUploadParams.TblRequisitiondtlList[dtl].ReqNo))
                        {
                            BatchUploadParams.TblRequisitiondtlList[dtl].ReqNo = _TblRequisitionhdr.Reqno;
                            BatchUploadParams.TblRequisitiondtlList[dtl].AuditUser = BatchUploadParams.UserID;
                            BatchUploadParams.TblRequisitiondtlList[dtl].AuditDate = DateTime.Now;
                            BatchUploadParams.TblRequisitiondtlList[dtl].EditUser = BatchUploadParams.UserID;
                            BatchUploadParams.TblRequisitiondtlList[dtl].EditDate = DateTime.Now;
                            //Debug = "Starting to create Req Batch DTL";
                            //_requisitionParams.RequisitionDtlList.Add(BatchRequisitionViewModel.TblRequisitiondtl[dtl]);
                            _response = await _RepositoryUnit.TblRequisitionDtlRepository.CreateRequisitionDtl(BatchUploadParams.TblRequisitiondtlList[dtl]);
                        }
                    }

                    _response = await _RepositoryUnit.TblRequisitionDtlSummaryRepository.Create(_TblRequisitionhdr.Reqno, BatchUploadParams.UserID);

                    //requestAddress = BaseUrlRepo + "/Ron/CreatePRAuthorization?prno=" + _TblRequisitionhdr.Reqno + "&reqtype=PO";
                    _response = await _ronController.CreatePRAuthorization(_TblRequisitionhdr.Reqno, "PO"); //await UtilitesHttpClient<string>.PostAsync(_TblRequisitionhdr.Reqno, requestAddress);

                    _RequisitionParams.TblRequisitionDtlSummary = await _RepositoryUnit.TblRequisitionDtlSummaryRepository.ReadList(_TblRequisitionhdr.Reqno);
                    BatchUploadParams.TblRequisitionhdrList[i] = _TblRequisitionhdr;
                }

                //AuthorizationParams _authorizationParams = new AuthorizationParams();
                //_authorizationParams.ReqNo = BatchUploadParams.TblRequisitionhdrList.Select(a => a.Reqno).ToList();
                //_authorizationParams.UserCode = BatchUploadParams.UserID;
                //_authorizationParams.ReqType = "PY";

                //var resp = await  _ronController.SendEmailAuthorization_PRNO_Batch(BatchRequisitionViewModel.TblRequisitionhdr.Select(a => a.Reqno).ToList(), _UserID);

                //logger.LogInformation("CanVoidRequisition - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                //BatchUploadParams.TblResponse = await _RepositoryUnit.TblRequisitionHdrRepository.BulkInsert(BatchUploadParams.TblRequisitionhdrList);
                //BatchUploadParams.TblResponse = await _RepositoryUnit.TblRequisitionDtlRepository.BulkInsert(BatchUploadParams.TblRequisitiondtlList);

                return Ok(BatchUploadParams);
            }
            catch (Exception ex)
            {
                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(BatchUploadParams.TblRequisitionhdrList.Select(a => a.BatchNo).FirstOrDefault(), "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);
            }
        }

        [HttpPost("InsertBatchPaymentList")]
        public async Task<IActionResult> InsertBatchPaymentList(BatchUploadParams BatchUploadParams)
        {
            try
            {
                string _ReqBatch = string.Empty;
                string requestAddress = string.Empty;
                string DtlCompanyCode = string.Empty;
                RequisitionParams _RequisitionParams = new RequisitionParams();
                TblLoanhdr _TblLoanhdr = new TblLoanhdr();

                _RequisitionParams._TblRequisitionhdr_old = await _RepositoryUnit.TblRequisitionHdrRepository.GetlatestPRBatchNo();

                requestAddress = BaseUrlService + "/Requisition/GenerateBatchNo";
                _RequisitionParams = await UtilitiesHttpClient<RequisitionParams>.PostAsyncT<RequisitionParams>(_RequisitionParams, requestAddress);

                requestAddress = OSPUrlRepo + "/CommonRepository/GetAllCompanyDetails";
                IList<qryCompanyDetails> qryCompDtlList = await UtilitiesHttpClient<qryCompanyDetails>.GetJsonlist(requestAddress);

                RefTrxweek refTrxweek = await _RepositoryUnit.ReftrxweekRepository.GetReftrxweek(DateTime.Now);

                _ReqBatch = _RequisitionParams.BatchReqNo;
                BatchUploadParams.TblPurchaseorderhdrList = new List<TblPurchaseorderhdr>();
                BatchUploadParams.TblRequisitionhdrList = new List<TblRequisitionhdr>();

                foreach (var item in BatchUploadParams.qryBatchPaymentHdrList)
                {
                    string _TempReqNo = string.Empty;
                    decimal _FreightPerUnit = 0m;
                    _TempReqNo = string.Empty;
                    //decimal _TotalFreight = 0.00m;

                    //process = "GetPOHdrByPONo";
                    TblPurchaseorderhdr _POhdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(item.PONo);
                    //process = "ReadRequestByPRNo";
                    TblRequisitionhdr _oldreq = await _RepositoryUnit.TblRequisitionHdrRepository.ReadRequisitionHdr(_POhdr.Reqno);

                    qryVendorDetails _qryVendorDetails = new qryVendorDetails();
                    _qryVendorDetails = await _RepositoryUnit.VendorRepository.GetVendorDetails(_oldreq.VendorCode, _oldreq.PayClassCode);

                    List<TblRequisitiondtl> _ReqDtlList = new List<TblRequisitiondtl>();
                    List<qryRequisitionDtl> _qryRequisitionDtl = new List<qryRequisitionDtl>();

                    string _PRNo = string.Empty;
                    _FreightPerUnit = item.FreightAmount / BatchUploadParams.qryBatchPaymentDtlList.Sum(a => a.Quantity); //_ReqDtlList.Sum(a => a.Quantity);

                    foreach (var itemDtl in BatchUploadParams.qryBatchPaymentDtlList.Where(t => t.SalesInvoice == item.SalesInvoiceNo && t.PONo == item.PONo).ToList())
                    {
                        string[] DeptName = itemDtl.Department.Split('-');
                        //process = "GetCompanyDetails";

                        //requestAddress = OSPUrlRepo + "/CommonRepository/GetCompanyDetails";

                        //var query = new Dictionary<string, string>()
                        //{
                        //    ["CompanyType"] = DeptName[0],
                        //    ["DeptCode"] = DeptName[1]
                        //};

                        //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                        qryCompanyDetails _qryCompanyDetails = qryCompDtlList.Where(a => a.CompanyType.Equals(DeptName[0]) && a.DeptCode.Equals(DeptName[1])).FirstOrDefault(); //await UtilitiesHttpClient<qryCompanyDetails>.GetJsonlist1(requestAddress);


                        qryVendorDetails qryVendorDetails = await _RepositoryUnit.VendorRepository.GetVendorDetails(_oldreq.VendorCode, _oldreq.PayClassCode);
                        string itemcode = await _RepositoryUnit.RefItemsRepository.GetItemCodeByDesc(Utilities.ChangeItemDjango(itemDtl.ItemDescription, false));

                        //_TotalFreight += itemDtl.FreightAmount;

                        TblRequisitiondtl _origReqDtl =
                         await _RepositoryUnit.TblRequisitionDtlRepository.ReadRequisitionDtl(_oldreq.Reqno, _qryCompanyDetails.CompanyCode, _qryCompanyDetails.DeptCode, itemcode);

                        qryComputeBreakdown _criteria = new qryComputeBreakdown()
                        {
                            Qty = itemDtl.Quantity,
                            Gross = itemDtl.TemPriceAmount,//_origReqDtl.Price,
                            VatRate = 1.12m,
                            Discount = _origReqDtl.Discount,
                            Disccode = "002",
                            isVAT = qryVendorDetails.isVat
                        };
                        //_requisitionParams.qryComputeBreakdown = new qryComputeBreakdown();
                        //_requisitionParams.qryComputeBreakdown = _criteria;

                        requestAddress = BaseUrlService + "/PaymentRequest/ComputeBreakDown";
                        TblAPIResponse<qryComputeBreakdown> response = await UtilitiesHttpClient<qryComputeBreakdown>.PostAsyncEntity<qryComputeBreakdown, TblAPIResponse<qryComputeBreakdown>>(_criteria, requestAddress);
                        qryComputeBreakdown _qryComputeBreakdown = response.Data;

                        TblRequisitiondtl _reqdtl = new TblRequisitiondtl()
                        {
                            ReqItemNo = 0,
                            ReqNo = _PRNo,
                            CompanyCode = _origReqDtl.CompanyCode,
                            DeptCode = _origReqDtl.DeptCode,
                            ItemCode = _origReqDtl.ItemCode,
                            Unit = _origReqDtl.Unit,
                            Price = itemDtl.TemPriceAmount, //_origReqDtl.Price,
                            Quantity = itemDtl.Quantity,
                            Gross = _qryComputeBreakdown.Gross,
                            VatRate = _qryComputeBreakdown.VatRate,
                            Vat = _qryComputeBreakdown.Vat,
                            NetofVat = _qryComputeBreakdown.NetOfVAT,
                            TotalTax = 0.00m,
                            Discount = _qryComputeBreakdown.Discount,
                            TotalAmount = _qryComputeBreakdown.AmountDue,
                            Deduction = item.HPDeduction,
                            Freight = itemDtl.FreightAmount,
                            FreightPerUnit = _FreightPerUnit,//itemDtl.FreightAmount,
                            Void = false,
                            AuditUser = BatchUploadParams.UserID,
                            AuditDate = DateTime.Now,
                            UploadStat = false,
                            EditUser = BatchUploadParams.UserID,
                            EditDate = DateTime.Now
                        };
                        DtlCompanyCode = _origReqDtl.CompanyCode;

                        qryRequisitionDtl _qryReqDtl = new qryRequisitionDtl()
                        {
                            Gross = _reqdtl.Gross,
                            VAT = _reqdtl.Vat,
                            NetOfVAT = _reqdtl.NetofVat,
                            TotalTax = _reqdtl.TotalTax,
                            Discount = _reqdtl.Discount,
                            TotalAmount = _reqdtl.TotalAmount,
                            //Deduction = _reqdtl.Deduction
                        };

                        _qryRequisitionDtl.Add(_qryReqDtl);
                        _ReqDtlList.Add(_reqdtl);

                        //process = "PostCreateRequisitionDtl";
                        //await _RepositoryUnit.TblRequisitionRepository.PostCreateRequisitionDtl(BaseUrlRepo, _reqdtl);
                    }

                    qryRequisitionHdrComputation _qryRequisitionHdrComputation = new qryRequisitionHdrComputation();

                    requestAddress = BaseUrlService + "/PaymentRequest/ComputeHdrBreakDown";
                    _qryRequisitionHdrComputation = await UtilitiesHttpClient<List<qryRequisitionDtl>>.PostAsyncT<qryRequisitionHdrComputation>(_qryRequisitionDtl, requestAddress);

                    //_qryRequisitionHdrComputation = await _ServiceUnit.PaymentRequestService.ComputeBreakDownHdr(BaseUrlService, _qryRequisitionDtl);
                    item.ReferenceReceiptNo = item.SalesInvoiceNo;
                    TblRequisitionhdr _TblRequisitionhdr = new TblRequisitionhdr()
                    {
                        Reqno = string.Empty,
                        MainReqNo = _oldreq.Reqno,
                        BatchNo = _ReqBatch,
                        CompanyCode = _oldreq.CompanyCode,
                        DeptCode = _oldreq.DeptCode,
                        ReqDate = DateTime.Now,
                        PayClassCode = _oldreq.PayClassCode,
                        Active = true,
                        VendorCode = _oldreq.VendorCode,
                        PayeeName = _qryVendorDetails.PayeeName,
                        PayMethodCode = _qryVendorDetails.PaymethodCode,
                        BankCode = _qryVendorDetails.BankCode,
                        Destination = _qryVendorDetails.AccountNo,
                        Vat = _qryRequisitionHdrComputation.Vat,
                        NetofVat = _qryRequisitionHdrComputation.NetOfVat,
                        TotalTax = _qryRequisitionHdrComputation.TotalTax,
                        Deduction = item.HPDeduction,//_qryRequisitionHdrComputation.Deduction,
                        Discount = _qryRequisitionHdrComputation.Discount,
                        AmountDue = _qryRequisitionHdrComputation.AmountDue,
                        //TotalAmount = ComputeDeduction(item.Amount, item.HPDeduction),
                        TotalAmount = _qryRequisitionHdrComputation.Gross,
                        TotalFreight = item.FreightAmount,
                        TransType = "REG",
                        Remarks = _oldreq.Remarks,
                        Void = false,
                        VoidUser = string.Empty,
                        VoidDate = Convert.ToDateTime("1/1/1900"),
                        Printed = false,
                        AuditUser = BatchUploadParams.UserID,
                        AuditDate = DateTime.Now,
                        UploadStat = false,
                        EditUser = BatchUploadParams.UserID,
                        EditDate = DateTime.Now,
                        TrxMonth = refTrxweek.TrxMonth,
                        TrxWeek = refTrxweek.WeekNo,
                        RefNo = item.ReferenceReceiptNo,
                        DtlCompanyCode = DtlCompanyCode,
                        OrigQty = _ReqDtlList.Sum(a => a.Quantity)
                    };

                    TblPaymentrequisitionhdr _TblPaymentrequisitionhdr = new TblPaymentrequisitionhdr()
                    {
                        Reqno = string.Empty,
                        PRno = string.Empty,
                        PRDate = DateTime.Now,
                        Active = true,
                        TotalAmount = _TblRequisitionhdr.TotalAmount,//item.Amount,
                        SalesInvoiceNo = string.IsNullOrEmpty(item.SalesInvoiceNo) ? string.Empty : item.SalesInvoiceNo,
                        SalesInvoiceDate = item.SalesInvoiceDate,
                        DeliveryNo = item.DeliveryNo,
                        DeliveryDate = item.DeliveryDate,
                        Printed = false,
                        AuditUser = BatchUploadParams.UserID,
                        AuditDate = DateTime.Now,
                        TrxMonth = _TblRequisitionhdr.TrxMonth,
                        TrxWeek = _TblRequisitionhdr.TrxWeek,
                    };

                    _TblPaymentrequisitionhdr.TotalAmount = _TblRequisitionhdr.AmountDue;

                    TblRequisitionhdr _oldReqhdr = await _RepositoryUnit.TblRequisitionHdrRepository.GetLatestPRRow(_oldreq.CompanyCode);
                    //_RequisitionParams.LastNo = _oldReqhdr.Reqno;
                    _RequisitionParams.CompanyCode = _oldreq.CompanyCode;
                    _RequisitionParams._TblRequisitionhdr_old = _oldReqhdr;

                    requestAddress = BaseUrlService + "/Requisition/GenerateNewPRNo";
                    _RequisitionParams = await UtilitiesHttpClient<RequisitionParams>.PostAsyncT<RequisitionParams>(_RequisitionParams, requestAddress);
                    _PRNo = _RequisitionParams.ReqNo;
                    _TblRequisitionhdr.Reqno = _PRNo;

                    BatchUploadParams.TblResponse = await _RepositoryUnit.TblRequisitionHdrRepository.CreateTblRequisitionHdr(_TblRequisitionhdr);

                    foreach (var ReqDtl in _ReqDtlList)
                    {
                        //process = "PostCreateRequisitionDtl";
                        ReqDtl.ReqNo = _PRNo;
                        _RequisitionParams.TblResponse = await _RepositoryUnit.TblRequisitionDtlRepository.CreateRequisitionDtl(ReqDtl);



                        // if (_RequisitionParams.TblResponse.Status == "FAILED")
                        // {
                        //     //return Json(new
                        //     //{
                        //         success = resp.Status,
                        //         msg = resp.ErrorMessage + " " + Debug
                        //     //},
                        ////new JsonSerializerOptions());
                        // }
                    }

                    _TblRequisitionhdr.Reqno = _PRNo;
                    _TblPaymentrequisitionhdr.Reqno = _PRNo;
                    _TblPaymentrequisitionhdr.PRno = _PRNo;
                    var Res = await _RepositoryUnit.TblRequisitionDtlSummaryRepository.Create(_TblPaymentrequisitionhdr.Reqno, BatchUploadParams.UserID);
                    List<TblRequisitionDtlSummary> _TblRequisitionDtlSummary = new List<TblRequisitionDtlSummary>();

                    _TblRequisitionDtlSummary = await _RepositoryUnit.TblRequisitionDtlSummaryRepository.ReadList(_TblRequisitionhdr.Reqno);

                    TblPurchaseorderhdr _reqPOhdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByReqNo(_TblRequisitionhdr.MainReqNo);

                    BatchUploadParams.TblPurchaseorderhdrList.Add(_reqPOhdr);
                    BatchUploadParams.TblRequisitionhdrList.Add(_TblRequisitionhdr);
                    _TblRequisitionhdr.TotalFreight = _TblRequisitionDtlSummary.Sum(a => a.Freight);
                    //process = "PostCreateRequisitionHdr";

                    // if (resp.Status == "FAILED")
                    // {
                    //     return Json(new
                    //     {
                    //         success = resp.Status,
                    //         msg = resp.ErrorMessage + " " + Debug
                    //     },
                    //new JsonSerializerOptions());
                    // } 
                    //process = "PostCreatePaymentRequisitionHdr";
                    BatchUploadParams.TblResponse = await _RepositoryUnit.PaymentrequisitionhdrRepository.CreateTblPaymentrequisitionhdr(_TblPaymentrequisitionhdr);
                    // if (resp.Status == "FAILED")
                    // {
                    //     return Json(new
                    //     {
                    //         success = resp.Status,
                    //         msg = resp.ErrorMessage + " " + Debug
                    //     },
                    //new JsonSerializerOptions());
                    // }
                    //process = "GetPOHdrByReqNo";
                    //TblPurchaseorderhdr _reqPOhdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByReqNo(_TblRequisitionhdr.MainReqNo);

                    //List<IFormFile> _reqAttachments = new List<IFormFile>();

                    _TblLoanhdr = new TblLoanhdr()
                    {
                        LAFNo = _TblRequisitionhdr.Reqno,
                        LPANo = _TblRequisitionhdr.MainReqNo,
                        AppliedLoan = _TblRequisitionhdr.TotalAmount
                    };

                    BatchUploadParams.TblResponse = await _RepositoryUnit.TblLoanHdrRepository.CreateLoanHdr(_TblLoanhdr);

                    string _reqtype = "PY";
                    //requestAddress = BaseUrlRepo + "/Ron/CreatePRAuthorization?prno=" + _TblRequisitionhdr.Reqno + "&reqtype=" + _reqtype + "";

                    BatchUploadParams.TblResponse = await _ronController.CreatePRAuthorization(_TblRequisitionhdr.Reqno, _reqtype); // await UtilitesHttpClient<string>.PostAsync(_TblRequisitionhdr.Reqno, requestAddress);

                    //Insert DrList
                    //var SIList = BatchUploadParams.qryBatchPaymentDtlList.Where(a => a.SalesInvoice.Equals(item.SalesInvoiceNo)).GroupBy(a => a.DeliveryNo).ToList();
                    var SIList = BatchUploadParams.qryBatchPaymentDtlList.Where(a => a.SalesInvoice.Equals(item.SalesInvoiceNo))
                        .GroupBy(a => new { a.SalesInvoice, a.DeliveryNo })
                        .Select(a => new qryBatchPaymentDtl { SalesInvoice = a.Key.SalesInvoice, DeliveryNo = a.Key.DeliveryNo })
                        .ToList();

                    foreach (var Dtl in SIList)
                    {
                        TblDRNo _TblDRNo = new TblDRNo()
                        {
                            ReqNo = _PRNo,
                            SINo = Dtl.SalesInvoice,
                            DRNo = Dtl.DeliveryNo,
                            DeliveryDate = BatchUploadParams.qryBatchPaymentDtlList.Where(a => a.DeliveryNo.Equals(Dtl.DeliveryNo) && a.SalesInvoice.Equals(Dtl.SalesInvoice)).Select(a => a.DeliveryDate).FirstOrDefault(),
                            AuditUser = BatchUploadParams.UserID,
                            AuditDate = DateTime.Now
                        };
                        BatchUploadParams.TblResponse = await _RepositoryUnit.TblDRNoRepository.CreateDRNo(_TblDRNo);
                    }

                    // if (resp.Status == "FAILED")
                    // {
                    //     return Json(new
                    //     {
                    //         success = resp.Status,
                    //         msg = resp.ErrorMessage + " " + Debug
                    //     },
                    //new JsonSerializerOptions());
                    // }

                    //Update PO


                }

                //foreach (var item in BatchUploadParams.qryBatchPaymentHdrList)
                //{
                //    var SIList = BatchUploadParams.qryBatchPaymentDtlList.Where(a => a.SalesInvoice.Equals(item.SalesInvoiceNo)).Distinct().ToList();
                //    foreach (var Dtl in SIList)
                //    {
                //        TblDRNo _TblDRNo = new TblDRNo()
                //        {
                //            SINo = Dtl.SalesInvoice,
                //            DRNo = Dtl.DeliveryNo,
                //            DeliveryDate = Dtl.DeliveryDate,
                //            AuditUser = BatchUploadParams.UserID,
                //            AuditDate = DateTime.Now
                //        };
                //        BatchUploadParams.TblResponse = await _RepositoryUnit.TblDRNoRepository.CreateDRNo(_TblDRNo);
                //    }
                //}

                var DtlList = BatchUploadParams.qryBatchPaymentDtlList.GroupBy(a => new { a.PONo, a.ItemDescription, a.TemPriceAmount }).Select(h => h.Key).ToList();
                foreach (var item in DtlList)
                {
                    string _ItemCode = await _RepositoryUnit.RefItemsRepository.GetItemCodeByDesc(Utilities.ChangeItemDjango(item.ItemDescription, false));

                    //TEMPORARY UPDATING OF PO AMOUNT BASED ON PAYMENT PRICE
                    if (await _RepositoryUnit.TblRequisitionDtlRepository.GetItemPriceByPOItemCode(item.PONo, _ItemCode) != item.TemPriceAmount)
                    {
                        BatchUploadParams.TblResponse = await _RepositoryUnit.TblPurchaseorderhdrRepository.UpdatePOPrice(item.PONo, _ItemCode, item.TemPriceAmount);
                    }
                }

                AuthorizationParams _authorizationParams = new AuthorizationParams();
                _authorizationParams.ReqNo = BatchUploadParams.TblRequisitionhdrList.Select(a => a.Reqno).ToList();
                _authorizationParams.UserCode = BatchUploadParams.UserID;
                _authorizationParams.ReqType = "PY";

                var resp = await _ronController.ProcessAuthorization(_authorizationParams);

                foreach (var item in BatchUploadParams.TblRequisitionhdrList.Select(a => a.Reqno).ToList())
                {
                    BatchUploadParams.TblResponse = await _RepositoryUnit.TblPaymentRequestAuthRepository.UpdatePayment(item, "FOR ENCODING", "PD", Convert.ToDateTime("1900-01-01 00:00:00.000"), 0, BatchUploadParams.UserID);
                }


                //BatchUploadParams.TblResponse = JsonConvert.DeserializeObject(resp.ToString());

                BatchUploadParams.TblResponse = new TblResponse
                {
                    Status = "SUCCESS",
                    AuditDate = DateTime.Now,
                    ErrorMessage = "SUCCESS",
                    MethodName = "Insert Batch Payment",
                    TrxNo = BatchUploadParams.TblRequisitionhdrList.Select(x => x.BatchNo).FirstOrDefault(),
                    UniqueInfo = "1"
                };

                return Ok(BatchUploadParams);
            }
            catch (Exception ex)
            {
                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(BatchUploadParams.TblRequisitionhdrList.Select(a => a.BatchNo).FirstOrDefault(), "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);
            }
        }

        [HttpPost("ReadBatchPaymentList")]
        public async Task<IActionResult> ReadBatchPaymentList(BatchUploadParams BatchUploadParams)
        {
            try
            {
                _response = new TblResponse();
                logger.LogInformation("GetAllCompanyDetails in ReadBatchPayment - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");

                string requestAddress = OSPUrlRepo + "/CommonRepository/GetAllCompanyDetails";
                var qryCompanyDetailsLst = await UtilitiesHttpClient<List<qryCompanyDetails>>.GetJsonlist1(requestAddress);
                var qrycompanyDetails = new qryCompanyDetails();
                TblRequisitionhdr tblRequisitionhdr = new TblRequisitionhdr();
                TblRequisitionhdr _mainreq = new TblRequisitionhdr();
                TblVendor tblVendor = new TblVendor();

                logger.LogInformation("ReadBatchPayment - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");

                TblPurchaseorderhdr _POhdr = new TblPurchaseorderhdr();
                
                foreach (var item in BatchUploadParams.qryBatchPaymentDtlList)
                {
                    string _tempItem = await _RepositoryUnit.RefItemsRepository.GetItemCodeByDesc(item.ItemDescription);
                    string _tempItemDesc = await _RepositoryUnit.RefItemsRepository.GetItemDesc(_tempItem);

                    string[] DeptName = item.Department.Split('-');
                    qrycompanyDetails = qryCompanyDetailsLst.Where(a => a.CompanyType == DeptName[0] && a.DeptCode == DeptName[1]).FirstOrDefault();

                    _POhdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(item.PONo);
                    var _reqdtl = await _RepositoryUnit.TblRequisitionDtlRepository.ReadRequisitionDtl(_POhdr.Reqno, qrycompanyDetails.CompanyCode, qrycompanyDetails.DeptCode, _tempItem);
                    tblRequisitionhdr = await _RepositoryUnit.TblRequisitionHdrRepository.ReadRequisitionHdr(_POhdr.Reqno);

                    tblVendor = await _RepositoryUnit.VendorRepository.Read(tblRequisitionhdr.VendorCode);

                    item.SalesInvoice = tblVendor.Prefix + item.SalesInvoice;
                    item.ItemDescription = _tempItemDesc;
                    item.Amount = item.Amount * item.Quantity;
                    item.FreightAmount = item.FreightAmount * item.Quantity;
                }

                foreach (var item in BatchUploadParams.qryBatchPaymentHdrList)
                {
                    _POhdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(item.PONo);

                    _mainreq = await _RepositoryUnit.TblRequisitionHdrRepository.ReadRequisitionHdr(_POhdr.Reqno);
                    //qryCompanyDetails qryCompanyDetails = qryCompanyDetailsLst.Where(a=>a.)       
                    tblVendor = await _RepositoryUnit.VendorRepository.Read(tblRequisitionhdr.VendorCode);
                    //item.SalesInvoiceNo = tblVendor.Prefix + item.SalesInvoiceNo;

                    tblRequisitionhdr = await _RepositoryUnit.TblRequisitionHdrRepository.ReadRequisitionHdr(_POhdr.Reqno);

                    tblVendor = await _RepositoryUnit.VendorRepository.Read(tblRequisitionhdr.VendorCode);
                    item.PayeeName = tblVendor.DisplayName;

                    item.Amount = BatchUploadParams.qryBatchPaymentDtlList.Where(a => a.PONo == item.PONo && a.SalesInvoice.Equals(item.SalesInvoiceNo)).Sum(x => x.Amount);
                    item.FreightAmount = BatchUploadParams.qryBatchPaymentDtlList.Where(a => a.PONo == item.PONo && a.SalesInvoice.Equals(item.SalesInvoiceNo)).Sum(x => x.FreightAmount);
                }

                return Ok(BatchUploadParams);
            }
            catch (Exception ex)
            {

                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(BatchUploadParams.TblRequisitionhdrList.Select(a => a.BatchNo).FirstOrDefault(), "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);
            }
        }


        [HttpGet("GetBarCodes")]
        public async Task<IActionResult> GetBarCodes(string PONo)
        {
            try
            {
                IList<TblItemBarcodes> barcodes = await _RepositoryUnit.TblItemBarcodesRepository.GetBarCodes(PONo);

                var qrypobarcode = barcodes.Select(a => new qryPOBarcodes
                {
                    PONo = a.PONo,
                    Barcode = a.BarCode,
                    ItemCode = a.ItemCode
                });
                return Ok(qrypobarcode);
            }
            catch (Exception ex)
            {

                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                //_response = await _RepositoryUnit.ResponseRepository.CreateResponse(BatchUploadParams.TblRequisitionhdrList.Select(a => a.BatchNo).FirstOrDefault(), "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);
            }
        }

        [HttpGet("GetBarCodesSummary")]
        public async Task<IActionResult> GetBarCodesSummary(string PONO)
        {
            try
            {
                var barcodes = await _RepositoryUnit.TblItemBarcodesRepository.GetBarCodesSummary(PONO);
                return Ok(barcodes);
            }
            catch (Exception ex)
            {

                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                //_response = await _RepositoryUnit.ResponseRepository.CreateResponse(BatchUploadParams.TblRequisitionhdrList.Select(a => a.BatchNo).FirstOrDefault(), "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);
            }
        }

        [HttpGet("GetPONo")]
        public async Task<IActionResult> GetPONo()
        {
            try
            {
                //IList<TblPurchaseorderhdr> pohdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPONo();
                var qrypohdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPONo();
                return Ok(qrypohdr);
            }
            catch (Exception ex)
            {

                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                //_response = await _RepositoryUnit.ResponseRepository.CreateResponse(BatchUploadParams.TblRequisitionhdrList.Select(a => a.BatchNo).FirstOrDefault(), "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);
            }
        }
    }
}
