using Microsoft.AspNetCore.Mvc;
using OSP.Common.Service.Service;
using OSP.Common.Service.Utility;
using OSP.Common.Domain.Tables;
using OSP.Common.Service.ServiceContract;
using OSP.Common.Service.OperationContract;
//using OSP.SPASv2.Domain.Tables;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSP.Common.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        private ServiceUnit _ServiceUnit;
        private ILogger _logger;
        private string errorMessage;
        private readonly ISendEmailService<TblSendEmail> _mailService;

        public SendEmailController(ILogger<SendEmailController> logger, ISendEmailService<TblSendEmail>    mailService)
        {
            _logger = logger;


            _ServiceUnit = new ServiceUnit();
            _mailService = mailService;
        }

        //[HttpGet("SendEmail")]
        //public async Task<ActionResult > SendEmail()
        //{
        //    try
        //    {
        //        _logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " -  "+ Utilities.GetmethodName() + " ");

        //        _ServiceUnit.SendEmailService.SendEmail();

        //        _logger.LogInformation("Success - " + Utilities.Getprojectname() + " -  " + Utilities.GetmethodName() + " ");
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {

        //        string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
        //        _logger.LogError(ex, error);
        //        return Ok(errorMessage);
        //    }

        //}
        TblResponse _response = new TblResponse();
        [HttpPost("SendEmail")]
        public async Task<TblResponse> SendEmail(TblSendEmail _tblSendEmail)
        {
            try
            {
                _logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " -  " + Utilities.GetmethodName() + " ");
                _response =  await _ServiceUnit.SendEmailService.SendEmailAsync(_tblSendEmail); 
                _logger.LogInformation("Success - " + Utilities.Getprojectname() + " -  " + Utilities.GetmethodName() + " ");

                _response.Status = "SUCCESS";
                _response.ErrorMessage = "Success Email";
                return _response;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                _logger.LogError(ex, error);

                _response.Status = "FAILED";
                _response.ErrorMessage = "Email Failed : " + ex.Message;
                return _response;
            }
        }


        [HttpPost("SendEmailMultiple")]
        public async Task<TblResponse> SendEmailMultiple(List<TblSendEmail> _tblSendEmail)
        {
            try
            {
                foreach (var item in _tblSendEmail)
                {
                    _logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " -  " + Utilities.GetmethodName() + " ");
                    _response = await _ServiceUnit.SendEmailService.SendEmailAsync(item);
                    _logger.LogInformation("Success - " + Utilities.Getprojectname() + " -  " + Utilities.GetmethodName() + " ");
                }       
                _response.Status = "SUCCESS";
                _response.ErrorMessage = "Success Email";
                return _response;
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                _logger.LogError(ex, error);

                _response.Status = "FAILED";
                _response.ErrorMessage = "Email Successfully Failed";
                return _response;
            }
        }







    }
}
