namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface IRefCompanyAdapterRepository<TEntity> where TEntity : class
    {

        public Task<string> GetSPASv1CompCode(string SPASv2CompanyCode);

    }
}
