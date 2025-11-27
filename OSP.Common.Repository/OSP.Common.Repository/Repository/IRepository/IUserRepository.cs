using OSP.Common.Domain;

namespace OSP.Common.Repository.IRepository
{
    public interface IUserRepository<TEntity> where TEntity : class
    {
        public Task<TblUser> GetUserDetails(TEntity entity);
    }
}
