using OSP.Common.Domain.Tables;
using OSP.SPASv2.Domain.References;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Domain.View;

namespace OSP.SPASv2.Domain.Params
{
    public class RequisitionParams
    {
        public List<TblRequisitionhdr> RequisitionHdrList { get; set; }
        public List<TblRequisitiondtl> RequisitionDtlList { get; set; }
        //public qryComputeBreakdown qryComputeBreakdown { get; set; }
        public List<TblLoanhdr> tblLoanhdrs { get; set; }
        public List<TblRequisitionDtlSummary> TblRequisitionDtlSummary { get; set; }
        public TblResponse TblResponse { get; set; }

        public string LastNo { get; set; }
        public string CompanyCode { get; set; }
        public string ReqNo { get; set; }
        public string UserID { get; set; }
        public List<TblDataSourceDtl> TblDataSourceDtl_List { get; set; }
        public List<RefAccountMap> RefAccountMap { get; set; }
        public decimal EWTPercentage { get; set; }
        public decimal CreditAP { get; set; }
        public decimal CreditEWT { get; set; }
        public decimal TotalVAT { get; set; }
        public string Payclass { get; set; }
        public TblRequisitionhdr _TblRequisitionhdr_old { get; set; }
        public string BatchReqNo { get; set; }
        public string PONo { get; set; }
        public string ServerPOPath { get; set; }
        //public List<qryRequisition> qryRequisitionList { get; set; }
    }
}
