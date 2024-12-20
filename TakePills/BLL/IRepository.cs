namespace TakePills.BLL;

public interface IRepository<T> where T : class
{
    Task Add(T entity);
    Task<T> Get(int id);
    Task Update(T entity);
    Task Delete(int id);
}
