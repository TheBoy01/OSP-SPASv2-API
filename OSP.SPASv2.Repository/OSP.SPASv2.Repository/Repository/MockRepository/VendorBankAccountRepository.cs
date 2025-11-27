using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Repository.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository.MockRepository
{
    public class VendorBankAccountRepository : IVendorBankAccountRepository<TblVendorpaymethod>
    {
        public Task<IList<TblVendorpaymethod>> GetVendorAcctNo(string vendorcode)
        {
            throw new NotImplementedException();
        }

        public Task<TblVendorpaymethod> GetVendorAcctNo1(string vendorcode)
        {
            throw new NotImplementedException();
        }

        public Task<IList<TblVendorpaymethod>> GetVendorAccttype()
        {
            throw new NotImplementedException();
        }
    }
}
