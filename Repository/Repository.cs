using AvocoBackend.Data.Models;
using AvocoBackend.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AvocoBackend.Repository
{
	public class Repository<T> : IRepository<T> where T:class//where T:BaseModel
	{
		private readonly ApplicationDbContext _context;
		private DbSet<T> _dbSet;

		public Repository(ApplicationDbContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}

		public bool Exists(Expression<Func<T, bool>> expression)
		{
			return _dbSet.Any(expression);
		}

		public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
		{
			IQueryable<T> query = _dbSet;
			foreach (var include in includes)
			{
				query = query.Include(include);
			}
			return query;
		}

		public IEnumerable<T> GetAllBy(Expression<Func<T, bool>> getBy, params Expression<Func<T, object>>[] includes)
		{
			IQueryable<T> query = _dbSet;
			foreach (var include in includes)
			{
				query = query.Include(include);
			}
			var result = query.Where(getBy);
			return result;
		}

		public T GetBy(Expression<Func<T, bool>> getBy, params Expression<Func<T, object>>[] includes)
		{
			IQueryable<T> query = _dbSet;
			foreach (var include in includes)
			{
				query = query.Include(include);
			}
			var result = query.FirstOrDefault(getBy);
			return result;
		}

		public void Insert(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			_dbSet.Add(entity);
			_context.SaveChanges();
		}

		public void Update(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			_dbSet.Update(entity);
			_context.SaveChanges();
		}

		public void Delete(Expression<Func<T, bool>> expression)
		{
			var entity = _dbSet.SingleOrDefault(expression);
			if (entity == null)
			{
				throw new NullReferenceException();
			}
			_dbSet.Remove(entity);
			_context.SaveChanges();
		}
		public void Delete(T entity)
		{
			if (entity == null)
			{
				throw new NullReferenceException();
			}
			_dbSet.Remove(entity);
			_context.SaveChanges();
		}
		public void SaveChanges()
		{
			_context.SaveChanges();
		}
	}
}
