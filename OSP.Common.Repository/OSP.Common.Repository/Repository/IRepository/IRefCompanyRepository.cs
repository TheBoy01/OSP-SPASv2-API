

using OSP.Common.Domain.References;

namespace OSP.Common.Repository.IRepository.Table
{
    public interface IRefCompanyRepository<TEntity> where TEntity : class
    {
        public Task<IList<RefCompany>> GetCompanylist(string company);
        public Task<IList<RefCompany>> GetCompanies();
        public Task<string> GetCompanycode(string companydesc);
        public Task<IList<qryCompanyType>> GetCompanyTypesAccess(string personid);
    }
}
