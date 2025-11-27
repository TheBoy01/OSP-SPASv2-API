using OSP.SPASv2.Domain;
using OSP.SPASv2.Domain.Tables;

namespace OSP.SPASv2.Repository.IRepository.Table
{
    public interface IResponseRepository<TEntity> where TEntity : class
    {
        public Task CreateResponse(TEntity entity);
    }
}
