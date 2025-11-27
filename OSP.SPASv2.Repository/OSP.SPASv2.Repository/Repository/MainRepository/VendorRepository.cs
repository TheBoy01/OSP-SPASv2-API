using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;

using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Domain.View;
using OSP.Common.Domain.References;
using System.Formats.Asn1;
//using OSP.SPASv2.Domain.View;
//using Microsoft.AspNetCore.Mvc;




namespace OSP.SPASv2.Repository.Repository
{
    public class VendorRepository : IVendorRepository<TblVendor> 
    {
        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblVendor> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        #endregion


        #region Contructor
        public VendorRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblVendor>(_context);
        }
        #endregion


        #region Public Methods
        
        public async Task<TblResponse> CreateVendor(TblVendor entity)
        {
            await _AbstractRepository.Insert(entity);
            return await Task.FromResult(_response);
        }
        public async Task<TblResponse> UpdateVendor(TblVendor entity)
        {
                var oldEntity = await this.Read(entity.VendorCode);
                _AbstractRepository.Update(oldEntity, entity);
                new Task(() => { TrailEdit(oldEntity, entity, "TblVendor", "Vendor", entity.VendorCode); }).Start();
                return await Task.FromResult(_response);
        }

        public async Task<TblResponse> DeleteVendor(TblVendor entity)
        {
            _AbstractRepository.Delete(entity);
            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> DeleteVendor(object Primarykey, object Primarykey2)
        {
            _AbstractRepository.DeleteByComposite(Primarykey, Primarykey2);
            return await Task.FromResult(_response);
        }

        public void TrailEdit(TblVendor entityOldValue, TblVendor entityToUpdate, string TableName, string FormName, string UniqueInfo)
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
            
        public async Task<TblVendor> Read(object Primarykey)
        {
            return await _AbstractRepository.GetByID(Primarykey);
        }

        public async Task<IList<TblVendor>> GetAllObjects()
        {
            var vlist = await Task.FromResult(_context.TblVendor.FromSqlRaw("select * from TblVendor").ToList());
            return vlist;
        }

        public async Task<IList<qryVendorList>> GetVendorLists()
        {

            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<qryVendorList> vlist = await _context.qryVendorList.FromSqlRaw("exec sp_GetVendorList").ToListAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<TblVendorTIN> GetVendorTIN(string vendorcode)
        {
            try
            {
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                TblVendorTIN vlist = await _context.TblVendorTIN.Where(a => DateTime.Now.Date <= a.EndDate && DateTime.Now.Date >= a.StartDate && a.VendorCode == vendorcode).FirstOrDefaultAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<IList<qryVendorList>> GetVendorLists1(string vendordesc,string payclass)
        { 
            try
            { 
                //var vlist =   _context.qryBranch.FromSqlRaw("exec sp_getbranchdetails '" + companycode + "','" + branchcode + "'").AsEnumerable().FirstOrDefault();
                IList<qryVendorList> vlist = await _context.qryVendorList.FromSqlRaw("exec sp_GetVendorLists '"+ vendordesc + "','"+ payclass +"'").ToListAsync();
                
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        } 
        public async Task<IList<qryVendorList>> GetVendorList()
        { 
            var vlist = await _context.qryVendorList.FromSqlRaw("exec sp_GetVendorList").ToListAsync();
            return vlist;
        }

        public async Task<TblVendor> GetLatestVendorCode()
        {
            try
            {
                string YearMonth = DateTime.Now.ToString("yyMM");
                StringBuilder sb= new StringBuilder();
                sb.Append("select top 1 * from TblVendor where left(VendorCode,4)='" + YearMonth + "' order by VendorCode desc");
                //sb.Append("select top 1 VendorCode from TblVendor");

                return await _context.TblVendor.FromSqlRaw<TblVendor>(sb.ToString()).FirstOrDefaultAsync();
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<qryVendorDetails> GetVendorDetails(string vendorcode, string payclass)
        {
            try
            {
               // var vlist = await _context.qryVendorDetails.FromSqlRaw("exec sp_GetVendorDetails '" + vendorcode + "','" + payclass + "'").FirstOrDefaultAsync();
                var vlist = await _context.qryVendorDetails
            .FromSqlRaw("select * from qryVendorDetails where VendorCode ='"+ vendorcode +"'").FirstOrDefaultAsync();
                return vlist;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public async Task<string> GetVendorIDByVendorCode(string _VendorCode)
        {
            try
            {
                // var vlist = await _context.qryVendorDetails.FromSqlRaw("exec sp_GetVendorDetails '" + vendorcode + "','" + payclass + "'").FirstOrDefaultAsync();
                return await _context.TblVendor.Where(a => a.VendorCode == _VendorCode).Select(a => a.VendorID).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetVendorCodeByDisplayName(string displayName)
        {
            try
            {
                return await _context.TblVendor.Where(a => a.DisplayName == displayName).Select(a => a.VendorCode).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetVendorNameByVendorCode(string vendorCode)
        {
            try
            {
                return await Task.FromResult(_context.TblVendor.Where(p => p.VendorCode.Equals(vendorCode))
                                                                .Select(p => p.DisplayName).FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




        #endregion
    }
}
