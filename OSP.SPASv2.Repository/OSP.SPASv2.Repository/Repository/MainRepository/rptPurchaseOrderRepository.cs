using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository
{
    public class rptPurchaseorderRepository : IRptPurchaseOrderRepository<RptPurchaseorder>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RptPurchaseorder> _AbstractRepository;
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
        public rptPurchaseorderRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RptPurchaseorder>(_context);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        public async Task<TblResponse> Create(RptPurchaseorder entity)
        {
            await _AbstractRepository.Insert(entity);
            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> Update(RptPurchaseorder entity)
        {
            //var oldEntity = this.Read(entity.VendorCode);
            //_AbstractRepository.Update(oldEntity, entity);
            //new Task(() => { TrailEdit(oldEntity, entity, "RptPurchaseorder", "Purchaseorder", entity.VendorCode); }).Start();
            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> Delete(RptPurchaseorder entity)
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
        #region Public Functions

        public async Task<IList<RptPurchaseorder>> GetAllObjects()
        {
            try
            {
                var vlist = await Task.FromResult(_context.RptPurchaseorder.FromSqlRaw("select * from RptPurchaseorder").ToList());
                return vlist;
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        public async Task<List<RptPurchaseorder>> GetListByPONo(string PONo)
        {
            try
            {
                return await _context.RptPurchaseorder.Where(a => a.PONo.Equals(PONo)).OrderBy(a => a.Description).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

    }
}

