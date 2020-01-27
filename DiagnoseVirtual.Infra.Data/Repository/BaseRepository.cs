using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Domain.Interfaces;
using NHibernate;
using System.Linq;

namespace DiagnoseVirtual.Infra.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private ISession session;

        public BaseRepository(ISession session)
        {
            this.session = session;
        }

        public void Insert(T obj)
        {
            session.Save(obj);
        }

        public void Update(T obj)
        {
            session.Update(obj);
        }

        public void Remove(int id)
        {
            var obj = session.Query<T>().FirstOrDefault(x => x.Id == id);
            session.Delete(obj);
        }

        public IQueryable<T> SelectAll()
        {
            return session.Query<T>();
        }

        public T Select(int id)
        {
            return session.Query<T>().FirstOrDefault(x => x.Id == id);
        }
    }
}
