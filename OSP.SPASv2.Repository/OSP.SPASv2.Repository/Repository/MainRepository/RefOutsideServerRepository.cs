using Microsoft.EntityFrameworkCore;
using OSP.Common.Domain.View;
using OSP.SPASv2.Repository.Context;
using OSP.SPASv2.Repository.Controllers;
using OSP.SPASv2.Repository.Utility;
using SPASv2.Context;
using SPASv2.Repository.Repository;
using static SPASv2.Context.SPASv2Context;
using static System.Data.Odbc.ODBC32;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class RefOutsideServerRepository
    {
        private SPASv1Context _context;

        AbstractRepository<TblDataSourceHdr> _AbstractRepositoryDtSourceHdr;
        AbstractRepository<TblDataSourceDtl> _AbstractRepositoryDtSourceDtl;
        TblResponse _response = new TblResponse();
        private ILogger<WaController> logger;
        
        public RefOutsideServerRepository(SPASv1Context context)
        {
            _context = context; 
            _context = DbContextFactory.Create("TSPG");
            _AbstractRepositoryDtSourceHdr = new AbstractRepository<TblDataSourceHdr>(_context);
            _AbstractRepositoryDtSourceDtl = new AbstractRepository<TblDataSourceDtl>(_context);
        }

        //public async Task<string> GetLatestBatch(string CompanyCode)
        //{
        //    //query using SPASv1Context
        //    try
        //    { 
        //        //                var vlist = await _context.TblPaymentrequesthdr.FromSqlRaw("select top 1 * from tblpaymentrequesthdr where companycode ='" + companycode + "' and deptcode='" + branchcode + "' order by auditdate desc").FirstOrDefaultAsync();
        //       return await _context.TblDataSourceHdr.Where(n => n.BatchName.Contains("PR")).OrderByDescending(n => n.AuditDate).Select(n => n.BatchName).FirstOrDefaultAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task<TblResponse> Test(TblDataSourceDtl TblDataSourceDtl)
        { 
            try
            {
                var DbContext = DbContextFactory.Create("TFCMCI");
                // return await Task.FromResult(DbContext.Set<ValReturn<string>>()
                //  .FromSqlRaw("select isnull(VendorID,'Empty') as value from TblVendor  where VendorID='ELEC_CUBA'").AsEnumerable()
                //.FirstOrDefault().Value); 
                _AbstractRepositoryDtSourceDtl = new AbstractRepository<TblDataSourceDtl>(DbContext);
                await _AbstractRepositoryDtSourceDtl.Insert(TblDataSourceDtl);

                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TblResponse> EndorseDataSourceHdr(TblDataSourceHdr entity, string CompanyCode)
        {
            //var dynamicConnectionString = "Server=192.168.5.25;Database=TFCMCI;User Id=sa;Password=1970SPLPi@2o24Spg53;TrustServerCertificate=True;";
            //var options = new DbContextOptionsBuilder<SPASv1Context>()
            //    .UseSqlServer(dynamicConnectionString)
            //    .Options;

            try
            {
                var DbContext = DbContextFactory.Create(CompanyCode);
                _AbstractRepositoryDtSourceHdr = new AbstractRepository<TblDataSourceHdr>(DbContext);
                await _AbstractRepositoryDtSourceHdr.Insert(entity);
                //}

                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TblResponse> EndorseDataSourceDtl(TblDataSourceDtl DtSourceDtl,string CompanyCode)
        {
            try
            {
                var DbContext = DbContextFactory.Create(CompanyCode);
                _AbstractRepositoryDtSourceDtl = new AbstractRepository<TblDataSourceDtl>(DbContext);
                await _AbstractRepositoryDtSourceDtl.Insert(DtSourceDtl);
                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {
                string error = "Error - " + Utilities.Getprojectname + " - " + Utilities.GetmethodName() + " - " + ex.Message;
                logger.LogError(error);

                //await _RepositoryUnit.ResponseRepository.CreateResponse(pscode, "FAILED", ex.Message, Utilities.GetmethodName());
                //return BadRequest(_response);
                return _response;
            }
        }

        public async Task<string> Get_BankValueByBankCode(string _BankCode)
        {
            try
            { 
                return await Task.FromResult(_context.Set<ValReturn<string>>()
                  .FromSqlRaw("select BankCode as Value From RefPNBInstapayBank where BankCode = '" + _BankCode + "' and Active=1").AsEnumerable()
                .First().Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> CheckClassIDByDesc(string _ClassID)
        {
            try
            { 
                return await Task.FromResult(_context.Set<ValReturn<string>>()
                  .FromSqlRaw("select RequestDesc as Value From RefPaymentRequestType where UPPER(RequestDesc) = '" + _ClassID.ToUpper() + "'").AsEnumerable()
                .First().Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        } 
        public async Task<string> CheckCOADept(string _DeptCode)
        {
            try
            { 
                return await Task.FromResult(_context.Set<ValReturn<string>>()
                  .FromSqlRaw("select DeptCode as Value From TblChartOfAccountsDepartment where UPPER(DeptCode) = '" + _DeptCode.ToUpper() + "'").AsEnumerable()
                .First().Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetAcctCodeByDeptCode(string deptCode,string CompanyCode)
        {
            try
            {
                var DbContext = DbContextFactory.Create(CompanyCode);
                _AbstractRepositoryDtSourceDtl = new AbstractRepository<TblDataSourceDtl>(DbContext);
                return await Task.FromResult(DbContext.Set<ValReturn<string>>()
                  .FromSqlRaw("select AccountCode as Value From TblChartOfAccountsDepartment where UPPER(DeptCode) = '" + deptCode.ToUpper() + "'").AsEnumerable()
                .First().Value);
            }
            catch (Exception ex)
            {
                throw new Exception( "Department Code error: " + deptCode + ", Company Code: " + CompanyCode.Replace("T","") + ", Error : " + ex.Message);
            }
        }
        public async Task<string> CheckAcctDeptCode(string AcctDeptCode)
        {
            try
            { 
                return await Task.FromResult(_context.Set<ValReturn<string>>()
                  .FromSqlRaw("select isnull(AccountCode,'') as Value From TblChartOfAccountsDepartment where UPPER(AccountCode) = '" + AcctDeptCode.ToUpper() + "'").AsEnumerable()
                .First().Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public string (string VendorCode,string companyCode)
        //{
        //    tryCheckVendor
        //    {
        //        var DbContext = DbContextFactory.Create(companyCode);
        //        _AbstractRepositoryDtSourceDtl = new AbstractRepository<TblDataSourceDtl>(DbContext);

        //        return await Task.FromResult(DbContext.Set<ValReturn<string>>()
        //          .FromSqlRaw("select AccountCode as Value From TblChartOfAccountsDepartment where UPPER(DeptCode) = '" + deptCode.ToUpper() + "'").AsEnumerable()
        //        .First().Value);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Department Code error: " + deptCode + ", Company Code: " + CompanyCode.Replace("T", "") + ", Error : " + ex.Message);
        //    }
        //}
    }
}
