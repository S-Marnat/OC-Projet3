using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Threading.Tasks;

namespace P3AddNewFunctionalityDotNetCore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(IStringLocalizer<AccountController> localizer, UserManager<IdentityUser> userMgr,
        SignInManager<IdentityUser> signInMgr)
        {
            _localizer = localizer;
            _userManager = userMgr;
            _signInManager = signInMgr;
        }

        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user =
                await _userManager.FindByNameAsync(loginModel.Name);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    if ((await _signInManager.PasswordSignInAsync(user,
                    loginModel.Password, false, false)).Succeeded)
                    {
                        return Redirect(loginModel.ReturnUrl ?? "/Admin/Index");                       
                    }
                }
            }
            ModelState.AddModelError("", _localizer["CredentialsInvalid"]);
            return View(loginModel);
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}