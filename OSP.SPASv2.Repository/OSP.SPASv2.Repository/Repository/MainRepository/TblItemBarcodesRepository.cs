using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository.MainRepository
{
    public class TblItemBarcodesRepository : ITblItemBarcodesRepository<TblItemBarcodes>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<TblItemBarcodes> _AbstractRepository;
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
        public TblItemBarcodesRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<TblItemBarcodes>(_context);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Functions

        #region Public Methods

        public async Task<TblResponse> BulkInsert(List<TblItemBarcodes> entity)
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

        public async Task<TblResponse> Create(TblItemBarcodes entity)
        {
            await _AbstractRepository.Insert(entity);
            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> Update(TblItemBarcodes entity)
        {
            var oldEntity = await _AbstractRepository.GetByID(entity.BarCode);
            _AbstractRepository.Update(oldEntity, entity);
            //new Task(() => { TrailEdit(oldEntity, entity, "TblRequisitionhdr", "Requisitionhdr", entity.Reqno); }).Start();
            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> Delete(TblItemBarcodes entity)
        {
            await _AbstractRepository.Delete(entity);
            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> Delete(object Primarykey, object Primarykey2)
        {
            await _AbstractRepository.DeleteByComposite(Primarykey, Primarykey2);
            return await Task.FromResult(_response);
        }
        #endregion

        public async Task<TblItemBarcodes> GetLatestBarCode(TblVendor Vendor, string ItemCode)
        {
            try
            {
                string YearMonth = string.Empty;
                YearMonth = Vendor.Prefix + ItemCode + DateTime.Now.ToString("yyMM");
                return await _context.TblItemBarcodes.FromSqlRaw("select top 1 * from TblItemBarcodes where left(BarCode," + YearMonth.Length + ")= '" + YearMonth + "' and VendorCode='" + Vendor.VendorCode + "' and ItemCode='" + ItemCode + "' order by BarCode desc").FirstOrDefaultAsync();

                //var vlist = await _context.TblRequisitionhdr.FromSqlRaw("select top 1 * from TblRequisitionHdr where companycode ='" + companycode + "'  order by auditdate desc").FirstOrDefaultAsync()
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task <List<TblItemBarcodes>> GetTblItemBarcodesAsync(string POno)
        {
            try
            {
                var vlist = await _context.TblItemBarcodes.Where(x => x.PONo == POno).ToListAsync();

                return
                    vlist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<TblItemBarcodes>> GetBarCodes(string PONo)
        {
            try
            {
                return await _context.TblItemBarcodes.Where(a=>a.PONo.Equals(PONo)).ToListAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<qryPOBarcodesSummary>> GetBarCodesSummary(string PONo)
        {
            try
            {
                return await _context.qryPOBarcodesSummary.FromSqlRaw("exec [sp_GetPONoBarcodes] "+ PONo +"").ToListAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<TblResponse> BulkCreate(List<TblItemBarcodes> entity)
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

       



        #endregion
    }
    }
