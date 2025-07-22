using PriceTracker.Interfaces;
using PriceTracker.Models;
using SQLite;

namespace PriceTracker.Data.Repositories;

public class StoreRepository : IStoreRepository
{
    private readonly SQLiteAsyncConnection _db;

    public StoreRepository(AppDbContext context)
    {
        _db = context.Database;
    }
    public async Task<int> AddStoreAsync(Store store)
    {
        await _db.InsertAsync(store);
        return store.Id;
    }
    public async Task<int> GetStoreByName(string name)
    {
        var store = await _db.Table<Store>().Where(s => s.Name == name).FirstOrDefaultAsync();
        return store?.Id ?? 0;
    }
    public Task<List<Store>> GetAllStores()
    {
        return _db.Table<Store>().ToListAsync();
    }
    public Task<Store> GetStoreById(int id)
    {
        return _db.Table<Store>().Where(s => s.Id == id).FirstOrDefaultAsync();
    }
    public async Task UpdateStoreAsync(Store store)
    {
        await _db.UpdateAsync(store);
    }
}