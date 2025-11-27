using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using SPASv2.Context;
using SPASv2.Repository.Repository;
using System.Text;

namespace OSP.SPASv2.Repository.Repository.MockRepository
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

        public Task<RefTrxweek> GetReftrxweek(DateTime auditdate)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods



        #endregion
        #region Public Functions


        #endregion

    }
}

