using DiagnoseVirtual.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DiagnoseVirtual.Domain.Interfaces
{
    public interface IService<T> where T : BaseEntity
    {
        T Post(T obj);

        T Put(T obj);

        void Delete(int id);

        T Get(int id);

        IQueryable<T> Get();
    }
}
