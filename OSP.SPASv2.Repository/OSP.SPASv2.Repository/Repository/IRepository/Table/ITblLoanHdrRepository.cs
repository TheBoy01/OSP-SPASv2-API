
namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblLoanHdrRepository<TEntity> where TEntity : class
    {
        public Task<TblResponse> CreateLoanHdr(TEntity entity);
    }
}
