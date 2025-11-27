using OSP.Common.Domain;

namespace OSP.Common.Repository.Rules
{
    public class UserRules : IRules<TblUser>
    {
        public bool CanCreate(TblUser entity)
        {
            throw new NotImplementedException();
        }

        public bool CanDelete(TblUser entity)
        {
            throw new NotImplementedException();
        }

        public bool CanRead(TblUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<string> CanReadAsync(TblUser entity)
        {
            throw new NotImplementedException();
        }

        public bool CanUpdate(TblUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
