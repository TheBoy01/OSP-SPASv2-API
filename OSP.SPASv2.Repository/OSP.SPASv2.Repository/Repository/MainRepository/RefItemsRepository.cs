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
    public class RefItemsRepository  : IRefItemsRepository<RefItems>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefItems> _AbstractRepository;
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
        public RefItemsRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefItems>(_context);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        public async Task<TblResponse> Create(RefItems entity)
        {
            await _AbstractRepository.Insert(entity);
            return await Task.FromResult(_response);
        }

        public async Task<RefItems> Read(object Primarykey)
        {
            
            return await _AbstractRepository.GetByIDAsync(Primarykey);
        }

        public async Task<TblResponse> Update(RefItems entity)
        {
            //var oldEntity = this.Read(entity.ItemCode);
            //_AbstractRepository.Update(oldEntity, entity);
            //new Task(() => { TrailEdit(oldEntity, entity, "RefItems", "Items", entity.ItemCode); }).Start();
            return await Task.FromResult(_response);
        }

        public async Task<TblResponse> Delete(RefItems entity)
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

        public async Task<IList<RefItems>> GetAllObjects()
        {
            try
            {
                var vlist = await Task.FromResult(_context.RefItems.FromSqlRaw("select * from RefItems").ToList());
                return vlist;
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        public async Task<string> GetItemCodeByDesc(string itemDesc)
        {
            try
            {
                //string vlist = await _context.RefCompany.FromSqlRaw("select companycode from RefCompany where active=1 and companydesc like '"+ company + "'").FirstOrDefaultAsync();
                var desc = await Task.FromResult(_context.RefItems.Where(p => p.ItemDesc.Equals(itemDesc))
                                                                   .Select(p => p.ItemCode).FirstOrDefault());
                return desc;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetItemDesc(string itemcode)
        {
            try
            {
                //string vlist = await _context.RefCompany.FromSqlRaw("select companycode from RefCompany where active=1 and companydesc like '"+ company + "'").FirstOrDefaultAsync();
                var desc = await Task.FromResult(_context.RefItems.Where(p => p.ItemCode.Equals(itemcode))
                                                                   .Select(p => p.ItemDesc).FirstOrDefault());
                return desc;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

    }
}

