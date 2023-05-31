namespace Dal.Base.Entity;

/// <summary>
/// base entity class
/// </summary>
/// <typeparam name="T">the unique identifier of the entity</typeparam>
public class BaseDal<T>
{ 
    public T Id { get; set; }
}