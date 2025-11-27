using OSP.SPASv2.Domain;
using OSP.SPASv2.Domain.References;
using OSP.SPASv2.Domain.Tables;

namespace OSP.SPASv2.Repository.IRepository.Table
{
    public interface IRefVendorTypeRepository<TEntity> where TEntity : class
    {

        public Task<IList<RefVendorType>> GetVendorTypeList();

    }
}
