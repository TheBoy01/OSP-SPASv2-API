namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefAccountMapRepository<TEntity> where TEntity : class
    {
        public Task<List<RefAccountMap>> GetAccountMapList(bool IsVatable);

    }
}
