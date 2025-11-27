using OSP.SPASv2.Domain.Tables;
using System.ComponentModel;

namespace SPASv2.Models
{
    public class PRAuthorizationModel
    {
        public TblPaymentRequestAuth TblPaymentRequestAuth { get; set; }
        public IList<qryPRAuthorizationList> qryPRAuthorizationList { get; set; }
        public IList<qryListOfAuthorizerPayclass> qryListOfAuthorizerPayclass { get; set; }

        public string PaymentClass { get; set; }

        public IList<RefPaymentClass> Paymenttypelist { get; set; }

        public string AuthorizeClassButton { get; set; }
        public string RushClassButton { get; set; }
        public string BatchPRNo { get; set; }

        public IList<TblBatchPRHdr> TblBatchPRHdr { get; set; }

        public IList<TblRequisitionhdr> TblRequisitionhdr { get; set; }
        public IList<string> batchno { get; set; } = new List<string>();    
        public string BatchPRNoCombobox { get; set; }
        public string AuthorizationBatchTitle { get; set; }
        public IList<qryPaymentClassAuthorization> qryPaymentClassAuthorization { get; set; }
        public IList<qryDeclineReason> lstReason { get; internal set; }
    }
}
