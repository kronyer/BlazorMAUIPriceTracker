using PriceTracker.Models;

namespace PriceTracker.Interfaces;

public interface IBrandRepository
{
    Task<int> AddBrandAsync(Brand brand);
    Task<Brand> GetBrandById(int id);
    Task<List<Brand>> GetAllBrands();
    Task<int> GetBrandByName(string name);
    Task UpdateBrandAsync(Brand brand);
}
