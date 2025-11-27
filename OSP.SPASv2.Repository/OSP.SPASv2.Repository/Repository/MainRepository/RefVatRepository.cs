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
    public class VatRepository : IRefVatRepository<RefVat>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefVat> _AbstractRepository;
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
        public VatRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefVat>(_context);
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        //public async Task<TblResponse> Create(RefVat entity)
        //{
        //    await _AbstractRepository.Insert(entity);
        //    return await Task.FromResult(_response);
        //}

        //public async Task<TblResponse> Update(RefVat entity)
        //{
        //    var oldEntity = this.Read(entity.VendorCode);
        //    _AbstractRepository.Update(oldEntity, entity);
        //    new Task(() => { TrailEdit(oldEntity, entity, "RefVat", "Vat", entity.VendorCode); }).Start();
        //    return await Task.FromResult(_response);
        //}

        //public async Task<TblResponse> Delete(RefVat entity)
        //{
        //    await _AbstractRepository.Delete(entity);
        //    return await Task.FromResult(_response);
        //}

        //public async Task<TblResponse> Delete(object Primarykey, object Primarykey2)
        //{
        //    await _AbstractRepository.DeleteByComposite(Primarykey, Primarykey2);
        //    return await Task.FromResult(_response);
        //}

        #endregion
        #region Public Functions

        public async Task<IList<RefVat>> GetAllObjects()
        {
            try
            {
                var vlist = await Task.FromResult(_context.RefVat.FromSqlRaw("select * from RefVat").ToList());
                return vlist;
            }
           catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        public async Task<RefVat> GetRefVat()
        {
            try
            {
                //var vlist = await _context.RefVat.FromSqlRaw("select * from RefVat where active=1").FirstOrDefaultAsync();
                //return vlist;
                return await _context.Set<RefVat>()
                 .AsNoTracking()
                 .FirstOrDefaultAsync(e => e.Active == true);


                
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        #endregion

    }
}

