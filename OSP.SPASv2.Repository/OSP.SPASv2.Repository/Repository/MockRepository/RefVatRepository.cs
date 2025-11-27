using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Repository.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository.MockRepository
{
    public class RefVatRepository : IRefVatRepository<RefVat>
    {
        public Task<RefVat> GetRefVat()
        {
            throw new NotImplementedException();
        }
    }
}
