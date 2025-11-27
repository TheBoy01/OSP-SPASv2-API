using OSP.Common.Domain.Tables;
using OSP.Common.Domain.View;
using OSP.SPASv2.Domain.References;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Domain.View;

namespace OSP.SPASv2.Domain.Params
{
    public class ReportParams
    {
        public IList<RefReportType> ReportTypeList { get; set; } = new List<RefReportType>();
        public IList<RefReportname> ReportNameList { get; set; } = new List<RefReportname>();
        public string PersonId { get; set; }
    }
}
