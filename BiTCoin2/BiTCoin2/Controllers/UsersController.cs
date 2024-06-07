using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BiTCoin2.Data;
using BiTCoin2.NewFolder;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Data.SqlClient;
using BitCoin.Controllers;

namespace BiTCoin2.Controllers
{
    public class UsersController : Controller
    {
        private readonly DBContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;
       
        private readonly BTCController _btcController;
        public UsersController(DBContext context,IConfiguration configuration, ILogger<LoginController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
        }

        SqlConnection _connection;
        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserTable.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.UserTable
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.UserTable.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Username,Email,Password")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.UserTable
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.UserTable.FindAsync(id);
            if (user != null)
            {
                _context.UserTable.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.UserTable.Any(e => e.UserID == id);
        }
        [HttpPost]
        public async Task<IActionResult> Signup(string email, string password, string username)
        {
            // Check if user with the provided email already exists
            bool existingUser = CheckIfUserExists(email);

            if (existingUser)
            {
                // If user exists, return an alert message
                TempData["AlertMessage"] = "User already exists.";
                return RedirectToAction("Create", "Users"); // Redirect to the login page or wherever you want
            }
            else
            {
                // If user doesn't exist, create a new user account
                var newUser = new User
                {
                    Username = username,
                    Email = email,
                    Password = password
                };

                // Add the new user to the database
                _context.UserTable.Add(newUser);
                await _context.SaveChangesAsync();

                // Redirect to the home page or wherever you want
                return RedirectToAction("Index", "Home");
            }
        }




        private bool CheckIfUserExists(string email)
        {
            // Check if a user with the provided email already exists
            bool userExists = _context.UserTable.Any(u => u.Email == email);

            if (userExists)
            {
                TempData["AlertMessage_Signup"] = "User with email " + email + " already exists";
                _logger.LogInformation("User with email " + email + " already exists");
            }
            else
            {
                _logger.LogInformation("User with email " + email + " does not exist");
            }

            return userExists;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Username,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // If model state is not valid, return to the Create view with the user model
            return View(user);
        }


    }
}
