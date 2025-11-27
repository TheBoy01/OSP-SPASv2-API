using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Presentation;
//using DocumentFormat.OpenXml.Drawing.Charts;
//using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using OSP.Common.Domain.View;
using OSP.SPASv2.Domain.Params;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Repository.Context;
using OSP.SPASv2.Repository.Parameters;
using OSP.SPASv2.Repository.Repository.MainRepository;
using OSP.SPASv2.Repository.Rules;
using OSP.SPASv2.Repository.Utility;
using OSP.SPASv2.Web.Models;
using SPASv2.Context;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks.Dataflow;
using System.Web.Http.Results;
using SysIO = System.IO;

namespace OSP.SPASv2.Repository.Repository.ServiceUnit
{

    public interface IVendorService
    {
        Task<string> CreateExcelForVendor(RequisitionParams RequisitionParams, string ReportPath);
    }


    public class VendorService : IVendorService
    {
        RepositoryUnit _RepositoryUnit;
        RepositoryUnit _RepositoryUnitV1;
        SPASv2Context _SPASv2Context;
        SPASv1Context _SPASv1Context;

        string ServerFiles;

        public VendorService(SPASv2Context context)
        {
            _SPASv2Context = context;
            _RepositoryUnit = new RepositoryUnit(_SPASv2Context);
        }

        public VendorService(SPASv1Context context)
        {
            _SPASv1Context = context;
            _RepositoryUnitV1 = new RepositoryUnit(_SPASv1Context);
        }


        public async Task<string> CreateExcelForVendor(RequisitionParams RequisitionParams, string ReportPath)
        {
            string FileName = string.Empty;
            string PONo = string.Empty;
            string BatchApprovalNo = string.Empty;
            TblResponse _resp = new TblResponse();
            TblAssignedtoVendor_CMS _TblAssignedtoVendor_CMS = new TblAssignedtoVendor_CMS();
            try
            {
                DataTable POTable = new DataTable("Purchase Order");
                POTable.Columns.Add(new DataColumn("Vendor", typeof(string)));
                POTable.Columns.Add(new DataColumn("Chapel", typeof(string)));
                POTable.Columns.Add(new DataColumn("Casket", typeof(string)));
                POTable.Columns.Add(new DataColumn("Quantity", typeof(string)));
                //POTable.Columns.Add(new DataColumn("Quantity", typeof(int)));
                //POTable.Columns.Add(new DataColumn(" ", typeof(string)));
                //POTable.Columns.Add(new DataColumn("Casket Item", typeof(string)));
                //POTable.Columns.Add(new DataColumn("Barcodes", typeof(string)));

                DataTable Barcodes = new DataTable("Barcodes");
                Barcodes.Columns.Add(new DataColumn("OrderNo", typeof(string)));
                Barcodes.Columns.Add(new DataColumn("PONo", typeof(string)));
                Barcodes.Columns.Add(new DataColumn("Casket", typeof(string)));
                Barcodes.Columns.Add(new DataColumn("Barcodes", typeof(string)));

                List<RptPurchaseorder> _RptPurchaseorder = new List<RptPurchaseorder>();
                _RptPurchaseorder = await _RepositoryUnit.rptPurchaseorderRepository.GetListByPONo(RequisitionParams.PONo);
                PONo = _RptPurchaseorder.Select(a => a.PONo).FirstOrDefault();
                BatchApprovalNo = await _RepositoryUnit.BatchApprovalRepository.GetBatchNoByReqNo(_RptPurchaseorder.Select(a => a.ReqNo).FirstOrDefault());

                //var PODetails = _RptPurchaseorder.GroupBy(a => new { a.VendorName, a.Department, a.Description }).Select(n => new { VendorName = n.Key.VendorName, Department = n.Key.Department, Description = n.Key.Description }).ToList();
                //foreach (var item in PODetails)
                //{
                //    //if (item.Description.Contains("DJANGO"))
                //    //{ 
                //    //}
                //    int Qty = _RptPurchaseorder.Where(a => a.VendorName.Equals(item.VendorName) && a.Department.Equals(item.Department) && a.Description.Equals(item.Description)).Select(a => a.Qty).FirstOrDefault();
                //    string Desc = Utilities.ChangeItemDjango(item.Description);
                //    POTable.Rows.Add(item.VendorName, item.Department, Desc, Qty);

                //    //Barcodes example
                //    //for (int i = 0; i < item.Qty; i++)
                //    //{
                //    //    DeliveryTable.Rows.Add(item.Description, item.VendorName.Substring(0, 3) + "0000" + item.Description.Substring(0, 5).Replace(".", "0") + i);
                //    //}
                //}
                List<TblItemBarcodes> barcodeList = new List<TblItemBarcodes>();
                string _BarCode = string.Empty;
                _TblAssignedtoVendor_CMS = new TblAssignedtoVendor_CMS();
                string Orderno = string.Empty;
                //fetch barcodes
                var _barcodes = await _RepositoryUnit.TblItemBarcodesRepository.GetTblItemBarcodesAsync(RequisitionParams.PONo);
                var tblpo = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(RequisitionParams.PONo);
                _TblAssignedtoVendor_CMS = await _RepositoryUnit.TblAssignedtoVendor_CMSRepository.ReadByReqNo(tblpo.Reqno);

                if (_barcodes != null && _barcodes.Count > 0)
                {
                    Orderno = _TblAssignedtoVendor_CMS.OrderNo;

                    //barcodeList.AddRange(_barcodes);
                    
                    foreach (var item in _barcodes)
                    {
                        var casketdesc = await _RepositoryUnit.RefItemsRepository.GetItemDesc(item.ItemCode);
                        Barcodes.Rows.Add(item.BarCode, Utilities.ChangeItemDjango(casketdesc, true));
  
                    }

                    foreach (var item in _RptPurchaseorder)
                    {

                        POTable.Rows.Add(item.VendorName, item.Department, Utilities.ChangeItemDjango(item.Description, true), item.Qty);
                    }
                  


                }
                else
                {
                    for (int i = 0; i < _RptPurchaseorder.Count; i++)
                    {
                        TblVendor _TblVendor = new TblVendor();
                        _TblVendor = await _RepositoryUnit.VendorRepository.Read(await _RepositoryUnit.VendorRepository.GetVendorCodeByDisplayName(_RptPurchaseorder[i].VendorName));
                        string _ItemCode = await _RepositoryUnit.RefItemsRepository.GetItemCodeByDesc(_RptPurchaseorder[i].Description);
                  
                        

                        POTable.Rows.Add(_RptPurchaseorder[i].VendorName, _RptPurchaseorder[i].Department, Utilities.ChangeItemDjango(_RptPurchaseorder[i].Description, true), _RptPurchaseorder[i].Qty);


                        
                        TblItemBarcodes _latestBarcode = new TblItemBarcodes();



                        //---->> generate barcodes <---
                        for (int item = 0; item < _RptPurchaseorder[i].Qty; item++)
                        {


                            if (string.IsNullOrEmpty(_BarCode))
                            {
                                _latestBarcode = await _RepositoryUnit.TblItemBarcodesRepository.GetLatestBarCode(_TblVendor, _ItemCode);


                            }
                            _BarCode = GenerateBarCode(_latestBarcode, _TblVendor.Prefix, _ItemCode, _BarCode);

                            TblItemBarcodes newBarcode = new TblItemBarcodes()
                            {
                                PONo = _RptPurchaseorder[i].PONo,
                                BarCode = _BarCode,
                                ItemCode = _ItemCode,
                                VendorCode = _TblVendor.VendorCode,
                                AuditUser = RequisitionParams.UserID,
                                AuditDate = DateTime.Now,
                                Cancel = false
                            };

                            barcodeList.Add(newBarcode);
                            Barcodes.Rows.Add(_BarCode, Utilities.ChangeItemDjango(_RptPurchaseorder[i].Description, true));

                        }




                        //for (int Item = 0; Item < _RptPurchaseorder[i].Qty; Item++)
                        //{
                        //    _resp = new TblResponse();
                        //    string _BarCode = string.Empty;

                        //    TblItemBarcodes _TblItemBarcodes = new TblItemBarcodes();

                        //    _TblItemBarcodes = await _RepositoryUnit.TblItemBarcodesRepository.GetLatestBarCode(_TblVendor, _ItemCode);
                        //    _BarCode = GenerateBarCode(_TblItemBarcodes, _TblVendor.Prefix, _ItemCode);

                        //    _TblItemBarcodes = new TblItemBarcodes()
                        //    {
                        //        PONo = _RptPurchaseorder[i].PONo,
                        //        BarCode = _BarCode,
                        //        ItemCode = _ItemCode,
                        //        VendorCode = _TblVendor.VendorCode,
                        //        AuditUser = RequisitionParams.UserID,
                        //        AuditDate = DateTime.Now,
                        //        Cancel = false
                        //    };

                        //    _resp = await _RepositoryUnit.TblItemBarcodesRepository.Create(_TblItemBarcodes);
                        //    Barcodes.Rows.Add(_BarCode, Utilities.ChangeItemDjango(_RptPurchaseorder[i].Description, true));
                        //}

                        //POTable.Rows.Add("",item.Description); 
                        //Barcodes example
                        //for (int i = 0; i < item.Qty; i++)
                        //{
                        //    DeliveryTable.Rows.Add(item.Description, item.VendorName.Substring(0, 3) + "0000" + item.Description.Substring(0, 5).Replace(".", "0") + i);
                        //}
                    }

                    if (barcodeList.Count > 0)
                    {
                        try
                        {
                            _resp = await _RepositoryUnit.TblItemBarcodesRepository.BulkCreate(barcodeList);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error during bulk insert: {ex.Message}");
                        }
                    }
                }
                   

             

              
                
                FileName =  SysIO.Path.Combine(BatchApprovalNo,PONo + "-" + _RptPurchaseorder.Select(a => a.VendorName).FirstOrDefault());
                //int Counter = 1;
                //while (System.IO.File.Exists(Path.Combine(ServerFiles, "CIS PO", FileName + ".xlsx")))
                //{
                //    Counter++;
                //    FileName = FileName + "_" + Counter;
                //}

                using (XLWorkbook wb = new XLWorkbook(ReportPath))
                {
                    //var ws = wb.Worksheets.Add(POTable);
                    //ws = wb.Worksheets.Add(DeliveryTable); 

                    var wsPO = wb.Worksheet("Purchase Order");
                    var wsBCodes = wb.Worksheet("Barcodes"); //wb.Worksheets.Add(Barcodes);

                    //var sortedRows = Barcodes.AsEnumerable()
                    //        .OrderBy(row => row["Barcodes"]);
                    //DataTable sortedDataTable = sortedRows.CopyToDataTable();

                    //DataView BarcodeView = Barcodes.DefaultView;
                    //BarcodeView.Sort = "Barcodes";
                    //Barcodes = BarcodeView.ToTable();
                    wsBCodes.Cell(2, 1).InsertData(Barcodes);
                    wsPO.Cell(2, 1).InsertData(POTable);

                    wsBCodes.Columns().AdjustToContents();
                    wsPO.Columns().AdjustToContents();

                    var ws = wb.Worksheet("Delivery");

                    //ws.Cell("A1").Value = "PONo";
                    //ws.Cell("A2").Value = "Chapel";
                    //ws.Cell("A3").Value = "Vendor Name";
                    //ws.Cell("A4").Value = "Delivery No";
                    //ws.Cell("A5").Value = "Delivery Date";
                    //ws.Cell("A6").Value = "Remarks";

                    //var wsRange2 = ws.Range(1, 1, 7, 1); //
                    //wsRange2.Style.Font.Bold = true;

                    ws.Cell("B1").Value = Orderno;
                    ws.Cell("B2").Value = RequisitionParams.PONo;
                    ws.Cell("B3").Value = _RptPurchaseorder.Select(a => a.Department).FirstOrDefault();
                    ws.Cell("B4").Value = _RptPurchaseorder.Select(a => a.VendorName).FirstOrDefault();

                    //ws.Cell("B7").Value = "Casket Description";
                    //ws.Cell("C7").Value = "Barcode";
                    ws.Columns().AdjustToContents();
                    ws.Column(1).Width = 40;

                    //for (int i = 0; i < _RptPurchaseorder.Count; i++)
                    //{
                    //    _resp = new TblResponse();
                    //    string _BarCode = string.Empty;
                    //    //string _VendorCode = a;
                    //    TblVendor _TblVendor = new TblVendor();
                    //    _TblVendor = await _RepositoryUnit.VendorRepository.Read(await _RepositoryUnit.VendorRepository.GetVendorCodeByDisplayName(_RptPurchaseorder[i].VendorName));
                    //    TblItemBarcodes _TblItemBarcodes = new TblItemBarcodes();
                    //    string _ItemCOde = await _RepositoryUnit.RefItemsRepository.GetItemCodeByDesc(_RptPurchaseorder[i].Description);
                    //    _TblItemBarcodes = await _RepositoryUnit.TblItemBarcodesRepository.GetLatestBarCode(_TblVendor, _ItemCOde);
                    //    _BarCode = GenerateBarCode(_TblItemBarcodes, _TblVendor.Prefix, _ItemCOde);

                    //    int _row = i + 8;
                    //    ws.Cell("A" + _row).Value = "";
                    //    ws.Cell("B" + _row).Value = _RptPurchaseorder[i].Description;
                    //    ws.Cell("C" + _row).Value = _BarCode;

                    //    _TblItemBarcodes = new TblItemBarcodes()
                    //    {
                    //        BarCode = _BarCode,
                    //        ItemCode = _ItemCOde,
                    //        VendorCode = _TblVendor.VendorCode,
                    //        AuditUser = RequisitionParams.UserID,
                    //        AuditDate = DateTime.Now
                    //    };

                    //    _resp = await _RepositoryUnit.TblItemBarcodesRepository.Create(_TblItemBarcodes);

                    //    //POTable.Rows.Add("",item.Description); 
                    //    //Barcodes example
                    //    //for (int i = 0; i < item.Qty; i++)
                    //    //{
                    //    //    DeliveryTable.Rows.Add(item.Description, item.VendorName.Substring(0, 3) + "0000" + item.Description.Substring(0, 5).Replace(".", "0") + i);
                    //    //}
                    //}


                    //var wsRange = ws.Range(2, 15, custTable.Rows.Count + 1, 19);
                    //wsRange.Style.NumberFormat.Format = "#,###,###.0000;(#,###,###.0000)";

                    //copy to local server \\192.168.1.6\spasv2$\Files\CIS PO 

                    var range = wsBCodes.Range($"A2:B{Barcodes.Rows.Count + 1}");

                    // Apply sorting to the range by the first column
                    range.Sort("A", XLSortOrder.Ascending);

                    ServerFiles = RequisitionParams.ServerPOPath;
                    wb.SaveAs(SysIO.Path.Combine(ServerFiles, "CIS PO", FileName + ".xlsx"), true);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //using (MemoryStream stream = new MemoryStream())
            //{
            //    //download file
            //    wb.SaveAs(stream);
            //    //return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName + ".xlsx");

            //}
            return SysIO.Path.Combine(ServerFiles, "CIS PO", FileName + ".xlsx");
        }
        private string GenerateBarCode(TblItemBarcodes TblItemBarcodes, string VendorPrefix, string ItemCode,string barcode)
        {
            string _BarCode = string.Empty;
            string ctr = string.IsNullOrEmpty(barcode) ? "1" : barcode.Substring(barcode.Length-4);
            _BarCode = VendorPrefix + ItemCode + DateTime.Now.ToString("yyMM");
            if (TblItemBarcodes != null || ctr != "1")
            {
                //if (!string.IsNullOrEmpty(TblItemBarcodes.BarCode) )
                //{

                // int _Increment = Convert.ToInt32(TblItemBarcodes.BarCode.Substring(TblItemBarcodes.BarCode.Length - 5)) + 1;
                ctr = Convert.ToString(Convert.ToInt32(ctr) + 1);
                    _BarCode = _BarCode + ctr.ToString().PadLeft(5, '0');
                //}
            }
            else { _BarCode = _BarCode + "00001"; }

            return _BarCode;
        }

    }
}
