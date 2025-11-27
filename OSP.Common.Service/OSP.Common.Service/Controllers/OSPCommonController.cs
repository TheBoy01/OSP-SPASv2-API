using ClosedXML.Excel;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
using OSP.Common.Service.APIRepository;
using OSP.Common.Service.Service;
using OSP.Common.Service.Utility;
using System.Reflection;
using System.Data;
using System.Diagnostics;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Server.IIS.Core;
using Core6_FileDownload.Models;
using IO = System.IO;
using Microsoft.Extensions.Configuration;
using Google.Cloud.Vision.V1;
using static Google.Cloud.Vision.V1.ProductSearchResults.Types;
using System.IO;
using OSP.Common.Domain.Params;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace OSP.Common.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OSPCommonController : ControllerBase
    {
        TblResponse _response = new TblResponse();
        private RepositoryUnit _RepositoryUnit;
        private ServiceUnit _ServiceUnit;
        private ILogger _logger;
        private IConfiguration _Iconfig;
        private IHostEnvironment _env;
        private string _filedirtemplate;
        private IConfiguration _configuration;

        public OSPCommonController(ILogger<OSPCommonController> logger, IConfiguration iConfig,IHostEnvironment env, IConfiguration configuration)
        {
            _logger = logger;
            _ServiceUnit = new ServiceUnit();
            _RepositoryUnit = new RepositoryUnit();
            _Iconfig = iConfig;
            _response = new TblResponse();
            _env= env;
            //_filedirtemplate = _configuration.GetSection("UploadingPath")["ReqTemplate"];
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet("EncryptPW")]
        public ActionResult<string> EncryptPW(string password)
        {
            var pw = Utility.Utilities.EncryptPW(password);
            return pw.ToString();
        }

        [HttpPost("UploadFiles")]
        public async Task<TblResponse> UploadFiles(List<IFormFile> files, string ReferenceNo, string UploadingFilePath)
        {
            return await Utility.Utilities.UploadFiles(files, ReferenceNo, UploadingFilePath);
        }



        [HttpPost("Send")]
        public async Task<TblResponse> Send(qryNotification _qryNotification)
        {
            string Key = _Iconfig.GetValue<string>("APIBaseURLCommon:Common.Repository");

            try
            {
                if (_qryNotification.NotificationCode == "SMS")
                {
                    
                    _logger.LogInformation("Fetching - " + Utilities.Getprojectname() + " -  " + Utilities.GetmethodName() + " ");


                    if (_qryNotification.Network == "SMART")
                    {
                        TblSendSMSSmart _TblSendSMSSmart = new TblSendSMSSmart();

                        _TblSendSMSSmart.Message = _qryNotification.Message; ;
                        _TblSendSMSSmart.Receiver = _qryNotification.Receiver;
                        _ServiceUnit.SendSMSService.SendSMSSmart(_TblSendSMSSmart);
                    }
                    else
                    {
                        TblSendSMSYondu _TblSendSMSYondu = new TblSendSMSYondu();
                        _TblSendSMSYondu.Receiver = _qryNotification.Receiver;
                        _TblSendSMSYondu.Message = _qryNotification.Message;
                        _ServiceUnit.SendSMSService.SendSMSYondu(_TblSendSMSYondu);
                    }
                  
                    
                    
                    _logger.LogInformation("Success - " + Utilities.Getprojectname() + " -  " + Utilities.GetmethodName() + " ");
                    _response.Status = "SUCCESS";
                    _response.ErrorMessage = "SUCCESSFULLY SAVE!";
                }
                TblNotification _notification = new TblNotification();
                _notification.Idx = 0;
                _notification.SystemCode = _qryNotification.SystemCode;
                _notification.ReferenceCode = "NA";
                _notification.ReferenceNo = _qryNotification.ReferenceNo;
                _notification.NotificationCode = _qryNotification.NotificationCode;
                _notification.Sender = "SYSTEM";
                _notification.Receiver = _qryNotification.Receiver;
                _notification.SendType = "NA";
                _notification.StatusCode = "SUCCESS";
                _notification.Remarks = _qryNotification.Message;
                _notification.SendDate = DateTime.Now;
                _notification.AuditUser = "SYSTEM";
                _notification.AuditDate = DateTime.Now;


                _RepositoryUnit = new RepositoryUnit();

                await _RepositoryUnit.NotificationRepository.CreateNotification(_notification, Key);

                _response.Status = _notification.StatusCode; ;
                _response.ErrorMessage = _notification.StatusCode;

                return _response;
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                _logger.LogError(ex, error);
                _response.Status = "FAILED";
                _response.ErrorMessage = error;
                return _response;
            }


        }


       

        [HttpGet("DownloadFile1")]
        public async Task<IActionResult> DownloadFile1()
        {
            try
            {
                //var path = @"\\splpdevserver\SPAS_Docs\Template\SPASv2 Batch Template.xlsx";
                //var path = @"\\192.168.23.185\SPASv2$\Sample\SPASv2 Batch Template.xlsx";
                //var path = @"C:\SPASv2\Sample\SPASv2 Batch Template.xlsx";
                string contentRootPath = _env.ContentRootPath;
                string rootpath =IO.Path.Combine(contentRootPath, "Files");
                string filename = "SPASv2 Batch Template.xlsx";
                var path    = IO.Path.Combine(rootpath, filename);
                //throw new Exception("ERROR TEST");
                //FileDownload file = await _ServiceUnit.DownloadFileService.DownloadFile1(rootpath + @"\" + filename);
                FileDownload file = await _ServiceUnit.DownloadFileService.DownloadFile1(path);
                //file.FileName = filename;
                //byte[] bytes;

                //using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
                //{
                //    bytes = new byte[fileStream.Length];
                //    await fileStream.ReadAsync(bytes, 0, (int)fileStream.Length);
                //}

                //var contentType = "";
                //new FileExtensionContentTypeProvider().TryGetContentType(path, out contentType);

                //// Ensure the file content type is set to a known type (e.g., "application/octet-stream")
                //contentType = string.IsNullOrEmpty(contentType) ? "application/octet-stream" : contentType;

                return File(file.bytes, file.contentType, file.FileName);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                _logger.LogError(ex, error);
                _response.Status = "FAILED";
                _response.ErrorMessage = error;
                return BadRequest(_response);
            }
            

        }


        [HttpGet("DownloadFile")]
        public IActionResult DownloadFile()
        {
            //var filePath = @"\\splpdevserver\SPAS_Docs\Template\SPASv2 Batch Template.xlsx";
            var filePath = @"C:\SPASv2\Sample\SPASv2 Batch Template.xlsx";
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"File not found: {filePath}");
            }
            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                var contentTypeProvider = new FileExtensionContentTypeProvider();
                if (!contentTypeProvider.TryGetContentType(filePath, out string contentType))
                {
                    contentType = "application/octet-stream"; // Default content type if not found
                }

                var result = new GetFileDownload
                {
                    Result = "OK",
                    FileName = "SPASv2 Batch Template.xlsx",
                    MimeType = contentType,
                    File = fileBytes
                };

                return Ok(result);
            }
            catch (Exception ex)
            {

                // Log the exception or handle it appropriately
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }

        [HttpPost("ExportExcel")]
        public IActionResult ExportExcel()
        {
            var _complist = GetCompany();
            DataTable dataTable = Utilities.ConvertListToDataTable(_complist);

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.AddWorksheet(dataTable, "Records");
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheet", "Sample.xls");
                }
            }

        }

        [HttpGet("OpenExcelFile")]
        public IActionResult OpenExcelFile()
        {
            try
            {
                //string path = @"C:\SIS\SPASv2\wwwroot\Files";


                //var psi = new ProcessStartInfo
                //{
                //    FileName = path,
                //    UseShellExecute = true
                //};
                //Process.Start(psi);

                string contentRootPath = _env.ContentRootPath;
                string rootpath = IO.Path.Combine(contentRootPath, "Files");
                string filename = "SPASv2 Batch Template.xlsx";
                var path = IO.Path.Combine(rootpath, filename);

                string filePath = @"C:\SPASv2\Sample.xlsx";
                using (var workbook = new XLWorkbook(path))
                {

                    var worksheet = workbook.Worksheet(1); // Assuming the data is in the first worksheet
                    var refsheet = workbook.Worksheets.Worksheet("References");
                    var _complist = GetCompany();
                    //DataTable dataTable = Utilities.ConvertListToDataTable(_complist);

                    // Sample modification - changing the value in cell A2
                    //worksheet.Cell("A2").Value = 999;

                    // Add headers
                    refsheet.Cell(1, 1).Value = "super";
                    refsheet.Cell(1, 2).Value = "visor ";
                    refsheet.Cell(1, 3).Value = "wag ako";
                    refsheet.Cell(1, 4).Value = 1900;

                    // Add data to the worksheet
                    for (int i = 0; i < _complist.Count; i++)
                    {
                        var data = _complist[i];
                        refsheet.Cell(i + 2, 1).Value = data.CompanyType;

                    }

                    // Save the modified workbook
                    workbook.Save();



                    // Save the workbook to a MemoryStream
                    using (MemoryStream stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);

                        // Return the Excel file as a response
                        //return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "output.xlsx");
                    }
                }
                return Ok();
                //// Use the default application associated with .xlsx files to open the Excel file
                //Process.Start(filePath);
                //return 
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error opening Excel file: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("DownloadFile2")]
        public async Task<IActionResult> DownloadFile2(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || fileName == null)
            {
                return Content("File Name is Empty...");
            }

            // get the filePath

            var filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(),
                "ServerFiles", fileName);

            // create a memorystream
            var memoryStream = new MemoryStream();

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }
            // set the position to return the file from
            memoryStream.Position = 0;

            // Get the MIMEType for the File
            var mimeType = (string file) =>
            {
                var mimeTypes = MimeTypes.GetMimeTypes();
                var extension = System.IO.Path.GetExtension(file).ToLowerInvariant();
                return mimeTypes[extension];
            };

            return File(memoryStream, mimeType(filePath), System.IO.Path.GetFileName(filePath));

        }


        [HttpGet("GetCompanies")]
        public List<qryCompanyType> GetCompanies()
        {
            List<qryCompanyType> qryCompanyType = new List<qryCompanyType>();
            qryCompanyType qrycompany = new qryCompanyType()
            { CompanyType = "SPLPI" };

            qryCompanyType.Add(qrycompany);
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "FCMCI" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "FHFHSI" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "GGMCVI" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "SPCLI" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "SPCFTV" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "SPCMM" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "SPCMTL" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "SPCMWI" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "SPCNLI" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "SPCSLI" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "SPFMI" });




            return qryCompanyType;
        }

        [NonAction]
        public List<qryCompanyType> GetCompany()
        {
            List<qryCompanyType> qryCompanyType = new List<qryCompanyType>();
            qryCompanyType qrycompany = new qryCompanyType()
            { CompanyType = "SPLPI" };

            qryCompanyType.Add(qrycompany);
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "FCMCI" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "FHFHSI" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "GGMCVI" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "SPCLI" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "SPCFTV" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "SPCMM" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "SPCMTL" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "SPCMWI" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "SPCNLI" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "SPCSLI" });
            qryCompanyType.Add(new qryCompanyType() { CompanyType = "SPFMI" });




            return qryCompanyType;
        }

        [HttpPost("ReadImageText")]
        public string ReadImageText(ImageTextReaderParams ImgParams)
        {
            
            string result = string.Empty;

            if (ImgParams.UserName != "OSPAPI" || ImgParams.Password!= "e10adc3949ba59abbe56e057f20f883e")
            {
                return "Invalid Username or Password!";
            }

            try
            {
                ImageAnnotatorClientBuilder builder = new ImageAnnotatorClientBuilder();
                string jsonCredential = "{\r\n  'account': '',\r\n  'client_id': '764086051850-6qr4p6gpi6hn506pt8ejuq83di341hur.apps.googleusercontent.com',\r\n  'client_secret': 'd-FL95Q19q7MQmFpd7hHD0Ty',\r\n  'quota_project_id': 'ocr-demo-azure-ai-vision',\r\n  'refresh_token': '1//0gkWuhsfXdBT1CgYIARAAGBASNwF-L9IrE57Hosp0EHRHNuZUrU9U4o7e1nBB6jntp43d45dh6r7CTasRiRqeOek6AhOxUdetJq4',\r\n  'type': 'authorized_user',\r\n  'universe_domain': 'googleapis.com'\r\n}";
                builder.JsonCredentials = jsonCredential;
                ImageAnnotatorClient client = builder.Build();

                var img = //Image.FromStream(formFile.OpenReadStream());
                Image.FromUri(ImgParams.ImageURL);
                var resp = client.DetectText(img);

                foreach (EntityAnnotation entityAnnotation in resp)
                {
                    result = result + " " + entityAnnotation.Description;
                    break;
                }


            }
            catch (Exception ex)
            {

                result = ex.Message;
            }

            return result;
        }


        [HttpPost("DocAPITest")]
        public async Task<TblResponse> DocAPITest(ImageTextReaderParams ImgParams)
        {
            TblResponse response;
            string requestAddress = "https://onlineforms.stpeter.com.ph/Common/CommonRepository/CreateNotification";

            response = await UtilitiesHttpClient<ImageTextReaderParams>.PostAsync(ImgParams, requestAddress);
            //await UtilitesHttpClient<string>.PostAsync(_prno, requestAddress2);

            return response;
        }
    }

}
