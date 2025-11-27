namespace OSP.SPASv2.Domain.View
{
    public class qryRequisitionItem
    {
        public int ReqItemNo { get; set; }
        public string ReqNo { get; set; }
        public string CompanyType { get; set; }
        public string DeptDesc { get; set; }
        public string DeptCode { get; set; }
        public string ItemCode { get; set; }
        public string Item { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string DiscountCode { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Vat { get; set; }
        public decimal NetOfVat { get; set; }
        public bool isDeduct { get; set; }
        public decimal Freight { get; set; }
        public decimal TotalTax { get; set; }
    }
}
