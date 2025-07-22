using PriceTracker.DTO;
using PriceTracker.Interfaces;
using PriceTracker.Models;
using SQLite;

namespace PriceTracker.Data.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly SQLiteAsyncConnection _db;

    public BrandRepository(AppDbContext context)
    {
        _db = context.Database;
    }

    public async Task<int> AddBrandAsync(Brand brand)
    {
        await _db.InsertAsync(brand);
        return brand.Id;

    }
    public async Task<int> GetBrandByName(string name)
    {
        var brand = await _db.Table<Brand>().Where(b => b.Name == name).FirstOrDefaultAsync();
        return brand?.Id ?? 0;
    }
    public Task<Brand> GetBrandById(int id)
    {
        return _db.Table<Brand>().Where(b => b.Id == id).FirstOrDefaultAsync();
    }
    public Task<List<Brand>> GetAllBrands()
    {
        return _db.Table<Brand>().ToListAsync();
    }
    public async Task UpdateBrandAsync(Brand brand)
    {
        await _db.UpdateAsync(brand);
    }
}