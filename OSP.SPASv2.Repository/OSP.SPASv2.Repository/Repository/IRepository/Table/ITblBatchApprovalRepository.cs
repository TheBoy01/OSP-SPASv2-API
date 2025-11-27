namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblBatchApprovalRepository<TEntity> where TEntity : class
    {
        public Task<TblResponse> CreateTblBatchApproval(TEntity entity);
    }
}
