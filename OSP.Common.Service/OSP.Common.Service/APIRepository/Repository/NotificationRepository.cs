using OSP.Common.Service.Utility;
using System.Text.Json;

namespace OSP.Common.Service.APIRepository.Repository
{
    public class NotificationRepository
    {
        
     
        string APIBaseURLRepo;
        string APIBaseURLCommonRepo;

        public NotificationRepository()
        {
     
            
        }
        TblResponse response;
        public async Task<TblResponse> CreateNotification(TblNotification _tblnotification)
        {
            try
            {
                // var config1 = ip;

                // var config1 = ip;
                string requestAddress = APIBaseURLCommonRepo + "/CommonRepository/CreateNotification";
                //string requestAddress = "https://onlineforms.stpeter.com.ph/OSPRepository/CommonRepository/CreateNotification";

                response = await UtilitiesHttpClient<TblNotification>.PostAsync(_tblnotification, requestAddress);
                //await UtilitesHttpClient<string>.PostAsync(_prno, requestAddress2);


                return response;


            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();

                response.Status = ex.Message;
                return response;

            }
        }

        public async Task<TblResponse> CreateNotification(TblNotification _tblnotification,string key)
        {
            try
            {
                // var config1 = ip;
                TblResponse response;
                // var config1 = ip;
                string requestAddress = key + "/CommonRepository/CreateNotification";
                //string requestAddress = "https://onlineforms.stpeter.com.ph/Common/CommonRepository/CreateNotification";

                response = await UtilitiesHttpClient<TblNotification>.PostAsync(_tblnotification, requestAddress);
                //await UtilitesHttpClient<string>.PostAsync(_prno, requestAddress2);


                return response;


            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();

                response.Status = ex.Message;
                return response;

            }
        }


        public async Task<TblResponse> CreateNotificationS(string Message)
        {
            try
            {
                // var config1 = ip;

                // var config1 = ip;
                //string requestAddress = APIBaseURLCommonRepo + "/CommonRepository/CreateNotification";
                string requestAddress = "https://onlineforms.stpeter.com.ph/OSPRepository/CommonRepository/CreateNotification";

                response = await UtilitiesHttpClient<string>.PostAsync(Message, requestAddress);
                //await UtilitesHttpClient<string>.PostAsync(_prno, requestAddress2);


                return response;


            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();

                response.Status = ex.Message;
                return response;

            }
        }

    }
}
