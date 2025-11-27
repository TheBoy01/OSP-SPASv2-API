using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using SPASv2.Context;
using SPASv2.Repository.Repository;
using System.Text;

namespace OSP.SPASv2.Repository.Repository.MockRepository
{
    public class RefDiscountRepository : IRefDiscountRepository<RefDiscount>
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefDiscount> _AbstractRepository;
        StringBuilder sb;
       

        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public RefDiscountRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefDiscount>(_context);
        }

        public Task<IList<RefDiscount>> GetRefDiscount()
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
