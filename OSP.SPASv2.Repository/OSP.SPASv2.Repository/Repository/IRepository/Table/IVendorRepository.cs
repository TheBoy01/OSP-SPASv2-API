using OSP.SPASv2.Domain;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Domain.View;

namespace OSP.SPASv2.Repository.IRepository
{
    public interface IVendorRepository<TEntity> where TEntity : class
    {
        public Task<TblResponse> CreateVendor(TEntity entity);
        public Task<IList<TblVendor>> GetAllObjects();
        public Task<IList<qryVendorList>> GetVendorLists();
        public Task<IList<qryVendorList>> GetVendorLists1(string vendorname,string payclass);
        public  Task<qryVendorDetails> GetVendorDetails(string vendorcode, string payclass);
    }
}
