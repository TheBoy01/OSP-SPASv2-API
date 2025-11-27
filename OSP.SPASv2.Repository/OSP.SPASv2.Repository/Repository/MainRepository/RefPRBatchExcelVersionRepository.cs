using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using SPASv2.Context;
using SPASv2.Repository.Repository;
using System.Text;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Domain.References.OSP.SPASv2.Domain.Tables;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class RefPRBatchExcelVersionRepository : IRefPRBatchExcelVersionRepository<RefPrbatchexcelversion>
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefPrbatchexcelversion> _AbstractRepository;
        StringBuilder sb;
        //        VendorRules vrules = new VendorRules();

        #endregion

        #region Constructors
        public RefPRBatchExcelVersionRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefPrbatchexcelversion>(_context);
        }

        public Task<RefPrbatchexcelversion> CheckBatchVersion()
        {
            throw new NotImplementedException();
        }
        #endregion
        //public Task<RefPrbatchexcelversion> CheckBatchVersion()
        //{
        //    try
        //    {
        //        RefPrbatchexcelversion vlist = await _context.RefATC.FromSqlRaw("select * from RefATC").ToListAsync();
        //        //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
        //        //                                                   .ToList();

        //        return vlist;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
