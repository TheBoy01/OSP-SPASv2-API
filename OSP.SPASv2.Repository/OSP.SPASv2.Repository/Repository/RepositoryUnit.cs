using OSP.SPASv2.Repository.Context;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using SPASv2.Context;
using System.Diagnostics.Metrics;

namespace OSP.SPASv2.Repository.Repository.MainRepository

    //namespace OSP.SPASv2.Repository.Repository.GoogleSQLRepository
{
    public class RepositoryUnit
    {

        #region Private Member Variables

        #endregion

        #region Constructor

        public RepositoryUnit(SPASv2Context context)
        {
            this.context = context;
        }
        public RepositoryUnit(SPASv1Context SPASv1Cont)
        {
            this.v1Context = SPASv1Cont;
        }

        #endregion

        #region Public Properties

        private SPASv2Context context;
        private SPASv1Context v1Context;
        private EmployeeRepository _EmployeeRepository;
        private VendorRepository _VendorRepository;
        private RefBranchRepository1 _RefBranchRepository;
        private RefCompanyRepository _RefCompanyRepository;
        private ResponseRepository _ResponseRepository;
        private RefChapelRepository _RefChapelRepository;
        private RefPaymentTypeRepository _RefPaymentTypeRepository;
        private RefVendorTypeRepository _RefVendorTypeRepository;
        private RefAddressTypeRepository _RefAddressTypeRepository;
        private VendorbankaccountRepository _VendorbankaccountRepository;
        private VendorItemsRepository _VendorItemsRepository;
        private tmpPaymentRequestInventoryRepository _tmpPaymentRequestInventoryRepository;
        private PRAuthorizationRepository _PRAuthorizationRepository;
        private PaymentrequesthdrRepository _PaymentrequesthdrRepository;
        private ReftrxweekRepository _ReftrxweekRepository;
        private RefDiscountRepository _RefDiscountRepository;
        private VatRepository _VatRepository;
        private TblVendorAddressRepository _TblVendorAddressRepository;
        private RefBankAcctTypeRepository _RefBankAcctTypeRepository;
        private RefBankRepository _RefBankRepository;
        private RefATCRepository _RefATCRepository;
        private RefATCTypeRepository _RefATCTypeRepository;
        private RefVendorDocsRepository _RefVendorDocsRepository;
        private RefOutsideServerRepository _RefOutsideServerRepository;
        private RefSystemsRepository _RefSystemsRepository;
        private TblVendorContactPersonRepository _TblVendorContactPersonRepository;
        private TblBatchPRHdrRepository _TblBatchPRHdrRepository;
        private TblBatchPRDtlRepository _TblBatchPRDtlRepository;
        private TblPaymentRequestDtlRepository _TblPaymentRequestDtlRepository;

        private RefRegionRepository _RefRegionRepository;
        private RefProvinceRepository _RefProvinceRepository;
        private RefBrgyRepository _RefBrgyRepository;
        private RefCityRepository _RefCityRepository;

        private TblRequisitionHdrRepository _TblRequisitionHdrRepository;
        private TblRequisitionDtlRepository _TblRequisitionDtlRepository;

        private TblPurchaseorderhdrRepository _TblPurchaseorderhdrRepository;
        private RefItemsRepository _RefItemsRepository;
        private RefPaymentClassRepository _RefPaymentClassRepository;
        private rptPurchaseorderRepository _rptPurchaseorderRepository;

        private TblLoanHdrRepository _TblLoanHdrRepository;
        private TblVendorAdapterRepository _TblVendorAdapterRepository;
        private RefCompanyAdapterRepository _RefCompanyAdapterRepository;
        private RefAccountMapRepository _RefAccountMapRepository;
        private ReportRepository _ReportRepository;
        private TblRequisitionDtlSummaryRepository _TblRequisitionDtlSummaryRepository;

        private BatchApprovalRepository _BatchApprovalRepository;
        private TblRequisitionReasonRepository _TblRequisitionReasonRepository;
        private TblPaymentRequestAuthRepository _TblPaymentRequestAuthRepository;
        private PaymentrequisitionhdrRepository _PaymentrequisitionhdrRepository;
        private TblDRNoRepository _TblDRNoRepository;
        private TblItemBarcodesRepository _TblItemBarcodesRepository;
        private TblAssignedtoVendor_CMSRepository _TblAssignedtoVendor_CMSRepository;
        private TblVendorPayClassRepository _TblVendorPayClassRepository;
        private RefChapelBranchRepository _RefChapelBranchRepository;
        private RefChapelEmailRepository _RefChapelEmailRepository;

        public TblDRNoRepository TblDRNoRepository
        { 
            get
            {
                if (_TblDRNoRepository == null)
                {
                    this._TblDRNoRepository = new TblDRNoRepository(context);
                }
                return _TblDRNoRepository; 
            }
        }

        public TblRequisitionReasonRepository TblRequisitionReasonRepository
        {
            get
            {
                if (_TblRequisitionReasonRepository == null)
                {
                    this._TblRequisitionReasonRepository = new TblRequisitionReasonRepository(context);
                }
                return _TblRequisitionReasonRepository;
            }
        }
        public BatchApprovalRepository BatchApprovalRepository
        {
            get
            {
                if (_BatchApprovalRepository == null)
                {
                    this._BatchApprovalRepository = new BatchApprovalRepository(context);
                }
                return _BatchApprovalRepository;
            }
        }
        public TblRequisitionDtlSummaryRepository TblRequisitionDtlSummaryRepository
        {
            get
            {
                if (_TblRequisitionDtlSummaryRepository == null)
                {
                    this._TblRequisitionDtlSummaryRepository = new TblRequisitionDtlSummaryRepository(context);
                }
                return _TblRequisitionDtlSummaryRepository;
            }
        }
        public RefAccountMapRepository RefAccountMapRepository
        {
            get
            {
                if (_RefAccountMapRepository == null)
                {
                    this._RefAccountMapRepository = new RefAccountMapRepository(context);
                }
                return _RefAccountMapRepository;
            }
        }

        public ReportRepository ReportRepository
        {
            get
            {
                if (_ReportRepository == null)
                {
                    this._ReportRepository = new ReportRepository(context);
                }
                return _ReportRepository;
            }
        }

        
        public RefCompanyAdapterRepository RefCompanyAdapterRepository
        {
            get
            {
                if (_RefCompanyAdapterRepository == null)
                {
                    this._RefCompanyAdapterRepository = new RefCompanyAdapterRepository(context);
                }
                return _RefCompanyAdapterRepository;
            }
        }

        public TblVendorAdapterRepository TblVendorAdapterRepository
        {
            get
            {
                if (_TblVendorAdapterRepository == null)
                {
                    this._TblVendorAdapterRepository = new TblVendorAdapterRepository(context);
                }
                return _TblVendorAdapterRepository;
            }
        }

        public TblLoanHdrRepository TblLoanHdrRepository
        { 
            get 
            {
                if (_TblLoanHdrRepository == null)
                {
                    this._TblLoanHdrRepository = new TblLoanHdrRepository(context);
                }
                return _TblLoanHdrRepository;
            }
        }

        public rptPurchaseorderRepository rptPurchaseorderRepository
        {
            get
            {
                if (_rptPurchaseorderRepository == null)
                {
                    this._rptPurchaseorderRepository = new rptPurchaseorderRepository(context);
                }
                return _rptPurchaseorderRepository;
            }
        }
        public RefPaymentClassRepository RefPaymentClassRepository
        { 
            get 
            {
                if (_RefPaymentClassRepository == null)
                {
                    this._RefPaymentClassRepository = new RefPaymentClassRepository(context);
                }
                return _RefPaymentClassRepository;
            }
        }

        public RefItemsRepository RefItemsRepository
        {
            get
            {
                if (_RefItemsRepository == null)
                {
                    this._RefItemsRepository = new RefItemsRepository(context);
                }
                return _RefItemsRepository;
            }
        }

        public PaymentrequisitionhdrRepository PaymentrequisitionhdrRepository
        {
            get
            {
                if (_PaymentrequisitionhdrRepository == null)
                {
                    this._PaymentrequisitionhdrRepository = new PaymentrequisitionhdrRepository(context);
                }
                return _PaymentrequisitionhdrRepository;
            }
        }

        public TblPurchaseorderhdrRepository TblPurchaseorderhdrRepository
        {
            get
            {
                if (_TblPurchaseorderhdrRepository == null)
                {
                    this._TblPurchaseorderhdrRepository = new TblPurchaseorderhdrRepository(context);
                }
                return _TblPurchaseorderhdrRepository;
            }
        }
        public TblRequisitionHdrRepository TblRequisitionHdrRepository
        {
            get {
                if (_TblRequisitionHdrRepository == null)
                {
                    this._TblRequisitionHdrRepository = new TblRequisitionHdrRepository(context);
                }
                return _TblRequisitionHdrRepository;
            }
        }

        public TblRequisitionDtlRepository TblRequisitionDtlRepository
        {
            get {
                if (_TblRequisitionDtlRepository == null)
                {
                    this._TblRequisitionDtlRepository = new TblRequisitionDtlRepository(context);
                }
                return _TblRequisitionDtlRepository;
            }
        }

        public RefCityRepository RefCityRepository
        {
            get
            {
                if (_RefCityRepository == null)
                {
                    this._RefCityRepository = new RefCityRepository(context);
                }
                return _RefCityRepository;
            }
        }
        public RefBrgyRepository RefBrgyRepository
        {
            get
            {
                if (_RefBrgyRepository == null)
                {
                    this._RefBrgyRepository = new RefBrgyRepository(context);
                }
                return _RefBrgyRepository;
            }
        }
        public RefProvinceRepository RefProvinceRepository
        {
            get
            {
                if (_RefProvinceRepository == null)
                {
                    this._RefProvinceRepository = new RefProvinceRepository(context);
                }
                return _RefProvinceRepository;
            }
        }
        public RefRegionRepository RefRegionRepository
        {
            get
            {
                if (_RefRegionRepository == null)
                {
                    this._RefRegionRepository = new RefRegionRepository(context);
                }
                return _RefRegionRepository;
            }
        }

        public TblPaymentRequestDtlRepository TblPaymentRequestDtlRepository
        {
            get
            {
                if (_TblPaymentRequestDtlRepository == null)
                {
                    this._TblPaymentRequestDtlRepository = new TblPaymentRequestDtlRepository(context);
                }
                return _TblPaymentRequestDtlRepository;
            }
        }

        public TblBatchPRDtlRepository TblBatchPRDtlRepository
        {
            get
            {
                if (_TblBatchPRDtlRepository == null)
                {
                    this._TblBatchPRDtlRepository = new TblBatchPRDtlRepository(context);
                }
                return _TblBatchPRDtlRepository;
            }
        }

        public TblBatchPRHdrRepository TblBatchPRHdrRepository
        {
            get
            {
                if (_TblBatchPRHdrRepository == null)
                {
                    this._TblBatchPRHdrRepository = new TblBatchPRHdrRepository(context);
                }
                return _TblBatchPRHdrRepository;
            }
        }

        public TblVendorContactPersonRepository TblVendorContactPersonRepository
        {
            get
            {
                if (_TblVendorContactPersonRepository == null)
                {
                    this._TblVendorContactPersonRepository = new TblVendorContactPersonRepository(context);
                }
                return _TblVendorContactPersonRepository;
            }
        }
        public RefOutsideServerRepository RefOutsideServerRepository
        {
            get
            {
                if (_RefOutsideServerRepository == null)
                {
                    this._RefOutsideServerRepository = new RefOutsideServerRepository(v1Context);
                }
                return _RefOutsideServerRepository;
            }
        }

        public RefVendorDocsRepository RefVendorDocsRepository
        {
            get
            {
                if (_RefVendorDocsRepository == null)
                {
                    this._RefVendorDocsRepository = new RefVendorDocsRepository(context);
                }
                return _RefVendorDocsRepository;
            }
        }

        public RefBankRepository RefBankRepository
        {
            get
            {
                if (_RefBankRepository == null)
                {
                    this._RefBankRepository = new RefBankRepository(context);
                }
                return _RefBankRepository;
            }
        }
        public RefATCRepository RefATCRepository
        {
            get
            {
                if (_RefATCRepository == null)
                {
                    this._RefATCRepository = new RefATCRepository(context);
                }
                return _RefATCRepository;
            }
        }
        public RefATCTypeRepository RefATCTypeRepository
        {
            get
            {
                if (_RefATCTypeRepository == null)
                {
                    this._RefATCTypeRepository = new RefATCTypeRepository(context);
                }
                return _RefATCTypeRepository;
            }
        }
        public RefBankAcctTypeRepository RefBankAcctTypeRepository
        {
            get
            {
                if (_RefBankAcctTypeRepository == null)
                {
                    this._RefBankAcctTypeRepository = new RefBankAcctTypeRepository(context);
                }
                return _RefBankAcctTypeRepository;
            }
        }
        public TblVendorAddressRepository TblVendorAddressRepository
        {
            get 
            {
                if (_TblVendorAddressRepository == null)
                {
                    this._TblVendorAddressRepository = new TblVendorAddressRepository(context);
                }
                return _TblVendorAddressRepository;
            }
        }
        public VatRepository VatRepository
        {
            get
            {
                if (_VatRepository == null)
                {
                    this._VatRepository = new VatRepository(context);
                }
                return _VatRepository;
            }
        }

        public RefDiscountRepository RefDiscountRepository
        {
            get
            {
                if (_RefDiscountRepository == null)
                {
                    this._RefDiscountRepository = new RefDiscountRepository(context);
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
                    this._ReftrxweekRepository = new ReftrxweekRepository(context);
                }
                return _ReftrxweekRepository;
            }
        }

        public PaymentrequesthdrRepository PaymentrequesthdrRepository
        {
            get
            {
                if (_PaymentrequesthdrRepository == null)
                {
                    this._PaymentrequesthdrRepository = new PaymentrequesthdrRepository(context);
                }
                return _PaymentrequesthdrRepository;
            }
        }

        public PRAuthorizationRepository PRAuthorizationRepository
        {
            get
            {
                if (_PRAuthorizationRepository == null)
                {
                    this._PRAuthorizationRepository = new PRAuthorizationRepository(context);
                }
                return _PRAuthorizationRepository;
            }
        }



        public tmpPaymentRequestInventoryRepository tmpPaymentRequestInventoryRepository
        {
            get
            {
                if (_tmpPaymentRequestInventoryRepository == null)
                {
                    this._tmpPaymentRequestInventoryRepository = new tmpPaymentRequestInventoryRepository(context);
                }
                return _tmpPaymentRequestInventoryRepository;
            }
        }

        public VendorItemsRepository VendorItemsRepository
        {
            get
            {
                if (_VendorItemsRepository == null)
                {
                    this._VendorItemsRepository = new VendorItemsRepository(context);
                }
                return _VendorItemsRepository;
            }
        }
        public RefAddressTypeRepository RefAddressTypeRepository
        {
            get
            {
                if (_RefAddressTypeRepository == null)
                {
                    this._RefAddressTypeRepository = new RefAddressTypeRepository(context);
                }
                return _RefAddressTypeRepository;
            }
        }
        public RefVendorTypeRepository RefVendorTypeRepository
        {
            get
            {
                if (_RefVendorTypeRepository == null)
                {
                    this._RefVendorTypeRepository = new RefVendorTypeRepository(context);
                }
                return _RefVendorTypeRepository;
            }
        }

        public VendorbankaccountRepository VendorbankaccountRepository
        {
            get
            {
                if (_VendorbankaccountRepository == null)
                {
                    this._VendorbankaccountRepository = new VendorbankaccountRepository(context);
                }
                return _VendorbankaccountRepository;
            }
        }

        public RefPaymentTypeRepository RefPaymentTypeRepository
        {
            get
            {
                if (_RefPaymentTypeRepository == null)
                {
                    this._RefPaymentTypeRepository = new RefPaymentTypeRepository(context);
                }
                return _RefPaymentTypeRepository;
            }
        }

        public RefChapelRepository RefChapelRepository
        {
            get
            {
                if (_RefChapelRepository == null)
                {
                    this._RefChapelRepository = new RefChapelRepository(context);
                }
                return _RefChapelRepository;
            }
        }

        public RefCompanyRepository RefCompanyRepository
        {
            get
            {
                if (_RefCompanyRepository == null)
                {
                    this._RefCompanyRepository = new RefCompanyRepository(context);
                }
                return _RefCompanyRepository;
            }
        }

        public RefBranchRepository1 RefBranchRepository
        {
            get
            {
                if (_RefBranchRepository == null)
                {
                    this._RefBranchRepository = new RefBranchRepository1(context);
                }
                return _RefBranchRepository;
            }
        }


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

        public VendorRepository VendorRepository
        {
            get
            {
                if (_VendorRepository == null)
                {
                    this._VendorRepository = new VendorRepository(context);
                }
                return _VendorRepository;
            }
        }

        public ResponseRepository ResponseRepository
        {
            get
            {
                if (_ResponseRepository == null)
                {
                    this._ResponseRepository = new ResponseRepository(context);
                }
                return _ResponseRepository;
            }
        }

        public RefSystemsRepository RefSystemsRepository
        {
            get
            {
                if (_RefSystemsRepository == null)
                {
                    this._RefSystemsRepository = new RefSystemsRepository(context);
                }
                return _RefSystemsRepository;
            }
        }

        public TblPaymentRequestAuthRepository TblPaymentRequestAuthRepository
        {
            get
            {
                if (_TblPaymentRequestAuthRepository == null)
                {
                    this._TblPaymentRequestAuthRepository = new TblPaymentRequestAuthRepository(context);
                }
                return _TblPaymentRequestAuthRepository;
            }
        }

        public TblItemBarcodesRepository TblItemBarcodesRepository
        {
            get
            {
                if (_TblItemBarcodesRepository == null)
                {
                    this._TblItemBarcodesRepository = new TblItemBarcodesRepository(context);
                }
                return _TblItemBarcodesRepository;
            }
        }

        public TblAssignedtoVendor_CMSRepository TblAssignedtoVendor_CMSRepository
        {
            get
            {
                if (_TblAssignedtoVendor_CMSRepository == null)
                {
                    this._TblAssignedtoVendor_CMSRepository = new TblAssignedtoVendor_CMSRepository(context);
                }
                return _TblAssignedtoVendor_CMSRepository;
            }
        }

        public TblVendorPayClassRepository TblVendorPayClassRepository
        {
            get
            {
                if (_TblVendorPayClassRepository == null)
                {
                    this._TblVendorPayClassRepository = new TblVendorPayClassRepository(context);
                }
                return _TblVendorPayClassRepository;
            }
        }

        public RefChapelBranchRepository RefChapelBranchRepository
        {
            get
            {
                if (_RefChapelBranchRepository == null)
                {
                    this._RefChapelBranchRepository = new RefChapelBranchRepository(context);
                }
                return _RefChapelBranchRepository;
            }
        }

        public RefChapelEmailRepository RefChapelEmailRepository
        {
            get 
            {
                if (_RefChapelEmailRepository == null)
                {
                    this._RefChapelEmailRepository = new RefChapelEmailRepository(context);
                }
                return _RefChapelEmailRepository;
            }   
        }
        #endregion



    }
}
