namespace OSP.Common.Repository.Rules
{
    public interface IRules<TEntity> where TEntity : class
    {
        bool CanCreate(TEntity entity);

        bool CanRead(TEntity entity);

        bool CanUpdate(TEntity entity);

        bool CanDelete(TEntity entity);

        public Task<string> CanReadAsync(TEntity entity);

    }
}
