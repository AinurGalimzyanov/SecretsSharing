using Dal.Base.Entity;
using Dal.Base.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Dal.Base.Repository;


/// <summary>
/// base repository
/// performs CRUD operations for any Basedal entities
/// </summary>
/// <typeparam name="T">The type of entity for which we use the repository (Dal)</typeparam>
/// <typeparam name="TI">Type of unique identifier (Id)</typeparam>
public class BaseRepository<T, TI> : IBaseRepository<T, TI> where T : BaseDal<TI>
{
    /// <summary>
    /// an object representing a snapshot of the database
    /// </summary>
    private readonly DataContext _context;
    /// <summary>
    /// list of all T table entries
    /// </summary>
    private readonly DbSet<T> _dbSet;
    
    /// <summary>
    /// initializes the file repository
    /// </summary>
    /// <param name="context">database session</param>
    public BaseRepository(DataContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }
    
    /// <summary>
    /// Inserts an entry into the database
    /// </summary>
    /// <param name="dal">the entity representing the new record</param>
    /// <returns>unique identifier of the insert record</returns>
    public async Task<TI> InsertAsync(T dal)
    {
        var entity = await _dbSet.AddAsync(dal);
        await _context.SaveChangesAsync();
        return entity.Entity.Id;
    }

    /// <summary>
    /// deletes an entity by a unique identifier in the database 
    /// </summary>
    /// <param name="id">unique entity id</param>
    /// <returns></returns>
    public async Task DeleteAsync(TI id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// get data from the table by Id
    /// </summary>
    /// <param name="id">unique entity id</param>
    /// <returns>an entity representing a record from a table</returns>
    public async Task<T?> GetAsync(TI id)
    {
        var entity = await _dbSet.FindAsync(id);
        return entity;
    }

    /// <summary>
    /// updates a record in a table in the database
    /// </summary>
    /// <param name="dal">the entity representing the updated record</param>
    /// <returns>unique identifier of the updated record</returns>
    public async Task<TI> UpdateAsync(T dal)
    {
        _context.Entry(dal).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return dal.Id;
    }
}