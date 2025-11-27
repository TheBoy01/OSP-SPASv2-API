using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OSP.Common.Domain.Tables;
using OSP.SPASv2.Web.Models;
using OSP.SPASv2.Web.Utility;
using System;
using System.Data;
using System.Dynamic;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static OSP.SPASv2.Web.Utility.Utilities;
using static System.Data.Odbc.ODBC32;

namespace OSP.SPASv2.Web.APIServices.Repository
{
    public class EmployeeRepository
    {
        public async Task<qryEmployee> GetEmployeeDetails(string URLCommonRepo, string personid)
        {
            string requestAddress = URLCommonRepo + "/CommonRepository/GetEmployeeDetails";
            var query = new Dictionary<string, string>()
            {
                ["personid"] = personid,
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            qryEmployee empdetails = await UtilitiesHttpClient<qryEmployee>.GetJsonlist1(requestAddress);
            return empdetails;
        }
    }
}
