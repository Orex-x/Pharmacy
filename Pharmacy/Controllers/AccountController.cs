using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Models;
using Pharmacy.ViewModels;

namespace Pharmacy.Controllers;
public class AccountController : Controller
{
    private readonly ApplicationContext _db;
    
    public AccountController(ApplicationContext context)
    {
        _db = context;
    }
    
    public IActionResult Login()
    {
        return View();
    }
        
    public IActionResult Register()
    {
        return View();
    } 
    
    
    [HttpPost] 
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var findUser = await _db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);

        if (findUser != null) return View(model);
        if (model.Password.Length > 7)
        {
            var hasher = new PasswordHasher<User>();
                    
            var user = new User
            {
                Name = model.Name,
                Login = model.Login,
            };
            user.Password = hasher.HashPassword(user, model.Password);
            await Authenticate(user.Login);
                
                
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return RedirectToAction("Account", "Account");
        } 
        ModelState.AddModelError("", "The" +
                                     " password must contain more than 8 characters");
        return View(model);
    }
       
    [HttpPost] 
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid) return View(model);
        
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
            
        if (user != null)
        {
            var hasher = new PasswordHasher<User>();
            var s = hasher
                .VerifyHashedPassword(user, user.Password, model.Password);
                
            if (s == PasswordVerificationResult.Success)
            {
                await Authenticate(user.Login); 
                return RedirectToAction("Account", "Account");
            }
        }
        ModelState.AddModelError("", "Wrong password or login");
        return View(model);
    }
       
    private async Task Authenticate(string userName)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, userName)
        };
        var id = new ClaimsIdentity(claims, "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }
      
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
      
    [Authorize]
    public IActionResult Account()
    {
        var userName = User.Identity!.Name;
        var user = _db.Users.FirstOrDefault(u => u.Login == userName);
        return View(user);
    }
}