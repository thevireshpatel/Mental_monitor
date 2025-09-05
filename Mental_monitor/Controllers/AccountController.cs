using MentalMonitor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MentalMonitor.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signIn;
    private readonly UserManager<ApplicationUser> _userMgr;

    public AccountController(SignInManager<ApplicationUser> s, UserManager<ApplicationUser> u)
    {
        _signIn = s; _userMgr = u;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password, bool remember, string? returnUrl)
    {
        var user = await _userMgr.FindByEmailAsync(email);
        if (user != null)
        {
            var res = await _signIn.PasswordSignInAsync(user.UserName!, password, remember, lockoutOnFailure: false);
            if (res.Succeeded) return LocalRedirect(returnUrl ?? "~/");
        }
        ModelState.AddModelError("", "Invalid login");
        return View();
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string email, string password)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = email, Email = email };
            var res = await _userMgr.CreateAsync(user, password);
            if (res.Succeeded)
            {
                await _signIn.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            foreach (var e in res.Errors) ModelState.AddModelError("", e.Description);
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signIn.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}