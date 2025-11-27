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

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class RefVendorTypeRepository : IRefVendorTypeRepository<RefVendorType>
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefVendorType> _AbstractRepository;
        StringBuilder sb;
        //        VendorRules vrules = new VendorRules();

        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public RefVendorTypeRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefVendorType>(_context);
        }

        public async Task<IList<RefVendorType>> GetAllObjects()
        {
            try
            {

                IList<RefVendorType> vlist = await _context.RefVendorType.FromSqlRaw("select * from RefVendorType").ToListAsync();
                //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
                //                                                   .ToList();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<IList<RefVendorType>> GetVendorTypeList()
        {
            try
            {

                IList<RefVendorType> vlist = await _context.RefVendorType.FromSqlRaw("select * from RefVendorType").ToListAsync();
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

        #region Public Properties

        #endregion

        #region Public Methods



        #endregion

        #region Public Functions


        #endregion
    }
}
