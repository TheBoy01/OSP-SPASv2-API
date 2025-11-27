using SPASv2.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.SPASv2.Repository.IRepository;
using SPASv2.Context;
using System.Text;
using OSP.SPASv2.Domain.Tables;
using System.Reflection;
using OSP.SPASv2.Repository.Repository.IRepository.Table;
using static SPASv2.Context.SPASv2Context;
using OSP.Common.Domain.View;

namespace OSP.SPASv2.Repository.Repository
{
    public class RefSystemsRepository : IRefSystemsRepository<RefSystems>
    {

        #region Private Member Variables

        private SPASv2Context _context;
        AbstractRepository<RefSystemsRepository> _AbstractRepository;
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
        public RefSystemsRepository(SPASv2Context context)
        {
            _context = context;
            _AbstractRepository = new AbstractRepository<RefSystemsRepository>(_context);
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

        public async Task<IList<RefSystems>> GetAllObjects()
        {
            try
            {
                var vlist = await Task.FromResult(_context.RefSystems.FromSqlRaw("select * from RefSystems").ToList());
                return vlist;
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        public Task<RefSystems> GetRefSystems()
        {
            throw new NotImplementedException();
        }

        public async Task<DateTime> GetSaveLastDate()
        {
            var result = await Task.FromResult(_context.Set<ValReturn<DateTime>>()
                .FromSqlRaw("select LastSave as Value from refsystems")
                .AsEnumerable()
                .First().Value);
            return result;
        }

        public async Task<string> GetSystemCode()
        {
            try
            {
                return await Task.FromResult(_context.RefSystems.Select(a => a.SystemCode).FirstOrDefault());
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }
        #endregion

    }
}

