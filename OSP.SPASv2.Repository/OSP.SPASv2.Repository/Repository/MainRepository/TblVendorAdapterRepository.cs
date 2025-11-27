using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class TblVendorAdapterRepository : ITblVendorAdapterRepository<TblVendorAdapter>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblVendorAdapter> _AbstractRepository;
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
        public TblVendorAdapterRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblVendorAdapter>(_context);
        }

        public async Task<string> GetVendorID(string VendorCode, string CompanyCode)
        {
            try
            {
                return await _context.TblVendorAdapter.Where(a => a.CompanyCode.Equals(CompanyCode) && a.VendorCode.Equals(VendorCode) && a.Active).Select(a => a.VendorID).FirstOrDefaultAsync();
               
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion



    }
}
