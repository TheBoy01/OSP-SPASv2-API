using OSP.SPASv2.Domain;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Domain.View;

namespace OSP.SPASv2.Repository.IRepository
{
    public interface IPRAuthorizationRepository<TEntity> where TEntity : class
    {
        public Task<TblResponse> CreatePRAuthorization(TEntity entity);
    }
}
