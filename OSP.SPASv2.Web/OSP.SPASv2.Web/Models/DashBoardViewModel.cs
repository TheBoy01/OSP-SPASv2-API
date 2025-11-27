namespace SPASv2.Models
{
    public class DashBoardViewModel
    {
        public IList<PaymentRequestModel> ListofPR;
        public IList<qryRequestPaymentRequestbyStatus> RequestList;
        private string _ActiveCtr;
        private string _DeniedCtr;
        private string _PaymentValidationCtr;
        private string _OverdueCtr;
        private string _SpecialCaseCtr;
        private string _RushCtr;
        private string _ForPaymentCtr;

        public DashBoardViewModel()
        {
            ListofPR = new List<PaymentRequestModel>();
            RequestList = new List<qryRequestPaymentRequestbyStatus>();
            _ActiveCtr = "0";
            _ForPaymentCtr = "0";
            _DeniedCtr = "0";
            _PaymentValidationCtr = "0";

            PDVerifierCtr = "0";
            _OverdueCtr = "0";

            PDApproverCtr = "0";
            _SpecialCaseCtr = "0";
            _RushCtr = "0";


            RequestJourney = new List<qryPaymentRequestAuthDtl>();
            RequestNo = "";
            RequestListTitle = "";

            DashboardDate = new DateTime(1900,1,1);

            isSearch = false;
        }

        public bool isSearch { get; set; }

       public bool DashboardIsUptodate { get; set; } = false;

        public string RushCtr
        {
            get
            {
                return _RushCtr;
            }
            set
            {
                _RushCtr = value;
            }
        }
        public string SpecialCaseCtr
        {
            get
            {
                return _SpecialCaseCtr;
            }
            set
            {
                _SpecialCaseCtr = value;
            }
        }

        public string OverdueCtr
        {
            get
            {
                return _OverdueCtr;
            }
            set
            {
                _OverdueCtr = value;
            }
        }

        public string PaymentValidationCtr
        {
            get
            {
                return _PaymentValidationCtr;
            }
            set
            {
                _PaymentValidationCtr = value;
            }
        }

        public string DeniedCtr
        {
            get
            {
                return _DeniedCtr;
            }
            set
            {
                _DeniedCtr = value;
            }
        }

        public string ActiveCtr {
            get {
                return _ActiveCtr;
            }
            set
            {
                _ActiveCtr = value;
            }
        }

        public string ForPaymentCtr
        {
            get
            {
                return _ForPaymentCtr;
            }
            set
            {
                _ForPaymentCtr = value;
            }
        }
        

        public string PDVerifierCtr { get; set; }
        public string PDApproverCtr { get; set; }

        public IList<qryPaymentRequestAuthDtl> RequestJourney { get; set; }
        public IList<qryPaymentRequestAuthDtl> PORequestJourney { get; set; }
        public bool isPaymentRequest { get; set; }
        public string RequestJourneyType { get; set; } = "";
        public string RequestNo { get; set; }
        public bool isStartUp { get; set; }
        public string PersonID { get; set; }

        public string RequestListTitle { get; set; }

        public qryPaymentRequestHdr PaymentRequestHdr { get; set; }

        public IList<DonutChartProperties> DonutChartValue { get; set; }
        public IList<string> DonutChartLabel { get; set; }
        public IList<decimal> DonutChartCtr { get; set; }

        public IList<string> BarCharRequestList { get; set; }
        public IList<decimal> BarChartRequestData { get; set; }
        public IList<decimal> BarChartCreditedData { get; set; }

        public string imgSrc { get; set; }

        public DateTime DashboardDate { get; set; }
        public string DashboardType { get; set; }
        public string POWithBalanceCtr { get; internal set; }
        public string RequesterRushCtr { get; internal set; }
        public string QuickLinkID { get; set; }
        public IList<qryVendorRunningBalance> VendorRunningBalanceList { get; set; } = new List<qryVendorRunningBalance>();
        public IList<RefPaymentClass> Paymenttypelist { get; set; }
        public IList<qryRequisitionDepartment> RequestDepartmentList { get; set; }
    }

    public class DonutChartProperties
    {
        public DonutChartProperties(string _name, decimal _value) 
        {
            name = _name;
            value = _value;
        }
        public string name { get; set; }
        public decimal value { get; set; }
        
    }
}
