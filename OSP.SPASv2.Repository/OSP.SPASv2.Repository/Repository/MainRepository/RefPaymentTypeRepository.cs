using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using SPASv2.Context;
using SPASv2.Repository.Repository;
using System.Text;

using Microsoft.EntityFrameworkCore;
using System;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class RefPaymentTypeRepository : IRefPaymentTypeRepository<RefPaymentClass>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefPaymentClass> _AbstractRepository;
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
        public RefPaymentTypeRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefPaymentClass>(_context);
        }




        #endregion

        #region Public Properties

        #endregion

        #region Public Methods



        #endregion

        #region Public Functions



        public async Task<IList<RefPaymentClass>> GetPaymentTypes(string paymenttype)
        {
            try
            {
                IList<RefPaymentClass> vlist = await _context.RefPaymentClass.FromSqlRaw("select * from refpaymenttype where paydesc like '%" + paymenttype + "%'").ToListAsync();
                //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
                //                                                   .ToList();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<IList<RefPaymentClass>> GetPaymentTypeList()
        {
            try
            {
                IList<RefPaymentClass> vlist = await _context.RefPaymentClass.FromSqlRaw("select * from refpaymentClass ").ToListAsync();
                //IList<RefCompany> vlist = await _context.RefCompany.Where(p=>p.CompanyDesc.Contains(company))
                //                                                   .ToList();
               return await _context.RefPaymentClass.Where(s => s.Active == true).ToListAsync();
                //return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetClassIDDescByCode(string payClassCode)
        {
            try
            {
                return await _context.RefPaymentClass.Where(a => a.PayClassCode == payClassCode).Select(a => a.PayDesc).FirstOrDefaultAsync() ;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<RefPaymentClass> GetPaymentClass(string payClassCode)
        {
            try
            {
                return await _context.RefPaymentClass.Where(a => a.PayClassCode == payClassCode).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}