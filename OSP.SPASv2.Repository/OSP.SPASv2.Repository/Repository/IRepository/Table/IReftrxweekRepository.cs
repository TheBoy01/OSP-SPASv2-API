using OSP.SPASv2.Domain.Tables;

namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IReftrxweekRepository<TEntity> where TEntity : class
    {
        public Task<RefTrxweek> GetReftrxweek(DateTime auditdate);
    }
}
