using OSP.SPASv2.Domain.Tables;

namespace OSP.SPASv2.Repository.Parameters
{
    public class PaymentRequestParams
    {
        public tmpPaymentRequestInventory tmpPaymentRequestInventory { get; set; }
        public TblResponse TblResponse { get; set; }
        public TblPaymentrequesthdr TblPaymentrequesthdr { get; set; }
        public TblPaymentRequestAuth TblPaymentRequestAuth { get; set; }
        public string BankCode { get; set; }
        public bool IsClassIdExist { get; set; }
        public bool IsCOADeptExist { get; set; }
        public List<TblVendor> VendorName { get; set; }
        public List<TblVendorItems   > VendorItem { get; set; }
        //public List<string> VendorName { get; set; }

    }
}
