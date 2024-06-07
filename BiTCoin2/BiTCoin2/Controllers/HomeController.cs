using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AutoMapper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BiTCoin2.Data;
using BiTCoin2.Models;
using BiTCoin2.NewFolder;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace BitCoin.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBContext _dbContext;
        private readonly BTCController _btcController;
        private IConfiguration _config;
        public HomeController(ILogger<HomeController> logger, DBContext dbContext, BTCController btcController,IConfiguration configuration1)
        {
            _logger = logger;
            _dbContext = dbContext;
            _btcController = btcController;
            _config = configuration1;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                //display db info 
                var model = await _dbContext.BCT12.ToListAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving the data");
            }
        }

        
        [Authorize]
        [HttpGet("call-BTC")]
        public async Task<IActionResult> CallBtc()
        {
            try
            {
                // Call the methods to get results asynchronously
                var result = await _btcController.GetBtc();
                var result2 = await _btcController.GetBtc2();
                // Return the combined result
                return Ok((result, result2));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "An error occurred while retrieving the data");
            }
        }
     
    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        //{
        //    app.UseAuthentication();
        //    app.UseMvc();
        //}


    }
}
