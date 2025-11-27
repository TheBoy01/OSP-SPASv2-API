using OSP.SPASv2.Domain.Tables;

namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IVendorBankAccountRepository<TEntity> where TEntity : class
    {
        public Task<IList<TblVendorpaymethod>> GetVendorAccttype();

        public Task<IList<TblVendorpaymethod>> GetVendorAcctNo(string vendorcode);
        public Task<TblVendorpaymethod> GetVendorAcctNo1(string vendorcode);
    }
}
