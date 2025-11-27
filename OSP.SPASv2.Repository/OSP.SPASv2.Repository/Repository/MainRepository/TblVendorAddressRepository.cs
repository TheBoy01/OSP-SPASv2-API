using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Domain.View;
using OSP.SPASv2.Domain.View;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using OSP.SPASv2.Repository.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class TblVendorAddressRepository : ITblVendorAddressRepository<TblVendorAddress>
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblVendorAddress> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        #endregion



        public TblVendorAddressRepository(SPASv2Context context) 
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblVendorAddress>(_context);
        }

        #region Public Methods
        public async Task<TblResponse> CreateVendorAddress(TblVendorAddress entity)
        {
            await _AbstractRepository.Insert(entity);
            return await Task.FromResult(_response);
        }

        #endregion
    }
}
