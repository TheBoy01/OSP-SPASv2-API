using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Mvc;
//using System.Data.Entity;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.Data.SqlClient;
using DocumentFormat.OpenXml.Office2010.Drawing;
using OSP.Common.Domain.View;
using Microsoft.Reporting.NETCore;

namespace OSP.SPASv2.Repository.Repository
{
    public class TblRequisitionHdrRepository : ITblRequisitionHdrRepository<TblRequisitionhdr>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblRequisitionhdr> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public TblRequisitionHdrRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblRequisitionhdr>(_context);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        public async Task<TblResponse> BulkInsert(List<TblRequisitionhdr> entity)
        {
            try
            {
                await _AbstractRepository.BulkInsert(entity);
                return await Task.FromResult(_response);
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        public async Task<TblResponse> Create(TblRequisitionhdr entity)
        {
            await _AbstractRepository.Insert(entity);
            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> Update(TblRequisitionhdr entity)
        {
            var oldEntity = await _AbstractRepository.GetByID(entity.Reqno);
            _AbstractRepository.Update(oldEntity, entity);
            //new Task(() => { TrailEdit(oldEntity, entity, "TblRequisitionhdr", "Requisitionhdr", entity.Reqno); }).Start();
            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> Delete(TblRequisitionhdr entity)
        {
            await _AbstractRepository.Delete(entity);
            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> Delete(object Primarykey, object Primarykey2)
        {
            await _AbstractRepository.DeleteByComposite(Primarykey, Primarykey2);
            return await Task.FromResult(_response);
        }
        #endregion

        #region Public Functions

        public async Task<IList<TblRequisitionhdr>> GetAllObjects()
        {
            try
            {
                var vlist = await Task.FromResult(_context.TblRequisitionhdr.FromSqlRaw("select * from TblRequisitionhdr").ToList());
                return vlist;
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        public async Task<TblResponse> CreateTblRequisitionHdr(TblRequisitionhdr entity)
        {
            try
            {
                await _AbstractRepository.Insert(entity);
                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<TblRequisitionhdr> GetLatestPRRow(string companycode)
        {
            try
            {
                string YearMonth = string.Empty;
                YearMonth = companycode.ToUpper().TrimStart().TrimEnd() + DateTime.Today.Year.ToString().Substring(2, 2) + DateTime.Now.ToString("MM") + "-";
                var vlist = await _context.TblRequisitionhdr.FromSqlRaw("select top 1 * from TblRequisitionhdr where left(Reqno," + YearMonth.Length + ")= '" + YearMonth +"' order by Reqno desc").FirstOrDefaultAsync();

                //var vlist = await _context.TblRequisitionhdr.FromSqlRaw("select top 1 * from TblRequisitionHdr where companycode ='" + companycode + "'  order by auditdate desc").FirstOrDefaultAsync()
                return vlist; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TblRequisitionhdr> GetlatestPRBatchNo()
        {
            try
            {
                string YearMonth = string.Empty;
                YearMonth = "BN" + DateTime.Today.Year.ToString().Substring(2, 2) + DateTime.Now.ToString("MM");
                var vlist = await _context.TblRequisitionhdr.FromSqlRaw("select top 1 * from TblRequisitionhdr where left(BatchNo," + YearMonth.Length + ")= '" + YearMonth +"' order by BatchNo desc").FirstOrDefaultAsync();
                //var vlist = await _context.TblRequisitionhdr.FromSqlRaw("select top 1 * from TblRequisitionhdr where batchno != '' order by auditdate desc").FirstOrDefaultAsync();
                //         var vlist = await _context.TblPaymentrequesthdr.OrderByDescending(n=>n.AuditDate).FirstOrDefaultAsync(n=>n.CompanyCode== companycode && n.DeptCode==branchcode);

                //vlist = new TblPaymentrequesthdr();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<TblRequisitionhdr> ReadRequisitionHdr(string ReqNo)
        {
            try
            {
               
                return await _context.TblRequisitionhdr.Where(a => a.Reqno.Equals(ReqNo)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<qryRequisitionInfo> GetRequisitionInfo(string ReqNo)
        {
            try
            {
                //var vlist = await _context.qryRequisitionInfo.FromSqlRaw("select * from qryRequisitionInfo where ReqNo='" + ReqNo + "'").FirstOrDefaultAsync();
                var vlist = await _context.qryRequisitionInfo.Where(t=> t.ReqNo == ReqNo).FirstOrDefaultAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<qryRequisitionInfo> GetRequisitionInfoByApprovalNo(string ApprovalNo)
        {
            try
            {
                //var vlist = await _context.qryRequisitionInfo.FromSqlRaw("select * from qryRequisitionInfo where ReqNo='" + ReqNo + "'").FirstOrDefaultAsync();
                var vlist = await _context.qryRequisitionInfo.Where(t => t.ReqApprovalNo == ApprovalNo).FirstOrDefaultAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<qryRequisitionInfo> GetRequisitionInfoByRefNo(string RefNo)
        {
            try
            {
                //var vlist = await _context.qryRequisitionInfo.FromSqlRaw("select * from qryRequisitionInfo where ReqNo='" + ReqNo + "'").FirstOrDefaultAsync();
                var vlist = await _context.qryRequisitionInfo.Where(t => t.RefNo == RefNo).FirstOrDefaultAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<qryRequisitionInfo>> GetRequisitionInfoByMainReqNo(string MainReqNo)
        {
            try
            {
                var vlist = await _context.qryRequisitionInfo.FromSqlRaw("select * from qryRequisitionInfo where MainReqNo = '"+ MainReqNo + "' and ReqNo <> '"+ MainReqNo + "'").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }



        public async Task<qryRequisitionInfo> GetRequisitionInfoByMainReq(string MainReqNo)
        {
            try
            {
                var vlist = await _context.qryRequisitionInfo.FromSqlRaw("select * from qryRequisitionInfo where MainReqNo = '" + MainReqNo + "'").FirstOrDefaultAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<qryRequisitionItem>> GetRequisitionItemList(string ReqNo)
        {
            try
            {
                var vlist = await _context.qryRequisitionItem.FromSqlRaw("select * from qryRequisitionItem where ReqNo='" + ReqNo + "'").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<TblRequisitionhdr> GetMasterRequisition(string ReqNo)
        {
            try
            {
                var vlist = await _context.TblRequisitionhdr.FromSqlRaw("select * from tblrequisitionhdr where ReqNo='" + ReqNo + "' and a.Reqno=a.MainReqNo ").FirstOrDefaultAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetReqPOPY(string reqNo)
        {
            try
            {
                var vlist = await _context.TblRequisitionhdr.Join(_context.TblPaymentrequisitionhdr, a => a.Reqno, b => b.Reqno,

                    (a, b) => new { a.Reqno  }).Where(a => a.Reqno.Equals(reqNo)).Select(a => a.Reqno).FirstOrDefaultAsync();


                return string.IsNullOrEmpty(vlist) ? "PO" : "PY";
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<TblResponse> UpdateTransType(string _transtype,string reqno)
        {

            _context.Database.ExecuteSqlRaw("Update tblrequisitionhdr set Transtype = '" + _transtype + "' where reqno = '" + reqno + "'");
            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> UpdateRequestQtySummary(string ReqNo)
        {
            try
            {
                await Task.FromResult(_context.Database.ExecuteSqlRaw("exec SP_UpdateReqHdrBalance '"+ ReqNo + "' "));
                return await Task.FromResult(_response);
            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }

        }

        public async Task<TblResponse> VoidRequisitionByReqNo(string ReqNo, string UserId)
        {
            try
            {
                await Task.FromResult(_context.Database.ExecuteSqlRaw("exec SP_CancelRequisition '" + ReqNo + "' ,'"+UserId+"' "));
                return await Task.FromResult(_response);
            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }

        }

        public async Task<IList<qryVendorRunningBalance>> GetVendorRunningBalance(string PayClassCode, string AsOfMode)
        {

            try
            {
                IList<qryVendorRunningBalance> vlist = await _context.qryVendorRunningBalance.FromSqlRaw("exec SP_VendorRunningBalanceAsOf '"+ PayClassCode + "','"+ AsOfMode + "'").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<DateTime> GetMaxEditDate()
        {

            try
            {
                var result = await Task.FromResult(_context.Set<ValReturn<DateTime>>()
                 .FromSqlRaw("exec SP_GetMaxEditDate")
                 .AsEnumerable()
                 .FirstOrDefault().Value);
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<int> DeleteRequisition(string reqNo)
        {
            try
            {
               
                var result = await Task.Run(() =>  _context.Database.ExecuteSqlRaw("exec sp_DeleteRequisition", reqNo));
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<int> CancelRequisition(string reqNo,string editUser)
        {
            try
            {
                var parameter = new List<SqlParameter>();
                parameter.Add(new SqlParameter("@ReqNo", reqNo));
                parameter.Add(new SqlParameter("@PersonID", editUser));
                var result = await Task.Run(() => _context.Database.ExecuteSqlRaw("exec SP_CancelRequisition",parameter.ToArray()));
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<string>> GetReqNoListByMainReqNo(string MainReqNo)
        {
            try
            {
                var result = await _context.TblRequisitionhdr.Where(a => a.MainReqNo.Equals(MainReqNo)).Select(a => a.Reqno).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<string>> CheckVoidedReq(string reqno)
        {
            try
            {
                var result = await _context.TblRequisitionhdr.Where(a => a.MainReqNo.Equals(reqno) && a.Void).Select(a => a.Reqno).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        } 
        public async Task<List<string>> GetActiveReq(List<string> SInoList)
        {
            try
            {
                var vlist = await _context.qryActiveRequisition.Where(a => SInoList.Contains(a.SalesInvoiceNo)).Select( a => a.SalesInvoiceNo).ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<qryRequisitionDepartment>> GetRequisitionDepartment(string personid)
        {

            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<qryRequisitionDepartment> vlist = await _context.qryRequisitionDepartment.FromSqlRaw("exec sp_GetRequisitionChapels '" + personid + "'  ").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<TblResponse> UpdateDetailsByReqNo(qryUpdateReqDetails qryUpdateReqDetails)
        {
            try
            {
                await Task.FromResult(_context.Database.ExecuteSqlRaw("exec sp_UpdateReqDeductions '" + qryUpdateReqDetails.ReqNo + "','" + qryUpdateReqDetails.Deduction + "','" + qryUpdateReqDetails.Freight + "','" + qryUpdateReqDetails.SINo +"','" + qryUpdateReqDetails.SIDate +"','" + qryUpdateReqDetails.UserCode +"'"));
                _response = new TblResponse
                {
                    Status = "SUCCESS",
                    AuditDate = DateTime.Now,
                    ErrorMessage = "SUCCESS",
                    MethodName = "UPDATE REQ DETAILS",
                    TrxNo = qryUpdateReqDetails.ReqNo,
                    UniqueInfo = "1"
                };

                return await Task.FromResult(_response);
            }
            catch (Exception e)
            {
                _response = new TblResponse
                {
                    Status = "FAILED",
                    AuditDate = DateTime.Now,
                    ErrorMessage = "ERROR: " + e.Message,
                    MethodName = "UPDATE REQ DETAILS",
                    TrxNo = qryUpdateReqDetails.ReqNo,
                    UniqueInfo = "1"
                };

                return _response;
            }
        }

        public async Task<decimal> GetTotalAmount(string ReqNo)
        {
            try
            { 
                return await _context.TblRequisitionhdr.Where(a => a.Reqno.Equals(ReqNo)).Select(a => a.TotalAmount).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message);
            }
        }

        #endregion

    }
}

