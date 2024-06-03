using Microsoft.EntityFrameworkCore;

namespace Models.Repositories;

public abstract class GenericRepository<T, K>(LibraryContext context)
    where T : class
{
    protected LibraryContext _context = context;

    public virtual void Create(T entity) => _context.Add(entity);

    public virtual T? Read(K id) => _context.Set<T>().Find(id);

    public virtual void Update(T entity) => _context.Entry(entity).State = EntityState.Modified;

    public virtual void Delete(K id)
    {
        var entity = Read(id);
        if (entity != null)
            _context.Entry(entity).State = EntityState.Deleted;
    }

    public void Save() => _context.SaveChanges();
}