namespace PriceTracker.DTO;

public class ListItemDTO
{
    public string Id { get; set; } = string.Empty;      // Pode ser Barcode, BrandId ou StoreId
    public string Name { get; set; } = string.Empty;    // Nome do produto, marca ou mercado
    public string? Brand { get; set; }                  // Para produto: nome da marca
    public string? Store { get; set; }                  // Para produto: nome do mercado
    public decimal? Price { get; set; }                 // Para produto: preço
    public DateTime? Date { get; set; }                 // Para produto: data
    public ListItemType Type { get; set; }              // Produto, Marca ou Mercado
}

public enum ListItemType
{
    Product,
    Brand,
    Store
}
