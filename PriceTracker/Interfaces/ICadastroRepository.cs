using PriceTracker.DTO;

namespace PriceTracker.Interfaces;

public interface ICadastroRepository
{
    Task<List<ListItemDTO>> GetAllCadastrosAsync(string searchTerm = "", string? filter = null);
    Task<bool> DeleteCadastroAsync(string id, ListItemType type);
}