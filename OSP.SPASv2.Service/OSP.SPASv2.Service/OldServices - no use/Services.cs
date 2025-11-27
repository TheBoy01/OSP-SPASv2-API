using OSP.SPASv2.Service.Model;
using OSP.SPASv2.Service.Services;

namespace OSP.SPASv2.Service.OldServices
{
    public class Services : IServices
    {
        public User Get(User user)
        {
            if (user.Username != "wa" && user.Password != "123412")
            {
                user = null;
            }
            
            return user;
        }
    }
}
