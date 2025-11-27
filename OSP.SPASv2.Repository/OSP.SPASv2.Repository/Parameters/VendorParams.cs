using OSP.SPASv2.Domain.Tables;

namespace OSP.SPASv2.Repository.Parameters
{
    public class VendorParams
    {
        public TblVendor TblVendor { get; set; }
        public TblResponse TblResponse { get; set; }
        
        public TblVendorAddress TblVendorAddress { get; set; }
    }
}
