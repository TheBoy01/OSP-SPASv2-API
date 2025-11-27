
namespace OSP.SPASv2.Repository.Parameters
{
    public class SPASv1Params
    {
        public TblResponse TblResponse { get; set; }
        public string BankCode { get; set; }
        public string BankAcctNo { get; set; }
        public bool IsClassIdExist { get; set; }
        public bool IsCOADeptExist { get; set; } 
        public string PayMethodDesc { get; set; }
        public TblDataSourceHdr TblDataSourceHdr { get; set; }
        public TblPaymentrequesthdr TblPaymentrequesthdr { get; set; }
        public TblRequisitionhdr TblRequisitionhdr { get; set; }

    }
}
