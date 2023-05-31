using Dal.Base.Entity;
using Dal.Base.Repository.Interface;
using Logic.Managers.Base.Interface;

namespace Logic.Managers.Base;

/// <summary>
/// base manager for CRUD operations
/// </summary>
/// <typeparam name="T">The type of entity for which we use the repository (Dal)</typeparam>
/// <typeparam name="TI">Type of unique identifier (Id)</typeparam>
public class BaseManager<T, TI> : IBaseManager<T, TI> where T : BaseDal<TI>
{
    protected readonly IBaseRepository<T, TI> Repository;

    public BaseManager(IBaseRepository<T, TI> repository)
    {
        Repository = repository;
    }

    /// <summary>
    /// Inserts an entry into the database
    /// </summary>
    /// <param name="dal">the entity representing the updated record</param>
    /// <returns>unique identifier of the insert record</returns>
    public async Task<TI> InsertAsync(T dal)
    {
        return await Repository.InsertAsync(dal);
    }

    /// <summary>
    /// deletes an entity by a unique identifier in the database 
    /// </summary>
    /// <param name="id">unique entity id</param>
    /// <returns></returns>
    public async Task DeleteAsync(TI id)
    { 
        await Repository.DeleteAsync(id);
    }
    
    /// <summary>
    /// get data from the table by Id
    /// </summary>
    /// <param name="id">unique entity id</param>
    /// <returns>an entity representing a record from a table</returns>
    public async Task<T?> GetAsync(TI id)
    {
        return await Repository.GetAsync(id);
    }

    /// <summary>
    /// updates a record in a table in the database
    /// </summary>
    /// <param name="dal">the entity representing the updated record</param>
    /// <returns>unique identifier of the updated record</returns>
    public async Task<TI> UpdateAsync(T dal)
    {
        return await Repository.UpdateAsync(dal);
    }
}