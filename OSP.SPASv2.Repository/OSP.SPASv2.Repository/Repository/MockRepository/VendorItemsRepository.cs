using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository.MockRepository
{
    public class VendorItemsRepository : IVendorItemsRepository<TblVendorItems>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblVendorItems> _AbstractRepository;
        StringBuilder sb;
        //VendorbankaccountRules vrules = new VendorbankaccountRules();

        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public VendorItemsRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblVendorItems>(_context);
        }

        public Task<IList<TblVendorItems>> GetVendorItems(string vendorcode, string paymentclasscode)
        {
            throw new NotImplementedException();
        }

        public Task<TblVendorItems> GetVendorItemsDetails(string vendorcode, string itemcode)
        {
            throw new NotImplementedException();
        }

        public Task<IList<TblVendorItems>> GetVendorItemsList(string vendorcode)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        //public async Task<RepositoryResponse> Create(TblVendorpaymethod entity)
        //{
        //    RepositoryResponse _r = new RepositoryResponse();
        //    if (vrules.CanCreate(entity) == false)
        //    {
        //        _r = new RepositoryResponse()
        //        {
        //            StatusCode = " ",
        //            StatusDesc = " "
        //        };
        //    }
        //    else
        //    {
        //        _r = new RepositoryResponse()
        //        {
        //            StatusCode = " ",
        //            StatusDesc = " "
        //        };
        //    }
        //    this.CreateVendorbankaccount(entity);
        //    return await Task.FromResult(_r);
        //}
        #endregion

        #region Public Functions

        //public async Task<IList<TblVendorItems>> GetVendorAccttype()
        //{
        //    try
        //    {
        //        var vlist = await _context.TblVendorpaymethod.FromSqlRaw("select * from TblVendorpaymethod where active=1").ToListAsync();
        //        return vlist;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }

        //}



        #endregion

    }
}

