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
    public class RefVendorDocsRepository : IRefVendorDocsRepository<RefVendorDocs>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefVendorDocs> _AbstractRepository;
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
        public RefVendorDocsRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefVendorDocs>(_context);
        }

        public Task<TblResponse> CreatePaymentRequest(RefVendorDocs entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<RefVendorDocs>> GetVendorDocsList()
        {
            try
            {

                IList<RefVendorDocs> vlist = await _context.RefVendorDocs.FromSqlRaw("Select * From RefVendorDocs").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }



        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        #endregion
    }
}
