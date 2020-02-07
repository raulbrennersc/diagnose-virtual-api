using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Domain.Interfaces;
using DiagnoseVirtual.Infra.Data.Context;
using System.Collections.Generic;
using System.Linq;

namespace DiagnoseVirtual.Infra.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly PsqlContext context;
        public BaseRepository(PsqlContext context)
        {
            this.context = context;
        }


        public void Insert(T obj)
        {
            context.Set<T>().Add(obj);
            context.SaveChanges();
        }

        public void Insert(List<T> objs)
        {
            context.Set<T>().AddRange(objs);
            context.SaveChanges();
        }

        public void Update(T obj)
        {
            context.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }

        public void Remove(int id)
        {
            context.Set<T>().Remove(Select(id));
            context.SaveChanges();
        }

        public IQueryable<T> SelectAll()
        {
            return context.Set<T>();
        }

        public T Select(int id)
        {
            return context.Set<T>().Find(id);
        }
    }
}
