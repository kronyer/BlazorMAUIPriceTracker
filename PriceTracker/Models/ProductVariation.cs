using SQLite;

namespace PriceTracker.Models;

public class ProductVariation
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string ProductBarcode { get; set; } = string.Empty; // Foreign key

    public decimal Price { get; set; }

    public DateTime Date { get; set; }
    public int StoreId { get; set; } // Foreign key to Store
    [Ignore]
    public Store Store { get; set; } // Optional store name
}