using Microsoft.EntityFrameworkCore;
using OSP.Common.Domain.View;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using SPASv2.Context;
using SPASv2.Repository.Repository;
using System.Text;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class TblDRNoRepository : ITblDRNoRepository<TblDRNo>
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblDRNo> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        #endregion

        #region Constructors
        public TblDRNoRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblDRNo>(_context);
        }

        #endregion
        public async Task<TblResponse> CreateDRNo(TblDRNo entity)
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

        public async Task<List<string>> GetDRListByReqNo(string requisitionNo)
        {
            try
            {
                var list = await _context.TblDRNo.Where(a => a.ReqNo.Equals(requisitionNo)).Select(a => a.DRNo).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<TblResponse> UpdateDetailsByReqNo(string reqNo, string sINo)
        {
            try
            {
                await Task.FromResult(_context.Database.ExecuteSqlRaw("Update TblDRNo set SINo='" + sINo + "' where ReqNo='" + reqNo + "'"));
                _response = new TblResponse
                {
                    Status = "SUCCESS",
                    AuditDate = DateTime.Now,
                    ErrorMessage = "SUCCESS",
                    MethodName = "UPDATE DR DETAILS",
                    TrxNo = reqNo,
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
                    MethodName = "UPDATE DR DETAILS",
                    TrxNo = reqNo,
                    UniqueInfo = "1"
                };

                return _response;
            }
        }
    }
}
