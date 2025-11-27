namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblItemBarcodesRepository<TEntity> where TEntity : class
    { 
        public Task<TblItemBarcodes> GetLatestBarCode(TblVendor VendorCode, string ItemCode);
    }
}
