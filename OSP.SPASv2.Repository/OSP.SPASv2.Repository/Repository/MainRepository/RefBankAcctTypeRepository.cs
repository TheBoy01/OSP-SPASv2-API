using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Domain.References;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using SPASv2.Context;
using SPASv2.Repository.Repository;
using System.Text;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class RefBankAcctTypeRepository : IRefBankAcctTypeRepository<RefBankAcctType>
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefBankAcctType> _AbstractRepository;
        StringBuilder sb;
        //        VendorRules vrules = new VendorRules();

        #endregion
        public RefBankAcctTypeRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefBankAcctType>(_context);
        }
        public async Task<IList<RefBankAcctType>> GetBankAcctTypeList()
        {
            try
            {

                IList<RefBankAcctType> vlist = await _context.RefBankAcctType.FromSqlRaw("select * from RefBankAcctType").ToListAsync();
                //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
                //                                                   .ToList();

                return vlist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
