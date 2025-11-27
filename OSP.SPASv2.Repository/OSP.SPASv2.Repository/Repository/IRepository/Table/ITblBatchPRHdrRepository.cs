namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblBatchPRHdrRepository<TEntity> where TEntity : class
    {  
        public Task<TblResponse> CreateBatchHdr(TEntity entity);

    }
}
