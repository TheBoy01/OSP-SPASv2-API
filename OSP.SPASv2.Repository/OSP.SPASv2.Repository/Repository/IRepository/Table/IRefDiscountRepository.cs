using OSP.SPASv2.Domain.Tables;

namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefDiscountRepository<TEntity> where TEntity : class
    {
        public Task<IList<RefDiscount>> GetRefDiscount();
    }
}
