using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SPASv2.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using OSP.Common.Domain.Msgbox;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Web.Models;
using DocumentFormat.OpenXml.Bibliography;
using System;

namespace SPASv2.Models
{
    public class ReportParamsModel
    {
        public string ReportType { get; set;}
        public string ReportName { get; set; }
        public string DateSelectionType { get; set; }
        public DateTime? RangeFrom { get; set; }
        public DateTime? RangeTo { get; set; }
        public DateTime? Month { get; set; }
        public string Week { get; set; }
        public string PersonId { get; set; }
    }

    public class ReportViewModel
    {
        public ReportParams Params { get; set; }
    }
}
