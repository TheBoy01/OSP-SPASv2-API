namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefVendorDocsRepository<TEntity> where TEntity : class
    {

        public Task<TblResponse> CreatePaymentRequest(TEntity entity);

        public Task<IList<RefVendorDocs>> GetVendorDocsList();

    }
}
