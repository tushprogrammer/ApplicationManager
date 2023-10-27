using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using ApplicationManager.AuthApp;
using ApplicationManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;

namespace ApplicationManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly IAppData data;

        public AccountController( /*IAppData Data*/
            UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            //data = Data;
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
            //если была попытка перехода на страницу, где обязательна авторизация
            //после успешной авторизации будет пересылка на нужную страницу
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

                //var loginResult = await data.Authenticate(model.LoginProp, model.Password);
                var loginResult = await _signInManager.PasswordSignInAsync(model.LoginProp,
                    model.Password,
                    false,
                    lockoutOnFailure: false); //попытка найти в бд пользователя с введенными логином и паролем, вход в систему

                if (loginResult.Succeeded) //если авторизация успешна
                {
                    if (Url.IsLocalUrl(model.ReturnUrl)) // если при входе была переадресация с другой страницы
                    {
                        return Redirect(model.ReturnUrl); //возврат на исходную страницу
                    }

                    return RedirectToAction("Index", "Book"); // иначе возврат на стартовую страницу
                }

            }
            //выводится в любом случае, пусть то даже неккоректный ввод или не найденный пользователь в бд
            ModelState.AddModelError("", "Пользователь не найден");
            return View(model); //если не найден, повторная попытка
        }
        #endregion

        #region Регистрация 
        [HttpGet, Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View(new UserRegistration());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistration model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.LoginProp, Email = model.Email };
                var createResult = await _userManager.CreateAsync(user, model.Password);

                //добавление нового пользователя гарантированно идет от админа,
                //поэтому пока что автоматическая авторизация не нужна
                if (createResult.Succeeded && model.IsAdmin)
                {
                    var res = _userManager.AddToRoleAsync(user, "Admin").Result; //админ создает других админов
                    return RedirectToAction("UserList", "Account"); //открывается по умолчанию страница со списком пользователей
                }
                else //если регистрация не удалась
                {
                    //вывод ошибкок - причин
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
        
        [Authorize(Roles = "Admin")]
        public IActionResult UserList() //список пользователей
        {
            return View("Users", _userManager.Users.ToList());
        }

        #region Удаление пользователя

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string userid)
        {
            User user = await _userManager.FindByIdAsync(userid);
            if (user != null)
            {
                var r = await _userManager.DeleteAsync(user);
            }
            return Redirect("UserList");
        }
        #endregion
    }
}
