using PriceTracker.DTO;
using PriceTracker.Interfaces;
using PriceTracker.Models;
using SQLite;

namespace PriceTracker.Data.Repositories;

public class CadastroRepository : ICadastroRepository
{
    private readonly SQLiteAsyncConnection _db;

    public CadastroRepository(AppDbContext context)
    {
        _db = context.Database;
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
}