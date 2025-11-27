using System.Text.Json.Serialization;

namespace OSP.SPASv2.Domain.Tables
{
    public class TblAPIResponse<T>
    {

        [JsonPropertyName("statusCode")]
        public string StatusCode { get; set; }

        [JsonPropertyName("statusDesc")]
        public string StatusDesc { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }

        //[JsonPropertyName("data1")]
        //public T1 Data1 { get; set; }

  

    }
}
