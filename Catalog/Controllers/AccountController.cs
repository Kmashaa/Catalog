using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Catalog.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.AspNetCore.Identity;
using Catalog.Migrations;
using Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using Catalog.Data;
using static System.Formats.Asn1.AsnWriter;

namespace Catalog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { Email = model.Email, UserName = model.Email, Surname=model.Surname, Name=model.Name, Patronymic=model.Patronymic, RoleId=2, EmailConfirmed=true };


                var email = new Microsoft.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@Email",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Direction = System.Data.ParameterDirection.Input,
                    Size = 50, 
                    Value = user.Email
                };
                var username = new Microsoft.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@UserName",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Direction = System.Data.ParameterDirection.Input,
                    Size = 50,
                    Value = user.UserName
                };
                var surname = new Microsoft.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@Surname",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Direction = System.Data.ParameterDirection.Input,
                    Size = 50,
                    Value = user.Surname

                };
                var name = new Microsoft.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@Name",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Direction = System.Data.ParameterDirection.Input,
                    Size = 50,
                    Value = user.Name

                };
                var patronymic = new Microsoft.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@Patronymic",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Direction = System.Data.ParameterDirection.Input,
                    Size = 50,
                    Value = user.Patronymic

                };
                var password = new Microsoft.Data.SqlClient.SqlParameter
                {
                    ParameterName = "@Password",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Direction = System.Data.ParameterDirection.Input,
                    Size = 50,
                    Value = model.Password

                };

                _context.Database.ExecuteSqlRaw("register @Email, @UserName, @Surname, @Name, @Patronymic, @Password", email, username, surname, name, patronymic, password );
                 //установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                //if (result.Succeeded)
                //{
                //    // установка куки
                //    await _signInManager.SignInAsync(user, false);
                //    return RedirectToAction("Index", "Home");
                //}
                //else
                //{
                //    foreach (var error in result.Errors)
                //    {
                //        ModelState.AddModelError(string.Empty, error.Description);
                //    }
                //}
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {

            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            
            if (ModelState.IsValid)
            {

                var user = _context.Users.Where(d => d.UserName == model.UserName).ToList();
                var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, model.UserName),
                        new(ClaimTypes.Role,user[0].RoleId.ToString())
                    };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
                //    var result =
                //            await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                //    if (result.Succeeded)
                //    {

                //        // проверяем, принадлежит ли URL приложению
                //        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                //        {
                //            return Redirect(model.ReturnUrl);
                //        }
                //        else
                //        {
                //            return RedirectToAction("Index", "Home");
                //        }
                //    }

                //    else
                //    {
                //        ModelState.AddModelError("", "Неправильный логин и (или) пароль");

                //    }
                //}
            }
                return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync();
            HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
            return RedirectToAction("Index", "Home");
        }


        public string CheckRole(string login)
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            //var email = HttpContext.User.Identity.Name;
            if (identity == null)
            {
                return "-1";
            }
            else
            {
                var role = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).SingleOrDefault();
                return role;
            }
        }
    }
}