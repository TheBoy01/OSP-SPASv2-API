using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Mvc;
//using System.Data.Entity;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.Data.SqlClient;
using DocumentFormat.OpenXml.Office2010.Drawing;
using OSP.Common.Domain.View;
using static System.Data.Odbc.ODBC32;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class TblAssignedtoVendor_CMSRepository : ITblAssignedtoVendor_CMSRepository<TblAssignedtoVendor_CMS>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblAssignedtoVendor_CMS> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;

        #endregion

        #region Constructors
        public TblAssignedtoVendor_CMSRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblAssignedtoVendor_CMS>(_context);
        }

        #endregion

        public async Task<TblResponse> BulkInsert(List<TblAssignedtoVendor_CMS> entity)
        {
            try
            {
                await _AbstractRepository.BulkInsert(entity);
                return await Task.FromResult(_response);
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        public async Task<TblResponse> UpdateReqNo(string vendorCode, string reqno, string deptCode)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("Update TblAssignedtoVendor_CMS set ReqNo = '" + reqno + "' where CompanyCode = '" + vendorCode + "' and DeptCode='" + deptCode + "' and ReqNo is null");
                return await Task.FromResult(_response);
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        public async Task<TblAssignedtoVendor_CMS> ReadByReqNo(string reqNo)
        {
            try
            {
                return await _context.TblAssignedtoVendor_CMS.FromSqlRaw("Select * From TblAssignedtoVendor_CMS where Reqno='" + reqNo + "'").FirstOrDefaultAsync();
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        public async Task<List<TblAssignedtoVendor_CMS>> ReadByReqNoList(string reqNo)
        {
            try
            {
                return await _context.TblAssignedtoVendor_CMS.Where(a=>a.ReqNo.Equals(reqNo)).ToListAsync();
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        public async Task<TblResponse> UpdatePONo(string PONo, string ReqNo)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("Update TblAssignedtoVendor_CMS set PONo = '" + PONo + "' where ReqNo='" + ReqNo + "' and PONo is null");
                return await Task.FromResult(_response);
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }
    }
}
