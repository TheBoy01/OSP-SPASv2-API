using OSP.SPASv2.Domain.Tables;

namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IVendorItemsRepository<TEntity> where TEntity : class
    {
        public Task<IList<TblVendorItems>> GetVendorItems(string vendorcode,string  paymentclasscode);
        public Task<IList<TblVendorItems>> GetVendorItemsList(string vendorcode);
        public Task<TblVendorItems> GetVendorItemsDetails(string vendorcode, string itemcode);
    }
}
