namespace OSP.SPASv2.Domain.View
{
    public class qryRptPurchaseOrderDetails
    {
        public string Department { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public string UOM { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class qryRptPurchaseOrderConsolidated
    {
        public string PONo { get; set; }
        public string Department { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public string UOM { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string CompanyCode { get; set; }

    }

    public class qryRptTransmittalFO
    {
        public string ReqNo { get; set; }

        public string CompanyCode { get; set; }

        public string Department { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public string UOM { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public decimal Freight { get; set; }

    }
}
