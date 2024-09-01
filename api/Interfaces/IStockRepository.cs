using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocksAsync(QueryObject query);
        Task<Stock?> GetStockByIdAsync(int id); // add ? cos we're using FirstOrDefault
        Task<Stock?> GetStockBySymbolAsync(string symbol);
        Task<Stock> CreateStockAsync(Stock stockModel);
        Task<Stock?> UpdateStockAsync(int id, Stock stockModel);
        Task<Stock?> DeleteStockByIdAsync(int id);
        Task<bool> StockExists(int id);
    }
}