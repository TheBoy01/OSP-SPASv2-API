using OSP.SPASv2.Domain.Tables;

namespace OSP.SPASv2.Repository.Repository.IRepository.Table
{
    public interface ItmpPaymentRequestInventoryRepository<TEntity> where TEntity : class
    {

        public Task<IList<tmpPaymentRequestInventory>> GettmpPaymentRequestInventory();
        public Task<TblResponse> PosttmpPaymentRequestInventory(tmpPaymentRequestInventory tmp);
        public Task<IList<tmpPaymentRequestInventory>> GettmpPaymentRequestInventoryA(string audituser,string prno);
    }
}
