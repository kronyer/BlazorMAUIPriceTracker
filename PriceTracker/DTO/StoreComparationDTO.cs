using PriceTracker.Models;

namespace PriceTracker.DTO;

public class StoreComparativoDTO
{
    public string StoreName { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public List<ProdutoComparativoDTO> Produtos { get; set; } = new();
}

public class ProdutoComparativoDTO
{
    public string ProductName { get; set; } = string.Empty;
    public string BrandName { get; set; }
    public decimal Price { get; set; }
    public bool IsMedia { get; set; }
}


