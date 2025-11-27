using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using OSP.Common.Domain.View;
using DocumentFormat.OpenXml.InkML;
//using System.Data.Entity;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class TblPurchaseorderhdrRepository : ITblPurchaseorderhdrRepository<TblPurchaseorderhdr>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblPurchaseorderhdr> _AbstractRepository;
        //GenericRepository<TblPurchaseorderhdr> _GenericRepository;
        TblResponse _response = new TblResponse();
        StringBuilder sb;


        #endregion

        #region Private Properties

        #endregion

        #region Private Methods

        #endregion

        #region Private Function

        #endregion

        #region Constructors
        public TblPurchaseorderhdrRepository(SPASv2Context context)
        {
            _context = context;

            _AbstractRepository = new AbstractRepository<TblPurchaseorderhdr>(_context);
            //_GenericRepository = new GenericRepository<TblPurchaseorderhdr>(_context);
        }

        public async Task<TblResponse> CreateTblPurchaseOrderHdr(TblPurchaseorderhdr entity)
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

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods



        public async Task<List<qryPOHdr>> GetPONo()
        {
            try
            {
                //var result = await _context.TblPurchaseOrderHdr
                //    .Join(
                //        _context.TblItemBarcodes,
                //        po => po.PONo,           
                //        it => it.PONo,          
                //        (po, it) => new qryPOHdr  
                //        {
                //            PONo = po.PONo,
                //            PODate = po.PODate,
                //            Remarks = po.Remarks
                //        }
                //    ).Distinct().ToListAsync();
               // return result;
                return await _context.qryPOHdr.FromSqlRaw("EXEC sp_GetPONo")
           .ToListAsync();

               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<TblPurchaseorderhdr> GetPOHdrByPONo(string PONo)
        {
            try
            {
                var result = await _context.TblPurchaseOrderHdr.FromSqlRaw("Select * From TblPurchaseOrderHdr where PONo='" + PONo + "'").FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<TblPurchaseorderhdr> GetPOHdrByReqNo(string ReqNo)
        {
            try
            {
                return await _context.TblPurchaseOrderHdr.FromSqlRaw("Select * From TblPurchaseOrderHdr where Reqno='" + ReqNo + "'").FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<TblPurchaseorderhdr> GetPObyMainReqno(string ReqNo)
        {
            try
            {
                var vlist = await _context.TblPurchaseOrderHdr.FromSqlRaw("select b.* from TblRequisitionHdr a inner join TblPurchaseOrderHdr b on a.MainReqNo=b.Reqno where a.mainReqNo='" + ReqNo + "'").FirstOrDefaultAsync();

                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<TblPurchaseorderhdr> GETPObyReqNo(string reqno)
        {
            try
            {
                var pohdr = await _context.TblPurchaseOrderHdr.FromSqlRaw("select * from TblPurchaseorderhdr where reqno='" + reqno + "' ").FirstOrDefaultAsync();
                return pohdr;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<string> GetLatestPONo(string CompanyCode)
        {
            var result = await Task.FromResult(_context.Set<ValReturn<string>>()
               .FromSqlRaw("sp_GeneratePONo '" + CompanyCode + "'")
               .AsEnumerable()
               .First().Value);
            return result;
        }

        public async Task<TblResponse> UpdatePOPrice(string PONo, string ItemCode, decimal TempPriceAmount)
        {
            try
            {
                await Task.FromResult(_context.Database.ExecuteSqlRaw("exec sp_UpdatePOPrice '" + PONo + "','" + ItemCode + "'," + TempPriceAmount + ""));
                _response = new TblResponse
                {
                    Status = "SUCCESS",
                    AuditDate = DateTime.Now,
                    ErrorMessage = "SUCCESS",
                    MethodName = "SP UPDATE PO PRICE",
                    TrxNo = PONo,
                    UniqueInfo = "1"
                };

                return await Task.FromResult(_response);
            }
            catch (Exception e)
            {
                _response = new TblResponse
                {
                    Status = "FAILED",
                    AuditDate = DateTime.Now,
                    ErrorMessage = "ERROR: " + e.Message,
                    MethodName = "SP UPDATE PO PRICE",
                    TrxNo = PONo,
                    UniqueInfo = "1"
                };

                return _response;
            }
        }

        #endregion
        #region Public Functions
        #endregion

    }
}
