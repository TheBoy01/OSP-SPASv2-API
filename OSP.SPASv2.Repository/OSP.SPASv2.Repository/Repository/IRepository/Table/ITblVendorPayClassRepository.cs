namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblVendorPayClassRepository<TEntity> where TEntity : class
    {

        public Task<TblVendorPayClass> ReadPayClass(string VendorCode, string PayclassCode);

    }
}
