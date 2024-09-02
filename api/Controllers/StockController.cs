using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;

        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _context = context;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var stocks = await _stockRepo.GetAllStocksAsync(query);
            var stockDto = stocks.Select(s => s.ToStockDto());
            return Ok(stockDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var stock = await _stockRepo.GetStockByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StockRequest stockRequest)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var stockModel = stockRequest.ToStock();
            await _stockRepo.CreateStockAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new Stock { Id = stockModel.Id, }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] StockRequest updatedStock)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var stockModel = updatedStock.ToStock();
            await _stockRepo.UpdateStockAsync(id, stockModel);
            if (stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var stockModel = await _stockRepo.DeleteStockByIdAsync(id);
            if (stockModel == null)
            {
                return NotFound();
            }
            return Ok(stockModel);
        }
    }

}