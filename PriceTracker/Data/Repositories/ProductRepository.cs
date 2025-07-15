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

    public async Task<IEnumerable<ProductListDTO>> GetPagedAsync(
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


    public async Task<string> AddAsync(ProductClass product)
    {
        await _db.InsertAsync(product);
        return product.Barcode; // Assuming ProductClass has an Id property
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

    public async Task<int> DeleteAsync(string barcode)
    {
        return await _db.Table<ProductClass>().DeleteAsync(p => p.Barcode == barcode);
    }

    public Task<Brand> GetBrandById(int id)
    {
        return _db.Table<Brand>().Where(b => b.Id == id).FirstOrDefaultAsync();
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

    public Task<List<Brand>> GetAllBrands()
    {
        return _db.Table<Brand>().ToListAsync();
    }

    public Task<List<Store>> GetAllStores()
    {
        return _db.Table<Store>().ToListAsync();
    }


    public async Task<List<ListItemDTO>> GetAllCadastrosAsync(string searchTerm = "", string? filter = null)
    {
        var items = new List<ListItemDTO>();
        var term = searchTerm?.Trim() ?? "";

        // Produtos
        if (string.IsNullOrEmpty(filter) || filter == "Product")
        {
            // Filtra produtos no banco
            var query = _db.Table<ProductClass>();
            if (!string.IsNullOrWhiteSpace(term))
                query = query.Where(p => p.Name.Contains(term));

            var products = await query.ToListAsync();

            items.AddRange(products.Select(p => new ListItemDTO
            {
                Id = p.Barcode,
                Name = p.Name,
                Type = ListItemType.Product,
            }));
        }

        // Marcas
        if (string.IsNullOrEmpty(filter) || filter == "Brand")
        {
            var brandQuery = _db.Table<Brand>();
            if (!string.IsNullOrWhiteSpace(term))
                brandQuery = brandQuery.Where(b => b.Name.Contains(term));
            var brands = await brandQuery.ToListAsync();

            items.AddRange(brands.Select(b => new ListItemDTO
            {
                Id = b.Id.ToString(),
                Name = b.Name,
                Type = ListItemType.Brand
            }));
        }

        // Mercados
        if (string.IsNullOrEmpty(filter) || filter == "Store")
        {
            var storeQuery = _db.Table<Store>();
            if (!string.IsNullOrWhiteSpace(term))
                storeQuery = storeQuery.Where(s => s.Name.Contains(term));
            var stores = await storeQuery.ToListAsync();

            items.AddRange(stores.Select(s => new ListItemDTO
            {
                Id = s.Id.ToString(),
                Name = s.Name,
                Type = ListItemType.Store
            }));
        }

        // Ordene por data (produtos) ou nome (outros)
        return items
            .OrderByDescending(i => i.Date ?? DateTime.MinValue)
            .ThenBy(i => i.Name)
            .ToList();
    }

    public async Task<bool> DeleteCadastroAsync(string id, ListItemType type)
    {
        switch (type)
        {
            case ListItemType.Product:
                // Verifica se existe variação para o produto
                var hasVariations = await _db.Table<ProductVariation>().Where(v => v.ProductBarcode == id).FirstOrDefaultAsync() != null;
                if (hasVariations)
                    return false;
                await _db.Table<ProductClass>().DeleteAsync(p => p.Barcode == id);
                return true;

            case ListItemType.Brand:
                // Verifica se existe produto com essa marca
                if (int.TryParse(id, out int brandId))
                {
                    var hasProducts = await _db.Table<ProductClass>().Where(p => p.BrandId == brandId).FirstOrDefaultAsync() != null;
                    if (hasProducts)
                        return false;
                    await _db.Table<Brand>().DeleteAsync(b => b.Id == brandId);
                    return true;
                }
                return false;

            case ListItemType.Store:
                // Verifica se existe variação com essa loja
                if (int.TryParse(id, out int storeId))
                {
                    var hasVariationsStore = await _db.Table<ProductVariation>().Where(v => v.StoreId == storeId).FirstOrDefaultAsync() != null;
                    if (hasVariationsStore)
                        return false;
                    await _db.Table<Store>().DeleteAsync(s => s.Id == storeId);
                    return true;
                }
                return false;

            default:
                return false;
        }
    }

    public Task<Store> GetStoreById(int id)
    {
        return _db.Table<Store>().Where(s => s.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateProductAsync(ProductClass product)
    {
        await _db.UpdateAsync(product);
    }

    public async Task UpdateBrandAsync(Brand brand)
    {
        await _db.UpdateAsync(brand);
    }

    public async Task UpdateStoreAsync(Store store)
    {
        await _db.UpdateAsync(store);
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