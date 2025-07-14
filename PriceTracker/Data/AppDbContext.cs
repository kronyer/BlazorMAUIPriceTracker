using PriceTracker.Models;
using SQLite;

namespace PriceTracker.Data;

public class AppDbContext
{
    private readonly SQLiteAsyncConnection _database;

    public AppDbContext(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        // Enable foreign key constraints
        _database.ExecuteAsync("PRAGMA foreign_keys = ON;").Wait();
        _database.CreateTableAsync<Brand>().Wait();
        _database.CreateTableAsync<ProductClass>().Wait();
        _database.CreateTableAsync<Store>().Wait();
        _database.CreateTableAsync<ProductVariation>().Wait();
    }

    public SQLiteAsyncConnection Database => _database;
}