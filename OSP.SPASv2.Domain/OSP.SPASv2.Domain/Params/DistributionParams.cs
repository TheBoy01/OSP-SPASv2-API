using OSP.Common.Domain.Tables;
using OSP.Common.Domain.View;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Domain.View;

namespace OSP.SPASv2.Domain.Params
{
    public class DistributionParams
    { 
        public List<qryRequisitionCasket> qryRequisitionCasket { get; set; }
        public List<tmpAssignedtoVendor_CMS> tmpAssignedtoVendor_CMS { get; set; }
        public TblResponse TblResponse { get; set; }
    }
}
