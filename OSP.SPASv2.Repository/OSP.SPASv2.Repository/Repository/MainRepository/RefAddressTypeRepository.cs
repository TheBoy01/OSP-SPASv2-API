using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using OSP.SPASv2.Repository.Rules;
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

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class RefAddressTypeRepository : IRefAddressTypeRepository<RefAddressType>
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefAddressType> _AbstractRepository;
        StringBuilder sb;
        //        VendorRules vrules = new VendorRules();

        #endregion

        #region Constructors
        public RefAddressTypeRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefAddressType>(_context);
        }

        public async Task<IList<RefAddressType>> GetAddressTypeList()
        {
            try
            { 
                IList<RefAddressType> vlist = await _context.RefAddressType.FromSqlRaw("select * from RefAddressType").ToListAsync();
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
