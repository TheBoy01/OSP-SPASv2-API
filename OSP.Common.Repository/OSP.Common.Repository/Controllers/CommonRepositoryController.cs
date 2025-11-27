using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OSP.Common.Domain.Params;
using OSP.Common.Domain.References;
using OSP.Common.Domain.Tables;
using OSP.Common.Repository.Context;
using OSP.Common.Repository.Repository;
using OSP.Common.Repository.Rules;
using OSP.Common.Repository.Utility;
using OSP.SPASv2.Domain.Params;

namespace OSP.Common.Repository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonRepositoryController : ControllerBase
    {
        private readonly JWTAuthenticationManager jwtAuthenticationManager;

        RepositoryUnit _RepositoryUnit;
        private ILogger<CommonRepositoryController> logger;
        private OSPContext context;

        TblResponse _response = new TblResponse();
        //string _validmessage;
        DeptartmentRules _DeptartmentRules;

        public CommonRepositoryController(ILogger<CommonRepositoryController> _logger, OSPContext _context, JWTAuthenticationManager _jwt)
        {
            logger = _logger;
            this.context = _context;
            _RepositoryUnit = new RepositoryUnit(_context);
            this.jwtAuthenticationManager = _jwt;
            _DeptartmentRules = new DeptartmentRules(_context);
        }

        [HttpGet("GetEmployeeDetails")]
        public async Task<IActionResult> GetEmployeeDetails(string personid)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.EmployeeRepository.GetEmployeeDetails(personid);

                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
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

        [HttpGet("GetBranchesByPersonID")]
        //public async Task<ActionResult<IList<RefBranch>>> GetBranches(string branchdesc, string companydesc)
        public async Task<ActionResult<IList<RefBranch>>> GetBranchesByPersonID(string personid)
        {
            try
            {
                logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");
                //string companycode = await _RepositoryUnit.RefCompanyRepository.GetCompanycode(companydesc);


                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.RefBranchRepository.GetBranchesByPersonID(personid);
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

        [HttpGet("GetChapelsByPersonID")]
        public async Task<ActionResult<IList<RefChapel>>> GetChapelsByPersonID(string personid)
        {
            try
            {
                logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");
                //string companycode = await _RepositoryUnit.RefCompanyRepository.GetCompanycode(companydesc);


                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.RefChapelRepository.GetChapelsByPersonID(personid);
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

        [HttpGet("GetDeptByPersonID")]
        public async Task<ActionResult<IList<RefDepartments>>> GetDeptByPersonID(string personid, string companytype)
        {
            try
            {
                logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");
                //string companycode = await _RepositoryUnit.RefCompanyRepository.GetCompanycode(companydesc);


                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.RefDepartmentRepository.GetDeptByPersonID(personid, companytype);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");




                return new List<RefDepartments>(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return null;

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

        [HttpGet("GetBranchdetails")]
        public async Task<IActionResult> GetBranchdetails(string companydesc, string branchcode)
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


        [HttpGet("GetCompanyCode")]
        public async Task<IActionResult> GetCompanyCode(string DeptDesc, string CompanyType)
        {
            try
            {
                var result = await _RepositoryUnit.RefCompanyRepository.CompanyCode(DeptDesc, CompanyType);
                //var str = result.PRNo;
                return Ok(result);
            }
            catch (Exception ex)
            {
                //string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                //logger.LogError(ex, error);
                //return NotFound();
                return Ok(ex.Message.ToString());
            }
        }

        [HttpPost("CreateNotification")]
        public async Task<IActionResult> CreateNotification(TblNotification _tblnotification)
        {
            try
            {
                var result = await _RepositoryUnit.NotificationRepository.CreateNotification(_tblnotification);
                //var str = result.PRNo;

                _response = result;

                return Ok(result);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.InnerException.Message;
                logger.LogError(ex, error);

                _response.ErrorMessage = error;
                return Ok(ex.Message.ToString());
            }
        }

        [HttpPost("CreateAllNotif")]
        public async Task<IActionResult> CreateAllNotif(OSPParams oSPParams)
        {
            try
            {
                foreach (var item in oSPParams.TblNotificationList)
                {
                    await _RepositoryUnit.NotificationRepository.CreateNotification(item);
                }

                foreach (var item1 in oSPParams.tblSendemaildtlsList)
                {
                    await _RepositoryUnit.SendemaildtlRepository.Create(item1);
                }
               
                //var str = result.PRNo;

                oSPParams.TblResponse = new TblResponse() { Status="SUCESS",ErrorMessage="NOTIFICATIONS SUCCESSFULLY CREATED."};

                return Ok(oSPParams);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.InnerException.Message;
                logger.LogError(ex, error);

                _response.ErrorMessage = error;
                return Ok(ex.Message.ToString());
            }
        }

        [HttpGet("GetCompanyDetails")]
        public async Task<IActionResult> GetCompanyDetails(string CompanyType, string DeptCode)
        {
            try
            {
                logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");
                //string companycode = await _RepositoryUnit.RefCompanyRepository.GetCompanycode(companydesc);

                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var _qryCompanyDetails = await _RepositoryUnit.RefCompanyRepository.GetCompanyDetails(CompanyType, DeptCode);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");

                return Ok(_qryCompanyDetails);
            }
            catch (Exception ex)
            {
                //string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                //logger.LogError(ex, error);
                //return null;
                return Ok(ex.Message.ToString());
            }
        }

        [HttpPost("GetCompanyDetails1")]
        public async Task<IActionResult> GetCompanyDetails1(qryCompanyDetails qryCompanyDetails)
        {
            try
            {
                logger.LogInformation("Fetching - GetCompanycode - " + Utilities.GetmethodName() + "");
                //string companycode = await _RepositoryUnit.RefCompanyRepository.GetCompanycode(companydesc);

                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var _qryCompanyDetails = await _RepositoryUnit.RefCompanyRepository.GetCompanyDetails(qryCompanyDetails.CompanyType, qryCompanyDetails.DeptCode);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");

                return Ok(_qryCompanyDetails);
            }
            catch (Exception ex)
            {
                //string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                //logger.LogError(ex, error);
                //return null;
                return Ok(ex.Message.ToString());
            }
        }

        [HttpGet("GetCompanyType")]
        public async Task<IActionResult> GetCompanyType(string CompanyCode)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.RefCompanyRepository.GetCompanyType(CompanyCode);

                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }
        }

        [HttpGet("GetCompanyDescByCode")]
        public async Task<IActionResult> GetCompanyDescByCode(string CompanyCode)
        {
            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.RefCompanyRepository.GetCompanyDescByCode(CompanyCode);

                return Ok(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }
        }


        [HttpPost("InfoTxtSendSMS")]
        public async Task<IActionResult> SaveToOutbox(InfoTextParams InfoTextParams)
        {
            try
            {
                logger.LogInformation("Sending SMS to " + InfoTextParams.MobileNo + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.TblOutboxRepository.SaveToOutbox(InfoTextParams);
                TblOutbox _tblOutbox = new TblOutbox();

                do
                {
                    _tblOutbox = new TblOutbox();
                    _tblOutbox = await _RepositoryUnit.TblOutboxRepository.GetRecentMsgStatus(InfoTextParams);
                    logger.LogInformation("Message is now on Queue. Waiting to be sent to " + _tblOutbox.COMNum + Utilities.Getprojectname() + " -  " + Utilities.GetmethodName() + " ");
                    //_IMainView.BGPercentage = 60; 
                    switch (_tblOutbox.Status)
                    {
                        case 1:
                            logger.LogInformation("Message sent to " + _tblOutbox.COMNum + Utilities.Getprojectname() + " -  " + Utilities.GetmethodName() + " ");

                            break;

                        case 16:
                            logger.LogInformation("Message sending failed to " + _tblOutbox.COMNum + Utilities.Getprojectname() + " -  " + Utilities.GetmethodName() + " ");
                            _response.ErrorMessage = "Message updated to 16. Failed to send. ";
                            break;
                    }

                } while (_tblOutbox.Status.Equals(0));

                return Ok(result);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return NotFound();
            }
        }


        [HttpGet("GetRecipient")]
        public async Task<IList<TblRecipient>> GetRecipient(string systemcode)
        {

            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.NotificationRepository.GetRecipient(systemcode);
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

        [HttpGet("GetTblEmployee")]
        public async Task<IActionResult> GetTblEmployee(string personid)
        {

            try
            {
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var vlist = await _RepositoryUnit.EmployeeRepository.GetEmployeeDetails(personid);
                logger.LogInformation("Success - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                return Ok(vlist);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return BadRequest(ex.Message);

            }
        }


        //[HttpPost("GetRecentMsgStatus")]
        //public async Task<IActionResult> GetRecentMsgStatus(InfoTextParams InfoTextParams)
        //{
        //    try
        //    {
        //        logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
        //        var result = await _RepositoryUnit.TblOutboxRepository.GetRecentMsgStatus(InfoTextParams, "SPLPIS2");

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {

        //        string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
        //        logger.LogError(ex, error);
        //        return NotFound();
        //    }
        //}


        [HttpPost("SPASv2CanUploadExcelDetails")]
        public async Task<IActionResult> SPASv2CanUploadExcelDetails(BatchUploadParams BatchUploadParams)
        {
            try
            {
             List< qryCompanyDetails> qryCompanyDetailsList = new List< qryCompanyDetails>();
                qryCompanyDetails qryCompanyDetails = new qryCompanyDetails();
                foreach (var item in BatchUploadParams.qryBatchRequistions)
                {
                    qryCompanyDetails.CompanyType = item.CompanyType;
                    qryCompanyDetails.DeptCode = item.Department;
                    qryCompanyDetailsList.Add(qryCompanyDetails);
                }
                var canread = await _DeptartmentRules.CanReadAsyncList(qryCompanyDetailsList);
                if (!string.IsNullOrEmpty(canread))
                {
                    BatchUploadParams.TblResponse.Status = "FAILED";
                    BatchUploadParams.TblResponse.ErrorMessage = canread;
                    return Ok(BatchUploadParams);
                }
                return Ok(BatchUploadParams);

            }
            catch (Exception ex)
            {
                //logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse("", "FAILED", ex.Message, Utilities.GetmethodName());
                BatchUploadParams.TblResponse = _response;
                return BadRequest(BatchUploadParams);
            }
        }


        [HttpGet("GetAllCompanyDetails")]
        public async Task<IActionResult> GetAllCompanyDetails()
        {
            try
            {
                var compdtls = await _RepositoryUnit.RefDepartmentRepository.GetAllCompanyDetails();
                return Ok(compdtls);

            }
            catch (Exception ex)
            {
                //logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse("", "FAILED", ex.Message, Utilities.GetmethodName());
               
                return BadRequest(_response);
            }
        }

        [HttpGet("GetEmailPerChapel")]
        public async Task<IActionResult> GetEmailPerChapel(string chapelcode)
        {
            try
            {
                RefChapelEmail chapelemail = await _RepositoryUnit.RefDepartmentRepository.GetEmailPerChapel(chapelcode);
                return Ok(chapelemail);

            }
            catch (Exception ex)
            {
                //logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse(chapelcode, "FAILED", ex.Message, Utilities.GetmethodName());

                return BadRequest(_response);
            }
        }

        [HttpPost("GetEPPSDetails")]
        public async Task<IActionResult> GetEPPSDetails(OSPParams OSPParams)
        {
            OSPParams.TblResponse  = new TblResponse();
            try
            {
                OSPParams.TblRecipientList  = await _RepositoryUnit.NotificationRepository.GetRecipient("SPASv2");
                OSPParams.qryEmployeeList = await _RepositoryUnit.EmployeeRepository.GetEmployeeList(OSPParams.PersonIdList);
                
                
                
                OSPParams.TblResponse.ErrorMessage = "SUCCESS FETCH!";
                OSPParams.TblResponse.Status = "SUCCESS";
                return Ok(OSPParams);

            }
            catch (Exception ex)
            {
                //logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse("", "FAILED", ex.Message, Utilities.GetmethodName());
                OSPParams.TblResponse = _response;
                return BadRequest(OSPParams);
            }
        }

        [HttpGet("companies")]
        public async Task<List<RefCompany>> companies()
        {
            
            try
            {
                return await _RepositoryUnit.RefCompanyRepository.companies();


            }
            catch (Exception ex)
            {
                //logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse("", "FAILED", ex.Message, Utilities.GetmethodName());
                //OSPParams.TblResponse = _response;
                throw new Exception(error);
                //return _response;
            }
        }

        [HttpPost("GetNotifications")]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                OSPParams oSPParams = new OSPParams();
                oSPParams.TblResponse = _response;
                oSPParams.TblNotificationList = new List<TblNotification>();
                oSPParams.tblSendemaildtlsList = new List<TblSendemaildtl>();
                oSPParams.TblNotificationList = await _RepositoryUnit.NotificationRepository.GetNotifications();
                oSPParams.tblSendemaildtlsList = await _RepositoryUnit.SendemaildtlRepository.GetAllPending();

                return Ok(oSPParams);

            }
            catch (Exception ex)
            {
                //logger.LogError("Error - " + Utilities.Getprojectname + " - " + Utilities.GetCallingMethodName() + " - " + Request.Path.Value + " - " + ex.Message);
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                _response = await _RepositoryUnit.ResponseRepository.CreateResponse("", "FAILED", ex.Message, Utilities.GetmethodName());

                return BadRequest(_response);
            }
        }
    }
}
