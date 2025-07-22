using PriceTracker.DTO;
using PriceTracker.Models;
using static PriceTracker.Components.Pages.Cadastros;

namespace PriceTracker.Interfaces;

public interface IProductRepository
{
    Task<ProductClass?> GetByBarcodeAsync(string barcode);
    Task<IEnumerable<ProductClass>> GetAllAsync();
    Task<IEnumerable<ProductClass>> GetPagedProductsAsync(int pageNumber, int pageSize, string searchTerm = "");
    Task<Paged<ProductListDTO>> GetPagedAsync(int pageNumber, int pageSize, string searchTerm = "", DateTime? fromDate = null, DateTime? toDate = null);
    Task<IEnumerable<ProductListDTO>> GetPagedAsyncPorProduto(int pageNumber, int pageSize, string prodBarcode, DateTime? fromDate = null, DateTime? toDate = null);
    Task<string> AddAsync(ProductClass product);
    Task<int> DeleteAsync(string barcode);
    Task UpdateProductAsync(ProductClass product);
}