using OSP.Common.Domain.References;
using OSP.Common.Domain.View;
using OSP.SPASv2.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository.MockRepository
{
    public class RefCompanyRepository : IRefCompanyRepository<RefCompany>
    {
        public Task<IList<RefCompany>> GetCompanies()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetCompanycode(string companydesc)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetCompanyDesc(string companycode)
        {
            throw new NotImplementedException();
        }

        public Task<IList<RefCompany>> GetCompanylist(string company)
        {
            throw new NotImplementedException();
        }

        public Task<IList<qryCompanyType>> GetCompanyTypes(string company)
        {
            throw new NotImplementedException();
        }

        public Task<IList<qryCompanyType>> GetCompanyTypesAccess(string PersonID)
        {
            throw new NotImplementedException();
        }
    }
}
