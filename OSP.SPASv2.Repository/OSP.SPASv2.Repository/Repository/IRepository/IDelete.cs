namespace OSP.SPASv2.Repository.IRepository
{
    public interface IDelete<TEntity> where TEntity : class
    {
        public void Delete(object id);
    }
}
