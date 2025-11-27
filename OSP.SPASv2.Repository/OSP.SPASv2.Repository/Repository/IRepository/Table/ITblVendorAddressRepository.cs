using OSP.SPASv2.Domain;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Domain.View;

namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ITblVendorAddressRepository<TEntity> where TEntity : class
    { 
        public Task<TblResponse> CreateVendorAddress(TEntity entity);

    }
}
