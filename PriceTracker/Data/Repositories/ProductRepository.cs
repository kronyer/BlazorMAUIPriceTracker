using PriceTracker.DTO;
using PriceTracker.Interfaces;
using PriceTracker.Models;
using SQLite;

namespace PriceTracker.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly SQLiteAsyncConnection _db;

    public ProductRepository(AppDbContext context)
    {
        _db = context.Database;
    }

    public async Task<ProductClass?> GetByBarcodeAsync(string barcode)
    {
        return await _db.Table<ProductClass>().Where(p => p.Barcode == barcode).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ProductClass>> GetAllAsync()
    {
        return await _db.Table<ProductClass>().ToListAsync().ContinueWith(t => (IEnumerable<ProductClass>)t.Result);
    }

    public async Task<Paged<ProductListDTO>> GetPagedAsync(
        int pageNumber, int pageSize, string searchTerm = "", DateTime? fromDate = null, DateTime? toDate = null)
    {
        var products = await _db.Table<ProductClass>().ToListAsync();
        var brands = await _db.Table<Brand>().ToListAsync();
        var variations = await _db.Table<ProductVariation>().ToListAsync();
        var stores = await _db.Table<Store>().ToListAsync();

        // Filtro por data
        if (fromDate.HasValue)
            variations = variations.Where(v => v.Date >= fromDate.Value).ToList();
        if (toDate.HasValue)
            variations = variations.Where(v => v.Date <= toDate.Value).ToList();

        // Filtro por termo de busca (nome do produto)
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var filteredBarcodes = products
                .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .Select(p => p.Barcode)
                .ToHashSet();

            variations = variations
                .Where(v => filteredBarcodes.Contains(v.ProductBarcode))
                .ToList();
        }

        // Ordena por data decrescente (mais recente primeiro)
        variations = variations.OrderByDescending(v => v.Date).ToList();
        var totalCount = variations.Count;

        var productDTOs = variations
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(v =>
            {
                var product = products.FirstOrDefault(p => p.Barcode == v.ProductBarcode);
                var brand = product != null ? brands.FirstOrDefault(b => b.Id == product.BrandId)?.Name ?? "" : "";
                var storeName = stores.FirstOrDefault(s => s.Id == v.StoreId)?.Name ?? "";

                return new ProductListDTO
                {
                    Barcode = v.ProductBarcode,
                    Name = product?.Name ?? "",
                    Brand = brand,
                    Price = v.Price,
                    Store = storeName,
                    Date = v.Date
                };
            })
            .ToList();

        return new Paged<ProductListDTO>
        {
            Items = productDTOs,
            TotalCount = totalCount
        };
    }


    public async Task<string> AddAsync(ProductClass product)
    {
        await _db.InsertAsync(product);
        return product.Barcode; // Assuming ProductClass has an Id property
    }
    public async Task<int> DeleteAsync(string barcode)
    {
        return await _db.Table<ProductClass>().DeleteAsync(p => p.Barcode == barcode);
    }
   
    public async Task<IEnumerable<ProductListDTO>> GetPagedAsyncPorProduto(int pageNumber, int pageSize, string prodBarcode, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var products = await _db.Table<ProductClass>().Where(p => p.Barcode == prodBarcode).ToListAsync();
        var brands = await _db.Table<Brand>().ToListAsync();
        var variations = await _db.Table<ProductVariation>().Where(v => v.ProductBarcode == prodBarcode).ToListAsync();
        var stores = await _db.Table<Store>().ToListAsync();

        // Filtro por data
        if (fromDate.HasValue)
            variations = variations.Where(v => v.Date >= fromDate.Value).ToList();
        if (toDate.HasValue)
            variations = variations.Where(v => v.Date <= toDate.Value).ToList();

        // Filtro por termo de busca (nome do produto)

        // Ordena por data decrescente (mais recente primeiro)
        variations = variations.OrderByDescending(v => v.Date).ToList();

        var productDTOs = variations
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(v =>
            {
                var product = products.FirstOrDefault(p => p.Barcode == v.ProductBarcode);
                var brand = product != null ? brands.FirstOrDefault(b => b.Id == product.BrandId)?.Name ?? "" : "";
                var storeName = stores.FirstOrDefault(s => s.Id == v.StoreId)?.Name ?? "";

                return new ProductListDTO
                {
                    Barcode = v.ProductBarcode,
                    Name = product?.Name ?? "",
                    Brand = brand,
                    Price = v.Price,
                    Store = storeName,
                    Date = v.Date
                };
            })
            .ToList();

        return productDTOs;
    }
    public async Task UpdateProductAsync(ProductClass product)
    {
        await _db.UpdateAsync(product);
    }
    public async Task<IEnumerable<ProductClass>> GetPagedProductsAsync(int pageNumber, int pageSize, string searchTerm = "")
    {
        var query = _db.Table<ProductClass>();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            // SQLite-net não suporta StringComparison, use Contains simples
            query = query.Where(p => p.Name.Contains(searchTerm));
        }

        // Primeiro busca do banco, depois pagina em memória
        var all = await query.ToListAsync();
        return all
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
}