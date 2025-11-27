namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefRegionRepository<TEntity> where TEntity : class
    { 
        public Task<IList<RefRegion>> GetRegionList();

    }
}
