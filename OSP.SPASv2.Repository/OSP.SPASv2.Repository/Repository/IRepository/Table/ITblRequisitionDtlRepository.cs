namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblRequisitionDtlRepository<TEntity> where TEntity : class
    {
        public Task<TblResponse> CreateRequisitionDtl(TEntity entity);

        public Task<TblRequisitiondtl> ReadRequisitionDtl(string ReqNo, string CompanyCode, string DeptCode, string ItemCode);
         
        public Task<TblResponse> BulkInsert(List<TEntity> entity);
    }
}
