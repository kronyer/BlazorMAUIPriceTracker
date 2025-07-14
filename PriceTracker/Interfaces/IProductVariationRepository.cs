using PriceTracker.Models;

namespace PriceTracker.Interfaces;

public interface IProductVariationRepository
{
    Task<ProductVariation> GetByIdAsync(int id);
    Task<IEnumerable<ProductVariation>> GetByProductBarcodeAsync(string barcode);
    Task<int> AddAsync(ProductVariation variation);
    Task<int> DeleteAsync(int id);

}