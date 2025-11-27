using OSP.Common.Domain.References;
using OSP.Common.Domain.View;

namespace OSP.Common.Domain.APIFactory.OSPService
{
    public interface IHttpOSPService
    {
        Task<List<qryCompanyType>> GetCompanies(string url);

    }
}
