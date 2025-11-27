using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.EntityFrameworkCore;
using OSP.Common.Domain.References;
using OSP.SPASv2.Domain.References;
using OSP.SPASv2.Repository.IRepository.Table;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using SPASv2.Context;
using SPASv2.Repository.Repository;
using System.Text;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class RefChapelEmailRepository : IRefChapelEmailRepository<RefChapelEmail>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefChapelEmail> _AbstractRepository;
        StringBuilder sb;
        //VendorRules vrules = new VendorRules();

        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public RefChapelEmailRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefChapelEmail>(_context);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods 

        public async Task<List<RefChapelEmail>> GetRefChapelEmailList()
        {
            try
            {
                
                List<RefChapelEmail> vlist = await _context.RefChapelEmail.FromSqlRaw("select * from osp.dbo.RefChapelEmail").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #endregion

        #region Public Functions

        #endregion
    }
}
