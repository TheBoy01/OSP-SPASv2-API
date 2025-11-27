using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using OSP.SPASv2.Domain.View;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.Data.SqlClient;
using static SPASv2.Context.SPASv2Context;
using System.Linq;
using OSP.Common.Domain.View;
using System.Collections.Immutable;
using Newtonsoft.Json.Linq;
using DocumentFormat.OpenXml.Spreadsheet;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class BatchApprovalRepository : ITblBatchApprovalRepository<TblBatchApproval>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblBatchApproval> _AbstractRepository;
        GenericService<TblBatchApproval> _Genericservice;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        #endregion
        public BatchApprovalRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblBatchApproval>(_context);
        }
        public async Task<TblResponse> CreateTblBatchApproval(TblBatchApproval entity)
        {
            await _AbstractRepository.Insert(entity);
            return await Task.FromResult(_response);
        }
        public async Task<string> GetLastestBANo(string reqtype, string payclasscode)
        {
            var result = await Task.FromResult(_context.Set<ValReturn<string>>()
                 .FromSqlRaw("sp_GenerateBANo '" + reqtype + "','" + payclasscode + "'")
                 .AsEnumerable()
                 .First().Value);
            return result;
        }

        public async Task<List<string>> GetReqnoListByBano(string bano)
        {
            try
            {
                var list = await _context.TblBatchApproval.Where(a => a.BANo.Equals(bano)).Select(a => a.ReqNo).ToListAsync();
                return list;
            
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GetBatchNoByReqNo(string ReqNo)
        {
            try
            {
                return _context.TblBatchApproval.Where(a => a.ReqNo.Equals(ReqNo)).Select(a => a.BANo).FirstOrDefault();
            }
            catch (Exception ex)
            { 
                throw;
            }
        }


    }
}
