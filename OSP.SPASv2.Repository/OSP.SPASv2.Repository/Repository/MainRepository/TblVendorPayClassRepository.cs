using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using OSP.Common.Domain.View;
using DocumentFormat.OpenXml.InkML;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class TblVendorPayClassRepository : ITblVendorPayClassRepository<TblVendorPayClass>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblVendorPayClass> _AbstractRepository;
        //GenericRepository<TblPurchaseorderhdr> _GenericRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb; 
        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public TblVendorPayClassRepository(SPASv2Context context)
        {
            _context = context;

            _AbstractRepository = new AbstractRepository<TblVendorPayClass>(_context);
            //_GenericRepository = new GenericRepository<TblPurchaseorderhdr>(_context);
        }

        public async Task<TblVendorPayClass> ReadPayClass(string VendorCode, string PayclassCode)
        {
            try
            {
                var result = await _context.TblVendorPayClass.Where(a => a.VendorCode.Equals(VendorCode) && a.PayClassCode.Equals(PayclassCode)) .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}
