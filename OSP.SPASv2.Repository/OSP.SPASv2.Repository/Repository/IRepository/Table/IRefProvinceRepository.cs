namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefProvinceRepository<TEntity> where TEntity : class
    {
        public Task<IList<RefProvince>> GetProvinceList();
    }
}
