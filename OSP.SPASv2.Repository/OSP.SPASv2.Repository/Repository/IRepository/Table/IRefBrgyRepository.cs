namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefBrgyRepository<TEntity> where TEntity : class
    {
        public Task<IList<RefBrgy>> GetBrgyList();
    }
}
