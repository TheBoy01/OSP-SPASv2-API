using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Web.Utility;
using System.Drawing;
using System.Formats.Asn1;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OSP.SPASv2.Web.APIServices.Repository
{
    public class VendorRepository
    {

        //public async Task<IList<qryVendorList>> GetVendorList()
        //{
        //    string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Rudy/GetVendorList";

        //    using (var _httpClient = new HttpClient())
        //    {

        //      //  Obtain a JWT token. This example uses "Sam" as a user name and an empty password.
        //        StringContent httpContent = new StringContent(@"{ ""Username"": ""wa"", ""Password"": ""123412"" }", Encoding.UTF8, "application/json");
        //        var response1 = await _httpClient.PostAsync("http://192.168.23.185/SPASv2Repo/api/Rudy/Authorize", httpContent);

        //        // Save the token for further requests.
        //        var token = await response1.Content.ReadAsStringAsync();
        //        // Set the authentication header. 
        //        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


        //        var response = await _httpClient.GetAsync(requestAddress);
        //        response.EnsureSuccessStatusCode();
        //        var content = await response.Content.ReadAsStringAsync();
        //        //var companies = JsonSerializer.Deserialize<List<TblVendor>>(content, _options);

        //        JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        //        var companies1 = JsonSerializer.Deserialize<IList<qryVendorList>>(content, _options);

        //        return companies1;
        //    }
        //}

        #region Get
        public async Task<IList<qryVendorList>> GetVendorLists(string URL)
        {
            try
            {
                // var config1 = ip;
                string requestAddress = URL + "/Rudy/GetVendorLists";
                // string requestAddress = "http://192.168.23.185:80/api/Rudy/GetVendorLists";
                //string requestAddress = ip + "/api/Repository/GetBranchlist";

                //var query = new Dictionary<string, string>()
                //{
                //    ["companydesc"] = company,
                //    ["branchcode"] = branch,

                //};

                //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<qryVendorList> vlist = await UtilitiesHttpClient<qryVendorList>.GetJsonlist(requestAddress);
                return vlist;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);
            }
        }

        public async Task<IList<qryVendorList>> GetVendorLists1(string URL,string vendordesc,string paymentclass)
        {
            try
            {
                // var config1 = ip;
                string requestAddress = URL + "/Repository/GetVendorLists1";
                // string requestAddress = "http://192.168.23.185:80/api/Rudy/GetVendorLists1";
                //string requestAddress = ip + "/api/Repository/GetBranchlist";

                var query = new Dictionary<string, string>()
                {
                    ["vendordesc"] = vendordesc,
                    ["paymentclass"] = paymentclass,


                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<qryVendorList> vlist = await UtilitiesHttpClient<qryVendorList>.GetJsonlist(requestAddress);
                return vlist;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }

        public async Task<IList<TblVendorItems>> GetVendorItemsList(string URL, string vendorcode)
        {
            try
            {
                // var config1 = ip;
                string requestAddress = URL + "/Repository/GetVendorItemsList";
                // string requestAddress = "http://192.168.23.185:80/api/Rudy/GetVendorItemsList";
                //string requestAddress = ip + "/api/Repository/GetBranchlist";

                var query = new Dictionary<string, string>()
                {
                    ["vendorcode"] = vendorcode,

                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<TblVendorItems> vlist = await UtilitiesHttpClient<TblVendorItems>.GetJsonlist(requestAddress);
                return vlist;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }
        public async Task<IList<TblVendorItems>> GetVendorItemsList1(string URL, string vendorcode,string itemdesc)
        {
            try
            {
                // var config1 = ip;
                string requestAddress = URL + "/Repository/GetVendorItemsList1";
                // string requestAddress = "http://192.168.23.185:80/api/Rudy/GetVendorItemsList";
                //string requestAddress = ip + "/api/Repository/GetBranchlist";

                var query = new Dictionary<string, string>()
                {
                    ["vendorcode"] = vendorcode,
                    ["itemdesc"] = itemdesc,
                    

                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<TblVendorItems> vlist = await UtilitiesHttpClient<TblVendorItems>.GetJsonlist(requestAddress);
                return vlist;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }
        public async Task<TblVendorItems> GetVendorItemsDetails(string URL, string vendorcode, string itemcode)
        {
            try
            {
                // var config1 = ip;
                string requestAddress = URL + "/Repository/GetVendorItemsDetails";
                // string requestAddress = "http://192.168.23.185:80/api/Rudy/GetVendorItemsDetails";
                //string requestAddress = ip + "/api/Repository/GetBranchlist";

                var query = new Dictionary<string, string>()
                {
                    ["vendorcode"] = vendorcode,
                    ["itemcode"] = itemcode,
                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                TblVendorItems vlist = await UtilitiesHttpClient<TblVendorItems>.GetJsonlist1(requestAddress);
                return vlist;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);
            }
        }

        public async Task<List<RefVendorType>> GetVendorTypeList(string URL)
        {
            string requestAddress = URL + "/Rudy/GetVendorTypeList";

            using (var _httpClient = new HttpClient())
            {
                //  Obtain a JWT token. This example uses "Sam" as a user name and an empty password.
                //StringContent httpContent = new StringContent(@"{ ""Username"": ""wa"", ""Password"": ""123412"" }", Encoding.UTF8, "application/json");
                //var response1 = await _httpClient.PostAsync("http://192.168.23.185/SPASv2Repo/api/Rudy/Authorize", httpContent);

                IList<RefVendorType> RefVendorTypeList = await UtilitiesHttpClient<RefVendorType>.GetJsonlist(requestAddress);
                // Save the token for further requests.
                //var token = await response1.Content.ReadAsStringAsync();
                //// Set the authentication header. 
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                //var response = await _httpClient.GetAsync(requestAddress);
                //response.EnsureSuccessStatusCode();
                //var content = await response.Content.ReadAsStringAsync();
                //var companies = JsonSerializer.Deserialize<List<TblVendor>>(content, _options);

                //JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                //return JsonSerializer.Deserialize<List<string>>(content, _options);

                //return ReturnData;

                return RefVendorTypeList.ToList();
            }
        }

        public async Task<IList<RefAddressType>> GetAddressTypeList(string URL)
        {
            string requestAddress = URL + "/Rudy/GetAddressTypeList";

            using (var _httpClient = new HttpClient())
            {
                //  Obtain a JWT token. This example uses "Sam" as a user name and an empty password.
                //StringContent httpContent = new StringContent(@"{ ""Username"": ""wa"", ""Password"": ""123412"" }", Encoding.UTF8, "application/json");
                //var response1 = await _httpClient.PostAsync("http://192.168.23.185/SPASv2Repo/api/Rudy/Authorize", httpContent);

                //// Save the token for further requests.
                //var token = await response1.Content.ReadAsStringAsync();
                //// Set the authentication header. 
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                var response = await _httpClient.GetAsync(requestAddress);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                //var companies = JsonSerializer.Deserialize<List<TblVendor>>(content, _options);

                JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                return JsonSerializer.Deserialize<IList<RefAddressType>>(content, _options);
            }
        }

        public async Task<IList<TblVendorpaymethod>> GetVendorAcctNo(string URL, string vendorcode)
        {
            // var config1 = ip;
            string requestAddress = URL + "/Rudy/GetVendorAcctNo";
            // string requestAddress = "http://192.168.23.185:80/api/Rudy/GetVendorAcctNo";
            //string requestAddress = ip + "/api/Repository/GetBranchlist";

            var query = new Dictionary<string, string>()
            {
                ["vendorcode"] = vendorcode,
                //["branchcode"] = branch,

            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            IList<TblVendorpaymethod> vlist = await UtilitiesHttpClient<TblVendorpaymethod>.GetJsonlist(requestAddress);
            return vlist;
        }

        public async Task<TblVendorpaymethod> GetVendorAcctNo1(string URL, string vendorcode)
        {
            // var config1 = ip;
            string requestAddress = URL + "/Rudy/GetVendorAcctNo1";
            // string requestAddress = "http://192.168.23.185:80/api/Rudy/GetVendorAcctNo";
            //string requestAddress = ip + "/api/Repository/GetBranchlist";

            var query = new Dictionary<string, string>()
            {
                ["vendorcode"] = vendorcode,
                //["branchcode"] = branch,

            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            TblVendorpaymethod vlist = await UtilitiesHttpClient<TblVendorpaymethod>.GetJsonlist1(requestAddress);
            return vlist;
        }

        public async Task<string> GetNewGeneratedVendorCode(string URL)
        {
            string requestAddress = URL + "/Rudy/GetLatestVendorCode";
            string LatestBatchNo = string.Empty; 
             
            using (var _httpClient = new HttpClient())
            { 
                var response = await _httpClient.GetAsync(requestAddress);
                //response.EnsureSuccessStatusCode();
                LatestBatchNo = await response.Content.ReadAsStringAsync();
            }

            return LatestBatchNo;
        }

        public async Task<IList<RefATC>> GetRefATCList(string URL)
        {
            // var config1 = ip;
            
            string requestAddress = URL + "/Rudy/GetATCList";
            // string requestAddress = "http://192.168.23.185:80/api/Rudy/GetVendorAcctNo";
            //string requestAddress = ip + "/api/Repository/GetBranchlist";

            //var query = new Dictionary<string, string>()
            //{
            //    ["vendorcode"] = vendorcode,
            //    //["branchcode"] = branch,

            //};

            //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            IList<RefATC> vlist = await UtilitiesHttpClient<RefATC>.GetJsonlist(requestAddress);
            return vlist;
        }

        public async Task<qryVendorDetails> GetVendorDetails(string url, string vendorcode,string payclass)
        {
            try
            {
                // var config1 = ip;
                string requestAddress = url + "/Repository/GetVendorDetails";
                // string requestAddress = "http://192.168.23.185:80/api/Rudy/GetVendorDetails";
                //string requestAddress = ip + "/api/Repository/GetBranchlist";

                var query = new Dictionary<string, string>()
                {
                    ["vendorcode"] = vendorcode,
                    ["payclass"] = payclass,


                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                qryVendorDetails vlist = await UtilitiesHttpClient<qryVendorDetails>.GetJsonlist1(requestAddress);
                return vlist;
            }
            catch (Exception)
            {
                throw;
            }
            
        } 

        public async Task<IList<RefVendorDocs>> GetVendorDocsList(string URL)
        { 
            string requestAddress = URL + "/Rudy/GetVendorDocsList";

            IList<RefVendorDocs> vlist = await UtilitiesHttpClient<RefVendorDocs>.GetJsonlist(requestAddress);
            return vlist;
        }

  
        #endregion

        #region Post

        public async Task<TblResponse> CreateVendor(string URL,TblVendor _TblVendor)
        {
            string requestAddress = URL + "/Rudy/CreateVendor";

            //string requestAddress = "http://192.168.23.185:80/api/Rudy/GettmpPaymentRequestInventory";
            //string requestAddress = ip + "/api/Repository/GetCompanylist";

            //var query = new Dictionary<string, string>()
            //{
            //    ["PRNo"] = PRNo,

            //};

            //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

            TblResponse response = await UtilitiesHttpClient<TblVendor>.PostAsync(_TblVendor, requestAddress);
            return response;
        }

        public async Task<TblResponse> CreateAddress(string URL, List<TblVendorAddress> _TblVendorAddress,string VendorCode)
        {
            string requestAddress = URL  + "/Rudy/CreateVendorAddress";
            TblResponse response = new TblResponse();
            foreach (TblVendorAddress item in _TblVendorAddress)
            {
                TblVendorAddress TblVendorAddress = new TblVendorAddress();
                TblVendorAddress = item;
                TblVendorAddress.AuditDate = DateTime.Now;
                TblVendorAddress.AuditUser = "MAIN ORUDYAB";
                TblVendorAddress.Active = true;
                TblVendorAddress.VendorCode = VendorCode;

                 response = await UtilitiesHttpClient<TblVendorAddress>.PostAsync(TblVendorAddress, requestAddress);
                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    //throw new Exception(response.ToString());
                    break;
                }
            }

            //string requestAddress = "http://192.168.23.185:80/api/Rudy/GettmpPaymentRequestInventory";
            //string requestAddress = ip + "/api/Repository/GetCompanylist";

            //var query = new Dictionary<string, string>()
            //{
            //    ["PRNo"] = PRNo,

            //};

            //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            return response;

        }

        public async Task<List<string>> GetBankAccTypeList(string URL)
        {
            string requestAddress = URL + "/Rudy/GetBankAcctTypeList";

            using (var _httpClient = new HttpClient())
            {
                //  Obtain a JWT token. This example uses "Sam" as a user name and an empty password.
                StringContent httpContent = new StringContent(@"{ ""Username"": ""wa"", ""Password"": ""123412"" }", Encoding.UTF8, "application/json");
                var response1 = await _httpClient.PostAsync("http://192.168.23.185/SPASv2Repo/api/Rudy/Authorize", httpContent);

                // Save the token for further requests.
                var token = await response1.Content.ReadAsStringAsync();
                // Set the authentication header. 
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync(requestAddress);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                //var companies = JsonSerializer.Deserialize<List<TblVendor>>(content, _options);

                JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                return JsonSerializer.Deserialize<List<string>>(content, _options);
            }
        }

        public async Task<IList<RefBank>> GetBankList(string URL)
        {
            string requestAddress = URL + "/Rudy/GetBankList";

            IList<RefBank> inventory = await UtilitiesHttpClient<RefBank>.GetJsonlist(requestAddress);
            return inventory;
        } 
        
        public async Task<string> GetVendorCodeByDisplayName(string URL,string DisplayName)
        {
            string requestAddress = URL + "/Rudy/GetVendorCodeByDisplayName";

            var query = new Dictionary<string, string>()
            {
                ["DisplayName"] = DisplayName 
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

            return await UtilitiesHttpClient<string>.GetJsonstring(requestAddress);
        }

        public async Task<IList<RefRegion>> GetRegionList(string URL)
        {
            string requestAddress = URL + "/Rudy/GetRegionList";

            using (var _httpClient = new HttpClient())
            {
                //  Obtain a JWT token. This example uses "Sam" as a user name and an empty password.
                //StringContent httpContent = new StringContent(@"{ ""Username"": ""wa"", ""Password"": ""123412"" }", Encoding.UTF8, "application/json");
                //var response1 = await _httpClient.PostAsync("http://192.168.23.185/SPASv2Repo/api/Rudy/Authorize", httpContent);

                //// Save the token for further requests.
                //var token = await response1.Content.ReadAsStringAsync();
                //// Set the authentication header. 
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                var response = await _httpClient.GetAsync(requestAddress);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                //var companies = JsonSerializer.Deserialize<List<TblVendor>>(content, _options);

                JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                return JsonSerializer.Deserialize<IList<RefRegion>>(content, _options);
            }
        }

        public async Task<IList<RefProvince>> GetProvinceList(string URL)
        {
            string requestAddress = URL + "/Rudy/GetProvinceList";

            using (var _httpClient = new HttpClient())
            {
                //  Obtain a JWT token. This example uses "Sam" as a user name and an empty password.
                //StringContent httpContent = new StringContent(@"{ ""Username"": ""wa"", ""Password"": ""123412"" }", Encoding.UTF8, "application/json");
                //var response1 = await _httpClient.PostAsync("http://192.168.23.185/SPASv2Repo/api/Rudy/Authorize", httpContent);

                //// Save the token for further requests.
                //var token = await response1.Content.ReadAsStringAsync();
                //// Set the authentication header. 
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                var response = await _httpClient.GetAsync(requestAddress);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                //var companies = JsonSerializer.Deserialize<List<TblVendor>>(content, _options);

                JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                return JsonSerializer.Deserialize<IList<RefProvince>>(content, _options);
            }
        }

        public async Task<IList<RefCity>> GetCityList(string URL)
        {
            string requestAddress = URL +"/Rudy/GetCityList";

            using (var _httpClient = new HttpClient())
            {
                //  Obtain a JWT token. This example uses "Sam" as a user name and an empty password.
                //StringContent httpContent = new StringContent(@"{ ""Username"": ""wa"", ""Password"": ""123412"" }", Encoding.UTF8, "application/json");
                //var response1 = await _httpClient.PostAsync("http://192.168.23.185/SPASv2Repo/api/Rudy/Authorize", httpContent);

                //// Save the token for further requests.
                //var token = await response1.Content.ReadAsStringAsync();
                //// Set the authentication header. 
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                var response = await _httpClient.GetAsync(requestAddress);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                //var companies = JsonSerializer.Deserialize<List<TblVendor>>(content, _options);

                JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                return JsonSerializer.Deserialize<IList<RefCity>>(content, _options);
            }
        }

        public async Task<IList<RefBrgy>> GetBrgyList(string URL)
        {
            string requestAddress = URL + "/Rudy/GetBrgyList";

            using (var _httpClient = new HttpClient())
            {
                //  Obtain a JWT token. This example uses "Sam" as a user name and an empty password.
                //StringContent httpContent = new StringContent(@"{ ""Username"": ""wa"", ""Password"": ""123412"" }", Encoding.UTF8, "application/json");
                //var response1 = await _httpClient.PostAsync("http://192.168.23.185/SPASv2Repo/api/Rudy/Authorize", httpContent);

                //// Save the token for further requests.
                //var token = await response1.Content.ReadAsStringAsync();
                //// Set the authentication header. 
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                var response = await _httpClient.GetAsync(requestAddress);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                //var companies = JsonSerializer.Deserialize<List<TblVendor>>(content, _options);

                JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                return JsonSerializer.Deserialize<IList<RefBrgy>>(content, _options);
            }
        }

        public async Task<IList<RefCity>> GetCityByProvinceCode(string URL, string provinceCode)
        {
            string requestAddress = URL + "/Rudy/GetCityByProvinceCode";
            // string requestAddress = "http://192.168.23.185:80/api/Rudy/GetVendorDetails";
            //string requestAddress = ip + "/api/Repository/GetBranchlist";

            var query = new Dictionary<string, string>()
            {
                ["provinceCode"] = provinceCode  
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

            using (var _httpClient = new HttpClient())
            {  
                var response = await _httpClient.GetAsync(requestAddress);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                //var companies = JsonSerializer.Deserialize<List<TblVendor>>(content, _options);

                JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                return JsonSerializer.Deserialize<IList<RefCity>>(content, _options);
            }
        }

        public async Task<IList<RefBrgy>> GetBrgyListByCityCode(string URL, string cityCode)
        {
            string requestAddress = URL + "/Rudy/GetBrgyListByCityCode";
            // string requestAddress = "http://192.168.23.185:80/api/Rudy/GetVendorDetails";
            //string requestAddress = ip + "/api/Repository/GetBranchlist";

            var query = new Dictionary<string, string>()
            {
                ["cityCode"] = cityCode
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

            using (var _httpClient = new HttpClient())
            {
                var response = await _httpClient.GetAsync(requestAddress);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                //var companies = JsonSerializer.Deserialize<List<TblVendor>>(content, _options);

                JsonSerializerOptions _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                return JsonSerializer.Deserialize<IList<RefBrgy>>(content, _options);
            }
        }

        public async Task<string> GetVendorNameByVendorCode(string URL,string vendorCode)
        {
            try
            {
                string requestAddress = URL + "/Rudy/GetVendorNameByVendorCode";
                //  string requestAddress = "http://192.168.23.185/OSPRepo/api/CommonRepository/GetCompanyTypesAccess";

                var query = new Dictionary<string, string>()
                {
                    ["vendorCode"] = vendorCode,
                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                return await UtilitiesHttpClient<string>.GetJsonstring(requestAddress);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<TblVendor> ReadVendor(string URL, string vendorCode)
        {
            try
            {
                string requestAddress = URL + "/Wa/ReadVendor";
                //  string requestAddress = "http://192.168.23.185/OSPRepo/api/CommonRepository/GetCompanyTypesAccess";

                var query = new Dictionary<string, string>()
                {
                    ["vendorcode"] = vendorCode,
                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                TblVendor vlist = await UtilitiesHttpClient<TblVendor>.GetJsonlist1(requestAddress);
                return vlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
