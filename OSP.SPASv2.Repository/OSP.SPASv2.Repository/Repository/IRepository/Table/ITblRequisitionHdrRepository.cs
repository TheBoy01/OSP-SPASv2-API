

using OSP.SPASv2.Domain.Tables;

namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblRequisitionHdrRepository<TEntity> where TEntity : class
    {
        public Task<TblResponse> CreateTblRequisitionHdr(TEntity entity);

        public Task<TblRequisitionhdr> ReadRequisitionHdr(string ReqNo);

        public Task<qryRequisitionInfo> GetRequisitionInfo(string ReqNo);

        public Task<IList<qryRequisitionItem>> GetRequisitionItemList(string ReqNo);

        public Task<TblResponse> BulkInsert(List<TEntity> entity);
    }
}
