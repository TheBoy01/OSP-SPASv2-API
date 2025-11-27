using OSP.SPASv2.Domain.References;

namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefATCTypeRepository<TEntity> where TEntity : class
    {
        public Task<IList<RefATCType>> GetATCTypeList();

    }
}
