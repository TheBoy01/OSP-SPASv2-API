using System.Linq.Expressions;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(object id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IList<TEntity>> GetAllIListAsync(Expression<Func<TEntity, bool>> filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
    Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> condition);
    void Insert(TEntity entity);
    void Update(TEntity entity);
    void Delete(object id);
    Task SaveAsync();
}
