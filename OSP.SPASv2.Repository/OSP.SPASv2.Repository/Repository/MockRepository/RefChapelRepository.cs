using OSP.Common.Domain.References;
using OSP.SPASv2.Domain.References;
using OSP.SPASv2.Repository.IRepository.Table;
using OSP.SPASv2.Repository.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository.MockRepository
{
    public class RefChapelRepository : IRefChapelRepository<RefChapel>
    {
        public Task<IList<RefChapel>> GetChapels(string branchdesc, string company)
        {
            throw new NotImplementedException();
        }
    }
}
