using ApplicationManager.AuthApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ApplicationManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        #region Вход
        [HttpGet, AllowAnonymous]
        public IActionResult Login(string returnUrl) //открытие страницы входа
        {
            if (returnUrl is null) //если нажали на кнопку напрямую
            {
                returnUrl = "/Home/Index";
            }
            return View(new UserLogin()
            {
                ReturnUrl = returnUrl
            });

        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogin model) // обработка введенных данных на странице входа
        {
            if (ModelState.IsValid)
            {
                var loginResult = await _signInManager.PasswordSignInAsync(model.LoginProp,
                    model.Password,
                    false,
                    lockoutOnFailure: false); //попытка найти в бд пользователя с введенными логином и паролем

                if (loginResult.Succeeded) //если попытка успешна
                {
                    if (Url.IsLocalUrl(model.ReturnUrl)) // если при входе была переадресация с другой страницы
                    {
                        return Redirect(model.ReturnUrl); //возврат на исходную страницу
                    }

                    return RedirectToAction("Index", "Book"); // иначе возврат на стартовую страницу
                }

            }

            ModelState.AddModelError("", "Пользователь не найден");
            return View(model); //если не найден, повторная попытка
        }
        #endregion

        #region Регистрация 
        [HttpGet, AllowAnonymous, Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View(new UserRegistration());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistration model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.LoginProp };
                var createResult = await _userManager.CreateAsync(user, model.Password);

                if (createResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else //если регистрация не удалась
                {
                    foreach (var identityError in createResult.Errors)
                    {
                        ModelState.AddModelError("", identityError.Description);
                    }
                }
            }

            return View(model);
        }
        #endregion

        #region Выход
        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
