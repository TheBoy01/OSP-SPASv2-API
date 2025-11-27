using OSP.SPASv2.Domain.References;

namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefATCRepository<TEntity> where TEntity : class
    {
        public Task<IList<RefATC>> GetATCList();

    }
}
