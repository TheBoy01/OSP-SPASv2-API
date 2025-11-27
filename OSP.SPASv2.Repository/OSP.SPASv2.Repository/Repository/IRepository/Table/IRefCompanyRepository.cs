

using OSP.Common.Domain.References;
using OSP.Common.Domain.View;

namespace OSP.SPASv2.Repository.IRepository.Table
{
    public interface IRefCompanyRepository<TEntity> where TEntity : class
    {
        public Task<IList<RefCompany>> GetCompanylist(string company);
        public Task<IList<RefCompany>> GetCompanies();
        public Task<string> GetCompanycode(string companydesc);
        public Task<string> GetCompanyDesc(string companycode);
        public Task<IList<qryCompanyType>> GetCompanyTypes(string company);
        public Task<IList<qryCompanyType>> GetCompanyTypesAccess(string PersonID);

    }
}
