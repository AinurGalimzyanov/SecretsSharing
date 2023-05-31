using Dal.Base.Entity;

namespace Logic.Managers.Base.Interface;

/// <summary>
/// base manager for CRUD operations
/// </summary>
/// <typeparam name="T">The type of entity for which we use the repository (Dal)</typeparam>
/// <typeparam name="TI">Type of unique identifier (Id)</typeparam>
public interface IBaseManager<T, TI> where T : BaseDal<TI>
{
    /// <summary>
    /// Inserts an entry into the database
    /// </summary>
    /// <param name="dal">the entity representing the new record</param>
    /// <returns>unique identifier of the insert record</returns>
    public Task<TI> InsertAsync(T dal);
    
    /// <summary>
    /// deletes an entity by a unique identifier in the database 
    /// </summary>
    /// <param name="id">unique entity id</param>
    /// <returns></returns>
    public Task DeleteAsync(TI id);
    
    /// <summary>
    /// get data from the table by Id
    /// </summary>
    /// <param name="id">unique entity id</param>
    /// <returns>an entity representing a record from a table</returns>
    public Task<T?> GetAsync(TI id);
    
    /// <summary>
    /// updates a record in a table in the database
    /// </summary>
    /// <param name="dal">the entity representing the updated record</param>
    /// <returns>unique identifier of the updated record</returns>
    public Task<TI> UpdateAsync(T dal);
}