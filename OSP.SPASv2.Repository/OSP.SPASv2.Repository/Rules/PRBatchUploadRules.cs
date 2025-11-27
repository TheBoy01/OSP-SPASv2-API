using OSP.SPASv2.Repository.Parameters;
using OSP.SPASv2.Repository.Repository.MainRepository;
using SPASv2.Context;
using System.Linq;
using System.Text;
using static System.Data.Odbc.ODBC32;

namespace OSP.SPASv2.Repository.Rules
{
    public class CompanyDepartment
    {
        public string CompanyType { get; set; }
        public string DeptCode { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is CompanyDepartment other)
            {
                return CompanyType == other.CompanyType && DeptCode == other.DeptCode;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CompanyType, DeptCode);
        }
    }

    public class CompanyDepartmentComparer : IEqualityComparer<CompanyDepartment>
    {
        public bool Equals(CompanyDepartment x, CompanyDepartment y)
        {
            if (x == null || y == null)
                return false;

            return x.CompanyType == y.CompanyType && x.DeptCode == y.DeptCode;
        }

        public int GetHashCode(CompanyDepartment obj)
        {
            if (obj == null)
                return 0;

            return HashCode.Combine(obj.CompanyType, obj.DeptCode);
        }
    }

    public class PRBatchUploadRules : IRules<PRBatchUploadParams>
    {
        StringBuilder sb = new StringBuilder();
        RepositoryUnit _RepositoryUnit;

        public PRBatchUploadRules(SPASv2Context _context)
        {
            _RepositoryUnit = new RepositoryUnit(_context);
        }

        public string CanCreateBatchHdr(PRBatchUploadParams entity)
        {
            string _ReturnMessage = string.Empty;

            //if ()
            //{

            //}

            return _ReturnMessage;
        }

        public string CanCreate(PRBatchUploadParams entity)
        {
            throw new NotImplementedException();
        }

        public string CanDelete(PRBatchUploadParams entity)
        {
            throw new NotImplementedException();
        }



        public async Task<string> CanRead(PRBatchUploadParams entity)
        {
            sb = new StringBuilder();
            List<string> vendornamelist = new List<string>();
            List<string> vendoritemnamelist = new List<string>();
            List<string> ErrorList = new List<string>();
            try
            {
                var payclass = await _RepositoryUnit.RefPaymentClassRepository.Read(entity._batchUploadParams.Payclass);
                if (payclass == null)
                {
                    sb.AppendLine("" + entity._batchUploadParams.Payclass + " is not existing. <br />");
                }

                for (int i = 0; i < entity._batchUploadParams.qryBatchRequistions.Count; i++)
                {
                    var vendorname = entity._batchUploadParams.qryBatchRequistions[i].VendorName;
                    var vendoritemname = entity._batchUploadParams.qryBatchRequistions[i].ItemDesc;
                    var vendorcode = await _RepositoryUnit.VendorRepository.GetVendorCodeByDisplayName(vendorname.Replace("'", "`"));
                    //var vendor = await _RepositoryUnit.VendorRepository.Read(vendorcode);
                    var vendoritem = await _RepositoryUnit.VendorItemsRepository.GetvendorItemAsync(vendorcode, vendoritemname);
                    if (!vendornamelist.Contains(vendorname))
                    {
                        if (string.IsNullOrEmpty(vendorcode))
                        {
                            vendornamelist.Add(vendorname);
                            sb.AppendLine("" + vendorname.ToUpper() + " is not existing. <br />");
                        }
                        else
                        {
                            if (vendoritem is null)
                            {
                                if (!vendornamelist.Contains(vendoritemname))
                                {
                                    vendornamelist.Add(vendoritemname);
                                    sb.AppendLine("" + vendorname + " with item " + vendoritemname.ToUpper() + " is not existing. <br />");
                                }
                            }
                        }
                    }

                    //Check if has double items
                    var CountItem = entity._batchUploadParams.qryBatchRequistions.Where(a => 
                                                                                        a.ItemDesc.ToUpper().TrimStart().TrimEnd().Equals(entity._batchUploadParams.qryBatchRequistions[i].ItemDesc.ToUpper().TrimStart().TrimEnd()) 
                                                                                        && a.VendorName.ToUpper().TrimStart().TrimEnd().Equals(entity._batchUploadParams.qryBatchRequistions[i].VendorName.ToUpper().TrimStart().TrimEnd())
                                                                                        && a.Department.ToUpper().TrimStart().TrimEnd().Equals(entity._batchUploadParams.qryBatchRequistions[i].Department.ToUpper().TrimStart().TrimEnd())).ToList();

                    if (CountItem.Count > 1)
                    {
                        ErrorList.Add(CountItem.Select(a => a.ItemDesc).FirstOrDefault().ToString().ToUpper());
                       
                    }

                    //paymentRequestParams.VendorItem.Add(vendoritem);
                }

                ErrorList = ErrorList.Distinct().ToList();

                if (ErrorList.Count > 0)
                {
                    sb.Append("Double encoded on item: " + string.Join(",", ErrorList) + ". <br />");
                }

                //var list1 = entity._batchUploadParams.qryBatchRequistions
                //.GroupBy(p => new { p.CompanyType, p.Department })
                //.Select(g => new { g.Key.CompanyType, DeptCode = g.Key.Department }).ToList();

                //var list2 = entity.qryCompanyDetailsList.Select(p => new { p.CompanyType, p.DeptCode }).ToList();

                var list1 = entity._batchUploadParams.qryBatchRequistions
                .GroupBy(p => new { p.CompanyType, p.Department })
                .Select(g => new CompanyDepartment
                {
                    CompanyType = g.Key.CompanyType,
                    DeptCode = g.Key.Department
                })
                .ToList();

                var list2 = entity.qryCompanyDetailsList
                                .Select(p => new CompanyDepartment
                                {
                                    CompanyType = p.CompanyType,
                                    DeptCode = p.DeptCode
                                })
                                .ToList();
                //sb.AppendLine("1.");

                var notInList1 = list1.Except(list2, new CompanyDepartmentComparer()).ToList();
                //sb.AppendLine("2.");
                //Console.WriteLine("Elements in list2 that are not in list1:");
                foreach (var item in notInList1)
                {
                    // Console.WriteLine($"Id: {item.DeptCode}, Name: {item.CompanyType}");
                    sb.AppendLine($"Id: {item.DeptCode}, Name: {item.CompanyType}");
                }

                //if (entity.tmpPaymentRequestInventory.PRNo != "A")
                //{
                //    sb.Append("Sample isexist.");
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return sb.ToString();
        }

        public string CanUpdate(PRBatchUploadParams entity)
        {
            throw new NotImplementedException();
        }

        string IRules<PRBatchUploadParams>.CanRead(PRBatchUploadParams entity)
        {
            throw new NotImplementedException();
        }

        public Task<string> CanReadAsync(PRBatchUploadParams entity)
        {
            throw new NotImplementedException();
        }

        public Task<string> CanDeleteAsync(PRBatchUploadParams entity)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CanUploadPayment(PRBatchUploadParams entity)
        {
            sb = new StringBuilder();
            List<string> vendornamelist = new List<string>();
            List<string> vendoritemnamelist = new List<string>();
            List<string> ErrorList = new List<string>();
            List<string> SIList = new List<string>();
            TblVendor tblVendor = new TblVendor();

            try
            {
                //Checking payment vs encoded in PO
                ErrorList = new List<string>();
                foreach (var item in entity._batchUploadParams.qryBatchPaymentDtlList)
                {
                    string ItemCode = string.Empty;
                    TblPurchaseorderhdr POhdr = new TblPurchaseorderhdr();
                    TblRequisitionhdr TblRequisitionhdr = new TblRequisitionhdr();
                    TblRequisitiondtl TblRequisitiondtl = new TblRequisitiondtl();

                    ItemCode = await _RepositoryUnit.RefItemsRepository.GetItemCodeByDesc(item.ItemDescription);
                    POhdr = await _RepositoryUnit.TblPurchaseorderhdrRepository.GetPOHdrByPONo(item.PONo);
                    TblRequisitionhdr = await _RepositoryUnit.TblRequisitionHdrRepository.ReadRequisitionHdr(POhdr.Reqno);
                    TblRequisitiondtl = await _RepositoryUnit.TblRequisitionDtlRepository.ReadRequisitionDtl(TblRequisitionhdr.Reqno, TblRequisitionhdr.DtlCompanyCode, item.Department.Split("-")[1], ItemCode);

                    if (string.IsNullOrEmpty(ItemCode))
                    {
                        ErrorList.Add("Item not found. Item: " + item.ItemDescription + ".</br>");
                        continue;
                    }
                    if (POhdr == null)
                    {
                        ErrorList.Add("PO not found. PO: " + item.ItemDescription + ".</br>");
                        continue;
                    }

                    if (TblRequisitiondtl == null)
                    {
                        ErrorList.Add("Item is not listed on the PO. PO: " + item.PONo+ "; Item: " + item.ItemDescription + ". </br>");
                        continue;
                    }

                    if (!TblRequisitionhdr.Active || TblRequisitionhdr.Void)
                    {
                        ErrorList.Add("Voided PO found. PO: " + POhdr.PONo + ".</br>");
                    }

                    if (await _RepositoryUnit.TblRequisitionDtlRepository.SumPOQuantity(TblRequisitionhdr.MainReqNo, TblRequisitionhdr.DtlCompanyCode, item.Department.Split("-")[1], ItemCode) > TblRequisitiondtl.Quantity)
                    {
                        ErrorList.Add("Quantity exceeded in the quantity total of PO: " + POhdr.PONo + " and Item: " + item.ItemDescription + ".</br>");
                    }
                    
                    if (await _RepositoryUnit.TblPaymentRequestAuthRepository.GetDeniedCount(TblRequisitionhdr.MainReqNo) > 0)
                    {
                        ErrorList.Add("Denied PO found. PO " + POhdr.PONo + "; Item: " + item.ItemDescription + ".</br>");
                    }

                    //if (TblRequisitiondtl.Price != item.TemPriceAmount)
                    //{
                    //    ErrorList.Add("Item has invalid price amount. Item price should be: " + TblRequisitiondtl.Price +". PO " + POhdr.PONo + "; Item: " + item.ItemDescription + ". </br>");
                    //}

                    //var CheckItems = entity._batchUploadParams.qryBatchPaymentDtlList.GroupBy(a => new { a.SalesInvoice, a.ItemDescription, a.PONo }).Where(a => a.Key.ItemDescription.Equals(item.ItemDescription) && a.Key.PONo.Equals(item.PONo)).Select(h => h.Key).ToList();
                    ////ErrorList = CheckItems.Select(a => "PO: " + a.PONo + " SI: " + a.SalesInvoice + " Item: " + a.ItemDescription).ToList();
                    //if (CheckItems.Count > 1)
                    //{
                    //    ErrorList.AddRange(CheckItems.Select(a => "SI with same casket in - " + "PO: " + a.PONo + " SI: " + a.SalesInvoice + " Item: " + a.ItemDescription).ToList());//"SI with same casket in - " + string.Join("</br>", ErrorList) + "</br>");
                    //}

                    var CheckitemPrice = entity._batchUploadParams.qryBatchPaymentDtlList.GroupBy(a => new { a.PONo, a.ItemDescription, a.TemPriceAmount }).Where(a => a.Key.ItemDescription.Equals(item.ItemDescription) && a.Key.PONo.Equals(item.PONo)).Select(h => h.Key).ToList();
                    if (CheckitemPrice.Count > 1)
                    {
                        ErrorList.AddRange(CheckitemPrice.Select(a => "Item " +  a.ItemDescription + " has a different price with amounting " + a.TemPriceAmount + ".</br>"));
                    }

                }

                ErrorList = ErrorList.Distinct().ToList();
                if (ErrorList.Count > 0)
                {
                    sb.Append("Unable to upload. </br>" + string.Join(" ", ErrorList));
                }

                ErrorList = new List<string>();
                ErrorList = await _RepositoryUnit.TblRequisitionHdrRepository.GetActiveReq(entity._batchUploadParams.qryBatchPaymentHdrList.Select(a => a.SalesInvoiceNo).ToList());
                ErrorList = ErrorList.Distinct().ToList();
                if (ErrorList.Count > 0)
                {
                    sb.AppendLine("Sales Invoice No. duplicate in: " + string.Join(", ", ErrorList) + "</br>");
                }
                //////
                ErrorList = new List<string>();
                //var ConsolidatedSI = entity._batchUploadParams.qryBatchPaymentDtlList.GroupBy(a => new { a.SalesInvoice}).ToList();
                //var ConsolidatedDR = entity._batchUploadParams.qryBatchPaymentDtlList.GroupBy(a => new { a.DeliveryNo }).ToList();
                //ErrorList = a.Select(bc => bc.PO).ToList(); 
                //ErrorList = a.Select(b => "DR: " + b.DeliveryNo + "; SI: " + b.SalesInvoice).ToList();

                foreach (var item in entity._batchUploadParams.qryBatchPaymentHdrList)
                {
                    //FOR DOUBLE DR
                    //var a = entity._batchUploadParams.qryBatchPaymentHdrList.Where(a => a.SalesInvoiceNo.Equals(item.SalesInvoiceNo)).Count();
                    //if (a > 1)
                    //{
                    //    var c = entity._batchUploadParams.qryBatchPaymentHdrList.Where(a => a.SalesInvoiceNo.Equals(item.SalesInvoiceNo)).Select(a => a.DeliveryNo).ToList();
                    //    ErrorList.Add("(Sales Invoice: " + item.SalesInvoiceNo + " with DR no.: " + string.Join(",", c) + ")");
                    //}

                    var b = entity._batchUploadParams.qryBatchPaymentHdrList.Where(a => a.DeliveryNo.Equals(item.DeliveryNo)).Count();
                    if (b > 1)
                    {
                        var bc = entity._batchUploadParams.qryBatchPaymentHdrList.Where(a => a.DeliveryNo.Equals(item.DeliveryNo)).Select(a => a.SalesInvoiceNo).ToList();
                        ErrorList.Add("(Delivery No: " + item.DeliveryNo + " with SI no.: " + string.Join(",", bc) + ")");
                    }

                    //deduction rules
                    //var aa = entity._batchUploadParams.qryBatchPaymentHdrList.Where(a => a.SalesInvoiceNo.Equals(item.SalesInvoiceNo)).Select(a => a.HPDeduction).Count();
                    //if (aa > 1)
                    //{
                    //    ErrorList.Add("Duplicate Entry of Deduction is invalid. SI No.: " + item.SalesInvoiceNo + "." );
                    //}
                }

                if (ErrorList.Count > 0)
                {
                    sb.AppendLine("Record conflict found. More than one Purchase Order Number has been detected for Sales Invoice/Delivery Receipt No. " + string.Join("</br>", ErrorList.Distinct()) +". Please review your data. </br>" );
                }

                //////
                ErrorList = new List<string>();
                var Items = entity._batchUploadParams.qryBatchPaymentDtlList.GroupBy(a => new { a.SalesInvoice, a.ItemDescription, a.PONo }).Where(g => g.Count() > 1).Select(h => h.Key).ToList();
                ErrorList = Items.Select(a => "PO: " + a.PONo + " SI: " + a.SalesInvoice + " Item: " + a.ItemDescription).ToList();
                if (ErrorList.Count > 0)
                {
                    sb.AppendLine("SI with same casket in - " + string.Join("</br>", ErrorList) + "</br>");
                }


                //var ab = entity._batchUploadParams.qryBatchPaymentDtlList.GroupBy(a => new { a.SalesInvoice, a.ItemDescription}).Select(b => new { SI = b.Key.SalesInvoice, Consolidated = b.Key.SalesInvoice + " Item: " + b.Key.ItemDescription }).ToList();
                //ErrorList = ab.Select(bc => bc.Consolidated).ToList();
                //if (ErrorList.Count != entity._batchUploadParams.qryBatchPaymentDtlList.Count())
                //{
                //    ErrorList = ab.GroupBy(t => t.Consolidated).Where(g => g.Count() > 1).Select(h => h.Key).ToList();
                //    ErrorList = a.Where(bc => ErrorList.Contains(bc.PO)).Select(t => t.POAndSI).ToList();

                //    sb.Append("<br/> SI with same casket has detected in - " + string.Join(", ", ErrorList));
                //}

            }
            //}
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return sb.ToString();
        }
    }
}
