using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Domain.View;
using OSP.SPASv2.Repository.Repository.MainRepository;
using SPASv2.Context;

namespace OSP.SPASv2.Repository.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly JWTAuthenticationManager jwtAuthenticationManager;
        RepositoryUnit _RepositoryUnit;
        private ILogger<VendorController> _logger;
        private SPASv2Context context;

        #region Private Member Variables

        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public VendorController(SPASv2Context _context, JWTAuthenticationManager _jwt)
        {
            //UtilityFoo._SuperLogger = logger;
            this.context = _context;
            _RepositoryUnit = new RepositoryUnit(_context);
            this.jwtAuthenticationManager = _jwt;
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        //[Authorize]
        //[HttpGet("GetVendorList")]
        //public async Task<ActionResult<IList<qryVendorList>>> GetVendorList()
        //{
        //    var vlist = await _RepositoryUnit.VendorRepository.GetVendorList();
        //    return new List<qryVendorList>(vlist);
        //}

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

        #endregion

        #region Public Functions

        [HttpGet("DoFoo")]
        public void DoFoo()
        {
            try
            {
               // UtilityFoo._SuperLogger1.LogInformation("Hello Vendor1");

            }
            catch (Exception ex)
            {
                //UtilityFoo._SuperLogger1.LogError(ex,"ASDF");
                throw;
            }
            
        }
        #endregion

    }
}

