using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using static SPASv2.Context.SPASv2Context;
using OSP.Common.Domain.View;

namespace OSP.SPASv2.Repository.Repository
{
    public class VendorbankaccountRepository : IVendorBankAccountRepository<TblVendorpaymethod>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblVendorpaymethod> _AbstractRepository;
        StringBuilder sb;
        //VendorbankaccountRules vrules = new VendorbankaccountRules();

        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public VendorbankaccountRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblVendorpaymethod>(_context);
        }


        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        //public async Task<RepositoryResponse> Create(TblVendorpaymethod entity)
        //{
        //    RepositoryResponse _r = new RepositoryResponse();
        //    if (vrules.CanCreate(entity) == false)
        //    {
        //        _r = new RepositoryResponse()
        //        {
        //            StatusCode = " ",
        //            StatusDesc = " "
        //        };
        //    }
        //    else
        //    {
        //        _r = new RepositoryResponse()
        //        {
        //            StatusCode = " ",
        //            StatusDesc = " "
        //        };
        //    }
        //    this.CreateVendorbankaccount(entity);
        //    return await Task.FromResult(_r);
        //}
        #endregion

        #region Public Functions

        public async Task<IList<TblVendorpaymethod>> GetVendorAccttype()
        {
            try
            {
                var vlist = await _context.TblVendorpaymethod.FromSqlRaw("select * from TblVendorpaymethod where active=1").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<IList<TblVendorpaymethod>> GetVendorAcctNo(string vendorcode)
        {
            try
            {
                var vlist = await _context.TblVendorpaymethod.FromSqlRaw("select * from tblvendorpaymethod where isdefault=1 and active=1 and vendorcode='" + vendorcode + "'").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<TblVendorpaymethod> GetVendorAcctNo1(string vendorcode)
        {
            try
            {
                var vlist = await _context.TblVendorpaymethod.FromSqlRaw("select * from tblvendorpaymethod where isdefault=1 and active=1 and vendorcode='" + vendorcode + "'").FirstOrDefaultAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<string> Get_VendorDefaultBankCode(string vendorcode)
        {
            try
            {
               return  await Task.FromResult(_context.Set<ValReturn<string>>()
                 .FromSqlRaw("select top 1 isnull(BankCode,'') as Value from TblVendorPayMethod where VendorCode = '" + vendorcode + "' and IsDefault=1").AsEnumerable()
               .First().Value);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<string> Get_VendorDefaultAcctNo(string vendorcode)
        {
            try
            {
                return await Task.FromResult(_context.Set<ValReturn<string>>()
               .FromSqlRaw("select top 1 isnull(AcctNo,'') as Value  from TblVendorPayMethod where VendorCode = '" + vendorcode + "' and IsDefault=1")
               .AsEnumerable()
               .First().Value);
                //return result;

                //return await Task.FromResult(_context.Set<ValReturn<string>>()
                //  .FromSqlRaw("select top 1 isnull(AcctNo,'')  from TblVendorPayMethod where VendorCode = '" + vendorcode + "' and IsDefault=1")
                //  .AsEnumerable()
                //  .First().ToString());

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

    }
}

