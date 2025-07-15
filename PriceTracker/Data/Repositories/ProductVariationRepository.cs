using PriceTracker.Interfaces;
using PriceTracker.Models;
using SQLite;

namespace PriceTracker.Data.Repositories;

public class ProductVariationRepository : IProductVariationRepository
{
    private readonly SQLiteAsyncConnection _db;

    public ProductVariationRepository(AppDbContext context)
    {
        _db = context.Database;
    }
    public async Task<ProductVariation> GetByIdAsync(int id)
    {
        return await _db.Table<ProductVariation>().Where(v => v.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ProductVariation>> GetByProductBarcodeAsync(string barcode)
    {
        return await _db.Table<ProductVariation>().Where(v => v.ProductBarcode == barcode).ToListAsync().ContinueWith(t => (IEnumerable<ProductVariation>)t.Result);
    }

    public async Task<int> AddAsync(ProductVariation variation)
    {
        return await _db.InsertAsync(variation);
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await _db.Table<ProductVariation>().DeleteAsync(v => v.Id == id);
    }

    public Task<IEnumerable<ProductVariation>> GetAll()
    {
        return _db.Table<ProductVariation>().ToListAsync().ContinueWith(t => (IEnumerable<ProductVariation>)t.Result);
    }
}