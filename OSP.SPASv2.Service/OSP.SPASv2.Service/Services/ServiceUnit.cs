namespace OSP.SPASv2.Service.Services
{
    public class ServiceUnit
    {
        private PaymentRequestService _PaymentRequestService;
        private VendorMaintenanceService _VendorMaintenanceService; 
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

        public VendorMaintenanceService VendorMaintenanceService
        {
            get 
            {
                if (_VendorMaintenanceService == null)
                {
                    this._VendorMaintenanceService = new VendorMaintenanceService();
                }
                return _VendorMaintenanceService;
            }
        } 

    }
}
