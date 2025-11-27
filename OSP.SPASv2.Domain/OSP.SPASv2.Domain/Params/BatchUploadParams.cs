using OSP.Common.Domain.Tables;
using OSP.Common.Domain.View;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Domain.View;

namespace OSP.SPASv2.Domain.Params
{
    public class BatchUploadParams
    {
        public TblBatchPRHdr TblBatchPRHdr { get; set; }
        //public TblPaymentrequesthdr TblPaymentrequesthdr { get; set; }
        //public TblBatchPRDtl TblBatchPRDtl { get; set; }
        public List<IFormFile> FilesAttached { get; set; }
        public qryBatchUploadExcel qryBatchUploadExcel { get; set; }
        public TblResponse TblResponse { get; set; }
        public string Payclass { get; set; }
        public List<qryBatchRequistion> qryBatchRequistions { get; set; }
        public List<qryRequisition> qryRequisitions { get; set; }
        public List<qryRequisitionHdr> qryRequisitionHdr { get; set; }
        public List<TblRequisitionhdr> TblRequisitionhdrList { get; set; }
        public List<TblRequisitiondtl> TblRequisitiondtlList { get; set; }
        public qryEmployee qryEmployee { get; set; }
        public string UserID { get; set; }
        public List<qryBatchPaymentHdr> qryBatchPaymentHdrList { get; set; }
        public List<qryBatchPaymentDtl> qryBatchPaymentDtlList { get; set; }
        public List<TblPurchaseorderhdr> TblPurchaseorderhdrList { get; set; }
        public List<qryCompanyDetails> qryCompanyDetails { get; set; }
        public string PONo { get; set; }
        public List<string> DRList { get; set; }
    }
}
