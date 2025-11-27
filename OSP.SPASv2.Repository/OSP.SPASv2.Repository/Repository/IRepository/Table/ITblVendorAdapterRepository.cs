namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblVendorAdapterRepository<TEntity> where TEntity : class
    {

        public Task<string> GetVendorID(string VendorCode, string CompanyCode);

    }
}
