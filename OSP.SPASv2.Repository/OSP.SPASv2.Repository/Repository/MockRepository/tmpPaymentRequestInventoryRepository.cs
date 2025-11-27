using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Repository.Repository.IRepository.Table;

namespace OSP.SPASv2.Repository.Repository.MockRepository   
{
    public class tmpPaymentRequestInventoryRepository : ItmpPaymentRequestInventoryRepository<tmpPaymentRequestInventory>
    {
        public Task<IList<tmpPaymentRequestInventory>> GettmpPaymentRequestInventory()
        {
            throw new NotImplementedException();
        }

        public Task<IList<tmpPaymentRequestInventory>> GettmpPaymentRequestInventoryA(string audituser,string prno)
        {
            throw new NotImplementedException();
        }

        public Task<TblResponse> PosttmpPaymentRequestInventory(tmpPaymentRequestInventory tmp)
        {
            throw new NotImplementedException();
        }
    }
}
