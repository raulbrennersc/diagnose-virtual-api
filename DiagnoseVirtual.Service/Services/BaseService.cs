using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Domain.Interfaces;
using DiagnoseVirtual.Infra.Data.Context;
using DiagnoseVirtual.Infra.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiagnoseVirtual.Service.Services
{
    public class BaseService<T> : IService<T> where T : BaseEntity
    {
        private readonly BaseRepository<T> repository;
        public BaseService(PsqlContext context)
        {
            repository = new BaseRepository<T>(context);
        }

        public T Post(T obj)
        {
            repository.Insert(obj);
            return obj;
        }

        public List<T> Post(List<T> objs)
        {
            repository.Insert(objs);
            return objs;
        }

        public T Put(T obj)
        {
            repository.Update(obj);
            return obj;
        }

        public void Delete(int id)
        {
            if (id == 0)
                throw new ArgumentException("The id can't be zero.");

            repository.Remove(id);
        }

        public IQueryable<T> GetAll() => repository.SelectAll();

        public T Get(int id)
        {
            if (id == 0)
                throw new ArgumentException("The id can't be zero.");

            return repository.Select(id);
        }
    }
}
