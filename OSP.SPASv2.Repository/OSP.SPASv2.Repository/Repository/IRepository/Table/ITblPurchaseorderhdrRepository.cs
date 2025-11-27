namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblPurchaseorderhdrRepository<TEntity> where TEntity : class
    {
        public Task<TblPurchaseorderhdr> GetPOHdrByPONo(string PONo);
        public Task<TblResponse> CreateTblPurchaseOrderHdr(TEntity entity);

    }
}
