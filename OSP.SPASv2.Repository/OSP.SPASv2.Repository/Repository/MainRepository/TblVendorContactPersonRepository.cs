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
    public class TblVendorContactPersonRepository : ITblVendorContactPersonRepository<TblVendorContactPerson>
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblVendorContactPerson> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        #endregion

        public TblVendorContactPersonRepository(SPASv2Context context)  
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblVendorContactPerson>(_context);
        }

        public async Task<IList<qryVendorContact>> GetVendorContact(string _VendorCode)
        {
            try
            {
                var vlist = await _context.qryVendorContact.FromSqlRaw("sp_GetVendorActiveContact '" + _VendorCode +"'").ToListAsync();
                //         var vlist = await _context.TblPaymentrequesthdr.OrderByDescending(n=>n.AuditDate).FirstOrDefaultAsync(n=>n.CompanyCode== companycode && n.DeptCode==branchcode);

                //vlist = new TblPaymentrequesthdr();
                return vlist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }  
        }

        public async Task<IList<qryVendorContact>> GetVendorContactEMAILByName(string _vendorname)
        {
            try
            {
                //var vlist = await _context.qryVendorContact.FromSqlRaw("select c.ContactType,b.ContactDetails From TblVendorContactPerson a inner join TblVendorContact b on a.ContactPersonID = b.ContactPersonID left join RefContactType c on b.ContactCode = c.ContactCode where a.Displayname = '" + _vendorname + "' and Contacttype = 'EMAIL'").ToListAsync();
                var vlist = await _context.qryVendorContact.FromSqlRaw("sp_EmailVendorcontact '"+ _vendorname + "','EMAIL'").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
