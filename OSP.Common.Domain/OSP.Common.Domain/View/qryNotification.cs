using System.ComponentModel.DataAnnotations;

namespace OSP.Common.Domain.View
{
    public class qryNotification
    {
        public string SystemCode { get; set; }

        public string ReferenceNo { get; set; }

        public string NotificationCode { get; set; }
        public string Receiver { get; set; }

        public string Message { get; set; }


        public string Network { get; set; }


    }
}
