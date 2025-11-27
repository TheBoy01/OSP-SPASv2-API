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
    public class RefChapelRepository : IRefChapelRepository<RefChapel>
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefChapel> _AbstractRepository;
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
        public RefChapelRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefChapel>(_context);
        }



        #endregion

        #region Public Properties

        #endregion

        #region Public Methods



        #endregion

        #region Public Functions

        public async Task<List<RefChapel>> GetAllChapels()
        {
            try
            {
                List<RefChapel> vlist = await _context.RefChapel.FromSqlRaw("select * from osp.dbo.RefChapel").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<RefChapel>> GetChapels(string branchdesc, string company)
        {

            try
            {
                IList<RefChapel> vlist = await _context.RefChapel.FromSqlRaw("select * from RefChapel where companycode = '" + company + "' and chapeldesc like '%" + branchdesc + "%'").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<RefChapel> GetChapelsDetails(string chapelcode)
        {

            try
            {
                RefChapel vlist = await _context.RefChapel.FromSqlRaw("select * from osp.dbo.RefChapel where chapelcode = '" + chapelcode + "'").FirstOrDefaultAsync();
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
