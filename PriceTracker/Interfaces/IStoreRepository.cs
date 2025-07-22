using PriceTracker.Models;

namespace PriceTracker.Interfaces;

public interface IStoreRepository
{
    Task<int> AddStoreAsync(Store store);
    Task<Store> GetStoreById(int id);
    Task<List<Store>> GetAllStores();
    Task<int> GetStoreByName(string name);
    Task UpdateStoreAsync(Store store);
}