using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using OSP.SPASv2.Repository.Parameters;
using OSP.SPASv2.Repository.Repository.MainRepository;
using OSP.SPASv2.Repository.Rules;
using OSP.SPASv2.Repository.Utility;
using OSP.SPASv2.Web.APIServices.Services;
using SPASv2.Context;
using System.Web.Http.Results;

namespace OSP.SPASv2.Repository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarController : ControllerBase
    {
        private readonly JWTAuthenticationManager jwtAuthenticationManager;

        RepositoryUnit _RepositoryUnit;
        private ILogger<WaController> logger;
        private SPASv2Context context;
        //TblResponse _response = new TblResponse();
        TblResponse _response;
        string _validmessage;
        PaymentRequestParams _PaymentRequestParams = new PaymentRequestParams();
        PaymentRequestRules prrules;
        ServiceUnit ServiceUnit = new ServiceUnit();

        public WarController(ILogger<WaController> _logger, SPASv2Context _context, IConfiguration configuration)
        {
            logger = _logger;
            this.context = _context;
            _RepositoryUnit = new RepositoryUnit(_context);
            //this.jwtAuthenticationManager = _jwt;
            prrules = new PaymentRequestRules(_context);
            string config = configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");

        }

        [HttpGet("GetRefDiscount")]
        public async Task<IActionResult> GetRefDiscount()
        {
            try
            {
                
                //logger.LogInformation("Fetching - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var result = await _RepositoryUnit.RefDiscountRepository.GetRefDiscount();
                //var result = "HELLO WORLD!!";
                //var str = result.PRNo;
                return Ok(result);
            }
            catch (Exception ex)
            {

                //string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                //logger.LogError(ex, error);
                return Ok(ex.ToString());
            }

        }


        [HttpPost("CheckWa")]
        public async Task<IActionResult> CheckWa(qryCompany qry)
        {
            try
            {

                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var result = "HELLO WORLD!!";
                //var str = result.PRNo;
                return   Ok(result);
            }
            catch (Exception ex)
            {
                
                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return BadRequest(error);
            }

        }


        [HttpGet("Trycast")]
        public async Task<IActionResult> Trycast(DateTime sdate)
        {
            try
            {
                //throw new Exception("sample");
                logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + "");
                var _RefTrxweek = await _RepositoryUnit.ReftrxweekRepository.GetReftrxweek(sdate); 
                
                //var str = result.PRNo;
                return Ok(_RefTrxweek);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetCallingMethodName() + " - " + ex.Message;
                logger.LogError(ex, error);
                return BadRequest(error);
            }

        }
    }
}
