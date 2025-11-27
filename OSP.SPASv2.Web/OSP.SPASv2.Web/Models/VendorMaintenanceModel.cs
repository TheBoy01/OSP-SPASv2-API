
//using OSP.SPASv2.Domain; 
//using OSP.SPASv2.DomainDummy.Models;
using OSP.SPASv2.Domain.Tables;
using System.ComponentModel;

namespace SPASv2.Models
{
    public class VendorMaintenanceModel
    {
        //[DisplayName("Vendor Name")]
        //public string VendorName { get; set; }

        //public string TIN { get; set; }

        //[DisplayName("CV Name")]
        //public string CVName { get; set; }
        public TblVendor TblVendor { get; set; }

        public TblVendorAddress TblVendorAddress { get; set; }
        public TblVendorpaymethod TblVendorpaymethod { get; set; }

        public TblVendorATC TblVendorATC { get; set; }
        public TblVendorDocRequired TblVendorDocRequired { get; set; }
        public TblVendorAttached TblVendorAttached { get; set; }


        //[DisplayName("Vendor Type")]
        //public VendorTypes VendorType { get; set; }

        [DisplayName("Class ID")]
        //public ClassID DefaultClassID { get; set; }

        //public PaymentType DefaultPaymentType { get; set; }

        public IList<TblVendor> VendorList { get; set; }

        public IList<TblVendorContact> VendorContact { get; set; }

        public IList<qryVendorList> qryVendorList { get; set; }

        [DisplayName("Vendor Type")]
        public IList<RefVendorType> VendorTypeList { get; set; }
        public IList<RefAddressType> AddressTypeList { get; set; }
        public List<string> BankAccTypeList { get; set; }
        public IList<RefATC> RefATCList { get; set; }
        public IList<RefVendorDocs> RefVendorDocsList { get; set; }


        public IList<RefRegion> RefRegion { get; set; }
        public IList<RefProvince> RefProvince { get; set; }
        public IList<RefCity> RefCity { get; set; }
        public IList<RefBrgy> RefBrgy { get; set; }
        //public string SelectedVendorDocType { get; set; }

        public string SelectedDiv { get; set; }

        public string ItemID { get; set; } 
    }

    //    public enum VendorTypes
    //    {
    //        Supplier,
    //        Company
    //    }

    //    public enum ClassID
    //    {
    //        PO,
    //        CSV
    //    }

    //    public enum PaymentType
    //    {
    //        CASH,
    //        CHECK
    //    }
}
