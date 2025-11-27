using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Repository.Repository.MainRepository;
using Microsoft.AspNetCore.Mvc;
using OSP.SPASv2.Domain;
using OSP.Common.Domain.References;
using OSP.SPASv2.Repository.IRepository.Table;
using OSP.SPASv2.Domain.References;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class RefATCRepository : IRefATCRepository<RefATC>
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefATC> _AbstractRepository;
        StringBuilder sb;
        //        VendorRules vrules = new VendorRules();

        #endregion

        #region Constructors
        public RefATCRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefATC>(_context);
        }



        public async Task<IList<RefATC>> GetATCList()
        {
            try
            {

                IList<RefATC> vlist = await _context.RefATC.FromSqlRaw("select * from RefATC").ToListAsync();
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
