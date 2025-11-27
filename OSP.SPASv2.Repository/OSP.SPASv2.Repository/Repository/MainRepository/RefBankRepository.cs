using Microsoft.EntityFrameworkCore;
using OSP.Common.Domain.References;
using OSP.SPASv2.Domain.References;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using SPASv2.Context;
using SPASv2.Repository.Repository;
using System.Text;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class RefBankRepository : IRefBankRepository<RefBank>
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefBank> _AbstractRepository;
        StringBuilder sb;
        //        VendorRules vrules = new VendorRules();

        #endregion

        #region Constructors
        public RefBankRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefBank>(_context);
        }
        public async Task<IList<RefBank>> GetBankList()
        {
            try
            {
                IList<RefBank> vlist = await _context.RefBank.FromSqlRaw("select * from RefBank where Active=1").ToListAsync();
                //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
                //                                                   .ToList();
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
