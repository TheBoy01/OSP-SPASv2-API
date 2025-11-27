using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OSP.Common.Domain.Tables
{
    public class TblSmtp
    {
        public TblSmtp()
        {
            HostID = string.Empty;
            Host = string.Empty;
            Sender = string.Empty;
            Port = 0;
            EnableSSL = false;
            Username = string.Empty;
            Password = string.Empty;
            Active = false;
            StartDate = Convert.ToDateTime("1/1/1900");
            EndDate = Convert.ToDateTime("1/1/1900");
        }
        [StringLength(10)]
        public string HostID { get; set; }

        [StringLength(50)]
        public string Host { get; set; }

        [StringLength(50)]
        public string Sender { get; set; }

        public int Port { get; set; }

        public bool EnableSSL { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        public bool Active { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

    }
}
