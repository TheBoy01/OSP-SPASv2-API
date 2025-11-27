namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefCityRepository<TEntity> where TEntity : class
    {
        public Task<IList<RefCity>> GetCityList();
    }
}
