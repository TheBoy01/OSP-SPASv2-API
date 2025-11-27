namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblPaymentRequestAuthRepository<TEntity> where TEntity : class
    {
        public Task<int> GetDeniedCount(string ReqNo);
    }
}
