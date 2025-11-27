using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using OSP.Common.Domain.View;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class RefPaymentClassRepository : IRefpaymentClassRepository<RefPaymentClass>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefPaymentClass> _AbstractRepository;
        StringBuilder sb;
        //VendorbankaccountRules vrules = new VendorbankaccountRules();

        #endregion 

        #region Constructors
        public RefPaymentClassRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefPaymentClass>(_context);
        } 
        #endregion


        public async Task<string> GetGetPayclassCodeByDesc(string PayClassDesc)
        {
            try
            {
                //string vlist = await _context.RefCompany.FromSqlRaw("select companycode from RefCompany where active=1 and companydesc like '"+ company + "'").FirstOrDefaultAsync();
                var desc = await Task.FromResult(_context.RefPaymentClass.Where(p => p.PayDesc.Equals(PayClassDesc))
                                                                   .Select(p => p.PayClassCode).FirstOrDefault()); 
                return desc;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<RefPaymentClass> Read(string PayClassCode)
        {
            try
            {
                var vlist = await _context.RefPaymentClass.Where(a => a.PayClassCode==PayClassCode).FirstOrDefaultAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
