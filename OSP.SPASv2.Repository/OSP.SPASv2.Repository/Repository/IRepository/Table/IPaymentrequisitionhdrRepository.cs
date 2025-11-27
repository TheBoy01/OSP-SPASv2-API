namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IPaymentrequisitionhdrRepository<TEntity> where TEntity : class
    {
        public Task<TblResponse> CreateTblPaymentrequisitionhdr(TEntity entity);
    }
}