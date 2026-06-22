using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;      // SymmetricSecurityKey, SigningCredentials ✅
using System.IdentityModel.Tokens.Jwt;     // JwtSecurityTokenHandler, SecurityTokenDescriptor ✅
using System.Security.Claims;              // Claim, ClaimTypes ✅
using System.Text;                         // Encoding ✅
using mvcApp.Data;
using mvcApp.Dto;
using mvcApp.Models;
namespace mvcApp.Controllers
{
    public class AuthController(AppDbContext _context) : Controller
    {
        public IActionResult Login()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public async Task<IActionResult> CreateUser(UserDto dto)
        {
            if(dto == null || String.IsNullOrEmpty(dto.Username) || String.IsNullOrEmpty(dto.Password) || String.IsNullOrEmpty(dto.Email))
            {
                ViewBag.ErrorMessage = "Kindly fill All the details.";
                return View("Register");
            }
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            
            if (existingUser == null)
            {
                var user = new User
                {
                    Email = dto.Email,
                    Password = dto.Password,
                    UserName = dto.Username
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "User registered successfully. Please log in.";
                return RedirectToAction("Login");

            }
            else
            {
                ViewBag.ErrorMessage = "User with this email address already exists.";
                return View("Register");
            } 
        }

        public async Task<IActionResult> LoginUser(UserDto dto)
        {
            if(dto == null || String.IsNullOrEmpty(dto.Username) ||String.IsNullOrEmpty(dto.Password))
            {
                ViewBag.ErrorMessage="Kindly fill in all the details";
                return View("Login");
            }
            var registeredUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName== dto.Username);
            
            if(registeredUser == null)
            {
                ViewBag.ErrorMessage = "Please Register First.";
                return View("Login");
            }
            else
            {
                if(registeredUser.Password != dto.Password)
                {
                    ViewBag.ErrorMessage = "Please enter correct Password.";
                    return View("Login");
                }
                else
                {
                    // ViewBag.SuccessMessage = "User loged in Successfuly.";
                    var token = GenerateJwtToken(dto);

                    Response.Cookies.Append("jwt_token", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddMinutes(30)
                    
                    });

                    return RedirectToAction("Index","Dashboard");

                }
                return View("Login");

            }
        }

        public IActionResult LogoutUser(UserDto dto)
        {
            Response.Cookies.Delete("jwt_token");
            return RedirectToAction("Login");
        }

        private string GenerateJwtToken(UserDto dto)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("R7wbEBn5BBwqadRy9bQh7eW9PGpwgC2di4hNkxPFBN2");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, dto.Username),
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtHandler.CreateToken(tokenDescriptor);
            return jwtHandler.WriteToken(token);
        }
    }
}