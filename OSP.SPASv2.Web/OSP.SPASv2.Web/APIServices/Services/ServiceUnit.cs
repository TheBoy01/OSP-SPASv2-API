using OSP.SPASv2.Web.APIServices;


namespace OSP.SPASv2.Web.APIServices.Services
{
    public class ServiceUnit
    {
        private VendorService _VendorService;
        private BranchService _BranchService;
        private CompanyService _CompanyService;
        private AccountService _AccountService;
        private PaymentRequestService _PaymentRequestService;
        private RequisitionService _RequisitionService;

        public RequisitionService RequisitionService
        {
            get
            {
                if (_RequisitionService == null)
                {
                    this._RequisitionService = new RequisitionService();
                }
                return _RequisitionService;
            }
        }

        public PaymentRequestService PaymentRequestService
        {
            get
            {
                if (_PaymentRequestService == null)
                {
                    this._PaymentRequestService = new PaymentRequestService();
                }
                return _PaymentRequestService;
            }
        }

        public CompanyService CompanyService
        {
            get
            {
                if (_CompanyService == null)
                {
                    this._CompanyService = new CompanyService();
                }
                return _CompanyService;
            }
        }

        public BranchService BranchService
        {
            get
            {
                if (_BranchService == null)
                {
                    this._BranchService = new BranchService();
                }
                return _BranchService;
            }
        }

        public VendorService VendorService
        {
            get
            {
                if (_VendorService == null)
                {
                    this._VendorService = new VendorService();
                }
                return _VendorService;
            }
        }


        public AccountService AccountService
        {
            get
            {
                if (_AccountService == null)
                {
                    this._AccountService = new AccountService();
                }
                return _AccountService;
            }
        }

    }
}
