using OSP.Common.Repository.Context;
using OSP.Common.Repository.Repository;
using OSP.SPASv2.Domain.References;
using System.Text;

namespace OSP.Common.Repository.Rules
{
    public class DeptartmentRules : IRules<qryCompanyDetails>
    {
        StringBuilder sb = new StringBuilder();
        RepositoryUnit _RepositoryUnit;

        public DeptartmentRules(OSPContext _context)
        {
            _RepositoryUnit = new RepositoryUnit(_context);
        }

        public bool CanCreate(qryCompanyDetails entity)
        {
            throw new NotImplementedException();
        }

        public bool CanDelete(qryCompanyDetails entity)
        {
            throw new NotImplementedException();
        }

        public bool CanRead(qryCompanyDetails entity)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CanReadAsyncList(List<qryCompanyDetails> entity)
        {
            sb = new StringBuilder();
           
            try
            {

                //var payclass = await _RepositoryUnit..GetGetPayclassCodeByDesc(entity._batchUploadParams.Payclass);
                //if (string.IsNullOrEmpty(payclass))
                //{
                //    sb.AppendLine("" + entity._batchUploadParams.Payclass + " is not existing.");
                //}

                for (int i = 0; i < entity.Count; i++)
                {
                   
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Task<string> CanReadAsync(qryCompanyDetails entity)
        {
            throw new NotImplementedException();
        }

        public bool CanUpdate(qryCompanyDetails entity)
        {
            throw new NotImplementedException();
        }
    }
}
