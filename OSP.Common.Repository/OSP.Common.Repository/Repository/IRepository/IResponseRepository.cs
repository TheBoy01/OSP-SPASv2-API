using OSP.Common.Domain;
using OSP.Common.Domain.Tables;

namespace OSP.Common.Repository.IRepository.Table
{
    public interface IResponseRepository<TEntity> where TEntity : class
    {
        public Task CreateResponse(TEntity entity);
    }
}
