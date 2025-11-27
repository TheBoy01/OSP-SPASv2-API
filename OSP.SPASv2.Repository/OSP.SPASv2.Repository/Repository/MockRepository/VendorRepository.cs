using OSP.SPASv2.Domain;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Domain.View;
using OSP.SPASv2.Repository.IRepository;
using System.Collections.Generic;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class IVendorRepository : IVendorRepository<TblVendor>
    {
        public Task<TblResponse> CreateVendor(TblVendor entity)
        {
            throw new NotImplementedException();
        }

        public Task<IList<TblVendor>> GetAllObjects()
        {
            throw new NotImplementedException();
        }

        public Task<qryVendorDetails> GetVendorDetails(string vendorcode, string payclass)
        {
            throw new NotImplementedException();
        }

        public Task<IList<qryVendorList>> GetVendorLists()
        {
            throw new NotImplementedException();
        }

        public Task<IList<qryVendorList>> GetVendorLists1(string vendorname, string payclass)
        {
            throw new NotImplementedException();
        }
    }
}
