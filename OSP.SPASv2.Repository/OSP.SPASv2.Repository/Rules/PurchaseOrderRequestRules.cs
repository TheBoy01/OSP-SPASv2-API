using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using OSP.SPASv2.Domain.Params;
using OSP.SPASv2.Repository.Parameters;
using OSP.SPASv2.Repository.Repository.MainRepository;
using SPASv2.Context;
using System.Text;

namespace OSP.SPASv2.Repository.Rules
{
    public class PurchaseOrderRequestRules : IRules<RequisitionParams>
    {
        StringBuilder sb = new StringBuilder();



        public string CanCreate(RequisitionParams entity)
        {
            throw new NotImplementedException();
        }

        public string CanRead(RequisitionParams entity)
        {
            sb = new StringBuilder();
            try
            {
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

        public string CanCreatetmp(RequisitionParams entity)
        {
            sb = new StringBuilder();
            try
            {
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

        public string CanDelete(RequisitionParams entity)
        {
            throw new NotImplementedException();
        }

       

        public string CanUpdate(RequisitionParams entity)
        {
            throw new NotImplementedException();
        }

        public Task<string> CanReadAsync(RequisitionParams entity)
        {
            throw new NotImplementedException();
        }

        public Task<string> CanDeleteAsync(RequisitionParams entity)
        {
            throw new NotImplementedException();
        }

        //public string CanEndorse(RequisitionParams entity)
        //{
        //    sb = new StringBuilder();
        //    try
        //    {
        //        if (entity.TblPaymentrequesthdr.PayMethodType != "CHEQUE")
        //        {
        //            if (string.IsNullOrEmpty(entity.BankCode) || string.IsNullOrWhiteSpace(entity.BankCode))
        //            {
        //                sb.Append("BankCode is not valid.");
        //            }
        //        }
        //        if (!entity.IsClassIdExist)
        //        {
        //            sb.Append("Class ID is not valid.");
        //        }
        //        if (!entity.IsCOADeptExist)
        //        {
        //            sb.Append("Department Code is not valid.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }

        //    return sb.ToString();

        //}



    }
}
