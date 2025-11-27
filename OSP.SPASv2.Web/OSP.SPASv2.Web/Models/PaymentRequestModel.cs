//using Domain.Models;
//using Domain.ViewModel;
//using OSP.SPASv2.Domain;
using OSP.SPASv2.Domain;
//using OSP.SPASv2.Domain.DataContract;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPASv2.Models
{
    public class PaymentRequestModel
    {
        public DashBoardViewModel dashboardViewModel { get; set; }

        public PaymentRequestModel()
        {
            
        }

        [Key]
        [DisplayName("Payment Request Number")]
        [Required(ErrorMessage = "This is required.")]
        public string PRNo { get; set; }

        [DisplayName("Request Date")]
        [Required(ErrorMessage = "This is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RequestDatetime { get; set; }

        [DisplayName("Request Date")]
        public string Requestdate { get; set; }

        [DisplayName("Sales Invoice Date")]
        [Required(ErrorMessage = "This is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SalesInvoiceDatetime { get; set; }

        [DisplayName("Sales Invoice Date")]
        public string InvoiceDate { get; set; }


        [DisplayName("Vendor Name")]
        [Required(ErrorMessage = "This is required.")]
        public string Supplier { get; set; }

        public IList<qryVendorList> VendorList { get; set; }

        public string Vendorcode { get; set; }

        [DisplayName("Payee CV Name")]
        [Required(ErrorMessage = "This is required.")]
        public string PCVName { get; set; }



        [Required(ErrorMessage = "This is required.")]
        public string Address { get; set; }

        [Key]
        [DisplayName("With Purchase Order")]
        [Required(ErrorMessage = "This is required.")]
        public string PONo { get; set; }


        [DisplayName("Department")]
        [Required(ErrorMessage = "This is required.")]
        public string Code { get; set; }

        public string BranchCode { get; set; }

        [DisplayName("Company Type")]
        [Required(ErrorMessage = "This is required.")]
        public string Company { get; set; }

        public string Companycode { get; set; }
        public string Companyid { get; set; }
        public IList<qryCompanyType> CompanyList { get; set; }
        public IList<RefDepartment> DeptList { get; set; }
        public qryBranch qryBranch { get; set; }
        //public IEnumerable<> Branchlist { get; set; }


        [DisplayName("Delivery Receipt Number")]
        [Required(ErrorMessage = "This is required.")]
        public string DeliveryReceiptNo { get; set; }


        [DisplayName("Product/Service Category")]
        [Required(ErrorMessage = "This is required.")]
        public string ItemCategory { get; set; }


        [DisplayName("Product/Service")]
        [Required(ErrorMessage = "This is required.")]
        public string ItemDesc { get; set; }

        public string ItemCode { get; set; }

        [DisplayName("Unit")]
        [Required(ErrorMessage = "This is required.")]
        public string UOM { get; set; }


        [DisplayName("Quantity")]
        [Required(ErrorMessage = "This is required.")]
        public int Quantity { get; set; }


        [DisplayName("Discount")]
        [Required(ErrorMessage = "This is required.")]
        //[DataType(DataType.Currency)]
        //[Column(TypeName = "decimal(18, 2)")]
        public string Discount { get; set; }

        public decimal Discountprice { get; set; }  
        public IList<RefDiscount> Discountlist { get; set; }

        [DisplayName("VAT")]
        [Required(ErrorMessage = "This is required.")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal VAT { get; set; }


        [DisplayName("Price per Unit")]
        [Required(ErrorMessage = "This is required.")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public  decimal ItemPrice { get; set; }


        [DisplayName("Net of VAT")]
        [Required(ErrorMessage = "This is required.")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Net { get; set; }


        [DisplayName("TIN")]    
        [Required(ErrorMessage = "This is required.")]
        public string TIN { get; set; }


        [DisplayName("Remarks")]
        [Required(ErrorMessage = "This is required.")]
        public string Remarks { get; set; }


        [DisplayName("Sales Invoice")]
        [Required(ErrorMessage = "This is required.")]
        public string SalesInvoiceLink { get; set; }

        [DisplayName("Sales Invoice No")]
        [Required(ErrorMessage = "This is required.")]
        public string InvoiceNo { get; set; }

        [DisplayName("Payment Number")]
        [Required(ErrorMessage = "This is required.")]
        public string PaymentNo { get; set; }


        [DisplayName("HO Remarks")]
        [Required(ErrorMessage = "This is required.")]
        public string SPGRemarks { get; set; }


        //public IEnumerable<qryPaymentRequestDtl> PaymentRequestdtl { get; set; }

        public IEnumerable<qryPaymode> PaymodeList { get; set; }


        [DisplayName("Payment Class")]
        [Required(ErrorMessage = "This is required.")]
        public string PaymentClass { get; set; }


        public IList<RefPaymentClass> Paymenttypelist { get; set; }

        [DisplayName("Payment Method")]
        [Required(ErrorMessage = "This is required.")]
        public string PaymentMethod { get; set; }

        [DisplayName("Payment Channel")]
        [Required(ErrorMessage = "This is required.")]
        public string PaymentNetwork { get; set; }

        [DisplayName("Destination Account No")]
        [Required(ErrorMessage = "This is required.")]
        public string Destination { get; set; }

        //[DisplayName("Bank")]
        //public string Bank { get; set; }

        public string BankCode { get; set; }

        public IList<TblVendorpaymethod> BankAccountList { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "This is required.")]
        public string Name { get; set; }

        [DisplayName("with Purchase Order")]
        public bool Truefalse { get; set; }


        [DisplayName("Total Amount")]
        [Required(ErrorMessage = "This is required.")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, 999999999.00, ErrorMessage = "Price must be between $0.01 and 999,999,999.00.00")]
        public decimal TAmount { get; set; }

        public string forapproval { get; set; }
        public string lblAuth { get; set; }
        public bool isverify { get; set; }

        public IEnumerable<RefStatus> StatusList { get; set; }

        [DisplayName("Reason Type")]
        public string Statuscode { get; set; }

        [DisplayName("Remarks")]
        public string StatusRemarks { get; set; }

        public IList<RefCompany> Companylist { get; set; }
        public IList<RefBranch> Branchlist { get; set; }

        public IList<tmpPaymentRequestInventory> tmpPaymentRequestInventory { get; set; }

        public string AuditUser { get; set; }

        public List<FileDetails> Files { get; set; }
            = new List<FileDetails>();

        public IList<qryPaymentRequestAuthDtl> RequestJourney { get; set; }
        public qryPaymentRequestHdr PaymentRequestHdr { get; set; }

        [DisplayName("Reference No")]
        public string RefNo { get; set; }

        public bool isCreate { get; set; } = false;
    }

    //public class FilesViewModel
    //{
    //    public List<FileDetails> Files { get; set; }
    //        = new List<FileDetails>();
    //}

    

}