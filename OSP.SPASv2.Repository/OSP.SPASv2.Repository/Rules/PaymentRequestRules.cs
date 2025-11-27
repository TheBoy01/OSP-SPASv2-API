using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using NuGet.Packaging;
using OSP.Common.Domain.View;
using OSP.SPASv2.Domain.Params;
using OSP.SPASv2.Repository.Parameters;
using OSP.SPASv2.Repository.Repository.MainRepository;
using SPASv2.Context;
using System.Formats.Asn1;
using System.Linq;
using System.Text;

namespace OSP.SPASv2.Repository.Rules
{
    public class PaymentRequestRules : IRules<RequisitionParams>
    {
        StringBuilder sb = new StringBuilder();

        RepositoryUnit _RepositoryUnit;

        public PaymentRequestRules(SPASv2Context _context)
        {
            _RepositoryUnit = new RepositoryUnit(_context);
        }

        public string CanCreate(RequisitionParams entity)
        {
            throw new NotImplementedException();
        }

        public string CanCreatetmp(RequisitionParams entity)
        {
            sb = new StringBuilder();
            //try
            //{
            //    if (entity.tmpPaymentRequestInventory.PRNo != "A")
            //    {
            //        sb.Append("Sample isexist.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            return sb.ToString();

        }

        public string CanDelete(RequisitionParams entity)
        {
            sb = new StringBuilder();
            //try
            //{
            //    if (entity.tmpPaymentRequestInventory.PRNo != "A")
            //    {
            //        sb.Append("Sample isexist.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            return sb.ToString();
        }

        public string CanRead(RequisitionParams entity)
        {
            throw new NotImplementedException();
        }

        public string CanUpdate(RequisitionParams entity)
        {
            throw new NotImplementedException();
        }

        public string CanEndorse(RequisitionParams entity)
        {
            sb = new StringBuilder();
            //try
            //{
            //    if (entity.TblPaymentrequesthdr.PayMethodType != "CHEQUE")
            //    {
            //        if (string.IsNullOrEmpty(entity.BankCode) || string.IsNullOrWhiteSpace(entity.BankCode))
            //        {
            //            sb.Append("BankCode is not valid.");
            //        }
            //    }
            //    if (!entity.IsClassIdExist)
            //    {
            //        sb.Append("Class ID is not valid.");
            //    }
            //    if (!entity.IsCOADeptExist)
            //    {
            //        sb.Append("Department Code is not valid.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}

            return sb.ToString();

        }


        public async Task<string> CanConfirm(RequisitionParams entity)
        {

            sb = new StringBuilder();
            //try
            //{

            //    if (entity.TblPaymentRequestAuth != null)
            //    {

            //        if (entity.TblPaymentRequestAuth.PersonID != "REQUESTER-VAL")
            //        {

            //            sb.Append("Invalid to confirm payment. "+ entity.TblPaymentRequestAuth.AuthorizeClass +" need to process first.");
            //        }

            //    }
            return await Task.FromResult(sb.ToString());
            //}
            //catch (Exception ex)
            //{

            //    throw new Exception(ex.Message);
            //}



        }

        public Task<string> CanReadAsync(RequisitionParams entity)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CanDeleteAsync(RequisitionParams entity)
        {
            sb = new StringBuilder();
            try
            {

                var paymentauth = await _RepositoryUnit.PRAuthorizationRepository.GetLatestAuthoriztionByAuthorizeLevel(entity.ReqNo);

                if (paymentauth.AuthorizeLevel > 1)
                {

                    sb.AppendLine("" + entity.ReqNo + " has been verified/approved.");
                }
                var tblreqhdr = await _RepositoryUnit.TblRequisitionHdrRepository.ReadRequisitionHdr(entity.ReqNo);
                if (tblreqhdr != null)
                {
                    if (!tblreqhdr.AuditUser.Equals(entity.UserID))
                    {
                        var AuthGroup = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizeGroupPersonIDandPayclass(tblreqhdr.AuditUser, tblreqhdr.PayClassCode);
                        if (!AuthGroup.Select(b => b.PersonID).Contains(entity.UserID))
                        {
                            sb.AppendLine("" + entity.ReqNo + " cannot be voided as you are not currently part of the " + AuthGroup.Select(a => a.GroupDesc).FirstOrDefault() + " group.");

                        }
                        //var UserID = await _RepositoryUnit.PRAuthorizationRepository.GetAuthorizer(entity.UserID);
                        //if (UserID.Where(a => a.GroupId == "LOGLOCENC").Select(b => b.PersonId).Contains(entity.UserID))
                        //{
                        //    sb.AppendLine("" + entity.ReqNo + " has been verified/approved.");
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return sb.ToString();
        }

        public async Task<string> CanCreateDistribution(List<TblAssignedtoVendor_CMS> TblAssignedtoVendor_CMSList, IList<qryCompanyDetails> qryCompDtlList, List<qryRequisition> qryRequisitionList)
        {
            sb = new StringBuilder();
            List<string> ErrList = new List<string>(); 
            try
            {
                List<string> _DistinctCompanies = new List<string>();
                List<string> _DistinctDeptCodes = new List<string>();

                _DistinctCompanies = TblAssignedtoVendor_CMSList.Select(a => a.CompanyCode).Where(a => !string.IsNullOrEmpty(a)).Distinct().ToList();
                _DistinctDeptCodes = TblAssignedtoVendor_CMSList.Select(a => a.DeptCode.ToUpper()).Distinct().ToList();
                var VendorList = await _RepositoryUnit.VendorRepository.GetVendorList();

                foreach (string item in _DistinctCompanies)
                {
                    if (await _RepositoryUnit.VendorRepository.GetVendorDetails(item, "") == null)
                    {
                        ErrList.Add(item);
                    }
                }

                ErrList.AddRange(_DistinctCompanies.Except(VendorList.Select(a => a.VendorCode).ToList()).ToList());

                ErrList.AddRange(_DistinctDeptCodes.Except(qryCompDtlList.Select(a => a.DeptCode).ToList()).ToList());

                if (ErrList.Count > 0) { sb.AppendLine("Department not found " + string.Join(",", ErrList.Distinct().ToList()) + ". \n"); ErrList = new List<string>(); }

                ErrList.AddRange(qryRequisitionList.Where(item => item.Price <= 0).Select(a => a.ItemDesc).ToList());
                if (ErrList.Count > 0) { sb.AppendLine("Invalid items with zero amount found: " + string.Join(",", ErrList.Distinct().ToList()) + ". \n"); ErrList = new List<string>(); }

                var GrpByItems = TblAssignedtoVendor_CMSList.GroupBy(a => new { a.CompanyCode,a.DeptCode, a.ItemCode }).Select(a => new TblAssignedtoVendor_CMS { VendorCode = a.Key.CompanyCode, ItemCode = a.Key.ItemCode }).Where(a => !string.IsNullOrEmpty(a.ItemCode) && !string.IsNullOrEmpty(a.VendorCode) ).ToList();
                foreach (var item in GrpByItems)
                {
                    if (await _RepositoryUnit.VendorItemsRepository.GetVendorItemsDetails(item.VendorCode, item.ItemCode) == null)
                    {
                        ErrList.Add(item.VendorCode + " itemcode: " + item.ItemCode);
                    }
                }

                if (ErrList.Count > 0) { sb.AppendLine("Invalid items not found in vendor: " + string.Join(",", ErrList.Distinct().ToList()) + ". \n"); ErrList = new List<string>(); }

                //ErrList.AddRange(qryCompDtlList.Where(a => a.DeptCode.Equals() ));
                //var Result = distributionParams.tmpAssignedtoVendor_CMS
                //        .GroupBy(a => new { a.DeptCode })
                //        .Select(a => new tmpAssignedtoVendor_CMS { DeptCode = a.Key.DeptCode })
                //        .ToList();
                ////var a = qryCompDtlList.Select(a => a.DeptCode).ToList();

                //var notInList2 = Result.Where(item => qryCompDtlList.Select(a => a.DeptCode).ToList().Contains(item.DeptCode)).ToList();
                //ErrList.AddRange(notInList2.Select(a => a.DeptCode).ToList());
                //if (ErrList.Count > 0) { sb.AppendLine("Company not found in the Department(s) of " + string.Join(",", ErrList.Distinct()) + "."); }

                //ErrList = new List<string>();
                //ErrList.AddRange(qryRequisitionList.Where(item => item.Price <= 0).Select(a => a.ItemDesc).ToList()); 
                //if (ErrList.Count > 0) { sb.AppendLine("Invalid items with zero amount found: " + ErrList.Distinct()); }

                //if (_qryRequisitionList.Exists(q => q.Equals(_qryRequisition)))
                //{
                //    throw new Exception("dup");
                //}
                //foreach (var item in distributionParams.tmpAssignedtoVendor_CMS)
                //{
                //    if (string.IsNullOrEmpty(qryCompDtlList.Where(a => a.DeptCode.Equals(item.VendorCode)).Select(a => a.CompanyCode).FirstOrDefault()))
                //    {
                //        ErrList.Add(item.DeptCode);
                //        sb.AppendLine("Company not found: " + string.Join(",", qryCompDtlList.Where(a => a.DeptCode.Equals(item.VendorCode) && a.CompanyType.Equals("FACTORY")).Select(a => a.CompanyCode).ToList()));
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return await Task.FromResult(sb.ToString());
        }

        public async Task<string> CanUpdateRequisition(qryUpdateReqDetails qryUpdateReqDetails)
        {
            sb = new StringBuilder();
            List<string> ErrList = new List<string>();

            try
            {
                if (string.IsNullOrEmpty(qryUpdateReqDetails.SINo))
                {
                    ErrList.Add("SI No is required.");
                }

                if (await _RepositoryUnit.TblRequisitionHdrRepository.GetTotalAmount(qryUpdateReqDetails.ReqNo) != qryUpdateReqDetails.TotalAmount)
                {
                    ErrList.Add("Total Amount must be equal to Requisition Amount.");
                }

                if (ErrList.Count > 0)
                {
                    sb.AppendLine(string.Join("<br/>", ErrList.ToList()));
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return await Task.FromResult(sb.ToString());
        }
    }
}
