
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using OSP.Common.Domain.References;
using OSP.SPASv2.Domain.References;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Repository.Repository.MainRepository;
using SPASv2.Context;
using OSP.SPASv2.Repository.Utility;
using OSP.SPASv2.Repository.Rules;
using OSP.SPASv2.Repository.Parameters;
using OSP.SPASv2.Domain.View;
using System;



//using OSP.SPASv2.Domain.View;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


//using System.Net;
//using OSP.SPASv2.Domain;
//using OSP.SPASv2.Repository.Repository;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSP.SPASv2.Repository.Controllers
{
    //[Authorize]
    //[ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    [ApiController]
    public class RepositoryController : ControllerBase
    {
        private readonly JWTAuthenticationManager jwtAuthenticationManager;

        RepositoryUnit _RepositoryUnit;
        private ILogger<RepositoryController> logger;
        private SPASv2Context context;
        TblResponse _response = new TblResponse();
        string _validmessage;
        
        public RepositoryController(ILogger<RepositoryController> _logger, SPASv2Context _context, JWTAuthenticationManager _jwt)
        {
            logger = _logger;
            this.context = _context;
            _RepositoryUnit = new RepositoryUnit(_context);
            this.jwtAuthenticationManager = _jwt;
 
        }

        ////UtilityFoo._SuperLogger.LogInformation("Hello Doc");
        //tblvendor = new TblVendor();
        //        {
        //            tblvendor.VendorCode = "TEMP000024";
        //            tblvendor.VendorType = "MARIA ASWANG";
        //            tblvendor.DisplayName = "LUZVIMOW";
        //            tblvendor.LastName = "FFFFFF";
        //            tblvendor.MiddleName = "ZAAFFFFWDCH";
        //            tblvendor.FirstName = "FFFFF";
        //            tblvendor.Active = false;
        //            tblvendor.AuditUser = "ORONOMF";
        //            tblvendor.AuditDate = DateTime.Now;
        //        }

        VendorRules vrules = new VendorRules();
        VendorParams _Vendorparams = new VendorParams();


        #region Boi
        
        #endregion

        #region Wa

        [HttpGet("GetCompanyTypes")]
        public async Task<IActionResult> GetCompayTypes(string company)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.RefCompanyRepository.GetCompanyTypes(company);

                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }
        }


        [HttpGet("GetCompanyTypesAccess")]
        public async Task<IActionResult> GetCompanyTypesAccess(string personid)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.RefCompanyRepository.GetCompanyTypesAccess(personid);

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

        [HttpGet("GetVendorItemsList1")]
        public async Task<IActionResult> GetVendorItemsList1(string vendorcode,string itemdesc)
        {
            try
            {


                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.VendorItemsRepository.GetVendorItemsList1(vendorcode,itemdesc);

                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }

        }

        [HttpGet("GetVendorItemsDetails")]
        public async Task<IActionResult> GetVendorItemsDetails(string vendorcode, string itemcode)
        {
            try
            {


                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.VendorItemsRepository.GetVendorItemsDetails(vendorcode, itemcode);

                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }

        }

        [HttpGet("GetPaymentTypeList")]
        public async Task<IList<RefPaymentClass>> GetPaymentTypeList()
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

        

        [HttpGet("PaymentClassDetails")]
        public async Task<RefPaymentClass> PaymentClassDetails(string PayClassCode)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.RefPaymentTypeRepository.GetPaymentClass(PayClassCode);
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

        [HttpGet("GetBranchlist")]
        public async Task<ActionResult<IList<RefBranch>>> GetBranchlist()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.RefBranchRepository.GetBranchlist();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return new List<RefBranch>(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }

        }

        [HttpGet("GetLifeplanBranches")]
        public async Task<ActionResult<IList<RefBranch>>> GetLifeplanBranches(string branchdesc, string companydesc)
        {
            try
            {
                logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");
                string companycode = await _RepositoryUnit.RefCompanyRepository.GetCompanycode(companydesc);


                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.RefBranchRepository.GetBranches(branchdesc, companycode);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");


                return new List<RefBranch>(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }

        }



        [HttpGet("SearchCompany")]
        public async Task<IActionResult> SearchCompany(string company)
        {
            logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");

            var result = await _RepositoryUnit.RefCompanyRepository.GetCompanylist(company);
            if (!result.Any())
            {
                return NotFound(company);
            }
            return Ok(result);
        }

        [HttpGet("GetBranchdetails")]
        public async Task<IActionResult> GetBranchdetails(string companydesc,string branchcode)
        {
            try
            {
                logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");
                //string companycode = await _RepositoryUnit.RefCompanyRepository.GetCompanycode(companydesc );

                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.RefBranchRepository.GetBranchdetails(companydesc, branchcode);
                //if (!result.Any())
                //{
                //    return NotFound(company);
                //}
                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;
            }
            
        }

        [HttpGet("GetBranches")]
        public async Task<ActionResult<IList<RefBranch>>> GetBranches(string branchdesc, string companydesc)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                //string companycode = await _RepositoryUnit.RefCompanyRepository.GetCompanycode(companydesc);


                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.RefBranchRepository.GetBranches("", companydesc);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");




                return new List<RefBranch>(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }

        }

        [HttpGet("GetChapels")]
        public async Task<ActionResult<IList<RefChapel>>> GetChapels(string chapeldesc, string companydesc)
        {
            try
            {
                logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");
                string companycode = await _RepositoryUnit.RefCompanyRepository.GetCompanycode(companydesc);


                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.RefChapelRepository.GetChapels(chapeldesc, companycode);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");




                return new List<RefChapel>(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }

        }

        [HttpGet("GetCompanylist")]
        public async Task<ActionResult<IList<RefCompany>>> GetCompanylist(string company)
        {
            try
            {
                //  company = "st. pete";
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.RefCompanyRepository.GetCompanylist(company);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return new List<RefCompany>(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }

        }

        [HttpGet("GetCompanylist1")]
        public async Task<ActionResult<IList<RefCompany>>> GetCompanylist1()
        {
            try
            {
                //  company = "st. pete";
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.RefCompanyRepository.GetCompanylist1();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return new List<RefCompany>(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }

        }

        [HttpGet("GetCompanies")]
        public async Task<IList<RefCompany>> GetCompanies()
        {
            try
            {
                //  company = "st. pete";
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.RefCompanyRepository.GetCompanies();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return new List<RefCompany>(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }

        }

        [HttpGet("GetCompanyDesc")]
        public async Task<string> GetCompanyDesc(string companycode)
        {
            try
            {
                //  company = "st. pete";
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var companydesc = await _RepositoryUnit.RefCompanyRepository.GetCompanyDesc(companycode);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return companydesc;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }

        }


        [HttpGet("GetCompanycode")]
        public async Task<string> GetCompanycode(string company)
        {
            try
            {
                //  company = "st. pete";
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var companycode = await _RepositoryUnit.RefCompanyRepository.GetCompanycode(company);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return companycode;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }

        }
        //}
        //}

        //        }

        [HttpGet("GetVendorLists")]
        public async Task<ActionResult<IList<qryVendorList>>> GetVendorLists()
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
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }
        }

        [HttpGet("GetVendorLists1")]
        public async Task<ActionResult<IList<qryVendorList>>> GetVendorLists1(string vendordesc,string paymentclass)
        {
            try
            {

                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.VendorRepository.GetVendorLists1(vendordesc, paymentclass);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return new List<qryVendorList>(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }

        }

        [HttpGet("GetVendorAccttype")]
        public async Task<ActionResult<IList<TblVendorpaymethod>>> GetVendorAccttype()
        {
            try
            {

                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.VendorbankaccountRepository.GetVendorAccttype();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return new List<TblVendorpaymethod>(vlist);
            }
            catch (Exception ex) 
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }

        }

        [HttpGet("GetVendorAcctNo")]
        public async Task<ActionResult<IList<TblVendorpaymethod>>> GetVendorAcctNo(string vendorcode)
        {
            try
            {

                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.VendorbankaccountRepository.GetVendorAcctNo(vendorcode);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return new List<TblVendorpaymethod>(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }

        }

        [HttpGet("GetVendorAcctNo1")]
        public async Task<ActionResult<TblVendorpaymethod>> GetVendorAcctNo1(string vendorcode)
        {
            try
            {

                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.VendorbankaccountRepository.GetVendorAcctNo1(vendorcode);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return  vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

            }

        }

        [HttpGet("GetVendorDetails")]
        public async Task<qryVendorDetails> GetVendorDetails(string vendorcode,string payclass)
        {
            try
            {

                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.VendorRepository.GetVendorDetails(vendorcode, payclass);
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

        #endregion

        #region Doc
        #endregion

        #region Dumz
        #endregion


        #region Jon
        [HttpGet("GetSaveLastDate")]
        public async Task<DateTime> GetSaveLastDate()
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                DateTime vlist = await _RepositoryUnit.RefSystemsRepository.GetSaveLastDate();
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return vlist;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return Convert.ToDateTime(("01/01/1900"));

            }
        }
        #endregion

    }


}
