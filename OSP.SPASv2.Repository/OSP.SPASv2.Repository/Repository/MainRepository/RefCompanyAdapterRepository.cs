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
    public class RefCompanyAdapterRepository : IRefCompanyAdapterRepository<RefCompanyAdapter>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefCompanyAdapter> _AbstractRepository;
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
        public RefCompanyAdapterRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefCompanyAdapter>(_context);
        }

        #endregion

        public async Task<string> GetSPASv1CompCode(string SPASv2CompanyCode)
        {
            try
            {
                return await _context.RefCompanyAdapter.Where(a => a.CompanyCode.Equals(SPASv2CompanyCode)).Select(a => a.CompanyCodev1).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
