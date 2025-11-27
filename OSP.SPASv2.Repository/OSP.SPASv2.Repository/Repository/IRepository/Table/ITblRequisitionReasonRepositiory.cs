namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblRequisitionReasonRepositioryy<TEntity> where TEntity : class
    {
        public Task<TblResponse> Create(TEntity entity);
    }
}
