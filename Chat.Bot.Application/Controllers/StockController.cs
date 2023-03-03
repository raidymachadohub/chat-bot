using AutoMapper;
using Chat.Bot.Domain.DTO;
using Chat.Bot.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Bot.Application.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;

        public StockController(IStockService stockService,
            IMapper mapper)
        {
            _stockService = stockService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string code)
        {
            try
            {
                var stock = await _stockService.GetValueStockAsync(code);
                var stockDto = _mapper.Map<StockDTO>(stock);
                return Ok(stockDto);
            }
            catch (Exception e)
            {
                return BadRequest($"Exception: {e.Message}");
            }
        }
    }
}