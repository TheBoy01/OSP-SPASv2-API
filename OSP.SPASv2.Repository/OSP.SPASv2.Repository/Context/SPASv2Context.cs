using Microsoft.EntityFrameworkCore;
using OSP.Common.Domain.View;
using OSP.SPASv2.Domain.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPASv2.Context
{
    public class SPASv2Context : DbContext
    {
        //public SPASv2Context()
        //{

        //}
        
        
        public SPASv2Context(DbContextOptions<SPASv2Context> options) : base(options)
        {

        }
        //public class ValReturn<T>
        //{
        //    public T Value { get; set; }
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ValReturn<bool>>().HasNoKey();
            modelBuilder.Entity<ValReturn<int>>().HasNoKey();
            modelBuilder.Entity<ValReturn<DateTime>>().HasNoKey();
            modelBuilder.Entity<ValReturn<string>>().HasNoKey();
            modelBuilder.Entity<ValReturn<decimal>>().HasNoKey();

            #region Tables
            modelBuilder.Entity<TblVendor>().HasKey(t => new { t.VendorCode });
            modelBuilder.Entity<TblVendorTIN>().HasNoKey();
            modelBuilder.Entity<TblVendorpaymethod>().HasKey(t => new { t.BankCode });
            modelBuilder.Entity<TblVendorItems>().HasKey(t => new { t.VendorCode,t.ItemCode });
            modelBuilder.Entity<TblPRAuthorization>().HasKey(t => new { t.PRNo, t.PersonID });
            modelBuilder.Entity<tmpPaymentRequestInventory>().HasKey( t => new { t.PRNo, t.ItemCode });
            modelBuilder.Entity<TblBatchApproval>().HasKey(t => new { t.BANo, t.ReqNo });
            modelBuilder.Entity<TblPaymentrequesthdr>().HasKey(t => new { t.PRNo });
            modelBuilder.Entity<TblVendorAddress>().HasKey(t => new { t.idx });
            modelBuilder.Entity<TblBatchPRHdr>().HasKey(t => new { t.BatchPRNo });
            modelBuilder.Entity<TblBatchPRDtl>().HasKey(t => new { t.BatchPRNo, t.PRNo });
            modelBuilder.Entity<TblRequisitionhdr>().HasKey(t => new { t.Reqno });
            modelBuilder.Entity<TblRequisitionhdr>().ToTable("TblRequisitionhdr");
            modelBuilder.Entity<TblRequisitiondtl>().HasKey(t => new { t.ReqItemNo }); 

            modelBuilder.Entity<TblResponse>().HasKey(t => new { t.TrxNo });
            modelBuilder.Entity<DummyStr>().HasNoKey();
            modelBuilder.Entity<TblPaymentRequestAuth>().HasKey(t => new { t.Reqno, t.PersonID });
            modelBuilder.Entity<RefVendorDocs>().HasKey(t => new { t.DocCode });
            modelBuilder.Entity<TblPurchaseorderhdr>().HasKey(t => new { t.PONo });

            modelBuilder.Entity<TblAuthorizerGroup>().HasKey(t => new { t.GroupId, t.PersonId });
            modelBuilder.Entity<TblRequisitionReason>().HasKey(t => new { t.ReqNo, t.ReasonCode });
            modelBuilder.Entity<TblVendorAdapter>().HasNoKey();
            modelBuilder.Entity<RefCompanyAdapter>().HasNoKey();//HasKey(t => new { t.CompanyCode });
            modelBuilder.Entity<RefAccountMap>().HasKey(t => new { t.Idx });
            modelBuilder.Entity<RefReportType>().HasKey(t => new { t.ReportID });
            modelBuilder.Entity<RefReportname>().HasKey(t => new { t.ReportNameID });

            modelBuilder.Entity<TblRequisitionDtlSummary>().HasKey(t => new { t.ReqNoDept });
            modelBuilder.Entity<TblDRNo>().HasKey(t => new { t.Idx });
            modelBuilder.Entity<TblItemBarcodes>().HasKey(t => new { t.BarCode });
            modelBuilder.Entity<TblAssignedtoVendor_CMS>().HasKey(t => new { t.OrderNo,t.VendorCode,t.DeptCode,t.ItemCode });
            modelBuilder.Entity<TblVendorPayClass>().HasKey(t => new { t.VendorCode, t.PayClassCode});
            //modelBuilder.Entity<>

            //modelBuilder.Entity<TblRequisitionhdr>().Property(x => x.NetofVat).HasColumnType("decimal(13,4)");

            #endregion

            #region References 
            modelBuilder.Entity<RefRegion>().HasKey(t => new { t.RegionCode });
            modelBuilder.Entity<RefProvince>().HasKey(t => new { t.ProvinceCode });
            modelBuilder.Entity<RefCity>().HasKey(t => new { t.CityCode });
            modelBuilder.Entity<RefBrgy>().HasKey(t => new { t.Idx });

            modelBuilder.Entity<RefBranch>().HasKey(t => new { t.BranchCode });
            modelBuilder.Entity<RefCompany>().HasKey(t => new { t.CompanyCode });
            modelBuilder.Entity<RefVendorType>().HasKey(t => new { t.VendorTypeCode });
            modelBuilder.Entity<RefChapel>().HasKey(t => new { t.ChapelCode });
            modelBuilder.Entity<RefPaymentClass>().HasKey(t => new { t.PayClassCode });
            
            
            modelBuilder.Entity<RefAddressType>().HasKey(t => new { t.AddressTypeCode });
            modelBuilder.Entity<RefTrxweek>().HasKey(t => new { t.TrxMonth,t.WeekNo });
            modelBuilder.Entity<RefDiscount>().HasKey(t => new { t.DiscountCode });
            modelBuilder.Entity<RefVat>().HasKey(t => new { t.Vatcode });
            modelBuilder.Entity<RefBankAcctType>().HasKey(t => new { t.AcctTypeCode });
            modelBuilder.Entity<RefATC>().HasKey(t => new { t.ATCCode });
            modelBuilder.Entity<RefATCType>().HasKey(t => new { t.ATCType });
            modelBuilder.Entity<RefBank>().HasKey(t => new { t.BankCode });
            modelBuilder.Entity<RefSystems>().HasKey(t => new { t.SystemCode });
            modelBuilder.Entity<RefItems>().HasKey(t => new { t.ItemCode });
            modelBuilder.Entity<RefChapelBranch>().HasKey(t => new { t.ChapelCode });

            modelBuilder.Entity<TblRequisitionhdr>().HasKey(t => new { t.Reqno });
            modelBuilder.Entity<RefChapelEmail>().HasKey(t => new { t.ChapelCode });


        #endregion

            #region Views
           
            modelBuilder.Entity<qryBranch>().HasKey(t => new { t.Branchcode });
            modelBuilder.Entity<qryVendorList>().HasKey(t => new { t.VendorCode });
            modelBuilder.Entity<qryPRAuthorizationList>().HasKey(t => new { t.Reqno });
            modelBuilder.Entity<qryCompanyType>().HasKey(t => new { t.CompanyType });
            modelBuilder.Entity<qryVendorDetails>().HasKey(t => new { t.VendorCode });
            modelBuilder.Entity<qryListOfAuthorizerPayclass>().HasKey(t => new { t.PersonID,t.PayClassDesc });
            modelBuilder.Entity<qryPaymentRequestHdr>().HasKey(t => new { t.PRNo });

            modelBuilder.Entity<qryPaymentRequestAuthDtl>().HasNoKey();
            modelBuilder.Entity<qryRequestPaymentRequestbyStatus>().HasNoKey();
            modelBuilder.Entity<qryVendorContact>().HasNoKey();
            modelBuilder.Entity<qryRequisitionInfo>().HasNoKey();
            modelBuilder.Entity<qryRequisitionItem>().HasNoKey();
            modelBuilder.Entity<qryGroupEmails>().HasNoKey();
            modelBuilder.Entity<qryRptPurchaseOrderDetails>().HasNoKey();
            modelBuilder.Entity<qryRptPurchaseOrderConsolidated>().HasNoKey();
            modelBuilder.Entity<qryRptPurchaseOrderHdr>().HasNoKey();
            modelBuilder.Entity<qryPaymentClassAuthorization>().HasNoKey();
            modelBuilder.Entity<qryPOSignatories>().HasNoKey();
            modelBuilder.Entity<qryRptTransmittalFO>().HasNoKey();
            modelBuilder.Entity<qryRptChapelAdvisory>().HasNoKey();
            modelBuilder.Entity<qryRptChapelAdvisory_GCM>().HasNoKey();
            modelBuilder.Entity<qryChapelBranchDetails>().HasNoKey();
            modelBuilder.Entity<qrySignatoriesChapelAdvisory>().HasNoKey();
            modelBuilder.Entity<qryDeclineReason>().HasNoKey();
            modelBuilder.Entity<TblPaymentrequisitionhdr>().HasKey(t => new { t.PRno });
            modelBuilder.Entity<RptPurchaseorder>().HasKey(t => new { t.PONo,t.Description,t.Department });
            modelBuilder.Entity<qryVendorRunningBalance>().HasNoKey();
            modelBuilder.Entity<qryAuthorizerGroup>().HasNoKey();
            modelBuilder.Entity<qryActiveRequisition>().HasNoKey();
            modelBuilder.Entity<qryRequisitionDepartment>().HasNoKey();
            modelBuilder.Entity<qryPOHdr>().HasNoKey();
            modelBuilder.Entity<qryPOBarcodesSummary>().HasNoKey();
            modelBuilder.Entity<qryPOBarcodes>().HasNoKey();
            #endregion



        }




 


        #region Tables

        public DbSet<TblRequisitionDtlSummary> TblRequisitionDtlSummary { get; set; }
        public DbSet<TblBatchPRHdr> TblBatchPRHdr { get; set; }
        public DbSet<TblBatchPRDtl> TblBatchPRDtl { get; set; }
        public DbSet<TblVendorAddress> TblVendorAddress { get; set; }
        public DbSet<TblVendor> TblVendor { get; set; }
        public DbSet<TblVendorTIN> TblVendorTIN { get; set; }
        public DbSet<TblResponse> TblResponse { get; set; }
        public DbSet<TblVendorItems> TblVendorItems { get; set; }
        public DbSet<TblPRAuthorization> TblPRAuthorization { get; set; }
        public DbSet<tmpPaymentRequestInventory> tmpPaymentRequestInventory { get; set; }
        public DbSet<TblPaymentrequesthdr> TblPaymentrequesthdr { get; set; }

        public DbSet<TblBatchApproval> TblBatchApproval { get; set; }
        public DbSet<TblVendorpaymethod> TblVendorpaymethod { get; set; }
        public DbSet<DummyStr> DummyStr { get; set; }
        public DbSet<TblPaymentRequestAuth> TblPaymentRequestAuth { get; set; }
        public DbSet<RefVendorDocs> RefVendorDocs { get; set; }
        public DbSet<TblPaymentrequestdtl> TblPaymentrequestdtl { get; set; }
        public DbSet<TblRequisitionhdr> TblRequisitionhdr { get; set; }
        public DbSet<TblRequisitiondtl> TblRequisitiondtl { get; set; }
        public DbSet<TblPurchaseorderhdr> TblPurchaseOrderHdr { get; set; }
        public DbSet<TblRequisitionReason> TblRequisitionReason { get; set; }
        public DbSet<TblAuthorizerGroup> TblAuthorizerGroup { get; set; }
        public DbSet<TblVendorAdapter> TblVendorAdapter { get; set; }
        public DbSet<TblPaymentrequisitionhdr> TblPaymentrequisitionhdr { get; set; }
        public DbSet<TblDRNo> TblDRNo { get; set; }
        public DbSet<TblItemBarcodes> TblItemBarcodes { get; set; }
        public DbSet<TblAssignedtoVendor_CMS> TblAssignedtoVendor_CMS { get; set; }
        public DbSet<TblVendorPayClass> TblVendorPayClass { get; set; }
        #endregion

        #region References
        public DbSet<RefAccountMap> RefAccountMap { get; set; }
        public DbSet<RefCompanyAdapter> RefCompanyAdapter { get; set; }
        public DbSet<RefRegion> RefRegion { get; set; }


        public DbSet<RefProvince> RefProvince { get; set; }
        public DbSet<RefCity> RefCity { get; set; }
        public DbSet<RefBrgy> RefBrgy { get; set; }

        public DbSet<RefBranch> RefBranch { get; set; }
        public DbSet<RefCompany> RefCompany { get; set; }
        public DbSet<RefChapel> RefChapel { get; set; }
        public DbSet<RefPaymentClass> RefPaymentClass { get; set; }
        public DbSet<RefVendorType> RefVendorType { get; set; }
        public DbSet<RefAddressType> RefAddressType { get; set; }
        public DbSet<RefTrxweek> RefTrxweek { get; set; }
        public DbSet<RefDiscount> RefDiscount { get; set; }
        public DbSet<RefVat> RefVat { get; set; }
        public DbSet<RefBankAcctType> RefBankAcctType { get; set; }
        public DbSet<RefATC> RefATC { get; set; }
        public DbSet<RefATCType> RefATCType { get; set; }
        public DbSet<RefBank> RefBank { get; set; }

        public DbSet<RefSystems> RefSystems { get; set; }
        public DbSet<RefItems> RefItems { get; set; }
        public DbSet<RefReportType> RefReportType { get; set; }
        public DbSet<RefReportname> RefReportname { get; set; }
        public DbSet<RefChapelBranch> RefChapelBranch { get; set; }
        public DbSet<RefChapelEmail> RefChapelEmail { get; set; }

        #endregion

        #region Views
        public DbSet<qryGroupEmails> qryGroupEmails { get; set; }

        public DbSet<qrySignatoriesChapelAdvisory> qrySignatoriesChapelAdvisory { get; set; }
        public DbSet<qryRptPurchaseOrderDetails> qryRptPurchaseOrderDetails { get; set; }
        public DbSet<qryDeclineReason> qryDeclineReason { get; set; }
        public DbSet<qryRptPurchaseOrderConsolidated> qryRptPurchaseOrderConsolidated { get; set; }
        public DbSet<qryRptChapelAdvisory> qryRptChapelAdvisory { get; set; }
        public DbSet<qryRptChapelAdvisory_GCM> qryRptChapelAdvisory_GCM { get; set; }
        public DbSet<qryRptTransmittalFO> qryRptTransmittalFO { get; set; }
        public DbSet<qryChapelBranchDetails> qryChapelBranchDetails { get; set; }
        public DbSet<qryRptPurchaseOrderHdr> qryRptPurchaseOrderHdr { get; set; }
        public DbSet<qryPRAuthorizationList> qryPRAuthorizationList { get; set; }
        public DbSet<qryListOfAuthorizerPayclass> qryListOfAuthorizerPayclass { get; set; }
        public DbSet<qryBranch> qryBranch { get; set; }
        public DbSet<qryVendorList> qryVendorList { get; set; }
        public DbSet<qryCompanyType> qryCompanyType { get; set; }
        public DbSet<qryVendorDetails> qryVendorDetails { get; set; }
        public DbSet<qryPaymentRequestHdr> qryPaymentRequestHdr { get; set; }
        public DbSet<qryPaymentRequestAuthDtl> qryPaymentRequestAuthDtl { get; set; }
        public DbSet<qryRequestPaymentRequestbyStatus> qryRequestPaymentRequestbyStatus { get; set; } 
        public DbSet<qryVendorContact> qryVendorContact { get; set; }
        public DbSet<qryRequisitionInfo> qryRequisitionInfo { get; set; }
        public DbSet<qryRequisitionItem> qryRequisitionItem { get; set; }

        public DbSet<qryPaymentClassAuthorization> qryPaymentClassAuthorization { get; set; }

        public DbSet<qryPOSignatories> qryPOSignatories { get; set; }

        public DbSet<qryVendorRunningBalance> qryVendorRunningBalance { get; set; }
        public DbSet<qryAuthorizerGroup> qryAuthorizerGroup { get; set; }
        public DbSet<qryActiveRequisition> qryActiveRequisition { get; set; }
        public DbSet<qryRequisitionDepartment> qryRequisitionDepartment { get; set; }
        public DbSet<qryPOHdr> qryPOHdr { get; set; }
        public DbSet<qryPOBarcodesSummary> qryPOBarcodesSummary { get; set; }
        public DbSet<qryPOBarcodes> qryPOBarcodes { get; set; }
        #endregion

        #region Reports

        public DbSet<RptPurchaseorder> RptPurchaseorder { get; set; }
        #endregion

        internal IEnumerable<object> GetValidationErrors()
        {
            throw new NotImplementedException();
        }
    }
}