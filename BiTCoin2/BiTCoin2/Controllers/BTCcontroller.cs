using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using BiTCoin2.NewFolder;
using BiTCoin2.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace BitCoin.Controllers
{
    [Authorize]
    [ApiController]
    public class BTCController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly DBContext _dbContext;
        private readonly ILogger<BTCController> _logger;

        public BTCController(IConfiguration configuration, IMapper mapper, DBContext dbContext, ILogger<BTCController> logger)
        {
            _configuration = configuration;
            _mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
        }
        [Authorize]
        [HttpGet("BTC")]
        public async Task<IActionResult> GetBtc()
        {
            using var httpClient = new HttpClient();
            var btcApiURL1 = _configuration["BtcApiURL1"];
            BTC1 btc1 = new BTC1();
            try
            {
                // Fetch data from the first API
                HttpResponseMessage response1 = await httpClient.GetAsync(btcApiURL1);
                if (response1.IsSuccessStatusCode)
                {
                    string jsonResponse1 = await response1.Content.ReadAsStringAsync();
                    btc1 = JsonConvert.DeserializeObject<BiTCoin2.NewFolder.BTC1>(jsonResponse1);
                }
                else
                {
                    _logger.LogError($"Failed to fetch data from {btcApiURL1}. Status code: {response1.StatusCode}");
                    return StatusCode((int)response1.StatusCode, $"Failed to fetch data from {btcApiURL1}");
                }
                // Combine data from both sources
                BCT12 bct12 = _mapper.Map<BCT12>(btc1);
                // Save to database
                _dbContext.BCT12.Add(bct12);
                await _dbContext.SaveChangesAsync();

                return Ok(bct12);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while retrieving the data");
            }
        }
        [Authorize]
        [HttpGet("BTC2")]
        public async Task<IActionResult> GetBtc2()
        {
            using var httpClient = new HttpClient();
            var btcApiURL2 = _configuration["BtcApiURL2"];
            BCT2 btc2 = new BCT2();
            try
            {
                // Fetch data from the second API
                HttpResponseMessage response2 = await httpClient.GetAsync(btcApiURL2);
                if (response2.IsSuccessStatusCode)
                {
                    string jsonResponse2 = await response2.Content.ReadAsStringAsync();
                    btc2 = JsonConvert.DeserializeObject<BCT2>(jsonResponse2);
                }
                else
                {
                    _logger.LogError($"Failed to fetch data from {btcApiURL2}. Status code: {response2.StatusCode}");
                    return StatusCode((int)response2.StatusCode, $"Failed to fetch data from {btcApiURL2}");
                }

                // Combine data from both sources
                BCT12 bct13 = _mapper.Map<BCT12>(btc2);
                // Save to database
                _dbContext.BCT12.Add(bct13);
                await _dbContext.SaveChangesAsync();

                return Ok(bct13);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}", ex);
                return StatusCode(500, "An error occurred while retrieving the data");
            }
        }

    }
}
