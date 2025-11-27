using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Domain.View;
using OSP.SPASv2.Domain.View;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using OSP.Common.Domain.View;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class TblBatchPRHdrRepository : ITblBatchPRHdrRepository<TblBatchPRHdr>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblBatchPRHdr> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        #endregion

        public TblBatchPRHdrRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblBatchPRHdr>(_context);
        }

        public async Task<TblResponse> CreateBatchHdr(TblBatchPRHdr entity)
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

        public async Task<TblBatchPRHdr> GetlatestPRBatchNo()
        {
            try
            { 
                var vlist = await _context.TblBatchPRHdr.FromSqlRaw("select top 1 * from TblBatchPRHdr order by auditdate desc").FirstOrDefaultAsync();
                //         var vlist = await _context.TblPaymentrequesthdr.OrderByDescending(n=>n.AuditDate).FirstOrDefaultAsync(n=>n.CompanyCode== companycode && n.DeptCode==branchcode);

                //vlist = new TblPaymentrequesthdr();
                return vlist;


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        } 
        
    }
}
