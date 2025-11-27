using OSP.Common.Domain.View;
using OSP.SPASv2.Domain.Params;

namespace OSP.SPASv2.Repository.Parameters
{
    public class PRBatchUploadParams
    {

       public qryBatchUploadExcel _qryBatchUploadExcel { get; set; }
        public BatchUploadParams _batchUploadParams { get; set; }
        public qryBatchPaymentDtl _qryBatchPaymentDtl { get; set; }
        public List<qryCompanyDetails> qryCompanyDetailsList { get; set; }
        //TblVendor _tblVendor = new TblVendor();
        //TblVendorItems _tblVendorItems = new TblVendorItems();

    }
}
