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
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Text;
using System.Linq;
using OSP.Common.Domain.View;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OSP.SPASv2.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequisitionController : ControllerBase
    {

        ServiceUnit _ServiceUnit;
        private ILogger<RequisitionController> _logger;
        private TblResponse _response;

        private MSAccessDBManager conn;
        private string trxCode;
        private string mdbCode;
        private string template;
        private string fileName;
        StringBuilder sb;
        public RequisitionController(ILogger<RequisitionController> logger)
        {
            //this.jwtAuthenticationManager = jwtAuthenticationManager;
            _ServiceUnit = new ServiceUnit();
            _logger = logger;
            _response = new TblResponse();

        }

        [HttpPost("GenerateBatchNo")]
        public async Task<RequisitionParams> GenerateBatchNo(RequisitionParams _RequisitionParams)
        {
            try
            {
                string error = "Generate - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName();
                _logger.LogError(error);

                if (_RequisitionParams._TblRequisitionhdr_old == null)
                {
                    _RequisitionParams.LastNo = "0";
                }
                else
                {
                    _RequisitionParams.LastNo = _RequisitionParams._TblRequisitionhdr_old.BatchNo;
                }

                _RequisitionParams.BatchReqNo = await _ServiceUnit.RequisitionService.GenerateBatchNo(_RequisitionParams.LastNo);

                return _RequisitionParams;
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                _logger.LogError(error);
                _RequisitionParams.TblResponse.Status = "FAILED";
                _RequisitionParams.TblResponse.ErrorMessage = error;
                return _RequisitionParams;
            }

        }


        [HttpPost("GenerateNewPRNo")]
        public async Task<RequisitionParams> GenerateNewPRNo(RequisitionParams RequisitionParams)
        {
            try
            {
                string error = "Generate - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName();
                _logger.LogError(error);
                // RequisitionParams.LastNo = await _ServiceUnit.RequisitionService.GeneratePaymentrequestno(RequisitionParams.LastNo, RequisitionParams.CompanyCode, RequisitionParams.RequisitionHdrList.Select(a => a.AuditDate).FirstOrDefault());
                if (RequisitionParams._TblRequisitionhdr_old == null)
                {
                    RequisitionParams.LastNo = "0"; 
                }
                else
                {
                    RequisitionParams.LastNo = RequisitionParams._TblRequisitionhdr_old.Reqno;
                    RequisitionParams.CompanyCode = RequisitionParams._TblRequisitionhdr_old.CompanyCode;
                }
                RequisitionParams.ReqNo = await _ServiceUnit.RequisitionService.GeneratePaymentrequestno(RequisitionParams.LastNo, RequisitionParams.CompanyCode, DateTime.Now);
                return RequisitionParams;
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                _logger.LogError(ex, error);
                RequisitionParams.TblResponse.Status = "FAILED";
                RequisitionParams.TblResponse.ErrorMessage = error;
                return RequisitionParams;
            }

        }

        [HttpPost("GroupRequisitionHdr")]
        public async Task<RequisitionParams> GroupRequisitionHdr(List<qryRequisition> _qryRequisitionList)
        {
            List<TblRequisitionhdr> _TblRequisitionhdrList = new List<TblRequisitionhdr>();
            List<TblRequisitiondtl> _TblRequisitiondtlList = new List<TblRequisitiondtl>();
            RequisitionParams _RequisitionParams = new RequisitionParams();
            decimal _RequisitionTotalAmount = 0.00m;
            string _CompanyCode = string.Empty;
            string _VendorName = string.Empty;

            _qryRequisitionList = _qryRequisitionList.OrderBy(t => t.CompanyCode).ThenBy(a => a.PayeeName).ToList();

            int _TempReqNo = 1;

            List<qryRequisitionDtl> Listqrydtl = new List<qryRequisitionDtl>();

            for (int i = 0; i < _qryRequisitionList.Count; i++)
            {
                int _Index = 0;

                qryComputeBreakdown _qryComputeBreakdown = new qryComputeBreakdown()
                {
                    Qty = _qryRequisitionList[i].Quantity,
                    Gross = _qryRequisitionList[i].Price,
                    VatRate = 1.12m,
                    Discount = _qryRequisitionList[i].Discount,
                    Disccode = "002",
                    isVAT = _qryRequisitionList[i].isVendorVat
                };
                qryComputeReqDtlCriteria _qryComputeReqDtlCriteria = new qryComputeReqDtlCriteria();
                _VendorName = _qryRequisitionList[i].VendorDesc;
                _qryComputeBreakdown = await _ServiceUnit.PaymentRequestService.ComputeBreakDown(_qryComputeBreakdown);
                _qryRequisitionList[i].DtlTotalAmount = _qryComputeBreakdown.AmountDue;
                _RequisitionTotalAmount += _qryComputeBreakdown.AmountDue;//_qryRequisitionList[i].TotalAmount;

                if (string.IsNullOrEmpty(_CompanyCode))
                {
                    _CompanyCode = _qryRequisitionList[i].CompanyCode.TrimStart().TrimEnd();
                }
                if (string.IsNullOrEmpty(_VendorName))
                {
                    _VendorName = _qryRequisitionList[i].PayeeName.TrimStart().TrimEnd();
                }

                if (i + 1 == _qryRequisitionList.Count())
                {
                    _Index = i;
                }
                else
                {
                    _Index = i + 1;
                }

                TblRequisitiondtl _TblRequisitiondtl = new TblRequisitiondtl()
                {
                    ReqNo = _TempReqNo.ToString(),
                    CompanyCode = _qryRequisitionList[i].CompanyCode,
                    DeptCode = _qryRequisitionList[i].DeptCode,
                    ItemCode = _qryRequisitionList[i].ItemCode,
                    Unit = _qryRequisitionList[i].Unit,
                    Price = _qryRequisitionList[i].Price,
                    Quantity = _qryComputeBreakdown.Qty,
                    Gross = _qryComputeBreakdown.Gross,
                    VatRate = _qryComputeBreakdown.VatRate,
                    Vat = _qryComputeBreakdown.Vat,
                    NetofVat = _qryComputeBreakdown.NetOfVAT,
                    TotalAmount = _qryComputeBreakdown.AmountDue,
                    Discount = _qryComputeBreakdown.Discount,
                    TotalTax = 0.00m,
                    AuditUser = _qryRequisitionList[i].AuditUser,
                    EditUser = _qryRequisitionList[i].AuditUser,
                };
                _TblRequisitiondtlList.Add(_TblRequisitiondtl);

                qryRequisitionDtl qrydtl = new qryRequisitionDtl()
                {
                    ReqNo = _TempReqNo.ToString(),
                    VAT = _qryComputeBreakdown.Vat,
                    NetOfVAT = _qryComputeBreakdown.NetOfVAT,
                    TotalTax = _TblRequisitiondtl.TotalTax,
                    Discount = _qryComputeBreakdown.Discount,
                    Deduction = _qryRequisitionList[i].Deduction,
                    Gross = _qryComputeBreakdown.Gross,
                    TotalAmount = _qryComputeBreakdown.AmountDue
                };
                Listqrydtl.Add(qrydtl);



                if (_CompanyCode.ToUpper() != _qryRequisitionList[_Index].CompanyCode.ToUpper().TrimStart().TrimEnd() ||
                    _VendorName.ToUpper() != _qryRequisitionList[_Index].PayeeName.ToUpper().TrimStart().TrimEnd() ||
                    i + 1 == _qryRequisitionList.Count)
                {

                    var qryresult = await _ServiceUnit.PaymentRequestService.ComputeBreakDownHdr(Listqrydtl);

                    //Create new request no. 
                    TblRequisitionhdr _TblRequisitionhdr = new TblRequisitionhdr()
                    {
                        Reqno = _TempReqNo.ToString(),
                        MainReqNo = _TempReqNo.ToString(),
                        BatchNo = string.Empty,
                        CompanyCode = _qryRequisitionList[i].CompanyCode,//User companycode
                        DeptCode = _qryRequisitionList[i].DeptCode,
                        ReqDate = DateTime.Now,
                        PayClassCode = _qryRequisitionList[i].PayClassCode,
                        Active = true,
                        VendorCode = _qryRequisitionList[i].VendorCode, //await _RepositoryUnit.VendorRepository.GetVendorCodeByDisplayName(_qryRequisitionList[i].PayeeName), // Get VendorCode by payee name
                        PayeeName = _qryRequisitionList[i].PayeeName,
                        PayMethodCode = _qryRequisitionList[i].PayMethodCode,
                        BankCode = _qryRequisitionList[i].BankCode,
                        Destination = "",
                        TotalAmount = _RequisitionTotalAmount,
                        Remarks = _qryRequisitionList[i].Remarks,
                        Void = false,
                        VoidUser = string.Empty,
                        VoidDate = Convert.ToDateTime("1/1/1900"),
                        Printed = false,
                        AuditUser = _qryRequisitionList[i].AuditUser,
                        AuditDate = DateTime.Now,
                        UploadStat = false,
                        EditUser = _qryRequisitionList[i].AuditUser,
                        EditDate = DateTime.Now,
                        TrxMonth = "DEC",
                        TrxWeek = 1,
                        RefNo = _qryRequisitionList[i].RefNo,
                        Vat = qryresult.Vat,
                        NetofVat = qryresult.NetOfVat,
                        TotalTax = qryresult.TotalTax,
                        Deduction = qryresult.Deduction,
                        Discount = qryresult.Discount,
                        AmountDue = qryresult.AmountDue,
                        TransType = "REG",
                        DtlCompanyCode = _CompanyCode
                    };

                    _TblRequisitionhdrList.Add(_TblRequisitionhdr);

                    //reset
                    _CompanyCode = string.Empty;
                    _VendorName = string.Empty;
                    _RequisitionTotalAmount = 0.00m;
                    _TempReqNo++;
                    Listqrydtl = new List<qryRequisitionDtl>();
                }
            }

            _RequisitionParams.RequisitionHdrList = _TblRequisitionhdrList;
            _RequisitionParams.RequisitionDtlList = _TblRequisitiondtlList;
            //_RequisitionParams.qryRequisitionList = _qryRequisitionList;

            //TblResponse _TblResponse = new TblResponse();

            return await Task.FromResult(_RequisitionParams);
        }

        [HttpPost("ComputeRequisitionHdr")]
        public async Task<IActionResult> ComputeRequisitionHdr(List<qryRequisitionDtl> qryRequisitionDtls)
        {
            try
            {
                _response = new TblResponse();
                string error = "Compute - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + Request.Path.Value + "";
                var result = await _ServiceUnit.PaymentRequestService.ComputeBreakDownHdr(qryRequisitionDtls);


                return Ok(_response);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + Request.Path.Value + " - " + ex.Message;
                _logger.LogError(error);

                //await _RepositoryUnit.ResponseRepository.CreateResponse(pscode, "FAILED", ex.Message, Utilities.GetmethodName());
                return BadRequest(_response);
            }
        }

        [HttpPost("ComputeCasketInventory_CreditAP_EWT")]
        public async Task<RequisitionParams> ComputeCasketInventory_CreditAPandEWT(RequisitionParams RequisitionParams)
        {
            try
            {
                _response = new TblResponse();
                string error = "Compute - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + Request.Path.Value + "";
                var result = await _ServiceUnit.RequisitionService.ComputeCasketInventory_CreditAPandEWT(RequisitionParams);

                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {

                string error = "Error - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + Request.Path.Value + " - " + ex.Message;
                _logger.LogError(error);

                //await _RepositoryUnit.ResponseRepository.CreateResponse(pscode, "FAILED", ex.Message, Utilities.GetmethodName());
                return RequisitionParams;
            }
        }

        [HttpPost("CreateCMSBKP")]
        public async Task<TblResponse> CreateCMSBKP(ServiceParams ServiceParams)
        {
            string destination = string.Empty;
            string pathOutBox = ServiceParams.BKPSavingPath;

            try
            {
                //Thread.Sleep(60000);
                //List<qryCMSPOHdr> qryCMSPOHdrList = new List<qryCMSPOHdr>();
                //List<qryCMSPODtl> qryCMSPODtlList = new List<qryCMSPODtl>();
                qryBKPName _qryBKPName = new qryBKPName();
                string bkpnameOrig = string.Empty;
                string newBkpNameOrig = string.Empty;

                //qryCMSPOHdrList = ServiceParams.qryCMSPOHdrList;
                //qryCMSPODtlList = ServiceParams.qryCMSPODtlList;
                //List<qryCMSPOHdr> a  = qryCMSPOHdrList.DistinctBy(a => a.ChapelCode).ToList();  
                DateTime tmpNow = DateTime.Now;
                trxCode = tmpNow.ToString("yyMMMdd hhmmt").ToUpper();
                mdbCode = "";
                template =  ServiceParams.BKPTemplatePath;
                fileName = "PO" + ServiceParams.qryCMSPOHdrList.Select(a => a.FactoryCode).First() + ServiceParams.qryCMSPOHdrList.Select(a=>a.PONo).First() + mdbCode + trxCode + ".MDB";

                _qryBKPName.BKPName = fileName;
                _qryBKPName.FactoryCode = ServiceParams.qryCMSPOHdrList.Select(a => a.FactoryCode).FirstOrDefault();
                _qryBKPName.SystemCode = "CMS";
                _qryBKPName.BKPType = "PO";
                _qryBKPName.StartDate = DateTime.Now;
                _qryBKPName.EndDate = DateTime.Now.AddDays(7);

                destination = Path.Combine(pathOutBox.Replace(",", ""), fileName);

                if (Directory.Exists(Path.GetDirectoryName(destination)))
                {
                    Directory.Delete(Path.GetDirectoryName(destination), true);
                }

                _ServiceUnit.RequisitionService.CopyTemplate(fileName, destination, template);

                ServiceParams.qryBKPName = _qryBKPName;
                conn = new MSAccessDBManager(destination, "", ServiceParams);

                //_ServiceUnit.RequisitionService.InsertqryCMSPOHdr(qryCMSPOHdrList, ref sb, ref conn);
                //_ServiceUnit.RequisitionService.InsertqryCMSPODtl(qryCMSPODtlList, ref sb, ref conn);

                //_ServiceUnit.RequisitionService.InsertFileName(fileName, ref conn);

                //Thread.Sleep(10000);

                //_ServiceUnit.RequisitionService.SetDBPassword(ref conn);
                //_ServiceUnit.RequisitionService.Dispose(ref conn);

                _ServiceUnit.RequisitionService.Compress(new FileInfo(Path.Combine(destination)));
                newBkpNameOrig = destination.Replace(".MDB", ".BKP");
                System.IO.File.Delete(destination);

            }
            catch (Exception ex)
            {
                throw new Exception("BKP Failed to Create");
            }

            TblResponse _TblResponse = new TblResponse
            {
                Status = "SUCCESS",
                AuditDate = DateTime.Now,
                ErrorMessage = "SUCCESS",
                MethodName = "CREATE BKP",
                TrxNo = pathOutBox,
                UniqueInfo = "1"
            };

            return await Task.FromResult(_TblResponse);
            //return pathOutBox.ToUpper();
        }
         

    }
}
