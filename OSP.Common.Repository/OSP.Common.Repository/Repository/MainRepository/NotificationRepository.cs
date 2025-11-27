using Azure;
using Common.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using OSP.Common.Domain.Tables;
using OSP.Common.Repository.Context;
using OSP.Common.Repository.IRepository;

namespace OSP.Common.Repository.Repository.MainRepository
{
    public class NotificationRepository
    {
        private OSPContext _context;
        AbstractRepository<TblNotification> _AbstractRepository;
        TblResponse _response = new TblResponse();
        public Task<TblUser> GetUserDetails(TblUser entity)
        {
            throw new NotImplementedException();
        }

        public NotificationRepository(OSPContext context)
        {
            _context = context;
            _context = DbContextFactory.Create("SPLPDEVSERVER");
            _AbstractRepository = new AbstractRepository<TblNotification>(_context);
        }

        public async Task<TblResponse> CreateNotification(TblNotification entity)
        {
            await _AbstractRepository.Insert(entity);
            
            return await Task.FromResult(_response);
        }

        public async Task<IList<TblRecipient>> GetRecipient(string systemcode)
        {
            try
            {
                var vlist = await Task.FromResult(_context.TblRecipient.FromSqlRaw("select * from TblRecipient where systemcode = '" + systemcode + "' and active = 1").ToList());
                return vlist;
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        public async Task<IList<TblNotification>> GetNotifications()
        {
            try
            {
                var vlist = await Task.FromResult(_context.TblNotification.Where(a => a.StatusCode != "SUCCESS").ToList());
                return vlist;
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

        public async Task<IList<TblNotification>> UpdateNotifications()
        {
            try
            {
                var vlist = await Task.FromResult(_context.TblNotification.Where(a => a.StatusCode != "SUCCESS").ToList());
                return vlist;
            }
            catch (Exception es)
            {
                throw new Exception(es.Message);
            }
        }

    }
}
