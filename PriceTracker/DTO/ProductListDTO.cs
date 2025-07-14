namespace PriceTracker.DTO;

public class ProductListDTO
{
    public string Barcode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public string Store { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
}