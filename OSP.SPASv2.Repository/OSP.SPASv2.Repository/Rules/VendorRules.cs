using OSP.SPASv2.Repository.Parameters;
using OSP.SPASv2.Domain.Tables;
using System.Web.Http.ModelBinding;
using System.Text;

namespace OSP.SPASv2.Repository.Rules
{
    public class VendorRules : IRules<VendorParams>
    {
        StringBuilder sb = new StringBuilder();

        public string CanCreate(VendorParams entity)
        {
            sb = new StringBuilder();
            try
            {
                if (entity.TblVendor.VendorCode == "A")
                {
                    sb.Append("Invalid Vendor Code");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return sb.ToString();

        }

        public string CanDelete(VendorParams entity)
        {
            throw new NotImplementedException();
        }

        public Task<string> CanDeleteAsync(VendorParams entity)
        {
            throw new NotImplementedException();
        }

        public string CanRead(VendorParams entity)
        {
            throw new NotImplementedException();
        }

        public Task<string> CanReadAsync(VendorParams entity)
        {
            throw new NotImplementedException();
        }

        public string CanUpdate(VendorParams entity)
        {
            throw new NotImplementedException();
        }
    }
}
