namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblRequisitionDtlSummaryRepository<TEntity> where TEntity : class
    {

        public Task<TblResponse> Create(string ReqNo, string AuditUser);

    }
}
