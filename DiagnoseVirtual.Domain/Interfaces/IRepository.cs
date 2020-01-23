using DiagnoseVirtual.Domain.Entities;
using System.Linq;

namespace DiagnoseVirtual.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Insert(T obj);

        void Update(T obj);

        void Remove(int id);

        T Select(int id);

        IQueryable<T> SelectAll();
    }
}
