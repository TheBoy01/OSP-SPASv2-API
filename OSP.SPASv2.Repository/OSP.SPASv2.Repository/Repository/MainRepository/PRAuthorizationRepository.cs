using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using OSP.SPASv2.Domain.View;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.Data.SqlClient;
using static SPASv2.Context.SPASv2Context;
using System.Linq;
using OSP.Common.Domain.View;
using System.Collections.Immutable;
using Newtonsoft.Json.Linq;
using DocumentFormat.OpenXml.Spreadsheet;

namespace OSP.SPASv2.Repository.Repository
{
    public class PRAuthorizationRepository : IPRAuthorizationRepository<TblPRAuthorization>
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblPRAuthorization> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        #endregion


        #region Contructor
        public PRAuthorizationRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblPRAuthorization>(_context);
        }
        #endregion


        #region Public Methods

        public async Task<TblResponse> CreatePRAuthorization(TblPRAuthorization entity)
        {
            await _AbstractRepository.Insert(entity);
            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> CreatePRAuthorization(string prno,string reqtype)
        {
            try
            {
                await Task.FromResult(_context.Database.ExecuteSqlRaw("exec sp_CreatePRAuthorization '" + prno + "','" + reqtype + "'"));
                return await Task.FromResult(_response);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public async Task<TblResponse> UpdatePRAuthorization(string prno,string personid,string statustype)
        {
            _context.Database.ExecuteSqlRaw("Update TblPRAuthorization set StatusType = '"+ statustype + "' where prno = '"+ prno + "' and personid = '"+ personid + "'");
            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> UpdatePaymentRequestAuthRemarks(string prno, string personid)
        {
            string remarks = string.Empty;

            var vlist = await Task.FromResult(_context.TblPaymentRequestAuth.Where(p => p.Reqno.Contains(prno) && p.PersonID.Contains(personid))
                                                                  .Select(p => p.AuthorizeClass).FirstOrDefault());
            switch (vlist)
            {
                case "VERIFIER":
                    remarks = "FOR VERIFY";
                    break;

                case "APPROVER1":
                    remarks = "FOR APPROVAL";
                    break;

                case "APPROVER2":
                    remarks = "FOR APPROVAL";
                    break;

                case "APPROVER3":
                    remarks = "FOR APPROVAL";
                    break;

                default:
                    break;
            }

            _context.Database.ExecuteSqlRaw("Update TblPRAuthorization set Remarks = '" + remarks + "' where prno = '" + prno + "' and personid = '" + personid + "'");
            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> ApprovePRAuthorization(qryUpdateStatusAuth _qryUpdateStatusAuth)
        {

            string remarks = string.Empty;

            var vlist = await Task.FromResult(_context.TblPaymentRequestAuth.Where(p => p.Reqno.Contains(_qryUpdateStatusAuth.PRRefNo) && p.PersonID.Contains(_qryUpdateStatusAuth.PersonID))
                                                                  .Select(p => p.AuthorizeClass).FirstOrDefault());

            if (string.IsNullOrEmpty(vlist))
            {
                if (vlist == null)
                {
                    vlist = 
                       _context.TblPaymentRequestAuth
                       .Join(_context.TblAuthorizerGroup, a => a.PersonID, b => b.GroupId,
                       (a, b) => new TblPaymentRequestAuth
                       {
                           Reqno = a.Reqno,
                           PersonID = b.PersonId,
                           StatusType = a.StatusType,
                           IsRead = a.IsRead,
                           ReadDate = a.ReadDate,
                           AuthorizedDate = a.AuthorizedDate,
                           Remarks = a.Remarks,
                           AuthorizeLevel = a.AuthorizeLevel,
                           AuthorizeClass = a.AuthorizeClass
                           
                       })
                       .Where(a => a.Reqno == _qryUpdateStatusAuth.PRRefNo && a.PersonID == _qryUpdateStatusAuth.PersonID)
                       .Select(p => p.AuthorizeClass).FirstOrDefault();
                }

            }
            //switch (vlist)
            //{
            //    case "VERIFIER":
            //        remarks = "VERIFIED";
            //        break;

            //    case "APPROVER1":
            //        remarks = "APPROVED";
            //        break;

            //    case "APPROVER2":
            //        remarks = "APPROVED";
            //        break;

            //    case "APPROVER3":
            //        remarks = "APPROVED";
            //        break;

            //    default:
            //        break;
            //}

            _context.Database.ExecuteSqlRaw("Update TblPaymentRequestAuth set  StatusType = '" + _qryUpdateStatusAuth.StatusType + "',AuthorizedDate = getdate() where Reqno = '" + _qryUpdateStatusAuth.PRRefNo + "' and personid = '" + _qryUpdateStatusAuth.PersonID + "'");
            //update as personid
            _context.Database.ExecuteSqlRaw("UPDATE TblPaymentRequestAuth SET StatusType =  '" + _qryUpdateStatusAuth.StatusType + "' , AuthorizedDate = GETDATE(), PersonId = '" + _qryUpdateStatusAuth.PersonID + "' " +" FROM TblPaymentRequestAuth INNER JOIN TblAuthorizerGroup AS b ON TblPaymentRequestAuth.PersonID = b.GroupId WHERE (TblPaymentRequestAuth.Reqno = '"+ _qryUpdateStatusAuth.PRRefNo + "') AND (b.PersonId = '"+ _qryUpdateStatusAuth.PersonID + "')");
            //update as group
            _context.Database.ExecuteSqlRaw("Update TblPaymentRequestAuth set  Remarks = (select DoneRemarks from RefDefaultAuthorizeClassRemarks where AuthorizeClass = '"+ vlist +"') where reqno = '" + _qryUpdateStatusAuth.PRRefNo + "' and personid = '" + _qryUpdateStatusAuth.PersonID + "'");
            
            //_context.Database.ExecuteSqlRaw("Update tblrequisitionhdr set  Active = 0,EditUser = '" + _qryUpdateStatusAuth.PersonID + "',Editdate = '" + DateTime.Now + "' where reqno = '" + _qryUpdateStatusAuth.PRRefNo + "'");


            if (_qryUpdateStatusAuth.StatusType == "DN")
            {
                _context.Database.ExecuteSqlRaw("Update TblPaymentRequestAuth set  Remarks = 'DISAPPROVED.' where reqno = '" + _qryUpdateStatusAuth.PRRefNo + "' and personid = '" + _qryUpdateStatusAuth.PersonID + "'");

            }



            return await Task.FromResult(_response);
        }

        

        public async Task<TblResponse> UpdateReadPRAuthorization(qryUpdateStatusAuth _qryUpdateStatusAuth)
        {

            string remarks = string.Empty;

            var vlist = await Task.FromResult(_context.TblPaymentRequestAuth.Where(p => p.Reqno.Contains(_qryUpdateStatusAuth.PRRefNo) && p.PersonID.Contains(_qryUpdateStatusAuth.PersonID))
                                                                  .Select(p => p.AuthorizeClass).FirstOrDefault());
         
            _context.Database.ExecuteSqlRaw("exec sp_UpdateIsReadpaymentAuthorization '"+ _qryUpdateStatusAuth.PRRefNo + "','"+ _qryUpdateStatusAuth.PersonID + "'");
           
            return await Task.FromResult(_response);
        }

      



        public async Task<TblResponse> DisapprovePRAuthorization(qryUpdateStatusAuth _qryUpdateStatusAuth)
        {

            string remarks = string.Empty;

            var vlist = await Task.FromResult(_context.TblPaymentRequestAuth.Where(p => p.Reqno.Contains(_qryUpdateStatusAuth.PRRefNo) && p.PersonID.Contains(_qryUpdateStatusAuth.PersonID))
                                                                  .Select(p => p.AuthorizeClass).FirstOrDefault());
            if (string.IsNullOrEmpty(vlist))
            {
                if (vlist == null)
                {
                    vlist =
                       _context.TblPaymentRequestAuth
                       .Join(_context.TblAuthorizerGroup, a => a.PersonID, b => b.GroupId,
                       (a, b) => new TblPaymentRequestAuth
                       {
                           Reqno = a.Reqno,
                           PersonID = b.PersonId,
                           StatusType = a.StatusType,
                           IsRead = a.IsRead,
                           ReadDate = a.ReadDate,
                           AuthorizedDate = a.AuthorizedDate,
                           Remarks = a.Remarks,
                           AuthorizeLevel = a.AuthorizeLevel,
                           AuthorizeClass = a.AuthorizeClass

                       })
                       .Where(a => a.Reqno == _qryUpdateStatusAuth.PRRefNo && a.PersonID == _qryUpdateStatusAuth.PersonID)
                       .Select(p => p.AuthorizeClass).FirstOrDefault();
                }

            }

            _context.Database.ExecuteSqlRaw("Update TblPaymentRequestAuth set  StatusType = '" + _qryUpdateStatusAuth.StatusType + "',AuthorizedDate = '" + DateTime.Now + "' where reqno = '" + _qryUpdateStatusAuth.PRRefNo + "' and personid = '" + _qryUpdateStatusAuth.PersonID + "'");
            //_context.Database.ExecuteSqlRaw("Update tblrequisitionhdr set  Void = 1,Editdate = '" + DateTime.Now + "' where reqno = '" + _qryUpdateStatusAuth.PRRefNo + "'");
            _context.Database.ExecuteSqlRaw("Update tblrequisitionhdr set  Active = 0,EditUser = '"+ _qryUpdateStatusAuth.PersonID + "',Editdate = '" + DateTime.Now + "' where reqno = '" + _qryUpdateStatusAuth.PRRefNo + "'");
            _context.Database.ExecuteSqlRaw("Update TblPaymentRequestAuth set  Remarks = 'DISAPPROVED.' where reqno = '" + _qryUpdateStatusAuth.PRRefNo + "' and personid = '" + _qryUpdateStatusAuth.PersonID + "'");
            _context.Database.ExecuteSqlRaw("UPDATE TblPaymentRequestAuth SET StatusType =  '" + _qryUpdateStatusAuth.StatusType + "' , AuthorizedDate = GETDATE(), PersonId = '" + _qryUpdateStatusAuth.PersonID + "' " + " FROM TblPaymentRequestAuth INNER JOIN TblAuthorizerGroup AS b ON TblPaymentRequestAuth.PersonID = b.GroupId WHERE (TblPaymentRequestAuth.Reqno = '" + _qryUpdateStatusAuth.PRRefNo + "') AND (b.PersonId = '" + _qryUpdateStatusAuth.PersonID + "')");

            _context.Database.ExecuteSqlRaw("Insert into TblRequisitionReason values ('" + _qryUpdateStatusAuth.PRRefNo + "','" + _qryUpdateStatusAuth.ReqReason + "','" + _qryUpdateStatusAuth.ReasonRemarks + "','" + _qryUpdateStatusAuth.PersonID + "','" + DateTime.Now + "')");

            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> InsertReqReason(qryUpdateStatusAuth _qryUpdateStatusAuth)
        {


            _context.Database.ExecuteSqlRaw("delete from  TblRequisitionReason where Reqno = '" + _qryUpdateStatusAuth.PRRefNo + "'");
            _context.Database.ExecuteSqlRaw("Insert into TblRequisitionReason values ('" + _qryUpdateStatusAuth.PRRefNo + "','" + _qryUpdateStatusAuth.ReqReason + "','" + _qryUpdateStatusAuth.ReasonRemarks + "','" + _qryUpdateStatusAuth.PersonID + "','" + DateTime.Now + "')");
            return await Task.FromResult(_response);
        }

        public async Task<string> GetAuthorizeClass(string prno, string personid)
        {
            var vlist = await Task.FromResult(_context.TblPaymentRequestAuth.Where(p => p.Reqno.Contains(prno) && p.PersonID.Contains(personid))
                                                                   .Select(p => p.AuthorizeClass).FirstOrDefault());
            return vlist;
        }

        public async Task<string> GetAuthorizeClass(string personid)
        {


            //var result = await Task.FromResult(_context.Set<ValReturn<string>>()
            //     .FromSqlRaw("select a.AuthorizeClass as Value from tblauthorizer a left join TblAuthorizerGroup b on a.PersonID = b.GroupId where a.PersonId = '" + personid + "' and b.PersonId is null UNION select a.AuthorizeClass as Value from tblauthorizer a inner join TblAuthorizerGroup b on a.PersonID = b.GroupId where b.personid = '" + personid + "'")
            //     .AsEnumerable()
            //     .First().Value);
            //return result;

            var result = await Task.FromResult(_context.Set<ValReturn<string>>()
                 .FromSqlRaw("sp_GetAuthorizeClass '" + personid + "'")
                 .AsEnumerable()
                 .First().Value);
            return result;


        }

        public async Task<string> GetLastestBANo(string reqtype,string payclasscode)
        {


            //var result = await Task.FromResult(_context.Set<ValReturn<string>>()
            //     .FromSqlRaw("select a.AuthorizeClass as Value from tblauthorizer a left join TblAuthorizerGroup b on a.PersonID = b.GroupId where a.PersonId = '" + personid + "' and b.PersonId is null UNION select a.AuthorizeClass as Value from tblauthorizer a inner join TblAuthorizerGroup b on a.PersonID = b.GroupId where b.personid = '" + personid + "'")
            //     .AsEnumerable()
            //     .First().Value);
            //return result;

            var result = await Task.FromResult(_context.Set<ValReturn<string>>()
                 .FromSqlRaw("sp_GenerateBANo '" + reqtype + "','" + payclasscode + "'")
                 .AsEnumerable()
                 .First().Value);
            return result;


        }

        public async Task<string> GetPositionCode(string personid)
        {
            var result = await Task.FromResult(_context.Set<ValReturn<string>>()
                .FromSqlRaw("select top 1 PositionCode as Value from tblauthorizer where personid = '"+ personid + "'")
                .AsEnumerable()
                .First().Value);
            return result;
        }


        public async Task<TblResponse> DeleteVendor(TblPRAuthorization entity)
        {
            _AbstractRepository.Delete(entity);
            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> DeleteVendor(object Primarykey, object Primarykey2)
        {
            _AbstractRepository.DeleteByComposite(Primarykey, Primarykey2);
            return await Task.FromResult(_response);
        }

        public void TrailEdit(TblVendor entityOldValue, TblPRAuthorization entityToUpdate, string TableName, string FormName, string UniqueInfo)
        {
            TblVendor _r = new TblVendor();
            PropertyInfo[] oldent = entityOldValue.GetType().GetProperties();
            PropertyInfo[] newent = entityToUpdate.GetType().GetProperties();


            foreach (var oPropertyInfo in oldent)
            {

                MethodInfo OLDMethodInfo = entityOldValue.GetType().GetMethod("get_" + oPropertyInfo.Name);
                ParameterInfo[] ArrParameterInfo = entityOldValue.GetType().GetMethod("get_" + oPropertyInfo.Name).GetParameters();
                var oldvalue = oPropertyInfo.Name + " = " + OLDMethodInfo.Invoke(entityOldValue, ArrParameterInfo);
                //var oldvalue = OLDMethodInfo.Invoke(entityOldValue, ArrParameterInfo);



                MethodInfo NewMethodInfo = entityToUpdate.GetType().GetMethod("get_" + oPropertyInfo.Name);
                ParameterInfo[] ArrParameterInfo2 = entityToUpdate.GetType().GetMethod("get_" + oPropertyInfo.Name).GetParameters();
                var newvalue = oPropertyInfo.Name + " = " + NewMethodInfo.Invoke(entityToUpdate, ArrParameterInfo2);
                //var newvalue = NewMethodInfo.Invoke(entityToUpdate, ArrParameterInfo2);
                

                if (!oldvalue.Contains("AuditUser") && !oldvalue.Contains("AuditDate") && !oldvalue.Contains("EditUser") && !oldvalue.Contains("EditDate"))
                {
                    if (oldvalue != newvalue)
                    {
                        sb = new StringBuilder();
                        sb.Append("Insert Into tbltrailedit ");
                        sb.Append("(TblName,UniqueInfo,FldName,OldValue,NewValue,FormName,AuditUser,AuditDate) ");
                        sb.Append("Values ");
                        sb.Append("('" + TableName.ToString().Trim() + "','" + UniqueInfo.ToString().Trim() + "', ");
                        sb.Append("'" + oPropertyInfo.Name + "','" + oldvalue.ToString() + "','" + newvalue.ToString() + "', ");
                        sb.Append("'" + FormName.ToString() + "') ");

                    }
                }

            }

        }
        #endregion

        #region Public Function

       

        public async Task<IList<TblVendor>> GetAllObjects()
        {
            var vlist = await Task.FromResult(_context.TblVendor.FromSqlRaw("select * from TblPRAuthorization").ToList());
            return vlist;
        }

        public async Task<IList<TblPaymentRequestAuth>> GetPRAuthorizationList(string personid)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<TblPaymentRequestAuth> vlist = await _context.TblPaymentRequestAuth.FromSqlRaw("select * from TblPaymentRequestAuth").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<TblPaymentRequestAuth> GetLatestAuthoriztionByAuthorizeLevel(string prno)
        {

            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                TblPaymentRequestAuth vlist = await _context.TblPaymentRequestAuth.FromSqlRaw("select top 1 * from TblPaymentRequestAuth where StatusType = 'PD' AND reqno = '"+ prno + "' order by AuthorizeLevel").FirstOrDefaultAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        

        public async Task<IList<TblPaymentRequestAuth>> GetLatestAuthoriztionByAuthorizeLevel()
        {

            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<TblPaymentRequestAuth> vlist = await _context.TblPaymentRequestAuth.FromSqlRaw("exec sp_GetPRAuthorizationNext").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<IList<TblPaymentRequestAuth>> GetALLTblPaymentRequestAuthByAuthorizeLevel(string prno)
        {

            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<TblPaymentRequestAuth> vlist = await _context.TblPaymentRequestAuth.FromSqlRaw("select * from tblpaymentrequestAuth where ReqNo = '"+prno+"' order by AuthorizeLevel").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<IList<qryPRAuthorizationList>> GetPRAuthorizationLists(string personid)
        {

            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
            
                IList<qryPRAuthorizationList> vlist = await _context.qryPRAuthorizationList.FromSqlRaw("exec sp_GetPRAuthorizationList '"+ personid + "'").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<IList<qryPRAuthorizationList>> GetPRAuthorizationLists_Batch(string personid,string BatchPRno)
        {

            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();

                IList<qryPRAuthorizationList> vlist = await _context.qryPRAuthorizationList.FromSqlRaw("exec sp_GetPRAuthorizationList_Batch '" + personid + "','"+ BatchPRno + "'").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<IList<qryPRAuthorizationList>> GetPRAuthorizationLists_BatchByPayclassCode(string personid, string Payclasscode)
        {

            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();

                IList<qryPRAuthorizationList> vlist = await _context.qryPRAuthorizationList.FromSqlRaw("exec sp_GetPRAuthorizationList_BatchByPayclassCode '" + personid + "','" + Payclasscode + "'").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<IList<qryPRAuthorizationList>> GetPRAuthorizationLists_BatchByPayClassCode(string personid, string BatchPRno)
        {

            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();

                IList<qryPRAuthorizationList> vlist = await _context.qryPRAuthorizationList.FromSqlRaw("exec sp_GetPRAuthorizationList_BatchByPayclassCode '" + personid + "','" + BatchPRno + "'").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<IList<qryListOfAuthorizerPayclass>> GetAuthorizerPayclassLists(string companytype,string payclass)
        {

            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();

                IList<qryListOfAuthorizerPayclass> vlist = await _context.qryListOfAuthorizerPayclass.FromSqlRaw("select a.PersonId,b.Paydesc as PayclassDesc,AuthorizeLevel,a.AuthorizeClass from tblauthorizer  as a inner join refpaymentclass as b on a.payclasscode = b.payclasscode where companytype = '" + companytype + "' and a.payclassCode = '"+ payclass +"'").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<IList<qryPaymentRequestAuthDtl>> GetPaymentRequestAuthListByPRNO(string Prno)
        {

            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<qryPaymentRequestAuthDtl> vlist = await _context.qryPaymentRequestAuthDtl.FromSqlRaw("select * from qryPaymentRequestAuthDtl where prno = '"+ Prno + "' order by authorizelevel ").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }



        public async Task<IList<qryRequestPaymentRequestbyStatus>> GetRequestPaymentRequestbyStatus(string status,string personid)
        {

            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<qryRequestPaymentRequestbyStatus> vlist = await _context.qryRequestPaymentRequestbyStatus.FromSqlRaw("exec sp_GetRequestPaymentRequestbyStatus '"+ status + "','"+ personid + "'  ").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        

        public async Task<string> GetNameofAuthorizer(string personid)
        {
            var result = await Task.FromResult(_context.Set<ValReturn<string>>()
                .FromSqlRaw("select top 1 Name as Value from qryPaymentRequestAuthDtl where personid = '" + personid + "'")
                .AsEnumerable()
                .First().Value);
            return result;
        }

        public async Task<string> GetNameofEmployee(string personid)
        {
            var result = await Task.FromResult(_context.Set<ValReturn<string>>()
                .FromSqlRaw("select top 1 (FirstName + ' ' + LastName) as Value from Tblemployee where personid = '" + personid + "'")
                .AsEnumerable()
                .First().Value);
            return result;
        }


        public async Task<DateTime> GetRequestDate(string prno)
        {
            var result = await Task.FromResult(_context.Set<ValReturn<DateTime>>()
                .FromSqlRaw("select top 1 ReqDate as Value from TblRequisitionHdr where ReqNo = '" + prno + "'")
                .AsEnumerable()
                .First().Value);
            return result;
        }

        public async Task<TblPaymentRequestAuth> GetALLTblPaymentRequestAuthByPersonId(string prno,string personid)
        {
            try
            {
                TblPaymentRequestAuth vlist = await _context.TblPaymentRequestAuth.Where(a => a.Reqno == prno && a.PersonID == personid && a.StatusType == "PD").FirstOrDefaultAsync();

                if (vlist == null)
                {
                    vlist = await
                       _context.TblPaymentRequestAuth
                       .Join(_context.TblAuthorizerGroup, a => a.PersonID, b => b.GroupId,
                       (a, b)  => new TblPaymentRequestAuth
                       {
                           Reqno = a.Reqno,
                           PersonID = b.PersonId,
                           StatusType = a.StatusType,
                           IsRead = a.IsRead,
                           ReadDate = a.ReadDate,
                           AuthorizedDate = a.AuthorizedDate,
                           Remarks = a.Remarks,
                           AuthorizeLevel = a.AuthorizeLevel,
                           AuthorizeClass = a.AuthorizeClass
                           
                       })
                       .Where(a => a.Reqno == prno && a.PersonID == personid && a.StatusType == "PD").FirstOrDefaultAsync();
                }

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        
        public async Task<IList<TblRequisitionhdr>> GetBatchPRNo(string personid)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<TblRequisitionhdr> vlist = await _context.TblRequisitionhdr.FromSqlRaw("exec sp_BatchPRNoList '"+ personid + "'").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<qryDeclineReason>> GetDeclineReason()
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<qryDeclineReason> vlist = await _context.qryDeclineReason.FromSqlRaw("select ReasonCode,ReasonDesc,Category from refreason").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<TblRequisitionhdr>> GetBatchPRNoByPayclassCode(string personid,string payclasscode)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<TblRequisitionhdr> vlist = await _context.TblRequisitionhdr.FromSqlRaw("exec sp_BatchPRNoListByPayclasscode '"+ personid +"', '"+ payclasscode + "'").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<qryPaymentClassAuthorization>> GetPaymentClassAuthorization(string personid)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<qryPaymentClassAuthorization> vlist = await _context.qryPaymentClassAuthorization.FromSqlRaw("exec sp_BatchPayclass '" + personid + "'").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }



        public async Task<IList<qryRptPurchaseOrderDetails>> GetRptPODtl(string pono)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<qryRptPurchaseOrderDetails> vlist = await _context.qryRptPurchaseOrderDetails.FromSqlRaw("select Department, Description, Qty, UOM, UnitPrice, TotalPrice from rptPurchaseOrder where PONo = '"+ pono + "'").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<qryRptPurchaseOrderConsolidated>> GetRptPurchaseOrderConsolidated(string bano)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<qryRptPurchaseOrderConsolidated> vlist = await _context.qryRptPurchaseOrderConsolidated.FromSqlRaw("select PONo,Department, Description, Qty, UOM, UnitPrice, TotalPrice from rptpurchaseorder where reqno in (select reqno from tblbatchapproval where Bano = '" + bano + "')").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<qryRptChapelAdvisory>> GetRptChapelAdvisory(string bano,string chapelcode)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<qryRptChapelAdvisory> vlist = await _context.qryRptChapelAdvisory.FromSqlRaw("select a.Description,a.Qty,Vendorname as Supplier,PONo from rptpurchaseorder  as a where  a.reqno in (select reqno from tblbatchapproval where Bano = '"+ bano + "')  and (select top 1 Chapelcode from OSP.dbo.refchapelbranch where ChapelDesc = a.department) in ('" + chapelcode +"') ").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<qryRptChapelAdvisory_GCM>> GetRptChapelAdvisory_GCM(string bano,string GCMID)
        {
            sb = new StringBuilder();

            try
            {

                //sb.AppendLine("select Department, ");
                //sb.AppendLine("(select address from OSP.dbo.refchapelbranch where chapelcode in ");
                //sb.AppendLine("(select distinct Deptcode from tblrequisitiondtl where reqno = a.reqno)) as DeliveryAddress, ");
                //sb.AppendLine("Description,Qty,PONo ,Vendorname as Supplier ");
                //sb.AppendLine("from rptpurchaseorder   as a  ");
                //sb.AppendLine("where reqno in (select reqno from tblbatchapproval where Bano = '"+ bano + "')");
                //sb.AppendLine("and (select distinct Deptcode from tblrequisitiondtl where reqno = a.reqno) in  ");
                //sb.AppendLine("(select Chapelcode from tblGCM_April_2024 where GCMID = '"+ GCMID + "') ");

                sb.AppendLine("select a.Department,a.Description,a.Qty,PONo,Vendorname as Supplier ");
                sb.AppendLine("from rptpurchaseorder as a where  a.reqno in (select reqno from tblbatchapproval where Bano = '"+  bano +"')   ");
                sb.AppendLine("and (select top 1 Chapelcode from OSP.dbo.refchapelbranch  ");
                sb.AppendLine("where ChapelDesc = a.department) in (select Chapelcode from tblGCM_April_2024 where GCMID =  '"+ GCMID + "') ORDER BY a.Department ");

                IList<qryRptChapelAdvisory_GCM> vlist = await _context.qryRptChapelAdvisory_GCM.FromSqlRaw(sb.ToString()).ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<qryRptTransmittalFO>> GetRptTransmittalLO(string bano)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<qryRptTransmittalFO> vlist = await _context.qryRptTransmittalFO.FromSqlRaw("select a.ReqNo,a.CompanyCode,(select DeptDesc from OSP.dbo.refdepartment where deptcode = a.DeptCode and companycode = a.companycode) as Department,b.ItemDesc as Description,a.Quantity as Qty,a.Unit as UOM,a.Price as UnitPrice,a.Gross as TotalPrice,(select sum(Freight) from TblRequisitionDtl where reqno = a.ReqNo) As Freight from TblRequisitionDtl a left join RefItems b on a.ItemCode=b.ItemCode left join TblBatchApproval c on a.ReqNo=c.ReqNo and ReqType='PY' where c.BANo = '" + bano + "'").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<qryRptTransmittalFO>> GetRptTransmittalLO(string bano, string CompanyCode)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<qryRptTransmittalFO> vlist = await _context.qryRptTransmittalFO.FromSqlRaw("select a.ReqNo,a.CompanyCode,(select DeptDesc from OSP.dbo.refdepartment where deptcode = a.DeptCode and companycode = a.companycode) as Department,b.ItemDesc as Description,a.Quantity as Qty,a.Unit as UOM,a.Price as UnitPrice,a.Gross as TotalPrice,(select sum(Freight) from TblRequisitionDtl where reqno = a.ReqNo) As Freight from TblRequisitionDtl a left join RefItems b on a.ItemCode=b.ItemCode left join TblBatchApproval c on a.ReqNo=c.ReqNo and ReqType='PY' where c.BANo = '" + bano + "' and a.CompanyCode = '"+ CompanyCode +"'").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<qryRptTransmittalFO>> GetRptTransmittalLO(string bano, string CompanyCode,string vendorcode)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<qryRptTransmittalFO> vlist = await _context.qryRptTransmittalFO.FromSqlRaw("select a.ReqNo,a.CompanyCode,(select DeptDesc from OSP.dbo.refdepartment where deptcode = a.DeptCode and companycode = a.companycode) as Department,b.ItemDesc as Description,a.Quantity as Qty,a.Unit as UOM,a.Price as UnitPrice,a.Gross as TotalPrice ,(select sum(Freight) from TblRequisitionDtl where reqno = a.ReqNo and  itemcode = a.itemcode) As Freight from TblRequisitionDtl a left join RefItems b on a.ItemCode=b.ItemCode left join TblBatchApproval c on a.ReqNo=c.ReqNo and ReqType='PY' where c.BANo = '" + bano + "' and a.CompanyCode = '" + CompanyCode + "' and (select VendorCode from TblRequisitionHdr where reqno = a.ReqNo) = '"+ vendorcode + "'").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<qryRptPurchaseOrderConsolidated>> GetRptPurchaseOrderConsolidated(string bano,string vendorname)
        {
            sb = new StringBuilder();


            try
            {
                sb.AppendLine("sp_RptConsolidated '"+ bano + "','"+ vendorname + "'");
                IList<qryRptPurchaseOrderConsolidated> vlist = await _context.qryRptPurchaseOrderConsolidated.FromSqlRaw(sb.ToString()).ToListAsync();


                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<IList<RptPurchaseorder>> GetRptPurchaseorderByBANo(string bano)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<RptPurchaseorder> vlist = await _context.RptPurchaseorder.FromSqlRaw("select * from rptpurchaseorder where reqno in (select reqno from tblbatchapproval where Bano = '"+ bano + "') order by vendorname").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IList<RptPurchaseorder>> GetRptPurchaseorderByBANo(string bano,string vendorname)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<RptPurchaseorder> vlist = await _context.RptPurchaseorder.FromSqlRaw("select * from rptpurchaseorder where reqno in (select reqno from tblbatchapproval where Bano = '" + bano + "' and verdorname = '"+ vendorname + "') order by vendorname").ToListAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        //public async Task<IList<qryRptPurchaseOrderConsolidated>> GetRptPurchaseOrderConsolidated(IList<string> reqno,string vendorname)
        //{


        //    string where = string.Join(",", reqno.Select(i => $"'{i}'"));

        //    try
        //    {
        //        //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
        //        IList<qryRptPurchaseOrderConsolidated> vlist = await _context.qryRptPurchaseOrderConsolidated.FromSqlRaw("select PONo,Department, Description, Qty, UOM, UnitPrice, TotalPrice from rptPurchaseOrder where reqno in (" + where + ")").ToListAsync();
        //        return vlist;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task<qryRptPurchaseOrderHdr> GetRptPOHdr(string bano)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                qryRptPurchaseOrderHdr vlist = await _context.qryRptPurchaseOrderHdr.FromSqlRaw("select distinct CompanyDesc,Department, Address, PONo, ReqNo, VendorName, PayeeName, TIN, PayMethod, PayClass, Terms from rptPurchaseOrder where reqno in (select reqno from tblbatchapproval where Bano = '" + bano + "')").FirstOrDefaultAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<qryRptPurchaseOrderHdr> GetRptPOHdr(string bano, string vendorname) 
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                qryRptPurchaseOrderHdr vlist = await _context.qryRptPurchaseOrderHdr.FromSqlRaw("select distinct CompanyDesc,Department, Address, PONo, ReqNo, VendorName, PayeeName, TIN, PayMethod, PayClass, Terms from rptPurchaseOrder where reqno in (select reqno from tblbatchapproval where Bano = '"+ bano + "') and vendorName = '"+ vendorname + "'").FirstOrDefaultAsync();
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<qryPOSignatories> GetPOSignatories(string pono)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<qryPOSignatories> vlist = await _context.qryPOSignatories.FromSqlRaw("sp_GetPOSignatories '"+ pono + "'").ToListAsync();

                return vlist.FirstOrDefault();


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<string> GetBatchNoByPRNo(string prno)
        {
            var result = await Task.FromResult(_context.Set<ValReturn<string>>()
                .FromSqlRaw("select top 1 BatchNo as Value from tblrequisitionhdr where reqno = '" + prno + "'")
                .AsEnumerable()
                .First().Value);
            return result;
        }

        public async Task<DateTime> GetRequestDateByBatchNo(string batchprno)
        {
            var result = await Task.FromResult(_context.Set<ValReturn<DateTime>>()
                .FromSqlRaw("select top 1 ReqDate as Value from tblrequisitionhdr where BatchNo = '" + batchprno + "'")
                .AsEnumerable()
                .First().Value);
            return result;
        }

        public async Task<string> GetEmailByPersonID(string personid)
        {
            var result = await Task.FromResult(_context.Set<ValReturn<string>>()
                .FromSqlRaw("select top 1 Emailaddress as Value from OSP.dbo.tblemployee where personid = '" + personid + "'")
                .AsEnumerable()
                .First().Value);
            return result;
        }

        public async Task<IList<qryGroupEmails>> GetEmailsByGroupId(string groupid)
        {
            List<qryGroupEmails> _qryGroupEmails = new List<qryGroupEmails>();
            //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
            _qryGroupEmails = await _context.qryGroupEmails.FromSqlRaw("sp_GetEmailsByGroupId '" + groupid + "'").ToListAsync();
            return _qryGroupEmails;

        }

        public async Task<IList<TblAuthorizerGroup>> GetAuthorizerGroup(string groupid)
        {
            List<TblAuthorizerGroup> _qryGroupEmails = new List<TblAuthorizerGroup>();
            //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
            _qryGroupEmails = await _context.TblAuthorizerGroup.Where(a => a.GroupId == groupid).ToListAsync() ;
            return _qryGroupEmails;

        }

        public async Task<IList<TblAuthorizerGroup>> GetAuthorizer(string UserId)
        {
            List<TblAuthorizerGroup> _qryGroupEmails = new List<TblAuthorizerGroup>();
            //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
            _qryGroupEmails = await _context.TblAuthorizerGroup.Where(a => a.PersonId == UserId).ToListAsync();
            return _qryGroupEmails;

        }

        public async Task<IList<qryAuthorizerGroup>> GetAuthorizeGroupPersonIDandPayclass(string UserId,string Payclass)
        {
            try
            {
                var parameter = new List<SqlParameter>();
                parameter.Add(new SqlParameter("@PersonId", UserId));
                parameter.Add(new SqlParameter("@PayClass", Payclass));
                var result = await Task.Run(() => _context.qryAuthorizerGroup.FromSqlRaw("exec SP_GetAuthorizeGroupByPersonIdAndPayClass @PersonId, @PayClass", parameter.ToArray()).ToListAsync());
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception( ex.Message) ;
            }
            

        }

        public async Task<string> GetGenderByPersonID(string personid)
        {
            var result = await Task.FromResult(_context.Set<ValReturn<string>>()
                .FromSqlRaw("select top 1 Gender as Value from OSP.dbo.tblemployee where personid = '" + personid + "'")
                .AsEnumerable()
                .First().Value);
            return result;
        }

        public async Task<List<qrySignatoriesChapelAdvisory>> GetSignatoriesChapelAdvisory(string bano,string payclass)
        {
            List<qrySignatoriesChapelAdvisory> _qrySignatoriesChapelAdvisory = new List<qrySignatoriesChapelAdvisory>();
            //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
            _qrySignatoriesChapelAdvisory = await _context.qrySignatoriesChapelAdvisory.FromSqlRaw("sp_GetSignatoriesChapelAdvisory '" + bano + "','"+ payclass + "'").ToListAsync();
            return _qrySignatoriesChapelAdvisory;
        }



        #endregion
    }
}
