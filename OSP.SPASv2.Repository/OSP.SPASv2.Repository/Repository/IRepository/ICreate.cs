using Microsoft.AspNetCore.Mvc;
using OSP.SPASv2.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSP.SPASv2.Repository.IRepository
{
    public interface ICreate<TEntity> where TEntity : class
    {
        public Task<RepositoryResponse>  Create(TEntity entity);
    }
}
