using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using OSP.Common.Domain.View;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using SPASv2.Context;
using SPASv2.Repository.Repository;
using System.Text;
using static System.Data.Odbc.ODBC32;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class TblRequisitionDtlSummaryRepository : ITblRequisitionDtlSummaryRepository<TblRequisitionDtlSummary>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblRequisitionDtlSummary> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        #endregion

        #region Constructors
        public TblRequisitionDtlSummaryRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblRequisitionDtlSummary>(_context);
        }

        #endregion

        public async Task<TblResponse> Create(string ReqNo, string AuditUser)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("exec sp_CreateDtlSummary '" + ReqNo + "', '" + AuditUser + "'");
                return await Task.FromResult(_response); 
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message);
            }
        }

        public async Task<TblRequisitionDtlSummary> Read(string ReqNo)
        {
            try
            {
                return await _context.TblRequisitionDtlSummary.Where(a => a.ReqNo == ReqNo).FirstAsync();
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

        public async Task<List<TblRequisitionDtlSummary>> ReadList(string ReqNo)
        {
            try
            {
                return await _context.TblRequisitionDtlSummary.Where(a => a.ReqNo == ReqNo).ToListAsync();
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

    }
}
