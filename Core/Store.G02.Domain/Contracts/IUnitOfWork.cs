using Store.G02.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Domain.Contracts
{
    public interface IUnitOfWork
    {
        #region Summary
        // Generate Repository responsible for : return any repository from any type
        // Save Changes 
        #endregion
        IGenericRepository<Tkey, TEntity> GetRepository<Tkey, TEntity>() where TEntity : BaseEntity<Tkey>;

        Task<int> SaveChangesAsync();
    }
}
