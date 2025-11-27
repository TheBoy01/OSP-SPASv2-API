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
    public class RefChapelBranchRepository : IRefChapelBranchRepository<RefChapelBranch>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefChapelBranch> _AbstractRepository;
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
        public RefChapelBranchRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefChapelBranch>(_context);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods



        #endregion

        #region Public Functions
         
        public async Task<List<RefChapelBranch>> GetRefChapelBranches()
        {
            try
            {
                List<RefChapelBranch> vlist = await _context.RefChapelBranch.FromSqlRaw("select * from osp.dbo.RefChapelBranch").ToListAsync();
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
