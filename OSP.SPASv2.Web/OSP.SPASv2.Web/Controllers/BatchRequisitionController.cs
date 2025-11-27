using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OSP.SPASv2.Web.APIServices.Services;
using OSP.SPASv2.Web.Models;
using OSP.SPASv2.Web.Utility;
//using Repository.IRepository;
using SPASv2.Models;
using System.Data;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Web.Helpers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Identity;
using OSP.SPASv2.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Build.Tasks;
using System.Text.Json;
using OSP.SPASv2.Web.APIServices;
//using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.Data.OleDb;
using System.Runtime.CompilerServices;
using NuGet.Packaging;
using SPASv2.Controllers;
using OSP.SPASv2.Domain.Tables;
using static System.Data.Odbc.ODBC32;
using System.Reflection;
using System;
using System.Formats.Asn1;
using IO = System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using OSP.SPASv2.Domain.View;
using System.IO.Pipelines;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.Extensions.Hosting.Internal;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using System.Security.Principal;
using Microsoft.AspNetCore.Http.Extensions;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Engineering;
using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML;
using System.IO.Compression;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.Extensions.Hosting;
using DocumentFormat.OpenXml.Bibliography;
using OSP.SPASv2.Web.Views.BatchRequisition;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.Security.Policy;
using OSP.SPASv2.Domain.Params;
using System.Linq;

namespace OSP.SPASv2.Web.Controllers
{
    [Authorize(Roles = "SPASV2-Requester")]
    public class BatchRequisitionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILogger<AuthorizationController> _AuthLogger;
        private readonly UserManager<OSPSPASv2ApplicationUser> _userManager;
        private IConfiguration _configuration;
        private TblResponse _response;

        private IHostEnvironment _env;


        ServiceUnit _ServiceUnit;
        private RepositoryUnit _RepositoryUnit;
        private string UploadingPathPR;
        private string PRBatchFilePath;

        string BaseUrlRepo;
        string BaseUrlService;
        string OSPUrlRepo;
        string OSPUrlService;
        string DevelopmentType;
        string ExcelFileCopyPath;
        private string process;

        private string _filedirtemplate;
        private string _ReqFiles;
        private string _ReqDownloadFiles;
        private string ReqPathDisplay;
        private string SrcFilePath;
        //private readonly IHttpClientFactory _httpClientFactory;
        public BatchRequisitionController(ILogger<HomeController> logger, UserManager<OSPSPASv2ApplicationUser> userManager, ILogger<AuthorizationController> AuthLogger,
                                          IConfiguration configuration, IHostEnvironment env)
        {
            _logger = logger;
            _AuthLogger = AuthLogger;
            this._userManager = userManager;
            _ServiceUnit = new ServiceUnit();
            _RepositoryUnit = new RepositoryUnit();
            _AuthLogger = AuthLogger;
            _configuration = configuration;
            _response = new TblResponse();
            UploadingPathPR = _configuration.GetSection("UploadingPath")["PaymentRequest"];
            PRBatchFilePath = _configuration.GetSection("UploadingPath")["PRBatchPath"];

            BaseUrlRepo = _configuration.GetSection("APIBaseURL")["SPASv2.Repository"];
            BaseUrlService = _configuration.GetSection("APIBaseURL")["SPASv2.Service"];
            OSPUrlRepo = _configuration.GetSection("APIBaseURLCommon")["Common.Repository"];
            OSPUrlService = _configuration.GetSection("APIBaseURLCommon")["Common.Service"];
            _env = env;
            DevelopmentType = _env.EnvironmentName;
            _filedirtemplate = _configuration.GetSection("UploadingPath")["ReqTemplate"];
            _ReqFiles = _configuration.GetSection("UploadingPath")["ReqFiles"];
            _ReqDownloadFiles = _configuration.GetSection("UploadingPath")["ReqDownloadFiles"];
            ExcelFileCopyPath = _configuration.GetSection("UploadingPath")["ExcelFileUpload"];

            //_httpClientFactory = httpClientFactory;
        }

        //[RequestSizeLimit(300000000)]
        //[RequestFormLimits(MultipartBodyLengthLimit = 300000000)]
        public IActionResult ReadPaymentAttachments(List<IFormFile> files, string vm)
        {
            BatchRequisitionViewModel model = JsonConvert.DeserializeObject<BatchRequisitionViewModel>(vm);

            model.RequestAttachments = new List<qryRequestAttachments>();

            model.isUploadTemplate = false;
            //IList<string> list1 = JsonConvert.DeserializeObject<List<string>>(list);

            CreateTmpDocsPath();
            foreach (var item in model.BatchPaymentHdrList)
            {
                ReadAttachmentsFromFolder(item.PONo, item.SalesInvoiceNo, files, model.RequestAttachments);
            }

            return View("_BatchPaymentList", model);
        }

        public IActionResult ReadRequisitionAttachments(List<IFormFile> files, string vm)
        {
            BatchRequisitionViewModel model = JsonConvert.DeserializeObject<BatchRequisitionViewModel>(vm);

            model.RequestAttachments = new List<qryRequestAttachments>();

            CreateTmpDocsPath();

            foreach (var item in model.qryRequisitionHdrList)
            {
                ReadAttachmentsFromFolder(item.CompanyName.Replace(".", "") + " - " + item.VendorName, files, model.RequestAttachments);
            }

            return View("_BatchRequisitionList", model);
        }

        private void CreateTmpDocsPath()
        {
            string userid = _userManager.GetUserId(this.User);
            string WebRootPath = _env.ContentRootPath + "\\wwwroot";
            SrcFilePath = "\\Files" + "\\" + userid + "\\TMP\\";
            ReqPathDisplay = WebRootPath + SrcFilePath;

            if (System.IO.Directory.Exists(ReqPathDisplay))
            {
                try
                {
                    System.IO.Directory.Delete(ReqPathDisplay, true);
                }
                catch (Exception)
                {
                }
            }

            string tempFoldername = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
            SrcFilePath = SrcFilePath + tempFoldername + "\\";
            ReqPathDisplay = ReqPathDisplay + tempFoldername;
            System.IO.Directory.CreateDirectory(ReqPathDisplay + "\\");
        }

        public async Task<IActionResult> ReadPaymentTemplate(List<IFormFile> files)
        {
            string error = string.Empty;
            string path1 = string.Empty;

            process = "";
            try
            {
                BatchRequisitionViewModel _model = new BatchRequisitionViewModel();
                BatchUploadParams _BatchUploadParams = new BatchUploadParams();
                string extension = System.IO.Path.GetExtension(files[0].FileName).ToLower();
                string connString = "";

                string[] validFileTypes = { ".xls", ".xlsx" };

                path1 = string.Format("{0}/{1}", @"C:\Tmp\SPASv2", files[0].FileName);
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(@"C:\Tmp\SPASv2");
                }

                if (validFileTypes.Contains(extension))
                {
                    if (System.IO.File.Exists(path1))
                    { System.IO.File.Delete(path1); }

                    using (var localFile = System.IO.File.OpenWrite(path1))
                    using (var uploadedFile = files[0].OpenReadStream())
                    {
                        uploadedFile.CopyTo(localFile);
                        _model.BatchFilePath = localFile.Name;
                    }

                    DataTable dtRequest = new DataTable();
                    DataTable dtExcelVer = new DataTable();
                    if (extension.Trim() != ".xls" && extension.Trim() != ".xlsx")
                    {
                        throw new Exception("Invalid File Type");
                    }

                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    dtRequest = Utilities.ConvertXSLXtoDataTable(path1, connString, "Payment Request Template", ref process);
                    //dtExcelVer = Utilities.ConvertXSLXtoDataTable(path1, connString, "SystemVer");

                    _model.BatchPaymentHdrList = new List<qryBatchPaymentHdr>();
                    _model.BatchPaymentDtlList = new List<qryBatchPaymentDtl>();

                    for (int i = 0; i < dtRequest.Rows.Count; i++)
                    {
                        DataRow dr = dtRequest.Rows[i];
                        if (string.IsNullOrEmpty(dr[0].ToString()))
                        {
                            break;
                        }

                        string _tempPONo = dr[0].ToString();
                        string _tempDept = dr[3].ToString();
                        string _tempItem = await _RepositoryUnit.RefItemRepository.GetItemCodeByDesc(BaseUrlRepo, dr[4].ToString());
                        string _tempItemDesc = await _RepositoryUnit.RefItemRepository.GetItemDesc(BaseUrlRepo, _tempItem);
                        string _tempBal = dr[7].ToString();
                        string _tempQty = dr[8].ToString();
                        string _tempReceipt = dr[9].ToString();
                        string _tempDrNo = dr[10].ToString();
                        string _tempDRDate = dr[11].ToString();
                        string _tempSI = dr[12].ToString();
                        string _tempSIDate = dr[13].ToString();
                        string _tempHPDeduction = dr[16].ToString();
                        string _tempFreightPerUnit = dr[17].ToString();
                        string _tempPriceAmount = string.IsNullOrEmpty(dr[14].ToString()) ? "1" : dr[14].ToString();


                        process = "GetPOHdrByPONo";
                        TblPurchaseorderhdr _POhdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(BaseUrlRepo, _tempPONo);
                        process = "ReadRequestByPRNo";
                        TblRequisitionhdr _oldreq = await _RepositoryUnit.TblRequisitionRepository.ReadRequestByPRNo(BaseUrlRepo, _POhdr.Reqno);

                        string[] DeptName = _tempDept.Split('-');
                        process = "GetCompanyDetails" + DeptName[0] + DeptName[1];
                        qryCompanyDetails _qryCompanyDetails = await _RepositoryUnit.CompanyRepository.GetCompanyDetails(OSPUrlRepo, DeptName[0], DeptName[1]);

                        process = "ReadRequsitionDtlByPRNo";
                        TblRequisitiondtl _origReqDtl =
                        await _RepositoryUnit.TblRequisitionRepository.ReadRequsitionDtlByPRNo(BaseUrlRepo, _oldreq.Reqno, _qryCompanyDetails.CompanyCode,
                        _qryCompanyDetails.DeptCode, _tempItem);

                        process = "GetVendorDetails ";

                        qryVendorDetails _TempVendor = await _RepositoryUnit.VendorRepository.GetVendorDetails(BaseUrlRepo, _oldreq.VendorCode, _oldreq.PayClassCode);
                        TblVendor tblVendor = await _RepositoryUnit.VendorRepository.ReadVendor(BaseUrlRepo, _oldreq.VendorCode);
                        decimal _itemTotalAmount = (Convert.ToDecimal(_tempPriceAmount) * (Convert.ToDecimal(_tempQty))); //_origReqDtl.Price * (Convert.ToDecimal(_tempQty));
                        //decimal _itemTotalFreight = (Convert.ToDecimal(_tempFreightPerUnit));
                        decimal _itemTotaFreight = Convert.ToDecimal(_tempQty) * (Convert.ToDecimal(_tempFreightPerUnit));

                        qryBatchPaymentHdr pyhdr = _model.BatchPaymentHdrList.Where(t => t.PONo == _tempPONo && t.SalesInvoiceNo == tblVendor.Prefix + _tempSI.ToString()).FirstOrDefault();

                        if (pyhdr != null)
                        {
                            foreach (var item in _model.BatchPaymentHdrList)
                            {
                                if (pyhdr.PONo == item.PONo && pyhdr.SalesInvoiceNo == item.SalesInvoiceNo)
                                {
                                    item.Amount = item.Amount + _itemTotalAmount;
                                    item.FreightAmount = item.FreightAmount + _itemTotaFreight;
                                }
                            }
                        }
                        else
                        {
                            pyhdr = new qryBatchPaymentHdr()
                            {
                                PONo = _tempPONo,
                                SalesInvoiceNo = tblVendor.Prefix + _tempSI,
                                PayeeName = _TempVendor.VendorName,
                                Amount = _itemTotalAmount,
                                DeliveryNo = _tempDrNo,
                                DeliveryDate = Convert.ToDateTime(_tempDRDate),
                                SalesInvoiceDate = Convert.ToDateTime(_tempSIDate),
                                ReferenceReceiptNo = _tempReceipt,
                                HPDeduction = Convert.ToDecimal(_tempHPDeduction.Trim().Equals(string.Empty) ? 0 : _tempHPDeduction),
                                //FreightAmount = Convert.ToDecimal(_tempFreightAmt.Trim().Equals(string.Empty) ? 0 : _tempFreightAmt)
                                FreightAmount = _itemTotaFreight
                            };

                            _model.BatchPaymentHdrList.Add(pyhdr);
                        }

                        qryBatchPaymentDtl _pydtl = new qryBatchPaymentDtl()
                        {
                            PONo = _tempPONo,
                            SalesInvoice = tblVendor.Prefix + _tempSI,
                            Department = _tempDept,
                            ItemDescription = _tempItemDesc,
                            Balance = Convert.ToInt32(_tempBal),
                            Quantity = Convert.ToInt32(_tempQty),
                            Amount = _itemTotalAmount,
                            ReferenceReceipt = _tempReceipt,
                            DeliveryNo = _tempDrNo,
                            DeliveryDate = Convert.ToDateTime(_tempDRDate),
                            SalesInvoiceDate = Convert.ToDateTime(_tempSIDate),
                            FreightAmount = Convert.ToDecimal(_tempFreightPerUnit.Trim().Equals(string.Empty) ? 0 : _tempFreightPerUnit),
                            TemPriceAmount = Convert.ToDecimal(_tempPriceAmount)
                        };

                        _model.BatchPaymentDtlList.Add(_pydtl);

                    }

                    process = "Create Folder Template";

                    _model.FileDirectory = CreateFolderTemplate(_model.BatchPaymentHdrList);

                    ViewData["FileDir"] = _model.FileDirectory;

                    ViewData["devtype"] = DevelopmentType;

                    _model.isUploadTemplate = true;
                    _model.ExcelFileUploadPath = path1;

                    return View("_BatchPaymentList", _model);
                }

            }
            catch (Exception err)
            {
                return Json(new
                {
                    success = false,
                    error = process + " - " + err.Message + " " + path1.ToString(),

                },
                    new JsonSerializerOptions());
            }

            return Json(new
            {
                success = true,
                error = error
            }, new JsonSerializerOptions());
        }

        public async Task<IActionResult> CheckTemplate(List<IFormFile> files)
        {
            string error = string.Empty;
            string path1 = string.Empty;
            bool result = false;
            try
            {
                BatchRequisitionViewModel _model = new BatchRequisitionViewModel();
                BatchUploadParams _BatchUploadParams = new BatchUploadParams();
                string extension = System.IO.Path.GetExtension(files[0].FileName).ToLower();
                string connString = "";

                string[] validFileTypes = { ".xls", ".xlsx" };

                //path1 = string.Format("{0}/{1}", @"C:\Tmp\SPASv2", files[0].FileName);
                path1 = @"C:\Tmp\SPASv2" + "\\" + files[0].FileName;
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(@"C:\Tmp\SPASv2");
                }

                if (validFileTypes.Contains(extension))
                {
                    if (System.IO.File.Exists(path1))
                    { System.IO.File.Delete(path1); }

                    using (var localFile = System.IO.File.OpenWrite(path1))
                    using (var uploadedFile = files[0].OpenReadStream())
                    {
                        uploadedFile.CopyTo(localFile);
                        _model.BatchFilePath = localFile.Name;
                    }

                    DataTable dtRequest = new DataTable();
                    DataTable dtExcelVer = new DataTable();
                    if (extension.Trim() != ".xls" && extension.Trim() != ".xlsx")
                    {
                        throw new Exception("Invalid File Type");
                    }

                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    dtRequest = Utilities.ConvertXSLXtoDataTable(path1, connString, "Payment Request Template", ref process);

                    string PayClassCode = string.Empty;
                    PayClassCode = dtRequest.Rows[0][0].ToString();
                    if (PayClassCode.Contains("PO AND PAY"))
                    {
                        result = true;
                    }
                }

                return Json(new { IsPOAndPAY = result },
              new JsonSerializerOptions()); ;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IActionResult> ReadPaymentTemplateNew(List<IFormFile> files)
        {
            string error = string.Empty;
            string path1 = string.Empty;

            process = "";
            try
            {
                BatchRequisitionViewModel _model = new BatchRequisitionViewModel();
                BatchUploadParams _BatchUploadParams = new BatchUploadParams();
                string extension = System.IO.Path.GetExtension(files[0].FileName).ToLower();
                string connString = "";

                string[] validFileTypes = { ".xls", ".xlsx" };

                //path1 = string.Format("{0}/{1}", @"C:\Tmp\SPASv2", files[0].FileName);
                path1 = @"C:\Tmp\SPASv2" + "\\" + files[0].FileName;
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(@"C:\Tmp\SPASv2");
                }

                if (validFileTypes.Contains(extension))
                {
                    if (System.IO.File.Exists(path1))
                    { System.IO.File.Delete(path1); }

                    using (var localFile = System.IO.File.OpenWrite(path1))
                    using (var uploadedFile = files[0].OpenReadStream())
                    {
                        uploadedFile.CopyTo(localFile);
                        _model.BatchFilePath = localFile.Name;
                    }

                    DataTable dtRequest = new DataTable();
                    DataTable dtExcelVer = new DataTable();
                    if (extension.Trim() != ".xls" && extension.Trim() != ".xlsx")
                    {
                        throw new Exception("Invalid File Type");
                    }

                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    dtRequest = Utilities.ConvertXSLXtoDataTable(path1, connString, "Payment Request Template", ref process);
                    //dtExcelVer = Utilities.ConvertXSLXtoDataTable(path1, connString, "SystemVer");

                    _model.BatchPaymentHdrList = new List<qryBatchPaymentHdr>();
                    _model.BatchPaymentDtlList = new List<qryBatchPaymentDtl>();
                    _BatchUploadParams.qryBatchPaymentHdrList = new List<qryBatchPaymentHdr>();
                    _BatchUploadParams.qryBatchPaymentDtlList = new List<qryBatchPaymentDtl>();
                    _BatchUploadParams.TblResponse = new TblResponse();

                    decimal _HPDeductionTotal = 0.00m;
                    for (int i = 0; i < dtRequest.Rows.Count; i++)
                    {
                        DataRow dr = dtRequest.Rows[i];
                        if (string.IsNullOrEmpty(dr[0].ToString()))
                        {
                            break;
                        }

                        string _tempPONo = dr[0].ToString();
                        string _tempDept = dr[3].ToString();
                        //string _tempItem = await _RepositoryUnit.RefItemRepository.GetItemCodeByDesc(BaseUrlRepo, dr[4].ToString());
                        //string _tempItemDesc = await _RepositoryUnit.RefItemRepository.GetItemDesc(BaseUrlRepo, _tempItem);
                        string _tempItem = dr[4].ToString();

                        string _tempBal = dr[7].ToString();
                        string _tempQty = dr[8].ToString();
                        string _tempReceipt = dr[9].ToString();
                        string _tempDrNo = dr[10].ToString();
                        string _tempDRDate = dr[11].ToString();
                        string _tempSI = dr[12].ToString();
                        string _tempSIDate = dr[13].ToString();
                        string _tempHPDeduction = dr[16].ToString();
                        string _tempFreightPerUnit = dr[17].ToString();
                        decimal _tempPriceAmount = Convert.ToDecimal(string.IsNullOrEmpty(dr[14].ToString()) ? "1" : dr[14].ToString());

                        string[] DeptName = _tempDept.Split('-');


                        decimal _itemTotalAmount = (Convert.ToDecimal(_tempPriceAmount) * (Convert.ToDecimal(_tempQty)));

                        decimal _itemTotaFreight = Convert.ToDecimal(_tempQty) * (Convert.ToDecimal(_tempFreightPerUnit)); 
                        _HPDeductionTotal += Convert.ToDecimal(_tempHPDeduction.Trim().Equals(string.Empty) ? 0 : _tempHPDeduction);
                        qryBatchPaymentHdr pyhdr = _model.BatchPaymentHdrList.Where(t => t.PONo == _tempPONo && t.SalesInvoiceNo == _tempSI.ToString()).FirstOrDefault();
                        
                        qryBatchPaymentDtl _pydtl = new qryBatchPaymentDtl()
                        {
                            PONo = _tempPONo,

                            SalesInvoice = _tempSI,
                            Department = _tempDept,
                            ItemDescription = _tempItem,
                            Balance = Convert.ToInt32(_tempBal),
                            Quantity = Convert.ToInt32(_tempQty),
                            Amount = _tempPriceAmount,
                            ReferenceReceipt = _tempReceipt,
                            DeliveryNo = _tempDrNo,
                            DeliveryDate = Convert.ToDateTime(_tempDRDate),
                            SalesInvoiceDate = Convert.ToDateTime(_tempSIDate),
                            FreightAmount = Convert.ToDecimal(_tempFreightPerUnit.Trim().Equals(string.Empty) ? 0 : _tempFreightPerUnit),
                            TemPriceAmount = Convert.ToDecimal(_tempPriceAmount)
                        };
                         
                        if (!_BatchUploadParams.qryBatchPaymentHdrList.Where(a => a.SalesInvoiceNo.Equals(_tempSI)).Select(a => a.PONo).Contains(_tempPONo))
                        {

                            pyhdr = new qryBatchPaymentHdr()
                            {
                                PONo = _tempPONo,
                                SalesInvoiceNo = _tempSI,
                                PayeeName = "",
                                Amount = _itemTotalAmount,
                                DeliveryNo = _tempDrNo,
                                DeliveryDate = Convert.ToDateTime(_tempDRDate),
                                SalesInvoiceDate = Convert.ToDateTime(_tempSIDate),
                                ReferenceReceiptNo = _tempReceipt,
                                HPDeduction = _HPDeductionTotal,//Convert.ToDecimal(_tempHPDeduction.Trim().Equals(string.Empty) ? 0 : _tempHPDeduction),
                                FreightAmount = _itemTotaFreight
                            };

                            //_model.BatchPaymentHdrList.Add(pyhdr);
                            _BatchUploadParams.qryBatchPaymentHdrList.Add(pyhdr);
                            _HPDeductionTotal = 0;
                        }

                        //_model.BatchPaymentDtlList.Add(_pydtl);
                        _BatchUploadParams.qryBatchPaymentDtlList.Add(_pydtl);

                    }

                   

                    var _CanUpload = await _RepositoryUnit.PRBatchUploadRepository.CanUploadPayment(BaseUrlRepo, _BatchUploadParams);
                    if (_CanUpload.TblResponse.Status == "FAILED")
                    {
                        error = _CanUpload.TblResponse.ErrorMessage;
                        goto FAILED;
                    }

                    // var resp = await _RepositoryUnit.TblRequisitionRepository.ReadBatchPaymentList(BaseUrlRepo, _BatchUploadParams);
                    _model.BatchPaymentHdrList = _CanUpload.qryBatchPaymentHdrList;
                    _model.BatchPaymentDtlList = _CanUpload.qryBatchPaymentDtlList;

                    process = "Create Folder Template";

                    _model.FileDirectory = CreateFolderTemplate(_model.BatchPaymentHdrList);

                    ViewData["FileDir"] = _model.FileDirectory;

                    ViewData["devtype"] = DevelopmentType;

                    _model.isUploadTemplate = true;
                    _model.ExcelFileUploadPath = path1;

                    return View("_BatchPaymentList", _model);
                }
            }
            catch (Exception err)
            {
                return Json(new
                {
                    success = false,
                    error = process + " - " + err.Message + " " + path1.ToString(),

                },
                    new JsonSerializerOptions());
            }
        FAILED:
            return Json(new
            {
                success = false,
                error = error
            }, new JsonSerializerOptions());
        }

        static void CompressToRar(string sourceFilePath, string destinationRarFilePath)
        {
            // Set the path to WinRAR executable
            string winrarPath = @"C:\Program Files\WinRAR\WinRAR.exe"; // Update with your WinRAR installation path

            // Set the WinRAR command
            string command = $"a -ep1 -r \"{destinationRarFilePath}\" \"{sourceFilePath}\"";

            // Start WinRAR process
            using (Process process = new Process())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = winrarPath,
                    Arguments = command,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                process.StartInfo = startInfo;
                process.Start();

                process.WaitForExit();

                // Check for errors
                if (process.ExitCode != 0)
                {
                    string errorOutput = process.StandardError.ReadToEnd();
                    Console.WriteLine($"Error while compressing file: {errorOutput}");
                }
            }
        }

        private string CreateFolderTemplate(IList<qryBatchPaymentHdr> batchPaymentHdrList)
        {
            try
            {
                //string path = @"\\192.168.23.185\SPASv2$\Files\Requisition\" + DateTime.Now.ToString("yyyy-MM-dd hhmmsstt").ToUpper();
                string path = IO.Path.Combine(_ReqFiles, "Requisition", DateTime.Now.ToString("yyyy-MM-dd hhmmsstt").ToUpper());
                string path1 = _ReqDownloadFiles;
                string foldername = DateTime.Now.ToString("yyyy-MM-dd hhmmsstt").ToUpper() + ".rar";
                string path2 = IO.Path.Combine(path1, "WINRAR");
                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }

                foreach (var item in batchPaymentHdrList)
                {
                    string dir = path + @"\" + item.PONo + @"\" + item.SalesInvoiceNo + @"\";

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                }

                //string sourceFilePath = "C:\\YourFilePath\\YourFile.txt";
                string destinationRarFilePath = IO.Path.Combine(path2, foldername);

                // Step 1: Compress the file into a RAR file using WinRAR
                CompressToRar(path, destinationRarFilePath);



                // DownloadFolder(foldername);

                return foldername;
                //var psi = new ProcessStartInfo
                //{
                //    FileName = path,
                //    UseShellExecute = true
                //};
                //Process.Start(psi);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }



        }

        private string CreateFolderTemplateB(IList<qryRequisitionVendorCompanyChapel> qrylist)
        {
            try
            {
                //string path = @"\\192.168.23.185\SPASv2$\Files\Requisition\" + DateTime.Now.ToString("yyyy-MM-dd hhmmsstt").ToUpper();
                string path = IO.Path.Combine(_ReqFiles, "Requisition", DateTime.Now.ToString("yyyy-MM-dd hhmmsstt").ToUpper());
                string path1 = _ReqDownloadFiles;
                string foldername = DateTime.Now.ToString("yyyy-MM-dd hhmmsstt").ToUpper() + ".rar";
                string path2 = IO.Path.Combine(path1, "WINRAR");

                string pathwithname = Path.Combine(_ReqDownloadFiles, foldername);
                if (Directory.Exists(pathwithname))
                {
                    Directory.Delete(pathwithname, true);
                }

                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }

                foreach (var item in qrylist)
                {
                    // string dir = path +  item.CompanyType + @"\";
                    //string dir = Path.Combine(path, item.CompanyType + " - " + item.DeptCode);
                    string dir = Path.Combine(path, item.CompanyDesc.Replace(".", ""), item.VendorName);


                    if (!Directory.Exists(dir))
                    {

                        Directory.CreateDirectory(dir);
                    }


                }

                //string sourceFilePath = "C:\\YourFilePath\\YourFile.txt";
                string destinationRarFilePath = IO.Path.Combine(path2, foldername);

                // Step 1: Compress the file into a RAR file using WinRAR
                CompressToRar(path, destinationRarFilePath);



                // DownloadFolder(foldername);

                return foldername;
                //var psi = new ProcessStartInfo
                //{
                //    FileName = path,
                //    UseShellExecute = true
                //};
                //Process.Start(psi);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }




        }

        public IActionResult DownloadFolder(string id)
        {
            bool http = HttpContext.Request.IsHttps;
            string httpval = "https://";
            if (!http)
            {
                httpval = "http://";
            }

            //string completeUrl = HttpContext.Request.GetEncodedUrl();
            string serverName = HttpContext.Request.Host.Value;
            // string 

            serverName = httpval + serverName + "/Files/WINRAR/" + id + "";
            return Redirect(serverName);

        }

        public decimal ComputeDeduction(decimal HdrTotalAmt, decimal HPDeduction)
        {
            return HdrTotalAmt - HPDeduction;
        }


        public IActionResult BatchRequisitionPayment(string POList)
        {
            //List<string> _POList = new List<string>();
            //List<qryRequestPaymentRequestbyStatus> _RequestList = new List<qryRequestPaymentRequestbyStatus>();
            //BatchRequisitionViewModel model = new BatchRequisitionViewModel();
            //_POList = JsonConvert.DeserializeObject<List<string>>(POList);
            //_RequestList = JsonConvert.DeserializeObject<List<qryRequestPaymentRequestbyStatus>>(Model);

            //List<qryBatchPaymentHdr> _SelectedPayment = new List<qryBatchPaymentHdr>();

            //foreach (var item in _RequestList) //Where(a => _POList.Contains(a.PONo.ToUpper())))
            //{
            //    if (string.IsNullOrEmpty(item.PONo))
            //    {
            //        continue;
            //    }
            //    for (int i = 0; i < _POList.Count; i++)
            //    {
            //        if (item.PONo.ToUpper().Equals(_POList[i].ToUpper()))
            //        {
            //            qryBatchPaymentHdr _qryBatchPaymentHdr = new qryBatchPaymentHdr
            //            {
            //                PONo = item.PONo,
            //                PayeeName = item.Vendor,
            //                Amount = item.Amount,
            //                SalesInvoiceNo = "",
            //                SalesInvoiceDate = DateTime.Now,
            //                DeliveryNo = "",
            //                DeliveryDate = DateTime.Now,
            //                ReferenceReceiptNo = "",
            //                HPDeduction = item.Deduction,
            //                FreightAmount = 0.00m,
            //            };
            //            _SelectedPayment.Add(_qryBatchPaymentHdr);
            //        }
            //    }
            //}

            //model.BatchPaymentHdrList = _SelectedPayment;

            //return View("BatchRFP", model);

            // Construct the URL you want to redirect to
            var redirectUrl = Url.Action("ForPayment", new { PO = POList });

            // Return the URL as JSON
            return Json(new { redirectUrl });
        }

        public async Task<IActionResult> ForPayment(string PO)
        {
            BatchRequisitionViewModel model = new BatchRequisitionViewModel();
            List<string> _POList = new List<string>();
            _POList = JsonConvert.DeserializeObject<List<string>>(PO);

            List<qryBatchPaymentHdr> _RequestList = new List<qryBatchPaymentHdr>();

            foreach (string _PO in _POList)
            {
                if (string.IsNullOrEmpty(_PO))
                {
                    continue;
                }
                TblPurchaseorderhdr _POHdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(BaseUrlRepo, _PO);

                //IList<qryRequisitionItem> _MainReqItem = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionItemList(BaseUrlRepo, _POHdr.Reqno);

                //List<qryRequisitionItem> _ReqItemList = new List<qryRequisitionItem>();
                //_ReqItemList.AddRange(_MainReqItem);

                //IList<qryRequisitionInfo> _PyReqList = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionListInfoByMainReqNo(BaseUrlRepo, _POHdr.Reqno);
                qryRequisitionInfo _ReqInfo = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionInfo(BaseUrlRepo, _POHdr.Reqno);
                qryBatchPaymentHdr _qryBatchPaymentHdr = new qryBatchPaymentHdr
                {
                    PONo = _PO,
                    PayeeName = _ReqInfo.PayeeName,
                    Amount = 0.00m,
                    SalesInvoiceNo = _ReqInfo.SalesInvoiceNo,
                    SalesInvoiceDate = Convert.ToDateTime(_ReqInfo.SalesInvoiceDate),
                    DeliveryNo = _ReqInfo.DeliveryNo,
                    DeliveryDate = Convert.ToDateTime(_ReqInfo.DeliveryDate),
                    ReferenceReceiptNo = _ReqInfo.RefNo,
                    HPDeduction = _ReqInfo.Deduction,
                    FreightAmount = _ReqInfo.TotalFreight,
                };
                _RequestList.Add(_qryBatchPaymentHdr);
            }
            model.BatchPaymentHdrList = _RequestList;
            //string userid = _userManager.GetUserId(this.User);

            //model.DashBoardViewModel = new DashBoardViewModel();
            //model.DashBoardViewModel.RequestList = await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-BATCHREQUESTER", userid);
            return View("BatchRFP", model);
        }
        public async Task<IActionResult> List()
        {
            BatchRequisitionViewModel model = new BatchRequisitionViewModel();
            string userid = _userManager.GetUserId(this.User);

            model.RequisitionViewModel = new RequisitionViewModel();
            model.RequisitionViewModel.dashboardViewModel = new DashBoardViewModel();
            model.RequisitionViewModel.dashboardViewModel.RequestList = await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-BATCHREQUESTER", userid, BaseUrlRepo);
            model.RequisitionViewModel.dashboardViewModel.RequestDepartmentList = await _RepositoryUnit.PRAuthorizationRepository.GetRequisitionDepartment(userid, BaseUrlRepo);
            return View("ActiveRequests", model);
        }
        public IActionResult Create()
        {
            BatchRequisitionViewModel model = new BatchRequisitionViewModel();
            model._BatchSummaryList = new List<qryPaymentRequestHdr>();
            model._BatchItemList = new List<qryPRBatchItems>();
            model.BatchFileName = string.Empty;
            return View("BatchRequisition", model);
        }

        //public IActionResult ReadAttachments(List<IFormFile> files, string list, string vm)
        //{
        //    BatchRequisitionViewModel model = JsonConvert.DeserializeObject<BatchRequisitionViewModel>(vm);

        //    model.RequestAttachments = new List<qryRequestAttachments>();

        //    IList<string> list1 = JsonConvert.DeserializeObject<List<string>>(list);

        //    foreach (var item in list1)
        //    {
        //        ReadAttachmentsFromFolder(item, files, model.RequestAttachments);
        //    }

        //    return View("_BatchSummaryList", model);
        //}

        [DisableRequestSizeLimit]
        private void ReadAttachmentsFromFolder(string PONo, string Primary, List<IFormFile> files, IList<qryRequestAttachments> requestAttachments)
        {
            foreach (var file in files)
            {
                string[] filenames = file.FileName.Split('/');

                if (filenames.Count() < 4)
                {
                    continue;
                }

                if (PONo.Equals(filenames[1].ToUpper()) && Primary.Equals(filenames[2]))
                {
                    string filename = Path.GetFileName(file.FileName);

                    string src = CopyFileToTmp(file);
                    src = "/Requisition/DocumentViewer?url=" + Utilities.EncodeBase64(src);
                    requestAttachments.Add(new qryRequestAttachments() { RequestNo = PONo + Primary, FileName = filename, Src = src });

                    //string filename = Path.GetFileName(file.FileName);
                    //requestAttachments.Add(new qryRequestAttachments() { RequestNo = PONo + Primary, FileName = filename });
                }
            }
        }

        private string CopyFileToTmp(IFormFile file)
        {
            string fname = file.FileName.Replace("#", "");

            string copyDestination = ReqPathDisplay + "\\" + System.IO.Path.GetFileName(fname);
            string src = SrcFilePath + System.IO.Path.GetFileName(fname);

            src = src.Replace("\\", "/");

            using (var localFile = System.IO.File.OpenWrite(copyDestination.Replace("#", "")))
            using (var uploadedFile = file.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }

            return src;
        }

        private void ReadAttachmentsFromFolder(string CompanyDept, List<IFormFile> files, IList<qryRequestAttachments> requestAttachments)
        {
            foreach (var file in files)
            {
                string[] filenames = file.FileName.Split('/');

                if (filenames.Count() < 3)
                {
                    continue;
                }


                if (CompanyDept.Replace(".", "").Equals(filenames[1].ToUpper().Replace(".", "") + " - " + filenames[2].ToUpper().Replace(".", "")))
                {
                    string filename = Path.GetFileName(file.FileName);

                    string src = CopyFileToTmp(file);
                    src = "/Requisition/DocumentViewer?url=" + Utilities.EncodeBase64(src);
                    requestAttachments.Add(new qryRequestAttachments() { RequestNo = CompanyDept, FileName = filename, Src = src });
                }
            }
        }

        public decimal GetTotalAmount(decimal _TotalAmount, int _Quantity)
        {
            return _TotalAmount * _Quantity;
        }

        public async Task<IActionResult> Details(string id = "")
        {
            string BatchRequestNo = id;

            BatchRequisitionViewModel model = new BatchRequisitionViewModel();
            RepositoryUnit _RepositoryUnit = new RepositoryUnit();

            model.DashBoardViewModel = new DashBoardViewModel();
            model.DashBoardViewModel.RequestJourney = await _RepositoryUnit.PRAuthorizationRepository.GetPaymentRequestAuthListByPRNO(BatchRequestNo, BaseUrlRepo);
            model.DashBoardViewModel.RequestNo = BatchRequestNo;

            model.BatchPRNo = BatchRequestNo;
            model._BatchSummaryList = new List<qryPaymentRequestHdr>();

            model.RequisitionViewModel = new RequisitionViewModel();
            model.RequisitionViewModel.dashboardViewModel = new DashBoardViewModel();
            model.RequisitionViewModel.dashboardViewModel.RequestList = await _RepositoryUnit.PRAuthorizationRepository.GetRequestPaymentRequestbyStatus("PD-BATCHREQUEST", BatchRequestNo, BaseUrlRepo);


            if (string.IsNullOrEmpty(BatchRequestNo))
            {
                RedirectToAction("/");
            }

            return View("BatchRequestDetails", model);
        }


        public async Task<IActionResult> ReadBatchTemplate(List<IFormFile> files)
        {
            string error = string.Empty;
            string path1 = string.Empty;
            string Debug = string.Empty;
            try
            {
                //var http = _httpClientFactory.CreateClient("SPASv2Api");

                BatchRequisitionViewModel _model = new BatchRequisitionViewModel();
                BatchUploadParams _BatchUploadParams = new BatchUploadParams();
                RequisitionParams _RequisitionParams = new RequisitionParams();
                string extension = System.IO.Path.GetExtension(files[0].FileName).ToLower();
                string connString = "";

                _model._BatchSummaryList = new List<qryPaymentRequestHdr>();
                _model._BatchItemList = new List<qryPRBatchItems>();

                string[] validFileTypes = { ".xls", ".xlsx" };

                path1 = Path.Combine(@"C:\Tmp\SPASv2\", files[0].FileName);
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(@"C:\Tmp\SPASv2\");
                }

                Debug = "Passed creating directory";

                if (validFileTypes.Contains(extension))
                {
                    if (System.IO.File.Exists(path1))
                    { System.IO.File.Delete(path1); }


                    using (var localFile = System.IO.File.OpenWrite(path1))
                    using (var uploadedFile = files[0].OpenReadStream())
                    {
                        uploadedFile.CopyTo(localFile);
                        _model.BatchFilePath = localFile.Name;
                    }

                    Debug = "Passed reading stream";

                    DataTable dtRequest = new DataTable();
                    DataTable dtExcelVer = new DataTable();

                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    dtRequest = Utilities.ConvertXSLXtoDataTable(path1, connString, "Requisition Template", ref Debug);

                    Debug = Debug + dtRequest.Rows.Count + path1 + " Passed reading excel";
                    string _PayClassCode = await _RepositoryUnit.RefPayClassRepository.GetPayclassCodeByDesc(BaseUrlRepo, dtRequest.Rows[0][0].ToString());

                    Debug = "Passed Payclass code";
                    _BatchUploadParams.Payclass = _PayClassCode;
                    _BatchUploadParams.qryBatchRequistions = new List<qryBatchRequistion>();
                    _BatchUploadParams.qryBatchUploadExcel = new qryBatchUploadExcel();
                    //_BatchUploadParams.TblBatchPRHdr = new TblBatchPRHdr(); 
                    _BatchUploadParams.TblResponse = new TblResponse();
                    _BatchUploadParams.UserID = _userManager.GetUserId(this.User);
                    Debug = "Passed User";
                    //_BatchUploadParams.FilesAttached = new List<IFormFile>();
                    for (int dr = 2; dr < dtRequest.Rows.Count; dr++)
                    {
                        if (!string.IsNullOrEmpty(dtRequest.Rows[dr][0].ToString()))
                        {
                            qryBatchRequistion qryBatchRequistion = new qryBatchRequistion()
                            {
                                CompanyType = dtRequest.Rows[dr][0].ToString().ToUpper(),
                                Department = dtRequest.Rows[dr][1].ToString().ToUpper(),
                                VendorName = dtRequest.Rows[dr][2].ToString().ToUpper(),
                                ReferenceNo = dtRequest.Rows[dr][3].ToString(),
                                ItemDesc = dtRequest.Rows[dr][4].ToString().ToUpper(),
                                Qty = Convert.ToInt32(dtRequest.Rows[dr][5].ToString()),
                                Disc = Convert.ToDecimal(string.IsNullOrEmpty(dtRequest.Rows[dr][6].ToString()) ? "0" : dtRequest.Rows[dr][6].ToString()),
                                Remarks = dtRequest.Rows[dr][7].ToString(),
                                AmountPerUnit = Convert.ToDecimal(string.IsNullOrEmpty(dtRequest.Rows[dr][8].ToString()) ? "0" : dtRequest.Rows[dr][8].ToString())
                            };
                            _BatchUploadParams.qryBatchRequistions.Add(qryBatchRequistion);
                        }
                    }

                    Debug = "Passed Loop";
                    _BatchUploadParams = await _RepositoryUnit.PRBatchUploadRepository.CanUploadExcelDetails(BaseUrlRepo, _BatchUploadParams);

                    if (_BatchUploadParams.TblResponse.Status == "FAILED")
                    {
                        error = _BatchUploadParams.TblResponse.ErrorMessage;
                        goto FAILED;
                    }
                    Debug = "Passed Can upload";

                    //_BatchUploadParams = await _RepositoryUnit.TblRequisitionRepository.ReadRequisitionList(BaseUrlRepo, _BatchUploadParams);

                    Debug = _BatchUploadParams.TblResponse.ErrorMessage;

                    _model.BatchFileName = files[0].FileName;
                    _model.TblRequisitionhdr = _BatchUploadParams.TblRequisitionhdrList.ToList();
                    _model.TblRequisitiondtl = _BatchUploadParams.TblRequisitiondtlList.OrderBy(a => a.CompanyCode).ToList();
                    _model.qryRequisition = _BatchUploadParams.qryRequisitions.OrderBy(a => a.CompanyCode).ThenBy(a => a.PayeeName).ToList();
                    _model.BatchFileName = files[0].FileName;
                    var sorted = _BatchUploadParams.qryRequisitions.GroupBy(a => new { a.CompanyDesc, a.VendorDesc, a.CompanyType })
                   .Select(a => new qryRequisitionVendorCompanyChapel { CompanyType = a.Key.CompanyType, CompanyDesc = a.Key.CompanyDesc, VendorName = a.Key.VendorDesc }).ToList();
                    _model.RequestNoList = new List<string>();
                    _model.qryRequisitionHdrList = _BatchUploadParams.qryRequisitionHdr;

                    Debug = "Passed ReadBatchTemplate function";
                    _model.FileDirectory = CreateFolderTemplateB(sorted);
                    ViewData["FileDir"] = _model.FileDirectory;
                    ViewData["devtype"] = DevelopmentType;
                    _model.isUploadTemplate = true;
                    _model.ExcelFileUploadPath = path1;

                    return View("_BatchRequisitionList", _model);
                }
            }
            catch (Exception err)
            {
                return Json(new
                {
                    success = false,
                    error = err.Message + " " + Debug
                },
                    new JsonSerializerOptions());
            }

        FAILED:
            return Json(new
            {
                success = false,
                error = error
            }, new JsonSerializerOptions());
        }

        //public async Task<IActionResult> ReadBatchTemplateOld(List<IFormFile> files)
        //{
        //    string error = string.Empty;
        //    string path1 = string.Empty;
        //    string Debug = string.Empty;
        //    try
        //    {
        //        var http = _httpClientFactory.CreateClient("SPASv2Api");

        //        //string requestAddress = URL + "/Rudy/CanUploadExcelDetails";

        //        //BatchUploadParams response = await UtilitiesHttpClient<BatchUploadParams>.PostAsyncT<BatchUploadParams>(_BatchUploadParams, requestAddress);
        //        //return response.TblResponse;



        //        BatchRequisitionViewModel _model = new BatchRequisitionViewModel();
        //        BatchUploadParams _BatchUploadParams = new BatchUploadParams();
        //        RequisitionParams _RequisitionParams = new RequisitionParams();
        //        string extension = System.IO.Path.GetExtension(files[0].FileName).ToLower();
        //        string connString = "";

        //        string[] validFileTypes = { ".xls", ".xlsx" };

        //        //string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

        //        //string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files[0].FileName);

        //        // path1 = string.Format("{0}/{1}", @"C:\Tmp\SPASv2\", files[0].FileName);
        //        path1 = Path.Combine(@"C:\Tmp\SPASv2\", files[0].FileName);
        //        if (!Directory.Exists(path1))
        //        {
        //            //Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
        //            Directory.CreateDirectory(@"C:\Tmp\SPASv2\");
        //        }

        //        Debug = "Passed creating directory";

        //        if (validFileTypes.Contains(extension))
        //        {
        //            if (System.IO.File.Exists(path1))
        //            { System.IO.File.Delete(path1); }


        //            using (var localFile = System.IO.File.OpenWrite(path1))
        //            using (var uploadedFile = files[0].OpenReadStream())
        //            {
        //                uploadedFile.CopyTo(localFile);
        //                _model.BatchFilePath = localFile.Name;
        //            }

        //            Debug = "Passed reading stream";

        //            DataTable dtRequest = new DataTable();
        //            DataTable dtExcelVer = new DataTable();
        //            //if (extension.Trim() != ".xls")
        //            //{
        //            //    throw new Exception("Invalid File Type");
        //            //}


        //            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
        //            dtRequest = Utilities.ConvertXSLXtoDataTable(path1, connString, "Requisition Template", ref Debug);
        //            //   dtExcelVer = Utilities.ConvertXSLXtoDataTable(path1, connString, "SystemVer",ref Debug);

        //            //foreach (DataRow row in dtRequest.Rows)
        //            //{
        //            //    foreach (DataColumn col in dtRequest.Columns)
        //            //    {
        //            //        string sssssss = ($"Raw Value in {col.ColumnName}{row.Table.Rows.IndexOf(row) + 1}: {row[col]}");
        //            //    }
        //            //}


        //            Debug = Debug + dtRequest.Rows.Count + path1 + " Passed reading excel";
        //            //ViewBag.Data = dt;

        //            //excel version checking
        //            //_BatchUploadParams.qryBatchUploadExcel.Version = dtExcelVer.Rows[0][0].ToString();
        //            //_BatchUploadParams.qryBatchUploadExcel.VersionDate = Convert.ToDateTime(dtExcelVer.Rows[1][0].ToString());
        //            //_BatchUploadParams.qryBatchUploadExcel.PayClass = dtRequest.Rows[0][0].ToString();
        //            string _PayClassCode = await _RepositoryUnit.RefPayClassRepository.GetPayclassCodeByDesc(BaseUrlRepo, dtRequest.Rows[0][0].ToString());
        //            _BatchUploadParams.Payclass = _PayClassCode;
        //            _BatchUploadParams.qryBatchRequistions = new List<qryBatchRequistion>();
        //            _BatchUploadParams.qryBatchUploadExcel = new qryBatchUploadExcel();
        //            //_BatchUploadParams.TblBatchPRHdr = new TblBatchPRHdr(); 
        //            _BatchUploadParams.TblResponse = new TblResponse();
        //            //_BatchUploadParams.FilesAttached = new List<IFormFile>();
        //            for (int dr = 2; dr < dtRequest.Rows.Count; dr++)
        //            {
        //                if (!string.IsNullOrEmpty(dtRequest.Rows[dr][0].ToString()))
        //                {
        //                    qryBatchRequistion qryBatchRequistion = new qryBatchRequistion()
        //                    {
        //                        CompanyType = dtRequest.Rows[dr][0].ToString(),
        //                        Department = dtRequest.Rows[dr][1].ToString(),
        //                        VendorName = dtRequest.Rows[dr][2].ToString(),
        //                        ReferenceNo = dtRequest.Rows[dr][3].ToString(),
        //                        ItemDesc = dtRequest.Rows[dr][4].ToString(),
        //                        Qty = Convert.ToInt32(dtRequest.Rows[dr][5].ToString()),
        //                        Disc = Convert.ToDecimal(dtRequest.Rows[dr][6].ToString()),
        //                        Remarks = dtRequest.Rows[dr][7].ToString(),
        //                        AmountPerUnit = Convert.ToDecimal(string.IsNullOrEmpty(dtRequest.Rows[dr][8].ToString()) ? "2" : dtRequest.Rows[dr][8].ToString())

        //                    };
        //                    _BatchUploadParams.qryBatchRequistions.Add(qryBatchRequistion);
        //                }

        //            }

        //            var _CanUpload = await _RepositoryUnit.PRBatchUploadRepository.CanUploadExcelDetails(BaseUrlRepo, _BatchUploadParams);
        //            if (_CanUpload.Status == "FAILED")
        //            {
        //                error = _CanUpload.ErrorMessage;
        //                goto FAILED;
        //            }


        //            _model._BatchSummaryList = new List<qryPaymentRequestHdr>();
        //            _model._BatchItemList = new List<qryPRBatchItems>();

        //            List<qryRequisition> _qryRequisitionList = new List<qryRequisition>();

        //            //collect List<qryPaymentRequestHdr> _qryPaymentRequestHdr_List = new List<qryPaymentRequestHdr>();

        //            Debug = "starting to do loop on Data table";
        //            for (int dr = 2; dr < dtRequest.Rows.Count; dr++)
        //            {
        //                if (string.IsNullOrEmpty(dtRequest.Rows[dr][2].ToString()))
        //                {
        //                    Debug = "Empty details";
        //                    break;
        //                }

        //                string _CompanyType = dtRequest.Rows[dr][0].ToString();
        //                string _Department = dtRequest.Rows[dr][1].ToString();
        //                string _VendorName = dtRequest.Rows[dr][2].ToString();
        //                string _ReferenceNo = dtRequest.Rows[dr][3].ToString();
        //                string _ItemDesc = dtRequest.Rows[dr][4].ToString();
        //                int _Qty = Convert.ToInt32(dtRequest.Rows[dr][5].ToString());
        //                decimal _Disc = Convert.ToDecimal(dtRequest.Rows[dr][6].ToString());
        //                string _Remarks = dtRequest.Rows[dr][7].ToString();
        //                decimal _AmountPerUnit = Convert.ToDecimal(string.IsNullOrEmpty(dtRequest.Rows[dr][8].ToString()) ? "2" : dtRequest.Rows[dr][8].ToString());


        //                qryCompanyDetails _qryCompanyDetails = new qryCompanyDetails();
        //                qryComputeBreakdown _qryComputeBreakdown = new qryComputeBreakdown();
        //                Debug = "Starting to query get company details" + OSPUrlRepo + " " + _CompanyType + " " + _Department;
        //                _qryCompanyDetails = await _RepositoryUnit.CompanyRepository.GetCompanyDetails(OSPUrlRepo, _CompanyType, _Department);
        //                Debug = "Getcompany details passed";

        //                string _VendorCode = string.Empty;
        //                string _ItemCode = string.Empty;
        //                qryVendorDetails _qryVendorDetails = new qryVendorDetails();
        //                TblVendorItems _TblVendorItems = new TblVendorItems();
        //                _VendorCode = await _RepositoryUnit.VendorRepository.GetVendorCodeByDisplayName(BaseUrlRepo, _VendorName.Replace("'", "`"));
        //                _ItemCode = await _RepositoryUnit.RefItemRepository.GetItemCodeByDesc(BaseUrlRepo, _ItemDesc);
        //                _qryVendorDetails = await _RepositoryUnit.VendorRepository.GetVendorDetails(BaseUrlRepo, _VendorCode, _PayClassCode);
        //                _TblVendorItems = await _RepositoryUnit.VendorRepository.GetVendorItemsDetails(BaseUrlRepo, _VendorCode, _ItemCode);
        //                _TblVendorItems.Amount = _AmountPerUnit; //TEMPORARY ONLY!!! SHOULD BE REMOVE BEFORE WE LIVE
        //                //_qryComputeBreakdown = await _ServiceUnit.RequisitionService.ComputeBreakDown(_Qty, _TblVendorItems.Amount,1.12m,_Disc,"002");
        //                if (_TblVendorItems is null)
        //                {
        //                    Debug = _ItemDesc + " " + _qryVendorDetails.VendorName;
        //                }
        //                qryRequisition _req = new qryRequisition()
        //                {
        //                    UserCompanyCode = _qryCompanyDetails.CompanyCode,
        //                    UserDeptCode = _qryCompanyDetails.DeptCode,
        //                    RequestDate = DateTime.Now,
        //                    PayClassCode = _PayClassCode,
        //                    VendorCode = _qryVendorDetails.VendorCode,//await _RepositoryUnit.VendorRepository.GetVendorCodeByDisplayName(BaseUrlRepo, _VendorName), //"202312000001",
        //                    VendorDesc = _qryVendorDetails.VendorName,
        //                    PayeeName = _VendorName,
        //                    PayMethodCode = _qryVendorDetails.PaymethodCode,
        //                    BankCode = _qryVendorDetails.BankCode,
        //                    //Destination = "091234567",
        //                    Destination = "",
        //                    TotalAmount = 0.00m,//GetTotalAmount(_TblVendorItems.Amount, _Qty),
        //                    //DtlTotalAmount = _TblVendorItems.Amount,
        //                    Remarks = _Remarks,
        //                    RefNo = _ReferenceNo,
        //                    CompanyCode = _qryCompanyDetails.CompanyCode,
        //                    CompanyDesc = _qryCompanyDetails.CompanyDesc,
        //                    DeptCode = _qryCompanyDetails.DeptCode,
        //                    ItemDesc = _ItemDesc,
        //                    ItemCode = _TblVendorItems.ItemCode,
        //                    Unit = _TblVendorItems.UOM,
        //                    Price = _TblVendorItems.Amount,
        //                    Quantity = _Qty,
        //                    Gross = _TblVendorItems.Amount,
        //                    VatRate = 0.00m,
        //                    VAT = 0.00m,
        //                    NetOfVAT = 0.00m,
        //                    TotalTax = 0.00m,
        //                    Discount = _Disc,
        //                    CompanyType = _CompanyType,
        //                    AuditUser = _userManager.GetUserId(this.User),
        //                    isVendorVat = _qryVendorDetails.isVat,
        //                    //PayDesc = "LOCAL OUTSOURCED CASKETS",//dr[0].ToString(),
        //                    //PRNo = "",
        //                    //CompanyType = dtRequest.Rows[dr][0].ToString(),
        //                    //DeptDesc = _qryCompanyDetails.DeptDesc,
        //                    //DeptCode = _qryCompanyDetails.DeptCode,
        //                    //CompanyCode = _qryCompanyDetails.CompanyCode,
        //                    ////DisplayName = dr[4].ToString(),
        //                    //PayeeName = dtRequest.Rows[dr][2].ToString(),
        //                    ////PayMethodType = dtRequest.Rows[dr][5].ToString(),
        //                    ////BankName = dtRequest.Rows[dr][6].ToString(),
        //                    //ReferenceNo = dtRequest.Rows[dr][3].ToString(),
        //                    //Remarks = dtRequest.Rows[dr][4].ToString(),
        //                    //TotalAmount = GetTotalAmount(Convert.ToDecimal(dtRequest.Rows[dr][6]), Convert.ToInt32(dtRequest.Rows[dr][8]))
        //                    ////TotalAmount = Convert.ToDecimal(dtRequest.Rows[dr][10].ToString())
        //                    ////TotalAmount = _ReqTotalAmt
        //                };
        //                Debug = "adding header passed";
        //                _qryRequisitionList.Add(_req);

        //            }
        //            Debug = "finish loop on Data table";


        //            TblResponse _TblResponse = new TblResponse();

        //            _RequisitionParams = await _ServiceUnit.RequisitionService.GroupRequisitionHdrDtl(BaseUrlService, _qryRequisitionList);
        //            Debug = "Passed Requisition Grouping";
        //            _model.TblRequisitionhdr = _RequisitionParams.RequisitionHdrList.ToList();
        //            _model.TblRequisitiondtl = _RequisitionParams.RequisitionDtlList.OrderBy(a => a.CompanyCode).ToList();
        //            _model.qryRequisition = _qryRequisitionList.OrderBy(a => a.CompanyCode).ThenBy(a => a.PayeeName).ToList();
        //            _model.BatchFileName = files[0].FileName;
        //            var sorted = _qryRequisitionList.GroupBy(a => new { a.CompanyDesc, a.VendorDesc, a.CompanyType })
        //                .Select(a => new qryRequisitionVendorCompanyChapel { CompanyType = a.Key.CompanyType, CompanyDesc = a.Key.CompanyDesc, VendorName = a.Key.VendorDesc }).ToList();
        //            _model.RequestNoList = new List<string>();

        //            //foreach (var item in _model._BatchSummaryList)
        //            //{
        //            //    _model.RequestNoList.Add(item.PRNo);
        //            //}
        //            List<qryRequisitionVendorCompanyChapel> qryRVCC = new List<qryRequisitionVendorCompanyChapel>();

        //            List<qryRequisitionHdr> _qryRequisitionHdrList = new List<qryRequisitionHdr>();

        //            for (int i = 0; i < _model.TblRequisitionhdr.Count; i++)
        //            {
        //                qryRequisitionHdr _qryRequisitionHdr = new qryRequisitionHdr()
        //                {
        //                    CompanyName = await _RepositoryUnit.CompanyRepository.GetCompanyDescByCompanyCode(OSPUrlRepo, _model.TblRequisitionhdr[i].CompanyCode),
        //                    PayeeName = _model.TblRequisitionhdr[i].PayeeName,
        //                    VendorName = await _RepositoryUnit.VendorRepository.GetVendorNameByVendorCode(BaseUrlRepo, _model.TblRequisitionhdr[i].VendorCode),
        //                    Amount = _model.TblRequisitionhdr[i].TotalAmount,
        //                    Remarks = _model.TblRequisitionhdr[i].Remarks,
        //                    Attachment = "",
        //                    CompanyCode = _model.TblRequisitionhdr[i].CompanyCode,
        //                    DeptCode = _model.TblRequisitionhdr[i].DeptCode,
        //                    CompanyType = await _RepositoryUnit.CompanyRepository.GetCompanyType(OSPUrlRepo, _model.TblRequisitionhdr[i].CompanyCode),
        //                };

        //                //qryRequisitionVendorCompanyChapel qryrvcc = new qryRequisitionVendorCompanyChapel()
        //                //{
        //                //    VendorName = _qryRequisitionHdr.VendorName,
        //                //    CompanyType = _qryRequisitionHdr.CompanyType,
        //                //    DeptCode = _qryRequisitionHdr.DeptCode
        //                //}
        //                _qryRequisitionHdrList.Add(_qryRequisitionHdr);
        //            }

        //            _model.qryRequisitionHdrList = _qryRequisitionHdrList;


        //            Debug = "Passed ReadBatchTemplate function";

        //            _model.FileDirectory = CreateFolderTemplateB(sorted); ;

        //            ViewData["FileDir"] = _model.FileDirectory;

        //            ViewData["devtype"] = DevelopmentType;


        //            _model.isUploadTemplate = true;
        //            _model.ExcelFileUploadPath = path1;

        //            return View("_BatchRequisitionList", _model);
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        return Json(new
        //        {
        //            success = false,
        //            error = err.Message + " " + Debug
        //        },
        //            new JsonSerializerOptions());

        //    }

        //FAILED:

        //    return Json(new
        //    {
        //        success = false,
        //        error = error
        //    }, new JsonSerializerOptions());
        //}

        private string CreateFolderTemplate(IList<qryRequisitionHdr> ReqList)
        {
            string path = @"C:\SPASv2\Files\Requisition\" + DateTime.Now.ToString("yyyy-MM-dd hhmmsstt").ToUpper() + @"\";

            foreach (var item in ReqList)
            {
                string dir = path + @"\" + item.CompanyType + " - " + item.DeptCode;

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }

            //var psi = new ProcessStartInfo
            //{
            //    FileName = path,
            //    UseShellExecute = true
            //};
            //Process.Start(psi);

            return path;
        }

        public async Task<IActionResult> UploadBatchPayment(List<IFormFile> files, string vm)
        {
            string Debug = string.Empty;
            string error = string.Empty;
            string DtlCompanyCode = string.Empty;
            try
            {
                TblResponse resp = new TblResponse();
                BatchRequisitionViewModel _batchReq = JsonConvert.DeserializeObject<BatchRequisitionViewModel>(vm);
                BatchUploadParams _BatchUploadParams = new BatchUploadParams();

                _batchReq.TblRequisitionhdr = new List<TblRequisitionhdr>();
                string _UserID = _userManager.GetUserId(this.User);
                TblRequisitionhdr _TblRequisitionhdr_old = new TblRequisitionhdr();
                //RequisitionParams _requisitionParams = new RequisitionParams();
                //TblLoanhdr _TblLoanhdr = new TblLoanhdr();
                //_requisitionParams.tblLoanhdrs = new List<TblLoanhdr>();
                //_requisitionParams.TblRequisitionDtlSummary = new List<TblRequisitionDtlSummary>();
                //_requisitionParams.RequisitionDtlList = new List<TblRequisitiondtl>();

                //_requisitionParams.RequisitionHdrList = _batchReq.TblRequisitionhdr.ToList();
                //process = "GetLatestPRBatchNo";

                _TblRequisitionhdr_old = await _RepositoryUnit.PRBatchUploadRepository.GetLatestPRBatchNo(BaseUrlRepo);
                string _ReqBatch = string.Empty;
                //_requisitionParams.LastNo = _TblRequisitionhdr_old.BatchNo;

                process = "GenerateNewPRBatchNo";
                //_ReqBatch = await GeneratePRBatchNo(_TblRequisitionhdr_old, _requisitionParams);//await _ServiceUnit.PaymentRequestService.GenerateNewPRBatchNo(BaseUrlService, _requisitionParams);

                //_batchReq.BatchPRNo = _ReqBatch;
                _BatchUploadParams.UserID = _UserID;
                _BatchUploadParams.qryBatchPaymentHdrList = _batchReq.BatchPaymentHdrList.ToList();
                _BatchUploadParams.qryBatchPaymentDtlList = _batchReq.BatchPaymentDtlList.ToList();

                _BatchUploadParams = await _RepositoryUnit.TblRequisitionRepository.InsertBatchPaymentList(BaseUrlRepo, _BatchUploadParams);
                resp = _BatchUploadParams.TblResponse;

                _ReqBatch = _BatchUploadParams.TblRequisitionhdrList.Select(x => x.BatchNo).FirstOrDefault();

                for (int i = 0; i < _BatchUploadParams.qryBatchPaymentHdrList.Count; i++)
                {
                    foreach (var item in _BatchUploadParams.TblRequisitionhdrList.Where(a => a.RefNo.Equals(_BatchUploadParams.qryBatchPaymentHdrList[i].ReferenceReceiptNo)))
                    {
                        List<IFormFile> _reqAttachments = new List<IFormFile>();

                        foreach (var file in files)
                        {
                            string[] filenames = file.FileName.Split('/');

                            if (filenames.Count() < 4)
                            {
                                continue;
                            }

                            TblPurchaseorderhdr _reqPOhdr = _BatchUploadParams.TblPurchaseorderhdrList.Where(a => a.Reqno.Equals(item.MainReqNo)).FirstOrDefault();

                            if (_reqPOhdr.PONo.Equals(filenames[1].ToUpper()) && _BatchUploadParams.qryBatchPaymentHdrList[i].SalesInvoiceNo.Equals(filenames[2]))
                            {
                                _reqAttachments.Add(file);
                            }
                        }
                        UploadFiles(_reqAttachments, item.Reqno);
                    }
                }

                //SAVING EXCEL FILE
                System.IO.File.Copy(_batchReq.ExcelFileUploadPath, Path.Combine(ExcelFileCopyPath, _ReqBatch + " - PY.xlsx"));

                //var result = await new AuthorizationController(_AuthLogger, _userManager, _configuration, _env).SendEmailAuthorization_PRNO_Batch(_batchReq.TblRequisitionhdr.Select(a => a.Reqno).ToList(), _UserID);
                //var result = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qryUpdateStatusAuth, BaseUrlRepo);

                //AuthorizationParams _authorizationParams = new AuthorizationParams();
                //_authorizationParams.ReqNo = _BatchUploadParams.TblRequisitionhdrList.Select(a => a.Reqno).ToList();
                //_authorizationParams.UserCode = _UserID;
                //_authorizationParams.ReqType = "PY";

                //var result = await _RepositoryUnit.PRAuthorizationRepository.ProcessAuthorization(_authorizationParams, BaseUrlRepo);
                //process = "Batch successfully uploaded";

                //OkObjectResult okResult = (OkObjectResult)result.Status;
                //TblResponse _rsp = (TblResponse)okResult.Value;
                //resp = _rsp;


                //resp.ErrorMessage = process;
                _batchReq.BatchPRNo = _BatchUploadParams.TblRequisitionhdrList.Select(a => a.BatchNo).FirstOrDefault();
                //_BatchUploadParams.TblResponse = new TblResponse
                //{
                //    Status = "SUCCESS",
                //    AuditDate = DateTime.Now,
                //    ErrorMessage = "SUCCESS",
                //    MethodName = "SUCCESS",
                //    TrxNo = "1",
                //    UniqueInfo = "1"
                //};
                return Json(new { response = _BatchUploadParams.TblResponse, batchreq = _batchReq, success = true },
                    new JsonSerializerOptions()); ;
            }

            catch (Exception err)
            {
                return Json(new
                {
                    success = false,
                    error = err.Message + " " + Debug
                },
                    new JsonSerializerOptions());
            }
        }

        public async Task<IActionResult> UploadBatchPaymentOld(List<IFormFile> files, string vm)
        {
            string Debug = string.Empty;
            string error = string.Empty;
            string DtlCompanyCode = string.Empty;
            try
            {
                TblResponse resp = new TblResponse();
                BatchRequisitionViewModel _batchReq = JsonConvert.DeserializeObject<BatchRequisitionViewModel>(vm);
                BatchUploadParams _BatchUploadParams = new BatchUploadParams();
                _batchReq.TblRequisitionhdr = new List<TblRequisitionhdr>();
                string _UserID = _userManager.GetUserId(this.User);

                TblRequisitionhdr _TblRequisitionhdr_old = new TblRequisitionhdr();
                RequisitionParams _requisitionParams = new RequisitionParams();
                TblLoanhdr _TblLoanhdr = new TblLoanhdr();
                _requisitionParams.tblLoanhdrs = new List<TblLoanhdr>();
                _requisitionParams.TblRequisitionDtlSummary = new List<TblRequisitionDtlSummary>();
                _requisitionParams.RequisitionDtlList = new List<TblRequisitiondtl>();
                _requisitionParams.RequisitionHdrList = _batchReq.TblRequisitionhdr.ToList();
                process = "GetLatestPRBatchNo";

                _TblRequisitionhdr_old = await _RepositoryUnit.PRBatchUploadRepository.GetLatestPRBatchNo(BaseUrlRepo);
                string _ReqBatch = string.Empty;
                _requisitionParams.LastNo = _TblRequisitionhdr_old.BatchNo;

                process = "GenerateNewPRBatchNo";
                //_ReqBatch = await GeneratePRBatchNo(_TblRequisitionhdr_old, _requisitionParams);//await _ServiceUnit.PaymentRequestService.GenerateNewPRBatchNo(BaseUrlService, _requisitionParams);

                _batchReq.BatchPRNo = _ReqBatch;

                foreach (var item in _batchReq.BatchPaymentHdrList)
                {
                    string _TempReqNo = string.Empty;
                    _TempReqNo = string.Empty;
                    decimal _TotalFreight = 0.00m;

                    process = "GetPOHdrByPONo";
                    TblPurchaseorderhdr _POhdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(BaseUrlRepo, item.PONo);
                    process = "ReadRequestByPRNo";
                    TblRequisitionhdr _oldreq = await _RepositoryUnit.TblRequisitionRepository.ReadRequestByPRNo(BaseUrlRepo, _POhdr.Reqno);

                    List<TblRequisitiondtl> _ReqDtlList = new List<TblRequisitiondtl>();
                    List<qryRequisitionDtl> _qryRequisitionDtl = new List<qryRequisitionDtl>();

                    //TblPurchaseorderhdr _POhdr = new TblPurchaseorderhdr();
                    //TblRequisitionhdr _oldreq = new TblRequisitionhdr();


                    TblRequisitionhdr _oldReqhdr = await _RepositoryUnit.TblRequisitionRepository.GetLatestPRRow(BaseUrlRepo, _oldreq.CompanyCode, _oldreq.DeptCode);
                    string _PRNo = string.Empty;
                    _requisitionParams.LastNo = _oldReqhdr.Reqno;
                    _requisitionParams.CompanyCode = _oldreq.CompanyCode;

                    foreach (var itemDtl in _batchReq.BatchPaymentDtlList.Where(t => t.SalesInvoice == item.SalesInvoiceNo && t.PONo == item.PONo).ToList())
                    {
                        string[] DeptName = itemDtl.Department.Split('-');
                        process = "GetCompanyDetails";
                        qryCompanyDetails _qryCompanyDetails = await _RepositoryUnit.CompanyRepository.GetCompanyDetails(OSPUrlRepo, DeptName[0], DeptName[1]);
                        qryVendorDetails qryVendorDetails = await _RepositoryUnit.VendorRepository.GetVendorDetails(BaseUrlRepo, _oldreq.VendorCode, _oldreq.PayClassCode);
                        string itemcode = await _RepositoryUnit.RefItemRepository.GetItemCodeByDesc(BaseUrlRepo, itemDtl.ItemDescription);
                        //if (_qryCompanyDetails.DeptCode == "DASMAR")
                        //{
                        //}

                        _TotalFreight += itemDtl.FreightAmount;

                        TblRequisitiondtl _origReqDtl =
                            await _RepositoryUnit.TblRequisitionRepository.
                            ReadRequsitionDtlByPRNo
                            (BaseUrlRepo, _oldreq.Reqno, _qryCompanyDetails.CompanyCode,
                            _qryCompanyDetails.DeptCode, itemcode);

                        qryComputeBreakdown _criteria = new qryComputeBreakdown()
                        {
                            Qty = itemDtl.Quantity,
                            Gross = itemDtl.TemPriceAmount,//_origReqDtl.Price,
                            VatRate = 1.12m,
                            Discount = _origReqDtl.Discount,
                            Disccode = "002",
                            isVAT = qryVendorDetails.isVat
                        };
                        //_requisitionParams.qryComputeBreakdown = new qryComputeBreakdown();
                        //_requisitionParams.qryComputeBreakdown = _criteria;
                        qryComputeBreakdown _qryComputeBreakdown = await _ServiceUnit.PaymentRequestService.ComputeBreakDown(_criteria, BaseUrlService);

                        TblRequisitiondtl _reqdtl = new TblRequisitiondtl()
                        {
                            ReqItemNo = 0,
                            ReqNo = _PRNo,
                            CompanyCode = _origReqDtl.CompanyCode,
                            DeptCode = _origReqDtl.DeptCode,
                            ItemCode = _origReqDtl.ItemCode,
                            Unit = _origReqDtl.Unit,
                            Price = itemDtl.TemPriceAmount, //_origReqDtl.Price,
                            Quantity = itemDtl.Quantity,
                            Gross = _qryComputeBreakdown.Gross,
                            VatRate = _qryComputeBreakdown.VatRate,
                            Vat = _qryComputeBreakdown.Vat,
                            NetofVat = _qryComputeBreakdown.NetOfVAT,
                            TotalTax = 0.00m,
                            Discount = _qryComputeBreakdown.Discount,
                            TotalAmount = _qryComputeBreakdown.AmountDue,
                            Deduction = item.HPDeduction,
                            Freight = itemDtl.FreightAmount * itemDtl.Quantity,
                            FreightPerUnit = itemDtl.FreightAmount,
                            Void = false,
                            AuditUser = _UserID,
                            AuditDate = DateTime.Now,
                            UploadStat = false,
                            EditUser = _UserID,
                            EditDate = DateTime.Now
                        };
                        DtlCompanyCode = _origReqDtl.CompanyCode;

                        qryRequisitionDtl _qryReqDtl = new qryRequisitionDtl()
                        {
                            Gross = _reqdtl.Gross,
                            VAT = _reqdtl.Vat,
                            NetOfVAT = _reqdtl.NetofVat,
                            TotalTax = _reqdtl.TotalTax,
                            Discount = _reqdtl.Discount,
                            TotalAmount = _reqdtl.TotalAmount,
                            Deduction = _reqdtl.Deduction
                        };

                        _qryRequisitionDtl.Add(_qryReqDtl);
                        _ReqDtlList.Add(_reqdtl);

                        //process = "PostCreateRequisitionDtl";
                        //await _RepositoryUnit.TblRequisitionRepository.PostCreateRequisitionDtl(BaseUrlRepo, _reqdtl);
                    }

                    qryRequisitionHdrComputation _qryRequisitionHdrComputation = new qryRequisitionHdrComputation();
                    _qryRequisitionHdrComputation = await _ServiceUnit.PaymentRequestService.ComputeBreakDownHdr(BaseUrlService, _qryRequisitionDtl);
                    TblRequisitionhdr _TblRequisitionhdr = new TblRequisitionhdr()
                    {
                        Reqno = string.Empty,
                        MainReqNo = _oldreq.Reqno,
                        BatchNo = _ReqBatch,
                        CompanyCode = _oldreq.CompanyCode,
                        DeptCode = _oldreq.DeptCode,
                        ReqDate = DateTime.Now,
                        PayClassCode = _oldreq.PayClassCode,
                        Active = true,
                        VendorCode = _oldreq.VendorCode,
                        PayeeName = _oldreq.PayeeName,
                        PayMethodCode = _oldreq.PayMethodCode,
                        BankCode = _oldreq.BankCode,
                        Destination = _oldreq.Destination,
                        Vat = _qryRequisitionHdrComputation.Vat,
                        NetofVat = _qryRequisitionHdrComputation.NetOfVat,
                        TotalTax = _qryRequisitionHdrComputation.TotalTax,
                        Deduction = _qryRequisitionHdrComputation.Deduction,
                        Discount = _qryRequisitionHdrComputation.Discount,
                        AmountDue = _qryRequisitionHdrComputation.AmountDue,
                        //TotalAmount = ComputeDeduction(item.Amount, item.HPDeduction),
                        TotalAmount = _qryRequisitionHdrComputation.AmountDue,
                        TotalFreight = item.FreightAmount,
                        TransType = "REG",
                        Remarks = _oldreq.Remarks,
                        Void = false,
                        VoidUser = string.Empty,
                        VoidDate = DateTime.Now,
                        Printed = false,
                        AuditUser = _UserID,
                        AuditDate = DateTime.Now,
                        UploadStat = false,
                        EditUser = _UserID,
                        EditDate = DateTime.Now,
                        TrxMonth = "JAN24",
                        TrxWeek = 1,
                        RefNo = item.ReferenceReceiptNo,
                        DtlCompanyCode = DtlCompanyCode
                    };

                    TblPaymentrequisitionhdr _TblPaymentrequisitionhdr = new TblPaymentrequisitionhdr()
                    {
                        Reqno = string.Empty,
                        PRno = string.Empty,
                        PRDate = DateTime.Now,
                        Active = true,
                        TotalAmount = _TblRequisitionhdr.TotalAmount,//item.Amount,
                        SalesInvoiceNo = item.SalesInvoiceNo,
                        SalesInvoiceDate = item.SalesInvoiceDate,
                        DeliveryNo = item.DeliveryNo,
                        DeliveryDate = item.DeliveryDate,
                        Printed = false,
                        AuditUser = _UserID,
                        AuditDate = DateTime.Now,
                        TrxMonth = _TblRequisitionhdr.TrxMonth,
                        TrxWeek = _TblRequisitionhdr.TrxWeek,
                    };


                    _TblPaymentrequisitionhdr.TotalAmount = _TblRequisitionhdr.AmountDue;
                    //_PRNo = await GenerateNewPRNo(_oldReqhdr, _requisitionParams);

                    process = "GetLatestPRRow";
                    _oldReqhdr = await _RepositoryUnit.TblRequisitionRepository.GetLatestPRRow(BaseUrlRepo, _oldreq.CompanyCode, _oldreq.DeptCode);

                    foreach (var ReqDtl in _ReqDtlList)
                    {
                        process = "PostCreateRequisitionDtl";
                        ReqDtl.ReqNo = _PRNo;
                        resp = await _RepositoryUnit.TblRequisitionRepository.PostCreateRequisitionDtl(BaseUrlRepo, ReqDtl);
                        if (resp.Status == "FAILED")
                        {
                            return Json(new
                            {
                                success = resp.Status,
                                msg = resp.ErrorMessage + " " + Debug
                            },
                       new JsonSerializerOptions());
                        }
                    }

                    _TblRequisitionhdr.Reqno = _PRNo;
                    _TblPaymentrequisitionhdr.Reqno = _PRNo;
                    _TblPaymentrequisitionhdr.PRno = _PRNo;
                    _requisitionParams.ReqNo = _TblPaymentrequisitionhdr.Reqno;
                    _requisitionParams.UserID = _UserID;
                    var Res = await _RepositoryUnit.TblRequisitionRepository.CreateDtLSummary(BaseUrlRepo, _requisitionParams);
                    List<TblRequisitionDtlSummary> _TblRequisitionDtlSummary = new List<TblRequisitionDtlSummary>();
                    _requisitionParams.TblResponse = new TblResponse();
                    _TblRequisitionDtlSummary = await _RepositoryUnit.TblRequisitionRepository.GetDtLSummary(BaseUrlRepo, _requisitionParams);


                    _batchReq.TblRequisitionhdr.Add(_TblRequisitionhdr);
                    _TblRequisitionhdr.TotalFreight = _TblRequisitionDtlSummary.Sum(a => a.Freight);
                    process = "PostCreateRequisitionHdr";
                    resp = await _RepositoryUnit.TblRequisitionRepository.PostCreateRequisitionHdr(BaseUrlRepo, _TblRequisitionhdr);
                    if (resp.Status == "FAILED")
                    {
                        return Json(new
                        {
                            success = resp.Status,
                            msg = resp.ErrorMessage + " " + Debug
                        },
                   new JsonSerializerOptions());
                    }

                    process = "PostCreatePaymentRequisitionHdr";
                    resp = await _RepositoryUnit.TblRequisitionRepository.PostCreatePaymentRequisitionHdr(BaseUrlRepo, _TblPaymentrequisitionhdr);
                    if (resp.Status == "FAILED")
                    {
                        return Json(new
                        {
                            success = resp.Status,
                            msg = resp.ErrorMessage + " " + Debug
                        },
                   new JsonSerializerOptions());
                    }
                    process = "GetPOHdrByReqNo";
                    TblPurchaseorderhdr _reqPOhdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByReqNo(BaseUrlRepo, _TblRequisitionhdr.MainReqNo);


                    List<IFormFile> _reqAttachments = new List<IFormFile>();

                    foreach (var file in files)
                    {
                        string[] filenames = file.FileName.Split('/');

                        if (filenames.Count() < 4)
                        {
                            continue;
                        }

                        if (_reqPOhdr.PONo.Equals(filenames[1].ToUpper()) && item.SalesInvoiceNo.Equals(filenames[2]))
                        {
                            _reqAttachments.Add(file);
                        }
                    }
                    UploadFiles(_reqAttachments, _TblRequisitionhdr.Reqno);

                    _TblLoanhdr = new TblLoanhdr()
                    {
                        LAFNo = _TblRequisitionhdr.Reqno,
                        LPANo = _TblRequisitionhdr.MainReqNo,
                        AppliedLoan = _TblRequisitionhdr.TotalAmount
                    };
                    _requisitionParams.tblLoanhdrs = new List<TblLoanhdr>();

                    _requisitionParams.tblLoanhdrs.Add(_TblLoanhdr);


                    resp = await _RepositoryUnit.PRAuthorizationRepository.CreatePRAuthorization(BaseUrlRepo, BaseUrlRepo, _TblRequisitionhdr.Reqno, "PR");
                    if (resp.Status == "FAILED")
                    {
                        return Json(new
                        {
                            success = resp.Status,
                            msg = resp.ErrorMessage + " " + Debug
                        },
                   new JsonSerializerOptions());
                    }
                }

                //SAVING EXCEL FILE
                System.IO.File.Copy(_batchReq.ExcelFileUploadPath, Path.Combine(ExcelFileCopyPath, _ReqBatch + " - PY.xlsx"));

                process = "Insert Loan";
                resp = await _RepositoryUnit.TblRequisitionRepository.CreateLoanHdr(BaseUrlRepo, _requisitionParams);
                if (resp.Status == "FAILED")
                {
                    return Json(new
                    {
                        success = resp.Status,
                        msg = resp.ErrorMessage + " " + Debug
                    },
               new JsonSerializerOptions());
                }
                process = "SendEmailAuthorization_PRNO_Batch";

                var result = await new AuthorizationController(_AuthLogger, _userManager, _configuration, _env).SendEmailAuthorization_PRNO_Batch(_batchReq.TblRequisitionhdr.Select(a => a.Reqno).ToList(), _UserID);
                //var result = await _RepositoryUnit.PRAuthorizationRepository.ApprovePRAuthorization(_qryUpdateStatusAuth, BaseUrlRepo);

                AuthorizationParams _authorizationParams = new AuthorizationParams();
                _authorizationParams.ReqNo = _batchReq.TblRequisitionhdr.Select(a => a.Reqno).ToList();
                _authorizationParams.UserCode = _UserID;
                _authorizationParams.ReqType = "PY";

                //var result = await _RepositoryUnit.PRAuthorizationRepository.ProcessAuthorization(_authorizationParams, BaseUrlRepo);
                process = "Batch successfully uploaded";



                //OkObjectResult okResult = (OkObjectResult)result.Status;
                //TblResponse _rsp = (TblResponse)okResult.Value;
                //resp = _rsp;


                //resp.ErrorMessage = process;
                return Json(new { response = result, batchreq = _batchReq },
                    new JsonSerializerOptions()); ;
            }

            catch (Exception err)
            {
                return Json(new
                {
                    success = false,
                    error = err.Message + " " + Debug
                },
                    new JsonSerializerOptions());
            }
        }

        public async Task<IActionResult> UploadBatch(List<IFormFile> files, string vm)
        {
            string Debug = string.Empty;
            string error = string.Empty;
            try
            {

                BatchRequisitionViewModel BatchRequisitionViewModel = JsonConvert.DeserializeObject<BatchRequisitionViewModel>(vm);

                if (files.Count <= 0)
                {
                    //error return batch
                    return View("_BatchRequisitionList", BatchRequisitionViewModel);
                }

                BatchUploadParams _BatchUploadParams = new BatchUploadParams();
                List<IFormFile> _reqAttachments = new List<IFormFile>();
                RequisitionParams _requisitionParams = new RequisitionParams();
                //_requisitionParams.RequisitionHdrList = BatchRequisitionViewModel.TblRequisitionhdr.ToList();

                string _UserID = _userManager.GetUserId(this.User);
                //qryEmployee qryEmployee = await _RepositoryUnit.EmployeeRepository.GetEmployeeDetails(OSPUrlRepo, _UserID);
                TblRequisitionhdr _TblRequisitionhdr_old = new TblRequisitionhdr();
                TblResponse _reponse = new TblResponse();

                _BatchUploadParams.UserID = _UserID;
                //_BatchUploadParams.qryEmployee = qryEmployee;
                _BatchUploadParams.TblRequisitiondtlList = new List<TblRequisitiondtl>();
                _BatchUploadParams.TblRequisitionhdrList = new List<TblRequisitionhdr>();


                _BatchUploadParams.TblRequisitionhdrList = BatchRequisitionViewModel.TblRequisitionhdr.ToList();
                _BatchUploadParams.TblRequisitiondtlList = BatchRequisitionViewModel.TblRequisitiondtl.ToList();

                //_BatchUploadParams.qryBatchPaymentHdrList = new List<qryBatchPaymentHdr>();
                //_BatchUploadParams.qryBatchPaymentDtlList = new List<qryBatchPaymentDtl>();
                //_BatchUploadParams.TblBatchPRHdr = new TblBatchPRHdr();
                //_BatchUploadParams.TblPurchaseorderhdrList = new List<TblPurchaseorderhdr>();
                //_BatchUploadParams.TblResponse = new TblResponse();
                //_BatchUploadParams.qryRequisitions = new List<qryRequisition>();
                //_BatchUploadParams.qryBatchRequistions = new List<qryBatchRequistion>();
                //_BatchUploadParams.qryRequisitionHdr = new List<qryRequisitionHdr>();
                //_BatchUploadParams.qryBatchUploadExcel = new qryBatchUploadExcel();
                _BatchUploadParams = await _RepositoryUnit.TblRequisitionRepository.InsertRequisitionList(BaseUrlRepo, _BatchUploadParams);

                _response = _BatchUploadParams.TblResponse;

                for (int i = 0; i < _BatchUploadParams.TblRequisitionhdrList.Count; i++)
                {
                    Debug = "Starting to loop files PR Batch";
                    foreach (var file in files)
                    {
                        string[] filenames = file.FileName.Split('/');

                        if (filenames.Count() < 3)
                        {
                            continue;
                        }
                        //var sorted = BatchRequisitionViewModel.qryRequisition.GroupBy(a => new { a.CompanyDesc, a.VendorDesc, a.CompanyType })
                        //.Select(a => new qryre { CompanyType = a.Key.CompanyType, CompanyDesc = a.Key.CompanyDesc, VendorName = a.Key.VendorDesc }).ToList();

                        foreach (var item in BatchRequisitionViewModel.qryRequisition.GroupBy(a => new { a.VendorDesc, a.CompanyDesc }))
                        {
                            //var vendorname = await _RepositoryUnit.VendorRepository.GetVendorNameByVendorCode(BaseUrlRepo, _TblRequisitionhdr.VendorCode);
                            //var compname = await _RepositoryUnit.CompanyRepository.GetCompanyDescByCompanyCode(OSPUrlRepo, _requisitionParams.TblRequisitionDtlSummary.Select(a => a.CompanyCode).FirstOrDefault());
                            ////string vendorname = await _RepositoryUnit.VendorRepository.GetVendorCodeByDisplayName(BaseUrlRepo, vendorcode);
                            //if (filenames[1].Replace(".", "").Equals(item.Key.CompanyDesc.Replace(".", "")))
                            //{
                            var name = filenames[1].ToUpper().Replace(".", "") + " - " + filenames[2].ToUpper();
                            var name2 = item.Key.CompanyDesc.Replace(".", "") + " - " + item.Key.VendorDesc;
                            if (name == name2)
                            {
                                if (!_reqAttachments.Select(a => a.FileName).Contains(filenames[3]))
                                {
                                    _reqAttachments.Add(file);
                                }
                            }
                            //}
                        }
                    }

                    Debug = "Starting to upload files PR Batch";
                    UploadFiles(_reqAttachments, _BatchUploadParams.TblRequisitionhdrList[i].Reqno);
                    _reqAttachments.Clear();

                    if (!Directory.Exists(Path.Combine(UploadingPathPR, _BatchUploadParams.TblRequisitionhdrList[i].Reqno)))
                    {
                        Directory.CreateDirectory(Path.Combine(UploadingPathPR, _BatchUploadParams.TblRequisitionhdrList[i].Reqno));
                    }

                }

                //BatchRequisitionViewModel.error = Debug + " " + _response;


                //SAVING EXCEL FILE

                System.IO.File.Copy(BatchRequisitionViewModel.ExcelFileUploadPath, Path.Combine(ExcelFileCopyPath, _BatchUploadParams.TblRequisitionhdrList.Select(a => a.BatchNo).FirstOrDefault() + " - PO.xlsx"), true);

                return Json(new { response = _response, BatchPRNo = _BatchUploadParams.TblRequisitionhdrList.Select(a => a.BatchNo).FirstOrDefault() },
                    new JsonSerializerOptions()); ;


                return View("_BatchRequisitionList", BatchRequisitionViewModel);
            }
            catch (Exception err)
            {
                return Json(new
                {
                    success = false,
                    error = err.Message + " " + Debug
                },
                    new JsonSerializerOptions());
            }
        }

        //UPLOAD BATCH REQUISITION OLD
        //public async Task<IActionResult> UploadBatchOld(List<IFormFile> files, string vm)
        //{

        //    string Debug = string.Empty;
        //    string error = string.Empty;
        //    try
        //    {
        //        BatchRequisitionViewModel BatchRequisitionViewModel = JsonConvert.DeserializeObject<BatchRequisitionViewModel>(vm);
        //        BatchUploadParams _BatchUploadParams = new BatchUploadParams();

        //        RequisitionParams _requisitionParams = new RequisitionParams();
        //        _requisitionParams.RequisitionHdrList = BatchRequisitionViewModel.TblRequisitionhdr.ToList();

        //        string _UserID = _userManager.GetUserId(this.User);
        //        qryEmployee qryEmployee = await _RepositoryUnit.EmployeeRepository.GetEmployeeDetails(OSPUrlRepo, _UserID);
        //        TblRequisitionhdr _TblRequisitionhdr_old = new TblRequisitionhdr();


        //        Debug = "Starting to get latest PR Batch";
        //        _TblRequisitionhdr_old = await _RepositoryUnit.PRBatchUploadRepository.GetLatestPRBatchNo(BaseUrlRepo);
        //        string _ReqBatch = string.Empty;

        //        Debug = "Starting to generate PR Batch. Requisition count: " + BatchRequisitionViewModel.TblRequisitionhdr.Count;
        //        _requisitionParams.LastNo = "0";
        //        if (_TblRequisitionhdr_old != null)
        //        {
        //            _requisitionParams.LastNo = _TblRequisitionhdr_old.BatchNo;
        //        }
        //        Debug = "Starting to generate PR Batch123" + _requisitionParams.LastNo + _requisitionParams.RequisitionHdrList.Select(a => a.AuditDate).FirstOrDefault();
        //        _ReqBatch = await GeneratePRBatchNo(_TblRequisitionhdr_old, _requisitionParams);

        //        _requisitionParams.TblResponse = new TblResponse();

        //        Debug = "Starting to create PR Batch";
        //        List<IFormFile> _reqAttachments = new List<IFormFile>();
        //        for (int i = 0; i < BatchRequisitionViewModel.TblRequisitionhdr.Count; i++)
        //        {
        //            //var cancreate = _RepositoryUnit.TblRequisitionRepository.

        //            string _TempReqNo = string.Empty;
        //            _TempReqNo = BatchRequisitionViewModel.TblRequisitionhdr[i].Reqno;
        //            TblRequisitionhdr _TblRequisitionhdr = new TblRequisitionhdr();
        //            _TblRequisitionhdr = BatchRequisitionViewModel.TblRequisitionhdr[i];

        //            _TblRequisitionhdr.BatchNo = _ReqBatch;
        //            _TblRequisitionhdr.AuditUser = _UserID;
        //            _TblRequisitionhdr.AuditDate = DateTime.Now;
        //            _TblRequisitionhdr.EditUser = _UserID;
        //            _TblRequisitionhdr.EditDate = DateTime.Now;
        //            _TblRequisitionhdr.DeptCode = qryEmployee.DeptCode;
        //            _TblRequisitionhdr.CompanyCode = qryEmployee.CompanyCode;

        //            TblRequisitionhdr OldRequisitionHdr = await _RepositoryUnit.TblRequisitionRepository.GetLatestPRRow(BaseUrlRepo, BatchRequisitionViewModel.TblRequisitionhdr[i].CompanyCode, BatchRequisitionViewModel.TblRequisitionhdr[i].DeptCode);
        //            string _PRNo = string.Empty;

        //            if (OldRequisitionHdr != null)
        //            {
        //                _requisitionParams.LastNo = OldRequisitionHdr.Reqno;
        //            }
        //            _requisitionParams.CompanyCode = BatchRequisitionViewModel.TblRequisitionhdr[i].CompanyCode;
        //            _PRNo = await GenerateNewPRNo(OldRequisitionHdr, _requisitionParams);

        //            _TblRequisitionhdr.Reqno = _PRNo;
        //            _TblRequisitionhdr.MainReqNo = _PRNo;
        //            _TblRequisitionhdr.RefNo = "1";
        //            Debug = "Starting to create PR HDR Batch" + System.Text.Json.JsonSerializer.Serialize(_TblRequisitionhdr);
        //            _requisitionParams.RequisitionHdrList.Add(_TblRequisitionhdr);
        //            _response = await _RepositoryUnit.TblRequisitionRepository.PostCreateRequisitionHdr(BaseUrlRepo, _TblRequisitionhdr);

        //            for (int dtl = 0; dtl < BatchRequisitionViewModel.TblRequisitiondtl.Count; dtl++)
        //            {
        //                if (_TempReqNo.Equals(BatchRequisitionViewModel.TblRequisitiondtl[dtl].ReqNo))
        //                {
        //                    BatchRequisitionViewModel.TblRequisitiondtl[dtl].ReqNo = _PRNo;
        //                    BatchRequisitionViewModel.TblRequisitiondtl[dtl].AuditUser = _UserID;
        //                    BatchRequisitionViewModel.TblRequisitiondtl[dtl].AuditDate = DateTime.Now;
        //                    BatchRequisitionViewModel.TblRequisitiondtl[dtl].EditUser = _UserID;
        //                    BatchRequisitionViewModel.TblRequisitiondtl[dtl].EditDate = DateTime.Now;
        //                    Debug = "Starting to create Req Batch DTL";
        //                    _requisitionParams.RequisitionDtlList.Add(BatchRequisitionViewModel.TblRequisitiondtl[dtl]);
        //                    _response = await _RepositoryUnit.TblRequisitionRepository.PostCreateRequisitionDtl(BaseUrlRepo, BatchRequisitionViewModel.TblRequisitiondtl[dtl]);


        //                }
        //            }

        //            _requisitionParams.ReqNo = _TblRequisitionhdr.Reqno;
        //            _requisitionParams.UserID = _UserID;
        //            _response = await _RepositoryUnit.TblRequisitionRepository.CreateDtLSummary(BaseUrlRepo, _requisitionParams);

        //            _requisitionParams.TblRequisitionDtlSummary = await _RepositoryUnit.TblRequisitionRepository.GetDtLSummary(BaseUrlRepo, _requisitionParams);

        //            Debug = "Starting to loop files PR Batch";
        //            foreach (var file in files)
        //            {
        //                string[] filenames = file.FileName.Split('/');

        //                if (filenames.Count() < 3)
        //                {
        //                    continue;
        //                }

        //                foreach (var item in _requisitionParams.TblRequisitionDtlSummary)
        //                {
        //                    var vendorname = await _RepositoryUnit.VendorRepository.GetVendorNameByVendorCode(BaseUrlRepo, _TblRequisitionhdr.VendorCode);
        //                    var compname = await _RepositoryUnit.CompanyRepository.GetCompanyDescByCompanyCode(OSPUrlRepo, _requisitionParams.TblRequisitionDtlSummary.Select(a => a.CompanyCode).FirstOrDefault());
        //                    ////string vendorname = await _RepositoryUnit.VendorRepository.GetVendorCodeByDisplayName(BaseUrlRepo, vendorcode);
        //                    if (item.CompanyCode.Equals(item.CompanyCode))
        //                    {
        //                        var name = filenames[1].ToUpper().Replace(".", "") + " - " + filenames[2].ToUpper();
        //                        var name2 = compname.Replace(".", "") + " - " + vendorname;
        //                        if (name == name2)
        //                        {
        //                            if (!_reqAttachments.Select(a => a.FileName).Contains(filenames[3]))
        //                            {
        //                                _reqAttachments.Add(file);
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            Debug = "Starting to upload files PR Batch";
        //            UploadFiles(_reqAttachments, _TblRequisitionhdr.Reqno);
        //            _reqAttachments.Clear();

        //            if (!Directory.Exists(Path.Combine(UploadingPathPR, _TblRequisitionhdr.Reqno)))
        //            {
        //                Directory.CreateDirectory(Path.Combine(UploadingPathPR, _TblRequisitionhdr.Reqno));
        //            }

        //            Debug = "Starting to create PR Auth Batch";
        //            _response = await _RepositoryUnit.PRAuthorizationRepository.CreatePRAuthorization(BaseUrlRepo, "", _TblRequisitionhdr.Reqno, "PO");
        //        }

        //        Debug = "Sending to create PR Batch";
        //        var result = await new AuthorizationController(_AuthLogger, _userManager, _configuration, _env).SendEmailAuthorization_PRNO_Batch(BatchRequisitionViewModel.TblRequisitionhdr.Select(a => a.Reqno).ToList(), _UserID);

        //        BatchRequisitionViewModel.BatchPRNo = _ReqBatch; //BatchRequisitionViewModel.TblRequisitionhdr.Count.ToString();

        //        OkObjectResult okResult = (OkObjectResult)result;
        //        TblResponse _rsp = (TblResponse)okResult.Value;
        //        _response = _rsp;

        //        //SAVING EXCEL FILE
        //        System.IO.File.Copy(BatchRequisitionViewModel.ExcelFileUploadPath, Path.Combine(ExcelFileCopyPath, _ReqBatch + " - PO.xlsx"));

        //        BatchRequisitionViewModel.error = Debug + " " + _response;



        //        return Json(new { response = _response, BatchPRNo = _ReqBatch },
        //            new JsonSerializerOptions()); ;


        //        return View("_BatchRequisitionList", BatchRequisitionViewModel);
        //    }
        //    catch (Exception err)
        //    {
        //        return Json(new
        //        {
        //            success = false,
        //            error = err.Message + " " + Debug
        //        },
        //            new JsonSerializerOptions());
        //    }
        //}

        public void UploadFiles(List<IFormFile> files, string prno)
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {

                var fileName = IO.Path.GetFileName(formFile.FileName);

                // Get file path to be uploaded
                //var path = Path.Combine(Directory.GetCurrentDirectory(), prno);
                var path = Path.Combine(UploadingPathPR, prno);
                path = path + @"\";
                var filePath = Path.Combine(UploadingPathPR, prno, fileName);
                //var filePath = "C:\\Wa\\sample\\" + fileName;

                // Check If file with same name exists and delete it

                if (IO.File.Exists(filePath))
                {
                    IO.File.Delete(filePath);
                }
                else
                {
                    Directory.CreateDirectory(path);
                }

                // Create a new local file and copy contents of uploaded file
                using (var localFile = System.IO.File.OpenWrite(filePath.Replace("#", "")))
                using (var uploadedFile = formFile.OpenReadStream())
                {
                    uploadedFile.CopyTo(localFile);
                }
            }
        }

        //public async Task<string> GenerateNewPRNo(TblRequisitionhdr OldRequisitionHdr, RequisitionParams _requisitionParams)
        //{
        //    string _PRNo = string.Empty;
        //    if (OldRequisitionHdr != null)
        //    {
        //        var newprno = await _ServiceUnit.RequisitionService.GenerateNewPRNo(BaseUrlService, _requisitionParams);
        //        _PRNo = newprno;
        //    }
        //    else
        //    {
        //        _requisitionParams.LastNo = "0";
        //        var newprno = await _ServiceUnit.RequisitionService.GenerateNewPRNo(BaseUrlService, _requisitionParams);
        //        _PRNo = newprno;
        //    }

        //    return _PRNo;
        //}

        //public async Task<string> GeneratePRBatchNo(TblRequisitionhdr _TblRequisitionhdr_old, RequisitionParams RequisitionParams)
        //{
        //    string _ReqBatch = string.Empty;
        //    if (_TblRequisitionhdr_old != null)
        //    {
        //        _ReqBatch = await _ServiceUnit.PaymentRequestService.GenerateNewPRBatchNo(BaseUrlService, RequisitionParams);
        //    }
        //    else
        //    {
        //        RequisitionParams.LastNo = "0";
        //        _ReqBatch = await _ServiceUnit.PaymentRequestService.GenerateNewPRBatchNo(BaseUrlService, RequisitionParams);
        //    }

        //    return _ReqBatch;
        //}


        public async Task<IActionResult> DownloadFileFromWeb()
        {
            try
            {
                // string filePath = IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", "SPASv2 Batch Template.xlsx");

                //// Check if the file exists
                // if (IO.File.Exists(filePath))
                // {
                //   //  Return the file using PhysicalFileResult
                //     return PhysicalFile(filePath, "application/octet-stream");
                // }
                // else
                // {
                //    // If the file doesn't exist, return a NotFound result
                //     return NotFound();
                // }
                bool http = HttpContext.Request.IsHttps;
                string httpval = "https://";
                if (!http)
                {
                    httpval = "http://";
                }

                //string completeUrl = HttpContext.Request.GetEncodedUrl();
                string serverName = HttpContext.Request.Host.Value;

                serverName = httpval + serverName + "/Files/SPASv2 Batch Template.xlsx";
                return Redirect(serverName);




                _response = new TblResponse();

                //string contentRootPath = _env.ContentRootPath;
                //string rootpath = System.IO.Path.Combine(contentRootPath, "Files");
                //string filename = "SPASv2 Batch Template.xlsx";

                //if (!IO.Directory.Exists(_filedirtemplate))
                //    IO.Directory.CreateDirectory(_filedirtemplate);


                //_filedirtemplate = Path.Combine(rootpath, filename);

                _logger.LogInformation("Downloading of template - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                var resp = await _ServiceUnit.RequisitionService.DownloadFileFromWeb(OSPUrlService, _filedirtemplate, "BatchTemplate.xlsx");

                //return resp;
                RedirectToAction();
                if (resp.Status == "SUCCESS")
                {

                    // File downloaded successfully, you can return a response or perform further actions
                    //return Ok("File downloaded successfully!");
                    //  return RedirectToAction("Files\");
                    _response.Status = resp.Status;
                    _response.ErrorMessage = resp.ErrorMessage;
                    return Json(new { response = resp, filedir = _filedirtemplate }, new JsonSerializerOptions());


                }
                else
                {
                    _logger.LogWarning("Failed to download template - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + "");
                    // Handle the case where file download failed
                    //return BadRequest("Failed to download file.");
                    _response.Status = resp.Status;
                    _response.ErrorMessage = resp.ErrorMessage;
                    return Json(new { response = resp, filedir = _filedirtemplate }, new JsonSerializerOptions());

                }
            }
            catch (Exception ex)
            {

                //throw new Exception(ex.Message);
                _response.Status = "FAILED";
                _response.ErrorMessage = ex.Message;
                return Json(new { response = _response }, new JsonSerializerOptions());
            }
        }

        public async Task<IActionResult> DownloadFolderWinrar()
        {
            try
            {
                string page = HttpContext.Request.Query["filedir"].ToString();

                bool http = HttpContext.Request.IsHttps;
                string httpval = "https://";
                if (!http)
                {
                    httpval = "http://";
                }

                //string completeUrl = HttpContext.Request.GetEncodedUrl();
                string serverName = HttpContext.Request.Host.Value;

                serverName = httpval + serverName + "/Files/" + page;
                return Redirect(serverName);



            }
            catch (Exception ex)
            {

                //throw new Exception(ex.Message);
                _response.Status = "FAILED";
                _response.ErrorMessage = ex.Message;
                return Json(new { response = _response }, new JsonSerializerOptions());
            }
        }

        public async Task DownloadAndSave()
        {
            string fileUrl = OSPUrlService + "/OSPCommon/DownloadFile1";
            string destinationPath = @"C:\SPASv2";
            string destinationFileName = "DownloadAndSave.xlsx";
            Stream fileStream = await UtilitiesHttpClient<GetFileDownload>.GetFileStream(fileUrl);

            if (fileStream != Stream.Null)
            {
                await SaveStream(fileStream, destinationPath, destinationFileName);
            }
        }

        public async Task SaveStream(Stream fileStream, string destinationFolder, string destinationFileName)
        {
            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            string path = Path.Combine(destinationFolder, destinationFileName);

            using (FileStream outputFileStream = new FileStream(path, FileMode.CreateNew))
            {
                await fileStream.CopyToAsync(outputFileStream);
            }
        }

        public async Task<IActionResult> ExportDataTabletoExcelAsync(string htmltable)
        {
            if (string.IsNullOrEmpty(htmltable))
            {
                return Redirect("/BatchRequisition/List");
            }

            string[] lstPO = htmltable.Split(',');

            List<string> list = (new List<string>(lstPO.Cast<string>().Distinct()));

            // Create a new DataTable.    
            DataTable custTable = new DataTable("Payment Request Template");
            custTable.Columns.Add(new DataColumn("PONo", typeof(string)));
            custTable.Columns.Add(new DataColumn("Company", typeof(string)));
            custTable.Columns.Add(new DataColumn("Vendor Name", typeof(string)));
            custTable.Columns.Add(new DataColumn("Department", typeof(string)));
            custTable.Columns.Add(new DataColumn("Item", typeof(string)));
            custTable.Columns.Add(new DataColumn("Orig Order", typeof(int)));
            custTable.Columns.Add(new DataColumn("Paid Qty", typeof(int)));
            custTable.Columns.Add(new DataColumn("Balance", typeof(int)));
            custTable.Columns.Add(new DataColumn() { ColumnName = "S.I. Qty", DataType = typeof(int), AllowDBNull = true });
            custTable.Columns.Add(new DataColumn("Reference Reciept", typeof(string)));
            custTable.Columns.Add(new DataColumn("D.R. No.", typeof(string)));
            custTable.Columns.Add(new DataColumn("D.R. Date", typeof(string)));
            custTable.Columns.Add(new DataColumn("S.I. No.", typeof(string)));
            custTable.Columns.Add(new DataColumn("S.I. Date", typeof(string)));

            custTable.Columns.Add(new DataColumn() { ColumnName = "Price Per Unit", DataType = typeof(decimal), AllowDBNull = true });
            custTable.Columns.Add(new DataColumn("Total Amount", typeof(string)));
            custTable.Columns.Add(new DataColumn() { ColumnName = "HP Deduction", DataType = typeof(decimal), AllowDBNull = true });
            custTable.Columns.Add(new DataColumn() { ColumnName = "Per Unit Freight Charge", DataType = typeof(decimal), AllowDBNull = true });
            custTable.Columns.Add(new DataColumn("Freight Amount", typeof(string)));
            //custTable.Columns.Add(new DataColumn("Price Amount", typeof(string)));

            foreach (var pono in list.OrderBy(a => a.ToString()))
            {
                if (string.IsNullOrEmpty(pono))
                {
                    continue;
                }

                TblPurchaseorderhdr _PO = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(BaseUrlRepo, pono);

                IList<qryRequisitionItem> _MainReqItem = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionItemList(BaseUrlRepo, _PO.Reqno);

                List<qryRequisitionItem> _ReqItemList = new List<qryRequisitionItem>();
                _ReqItemList.AddRange(_MainReqItem);

                IList<qryRequisitionInfo> _PyReqList = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionListInfoByMainReqNo(BaseUrlRepo, _PO.Reqno);
                qryRequisitionInfo _ReqInfo = await _RepositoryUnit.TblRequisitionRepository.GetRequisitionInfo(BaseUrlRepo, _PO.Reqno);


                foreach (var pyReq in _PyReqList)
                {
                    _ReqItemList.AddRange(await _RepositoryUnit.TblRequisitionRepository.GetRequisitionItemList(BaseUrlRepo, pyReq.ReqNo));
                }


                object PricePerUnitFormula = string.Empty;
                object FreightFormula = string.Empty;
                int Idx = 1;
                foreach (var _reqitem in _MainReqItem)
                {
                    qryCompanyDetails _CompDtl = await _RepositoryUnit.CompanyRepository.GetCompanyDetails(OSPUrlRepo, _reqitem.CompanyType, _reqitem.DeptCode);
                    int ItemQty = _ReqItemList.Where(t => t.ReqNo != _PO.Reqno && t.Item == _reqitem.Item && _reqitem.isDeduct).Sum(t => t.Quantity);
                    Idx++;
                    string department = _reqitem.CompanyType + "-" + _reqitem.DeptCode;
                    string Company = _CompDtl.CompanyDesc;
                    string vendor = _ReqInfo.Vendor;
                    string item = _reqitem.Item;
                    string balance = (Convert.ToInt32(_reqitem.Quantity) - ItemQty).ToString();
                    object QtyOrdered = Convert.ToInt32(_reqitem.Quantity);
                    object QtyDelivered = Convert.ToInt32(ItemQty);
                    object quantity = null;
                    string refno = string.Empty;
                    string drno = string.Empty;
                    string drdate = string.Empty;
                    string sino = string.Empty;
                    string sidate = string.Empty;
                    object balanceAmt = Convert.ToDecimal(_reqitem.Price); //((Convert.ToInt32(_reqitem.Quantity) - ItemQty) * _reqitem.Price);
                    object hpDeduction = null;
                    PricePerUnitFormula = "=SUM(O" + Idx + " * I" + Idx + ")";
                    FreightFormula = "=SUM(R" + Idx + " * I" + Idx + ")";

                    custTable.Rows.Add(pono, Company, vendor, department,
                     item,
                     QtyOrdered,
                     QtyDelivered,
                     balance,
                     quantity,
                     refno,
                     drno,
                     drdate,
                     sino,
                     sidate,
                     balanceAmt,
                     PricePerUnitFormula,
                     hpDeduction,
                     0,
                     FreightFormula
                        );
                }
            }


            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(custTable);

                //ws.Cell("A1").Value = "PONo";
                //ws.Cell("A2").Value = "Company";
                //ws.Cell("A3").Value = "VendorName";
                //ws.Cell("A4").Value = "Department";
                //ws.Cell("A5").Value = "Item";
                //ws.Cell("A6").Value = "Orig Order";
                //ws.Cell("A7").Value = "Paid Qty";
                //ws.Cell("A8").Value = "Balance";
                //ws.Cell("A9").Value = "S.I. Qty";
                //ws.Cell("A10").Value = "Reference Reciept";
                //ws.Cell("A11").Value = "D.R. No.";
                //ws.Cell("A12").Value = "D.R. Date";
                //ws.Cell("A13").Value = "S.I. No.";
                //ws.Cell("A14").Value = "S.I. Date";
                //ws.Cell("A15").Value = "Total Amount";
                //ws.Cell("A16").Value = "Price Per Unit";
                //ws.Cell("A17").Value = "HP Deduction";
                //ws.Cell("A18").Value = "Per Unit Freight Charge";
                //ws.Cell("A19").Value = "Freight Amount";

                // wb.Worksheets.Add(custTable);
                var wsRange = ws.Range(2, 15, custTable.Rows.Count + 1, 19);
                wsRange.Style.NumberFormat.Format = "#,###,###.0000;(#,###,###.0000)";

                ws.Columns().AdjustToContents();


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Payment Request Template.xlsx");
                }
            }
        }

        public void UploadFileList(List<IFormFile> files, string prnolist)
        {
            try
            {
                List<string> PRNoList = new List<string>();
                PRNoList = JsonConvert.DeserializeObject<List<string>>(prnolist);
                string path1 = string.Empty;
                for (int i = 0; i < PRNoList.Count; i++)
                {
                   
                    string _PRNo = PRNoList[i].ToString().TrimStart().TrimEnd();
                    foreach (FormFile file in files)
                    {
                        string fname = file.FileName.Replace("#", "");
                        if(fname.Contains(_PRNo))
                        {
                            path1 = Path.Combine(UploadingPathPR, _PRNo);

                            //string copyDestination = path1 + "\\" + System.IO.Path.GetFileName(fname);

                            int Counter = 0;
                            string copyDestination = Path.Combine(path1, System.IO.Path.GetFileNameWithoutExtension(fname)) + System.IO.Path.GetExtension(fname);

                            while (System.IO.File.Exists(copyDestination))
                            {
                                Counter++;
                                copyDestination = Path.Combine(path1, System.IO.Path.GetFileNameWithoutExtension(fname)) + "_" + Counter + System.IO.Path.GetExtension(fname);
                            }

                            using (var localFile = System.IO.File.OpenWrite(copyDestination))
                            using (var uploadedFile = file.OpenReadStream())
                            {
                                uploadedFile.CopyTo(localFile);
                            }
                        }
                    }

                    
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string CreateFolderTemplate(string ReqNoList)
        {
            try
            {
                List<string> PRNoList = new List<string>();
                PRNoList = JsonConvert.DeserializeObject<List<string>>(ReqNoList);
                //string path = @"\\192.168.23.185\SPASv2$\Files\Requisition\" + DateTime.Now.ToString("yyyy-MM-dd hhmmsstt").ToUpper();
                string path = IO.Path.Combine(_ReqFiles, "Requisition", DateTime.Now.ToString("yyyy-MM-dd hhmmsstt").ToUpper());
                string path1 = _ReqDownloadFiles;
                string foldername = DateTime.Now.ToString("yyyy-MM-dd hhmmsstt").ToUpper() + ".rar";
                string path2 = IO.Path.Combine(path1, "WINRAR");

                string pathwithname = Path.Combine(_ReqDownloadFiles, foldername);

                if (Directory.Exists(pathwithname))
                {
                    Directory.Delete(pathwithname, true);
                }

                if (!Directory.Exists(path2))
                {
                    Directory.CreateDirectory(path2);
                }

                foreach (string item in PRNoList)
                {
                    string dir = Path.Combine(path, item.TrimStart().TrimEnd());

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                } 
                //string sourceFilePath = "C:\\YourFilePath\\YourFile.txt";
                string destinationRarFilePath = IO.Path.Combine(path2, foldername);

                // Step 1: Compress the file into a RAR file using WinRAR
                CompressToRar(path, destinationRarFilePath);
                ViewData["devtype"] = DevelopmentType;

                return foldername;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
