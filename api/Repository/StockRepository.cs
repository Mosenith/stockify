using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<List<Stock>> GetAllStocksAsync()
        {
            return await _context.Stocks.ToListAsync();
        }
        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
            {
                return null;
            }
            return stockModel;
        }
        public async Task<Stock> CreateStockAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> UpdateStockAsync(int id, Stock updatedStock)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null)
            {
                return null;
            }
            stockModel.Symbol = updatedStock.Symbol;
            stockModel.CompanyName = updatedStock.CompanyName;
            stockModel.Purchase = updatedStock.Purchase;
            stockModel.LastDiv = updatedStock.LastDiv;
            stockModel.Industry = updatedStock.Industry;
            stockModel.MarketCap = updatedStock.MarketCap;
            await _context.SaveChangesAsync();
            return stockModel;
        }
        public async Task<Stock?> DeleteStockByIdAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null)
            {
                return null;
            }

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }
    }
}