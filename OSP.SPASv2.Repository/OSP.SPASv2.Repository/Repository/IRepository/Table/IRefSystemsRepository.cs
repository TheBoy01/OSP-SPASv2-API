namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefSystemsRepository<TEntity> where TEntity : class
    {
        public Task<RefSystems> GetRefSystems();
    }
}
