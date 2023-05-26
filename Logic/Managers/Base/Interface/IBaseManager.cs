﻿using Dal.Base.Entity;

namespace Logic.Managers.Base.Interface;

public interface IBaseManager<T, TI> where T : BaseDal<TI>
{
    public Task<TI> InsertAsync(T dal);
    public Task DeleteAsync(TI id);
    public Task<T?> GetAsync(TI id);
    public Task<List<T?>> GetAllAsync();
    public Task<TI> UpdateAsync(T dal);
}