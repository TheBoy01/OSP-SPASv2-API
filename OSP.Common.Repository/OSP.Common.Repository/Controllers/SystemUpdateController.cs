using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OSP.Common.Domain.APIFactory.OSPService;
using OSP.Common.Repository.Context;
using OSP.Common.Repository.Repository;

using OSP.Common.Repository.Utility;
using OSP.SPASv2.Domain.View;
using System.Security.Policy;
using static System.Net.WebRequestMethods;


namespace OSP.Common.Repository.Controllers
{

    [ApiController]
    public class SystemUpdateController : Controller
    {
        private readonly JWTAuthenticationManager jwtAuthenticationManager;

        RepositoryUnit _RepositoryUnit;
        private ILogger<SystemUpdateController> logger;
        private OSPContext context;

        TblResponse _response = new TblResponse();
        string _validmessage;

        private readonly IHttpOSPService _IHttpOSPService;

        public SystemUpdateController(ILogger<SystemUpdateController> _logger, OSPContext _context, JWTAuthenticationManager _jwt, IHttpOSPService IHTTPSOSPService)
        {
            logger = _logger;
            _context = context;
            _context = DbContextFactory.Create("SPLPDEVSERVER");
            this.context = _context;
            _RepositoryUnit = new RepositoryUnit(_context);
            this.jwtAuthenticationManager = _jwt;

            _IHttpOSPService = IHTTPSOSPService;
        }


        [HttpPost]
        [Route("api/SystemUpdate/CreateUpdateDtl")]
        public async Task<SytemUpdateResponse> Create(TblSystemUpdateDtl _tblSystemUpdateDtl)
        {
            SytemUpdateResponse _Response = new SytemUpdateResponse();
            try
            {
                //context.TblSystemUpdateDtl.Add(_tblSystemUpdateDtl);
                TblSystemUpdateDtl emp = await context.TblSystemUpdateDtl.FindAsync(_tblSystemUpdateDtl.UpdateCode, _tblSystemUpdateDtl.DepartmentCode);
                TblSystemUpdateDtl _old = emp;
                emp.isUpdated = _tblSystemUpdateDtl.isUpdated;
                emp.DepartmentUpdateDate = _tblSystemUpdateDtl.DepartmentUpdateDate;
                context.Entry(emp).State = EntityState.Modified;
                await context.SaveChangesAsync();

                _Response.ResponseMessage = "Success";
            }
            catch (Exception ex)
            {

                //throw new Exception(ex.Message);


                _Response.ResponseMessage = ex.Message;
                return _Response;
            }


            return _Response;

        }

        [HttpGet]
        [Route("sample01")]
        public async Task<string> sample()
        {

            return "SAMPLE OK";

        }

        [HttpGet]
        [Route("sample011")]
        public async Task<string> sample011()
        {
            var sample = await _IHttpOSPService.GetCompanies("api/OSPCommon/GetCompanies");
            string url  = "https://localhost:7090/api/OSPCommon/GetCompanies";
            string requestAddress = url  ;


            //requestAddress = Utilities.GetUrlWithQueryString(requestAddress);
            IList<qryCompanyType> vlist = await UtilitiesHttpClient<qryCompanyType>.GetJsonlist(requestAddress);

            //return vlist;

            return "SAMPLE OK";

        }

    }
}
