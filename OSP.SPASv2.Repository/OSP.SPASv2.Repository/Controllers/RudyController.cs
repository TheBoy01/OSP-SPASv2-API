using ClosedXML.Excel;
//using DocumentFormat.OpenXml.Drawing.Charts;
//using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OSP.Common.Domain.View;
using OSP.SPASv2.Domain.Params;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Repository.Context;
using OSP.SPASv2.Repository.Parameters;
using OSP.SPASv2.Repository.Repository.MainRepository;
using OSP.SPASv2.Repository.Repository.ServiceUnit;
using OSP.SPASv2.Repository.Rules;
using OSP.SPASv2.Repository.Utility;
using OSP.SPASv2.Web.Models;
using SPASv2.Context;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks.Dataflow;
using System.Web.Http.Results;


namespace OSP.SPASv2.Repository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RudyController : ControllerBase
    {
        private readonly JWTAuthenticationManager jwtAuthenticationManager;

        RepositoryUnit _RepositoryUnit;
        RepositoryUnit _RepositoryUnitV1;
        ServiceUnit _ServiceUnit;
        private ILogger<RudyController> logger;
        private IConfiguration _configuration;
        private SPASv2Context context;
        private SPASv1Context context1;
        //TblResponse _response = new TblResponse();
        TblResponse _response;

        string _validmessage;
        string _Task;
        string BaseUrlRepo;
        string BaseUrlService;
        string OSPUrlRepo;
        string OSPUrlService;
        string ServerFiles;
        string CMSDeliveryTemplate;
        string UploadingPathPR;

        VendorParams _Vendorparams = new VendorParams();
        //PaymentRequestParams _PaymentRequestParams = new PaymentRequestParams();
        PaymentRequestRules _PRRules;
        SPASv1Params _SPASv1Params = new SPASv1Params();
        SPASv1Rules _SPASv1Rules = new SPASv1Rules();

        PRBatchUploadParams _PRBatchUploadParams = new PRBatchUploadParams();
        PRBatchUploadRules _PRBatchUploadRules;

        VendorRules vrules = new VendorRules();
        WaController wacontroller;
        public RudyController(ILogger<RudyController> _logger, SPASv2Context _context, SPASv1Context _context1, JWTAuthenticationManager _jwt, IConfiguration iConfig,
            WaController _wacontroller)
        {
            logger = _logger;
            this.context = _context;
            this.context1 = _context1;

            _ServiceUnit = new ServiceUnit(_context);
            _RepositoryUnit = new RepositoryUnit(_context);
            _RepositoryUnitV1 = new RepositoryUnit(_context1);
            _PRBatchUploadRules = new PRBatchUploadRules(_context);
            _PRRules = new PaymentRequestRules(_context);
            this.jwtAuthenticationManager = _jwt;
            _configuration = iConfig;

            BaseUrlRepo = _configuration.GetSection("APIBaseURL")["SPASv2.Repository"];
            BaseUrlService = _configuration.GetSection("APIBaseURL")["SPASv2.Service"];
            OSPUrlRepo = _configuration.GetSection("APIBaseURLCommon")["Common.Repository"];
            OSPUrlService = _configuration.GetSection("APIBaseURLCommon")["Common.Service"];
            ServerFiles = _configuration.GetSection("UploadingPath")["ReqFiles"];
            CMSDeliveryTemplate = _configuration.GetSection("UploadingPath")["CMSDeliveryPathTemplate"];
            UploadingPathPR = _configuration.GetSection("UploadingPath")["PaymentRequest"];

            wacontroller = _wacontroller;
        }

        //--POST

        [HttpPost("CreateLoanHdr")]
        public async Task<IActionResult> CreateLoanHdr(RequisitionParams RequisitionParams)
        {
            try
            {
                logger.LogInformation("Create Loan - " + Utilities.GetmethodName() + "");

                for (int i = 0; i < RequisitionParams.tblLoanhdrs.Count; i++)
                {
                    await _RepositoryUnit.TblLoanHdrRepository.CreateLoanHdr(RequisitionParams.tblLoanhdrs[i]);
                }

                _response = new TblResponse();
                return Ok(_response);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("CreateVendorAddress")]
        public async Task<TblResponse> CreateVendorAddress(TblVendorAddress TblVendorAddress)
        {
            try
            {
                _Vendorparams.TblVendorAddress = TblVendorAddress;
                logger.LogInformation("Can Create - " + Utilities.GetmethodName() + "");
                _validmessage = vrules.CanCreate(_Vendorparams);
                if (String.IsNullOrEmpty(_validmessage))
                {
                    logger.LogInformation("Create Vendor - " + Utilities.GetmethodName() + "");
                    await _RepositoryUnit.TblVendorAddressRepository.CreateVendorAddress(TblVendorAddress);

                    logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                    await _RepositoryUnit.ResponseRepository.CreateResponse(TblVendorAddress.VendorCode, "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());
                }
                else
                {
                    await _RepositoryUnit.ResponseRepository.CreateResponse(TblVendorAddress.VendorCode, "FAILED", _validmessage, Utilities.GetmethodName());
                }
                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(TblVendorAddress.VendorCode, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }
        }

        [HttpPost("CreateVendor1")]
        public async Task<TblResponse> CreateVendor1(TblVendor tblvendor)
        {
            try
            {
                _Vendorparams.TblVendor = tblvendor;
                logger.LogInformation("Can Create - " + Utilities.GetmethodName() + "");
                _validmessage = vrules.CanCreate(_Vendorparams);
                if (String.IsNullOrEmpty(_validmessage))
                {
                    logger.LogInformation("Create Vendor - " + Utilities.GetmethodName() + "");
                    await _RepositoryUnit.VendorRepository.CreateVendor(tblvendor);

                    logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                    await _RepositoryUnit.ResponseRepository.CreateResponse(tblvendor.VendorCode, "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());
                }
                else
                {
                    await _RepositoryUnit.ResponseRepository.CreateResponse(tblvendor.VendorCode, "FAILED", _validmessage, Utilities.GetmethodName());
                }
                return await Task.FromResult(_response);

            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(tblvendor.VendorCode, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }

        }

        [HttpPost("CreateVendor")]
        public async Task<TblResponse> CreateVendor(TblVendor tblvendor)
        {
            try
            {
                _Vendorparams.TblVendor = tblvendor;
                logger.LogInformation("Can Create - " + Utilities.GetmethodName() + "");
                _validmessage = vrules.CanCreate(_Vendorparams);
                if (String.IsNullOrEmpty(_validmessage))
                {
                    logger.LogInformation("Create Vendor - " + Utilities.GetmethodName() + "");
                    await _RepositoryUnit.VendorRepository.CreateVendor(tblvendor);

                    logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                    await _RepositoryUnit.ResponseRepository.CreateResponse(tblvendor.VendorCode, "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());
                }
                else
                {
                    await _RepositoryUnit.ResponseRepository.CreateResponse(tblvendor.VendorCode, "FAILED", _validmessage, Utilities.GetmethodName());
                }
                return await Task.FromResult(_response);

            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(tblvendor.VendorCode, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }

        }

        [HttpPost("UpdateVendor")]
        public async Task<TblResponse> UpdateVendor(TblVendor tblvendor)
        {
            try
            {
                _Vendorparams.TblVendor = tblvendor;

                logger.LogInformation("Can Update - " + Utilities.GetmethodName() + "");
                if (vrules.CanUpdate(_Vendorparams) == string.Empty)
                {
                    logger.LogInformation("Update Vendor - " + Utilities.GetmethodName() + "");
                    await _RepositoryUnit.VendorRepository.UpdateVendor(tblvendor);

                    logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                    await _RepositoryUnit.ResponseRepository.CreateResponse(tblvendor.VendorCode, "SUCCESS", "SUCCESFULLY UPDATE.", Utilities.GetmethodName());
                }
                else
                {
                    await _RepositoryUnit.ResponseRepository.CreateResponse(tblvendor.VendorCode, "FAILED", vrules.CanCreate(_Vendorparams), Utilities.GetmethodName());
                }
                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(tblvendor.VendorCode, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }
        }

        [HttpPost("DeleteVendor")]
        public async Task<TblResponse> DeleteVendor(TblVendor tblvendor)
        {
            try
            {
                _Vendorparams.TblVendor = tblvendor;

                logger.LogInformation("Can Delete - " + Utilities.GetmethodName() + "");
                if (vrules.CanDelete(_Vendorparams) == string.Empty)
                {
                    logger.LogInformation("Delete Vendor " + Utilities.GetmethodName() + "");
                    await _RepositoryUnit.VendorRepository.DeleteVendor(tblvendor);

                    logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                    await _RepositoryUnit.ResponseRepository.CreateResponse(tblvendor.VendorCode, "SUCCESS", "SUCCESFULLY DELETE.", Utilities.GetmethodName());
                }
                else
                {
                    await _RepositoryUnit.ResponseRepository.CreateResponse(tblvendor.VendorCode, "FAILED", vrules.CanCreate(_Vendorparams), Utilities.GetmethodName());
                }
                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(tblvendor.VendorCode, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }

        }

        //public async Task<TblResponse> UpdateRequestByRequestNo(string PaymentRequestNo)
        //{
        //    try
        //    {
        //        _response = new TblResponse();
        //        logger.LogInformation("Update Payment Request No: " + PaymentRequestNo + " - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");

        //        _response = await _RepositoryUnit.PaymentrequesthdrRepository.UpdateRequestByRequestNo(PaymentRequestNo);


        //        logger.LogInformation("Create Response - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
        //        _response = await _RepositoryUnit.ResponseRepository.CreateResponse(PaymentRequestNo, "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());

        //        return await Task.FromResult(_response);
        //    }
        //    catch (Exception ex) 
        //    {
        //        string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
        //        logger.LogError(error);

        //        await _RepositoryUnit.ResponseRepository.CreateResponse(PaymentRequestNo, "FAILED", ex.Message, Utilities.GetmethodName());
        //        return await Task.FromResult(_response);
        //    }
        //}


        //--END OF POST--//



        //--GET

        //[HttpPost("TestingMultiple")]
        //public async Task<TblResponse> TestingMultiple(TblDataSourceDtl TblDataSourceDtl)
        //{
        //    try
        //    { 
        //        return await _RepositoryUnit.RefOutsideServerRepository.Test(TblDataSourceDtl);
        //    }
        //    catch (Exception ex)
        //    { 
        //        throw new Exception(ex.Message);
        //    }
        //}

        [HttpGet("GetLatestVendorCode")]
        public async Task<string> GetLatestVendorCode()
        {
            try
            {
                string _LatestVendorCode = string.Empty;
                TblVendor _VendorDetails = new TblVendor();
                logger.LogInformation("Select Latest Vendor Code - " + Utilities.GetmethodName() + "");

                _VendorDetails = await _RepositoryUnit.VendorRepository.GetLatestVendorCode();

                logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                await _RepositoryUnit.ResponseRepository.CreateResponse(_LatestVendorCode, "SUCCESS", "SUCCESFULLY GET.", Utilities.GetmethodName());

                if (_VendorDetails != null)
                {
                    _LatestVendorCode = _VendorDetails.VendorCode.Substring(_VendorDetails.VendorCode.Length - 6);
                }

                return await Task.FromResult(_LatestVendorCode);
            }
            catch (Exception ex)
            {
                //string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                //logger.LogError(error);

                //return await Task.FromResult(_LatestVendorCode);
                //await _RepositoryUnit.ResponseRepository.CreateResponse(_LatestVendorCode, "FAILED", ex.Message, Utilities.GetmethodName());
                //return await Task.FromResult(_response);

                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GetVendorList")]
        public async Task<ActionResult<IList<qryVendorList>>> GetVendorList()
        {

            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.VendorRepository.GetVendorLists();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return new List<qryVendorList>(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);
                await _RepositoryUnit.ResponseRepository.CreateResponse("VendorList", "FAILED", ex.Message, Utilities.GetmethodName());
                return null;
            }
        }

        //[AllowAnonymous]
        //[HttpPost("Authorize")]
        //public IActionResult AuthUser([FromBody] TblAccessCredentials user)
        //{
        //    var token = jwtAuthenticationManager.Authenticate(user.Username, user.Password);
        //    if (token == null)
        //    {
        //        return Unauthorized();
        //    }
        //    return Ok(token);
        //}

        [HttpGet("GetVendorTypeList")]
        public async Task<List<RefVendorType>> GetVendorTypeList()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var VendorTypeList = await _RepositoryUnit.RefVendorTypeRepository.GetVendorTypeList();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return VendorTypeList.ToList();
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;
            }
        }

        [HttpGet("GetAddressTypeList")]
        public async Task<IList<RefAddressType>> GetAddressTypeList()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var AddressTypeList = await _RepositoryUnit.RefAddressTypeRepository.GetAddressTypeList();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return AddressTypeList.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet("GetBankAcctTypeList")]
        public async Task<List<string>> GetBankAcctTypeList()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var AcctTypeList = await _RepositoryUnit.RefBankAcctTypeRepository.GetBankAcctTypeList();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return AcctTypeList.Select(a => a.AcctTypeCode).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetBankList")]
        public async Task<List<RefBank>> GetBankList()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var BankList = await _RepositoryUnit.RefBankRepository.GetBankList();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return new List<RefBank>(BankList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetATCTypeList")]
        public async Task<List<string>> GetATCTypeList()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var ATCTypeList = await _RepositoryUnit.RefATCTypeRepository.GetATCTypeList();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return ATCTypeList.Select(a => a.Value + "-" + a.ATCType).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetATCList")]
        public async Task<IList<RefATC>> GetATCList()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var ATCList = await _RepositoryUnit.RefATCRepository.GetATCList();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                //return ATCList.ToList();
                return new List<RefATC>(ATCList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetVendorDocsList")]
        public async Task<IList<RefVendorDocs>> GetVendorDocsList()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var VendorDocsList = await _RepositoryUnit.RefVendorDocsRepository.GetVendorDocsList();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                //return ATCList.ToList();
                return new List<RefVendorDocs>(VendorDocsList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetRegionList")]
        public async Task<IList<RefRegion>> GetRegionList()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var RegionList = await _RepositoryUnit.RefRegionRepository.GetRegionList();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                //return ATCList.ToList();
                return new List<RefRegion>(RegionList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetProvinceList")]
        public async Task<IList<RefProvince>> GetProvinceList()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var ProvinceList = await _RepositoryUnit.RefProvinceRepository.GetProvinceList();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                //return ATCList.ToList();
                return new List<RefProvince>(ProvinceList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetCityList")]
        public async Task<IList<RefCity>> GetCityList()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var CityList = await _RepositoryUnit.RefCityRepository.GetCityList();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                //return ATCList.ToList();
                return new List<RefCity>(CityList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetBrgyList")]
        public async Task<IList<RefBrgy>> GetBrgyList()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var BrgyList = await _RepositoryUnit.RefBrgyRepository.GetBrgyList();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return new List<RefBrgy>(BrgyList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("GetCityByProvinceCode")]
        public async Task<IList<RefCity>> GetCityByProvinceCode(string ProvinceCode)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var CityList = await _RepositoryUnit.RefCityRepository.GetCityByProvinceCode(ProvinceCode);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                //return ATCList.ToList();
                return new List<RefCity>(CityList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet("GetBrgyListByCityCode")]
        public async Task<IList<RefBrgy>> GetBrgyListByCityCode(string CityCode)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var BrgyList = await _RepositoryUnit.RefBrgyRepository.GetBrgyListByCityCode(CityCode);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return new List<RefBrgy>(BrgyList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetLatestPRRow")]
        public async Task<IActionResult> GetLatestPRRow(string companycode, string deptCode)
        {
            try
            {
                // logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");


                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.TblRequisitionHdrRepository.GetLatestPRRow(companycode);
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

        #region POST

        [HttpPost("EndorseToAccounting")]
        public async Task<IActionResult> EndorseToAccounting(AuthorizationParams _AuthParams)
        {
            try
            {
                string _AuditUser = _AuthParams.UserCode;
                List<string> _PRNoList = new List<string>();
                _PRNoList = _AuthParams.ReqNo.ToList();

                decimal _TotalAmount = 0;
                decimal _TotalVAT = 0;
                decimal _DebitAmount = 0.0000m;
                decimal _CreditAmount = 0.0000m;
                decimal _TotalDeduction = 0.0000m;

                string _ClassID = string.Empty;
                string _BankCode = "";
                string _BankAcctNo = "";
                string _Email = "";
                string _ContactNo = "";
                string _BatchName = _AuthParams.BANo;
                string _ReqNoCompanyCode = string.Empty;
                string _SPASv1CompanyCode = string.Empty;
                string _VendorID = string.Empty;
                string _Reason = string.Empty;
                string _AcctDeptCode = string.Empty;
                string _Remarks = string.Empty;

                qryVendorDetails _qryVendorDetails = new qryVendorDetails();
                IList<qryVendorContact> _qryVendorContact = new List<qryVendorContact>();
                List<RefAccountMap> _RefAccountMap = new List<RefAccountMap>();
                List<TblDataSourceDtl> _TblDataSourceDtl_List = new List<TblDataSourceDtl>();
                List<TblDataSourceDtl> _TblDataSourceDtl_List_Credit = new List<TblDataSourceDtl>();
                _ClassID = "Inventory";
                for (int i = 0; i < _PRNoList.Count; i++)
                {
                    TblRequisitionhdr _TblRequisitionhdr = new TblRequisitionhdr();
                    qryRequisitionInfo _qryRequisitionInfo = new qryRequisitionInfo();
                    TblPaymentrequisitionhdr _TblPaymentrequisitionhdr = new TblPaymentrequisitionhdr();

                    _TblRequisitionhdr = await _RepositoryUnit.TblRequisitionHdrRepository.ReadRequisitionHdr(_PRNoList[i]);
                    _TblPaymentrequisitionhdr = await _RepositoryUnit.PaymentrequisitionhdrRepository.GetTblPaymentrequisitionhdrBy(_PRNoList[i]);
                    _qryRequisitionInfo = await _RepositoryUnit.TblRequisitionHdrRepository.GetRequisitionInfoByMainReq(_TblRequisitionhdr.MainReqNo);

                    _TotalAmount += _TblRequisitionhdr.TotalAmount + _TblRequisitionhdr.TotalFreight;
                    _Reason = _Reason + "PO: " + _qryRequisitionInfo.PONo + ", SI: " + _TblPaymentrequisitionhdr.SalesInvoiceNo + "; ";

                    TblRequisitionDtlSummary _TblRequisitionDtlSummary = new TblRequisitionDtlSummary();
                    _TblRequisitionDtlSummary = await _RepositoryUnit.TblRequisitionDtlSummaryRepository.Read(_PRNoList[i]);
                    _SPASv1CompanyCode = await _RepositoryUnit.RefCompanyAdapterRepository.GetSPASv1CompCode(_TblRequisitionhdr.DtlCompanyCode);

                    if (string.IsNullOrEmpty(_qryVendorDetails.VendorCode))//(_qryVendorDetails == null)
                    {
                        _qryVendorDetails = await _RepositoryUnit.VendorRepository.GetVendorDetails(_TblRequisitionhdr.VendorCode, _TblRequisitionhdr.PayClassCode);
                        _VendorID = _TblRequisitionhdr.VendorCode; // await _RepositoryUnit.TblVendorAdapterRepository.GetVendorID(_TblRequisitionhdr.VendorCode, _SPASv1CompanyCode.Replace("T", ""));
                        _qryVendorContact = await _RepositoryUnit.TblVendorContactPersonRepository.GetVendorContact(_qryVendorDetails.VendorCode);
                        _BankCode = "104";
                        _BankAcctNo = "";

                        if (!_qryVendorDetails.PaymentMethod.ToUpper().Equals("CHEQUE"))
                        {
                            _BankCode = _qryVendorDetails.BankCode;
                            _BankAcctNo = _qryVendorDetails.AccountNo;
                        }

                        _Email = _qryVendorContact.Where(a => a.ContactType.ToUpper().TrimStart().TrimEnd().Equals("EMAIL")).Select(a => a.ContactDetails).FirstOrDefault();
                        _ContactNo = _qryVendorContact.Where(a => a.ContactType.ToUpper().Contains("SMS")).Select(a => a.ContactDetails).FirstOrDefault();

                        //for (int row = 0; row < _qryVendorContact.Count; row++)
                        //{
                        //    switch (_qryVendorContact[row].ContactType.ToUpper().TrimStart().TrimEnd())
                        //    {
                        //        case "EMAIL":
                        //            _Email = _qryVendorContact[row].ContactDetails;
                        //            break;

                        //        case "SMS - GLOBE":
                        //        case "SMS - SMART":
                        //            _ContactNo = _qryVendorContact[row].ContactDetails;
                        //            break;
                        //        default:
                        //            break;
                        //    }
                        //}
                        _RefAccountMap = await _RepositoryUnit.RefAccountMapRepository.GetAccountMapList(_qryVendorDetails.isVat);
                        _AcctDeptCode = _TblRequisitionhdr.DeptCode;
                        _Remarks = _AuthParams.BANo + " " + _qryVendorDetails.PaymentMethod;
                    }

                    decimal _VAT = _RepositoryUnit.VatRepository.GetRefVat().Result.Vat;
                    if (!_qryVendorDetails.isVat)
                    {
                        _VAT = 1;
                    }

                    foreach (var item in _RefAccountMap)
                    {
                        string _BranchAcctCode = string.Empty;
                        _BranchAcctCode = item.DeptAcctCode;

                        //if (string.IsNullOrEmpty(_BranchAcctCode))
                        //{
                        //    _BranchAcctCode = await _RepositoryUnitV1.RefOutsideServerRepository.GetAcctCodeByDeptCode(ReqDtl.DeptCode, _SPASv1CompanyCode);
                        //}
                        if (item.ForVat && !_qryVendorDetails.isVat)
                        {
                            continue;
                        }

                        switch (item.NormalBalance.ToUpper())
                        {
                            case "DEBIT":
                                //foreach (TblRequisitiondtl ReqDtl in Grp_TblRequisitiondtl)
                                //{
                                if (item.ForVat)
                                {
                                    _TotalVAT = _TotalVAT + await _RepositoryUnit.RefAccountMapRepository.ComputeDtlEntries(_PRNoList[i], item.Formula, _VAT);
                                    continue;
                                }
                                TblDataSourceDtl _TblDataSourceDtl_Debit = new TblDataSourceDtl()
                                {
                                    ReferenceNo = _AuthParams.BANo,
                                    AccountCode = item.AccountCode,
                                    BranchAcctCode = await _RepositoryUnitV1.RefOutsideServerRepository.GetAcctCodeByDeptCode(_TblRequisitionDtlSummary.DeptCode, _SPASv1CompanyCode), //item.DeptAcctCode.Equals(string.Empty) ? await _RepositoryUnitV1.RefOutsideServerRepository.GetAcctCodeByDeptCode(_TblRequisitionDtlSummary.DeptCode, _SPASv1CompanyCode) : item.DeptAcctCode,
                                    Debit = await _RepositoryUnit.RefAccountMapRepository.ComputeDtlEntries(_PRNoList[i], item.Formula, _VAT),
                                    Credit = _CreditAmount,
                                    Note = _TblPaymentrequisitionhdr.SalesInvoiceNo + "; ReqNo: " + _TblRequisitionhdr.Reqno,
                                };
                                _TblDataSourceDtl_List.Add(_TblDataSourceDtl_Debit);
                                //}
                                continue;

                            //case "CREDIT":



                            //    TblDataSourceDtl _TblDataSourceDtl_Credit = new TblDataSourceDtl()
                            //    {
                            //        ReferenceNo = _AuthParams.BANo,
                            //        AccountCode = item.AccountCode,
                            //        BranchAcctCode = item.DeptAcctCode,
                            //        Debit = _DebitAmount,
                            //        Credit = item.Hierarchy.Equals(4) ? await _RepositoryUnit.RefAccountMapRepository.ComputeDtlEntries(_PRNoList[i], item.Formula, _VAT) : Math.Abs(await _RepositoryUnit.RefAccountMapRepository.ComputeDtlEntries(_PRNoList[i], item.Formula, _VAT) - _TotalDeduction),
                            //        Note = "",
                            //    };
                            //    _TblDataSourceDtl_List_Credit.Add(_TblDataSourceDtl_Credit);
                            //    continue;

                            default:
                                break;
                        }
                    }
                    _TotalDeduction += Convert.ToDecimal(_TblRequisitionDtlSummary.Deduction);
                }



                _RefAccountMap = await _RepositoryUnit.RefAccountMapRepository.GetAccountMapList(_qryVendorDetails.isVat);
                RequisitionParams _RequisitionParams = new RequisitionParams();

                for (int i = 0; i < _RefAccountMap.Count; i++)
                {
                    RefAccountMap item = _RefAccountMap[i];

                    if (item.ForVat)
                    {
                        TblDataSourceDtl _TblDataSourceDtl_Debit = new TblDataSourceDtl()
                        {
                            ReferenceNo = _AuthParams.BANo,
                            AccountCode = item.AccountCode,
                            BranchAcctCode = item.DeptAcctCode,
                            Debit = _TotalVAT,
                            Credit = _CreditAmount,
                            Note = "",
                        };
                        _TblDataSourceDtl_List.Add(_TblDataSourceDtl_Debit);
                    }

                    switch (item.NormalBalance.ToUpper())
                    {
                        case "CREDIT":

                            if (_TotalDeduction > 0)
                            {
                                TblDataSourceDtl _TblDataSourceDtl_Deduction = new TblDataSourceDtl()
                                {
                                    ReferenceNo = _AuthParams.BANo,
                                    AccountCode = "1443000",
                                    BranchAcctCode = item.DeptAcctCode,
                                    Debit = _DebitAmount,
                                    Credit = _TotalDeduction,
                                    Note = "",
                                };
                                _TblDataSourceDtl_List.Add(_TblDataSourceDtl_Deduction);
                                _TotalDeduction = 0.00m;
                            }

                            if (item.Hierarchy.Equals(3))
                            {
                                //decimal _TotalInventory = _TblDataSourceDtl_List.Where(a => a.AccountCode.Equals(_RefAccountMap.Where(a => a.Hierarchy.Equals(0)).Select(a => a.AccountCode).FirstOrDefault())).Sum(a => a.Debit);
                                //decimal _TotalFreight = _TblDataSourceDtl_List.Where(a => a.AccountCode.Equals(_RefAccountMap.Where(a => a.Hierarchy.Equals(1)).Select(a => a.AccountCode).FirstOrDefault())).Sum(a => a.Debit);
                                //decimal _TotalEWT = _TotalInventory * 0.01m;

                                string requestAddress = BaseUrlService + "/Requisition/ComputeCasketInventory_CreditAP_EWT";
                                _RequisitionParams.TblDataSourceDtl_List = _TblDataSourceDtl_List;
                                _RequisitionParams.RefAccountMap = _RefAccountMap;
                                _RequisitionParams.EWTPercentage = 0.01m; //Change to vendor ATC percentage
                                _RequisitionParams.TotalVAT = _TotalVAT;
                                _RequisitionParams = await UtilitiesHttpClient<RequisitionParams>.PostAsyncT<RequisitionParams>(_RequisitionParams, requestAddress);

                                TblDataSourceDtl _TblDataSourceDtl_CreditAP = new TblDataSourceDtl()
                                {
                                    ReferenceNo = _AuthParams.BANo,
                                    AccountCode = item.AccountCode,
                                    BranchAcctCode = item.DeptAcctCode,
                                    Debit = _DebitAmount,
                                    Credit = _RequisitionParams.CreditAP,
                                    Note = "",
                                };
                                _TblDataSourceDtl_List.Add(_TblDataSourceDtl_CreditAP);

                                TblDataSourceDtl _TblDataSourceDtl_CreditEWT = new TblDataSourceDtl()
                                {
                                    ReferenceNo = _AuthParams.BANo,
                                    AccountCode = _RefAccountMap[i + 1].AccountCode,
                                    BranchAcctCode = item.DeptAcctCode,
                                    Debit = _DebitAmount,
                                    Credit = _RequisitionParams.CreditEWT,
                                    Note = "",
                                };
                                _TblDataSourceDtl_List.Add(_TblDataSourceDtl_CreditEWT);
                            }

                            //TblDataSourceDtl _TblDataSourceDtl_Credit = new TblDataSourceDtl()
                            //{
                            //    ReferenceNo = _TblDataSourceHdr.ReferenceNo,
                            //    AccountCode = item.AccountCode,
                            //    BranchAcctCode = item.DeptAcctCode,
                            //    Debit = _DebitAmount,
                            //    Credit = _TblDataSourceDtl_List_Credit.Where(a => a.AccountCode.Equals(item.AccountCode)).Sum(a => a.Credit),
                            //    Note = "",
                            //};
                            //_TblDataSourceDtl_List.Add(_TblDataSourceDtl_Credit);
                            break;
                    }
                }

                TblDataSourceHdr _TblDataSourceHdr = new TblDataSourceHdr
                {
                    BatchName = _AuthParams.BANo,
                    ReferenceNo = _AuthParams.BANo,
                    ClassID = _ClassID,
                    VendorID = _VendorID,
                    BankCode = _BankCode,
                    BankAccountNumber = _BankAcctNo,
                    CompanyName = _qryVendorDetails.VendorName,
                    CheckName = _qryVendorDetails.PayeeName,
                    AccountDeptCode = _AcctDeptCode,
                    Reason = _Reason,
                    Remarks = _Remarks,
                    AmountDue = _TotalAmount,
                    DebitAmount = _TotalAmount,
                    DebitInput = 0.00m,
                    CreditWtax = _RequisitionParams.CreditEWT,
                    CreditMisc = _TblDataSourceDtl_List.Where(a => a.AccountCode.Equals("1443000")).Sum(a => a.Credit),
                    CreditAP = _RequisitionParams.CreditAP,
                    Email = _Email, //"olgabr@stpeter.com.ph", 
                    ContactNo = _ContactNo, //"09175074110",
                    SystemCode = "SPASV2",
                    AuditUser = _AuthParams.UserCode,
                    AuditDate = DateTime.Now,
                };

                foreach (var item in _TblDataSourceDtl_List)
                {
                    item.Debit = Math.Round(item.Debit, 4);
                    item.Credit = Math.Round(item.Credit, 4);
                }

                if (_TblDataSourceDtl_List.Sum(a => Math.Round(a.Debit, 4)) != _TblDataSourceDtl_List.Sum(a => Math.Round(a.Credit, 4)))
                {
                    string _DebitCents = Math.Round(_TblDataSourceDtl_List.Sum(a => Math.Round(a.Debit, 4)), 4).ToString().Substring(_TblDataSourceDtl_List.Sum(a => Math.Round(a.Debit, 4)).ToString().IndexOf(".") + 1);
                    string _CreditCents = Math.Round(_TblDataSourceDtl_List.Sum(a => Math.Round(a.Credit, 4)), 4).ToString().Substring(_TblDataSourceDtl_List.Sum(a => Math.Round(a.Credit, 4)).ToString().IndexOf(".") + 1);

                    if (!_DebitCents.Equals(_CreditCents))
                    {
                        decimal _Discrepancy = 0.00m;
                        _Discrepancy = _TblDataSourceDtl_List.Sum(a => Math.Round(a.Credit, 4)) - _TblDataSourceDtl_List.Sum(a => Math.Round(a.Debit, 4));
                        foreach (var item in _TblDataSourceDtl_List)
                        {
                            if (item.AccountCode.Equals("1311001"))
                            {
                                item.Debit = Math.Round(item.Debit, 4) + _Discrepancy;
                                break;
                            }
                        }
                    }
                }

                _Task = "HDR ENDROSEMENT - " + _TblDataSourceHdr.ReferenceNo;
                _response = await _RepositoryUnitV1.RefOutsideServerRepository.EndorseDataSourceHdr(_TblDataSourceHdr, _SPASv1CompanyCode);
                logger.LogInformation(_Task + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_TblDataSourceHdr.ReferenceNo, "SUCCESS", _Task, Utilities.GetmethodName());

                _Task = "DTL ENDROSEMENT - " + _TblDataSourceHdr.ReferenceNo;

                foreach (TblDataSourceDtl DtlItem in _TblDataSourceDtl_List)
                {
                    if (DtlItem.Credit.Equals(0) && DtlItem.Debit.Equals(0))
                    {
                        continue;
                    }
                    _response = await _RepositoryUnitV1.RefOutsideServerRepository.EndorseDataSourceDtl(DtlItem, _SPASv1CompanyCode);
                    logger.LogInformation(_Task + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                    _response = await _RepositoryUnit.ResponseRepository.CreateResponse(DtlItem.Note, "SUCCESS", _Task, Utilities.GetmethodName());
                }

                _Task = "SUCCESSFULY SAVED!";
                logger.LogInformation(_Task + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_TblDataSourceHdr.ReferenceNo, "SUCCESS", _Task, Utilities.GetmethodName());


                return Ok(_response);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(error);
                //await _RepositoryUnit.ResponseRepository.CreateResponse("Endorsed to accounting", "FAILED", ex.Message, Utilities.GetmethodName());
                _response.ErrorMessage = error;
                _response.Status = "FAILED";
                return BadRequest(_response);
                //return await Task.FromResult(_response);
                //throw new Exception(ex.Message);
            }
        }

        [HttpPost("CreateBatchPRHdr")]
        public async Task<TblResponse> CreatePRBatchHdr(TblBatchPRHdr TblBatchPRHdr)
        {
            try
            {
                _response = new TblResponse();
                await _RepositoryUnit.TblBatchPRHdrRepository.CreateBatchHdr(TblBatchPRHdr);

                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(TblBatchPRHdr.BatchPRNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }
        }

        [HttpPost("CreateBatchPRDtl")]
        public async Task<TblResponse> CreateBatchPRDtl(TblBatchPRDtl TblBatchPRDtl)
        {
            try
            {
                //_Vendorparams.TblVendorAddress = TblVendorAddress;
                _response = new TblResponse();
                logger.LogInformation("Create Batch Detail - " + Utilities.GetmethodName() + "");
                await _RepositoryUnit.TblBatchPRDtlRepository.CreateBatchDtl(TblBatchPRDtl);

                logger.LogInformation("Create Response - " + Utilities.GetmethodName() + "");
                await _RepositoryUnit.ResponseRepository.CreateResponse(TblBatchPRDtl.BatchPRNo, "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());

                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.GetmethodName() + " " + DateTime.Now + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(TblBatchPRDtl.BatchPRNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }
        }

        [HttpGet("GetLatestBatchNo")]
        public async Task<IActionResult> GetLatestBatchNo()
        {
            try
            {
                // logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");

                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.TblRequisitionHdrRepository.GetlatestPRBatchNo();
                //var str = result.PRNo; 
                return Ok(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return Ok(error);
                //throw;
            }
        }

        [HttpGet("GetVendorCodeByDisplayName")]
        public async Task<string> GetVendorCodeByDisplayName(string DisplayName)
        {
            try
            {
                var VendorCode = await _RepositoryUnit.VendorRepository.GetVendorCodeByDisplayName(DisplayName);
                return VendorCode;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("CanCreatePRBatchHdr")]
        public async Task<TblResponse> CanCreatePRBatchHdr(BatchUploadParams BatchUploadParams)
        {
            try
            {
                return new TblResponse
                {
                    Status = "OK",
                    AuditDate = DateTime.Now,
                    MethodName = "Can create",
                    TrxNo = "1",
                    UniqueInfo = "2",
                    ErrorMessage = "succesfully failed"
                };
                //BatchUploadParams _Batch = BatchUploadParams;
                //return new TblResponse<BatchUploadParams>
                //{
                //    StatusCode = "OK",
                //    StatusDesc = "OK",
                //    Data = _Batch
                //};
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("sampleFormFile")]
        public async Task<TblResponse> sampleFormFile(List<IFormFile> Files)
        {
            try
            {
                return new TblResponse
                {
                    Status = "OK",
                    AuditDate = DateTime.Now,
                    MethodName = "Can create",
                    TrxNo = "1",
                    UniqueInfo = "2",
                    ErrorMessage = "succesfully failed"
                };
                //BatchUploadParams _Batch = BatchUploadParams;
                //return new TblResponse<BatchUploadParams>
                //{
                //    StatusCode = "OK",
                //    StatusDesc = "OK",
                //    Data = _Batch
                //};
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("CanUploadExcelDetails")]
        public async Task<IActionResult> CanUploadExcelDetails(BatchUploadParams BatchUploadParams)
        {
            try
            {
                string requestAddress = "";
                _response = new TblResponse();
                _PRBatchUploadParams = new PRBatchUploadParams();
                //_PRBatchUploadRules = new PRBatchUploadRules();

                //qryBatchUploadExcel _qryBatchUploadExcel = new qryBatchUploadExcel();
                //_qryBatchUploadExcel = BatchUploadParams.qryBatchUploadExcel;
                //PaymentRequestParams paymentRequestParams = new PaymentRequestParams();
                logger.LogInformation("CanUploadExcelDetails - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                _PRBatchUploadParams._batchUploadParams = BatchUploadParams;


                requestAddress = OSPUrlRepo + "/CommonRepository/GetAllCompanyDetails";
                IList<qryCompanyDetails> qryCompDtlList = await UtilitiesHttpClient<qryCompanyDetails>.GetJsonlist(requestAddress);
                _PRBatchUploadParams.qryCompanyDetailsList = qryCompDtlList.ToList();
                BatchUploadParams.qryCompanyDetails = qryCompDtlList.ToList();

                // var notInList2 = BatchUploadParams.qryBatchRequistions.Except(qryCompDtlList, new ItemComparer());

                //var vlist =
                //      _PRBatchUploadParams.qryCompanyDetailsList
                //      .Join(BatchUploadParams.qryBatchRequistions, a => a.CompanyType, b => b.Department,
                //      (a, b) => new qryCompanyDetails
                //      {
                //          DeptCode = a.DeptCode,
                //          DeptDesc = a.DeptDesc,
                //          CompanyType = a.CompanyType,
                //          CompanyCode = a.CompanyCode,
                //          CompanyDesc = a.CompanyDesc

                //      }).ToList();

                var canread = await _PRBatchUploadRules.CanRead(_PRBatchUploadParams);

                if (!string.IsNullOrEmpty(canread))
                {
                    BatchUploadParams.TblResponse.Status = "FAILED";
                    BatchUploadParams.TblResponse.ErrorMessage = canread;
                    return Ok(BatchUploadParams);
                }

                BatchUploadParams = await ReadRequisitionList(BatchUploadParams);

                return Ok(BatchUploadParams);
            }
            catch (Exception ex)
            {
                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);

                _response = await _RepositoryUnit.ResponseRepository.CreateResponse("", "FAILED", ex.Message, Utilities.GetmethodName());
                BatchUploadParams.TblResponse = _response;
                return BadRequest(BatchUploadParams);
            }
        }


        [HttpPost("CanUploadPayment")]
        public async Task<IActionResult> CanUploadPayment(BatchUploadParams BatchUploadParams)
        {
            try
            {
                string requestAddress = "";
                _response = new TblResponse();
                _PRBatchUploadParams = new PRBatchUploadParams();
                //_PRBatchUploadRules = new PRBatchUploadRules();

                //qryBatchUploadExcel _qryBatchUploadExcel = new qryBatchUploadExcel();
                //_qryBatchUploadExcel = BatchUploadParams.qryBatchUploadExcel;
                //PaymentRequestParams paymentRequestParams = new PaymentRequestParams();
                logger.LogInformation("CanUploadPayment - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                _PRBatchUploadParams._batchUploadParams = BatchUploadParams;

                //requestAddress = OSPUrlRepo + "/Requisition/GenerateNewPRNo";
                //BatchUploadParams = await UtilitesHttpClient<BatchUploadParams>.PostAsyncT<BatchUploadParams>(BatchUploadParams, requestAddress);

                foreach (var BatchHdr in _PRBatchUploadParams._batchUploadParams.qryBatchPaymentHdrList)
                {
                    TblPurchaseorderhdr _POhdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(BatchHdr.PONo);
                    var a = await _RepositoryUnit.TblRequisitionHdrRepository.ReadRequisitionHdr(_POhdr.Reqno);
                    var b = await _RepositoryUnit.VendorRepository.Read(a.VendorCode);

                    BatchHdr.SalesInvoiceNo = b.Prefix + BatchHdr.SalesInvoiceNo;
                }

                var CanUpload = await _PRBatchUploadRules.CanUploadPayment(_PRBatchUploadParams);

                if (!string.IsNullOrEmpty(CanUpload))
                {
                    BatchUploadParams.TblResponse = new TblResponse();
                    BatchUploadParams.TblResponse.Status = "FAILED";
                    BatchUploadParams.TblResponse.ErrorMessage = CanUpload;
                    return Ok(BatchUploadParams);
                }

                //readpayment
                var result = await wacontroller.ReadBatchPaymentList(BatchUploadParams);

                if (result is OkObjectResult okResult && okResult.Value is BatchUploadParams batchUploadParamsResult)
                {
                    BatchUploadParams = batchUploadParamsResult;
                    // Now you can use 's' as 'BatchUploadParams'
                }

                return Ok(BatchUploadParams);
            }
            catch (Exception ex)
            {
                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);

                _response = await _RepositoryUnit.ResponseRepository.CreateResponse("", "FAILED", ex.Message, Utilities.GetmethodName());
                BatchUploadParams.TblResponse = _response;
                return BadRequest(BatchUploadParams);
            }
        }


        [HttpPost("CreateRequisitionHdr")]
        public async Task<IActionResult> CreateRequisitionHdr(TblRequisitionhdr _TblRequisitionhdr)
        {
            try
            {
                _response = new TblResponse();

                logger.LogInformation("Getting TrxWeek - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                RefTrxweek _RefTrxweek = new RefTrxweek();
                _response.MethodName = "reftrxweek";
                _RefTrxweek = await _RepositoryUnit.ReftrxweekRepository.GetReftrxweek(_TblRequisitionhdr.AuditDate);
                _TblRequisitionhdr.TrxMonth = _RefTrxweek.TrxMonth;
                _TblRequisitionhdr.TrxWeek = _RefTrxweek.WeekNo;
                //_TblRequisitionhdr.TrxMonth = "JAN24";
                //_TblRequisitionhdr.TrxWeek = 3;
                logger.LogInformation("Create Payment Requsition HDR - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                _response.MethodName = "post";
                _response = await _RepositoryUnit.TblRequisitionHdrRepository.CreateTblRequisitionHdr(_TblRequisitionhdr);
                logger.LogInformation("Create Response - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_TblRequisitionhdr.Reqno, "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());

                return Ok(_response);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(error);
                _response.ErrorMessage = error + _response.MethodName;
                //await _RepositoryUnit.ResponseRepository.CreateResponse(_TblRequisitionhdr.Reqno, "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);
            }
        }

        [HttpPost("CreateRequisitionDtl")]
        public async Task<TblResponse> CreateRequisitionDtl(TblRequisitiondtl _TblRequisitiondtl)
        {
            try
            {
                _response = new TblResponse();
                //_TblRequisitiondtl = new TblRequisitiondtl(); 

                logger.LogInformation("Create Payment Requsition Dtl - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                await _RepositoryUnit.TblRequisitionDtlRepository.CreateRequisitionDtl(_TblRequisitiondtl);

                logger.LogInformation("Create Response - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_TblRequisitiondtl.ReqNo, "SUCCESS", "SUCCESFULLY SAVE.", Utilities.GetmethodName());

                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(error);

                await _RepositoryUnit.ResponseRepository.CreateResponse(_TblRequisitiondtl.ReqNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return await Task.FromResult(_response);
            }
        }
        #endregion

        [HttpGet("GetCompanyDescByCompanyCode")]
        public async Task<string> GetCompanyDescByCompanyCode(string companyCode)
        {
            try
            {
                var VendorCode = await _RepositoryUnit.RefCompanyRepository.GetCompanyDescByCompanyCode(companyCode);
                return VendorCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("GetVendorNameByVendorCode")]
        public async Task<string> GetVendorNameByVendorCode(string VendorCode)
        {
            try
            {
                return await _RepositoryUnit.VendorRepository.GetVendorNameByVendorCode(VendorCode);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("ReadRequisitionHdr")]
        public async Task<TblRequisitionhdr> ReadRequisitionHdr(string ReqNo)
        {
            try
            {
                return await _RepositoryUnit.TblRequisitionHdrRepository.ReadRequisitionHdr(ReqNo);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("ReadRequsitionDtlByPRNo")]
        public async Task<TblRequisitiondtl> ReadRequsitionDtlByPRNo(string ReqNo, string CompanyCode, string DeptCode, string ItemCode)
        {
            try
            {
                return await _RepositoryUnit.TblRequisitionDtlRepository.ReadRequisitionDtl(ReqNo, CompanyCode, DeptCode, ItemCode);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GetPayclassCodeByDesc")]
        public async Task<string> GetPayclassCodeByDesc(string PayClassDesc)
        {
            try
            {
                var VendorCode = await _RepositoryUnit.RefPaymentClassRepository.GetGetPayclassCodeByDesc(PayClassDesc);
                return VendorCode;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetItemCodeByDesc")]
        public async Task<string> GetItemCodeByDesc(string itemDesc)
        {
            try
            {
                var VendorCode = await _RepositoryUnit.RefItemsRepository.GetItemCodeByDesc(itemDesc);
                return VendorCode;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetItemDesc")]
        public async Task<string> GetItemDesc(string itemcode)
        {
            try
            {
                var VendorCode = await _RepositoryUnit.RefItemsRepository.GetItemDesc(itemcode);
                return VendorCode;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetReqPOPY")]
        public async Task<IActionResult> GetReqPOPY(string ReqNo)
        {
            try
            {
                var Result = await _RepositoryUnit.TblRequisitionHdrRepository.GetReqPOPY(ReqNo);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateDtLSummary")]
        public async Task<IActionResult> CreateDtLSummary(RequisitionParams requisitionParams)
        {
            try
            {
                var Result = await _RepositoryUnit.TblRequisitionDtlSummaryRepository.Create(requisitionParams.ReqNo, requisitionParams.UserID);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetDtlSummary")]
        public async Task<IActionResult> GetDtlSummary(RequisitionParams RequisitionParams)
        {
            try
            {
                RequisitionParams.TblRequisitionDtlSummary = await _RepositoryUnit.TblRequisitionDtlSummaryRepository.ReadList(RequisitionParams.ReqNo);

                return Ok(RequisitionParams);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CanEndorseRequest")]
        public async Task<IActionResult> CanEndorseRequest(SPASv1Params SPASv1Params)
        {
            try
            {
                var CanEndorse = await _SPASv1Rules.CanEndorse(SPASv1Params);
                if (!string.IsNullOrEmpty(CanEndorse))
                {
                    SPASv1Params.TblResponse.Status = "FAILED";
                    SPASv1Params.TblResponse.ErrorMessage = CanEndorse;
                }
                return Ok(SPASv1Params);
                //List<qryCompanyDetails> qryCompanyDetailsList = new List<qryCompanyDetails>();
                //qryCompanyDetails qryCompanyDetails = new qryCompanyDetails();
                //foreach (var item in BatchUploadParams.qryBatchRequistions)
                //{
                //    qryCompanyDetails.CompanyType = item.CompanyType;
                //    qryCompanyDetails.DeptCode = item.Department;
                //    qryCompanyDetailsList.Add(qryCompanyDetails);
                //}
                //var canread = await _DeptartmentRules.CanReadAsyncList(qryCompanyDetailsList);
                //if (!string.IsNullOrEmpty(canread))
                //{
                //    BatchUploadParams.TblResponse.Status = "FAILED";
                //    BatchUploadParams.TblResponse.ErrorMessage = canread;
                //    return Ok(BatchUploadParams);
                //}
                //return Ok(BatchUploadParams);

            }
            catch (Exception ex)
            {
                //logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse("", "FAILED", ex.Message, Utilities.GetmethodName());
                SPASv1Params.TblResponse = _response;
                return BadRequest(SPASv1Params);
            }
        }

        [HttpPost("ReadRequisitionList")]
        public async Task<BatchUploadParams> ReadRequisitionList(BatchUploadParams BatchUploadParams)
        {
            try
            {
                string requestAddress = string.Empty;
                RequisitionParams _RequisitionParams = new RequisitionParams();
                BatchUploadParams.qryRequisitions = new List<qryRequisition>();
                BatchUploadParams.TblRequisitionhdrList = new List<TblRequisitionhdr>();
                BatchUploadParams.TblRequisitiondtlList = new List<TblRequisitiondtl>();
                BatchUploadParams.qryRequisitionHdr = new List<qryRequisitionHdr>();
                var qryCompanyDetailsLst = BatchUploadParams.qryCompanyDetails;
                if (BatchUploadParams.qryCompanyDetails is null)
                {
                    requestAddress = OSPUrlRepo + "/CommonRepository/GetAllCompanyDetails";
                    qryCompanyDetailsLst = await UtilitiesHttpClient<List<qryCompanyDetails>>.GetJsonlist1(requestAddress);
                }

                BatchUploadParams.TblResponse.ErrorMessage = "Starting API ReadRequisitionList";

                for (int dr = 0; dr < BatchUploadParams.qryBatchRequistions.Count; dr++)
                {

                    qryCompanyDetails _qryCompanyDetails = new qryCompanyDetails();
                    qryComputeBreakdown _qryComputeBreakdown = new qryComputeBreakdown();
                    //Debug = "Starting to query get company details" + OSPUrlRepo + " " + _CompanyType + " " + _Department;
                    _qryCompanyDetails.CompanyType = BatchUploadParams.qryBatchRequistions[dr].CompanyType;
                    _qryCompanyDetails.DeptCode = BatchUploadParams.qryBatchRequistions[dr].Department;

                    //requestAddress = OSPUrlRepo + "/CommonRepository/GetCompanyDetails1";
                    _qryCompanyDetails = qryCompanyDetailsLst.Where(a => a.CompanyType.Equals(BatchUploadParams.qryBatchRequistions[dr].CompanyType) && a.DeptCode.Equals(BatchUploadParams.qryBatchRequistions[dr].Department)).FirstOrDefault(); //await UtilitesHttpClient<qryCompanyDetails>.PostAsyncT<qryCompanyDetails>(_qryCompanyDetails, requestAddress);
                    //Debug = "Getcompany details passed";

                    string _VendorCode = string.Empty;
                    string _ItemCode = string.Empty;
                    qryVendorDetails _qryVendorDetails = new qryVendorDetails();
                    TblVendorItems _TblVendorItems = new TblVendorItems();
                    _VendorCode = await _RepositoryUnit.VendorRepository.GetVendorCodeByDisplayName(BatchUploadParams.qryBatchRequistions[dr].VendorName.Replace("'", "`"));
                    _ItemCode = await _RepositoryUnit.RefItemsRepository.GetItemCodeByDesc(BatchUploadParams.qryBatchRequistions[dr].ItemDesc);
                    _qryVendorDetails = await _RepositoryUnit.VendorRepository.GetVendorDetails(_VendorCode, BatchUploadParams.Payclass);
                    _TblVendorItems = await _RepositoryUnit.VendorItemsRepository.GetVendorItemsDetails(_VendorCode, _ItemCode);
                    _TblVendorItems.Amount = BatchUploadParams.qryBatchRequistions[dr].AmountPerUnit; //TEMPORARY ONLY!!! SHOULD BE REMOVE BEFORE WE LIVE
                                                                                                      //_qryComputeBreakdown = await _ServiceUnit.RequisitionService.ComputeBreakDown(_Qty, _TblVendorItems.Amount,1.12m,_Disc,"002");
                    if (_TblVendorItems is null)
                    {
                        throw new Exception(BatchUploadParams.qryBatchRequistions[dr].ItemDesc + " " + _qryVendorDetails.VendorName);
                    }
                    qryRequisition _req = new qryRequisition()
                    {
                        UserCompanyCode = _qryCompanyDetails.CompanyCode,
                        UserDeptCode = _qryCompanyDetails.DeptCode,
                        RequestDate = DateTime.Now,
                        PayClassCode = BatchUploadParams.Payclass,
                        VendorCode = _qryVendorDetails.VendorCode,//await _RepositoryUnit.VendorRepository.GetVendorCodeByDisplayName(BaseUrlRepo, _VendorName), //"202312000001",
                        VendorDesc = _qryVendorDetails.VendorName,
                        PayeeName = BatchUploadParams.qryBatchRequistions[dr].VendorName,
                        PayMethodCode = _qryVendorDetails.PaymethodCode,
                        BankCode = _qryVendorDetails.BankCode,
                        //Destination = "091234567",
                        Destination = "",
                        TotalAmount = 0.00m,//GetTotalAmount(_TblVendorItems.Amount, _Qty),
                                            //DtlTotalAmount = _TblVendorItems.Amount,
                        Remarks = BatchUploadParams.qryBatchRequistions[dr].Remarks,
                        RefNo = BatchUploadParams.qryBatchRequistions[dr].ReferenceNo,
                        CompanyCode = _qryCompanyDetails.CompanyCode,
                        CompanyDesc = _qryCompanyDetails.CompanyDesc,
                        DeptCode = _qryCompanyDetails.DeptCode,
                        DeptDesc = _qryCompanyDetails.DeptDesc,
                        ItemDesc = BatchUploadParams.qryBatchRequistions[dr].ItemDesc,
                        ItemCode = _TblVendorItems.ItemCode,
                        Unit = _TblVendorItems.UOM,
                        Price = _TblVendorItems.Amount,
                        Quantity = BatchUploadParams.qryBatchRequistions[dr].Qty,
                        Gross = _TblVendorItems.Amount,
                        VatRate = 0.00m,
                        VAT = 0.00m,
                        NetOfVAT = 0.00m,
                        TotalTax = 0.00m,
                        Discount = BatchUploadParams.qryBatchRequistions[dr].Disc,
                        CompanyType = BatchUploadParams.qryBatchRequistions[dr].CompanyType,
                        AuditUser = BatchUploadParams.UserID,
                        isVendorVat = _qryVendorDetails.isVat,
                    };
                    //Debug = "adding header passed";
                    BatchUploadParams.qryRequisitions.Add(_req);

                }
                //Debug = "finish loop on Data table";

                TblResponse _TblResponse = new TblResponse();

                requestAddress = BaseUrlService + "/Requisition/GroupRequisitionHdr";
                _RequisitionParams = await UtilitiesHttpClient<List<qryRequisition>>.PostAsyncT<RequisitionParams>(BatchUploadParams.qryRequisitions.ToList(), requestAddress);

                //_RequisitionParams = await _ServiceUnit.RequisitionService.GroupRequisitionHdrDtl(BaseUrlService, BatchUploadParams.qryRequisitions);
                //Debug = "Passed Requisition Grouping";
                BatchUploadParams.TblRequisitionhdrList = _RequisitionParams.RequisitionHdrList.ToList();
                BatchUploadParams.TblRequisitiondtlList = _RequisitionParams.RequisitionDtlList.OrderBy(a => a.CompanyCode).ToList();
                BatchUploadParams.qryRequisitions = BatchUploadParams.qryRequisitions.OrderBy(a => a.CompanyCode).ThenBy(a => a.PayeeName).ToList();

                //foreach (var item in _model._BatchSummaryList)
                //{
                //    _model.RequestNoList.Add(item.PRNo);
                //}
                List<qryRequisitionVendorCompanyChapel> qryRVCC = new List<qryRequisitionVendorCompanyChapel>();

                List<qryRequisitionHdr> _qryRequisitionHdrList = new List<qryRequisitionHdr>();

                for (int i = 0; i < BatchUploadParams.TblRequisitionhdrList.Count; i++)
                {
                    string _CompanyName = string.Empty;
                    string _CompanyType = string.Empty;

                    //requestAddress = OSPUrlRepo + "/CommonRepository/GetCompanyDescByCode";
                    //var query = new Dictionary<string, string>()
                    //{
                    //    ["companyCode"] = BatchUploadParams.TblRequisitionhdrList[i].CompanyCode,
                    //};
                    //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                    _CompanyName = qryCompanyDetailsLst.Where(a => a.CompanyCode.Equals(BatchUploadParams.TblRequisitionhdrList[i].CompanyCode)).Select(a => a.CompanyDesc).FirstOrDefault(); //await UtilitesHttpClient<string>.GetJsonstring(requestAddress);

                    //requestAddress = OSPUrlRepo + "/CommonRepository/GetCompanyType"; 
                    //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                    //_CompanyType = await UtilitesHttpClient<string>.GetJsonstring(requestAddress);
                    _CompanyType = qryCompanyDetailsLst.Where(a => a.CompanyType.Equals(BatchUploadParams.TblRequisitionhdrList[i].CompanyCode)).Select(a => a.CompanyCode).FirstOrDefault();

                    qryRequisitionHdr _qryRequisitionHdr = new qryRequisitionHdr()
                    {
                        CompanyName = _CompanyName,
                        PayeeName = BatchUploadParams.TblRequisitionhdrList[i].PayeeName,
                        VendorName = await _RepositoryUnit.VendorRepository.GetVendorNameByVendorCode(BatchUploadParams.TblRequisitionhdrList[i].VendorCode),
                        Amount = BatchUploadParams.TblRequisitionhdrList[i].TotalAmount,
                        Remarks = BatchUploadParams.TblRequisitionhdrList[i].Remarks,
                        Attachment = "",
                        CompanyCode = BatchUploadParams.TblRequisitionhdrList[i].CompanyCode,
                        DeptCode = BatchUploadParams.TblRequisitionhdrList[i].DeptCode,
                        CompanyType = _CompanyType,
                    };

                    _qryRequisitionHdrList.Add(_qryRequisitionHdr);
                }

                BatchUploadParams.qryRequisitionHdr = _qryRequisitionHdrList;
                //return Ok(BatchUploadParams);
                return BatchUploadParams;
            }
            catch (Exception ex)
            {
                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                _response.Status = "FAILED";
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(BatchUploadParams.TblRequisitionhdrList.Select(a => a.BatchNo).FirstOrDefault(), "FAILED", ex.Message, Utilities.GetmethodName());
                //return BadRequest(_response);
                BatchUploadParams.TblResponse = _response;
                return BatchUploadParams;
            }
        }

        private void CreateVendorSPASv1(string VendorCode, string CompanyCode)
        {
            //if (string.IsNullOrEmpty(_RepositoryUnit.RefOutsideServerRepository.CheckVendor(VendorCode,CompanyCode)))
            //{

            //}
        }

        [HttpPost("CreateExcelForVendor")]
        public async Task<string> CreateExcelForVendor(string PONo)
        {
            RequisitionParams _RequisitionParams = new RequisitionParams();
            //string BatchApprovalNo = string.Empty;

            //DataTable POTable = new DataTable("Purchase Order");
            //POTable.Columns.Add(new DataColumn("Vendor", typeof(string)));
            //POTable.Columns.Add(new DataColumn("Chapel", typeof(string)));
            //POTable.Columns.Add(new DataColumn("Casket", typeof(string)));
            //POTable.Columns.Add(new DataColumn("Quantity", typeof(int)));
            //POTable.Columns.Add(new DataColumn(" ", typeof(string)));
            //POTable.Columns.Add(new DataColumn("Casket Item", typeof(string)));
            //POTable.Columns.Add(new DataColumn("Barcodes", typeof(string)));

            //DataTable DeliveryTable = new DataTable("Delivery");
            //DeliveryTable.Columns.Add(new DataColumn("Casket", typeof(string)));
            //DeliveryTable.Columns.Add(new DataColumn("Barcodes", typeof(string)));

            //List<RptPurchaseorder> _RptPurchaseorder = new List<RptPurchaseorder>();
            //_RptPurchaseorder = await _RepositoryUnit.rptPurchaseorderRepository.GetListByPONo(PONo);
            //BatchApprovalNo = await _RepositoryUnit.BatchApprovalRepository.GetBatchNoByReqNo(_RptPurchaseorder.Select(a => a.ReqNo).FirstOrDefault());

            //foreach (RptPurchaseorder item in _RptPurchaseorder)
            //{
            //    POTable.Rows.Add(item.VendorName, item.Department, item.Description, item.Qty, "", "", "");

            //    //Barcodes example
            //    for (int i = 0; i < item.Qty; i++)
            //    {
            //        DeliveryTable.Rows.Add(item.Description, item.VendorName.Substring(0, 3) + "0000" + item.Description.Substring(0, 5).Replace(".", "0") + i);
            //    }
            //}

            //string FileName = BatchApprovalNo + "-" + _RptPurchaseorder.Select(a => a.VendorName).FirstOrDefault();
            ////int Counter = 1;
            ////while (System.IO.File.Exists(Path.Combine(ServerFiles, "CIS PO", FileName + ".xlsx")))
            ////{
            ////    Counter++;
            ////    FileName = FileName + "_" + Counter;
            ////}

            //using (XLWorkbook wb = new XLWorkbook())
            //{
            //    var ws = wb.Worksheets.Add(POTable);
            //    ws = wb.Worksheets.Add(DeliveryTable);
            //    ws.Columns().AdjustToContents();

            //    //var wsRange = ws.Range(2, 15, custTable.Rows.Count + 1, 19);
            //    //wsRange.Style.NumberFormat.Format = "#,###,###.0000;(#,###,###.0000)";

            //    //copy to local server \\192.168.1.6\spasv2$\Files\CIS PO 

            //    wb.SaveAs(Path.Combine(ServerFiles, "CIS PO", FileName + ".xlsx"));

            //    using (MemoryStream stream = new MemoryStream())
            //    {
            //        //download file
            //        wb.SaveAs(stream);
            //        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");
            //    }
            //} 
            string FileName = string.Empty;
            _RequisitionParams.PONo = PONo;
            _RequisitionParams.ServerPOPath = ServerFiles;
            _RequisitionParams.UserID = "TEST";
            FileName = await _ServiceUnit.VendorService.CreateExcelForVendor(_RequisitionParams, Path.Combine(CMSDeliveryTemplate, "PO Delivery Template 2.xlsx"));

            return FileName;
            //using (XLWorkbook wb = new XLWorkbook())
            //{
            //    using (MemoryStream stream = new MemoryStream())
            //    {
            //        //download file
            //        wb.SaveAs(stream);
            //        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");
            //    }
            //}

        }


        [HttpPost("ReadPOAndPAY")]
        public async Task<BatchUploadParams> ReadPOAndPAY(BatchUploadParams BatchUploadParams)
        {
            //check and create PO
            await CanUploadExcelDetails(BatchUploadParams);

            //Auto approve of PO

            //Create Payment

            return BatchUploadParams;

        }

        [HttpGet("GetDRListByReqNo")]
        public async Task<BatchUploadParams> GetDRListByReqNo(string ReqNo)
        {
            BatchUploadParams _BatchUploadParams = new BatchUploadParams();

            _BatchUploadParams.DRList = await _RepositoryUnit.TblDRNoRepository.GetDRListByReqNo(ReqNo);

            return _BatchUploadParams;
        }

        [HttpPost("CreateChapelDelivery")]
        public async Task<IActionResult> CreateChapelDelivery(
        [FromForm] string qryPaymentDeliveries, // This will receive the JSON data as a string
        IFormFile blobData) // This will receive the file/blob data
        {

            //return await ProcessDelivery(qryPaymentDeliveries);
            return await ReadPaymentDelivery(qryPaymentDeliveries,blobData);
        }

        //[HttpPost("CreateTempDelivery")]
        //public async Task<IActionResult> CreateTempDelivery() // This will receive the file/blob data
        //{
        //    string qryPaymentDeliveries = "[{\"PONo\":\"CL24000163\",\"ChapelCode\":\"PARANA\",\"DeliveryDate\":\"2024-10-04T00:00:00\",\"DRNo\":\"PARANA2410000001\",\"ReferenceReceipt\":\"PARANA2410000001\",\"Qty\":4,\"ItemDesc\":\"ST. ANDREW METAL TYPE Size 60x190\",\"UserID\":\"MAIN OBOLIVWL\"},{\"PONo\":\"CL24000163\",\"ChapelCode\":\"PARANA\",\"DeliveryDate\":\"2024-10-04T00:00:00\",\"DRNo\":\"PARANA2410000001\",\"ReferenceReceipt\":\"PARANA2410000001\",\"Qty\":6,\"ItemDesc\":\"ST. BERNADETTE METAL TYPE Size 60x190\",\"UserID\":\"MAIN OBOLIVWL\"},{\"PONo\":\"CL24000163\",\"ChapelCode\":\"PARANA\",\"DeliveryDate\":\"2024-10-04T00:00:00\",\"DRNo\":\"PARANA2410000001\",\"ReferenceReceipt\":\"PARANA2410000001\",\"Qty\":1,\"ItemDesc\":\"ST. CLAIRE METAL TYPE Size 70x200\",\"UserID\":\"MAIN OBOLIVWL\"},{\"PONo\":\"CL24000163\",\"ChapelCode\":\"PARANA\",\"DeliveryDate\":\"2024-10-04T00:00:00\",\"DRNo\":\"PARANA2410000001\",\"ReferenceReceipt\":\"PARANA2410000001\",\"Qty\":2,\"ItemDesc\":\"DJANGO IMPORTED TYPE Size 60x190\",\"UserID\":\"MAIN OBOLIVWL\"},{\"PONo\":\"CL24000163\",\"ChapelCode\":\"PARANA\",\"DeliveryDate\":\"2024-10-04T00:00:00\",\"DRNo\":\"PARANA2410000001\",\"ReferenceReceipt\":\"PARANA2410000001\",\"Qty\":14,\"ItemDesc\":\"ST. DOROTHY WOOD TYPE Size 60x190\",\"UserID\":\"MAIN OBOLIVWL\"},{\"PONo\":\"CL24000163\",\"ChapelCode\":\"PARANA\",\"DeliveryDate\":\"2024-10-04T00:00:00\",\"DRNo\":\"PARANA2410000001\",\"ReferenceReceipt\":\"PARANA2410000001\",\"Qty\":8,\"ItemDesc\":\"METAL 1 METAL TYPE Size 60x190\",\"UserID\":\"MAIN OBOLIVWL\"},{\"PONo\":\"CL24000163\",\"ChapelCode\":\"PARANA\",\"DeliveryDate\":\"2024-10-04T00:00:00\",\"DRNo\":\"PARANA2410000001\",\"ReferenceReceipt\":\"PARANA2410000001\",\"Qty\":5,\"ItemDesc\":\"ST. PAUL WOOD TYPE Size 60x190\",\"UserID\":\"MAIN OBOLIVWL\"}]";

        //    return await ProcessDelivery(qryPaymentDeliveries);

        //}

        private async Task<IActionResult> ProcessDelivery(string qryPaymentDeliveries)
        {
            BatchUploadParams _BatchUploadParams = new BatchUploadParams();
            var deliveryList = JsonConvert.DeserializeObject<List<qryPaymentDelivery>>(qryPaymentDeliveries);
            string processid = deliveryList.FirstOrDefault().DRNo + "-" + deliveryList.FirstOrDefault().ChapelCode;


            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Create the directory if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            try
            {
                // Deserialize the JSON string into your desired object type
                // Process the blob file (if needed)

                //string filePath = Path.Combine(uploadsFolder, blobData.FileName);

                //// Save the file to the specified path
                //using (var fileStream = new FileStream(filePath, FileMode.Create))
                //{
                //    await blobData.CopyToAsync(fileStream);
                //}

                //if (blobData != null && blobData.Length > 0)
                //{
                //    using (var memoryStream = new MemoryStream())
                //    {
                //        blobData.CopyTo(memoryStream);
                //        byte[] blobBytes = memoryStream.ToArray();

                //    }
                //}


                //return Ok(new { Message = "Success", Data = _BatchUploadParams });
                //temp success

                qryBatchPaymentHdr _qryBatchPaymentHdr = new qryBatchPaymentHdr();
                List<qryBatchPaymentDtl> _qryBatchPaymentDtlList = new List<qryBatchPaymentDtl>();

                TblPurchaseorderhdr _POhdr = new TblPurchaseorderhdr();
                TblRequisitionhdr _ReqHdr = new TblRequisitionhdr();
                TblVendor _tblVendor = new TblVendor();

                _BatchUploadParams.qryBatchPaymentHdrList = new List<qryBatchPaymentHdr>();
                _BatchUploadParams.qryBatchPaymentDtlList = new List<qryBatchPaymentDtl>();

                _POhdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(deliveryList.Select(a => a.PONo).FirstOrDefault());
                _ReqHdr = await _RepositoryUnit.TblRequisitionHdrRepository.ReadRequisitionHdr(_POhdr.Reqno);
                _tblVendor = await _RepositoryUnit.VendorRepository.Read(_ReqHdr.VendorCode);

                //Create qryBatchPaymentHdr and qryBatchPaymentDtl
                foreach (qryPaymentDelivery item in deliveryList)
                {
                    string _ItemCode = await _RepositoryUnit.RefItemsRepository.GetItemCodeByDesc(Utilities.ChangeItemDjango(item.ItemDesc, false));
                    var ItemDetails = await _RepositoryUnit.VendorItemsRepository.GetVendorItemsDetails(_tblVendor.VendorCode, _ItemCode);
                    var ReqDtl = await _RepositoryUnit.TblRequisitionDtlRepository.ReadRequisitionDtl(_POhdr.Reqno, _ReqHdr.DtlCompanyCode, item.ChapelCode, _ItemCode);

                    qryBatchPaymentDtl _qryBatchPaymentDtl = new qryBatchPaymentDtl()
                    {
                        PONo = item.PONo,
                        SalesInvoice = string.Empty,
                        Department = "CHAPELS-" + item.ChapelCode,
                        ItemDescription = item.ItemDesc,
                        Balance = ReqDtl.Quantity - item.Qty, // get the item in PO then minus the quantity
                        Quantity = item.Qty,
                        Amount = ItemDetails.Amount * item.Qty,
                        FreightAmount = 0,
                        ReferenceReceipt = string.Empty,
                        DeliveryNo = item.DRNo,
                        DeliveryDate = item.DeliveryDate,
                        SalesInvoiceDate = DateTime.Now,
                        TemPriceAmount = ItemDetails.Amount
                    };

                    _qryBatchPaymentDtlList.Add(_qryBatchPaymentDtl);
                }

                _qryBatchPaymentHdr = new qryBatchPaymentHdr
                {
                    PONo = _POhdr.PONo,
                    PayeeName = _tblVendor.DisplayName,
                    Amount = _qryBatchPaymentDtlList.Sum(a => a.Amount),
                    SalesInvoiceNo = string.Empty,
                    SalesInvoiceDate = Convert.ToDateTime("1900/01/01"),
                    DeliveryNo = deliveryList.Select(a => a.DRNo).FirstOrDefault(),
                    DeliveryDate = deliveryList.Select(a => a.DeliveryDate).FirstOrDefault(),
                    ReferenceReceiptNo = string.Empty,
                    HPDeduction = 0,
                    FreightAmount = 0
                };



                _BatchUploadParams.qryBatchPaymentHdrList.Add(_qryBatchPaymentHdr);
                _BatchUploadParams.qryBatchPaymentDtlList.AddRange(_qryBatchPaymentDtlList);

                //Auto approve of PO

                //Create Payment
                _BatchUploadParams.UserID = "PISPLPI19181";
                var result = await wacontroller.InsertBatchPaymentList(_BatchUploadParams);

                //return Ok(_BatchUploadParams);

                return Ok(new { Message = "Success", Data = _BatchUploadParams });
            }
            catch (Exception ex)
            {
                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(processid, "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(new { Message = "Error", _response });
            }
        }

        //[HttpPost("ReadPaymentDelivery")]
        private async Task<IActionResult> ReadPaymentDelivery(
        [FromForm] string qryPaymentDeliveriesString,
        [FromForm] IFormFile blobData)
        {
            List<qryPaymentDelivery> qryPaymentDeliveries = new List<qryPaymentDelivery>();
            qryPaymentDeliveries = JsonConvert.DeserializeObject<List<qryPaymentDelivery>>(qryPaymentDeliveriesString);

            BatchUploadParams _BatchUploadParams = new BatchUploadParams();

            qryBatchPaymentHdr _qryBatchPaymentHdr = new qryBatchPaymentHdr();
            List<qryBatchPaymentDtl> _qryBatchPaymentDtlList = new List<qryBatchPaymentDtl>();

            TblPurchaseorderhdr _POhdr = new TblPurchaseorderhdr();
            TblRequisitionhdr _ReqHdr = new TblRequisitionhdr();
            TblVendor _tblVendor = new TblVendor();

            _BatchUploadParams.qryBatchPaymentHdrList = new List<qryBatchPaymentHdr>();
            _BatchUploadParams.qryBatchPaymentDtlList = new List<qryBatchPaymentDtl>();

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            string SourceFile = string.Empty;

            // Create the directory if it doesn't exist
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            try
            {
                _POhdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(qryPaymentDeliveries.Select(a => a.PONo).FirstOrDefault());
                _ReqHdr = await _RepositoryUnit.TblRequisitionHdrRepository.ReadRequisitionHdr(_POhdr.Reqno);
                _tblVendor = await _RepositoryUnit.VendorRepository.Read(_ReqHdr.VendorCode);


                // Deserialize the JSON string into your desired object type
                // Process the blob file (if needed)

                if (!string.IsNullOrEmpty(blobData.FileName))
                {
                    string filePath = Path.Combine(uploadsFolder, blobData.FileName);

                    // Save the file to the specified path
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await blobData.CopyToAsync(fileStream);
                    }
                    SourceFile = filePath;
                    //if (blobData != null && blobData.Length > 0)
                    //{
                    //    using (var memoryStream = new MemoryStream())
                    //    {
                    //        blobData.CopyTo(memoryStream);
                    //        byte[] blobBytes = memoryStream.ToArray();

                    //    }
                    //}
                }


                //Create qryBatchPaymentHdr and qryBatchPaymentDtl
                foreach (qryPaymentDelivery item in qryPaymentDeliveries)
                {
                    string _ItemCode = await _RepositoryUnit.RefItemsRepository.GetItemCodeByDesc(Utilities.ChangeItemDjango(item.ItemDesc, false));
                    var ItemDetails = await _RepositoryUnit.VendorItemsRepository.GetVendorItemsDetails(_tblVendor.VendorCode, _ItemCode);
                    var ReqDtl = await _RepositoryUnit.TblRequisitionDtlRepository.ReadRequisitionDtl(_POhdr.Reqno, _ReqHdr.DtlCompanyCode, item.ChapelCode, _ItemCode);

                    qryBatchPaymentDtl _qryBatchPaymentDtl = new qryBatchPaymentDtl()
                    {
                        PONo = item.PONo,
                        SalesInvoice = string.Empty,
                        Department = "CHAPELS-" + item.ChapelCode,
                        ItemDescription = item.ItemDesc,
                        Balance = ReqDtl.Quantity - item.Qty, // get the item in PO then minus the quantity
                        Quantity = item.Qty,
                        Amount = ItemDetails.Amount * item.Qty,
                        FreightAmount = 0,
                        ReferenceReceipt = string.Empty,
                        DeliveryNo = item.DRNo,
                        DeliveryDate = item.DeliveryDate,
                        SalesInvoiceDate = DateTime.Now,
                        TemPriceAmount = ItemDetails.Amount
                    };

                    _qryBatchPaymentDtlList.Add(_qryBatchPaymentDtl);
                }

                _qryBatchPaymentHdr = new qryBatchPaymentHdr
                {
                    PONo = _POhdr.PONo,
                    PayeeName = _tblVendor.DisplayName,
                    Amount = _qryBatchPaymentDtlList.Sum(a => a.Amount),
                    SalesInvoiceNo = string.Empty,
                    SalesInvoiceDate = Convert.ToDateTime("1900/01/01"),
                    DeliveryNo = qryPaymentDeliveries.Select(a => a.DRNo).FirstOrDefault(),
                    DeliveryDate = qryPaymentDeliveries.Select(a => a.DeliveryDate).FirstOrDefault(),
                    ReferenceReceiptNo = string.Empty,
                    HPDeduction = 0,
                    FreightAmount = 0
                };

                _BatchUploadParams.qryBatchPaymentHdrList.Add(_qryBatchPaymentHdr);
                _BatchUploadParams.qryBatchPaymentDtlList.AddRange(_qryBatchPaymentDtlList);
                _BatchUploadParams.UserID = "PISPLPI19181";
                //Auto approve of PO

                //Create Payment

                var result = await wacontroller.InsertBatchPaymentList(_BatchUploadParams);

                if (result is OkObjectResult okResult)
                {
                    // Cast the value to the expected model
                    _BatchUploadParams = okResult.Value as BatchUploadParams;

                    if (_BatchUploadParams != null)
                    {
                        foreach (TblRequisitionhdr item in _BatchUploadParams.TblRequisitionhdrList)
                        {
                            if (!Directory.Exists(Path.Combine(UploadingPathPR, item.Reqno)))
                            {
                                Directory.CreateDirectory(Path.Combine(UploadingPathPR, item.Reqno));
                            }

                            System.IO.File.Copy(SourceFile, Path.Combine(UploadingPathPR, item.Reqno, Path.GetFileName(SourceFile)));
                        }
                    }
                }
                 
                return Ok(_BatchUploadParams);
            }
            catch (Exception ex)
            {
                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(_BatchUploadParams.TblRequisitionhdrList.Select(a => a.BatchNo).FirstOrDefault(), "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);
            }
        }

        [HttpPost("UpdateReqDetails")]
        public async Task<TblResponse> UpdateReqDetails(qryUpdateReqDetails qryUpdateReqDetails)
        {
            try
            {
                _response = new TblResponse();
                _response = await _RepositoryUnit.TblPaymentRequestAuthRepository.UpdatePayment(qryUpdateReqDetails.ReqNo, "ENCODED", "AP", DateTime.Now, 0, qryUpdateReqDetails.UserCode);
                //_response = await _RepositoryUnit.PaymentrequisitionhdrRepository.UpdateDetailsByReqNo(qryUpdateReqDetails);
                _response = await _RepositoryUnit.TblDRNoRepository.UpdateDetailsByReqNo(qryUpdateReqDetails.ReqNo,qryUpdateReqDetails.SINo);
                _response = await _RepositoryUnit.TblRequisitionHdrRepository.UpdateDetailsByReqNo(qryUpdateReqDetails);
                
            }
            catch (Exception ex)
            {
                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(qryUpdateReqDetails.ReqNo, "FAILED", ex.Message, Utilities.GetmethodName());
                return _response;
            }

            return _response;
        }

        [HttpPost("CanUpdateReqDetails")]
        public async Task<TblResponse> CanUpdateReqDetails(qryUpdateReqDetails qryUpdateReqDetails)
        {
            try
            {
                _response = new TblResponse();
                //_response = await _RepositoryUnit.TblPaymentRequestAuthRepository.UpdatePayment(qryUpdateReqDetails.ReqNo, "ENCODED", "AP", DateTime.Now, 0, qryUpdateReqDetails.UserCode);
                ////_response = await _RepositoryUnit.PaymentrequisitionhdrRepository.UpdateDetailsByReqNo(qryUpdateReqDetails);
                //_response = await _RepositoryUnit.TblDRNoRepository.UpdateDetailsByReqNo(qryUpdateReqDetails.ReqNo, qryUpdateReqDetails.SINo);
                //_response = await _RepositoryUnit.TblRequisitionHdrRepository.UpdateDetailsByReqNo(qryUpdateReqDetails);
                _response.ErrorMessage = await _PRRules.CanUpdateRequisition(qryUpdateReqDetails);
                _response.TrxNo = qryUpdateReqDetails.ReqNo;
                _response.UniqueInfo = qryUpdateReqDetails.ReqNo;
                _response.Status = "SUCCESS";
                _response.MethodName = "Can Update Requisition Details";
                _response.AuditDate = DateTime.Now;

                if (!string.IsNullOrEmpty(_response.ErrorMessage))
                {
                    _response.Status = "FAILED";
                }

            }
            catch (Exception ex)
            {
                logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(qryUpdateReqDetails.ReqNo, "FAILED", ex.Message, Utilities.GetmethodName());
                _response.ErrorMessage = ex.Message;
                _response.Status = "ERROR";
                return _response;
            }

            return _response;
        }

    }
}
