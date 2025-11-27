namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblVendorContactPersonRepository<TEntity> where TEntity : class
    {

        public Task<IList<qryVendorContact>> GetVendorContact(string _VendorCode);

    }
}
