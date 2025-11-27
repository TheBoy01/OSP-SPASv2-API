using Microsoft.EntityFrameworkCore;
using OSP.Common.Domain.View;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using SPASv2.Context;
using SPASv2.Repository.Repository;
using System.Text;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class TblRequisitionDtlRepository : ITblRequisitionDtlRepository<TblRequisitiondtl>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblRequisitiondtl> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        #endregion

        #region Constructors
        public TblRequisitionDtlRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblRequisitiondtl>(_context);
        }

        #endregion



        public async Task<TblResponse> CreateRequisitionDtl(TblRequisitiondtl entity)
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

        public async Task<TblRequisitiondtl> ReadRequisitionDtl(string ReqNo, string CompanyCode, string DeptCode, string ItemCode)
        {
            try
            {
                return await _context.TblRequisitiondtl.Where( a=> a.ReqNo==ReqNo && a.CompanyCode==CompanyCode && a.DeptCode==DeptCode && a.ItemCode==ItemCode).FirstOrDefaultAsync();
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
        public async Task<List<TblRequisitiondtl>> Read(string ReqNo )
        {
            try
            {
                var vlist = await _context.TblRequisitiondtl.FromSqlRaw("select * from tblrequisitiondtl where reqno='"+ ReqNo +"'; ").ToListAsync(); 

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<string> ReadCompanyCodeReqDtl(string ReqNo)
        {
            try
            {
                var result = await Task.FromResult(_context.Set<ValReturn<string>>()
                .FromSqlRaw("select top 1 CompanyCode as Value from tblrequisitiondtl where Reqno = '" + ReqNo + "'")
                .AsEnumerable()
                .First().Value);
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<TblResponse> BulkInsert(List<TblRequisitiondtl> entity)
        {
            await _AbstractRepository.BulkInsert(entity);
            return await Task.FromResult(_response);
        }

        public async Task<int> SumPOQuantity(string mainReqNo, string companyCode, string department, string itemCode)
        {
            try
            {
                //return await Task.FromResult(_context.TblRequisitiondtl.Where(a => _context.TblRequisitionhdr.Where(a => a.MainReqNo.Equals(mainReqNo) && a.Active && !a.Void && a.VoidUser.Equals(string.Empty)).Select(a => a.Reqno).ToList().Contains(a.ReqNo)
                //                                                              && a.CompanyCode.ToUpper().Equals(department.ToUpper()) && a.DeptCode.ToUpper().Equals(department.ToUpper()) && a.ItemCode.ToUpper().Equals(itemCode.ToUpper())).ToList().Count());
                var result = await Task.FromResult(_context.Set<ValReturn<int>>()
               .FromSqlRaw("exec sp_GetItemTotalQty '" + mainReqNo + "', '" + companyCode +"','" + department +"','" + itemCode +"'")
               .AsEnumerable()
               .First().Value);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<decimal> GetItemPriceByPOItemCode(string PONo, string ItemCode)
        {
            var result = await Task.FromResult(_context.Set<ValReturn<decimal>>()
              .FromSqlRaw("select a.Price as Value From TblRequisitionDtl a inner join TblPurchaseOrderHdr b on a.ReqNo=b.Reqno where b.PONo='" + PONo + "' and ItemCode='" + ItemCode+ "'")
              .AsEnumerable()
              .First().Value);
            return result;
        }
    }
}
