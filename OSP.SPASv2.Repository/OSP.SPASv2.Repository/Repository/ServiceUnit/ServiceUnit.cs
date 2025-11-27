using OSP.SPASv2.Repository.Context;
using SPASv2.Context;

namespace OSP.SPASv2.Repository.Repository.ServiceUnit
{
    public class ServiceUnit
    {
        #region Private Member Variables

        #endregion

        #region Constructor
         
        public ServiceUnit(SPASv2Context context)
        {
            this.context = context;
        }
        public ServiceUnit(SPASv1Context SPASv1Cont)
        {
            this.v1Context = SPASv1Cont;
        }
        #endregion

        #region Public Properties

        private SPASv2Context context;
        private SPASv1Context v1Context;

        private VendorService _VendorService;

        #endregion

        public VendorService VendorService
        {
            get 
            {
                if (_VendorService == null)
                {
                    this._VendorService = new VendorService(context);
                }
                return _VendorService;
            }
        }
    }
}
