namespace OSP.SPASv2.Repository.IRepository
{
    public interface IGetAll<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> GetAllObjects();
     
    }
}
