using Microsoft.EntityFrameworkCore;
using signalr_best_practice_api_models;
using signalr_best_practice_core.Entities.Base;
using signalr_best_practice_core.Extensions;
using signalr_best_practice_core.Interfaces.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace signalr_best_practice_dl_postgresql.Repositories
{
    public class PostgreSqlRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
               where TEntity : BaseEntity<TKey>, new()
    {
        private DbSet<TEntity> _entities { get; set; }
        private DbContext _context { get; set; }

        public PostgreSqlRepository(IDbContext context)
        {
            _context = context as DbContext;
            _entities = _context.Set<TEntity>();
        }

        public Task<TEntity> GetById(TKey id)
        {
            var first = _entities.FirstOrDefault(x => x.Id.Equals(id));

            if (first == null)
            {
                throw new ArgumentException($"Entity is not found");
            }

            return Task.FromResult(first);
        }

        public Task Insert(TEntity entity)
        {
            entity.Created = DateTime.UtcNow;
            entity.Updated = entity.Created;
            entity.TrySetId();

            _entities.Add(entity);
            _context.SaveChanges();

            return Task.FromResult("Ok");
        }

        public Task Insert(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.Created = DateTime.UtcNow;
                entity.Updated = entity.Created;
                entity.TrySetId();

                _entities.Add(entity);
            }

            _context.SaveChanges();

            return Task.FromResult("Ok");
        }

        public Task Update(TEntity entity, Expression<Func<TEntity, bool>> search = null)
        {
            if (search == null)
            {
                search = x => x.Id.Equals(entity.Id);
            }

            var first = _entities.FirstOrDefault(search);
            if (first == null)
            {
                throw new ArgumentException($"Entity is not found");
            }

            if (first != entity)
            {
                first.Copy(entity);
                first.Updated = DateTime.UtcNow;
            }
            else
            {
                entity.Updated = DateTime.UtcNow;
            }

            _context.SaveChanges();
            return Task.FromResult("Ok");
        }

        public Task Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                var first = _entities.FirstOrDefault(x => x.Id.Equals(entity.Id));
                if (first == null)
                {
                    throw new ArgumentException($"Entity {entity.Id} is not found");
                }

                first.Copy(entity);
                first.Updated = DateTime.UtcNow;

                _entities.Update(first);
            }

            _context.SaveChanges();

            return Task.FromResult("Ok");
        }

        public Task Delete(TKey id)
        {
            var first = _entities.FirstOrDefault(x => x.Id.Equals(id));
            if (first == null)
            {
                throw new ArgumentException($"Entity is not found");
            }

            _entities.Remove(first);
            _context.SaveChanges();

            return Task.FromResult("Ok");
        }

        public Task Delete(TEntity entity)
        {
            var first = _entities.FirstOrDefault(x => x.Id.Equals(entity.Id));
            if (first == null)
            {
                throw new ArgumentException($"Entity {entity.Id} is not found");
            }

            _entities.Remove(first);
            _context.SaveChanges();

            return Task.FromResult("Ok");
        }

        public Task Delete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                var first = _entities.FirstOrDefault(x => x.Id.Equals(entity.Id));

                if (first == null)
                {
                    throw new ArgumentException($"Entity {entity.Id} is not found");
                }

                _entities.Remove(first);
            }

            _context.SaveChanges();

            return Task.FromResult("Ok");
        }

        public Task<bool> AnyAsync(Func<TEntity, bool> search)
        {
            return Task.FromResult(_entities.Any(search));
        }

        public Task<TEntity> FirstOrDefaultAsync(Func<TEntity, bool> search)
        {
            var entity = _entities.FirstOrDefault(search);
            if (entity == null)
            {
                throw new ArgumentException($"Entity is not found");
            }
            return Task.FromResult(entity);
        }

        public Task<IEnumerable<TEntity>> WhereAsync(Func<TEntity, bool> search)
        {
            return Task.FromResult(_entities.Where(search));
        }

        public Task<TValue> Max<TValue>(Func<TEntity, TValue> search)
        {
            return Task.FromResult(_entities.Max(search));
        }

        public Task<CollectionOfEntities<TEntity>> Get(int start, int count, EntitySortingEnum sort)
        {
            var result = new CollectionOfEntities<TEntity>();

            result.Total = _entities.Count();
            result.Start = start;
            result.EntitySorting = sort;

            var x = count == 0 ? result.Total - start : count;

            result.Entities = GetSorting(_entities, sort).Skip(start).Take(x);
            result.Count = result.Entities.Count();

            return Task.FromResult(result);
        }

        public Task<CollectionOfEntities<TEntity>> Get(int start, int count, EntitySortingEnum sort, params Func<TEntity, bool>[] filters)
        {
            var result = new CollectionOfEntities<TEntity>();
            IEnumerable<TEntity> collection = null;

            if (filters != null && filters.Length > 0)
            {
                collection = _entities.Where(filters[0]);

                for (int i = 1; i < filters.Length; i++)
                {
                    collection = collection.Where(filters[i]);
                }
            }
            else
            {
                collection = _entities.ToList();
            }

            result.Total = collection.Count();
            result.Start = start;
            result.EntitySorting = sort;

            var x = count == 0 ? result.Total - start : count;

            result.Entities = GetSorting(collection, sort).Skip(start).Take(x);
            result.Count = result.Entities.Count();

            return Task.FromResult(result);
        }

        protected IOrderedEnumerable<TEntity> GetSorting(IEnumerable<TEntity> entities, EntitySortingEnum sort)
        {
            switch (sort)
            {
                case EntitySortingEnum.ByCreate:
                    return entities.OrderByDescending(x => x.Created);

                case EntitySortingEnum.ByUpdate:
                    return entities.OrderByDescending(x => x.Updated);

                default:
                    throw new ArgumentException($"Sorting type {sort} is not valid.");
            }
        }

        protected IOrderedQueryable<TEntity> GetSorting(DbSet<TEntity> entities, EntitySortingEnum sort)
        {
            switch (sort)
            {
                case EntitySortingEnum.ByCreate:
                    return entities.OrderByDescending(x => x.Created);

                case EntitySortingEnum.ByUpdate:
                    return entities.OrderByDescending(x => x.Updated);

                default:
                    throw new ArgumentException($"Sorting type {sort} is not valid.");
            }
        }
    }
}
