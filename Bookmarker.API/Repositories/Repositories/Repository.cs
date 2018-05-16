﻿using Models.Abstracts;
using Repositories.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace Repositories.Repositories
{
    public class Repository<T> : IRepository<T> where T : ABaseEntity
    {
        private readonly IDbContext _dbContext;
        private IDbSet<T> _entities;

        public Repository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual IQueryable<T> Table => Entities.AsQueryable();

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                Entities.Remove(entity);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                // log
                throw e;
            }
        }

        public T GetById(object id)
        {
            return Entities.Find(id);
        }

        public void Insert(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                Entities.Add(entity);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                // log
                throw e;
            }
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                // log
                throw e;
            }
        }

        private IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _dbContext.Set<T>();
                }
                return _entities;
            }
        }
    }
}
