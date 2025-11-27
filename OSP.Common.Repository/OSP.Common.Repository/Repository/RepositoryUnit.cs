using OSP.Common;
using OSP.Common.Repository.Context;
using OSP.Common.Repository.Repository.MainRepository;
using OSP.Common.Repository.Repository;
using OSP.Common.Repository.MainRepository;

using OSP.SPASv2.Repository.Repository;

namespace OSP.Common.Repository.Repository

//namespace OSP.SPASv2.Repository.Repository.GoogleSQLRepository
{
    public class RepositoryUnit
    {
        #region Private Member Variables

        #endregion

        #region Constructor
        public RepositoryUnit(OSPContext context)
        {
            this.context = context;
        }
        #endregion

        #region Public Properties

        private OSPContext context;
        private UserRepository _UserRepository;
        private ResponseRepository _ResponseRepository;
        private RefCompanyRepository _RefCompanyRepository;
        private RefBranchRepository _RefBranchRepository;
        private RefChapelRepository _RefChapelRepository;
        private RefDepartmentRepository _RefDepartmentRepository;
        private NotificationRepository _NotificationRepository;
        private EmployeeRepository _EmployeeRepository;
        private TblOutboxRepository _TblOutboxRepository;
        private SendemaildtlRepository _SendemaildtlRepository;
        //UserRepository _UserRepository;

        public SendemaildtlRepository SendemaildtlRepository
        {
            get
            {
                if (_SendemaildtlRepository == null)
                {
                    this._SendemaildtlRepository = new SendemaildtlRepository(context);
                }
                return _SendemaildtlRepository;
            }
        }

        public TblOutboxRepository TblOutboxRepository
        {
            get
            {
                if (_TblOutboxRepository == null)
                {
                    this._TblOutboxRepository = new TblOutboxRepository(context);
                }
                return _TblOutboxRepository;
            }
        }

        public EmployeeRepository EmployeeRepository
        {
            get
            {
                if (_EmployeeRepository == null)
                {
                    this._EmployeeRepository = new EmployeeRepository(context);
                }
                return _EmployeeRepository;
            }
        }
        
        public NotificationRepository NotificationRepository
        {
            get
            {
                if (_NotificationRepository == null)
                {
                    this._NotificationRepository = new NotificationRepository(context);
                }
                return _NotificationRepository;
            }
        }


        public RefDepartmentRepository RefDepartmentRepository
        {
            get
            {
                if (_RefDepartmentRepository == null)
                {
                    this._RefDepartmentRepository = new RefDepartmentRepository(context);
                }
                return _RefDepartmentRepository;
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


        public RefBranchRepository RefBranchRepository
        {
            get
            {
                if (_RefBranchRepository == null)
                {
                    this._RefBranchRepository = new RefBranchRepository(context);
                }
                return _RefBranchRepository;
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

        #region Public Properties

        public UserRepository UserRepository
        {
            get
            {
                if (_UserRepository == null)
                {
                    this._UserRepository = new UserRepository(context);
                }
                return _UserRepository;
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


        #endregion


        #endregion

    }
}
