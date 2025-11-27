using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using OSP.Common.Domain.View;

namespace OSP.SPASv2.Repository.Repository.MainRepository
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

        public async Task<IList<TblVendorItems>> GetVendorItems(string vendorcode, string paymentclasscode)
        {
            try
            {
                var vlist = await _context.TblVendorItems.FromSqlRaw("select * from TblVendorItems where vendorcode='"+ vendorcode +"' and paymentclasscode='"+ paymentclasscode +"' and active=1").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<IList<TblVendorItems>> GetVendorItemsList(string vendorcode)
        {
            try
            {
                var vlist = await _context.TblVendorItems.FromSqlRaw("select * from TblVendorItems where vendorcode='" + vendorcode + "' and active=1").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<TblVendorItems>> GetVendorItemsList1(string vendorcode, string itemdesc)
        {
            try
            {
                
                if (string.IsNullOrEmpty(itemdesc))
                {
                    var vlist = await _context.TblVendorItems.FromSqlRaw("select * from TblVendorItems where vendorcode='" + vendorcode + "' and active=1").ToListAsync();
                    return vlist;
                }
                else
                {
                    var vlist = await _context.TblVendorItems.FromSqlRaw("select * from TblVendorItems where itemdesc like '%" + itemdesc + "%' and vendorcode='" + vendorcode + "' and active=1").ToListAsync();
                    return vlist;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<TblVendorItems> GetVendorItemsDetails(string vendorcode, string itemcode)
        {
            try
            {
                //itemcode =  await Task.FromResult(_context.Set<ValReturn<string>>()
                //.FromSqlRaw("select top 1 isnull(itemcode,'') as Value from tblvendoritems where vendorcode = '" + vendorcode + "' and itemdesc ='"+ itemcode + "'")
                //.AsEnumerable()
                //.First().Value);
                var vlist = await _context.TblVendorItems.FromSqlRaw("select * from TblVendorItems where itemcode = '" + itemcode + "' and vendorcode='" + vendorcode + "' and active=1").FirstOrDefaultAsync();
                //var item =  await Task.FromResult(_context.Set<ValReturn<string>>()
                //.FromSqlRaw("select top 1 itemcode as Value from tblvendoritems where vendorcode = '" + vendorcode + "' and itemdesc ='"+ itemcode + "'")
                //.AsEnumerable()
                //.FirstOrDefault());
                ////TblVendorItems vlist = null;


                //if (item != null)
                //{
                //     vlist = await _context.TblVendorItems.FromSqlRaw("select * from TblVendorItems where itemcode = '" + item + "' and vendorcode='" + vendorcode + "' and active=1").FirstOrDefaultAsync();
                //}
                //else
                //{
                //     vlist = await _context.TblVendorItems.FromSqlRaw("select * from TblVendorItems where itemcode = '" + itemcode + "' and vendorcode='" + vendorcode + "' and active=1").FirstOrDefaultAsync();
                //}
                
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<TblVendorItems> GetvendorItemAsync(string vendorcode, string itemdesc)
        {
            try
            {
                var vlist =  await _context.TblVendorItems.Where(a => a.ItemDesc == itemdesc.ToUpper() && a.VendorCode== vendorcode).FirstOrDefaultAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

    }
}

