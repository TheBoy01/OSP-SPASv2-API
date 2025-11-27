using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OSP.SPASv2.Service.Model;
using OSP.SPASv2.Service;
using OSP.SPASv2.Domain;

using OSP.SPASv2.Domain.View;
using OSP.SPASv2.Service.Services;
using System.Text.RegularExpressions;
using OSP.SPASv2.Domain.Params;
using OSP.Common.Domain.Tables;
using OSP.SPASv2.Service.Utility;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace OSP.SPASv2.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthorizationController :  ControllerBase
    {

        ServiceUnit _ServiceUnit;
        private ILogger<AuthorizationController> _logger;
        private TblResponse _response;

        public AuthorizationController(ILogger<AuthorizationController> logger)
        {
            //this.jwtAuthenticationManager = jwtAuthenticationManager;
            _ServiceUnit = new ServiceUnit();
            _logger = logger;
            _response = new TblResponse();

        }

      




    }
}
