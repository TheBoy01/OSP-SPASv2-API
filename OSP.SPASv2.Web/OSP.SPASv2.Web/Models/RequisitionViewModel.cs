using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SPASv2.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using OSP.Common.Domain.Msgbox;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Web.Models;

namespace SPASv2.Models
{
    public class ModelStateError
    {
        public string Key { get; set; }
        public string Message { get; set; }
    }

    public class FilterOption
    {
        public string FilterBy { get; set; }
        public string FilterVal { get; set; }
    }
    public class InsertRequisitionModel
    {
        [Required(ErrorMessage = "This is required.")]
        public string Supplier { get; set; }

        [DisplayName("Vendor Code")]
        [Required(ErrorMessage = "This is required.")]
        public string SupplierCode { get; set; }

        [DisplayName("Payee CV Name")]
        [Required(ErrorMessage = "This is required.")]
        public string PCVName { get; set; }

        [DisplayName("Payment Class")]
        [Required(ErrorMessage = "This is required.")]
        public string PaymentClass { get; set; }

        [DisplayName("Payment Method")]
        [Required(ErrorMessage = "This is required.")]
        public string PaymentMethod { get; set; }

        [DisplayName("Payment Channel")]
        [Required(ErrorMessage = "This is required.")]
        public string PaymentNetwork { get; set; }

        [DisplayName("Remarks")]
        [Required(ErrorMessage = "This is required.")]
        public string Remarks { get; set; }

        [DisplayName("Reference No")]
        [Required(ErrorMessage = "This is required.")]
        public string RefNo { get; set; }
        public List<qryRequisitionItem> RequisitionItemList { get; set; } = new List<qryRequisitionItem>();
        public IList<ModelStateError> ModelStateError { get; set; } = new List<ModelStateError>();
    }

    public class RequisitionViewModel
    {
        public RequisitionViewModel()
        {
            ListHidenColumns = new List<string>() { "Confirmation" };
        }

        [DisplayName("Payment Request Number")]
        public string PRNo { get; set; }

        [DisplayName("Request Date")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RequestDatetime { get; set; }

        [DisplayName("Request Date")]
        public string Requestdate { get; set; }

        [DisplayName("Sales Invoice Date")]
        public string InvoiceDate { get; set; }


        [DisplayName("Vendor Name")]
        //[Required(ErrorMessage = "This is required.")]
        public string Supplier { get; set; }

        [DisplayName("Vendor Code")]
        //[Required(ErrorMessage = "This is required.")]
        public string SupplierCode { get; set; }

        [DisplayName("Payee CV Name")]
        //[Required(ErrorMessage = "This is required.")]
        public string PCVName { get; set; }
        public string Address { get; set; }
        public string PONo { get; set; }

        [DisplayName("Department")]
        public string Code { get; set; }

        [DisplayName("Company Type")]
        public string Companycode { get; set; }

        [DisplayName("Delivery Receipt Number")]
        public string DeliveryReceiptNo { get; set; }


        [DisplayName("Product/Service Category")]
        public string ItemCategory { get; set; }


        [DisplayName("Product/Service")]
        public string ItemDesc { get; set; }

        [DisplayName("VAT")]
        public decimal VAT { get; set; }


        [DisplayName("Price per Unit")]
        public decimal ItemPrice { get; set; }


        [DisplayName("Net of VAT")]
        public decimal Net { get; set; }


        [DisplayName("TIN")]
        public string TIN { get; set; }


        [DisplayName("Remarks")]
        //[Required(ErrorMessage = "This is required.")]
        public string Remarks { get; set; }

        [DisplayName("Unit")]
        public string UOM { get; set; }


        [DisplayName("Quantity")]
        public int Quantity { get; set; }


        [DisplayName("Discount")]
        public string Discount { get; set; }

        [DisplayName("Sales Invoice")]
        public string SalesInvoiceLink { get; set; }

        [DisplayName("Sales Invoice No")]
        public string InvoiceNo { get; set; }

        [DisplayName("Payment Number")]
        public string PaymentNo { get; set; }


        [DisplayName("HO Remarks")]
        public string SPGRemarks { get; set; }

        [DisplayName("Payment Class")]
        //[Required(ErrorMessage = "This is required.")]
        public string PaymentClass { get; set; }

        [DisplayName("Payment Method")]
        //[Required(ErrorMessage = "This is required.")]
        public string PaymentMethod { get; set; }

        [DisplayName("Payment Channel")]
        //[Required(ErrorMessage = "This is required.")]
        public string PaymentNetwork { get; set; }

        [DisplayName("Destination Account No")]
        public string Destination { get; set; }

        [DisplayName("Reason Type")]
        public string Statuscode { get; set; }

        [DisplayName("Remarks")]
        public string StatusRemarks { get; set; }

        [DisplayName("Reference No")]
        //[Required(ErrorMessage = "This is required.")]
        public string RefNo { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("with Purchase Order")]
        public bool Truefalse { get; set; }


        [DisplayName("Total Amount")]

        public bool isModelValidated { get; set; } = false;
        public decimal TAmount { get; set; }

        public DashBoardViewModel dashboardViewModel { get; set; }
        public string value { get; set; }
        public string value1 { get; set; }
        public string value2 { get; set; }
        public string value3 { get; set; }
        public IList<qryRequestPaymentRequestbyStatus> RequisitionList { get; set; }
        public IList<qryRequestAttachments> RequestAttachments { get; set; }
        public bool ListCheck { get; set; } = false;
        public string ListCheckID { get; set; } = string.Empty;
        public List<string> ListHidenColumns { get; set; }

        public IList<qryVendorList> VendorList { get; set; }
        public string MainReqNo { get; set; }
        public string BranchCode { get; set; }
        public string Company { get; set; }


        public string Companyid { get; set; }
        public IList<qryCompanyType> CompanyList { get; set; }
        public IList<RefDepartment> DeptList { get; set; }
        public qryBranch qryBranch { get; set; }

        public string ItemCode { get; set; }



        public decimal Discountprice { get; set; }
        public IList<RefDiscount> Discountlist { get; set; }

        //public IEnumerable<qryPaymentRequestDtl> PaymentRequestdtl { get; set; }

        public IEnumerable<qryPaymode> PaymodeList { get; set; }

        public IList<RefPaymentClass> Paymenttypelist { get; set; }
        public string BankCode { get; set; }

        public IList<TblVendorpaymethod> BankAccountList { get; set; }

        public string forapproval { get; set; }
        public string lblAuth { get; set; }
        public bool isverify { get; set; }
        public bool CanVoid { get; set; } = false;
        public bool isDenied { get; set; } = false;

        public IEnumerable<RefStatus> StatusList { get; set; }



        public IList<RefCompany> Companylist { get; set; }
        public IList<RefBranch> Branchlist { get; set; }

        public IList<tmpPaymentRequestInventory> tmpPaymentRequestInventory { get; set; }

        public string AuditUser { get; set; }

        public List<FileDetails> Files { get; set; }
            = new List<FileDetails>();

        public IList<qryPaymentRequestAuthDtl> RequestJourney { get; set; }
        public qryPaymentRequestHdr PaymentRequestHdr { get; set; }
        public bool isCreate { get; set; } = false;
        public bool isCreateRequisition { get; set; } = false;
        public bool isCreatePayment { get; set; } = false;
        public bool isViewOnly { get; set; } = false;
        public bool showAddItemButton { get; set; } = false;
        public bool showItemBalance { get; set; } = false;
        public bool showDeleteDocs { get; set; } = false;

        public string ItemListName { get; set; } = string.Empty;
        public string SupDocsListName { get; set; } = string.Empty;
        public qryRequisitionInfo RequisitionInfo { get; set; } = new qryRequisitionInfo();
        public IList<qryRequisitionItem> RequisitionItemList { get; set; }
        public IList<qryRequisitionInfo> PaymentRequisitionInfoList { get; set; }
        public qryEmployee ReqEmpDetails { get; set; }
        public IList<TblVendorItems> VendorItemList { get; set; } = new List<TblVendorItems>();
        public bool isCreatePaymentBtn { get; set; } = false;
        public TblRequisitionhdr TblRequisitionhdr { get; set; } = new TblRequisitionhdr();
        public List<TblRequisitiondtl> RequisitionDtlList { get; set; } = new List<TblRequisitiondtl>();
        public List<IFormFile> FormFileList { get; set; } = new List<IFormFile>();

        #region PaymentRequisitionFields

        [Required]
        [StringLength(25)]
        public string SalesInvoiceNo { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime SalesInvoiceDate { get; set; }

        [Required]
        [StringLength(25)]
        public string DeliveryNo { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DeliveryDate { get; set; }
        public decimal Deduction { get; set; } = 0;
        public decimal Freight { get; set; } = 0;        
        IList<RequisitionItemChapelSummary> ItemSummary { get; set; }

        public IList<qryDeclineReason> lstReason { get; set; } = new List<qryDeclineReason>();


        #endregion
    }

    public class ReasonModalViewModel
    {
        public IList<qryDeclineReason> lstReason { get; set; } = new List<qryDeclineReason>();
        public string ModalName { get; set; }
        public string ModalTitle { get; set; }
        public string cboReasonId { get; set; }
        public string txtReasonRemarksId { get; set; }
        public string SuccessMethod { get; set; }
        public string Instruction { get; set; } = "Please enter reason details.";
    }

    public class UpdateDetailsModalViewModel
    {
        public string ModalName { get; set; }
        public string ModalTitle { get; set; }
        public string Instruction { get; set; } = "Please enter details.";
        public string SalesInvoiceNo { get; set; }
        public string SalesInvoiceDate { get; set; }
        public string Deduction { get; set; }
        public string TotalAmount { get; set; }
        public string Freight { get; set; }
        public FileDetails FileDetails { get; set; }
        public string SuccessMethod { get; set; }
    }

    public class FileDetails
    {
        public string ReqNo { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Src { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
