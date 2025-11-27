using OSP.Common.Domain.Tables;
using OSP.SPASv2.Domain.References;

namespace OSP.SPASv2.Domain.Params
{
    public class AuthorizationParams
    {


        public IList<string> ReqNo   { get; set; } = new List<string>();
        public IList<string> BatchReqno { get; set; } = new List<string>();
        public string ReqType { get; set; } 
        public string AuthorizationType { get; set; }

        public string UserCode { get; set; }
        public TblResponse Response { get; set; }
        public string BANo { get; set; }

    }
}
