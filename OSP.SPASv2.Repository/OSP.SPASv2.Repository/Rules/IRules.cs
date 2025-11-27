namespace OSP.SPASv2.Repository.Rules
{
    public interface IRules<TEntity> where TEntity : class
    {
        string CanCreate(TEntity entity);

        string CanRead(TEntity entity);

        string CanUpdate(TEntity entity);

        string CanDelete(TEntity entity);

        public  Task<string> CanReadAsync(TEntity entity);

        public Task<string> CanDeleteAsync(TEntity entity);
    }
}
