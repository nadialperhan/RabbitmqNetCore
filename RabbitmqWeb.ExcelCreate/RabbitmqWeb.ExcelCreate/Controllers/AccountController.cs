using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace RabbitmqWeb.ExcelCreate.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Login(string Email,string Password)
        {

            var hasuser= await _userManager.FindByEmailAsync(Email);
            if (hasuser == null) 
            {
                return View();
            }
            var signinresult = await _signInManager.PasswordSignInAsync(hasuser, Password, true, false);
            if (!signinresult.Succeeded) 
            {
                return View();
            }
            return RedirectToAction(nameof(HomeController.Index),"Home");
        }
    }
}
