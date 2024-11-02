using System.Linq.Expressions;

public interface IRepository<TEntity>
    where TEntity : class
{
    void Add(TEntity entity);
    bool Exists(Expression<Func<TEntity, bool>> predicate);

    List<TEntity> GetAllWithPagination(int pageNumber, int pageSize, Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[] includes);

    List<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[] includes);
    TEntity Get(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
    void Remove(TEntity entity);
    void Update(TEntity entity);
}