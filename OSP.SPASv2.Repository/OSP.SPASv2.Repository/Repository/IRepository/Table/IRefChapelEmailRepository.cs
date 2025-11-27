namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefChapelEmailRepository<TEntity> where TEntity : class
    {
        public Task<List<RefChapelEmail>> GetRefChapelEmailList();

    }
}
