namespace SPASv2.Models
{
    public class BatchRequisitionViewModel
    {
        public BatchRequisitionViewModel()
        {
            RequestAttachments = new List<qryRequestAttachments>();
        }
        public IList<qryPaymentRequestHdr> _BatchSummaryList { get; set; }

        public IList<qryPRBatchItems> _BatchItemList { get; set; }

        public DashBoardViewModel DashBoardViewModel { get; set; }

        public IList<string> RequestNoList { get; set; }

        public IList<qryRequestAttachments> RequestAttachments{get; set; }

        public string BatchFileName { get; set; }
        public string BatchPRNo { get; set; }
        public string BatchFilePath { get; set; }
        public IList<TblRequisitionhdr> TblRequisitionhdr { get; set; }
        public IList<qryRequisitionHdr> qryRequisitionHdrList { get; set; }
        public RequisitionViewModel RequisitionViewModel { get; set; }
        public IList<qryBatchPaymentHdr> BatchPaymentHdrList { get; set; }
        public IList<qryBatchPaymentDtl> BatchPaymentDtlList { get; set; }
        public IList<TblRequisitiondtl> TblRequisitiondtl { get; set; }
        public IList<qryRequisition> qryRequisition { get; set; }
        public string error { get; set; }
        public string FileDirectory { get; internal set; }
        public bool isUploadTemplate { get; set; } = false; 
        public string ExcelFileUploadPath { get; set; }

    }

    public class qryRequestAttachments
    {
        public string RequestNo { get; set; }
        public string FileName { get; set; }
        public string Src { get; set; }
    }


}
