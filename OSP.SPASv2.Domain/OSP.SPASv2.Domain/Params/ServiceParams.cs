using OSP.Common.Domain.View;
using OSP.SPASv2.Domain.View;

namespace OSP.SPASv2.Domain.Params
{
    public class ServiceParams
    {
        public string LastNo { get; set; }

        public List<qryCMSPOHdr> qryCMSPOHdrList { get; set; }
        public List<qryCMSPODtl> qryCMSPODtlList { get; set; }
        public List<qryCMSRefChapel> qryCMSRefChapelList { get; set; }
        public string BKPTemplatePath { get; set; }
        public string BKPSavingPath { get; set; }
        public string SystemCode { get; set; }
        public qryBKPName qryBKPName { get; set; }
    }
}
