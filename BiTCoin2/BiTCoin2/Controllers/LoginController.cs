using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BitCoin.Controllers;
using BiTCoin2.Data;
using Microsoft.AspNetCore.Identity.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BiTCoin2.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;
        private readonly DBContext _dbContext;
        private readonly BTCController _btcController;
    
        public LoginController(IConfiguration configuration, ILogger<LoginController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        SqlConnection _connection;

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        
        public bool CheckPassword(string email, string password)
        {
            int count = 0;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT COUNT(*) FROM UserTable WHERE Email=@Email AND Password=@Password", connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    count = (int)command.ExecuteScalar();
                }
            }

            if (count > 0)
            {
                
                _logger.LogInformation("User exists");
                return true;
            }
            else
            {
                TempData["AlertMessage"] = "users does not exist";
                _logger.LogInformation("User does not exist");
                return false;
            }
        }
        /*   [HttpPost]
           public IActionResult Login(string email,string password)
           {

               bool emailExists = CheckPassword(email,password);
               if (emailExists)
               {
                   var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyForAuthenticationOfApplication"));
                   var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                   var Sectoken = new JwtSecurityToken("https://localhost:7035",
                     "https://localhost:7035",
                     null,
                     expires: DateTime.Now.AddMinutes(120),
                     signingCredentials: credentials);

                   var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                   _logger.LogInformation(token);
                    return RedirectToAction("Index", "Home");


               }
               else
               {
                   TempData["AlertMessage"] = "Login failed. Please check your email and password.";
                   _logger.LogError("Incorrect email or password");
                   return RedirectToAction("Login", "Login");
               }
           }*/

        [HttpPost]
        public void Login(string email, string password)
        {

            bool emailExists = CheckPassword(email, password);
            if (emailExists)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyForAuthenticationOfApplication"));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var Sectoken = new JwtSecurityToken("https://localhost:7035",
                  "https://localhost:7035",
                  null,
                  expires: DateTime.Now.AddMinutes(120),
                  signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                _logger.LogInformation(token);
               // return RedirectToAction("Index", "Home");


            }
            else
            {
                TempData["AlertMessage"] = "Login failed. Please check your email and password.";
                _logger.LogError("Incorrect email or password");
               // return RedirectToAction("Login", "Login");
            }
        }
        [HttpPost("login")]
        public IActionResult Post([FromBody] LoginRequest loginRequest)
        {
            //your logic for login process
            //If login usrename and password are correct then proceed to generate token

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return Ok(token);
        }

    }
}
