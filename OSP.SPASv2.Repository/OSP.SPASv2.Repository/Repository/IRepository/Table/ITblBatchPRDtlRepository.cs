namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblBatchPRDtlRepository<TEntity> where TEntity : class
    { 
        public Task<TblResponse> CreateBatchDtl(TEntity entity);

    }
}
