using api.Models;

namespace api.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser user);
        Task<Portfolio> CreateAsync(Portfolio portfolioModel);
        Task<Portfolio> DeletePortfolio(AppUser user, string symbol);
    }
}