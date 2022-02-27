namespace cryptoapi.Data.Interfaces;

public interface IRepository<out T>
{
    IQueryable<T> Table { get; }

    T GetByName(string name);
}