using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OSP.SPASv2.Service.Model;
using OSP.SPASv2.Service;
using OSP.SPASv2.Domain;
using OSP.SPASv2.Domain.View;
using OSP.SPASv2.Service.Services;
using System.Text.RegularExpressions;
using OSP.SPASv2.Domain.Params;
using System.Text;
using OSP.SPASv2.Service.Utility;
using System.IO.Compression;
using OSP.Common.Domain.Tables;

namespace OSP.SPASv2.Service.Services
{
    public class RequisitionService
    {
        ServiceUnit _ServiceUnit = new ServiceUnit();

        public RequisitionService()
        {

        }

        public async Task<string> GeneratePaymentrequestno(string lastno, string companycode, DateTime auditdate)
        {
            string paymentreqno = "";
            //paymentreqno= lastno.Substring(lastno.Length-6)+ 1;
            if (lastno.Length < 6)
            {
                paymentreqno = lastno;
            }
            else
            {
                paymentreqno = lastno.Substring(lastno.Length - 6);
            }

            paymentreqno = Convert.ToString(Convert.ToInt16(paymentreqno) + 1);
            paymentreqno = companycode + DateTime.Now.ToString("yyMM") + '-' + paymentreqno.PadLeft(6, '0');
            //paymentreqno =   companycode +branchcode+'-'+Convert.ToString(Convert.ToInt16(lastno.Substring(lastno.Length-6) + 1).PadLeft(6, '0');
            return await Task.FromResult(paymentreqno.ToUpper());
        }

        public async Task<string> GenerateBatchNo(string LastNo)
        {
            string BatchReqNo = "";
            //paymentreqno= lastno.Substring(lastno.Length-6)+ 1;
            if (LastNo.Length < 6)
            {
                BatchReqNo = LastNo;
            }
            else
            {
                BatchReqNo = LastNo.Substring(LastNo.Length - 6);
            }

            BatchReqNo = Convert.ToString(Convert.ToInt16(BatchReqNo) + 1);
            BatchReqNo = "BN" + DateTime.Now.ToString("yyMM") + BatchReqNo.PadLeft(6, '0');//companycode + branchcode + auditdate.ToString("yyMM") + '-' + BatchReqNo.PadLeft(6, '0');
            //paymentreqno =   companycode +branchcode+'-'+Convert.ToString(Convert.ToInt16(lastno.Substring(lastno.Length-6) + 1).PadLeft(6, '0');
            return await Task.FromResult(BatchReqNo.ToUpper());
        }

        public async Task<RequisitionParams> ComputeCasketInventory_CreditAPandEWT(RequisitionParams requisitionParams)
        {
            decimal _TotalInventory = requisitionParams.TblDataSourceDtl_List.Where(a => a.AccountCode.Equals(requisitionParams.RefAccountMap.Where(a => a.Hierarchy.Equals(0)).Select(a => a.AccountCode).FirstOrDefault())).Sum(a => a.Debit);
            decimal _TotalFreight = requisitionParams.TblDataSourceDtl_List.Where(a => a.AccountCode.Equals(requisitionParams.RefAccountMap.Where(a => a.Hierarchy.Equals(1)).Select(a => a.AccountCode).FirstOrDefault())).Sum(a => a.Debit);
            decimal _TotalEWT = _TotalInventory * requisitionParams.EWTPercentage;
            decimal _TotalHPDeduction = requisitionParams.TblDataSourceDtl_List.Where(a => a.AccountCode.Equals("1443000")).Sum(a => a.Credit);

            requisitionParams.CreditAP = _TotalInventory + _TotalFreight + requisitionParams.TotalVAT - _TotalEWT;
            requisitionParams.CreditEWT = _TotalEWT;

            if (_TotalHPDeduction > 0)
            {

                _TotalEWT = _TotalInventory * requisitionParams.EWTPercentage;
                _TotalInventory = (_TotalInventory + _TotalFreight) - _TotalHPDeduction;

                requisitionParams.CreditAP = _TotalInventory - _TotalEWT;
                requisitionParams.CreditEWT = _TotalEWT;
            }


            return requisitionParams;
        }

        public void CopyTemplate(string fileName, string pathOutBox, string template)
        {
            TblResponse _response = new TblResponse();
            string source = template;

            //string destination = Path.Combine(pathOutBox.Replace(",",""),  fileName);
            
            if (!Directory.Exists(Path.GetDirectoryName(pathOutBox)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(pathOutBox));
            } 
          

            File.Copy(source, pathOutBox, true);
           // return _response;
        }

        public void InsertFileName(string FileName, ref MSAccessDBManager conn)
        {
            string bkpName = FileName.ToUpper().Replace(".MDB", ".BKP");
            conn.ExecuteQuery("Insert Into BkpName values ('" + bkpName + "')");
        }

        public void SetDBPassword(ref MSAccessDBManager conn)
        {
            conn.ExecuteQuery("ALTER DATABASE PASSWORD " + Utilities.MdbPw + " NULL ;");

        }

        public void Dispose(ref MSAccessDBManager conn)
        {
            conn.Dispose();
            conn = null;
        }
        //private void CopyTemplate(string fileName, string pathOutBox, string template, ref MSAccessDBManager conn)
        //{
        //    string source = template;

        //    string destination = pathOutBox + fileName;

        //    System.IO.File.Copy(source, destination);
        //}
        public void InsertqryCMSPOHdr(List<qryCMSPOHdr> qryCMSPOHdr, ref StringBuilder sb, ref MSAccessDBManager conn)
        {
            sb = new StringBuilder();
            //sb.Append("Select * from ");
            //sb.Append("tblROP ");
            //sb.Append("Where LPANo in " + strWhere + "");

            //IList<tblROP> _tblROP = new List<tblROP>();

            //using (var _context = new AAVContext())
            //{
            //    _tblROP = _context.Database.SqlQuery<tblROP>(sb.ToString()).ToList();

            foreach (var item in qryCMSPOHdr)
            {
                sb = new StringBuilder();
                sb.Append("Select ");
                sb.Append("'" + item.PONo + "' as [LPANo]");
                sb.Append("'" + item.FactoryCode + "' as [FactoryCode]");
                sb.Append("'" + item.ChapelCode + "' as [ChapelCode]");
                sb.Append("'" + item.PODate + "' as [PODate]");
                sb.Append("'" + item.POReceivedDate + "' as [POReceivedDate]");
                sb.Append("'" + item.Terms + "' as [Terms]");
                sb.Append("'" + item.Remarks + "' as [Remarks]");
                sb.Append("'" + item.POAmount + "' as [POAmount]");
                sb.Append("'" + item.AuditUser + "' as [AuditUser]");
                sb.Append("'" + item.AuditDate + "' as [AuditDate]");
                sb.Append("'" + item.EditUser + "' as [EditUser]");
                sb.Append("'" + item.EditDate + "' as [EditDate]");
                sb.Append("'" + item.Void + "' as [Void]");
                sb.Append("'" + item.VoidUser + "' as [VoidUser]");
                sb.Append("'" + item.VoidDate + "' as [VoidDate]");

                conn.ExecuteQuery(sb.ToString());
            }
            //} 
        }

        public void InsertqryCMSPODtl(List<qryCMSPODtl> qryCMSPODtl, ref StringBuilder sb, ref MSAccessDBManager conn)
        {
            //sb = new StringBuilder();
            //sb.Append("Select * from ");
            //sb.Append("tblROP ");
            //sb.Append("Where LPANo in " + strWhere + "");

            //IList<tblROP> _tblROP = new List<tblROP>();

            //using (var _context = new AAVContext())
            //{
            //    _tblROP = _context.Database.SqlQuery<tblROP>(sb.ToString()).ToList();

            foreach (var item in qryCMSPODtl)
            {
                sb = new StringBuilder();
                sb.Append("Select ");
                sb.Append("'" + item.FactoryCode + "' as [PONo]");
                sb.Append("'" + item.PONo + "' as [PONo]");
                sb.Append("'" + item.CasketCode + "' as [CasketCode]");
                sb.Append("'" + item.OrderQty + "' as [OrderQty]");
                sb.Append("'" + item.POAmount + "' as [POAmount]");
                sb.Append("'" + item.AuditUser + "' as [AuditUser]");
                sb.Append("'" + item.AuditDate + "' as [AuditDate]");
                sb.Append("'" + item.EditUser + "' as [EditUser]");
                sb.Append("'" + item.EditDate + "' as [EditDate]");

                conn.ExecuteQuery(sb.ToString());
            }
            //} 
        }

        public void Compress(FileInfo fileToCompress)
        {
            string path = string.Empty;
            using (FileStream originalFileStream = fileToCompress.OpenRead())
            {
                if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".BKP")
                {
                    path = fileToCompress.FullName.Substring(0, fileToCompress.FullName.Length - 4) + ".BKP";
                    using (FileStream compressedFileStream = File.Create(path))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(compressionStream);
                            Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                                fileToCompress.Name, fileToCompress.Length.ToString(), compressedFileStream.Length.ToString());
                        }
                    }
                }
            }
        }

        //private void RecomputeDtl(ref List<TblDataSourceDtl> TblDataSourceDtlList)
        //{

        //}
    }
}
