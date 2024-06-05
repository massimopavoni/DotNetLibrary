using DotNetLibrary.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DotNetLibrary.Models.Repositories;

public abstract class GenericRepository<T, K>(LibraryContext context)
    where T : class
{
    protected readonly LibraryContext Context = context;

    public virtual void Create(T entity) =>
        Context.Add(entity);

    public virtual T? Read(K id) =>
        Context.Set<T>().Find(id);

    public virtual void Update(T entity) =>
        Context.Entry(entity).State = EntityState.Modified;

    public virtual void Delete(K id)
    {
        var entity = Read(id);
        if (entity != null)
            Context.Entry(entity).State = EntityState.Deleted;
    }

    public virtual bool Exists(K id) =>
        Read(id) != null;

    public IDbContextTransaction BeginTransaction() =>
        Context.Database.BeginTransaction();

    public void SaveChanges() =>
        Context.SaveChanges();
}