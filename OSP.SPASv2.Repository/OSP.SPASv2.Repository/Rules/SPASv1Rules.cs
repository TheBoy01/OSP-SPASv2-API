using OSP.SPASv2.Repository.Parameters;
using OSP.SPASv2.Domain.Tables;
using System.Web.Http.ModelBinding;
using System.Text;

namespace OSP.SPASv2.Repository.Rules
{
    public class SPASv1Rules : IRules<SPASv1Params>
    {
        StringBuilder sb = new StringBuilder();

        public string CanCreate(SPASv1Params entity)
        {
            sb = new StringBuilder();
            try
            {
                if (entity.TblDataSourceHdr.BatchName == "A")
                {
                    sb.Append("Invalid Vendor Code");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return sb.ToString();
        }

        public string CanDelete(SPASv1Params entity)
        {
            throw new NotImplementedException();
        }

        public string CanRead(SPASv1Params entity)
        {
            throw new NotImplementedException();
        }

        public string CanUpdate(SPASv1Params entity)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CanEndorse(SPASv1Params entity)
        {
            sb = new StringBuilder();
            try
            {
                if (entity.PayMethodDesc.ToUpper() != "CHEQUE")
                {
                    if (string.IsNullOrEmpty(entity.BankCode) || string.IsNullOrWhiteSpace(entity.BankCode) || string.IsNullOrEmpty(entity.BankAcctNo) || string.IsNullOrWhiteSpace(entity.BankAcctNo))
                    {
                        sb.Append("Bank Account is not valid.");
                    }
                }
                if (!entity.IsClassIdExist)
                {
                    sb.Append("Class ID is not valid.");
                }
                if (!entity.IsCOADeptExist)
                {
                    sb.Append("Department Code is not valid.");
                }
            }
            catch (Exception ex)
            {
                sb.Append(ex.Message.ToString());
            }

            return sb.ToString();
        }

        public Task<string> CanReadAsync(SPASv1Params entity)
        {
            throw new NotImplementedException();
        }

        public Task<string> CanDeleteAsync(SPASv1Params entity)
        {
            throw new NotImplementedException();
        }
    }
}
