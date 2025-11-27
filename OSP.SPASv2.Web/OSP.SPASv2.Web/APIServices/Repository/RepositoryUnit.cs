using OSP.SPASv2.Web.APIServices;
using OSP.SPASv2.Web.APIServices.Repository;

namespace OSP.SPASv2.Web.APIServices
{
    public class RepositoryUnit
    {
        
        private VendorRepository _VendorRepository;
        private CompanyRepository _CompanyRepository;
        private BranchRepository _BranchRepository;
        private ChapelRepository _ChapelRepository;
        private RefPaymentTypeRepository _RefPaymentTypeRepository;
        private PaymentRequestRepository _PaymentRequestRepository;
        private PRAuthorizationRepository _PRAuthorizationRepository;
        private ReftrxweekRepository _ReftrxweekRepository;
        private RefDiscountRepository _RefDiscountRepository;
        private RefDepartmentRepository _RefDepartmentRepository;
        private PRBatchUploadRepository _PRBatchUploadRepository;
        private TblRequisitionRepository _TblRequisitionRepository;
        private TblPurchaseorderhdrRepository _TblPurchaseorderhdrRepository;
        private RefPayClassRepository _RefPayClassRepository;
        private RefItemRepository _RefItemRepository;
        private EmployeeRepository _EmployeeRepository;
        private ReportRepository _ReportRepository;
        

        public EmployeeRepository EmployeeRepository
        {
            get
            {
                if (_EmployeeRepository == null)
                {
                    this._EmployeeRepository = new EmployeeRepository();
                }
                return _EmployeeRepository;
            }
        }
        public RefItemRepository RefItemRepository
        {
            get
            {
                if (_RefItemRepository == null)
                {
                    this._RefItemRepository = new RefItemRepository();
                }
                return _RefItemRepository;
            }
        }
        public RefPayClassRepository RefPayClassRepository
        {
            get
            {
                if (_RefPayClassRepository == null)
                {
                    this._RefPayClassRepository = new RefPayClassRepository();
                }
                return _RefPayClassRepository;
            }
        }

        public TblPurchaseorderhdrRepository TblPurchaseorderhdrRepository
        { 
            get 
            {
                if (_TblPurchaseorderhdrRepository == null)
                {
                    this._TblPurchaseorderhdrRepository = new TblPurchaseorderhdrRepository();
                }
                return _TblPurchaseorderhdrRepository;
            }
        }

        public TblRequisitionRepository TblRequisitionRepository
        {
            get 
            {
                if (_TblRequisitionRepository == null)
                {
                    this._TblRequisitionRepository = new TblRequisitionRepository();
                }
                return _TblRequisitionRepository;
            }
        }
        public PRBatchUploadRepository PRBatchUploadRepository
        {
            get
            {
                if (_PRBatchUploadRepository == null)
                {
                    this._PRBatchUploadRepository = new PRBatchUploadRepository();
                }
                return _PRBatchUploadRepository;
            }
        }

        public RefDepartmentRepository RefDepartmentRepository
        {
            get
            {
                if (_RefDepartmentRepository == null)
                {
                    this._RefDepartmentRepository = new RefDepartmentRepository();
                }
                return _RefDepartmentRepository;
            }
        }

        public RefDiscountRepository RefDiscountRepository
        {
            get
            {
                if (_RefDiscountRepository == null)
                {
                    this._RefDiscountRepository = new RefDiscountRepository();
                }
                return _RefDiscountRepository;
            }
        }

        public ReftrxweekRepository ReftrxweekRepository
        {
            get
            {
                if (_ReftrxweekRepository == null)
                {
                    this._ReftrxweekRepository = new ReftrxweekRepository();
                }
                return _ReftrxweekRepository;
            }
        }

        public PRAuthorizationRepository PRAuthorizationRepository
        {
            get
            {
                if (_PRAuthorizationRepository == null)
                {
                    this._PRAuthorizationRepository = new PRAuthorizationRepository();
                }
                return _PRAuthorizationRepository;
            }
        }

        public PaymentRequestRepository PaymentRequestRepository
        {
            get
            {
                if (_PaymentRequestRepository == null)
                {
                    this._PaymentRequestRepository = new PaymentRequestRepository();
                }
                return _PaymentRequestRepository;
            }
        }

        public RefPaymentTypeRepository RefPaymentTypeRepository
        {
            get
            {
                if (_RefPaymentTypeRepository == null)
                {
                    this._RefPaymentTypeRepository = new RefPaymentTypeRepository();
                }
                return _RefPaymentTypeRepository;
            }
        }

        public ChapelRepository ChapelRepository
        {
            get
            {
                if (_ChapelRepository == null)
                {
                    this._ChapelRepository = new ChapelRepository();
                }
                return _ChapelRepository;
            }
        }

        public BranchRepository BranchRepository
        {
            get
            {
                if (_BranchRepository == null)
                {
                    this._BranchRepository = new BranchRepository();
                }
                return _BranchRepository;
            }
        }


        public CompanyRepository CompanyRepository
        {
            get
            {
                if (_CompanyRepository == null)
                {
                    this._CompanyRepository = new CompanyRepository();
                }
                return _CompanyRepository;
            }
        }

        public VendorRepository VendorRepository
        {
            get
            {
                if (_VendorRepository == null)
                {
                    this._VendorRepository = new VendorRepository();
                }
                return _VendorRepository;
            }
        }

        public ReportRepository ReportRepository
        {
            get
            {
                if (_ReportRepository == null)
                {
                    this._ReportRepository = new ReportRepository();
                }
                return _ReportRepository;
            }
        }

        

    }
}
