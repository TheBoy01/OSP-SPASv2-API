using OSP.Common.Domain.References;

namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefChapelRepository<TEntity> where TEntity : class
    {
        public  Task<IList<RefChapel>> GetChapels(string branchdesc, string company);
    }
}
