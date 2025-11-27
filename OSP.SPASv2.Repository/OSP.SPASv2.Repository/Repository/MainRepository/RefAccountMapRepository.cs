using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using DocumentFormat.OpenXml.VariantTypes;
using OSP.Common.Domain.View;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class RefAccountMapRepository : IRefAccountMapRepository<RefAccountMap>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefAccountMap> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;
        //PaymentrequesthdrRules vrules = new PaymentrequesthdrRules();

        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public RefAccountMapRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefAccountMap>(_context);
        }

        #endregion

        public async Task<List<RefAccountMap>> GetAccountMapList(bool IsVatable)
        {
            try
            {
                return await _context.RefAccountMap.OrderBy(a => a.Hierarchy).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        } 

        public async Task<decimal> ComputeDtlEntries(string referenceNo, string formula, decimal vAT)
        {
            try
            {
                return await Task.FromResult(_context.Set<ValReturn<decimal>>()
                 .FromSqlRaw("sp_ComputeDtlEntries '" + referenceNo +"','" + formula + "'," + vAT +"").AsEnumerable()
               .First().Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
