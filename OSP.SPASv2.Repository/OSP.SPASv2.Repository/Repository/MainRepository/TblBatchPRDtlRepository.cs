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
    public class TblBatchPRDtlRepository : ITblBatchPRDtlRepository<TblBatchPRDtl>
    { 
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblBatchPRDtl> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        #endregion

        public TblBatchPRDtlRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblBatchPRDtl>(_context);
        }

        public async Task<TblResponse> CreateBatchDtl(TblBatchPRDtl entity)
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

        public async Task<string> GetBatchNoByRefNo(string pRNo)
        {
            try
            {
                return await Task.FromResult(_context.Set<ValReturn<string>>()
                    .FromSqlRaw("Select isnull(BatchPRNo,'') as Value from TblBatchPRDtl where PRNo = '" + pRNo.ToUpper() + "'").AsEnumerable()
                  .First().Value);
            }
            catch (Exception ex)
            {
                return "";
                //throw new Exception(ex.Message);
            }
        }
    }
}
