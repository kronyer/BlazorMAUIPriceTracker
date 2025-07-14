using PriceTracker.DTO;
using PriceTracker.Models;

namespace PriceTracker.Interfaces;

public interface IProductRepository
{
    Task<ProductClass?> GetByBarcodeAsync(string barcode);
    Task<IEnumerable<ProductClass>> GetAllAsync();
    Task<IEnumerable<ProductListDTO>> GetPagedAsync(int pageNumber, int pageSize, string searchTerm = "", DateTime? fromDate = null, DateTime? toDate = null);
    Task<IEnumerable<ProductListDTO>> GetPagedAsyncPorProduto(int pageNumber, int pageSize, string prodBarcode, DateTime? fromDate = null, DateTime? toDate = null);
    Task<string> AddAsync(ProductClass product);
    Task<int> AddBrandAsync(Brand product);
    Task<Brand> GetBrandById(int id);
    Task<List<Brand>> GetAllBrands();
    Task<int> GetBrandByName(string name);
    Task<int> AddStoreAsync(Store product);
    Task<int> GetStoreByName(string name);
    Task<List<Store>> GetAllStores();
    Task<int> DeleteAsync(string barcode);
}