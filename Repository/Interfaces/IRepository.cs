using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AvocoBackend.Repository.Interfaces
{
    public interface IRepository<T> //T to jeden z modeli
    {
		IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);
		IEnumerable<T> GetAllBy(Expression<Func<T, bool>> getBy, params Expression<Func<T, object>>[] includes);
		T GetBy(Expression<Func<T, bool>> getBy, params Expression<Func<T, object>>[] includes);
		bool Exists(Expression<Func<T, bool>> expression);
		int Insert(T entity);
		void Update(T entity);
		void Delete(Expression<Func<T, bool>> expression);
    }
}
