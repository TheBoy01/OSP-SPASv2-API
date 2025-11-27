
using OSP.SPASv2.Domain.Tables;
namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblAssignedtoVendor_CMSRepository<TEntity> where TEntity : class
    {

        public Task<TblResponse> BulkInsert(List<TEntity> entity);
        public Task<TblAssignedtoVendor_CMS> ReadByReqNo(string ReqNo);

    }
}
