using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
//using Microsoft.Data.SqlClient;

namespace OSP.SPASv2.Repository.Repository
{
    public class PaymentrequesthdrRepository : IPaymentRequestHdrRepository<TblPaymentrequesthdr>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblPaymentrequesthdr> _AbstractRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;
        //PaymentrequesthdrRules vrules = new PaymentrequesthdrRules();

        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public PaymentrequesthdrRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblPaymentrequesthdr>(_context);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods



        public async Task<TblResponse> CreatePaymentRequest(TblPaymentrequesthdr entity)
        {
            try
            {
                await _AbstractRepository.Insert(entity);
                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<TblPaymentrequesthdr> GetLatestPRRow(string companycode, string branchcode)
        {
            try
            {

                var vlist = await _context.TblPaymentrequesthdr.FromSqlRaw("select top 1 * from tblpaymentrequesthdr where companycode ='" + companycode + "' and deptcode='" + branchcode + "' order by auditdate desc").FirstOrDefaultAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetLatestPRNo(string companycode, string branchcode)
        {
            try
            {

               
                var vlist = await _context.TblPaymentrequesthdr
                    .Where(n => n.CompanyCode == companycode && n.DeptCode == branchcode)
                    .OrderByDescending(n=>n.AuditDate).Select(n=>n.PRNo).FirstOrDefaultAsync();
                return vlist;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<TblResponse> UpdateRequestByRequestNo(string paymentRequestNo)
        {
            try
            {
                await _context.TblPaymentrequesthdr.FromSqlRaw("select top 1 * from tblpaymentrequesthdr where companycode").FirstOrDefaultAsync();
                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message);
            }
        }

        public async Task<TblPaymentrequesthdr> ReadRequestByPRNo(string _PRNo)
        {
            try
            {
               return await _context.TblPaymentrequesthdr.Where(a => a.PRNo == _PRNo).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {  
                throw new Exception(ex.Message);
            }
        }

        public async Task<qryPaymentRequestHdr> GetPaymentrequesthdr(string prno)
        {
            try
            {
                //await _context.TblPaymentrequesthdr..FirstOrDefaultAsync();
                return await _context.qryPaymentRequestHdr.FirstOrDefaultAsync(s => s.PRNo == prno);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<TblResponse> UpdateTblPaymentRequestHdr(TblPaymentrequesthdr entity)
        {
            try
            {
                await _AbstractRepository.Update(entity);
                return await Task.FromResult(_response);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        //public async Task<DummyStr> GetPRNo(string companycode, string branchcode)
        //{
        //    try
        //    {
        //        var s = Task.FromResult(_context.DummyStr.FromSqlRaw("exec CreatePRNo @p0, @p1", companycode, branchcode));
        //        //var s =  await _context.DummyStr.FromSqlRaw("exec CreatePRNo @p0, @p1", companycode, branchcode).FirstOrDefaultAsync();

        //        //string str = s.Select(s => s.str);
        //        //var prNo = _context.Set<string>().FromSqlRaw("exec CreatePRNo '{0}', '{1}'", companycode, branchcode).FirstOrDefault();
        //        //var prNo = await _context.DummyStr.FromSqlRaw("exec CreatePRNo '{0}', '{1}'", companycode, branchcode);

        //        return Task.FromResult(s);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }

        //}






        #endregion
        #region Public Functions

        //public async Task<IList<TblPaymentrequesthdr>> GetAllObjects()
        //{
        //    Try
        //    {
        //        var vlist = await Task.FromResult(_context.TblPaymentrequesthdr.FromSqlRaw("select * from TblPaymentrequesthdr").ToList());
        //        return vlist;
        //    }
        //   catch (Exception es)
        //    {
        //        throw new Exception(es.Message);
        //    }
        //}

        #endregion

    }
}

