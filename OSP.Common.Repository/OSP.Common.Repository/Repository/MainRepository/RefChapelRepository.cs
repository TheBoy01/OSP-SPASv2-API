using Common.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.Common.Domain.References;
using OSP.Common.Repository.Context;
using OSP.Common.Repository.Repository.IRepository.Table; 


using System.Text;

namespace OSP.Common.Repository.Repository.MainRepository
{
    public class RefChapelRepository : IRefChapelRepository<RefChapel>
    {
        #region Private Member Variables

        private OSPContext _context;
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
        public RefChapelRepository(OSPContext context)
        {
            _context = context;
            _context = DbContextFactory.Create("SPLPDEVSERVER");
            _AbstractRepository = new AbstractRepository<RefChapel>(_context);
        }



        #endregion

        #region Public Properties

        #endregion

        #region Public Methods



        #endregion

        #region Public Functions

       

        public async Task<IList<RefChapel>> GetChapelsByPersonID(string personid)
        {
            try
            {
                IList<RefChapel> vlist = await _context.RefChapel.FromSqlRaw("select a.* from refchapel a inner join refcompany b on a.CompanyCode=b.companycode and b.companytype='CHAPELS' and a.active=1 inner join tblpersonaccess c on b.CompanyType=c.CompanyType and a.chapelcode=c.deptcode where c.personid='" + personid + "' ").ToListAsync();
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
