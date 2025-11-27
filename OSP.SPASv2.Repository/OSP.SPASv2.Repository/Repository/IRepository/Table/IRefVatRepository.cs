using OSP.SPASv2.Domain.Tables;

namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefVatRepository<TEntity> where TEntity : class
    {
        public Task<RefVat> GetRefVat();
    }
}

