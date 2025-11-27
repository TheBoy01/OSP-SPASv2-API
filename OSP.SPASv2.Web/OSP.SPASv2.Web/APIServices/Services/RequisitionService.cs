using Microsoft.AspNetCore.Mvc;
using OSP.Common.Domain.Params;
using OSP.SPASv2.Web.Utility;

namespace OSP.SPASv2.Web.APIServices.Services
{
    public class RequisitionService
    { 
        public async Task<string> GenerateNewPRNo(string URL,RequisitionParams RequisitionParams)
        {
            string requestAddress = URL + "/Requisition/GenerateNewPRNo";

            //var query = new Dictionary<string, string>()
            //{
            //    ["lastno"] = prno,
            //    ["companycode"] = companycode,
            //    ["branchcode"] = branchcode,
            //    ["auditdate"] = auditdate.ToString("yyyy-MM-ddTHH:mm:ss"),

            //};

            //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            RequisitionParams = await UtilitiesHttpClient<RequisitionParams>.PostAsyncT<RequisitionParams>(RequisitionParams, requestAddress);
            return RequisitionParams.LastNo;
        }

        public async Task<RequisitionParams> GroupRequisitionHdrDtl(string URL, List<qryRequisition> qryRequisitionList)
        {
            try
            {
                string requestAddress = URL + "/Requisition/GroupRequisitionHdr";

                RequisitionParams RequisitionParams = await UtilitiesHttpClient<List<qryRequisition>>.PostAsyncT<RequisitionParams>(qryRequisitionList, requestAddress);
                return RequisitionParams;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public async Task<qryComputeBreakdown> ComputeBreakDown(string URL, int qty, decimal gross, decimal vatrate, decimal discount, string discountcode)
        //{
        //    try
        //    {
        //        string requestAddress = URL + "/PaymentRequest/ComputeBreakDown";

        //        var query = new Dictionary<string, string>()
        //        {
        //            ["qty"] = qty,
        //            ["gross"] = gross,
        //            ["vatrate"] = vatrate,
        //            ["auditdate"] = auditdate.ToString("yyyy-MM-ddTHH:mm:ss"),

        //        };

        //        requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
        //        string str = await UtilitiesHttpClient<string>.GetJsonstring(requestAddress);
        //        return str;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}


        public async Task<RequisitionParams> ComputeRequisitionHdr(string URL, List<qryRequisitionDtl> qryRequisitionDtls)
        {
            try
            {
                string requestAddress = URL + "/Requisition/ComputeRequisitionHdr";

                RequisitionParams RequisitionParams = await UtilitiesHttpClient<List<qryRequisitionDtl>>.PostAsyncT<RequisitionParams>(qryRequisitionDtls, requestAddress);
                return RequisitionParams;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<IActionResult> DownloadFile1(string URL)

        {
            try
            {
                string apiUrl = URL + "/OSPCommon/DownloadFile1"; // Replace with your actual API endpoint
                var fileResult = await UtilitiesHttpClient<GetFileDownload>.DownloadFileFromApiAsync1(apiUrl);

                if (fileResult != null)
                {
                    // File download was successful; you can return this FileResult from your API controller or handle it as needed
                    // For example, return it from an action method in a controller
                    return fileResult;
                }
                else
                {
                    // Handle the case where the file download was not successful
                    // You might want to log an error, return an appropriate response, etc.
                    var str = "File download failed.";
                    return new Microsoft.AspNetCore.Mvc.NotFoundObjectResult(str);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                // Here, we return a 500 Internal Server Error response, but you may want to customize it based on your application's requirements
                //return StatusCode(500, $"Internal Server Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<IActionResult> DownloadFile(string URL)

        {
            try
            {
                string requestAddress = URL + "/OSPCommon/DownloadFile1";

                // Call the static method in UtilitiesHttpClient class
                var fileResult = await UtilitiesHttpClient<GetFileDownload>.DownloadFileFromApiAsync(requestAddress);

                if (fileResult != null)
                {
                    // Return the FileResult if the file download was successful
                    return fileResult;
                }
                else
                {
                    //    // Handle the case where the file download was not successful
                    //return NotFound("File download failed.");
                    //}
                    var str = "File download failed.";
                    return new Microsoft.AspNetCore.Mvc.NotFoundObjectResult(str);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                // Here, we return a 500 Internal Server Error response, but you may want to customize it based on your application's requirements
                //return StatusCode(500, $"Internal Server Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<TblResponse> DownloadFileFromWeb(string URL,string filedir,string filename)
        {
            string fileUrl = URL + "/OSPCommon/DownloadFile1";
           // string destinationPath = @"C:\SPASv2\DownloadFileFromWeb.xlsx";
            var response = new TblResponse();
            filedir = Path.Combine(filedir, filename);
            var resp = await UtilitiesHttpClient<GetFileDownload>.DownloadFileAsync(fileUrl, filedir);
            if (resp.Status=="SUCCESS")
            {
                // File downloaded successfully, you can return a response or perform further actions
                //return Ok("File downloaded successfully!");
                // response = new TblResponse()
                //{ ErrorMessage= "File downloaded successfully!",
                //Status="SUCCESS"
               
                //};
                return resp;
            }
            else
            {
                // Handle the case where file download failed
               // return StatusCode(500, "Failed to download file.");
                //response = new TblResponse()
                //{
                //    ErrorMessage = "Failed to download file",
                //    Status = "FAILED"

                //};
                return resp;
            }
        }

        public async Task<TblResponse> ReadImageTextFromUrl(string URL, string imgurl)
        {
            string apiUrl = URL + "/OSPCommon/ReadImageText";
            //var fileResult = await UtilitiesHttpClient<string>.PostAsyncT(imgurl);
            ImageTextReaderParams para = new ImageTextReaderParams()
            { 
                SystemCode = "SPASV2",
                AuditUser = "MAINOJONIAN",
                ImageURL = imgurl
            }; 

            var resp = await UtilitiesHttpClient<ImageTextReaderParams>.PostAsyncT<ImageTextReaderParams>(para, apiUrl);

            return new TblResponse();
        }

            public async Task<TblResponse> ReadImageText1(string URL, IFormFile formFile)
        {
            string fileUrl = URL + "/OSPCommon/ReadImageText";
            string RespContent = "";
            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            using (var fileStream = formFile.OpenReadStream())
            {
                // Create a StreamContent object from the file stream
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = Path.GetFileName(formFile.FileName)
                };

                // Add the file content to the form data
                formData.Add(fileContent);

                // Send the POST request to the API endpoint
                //var response = await client.PostAsync("https://localhost:7090/api/OSPCommon/ReadImageText", formData);

                var response = await client.PostAsync("https://onlineforms.stpeter.com.ph/OSPService/OSPCommon/ReadImageText", formData);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("File uploaded successfully");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }

            return new TblResponse(){ };
        }

    }
}
