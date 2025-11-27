using OSP.Common.Service.APIRepository.Repository;

namespace OSP.Common.Service.APIRepository
{
    public class RepositoryUnit
    {
        private NotificationRepository _NotificationRepository;

        public NotificationRepository NotificationRepository
        {
            get
            {
                if (_NotificationRepository == null)
                {
                    this._NotificationRepository = new NotificationRepository();
                }
                return _NotificationRepository;
            }
        }
    }
}
