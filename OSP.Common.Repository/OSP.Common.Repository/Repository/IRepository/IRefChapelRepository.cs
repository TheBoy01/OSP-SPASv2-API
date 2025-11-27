using OSP.Common.Domain.References;

namespace OSP.Common.Repository.Repository.IRepository.Table
{
    public interface IRefChapelRepository<TEntity> where TEntity : class
    {
        public  Task<IList<RefChapel>> GetChapelsByPersonID(string personid);
    }
}
