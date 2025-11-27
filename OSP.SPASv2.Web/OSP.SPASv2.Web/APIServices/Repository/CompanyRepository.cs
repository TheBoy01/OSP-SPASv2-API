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
    public class CompanyRepository
    {
        private string APIBaseURLRepo;
        private string APIBaseURLCommonRepo;
        //private IConfiguration _configuration;

        public CompanyRepository()
        {
            //APIBaseURLRepo = "http://192.168.23.185/SPASv2Repo/api";
            //APIBaseURLCommonRepo = "http://192.168.23.185/OSPRepo/api";
            //_configuration = new IConfiguration();
            //this.APIBaseURLRepo = _configuration.GetSection("APIBaseURL")["SPASv2.Repository"];
            //this.APIBaseURLCommonRepo = _configuration.GetSection("APIBaseURLCommon")["Common.Repository"];
              }

        public async Task<IList<RefCompany>> GetCompanylist(string url, string companydesc)
        {
            // string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Repository/GetCompanylist";
            //string requestAddress = "http://192.168.23.185:80/api/Repository/GetCompanylist";
            //string requestAddress = ip + "/api/Repository/GetCompanylist";
            //string requestAddress = APIBaseURLRepo + "/Repository/GetCompanylist";
            string requestAddress = url + "/Repository/GetCompanylist";
            var query = new Dictionary<string, string>()
            {
                ["company"] = companydesc,

            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

            IList<RefCompany> company = await UtilitiesHttpClient<RefCompany>.GetJsonlist(requestAddress);
            return company;


        }

        public async Task<IList<RefCompany>> GetCompanylist1(string url)
        {
            string requestAddress = url + "/Repository/GetCompanylist1";
            // string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Repository/GetCompanylist1";
            //string requestAddress = "http://192.168.23.185:80/api/Repository/GetCompanylist";
            //string requestAddress = ip + "/api/Repository/GetCompanylist";

            //var query = new Dictionary<string, string>()
            //{
            //    ["company"] = companydesc,

            //};

            //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

            IList<RefCompany> company = await UtilitiesHttpClient<RefCompany>.GetJsonlist(requestAddress);
            return company;


        }

        public async Task<IList<RefCompany>> GetCompanies(string url)
        {
            string requestAddress = url + "/Repository/GetCompanylist";
            // string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Repository/GetCompanylist";
            //string requestAddress = "http://192.168.23.185:80/api/Repository/GetCompanylist";
            //string requestAddress = ip + "/api/Repository/GetCompanylist";



            IList<RefCompany> company = await UtilitiesHttpClient<RefCompany>.GetJsonlist(requestAddress);
            return company;
        }

        public async Task<IList<RefCompany>> SearchCompany(string companydesc, string url)
        {
            string requestAddress = url + "/Repository/SearchCompany";
            //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Repository/SearchCompany";
            //string requestAddress = "http://192.168.23.185:80/api/Repository/GetCompanylist";
            //string requestAddress = ip + "/api/Repository/GetCompanylist";

            var query = new Dictionary<string, string>()
            {
                ["company"] = companydesc,

            };
            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            IList<RefCompany> company = await UtilitiesHttpClient<RefCompany>.GetJsonlist(requestAddress);
            return company;
        }

        public async Task<IList<qryCompanyType>> GetCompanyTypes(string company, string url)
        {
            string requestAddress = url + "/Repository/GetCompanyTypes";
            // string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Repository/GetCompanyTypes";
            //string requestAddress = "http://192.168.23.185:80/api/Repository/GetCompanyTypes";
            //string requestAddress = ip + "/api/Repository/GetCompanylist";

            var query = new Dictionary<string, string>()
            {
                ["company"] = company,

            };
            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            IList<qryCompanyType> companytype = await UtilitiesHttpClient<qryCompanyType>.GetJsonlist(requestAddress);
            return companytype;
        }

        public async Task<IList<qryCompanyType>> GetCompanyTypesAccess(string personid, string url)
        {
            //string requestAddress = "http://192.168.23.185:80/api/Repository/GetCompanyTypes";
            //string requestAddress = ip + "/api/Repository/GetCompanylist";

            string requestAddress = url + "/CommonRepository/GetCompanyTypesAccess";
            //  string requestAddress = "http://192.168.23.185/OSPRepo/api/CommonRepository/GetCompanyTypesAccess";

            var query = new Dictionary<string, string>()
            {
                ["personid"] = personid,
                //["personid1"] = "123",
                //["Normalize"] = false

            };

            // GenericModel<string, string> model1 = ModelBuilder1.BuildModel<string,string>(personid);


            //var genericModel = MyGenericClass.CreateGenericModel<object>(query);

            //var linearRegression = new LinearRegression(query);
            //GenericUtility<LinearRegression>.PerformAction(linearRegression);


            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            IList<qryCompanyType> companytype = await UtilitiesHttpClient<qryCompanyType>.GetJsonlist(requestAddress);
            return companytype;
        }

        public async Task<qryCompanyDetails> GetCompanyDetails(string URLCommonRepo, string CompanyType, string DeptCode)
        {
            //string requestAddress = "http://192.168.23.185:80/api/Repository/GetCompanyTypes";
            //string requestAddress = ip + "/api/Repository/GetCompanylist";

            string requestAddress = URLCommonRepo + "/CommonRepository/GetCompanyDetails";
            //  string requestAddress = "http://192.168.23.185/OSPRepo/api/CommonRepository/GetCompanyTypesAccess";

            var query = new Dictionary<string, string>()
            {
                ["CompanyType"] = CompanyType,
                ["DeptCode"] = DeptCode
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            qryCompanyDetails companytype = await UtilitiesHttpClient<qryCompanyDetails>.GetJsonlist1(requestAddress);
            return companytype;
        }

        public async Task<string> GetCompanyDescByCompanyCode(string URL, string companyCode)
        {
            try
            {
                string requestAddress = URL + "/CommonRepository/GetCompanyDescByCode";
                //  string requestAddress = "http://192.168.23.185/OSPRepo/api/CommonRepository/GetCompanyTypesAccess";

                var query = new Dictionary<string, string>()
                {
                    ["companyCode"] = companyCode,
                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                return await UtilitiesHttpClient<string>.GetJsonstring(requestAddress);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetCompanyType(string URL, string companyCode)
        {
            try
            {
                string requestAddress = URL + "/CommonRepository/GetCompanyType";
                //  string requestAddress = "http://192.168.23.185/OSPRepo/api/CommonRepository/GetCompanyType";

                var query = new Dictionary<string, string>()
                {
                    ["companyCode"] = companyCode,
                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                return await UtilitiesHttpClient<string>.GetJsonstring(requestAddress);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class GenericModel<T>
    {
        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        public T GetValue(string propertyName)
        {
            if (_properties.ContainsKey(propertyName))
            {
                return (T)_properties[propertyName];
            }
            else
            {
                throw new KeyNotFoundException($"Property '{propertyName}' not found in GenericModel.");
            }
        }

        public void SetValue(string propertyName, object value)
        {
            if (typeof(T).IsAssignableFrom(value.GetType()))
            {
                _properties[propertyName] = value;
            }
            else
            {
                throw new ArgumentException($"Value of type '{value.GetType()}' cannot be assigned to property '{propertyName}' of type '{typeof(T)}'.");
            }
        }
    }

    public class MyGenericClass
    {
        public static GenericModel<T> CreateGenericModel<T>(Dictionary<string, string> parameters)
        {
            try
            {
                var model = new GenericModel<T>();

                foreach (var kvp in parameters)
                {
                    model.SetValue(kvp.Key, kvp.Value);
                }

                return model;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
    }

    public class LinearRegression
    {
        private Dictionary<string, object> properties = new Dictionary<string, object>();
        private string ModelName { get; }
        public LinearRegression(Dictionary<string, string> query)
        {
            if (query.ContainsKey("personid"))
            {
                ModelName = query["personid"];
                Console.WriteLine($"Creating LinearRegression with personid: {ModelName}");
            }
            else
            {
                Console.WriteLine("Creating LinearRegression without a valid personid.");
            }
        }
    }


}
