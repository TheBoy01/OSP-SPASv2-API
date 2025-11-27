namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IPaymentRequestDtlRepository<TEntity> where TEntity : class
    {
        public Task<TblResponse> CreatePaymentRequest(TEntity entity);
    }
}
