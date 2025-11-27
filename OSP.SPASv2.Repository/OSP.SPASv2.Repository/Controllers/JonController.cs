using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSP.SPASv2.Domain.Params;
using OSP.SPASv2.Repository.Parameters;
using OSP.SPASv2.Repository.Repository.MainRepository;
using OSP.SPASv2.Repository.Rules;
using OSP.SPASv2.Repository.Utility;
using SPASv2.Context;
using System.Security.Policy;
using System.Web.Http.Results;

namespace OSP.SPASv2.Repository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JonController : ControllerBase
    {
        private readonly JWTAuthenticationManager jwtAuthenticationManager;

        RepositoryUnit _RepositoryUnit;
        private ILogger<JonController> logger;
        private SPASv2Context context;
        //TblResponse _response = new TblResponse();
        TblResponse _response;
        string _validmessage;
        PaymentRequestParams _PaymentRequestParams = new PaymentRequestParams();
        PaymentRequestRules prrules;



        private IConfiguration configuration;
        //string GlobalPersonid ;
        private string UploadingPathPR;
        private string PRBatchFilePath;

        string BaseUrl;
        string BaseUrlRepo;
        string BaseUrlService;
        string OSPUrlRepo;
        string OSPUrlService;
        private IConfiguration _configuration;

        public JonController(ILogger<JonController> _logger, SPASv2Context _context, JWTAuthenticationManager _jwt, IConfiguration configuration)
        {
            logger = _logger;
            this.context = _context;
            _RepositoryUnit = new RepositoryUnit(_context);
            this.jwtAuthenticationManager = _jwt;
            prrules = new PaymentRequestRules(_context);
            _configuration = configuration;

            UploadingPathPR = _configuration.GetSection("UploadingPath")["PaymentRequest"];
            PRBatchFilePath = _configuration.GetSection("UploadingPath")["PRBatchPath"];

            BaseUrlRepo = _configuration.GetSection("APIBaseURL")["SPASv2.Repository"];
            BaseUrlService = _configuration.GetSection("APIBaseURL")["SPASv2.Service"];
            OSPUrlRepo = _configuration.GetSection("APIBaseURLCommon")["Common.Repository"];
            OSPUrlService = _configuration.GetSection("APIBaseURLCommon")["Common.Service"];
            BaseUrl = _configuration.GetSection("BaseURL").Value;


        }

        
        [HttpPost("CreatePaymentRequisitionHdr")]
        public async Task<TblResponse> CreatePaymentRequisitionHdr(TblPaymentrequisitionhdr _TblPaymentrequisitionhdr)
        {
            try
            {
                _response = new TblResponse();

                logger.LogInformation("Create Payment Requisition Hdr- " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                await _RepositoryUnit.PaymentrequisitionhdrRepository.CreateTblPaymentrequisitionhdr(_TblPaymentrequisitionhdr);

                logger.LogInformation("Create Response - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_TblPaymentrequisitionhdr.PRno, "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());

                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(_TblPaymentrequisitionhdr.PRno, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }
        }

        [HttpPost("CreateTblPurchaseOrderHdr")]
        public async Task<TblResponse> CreateTblPurchaseOrderHdr(TblPurchaseorderhdr _TblPurchaseorderhdr)
        {
            try
            {
                _response = new TblResponse();

                logger.LogInformation("Create Purchase Order Hdr- " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                RefTrxweek refTrxweek = await _RepositoryUnit.ReftrxweekRepository.GetReftrxweek(_TblPurchaseorderhdr.AuditDate);
                TblRequisitionhdr hdr = await _RepositoryUnit.TblRequisitionHdrRepository.ReadRequisitionHdr(_TblPurchaseorderhdr.Reqno);
                string dtlCompanyCode = await _RepositoryUnit.TblRequisitionDtlRepository.ReadCompanyCodeReqDtl(_TblPurchaseorderhdr.Reqno);


                _TblPurchaseorderhdr.PONo = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetLatestPONo(dtlCompanyCode);
                _TblPurchaseorderhdr.TrxMonth = refTrxweek.TrxMonth;
                _TblPurchaseorderhdr.TrxWeek = refTrxweek.WeekNo;
                await _RepositoryUnit.TblPurchaseorderhdrRepository.CreateTblPurchaseOrderHdr(_TblPurchaseorderhdr);

                RequisitionParams requisitionParams = new RequisitionParams();
               
                requisitionParams.RequisitionHdrList  = new List<TblRequisitionhdr>();
                {
                    TblRequisitionhdr _TblRequisitionhdr = new TblRequisitionhdr
                    {
                        Reqno = _TblPurchaseorderhdr.Reqno,
                        RefNo= hdr.RefNo,
                        Remarks = hdr.Remarks,
                        ReqDate = hdr.ReqDate,
                        MainReqNo = hdr.MainReqNo,  
                        Active = hdr.Active,
                        AuditDate = hdr.AuditDate,  
                        AuditUser = hdr.AuditUser, 
                        TotalAmount = hdr.TotalAmount,
                        BankCode = hdr.BankCode,    
                        BatchNo = hdr.BatchNo,
                        CompanyCode = hdr.CompanyCode,
                        DeptCode = hdr.DeptCode,
                        Destination = hdr.Destination,
                        EditDate = hdr.EditDate,
                        EditUser = hdr.EditUser,
                        PayClassCode = hdr.PayClassCode,
                        PayeeName  = hdr.PayeeName,
                        PayMethodCode = hdr.PayMethodCode,
                        Printed = hdr.Printed,
                        TrxMonth = hdr.TrxMonth,
                        TrxWeek = hdr.TrxWeek,
                        UploadStat = hdr.UploadStat,
                        VendorCode  = hdr.VendorCode,
                        Void = hdr.Void,
                        VoidDate = hdr.VoidDate,
                        VoidUser = hdr.VoidUser,
                        TransType = hdr.TransType,
                        DtlCompanyCode = hdr.DtlCompanyCode,

                    };

                    requisitionParams.RequisitionHdrList.Add(_TblRequisitionhdr);

                }

                List<TblRequisitiondtl> tblRequisitiondtl = await _RepositoryUnit.TblRequisitionDtlRepository.Read(_TblPurchaseorderhdr.Reqno);
                requisitionParams.RequisitionDtlList = new List<TblRequisitiondtl>();
                {
                    foreach (var item in tblRequisitiondtl)
                    {
                        TblRequisitiondtl _TblRequisitiondtl = new TblRequisitiondtl
                        {

                            ReqNo = _TblPurchaseorderhdr.Reqno,
                            VatRate = item.VatRate,
                            Vat = item.Vat,
                            Void = item.Void,
                            NetofVat = item.NetofVat,
                            AuditDate = item.AuditDate,
                            AuditUser   = item.AuditUser,
                            CompanyCode = item.CompanyCode,
                            DeptCode = item.DeptCode,
                            Discount = item.Discount,
                            EditDate = item.EditDate,
                            EditUser = item.EditUser,
                            Gross = item.Gross,
                            ItemCode = item.ItemCode,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            ReqItemNo = item.ReqItemNo,
                            TotalAmount = item.TotalAmount,
                            TotalTax = item.TotalTax,
                            Unit = item.Unit,
                            UploadStat = item.UploadStat,

                        };
                        requisitionParams.RequisitionDtlList.Add(_TblRequisitiondtl);
                    }
                             

                }

                string requestAddress = BaseUrlRepo + "/Wa/CreaterptPurchaseOrder";
                requisitionParams = await UtilitiesHttpClient<RequisitionParams>.PostAsyncT<RequisitionParams>(requisitionParams, requestAddress);

                logger.LogInformation("Create Response - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_TblPurchaseorderhdr.PONo, "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());

                

                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(_TblPurchaseorderhdr.PONo, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }
        }

        [HttpPost("UpdateRequestQtySummary")]
        public async Task<IActionResult> UpdateRequestQtySummary(AuthorizationParams authparams)
        {
            try
            {
                _response = new TblResponse();

                logger.LogInformation("Update RequestQtySummary- " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                 await _RepositoryUnit.TblRequisitionHdrRepository.UpdateRequestQtySummary(authparams.ReqNo.FirstOrDefault());
                logger.LogInformation("Create Response - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(authparams.ReqNo.FirstOrDefault(), "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());
                authparams.Response = _response;
                return  Ok(authparams);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(authparams.ReqNo.FirstOrDefault(), "FAILED", ex.Message, Utilities.GetmethodName());
                return  BadRequest(authparams);
            }
        }

        [HttpPost("VoidRequisitionByReqNo")]
        public async Task<TblResponse> VoidRequisitionByReqNo(string ReqNo, string UserId)
        {
            try
            {
                _response = new TblResponse();

                logger.LogInformation("Update VoidRequisitionByReqNo- " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                await _RepositoryUnit.TblRequisitionHdrRepository.VoidRequisitionByReqNo(ReqNo, UserId);
                logger.LogInformation("Create Response - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");

                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(ReqNo, "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());

                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(ReqNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }
        }

        


        [HttpGet("GetPOHdrByPONo")]
        public async Task<IActionResult> GetPOHdrByPONo(string PONo)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var POHdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(PONo);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return Ok(POHdr);
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = ex.Message;
                return Ok(_response);
            }
        }

        [HttpGet("GetLatestPONo")]
        public async Task<IActionResult> GetLatestPONo(string CompanyCode)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var LatestPONo = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetLatestPONo(CompanyCode);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return Ok(LatestPONo);
            }
            catch (Exception ex)
            {
                _response.ErrorMessage = ex.Message;
                return Ok(_response);
            }
        }

        [HttpGet("GetRequisitionInfo")]
        public async Task<qryRequisitionInfo> GetRequisitionInfo(string ReqNo)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var POHdr = await _RepositoryUnit.TblRequisitionHdrRepository.GetRequisitionInfo(ReqNo);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return POHdr;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetRequisitionInfoByApprovalNo")]
        public async Task<qryRequisitionInfo> GetRequisitionInfoByApprovalNo(string ApprovalNo)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var POHdr = await _RepositoryUnit.TblRequisitionHdrRepository.GetRequisitionInfoByApprovalNo(ApprovalNo);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return POHdr;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetRequisitionInfoByRefNo")]
        public async Task<qryRequisitionInfo> GetRequisitionInfoByRefNo(string RefNo)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var POHdr = await _RepositoryUnit.TblRequisitionHdrRepository.GetRequisitionInfoByRefNo(RefNo);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return POHdr;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetRequisitionListInfoByMainReqNo")]
        public async Task<IList<qryRequisitionInfo>> GetRequisitionListInfoByMainReqNo(string MainReqNo)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.TblRequisitionHdrRepository.GetRequisitionInfoByMainReqNo(MainReqNo);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetRequisitionItemList")]
        public async Task<IList<qryRequisitionItem>> GetRequisitionItemList(string ReqNo)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.TblRequisitionHdrRepository.GetRequisitionItemList(ReqNo);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet("GetPOHdrByReqNo")]
        public async Task<TblPurchaseorderhdr> GetPOHdrByReqNo(string ReqNo)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var POHdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByReqNo(ReqNo);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return POHdr;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetPaymentTypeList_Jon")]
        public async Task<IList<RefPaymentClass>> GetPaymentTypeList_Jon()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.RefPaymentTypeRepository.GetPaymentTypeList();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return new List<RefPaymentClass>(vlist);

            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;
            }
        }

        [HttpGet("GetVendorRunningBalance")]
        public async Task<IList<qryVendorRunningBalance>> GetVendorRunningBalance(string PayClassCode, string AsOfMode)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                IList<qryVendorRunningBalance> vlist = await _RepositoryUnit.TblRequisitionHdrRepository.GetVendorRunningBalance(PayClassCode, AsOfMode);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return new List<qryVendorRunningBalance>();

            }
        }

        [HttpPost("GetMaxEditdate")]
        public async Task<IActionResult> GetMaxEditdate()
        {
            DateTime maxEditDate = DateTime.Now;
            DashboardParams _params = new DashboardParams();
            _params.TblResponse = new TblResponse();
            try
            {
                logger.LogInformation("Get Max Editdate- " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                _params.MaxDateTime = await _RepositoryUnit.TblRequisitionHdrRepository.GetMaxEditDate();

                logger.LogInformation("Create Response - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_params.MaxDateTime.ToString(), "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());

                return Ok(_params);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(error);

                _params.TblResponse = await _RepositoryUnit.ResponseRepository.CreateResponse(_params.MaxDateTime.ToString(), "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_params);
            }
        }


        [HttpGet("GetRequisitionDepartment")]
        public async Task<IList<qryRequisitionDepartment>> GetRequisitionDepartment(string personid)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                IList<qryRequisitionDepartment> vlist = await _RepositoryUnit.TblRequisitionHdrRepository.GetRequisitionDepartment( personid);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return new List<qryRequisitionDepartment>();

            }
        }

        [HttpPost("GetReportByPersonID")]
        public async Task<IActionResult> GetReportByPersonID(ReportParams _ReportParams)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                _ReportParams.ReportTypeList = await _RepositoryUnit.ReportRepository.GetReportType(_ReportParams.PersonId);
                _ReportParams.ReportNameList = await _RepositoryUnit.ReportRepository.GetReportName(_ReportParams.PersonId);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                //return _ReportParams;

                return Ok(_ReportParams);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;
            }
        }
    }
}
