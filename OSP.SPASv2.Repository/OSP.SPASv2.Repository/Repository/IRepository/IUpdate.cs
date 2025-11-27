using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSP.SPASv2.Repository.IRepository
{
    public interface IUpdate<TEntity> where TEntity : class
    {
        public void Update(TEntity entity);
    }
}

