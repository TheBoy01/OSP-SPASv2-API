using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository
{
    public class PaymentrequisitionhdrRepository : IPaymentrequisitionhdrRepository<TblPaymentrequisitionhdr>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblPaymentrequisitionhdr> _AbstractRepository;
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
        public PaymentrequisitionhdrRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblPaymentrequisitionhdr>(_context);
        }

        public async Task<TblResponse> CreateTblPaymentrequisitionhdr(TblPaymentrequisitionhdr entity)
        {
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
        }

        public async Task<TblPaymentrequisitionhdr> GetTblPaymentrequisitionhdrBy(string ReqNo)
        {
            try
            {
                return await _context.TblPaymentrequisitionhdr.Where(a => a.Reqno.ToUpper().Equals(ReqNo.ToUpper())).FirstOrDefaultAsync();
                //var vlist = await _context.TblRequisitiondtl.FromSqlRaw("select * from TblRequisitiondtl where ReqNo='" + ReqNo + "' AND CompanyCode='" + CompanyCode +"' AND DeptCode='" + DeptCode +"' and ItemCode='" + ItemCode +"'").FirstOrDefaultAsync();
                //         var vlist = await _context.TblPaymentrequesthdr.OrderByDescending(n=>n.AuditDate).FirstOrDefaultAsync(n=>n.CompanyCode== companycode && n.DeptCode==branchcode);

                //vlist = new TblPaymentrequesthdr();
                //return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<string>> CheckSIByReqNoList(List<string> salesInvoiceList)
        {
            try
            {
                //var a =  await _context.TblPaymentrequisitionhdr.FromSqlRaw("Select ReqNo From TblPaymentrequisitionhdr where ReqNo in ('" + string.Join("','", reqNoList) + "') and SalesInvoiceNo='" + salesInvoice +"'").Select(a => a.Reqno).ToListAsync();
                var a = await _context.TblPaymentrequisitionhdr.Where(p => salesInvoiceList.Contains(p.SalesInvoiceNo)).Select(a => a.SalesInvoiceNo).ToListAsync();
                //var a = await _context.TblRequisitionhdr.Join(_context.TblPaymentrequisitionhdr,
                //                                       ReqHdr => ReqHdr.Reqno,
                //                                       PaymentReq => PaymentReq.PRno,
                //                                       (ReqHdr, PaymentReq) => new { Hdr = ReqHdr, PayReq = PaymentReq })
                //                                       .Where(JoinTbl => salesInvoiceList.Contains(JoinTbl.PayReq.SalesInvoiceNo) && JoinTbl.Hdr.Void).ToListAsync();

                //return a.Select(JoinTbl => JoinTbl.PayReq.SalesInvoiceNo).ToList();
                return a;
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
                await Task.FromResult(_context.Database.ExecuteSqlRaw("Update TblPaymentRequisitionHdr set SalesInvoiceNo='" + qryUpdateReqDetails.SINo +"',SalesInvoiceDate='" + qryUpdateReqDetails.SIDate +"' where ReqNo='" + qryUpdateReqDetails.ReqNo +"'"));
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

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods


        #endregion

    }
}

