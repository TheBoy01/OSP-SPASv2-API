using Microsoft.EntityFrameworkCore;
using SPASv2.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly SPASv2Context _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(SPASv2Context context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IList<TEntity>> GetAllIListAsync(Expression<Func<TEntity, bool>> filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync();
        //return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> condition)
    {
        return await _dbSet.Where(condition).ToListAsync();
    }

    public void Insert(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(object id)
    {
        var entity = _dbSet.Find(id);
        if (entity != null)
            _dbSet.Remove(entity);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}


public class GenericService<TEntity> where TEntity : class
{
    private readonly IGenericRepository<TEntity> _repository;

    public GenericService(IGenericRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public async Task<TEntity> GetByIdAsync(object id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> condition)
    {
        return await _repository.GetByConditionAsync(condition);
    }

    public void Insert(TEntity entity)
    {
        _repository.Insert(entity);
    }

    public void Update(TEntity entity)
    {
        _repository.Update(entity);
    }

    public void Delete(object id)
    {
        _repository.Delete(id);
    }

    public async Task SaveAsync()
    {
        await _repository.SaveAsync();
    }
}
