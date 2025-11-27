using Microsoft.AspNetCore.Mvc;
using OSP.Common.Service.Service;
using OSP.Common.Service.Utility;
using OSP.Common.Domain.Tables;
using OSP.Common.Domain.Params;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSP.Common.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendSMSController : ControllerBase
    {
        private ServiceUnit _ServiceUnit;
        private ILogger _logger;
        private string errorMessage;
        private string requestAddress;
        string OSPUrlRepo;
        private IConfiguration _configuration;

        public SendSMSController(ILogger<SendEmailController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _ServiceUnit = new ServiceUnit();
            _configuration = configuration;
            OSPUrlRepo = _configuration.GetSection("APIBaseURLCommon")["Common.Repository"];
        }

        TblResponse _response = new TblResponse();
        [HttpPost("SendSMS")]
        public async Task<TblResponse> SendSMS(TblSendSMS _TblSendSMS)
        {
            try
            {
                TblSendSMSYondu _TblSendSMSYondu = new TblSendSMSYondu();
                _TblSendSMSYondu.Receiver = "09954487592";
                _TblSendSMSYondu.Message = _TblSendSMS.Message + DateTime.Now.ToString();
                _logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " -  " + Utilities.GetmethodName() + " ");
                _ServiceUnit.SendSMSService.SendSMSYondu(_TblSendSMSYondu);
                _logger.LogInformation("Success - " + Utilities.Getprojectname() + " -  " + Utilities.GetmethodName() + " ");

                return _response;
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                _logger.LogError(ex, error);
                return _response;
            }
        }
   


    }
}
