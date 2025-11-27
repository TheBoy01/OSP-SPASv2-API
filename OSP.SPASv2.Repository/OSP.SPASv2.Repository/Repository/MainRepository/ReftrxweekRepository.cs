using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using SPASv2.Context;
using SPASv2.Repository.Repository;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class ReftrxweekRepository : IReftrxweekRepository<RefTrxweek>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefTrxweek> _AbstractRepository;
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
        public ReftrxweekRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefTrxweek>(_context);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods



        #endregion
        #region Public Functions
        public async Task<RefTrxweek> GetReftrxweek(DateTime auditdate)
        {
            try
            {
                //var vlist = await _context.RefTrxweek.FromSqlRaw("select * from reftrxweek where '" + Convert.ToDateTime(auditdate) + "' between startdate and enddate").FirstOrDefaultAsync();
                
                //var vlist = await _context.RefTrxweek.Where(a => a.EndDate <= auditdate && a.StartDate >= auditdate).FirstOrDefaultAsync();

                var vlist = await _context.RefTrxweek.Where(a => auditdate <=  a.EndDate   && auditdate >= a.StartDate ).FirstOrDefaultAsync();

                //var vlist = await _context.RefTrxweek.FromSqlRaw("  select  TRY_CAST('"+ auditdate + "' as DATETIME) as asd").FirstOrDefaultAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

    }
}

