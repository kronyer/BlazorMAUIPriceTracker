using PriceTracker.DTO;
using PriceTracker.Models;
using static PriceTracker.Components.Pages.Cadastros;

namespace PriceTracker.Interfaces;

public interface IProductRepository
{
    Task<ProductClass?> GetByBarcodeAsync(string barcode);
    Task<IEnumerable<ProductClass>> GetAllAsync();
    Task<IEnumerable<ProductClass>> GetPagedProductsAsync(int pageNumber, int pageSize, string searchTerm = "");
    Task<IEnumerable<ProductListDTO>> GetPagedAsync(int pageNumber, int pageSize, string searchTerm = "", DateTime? fromDate = null, DateTime? toDate = null);
    Task<IEnumerable<ProductListDTO>> GetPagedAsyncPorProduto(int pageNumber, int pageSize, string prodBarcode, DateTime? fromDate = null, DateTime? toDate = null);
    Task<string> AddAsync(ProductClass product);
    Task<int> AddBrandAsync(Brand product);
    Task<Brand> GetBrandById(int id);
    Task<List<Brand>> GetAllBrands();
    Task<int> GetBrandByName(string name);
    Task<int> AddStoreAsync(Store product);
    Task<int> GetStoreByName(string name);
    Task<Store> GetStoreById(int id);
    Task<List<Store>> GetAllStores();
    Task<int> DeleteAsync(string barcode);
    Task<List<ListItemDTO>> GetAllCadastrosAsync(string searchTerm = "", string? filter = null);
    Task<bool> DeleteCadastroAsync(string id, ListItemType type);
    Task UpdateProductAsync(ProductClass product);
    Task UpdateBrandAsync(Brand brand);
    Task UpdateStoreAsync(Store store);



}