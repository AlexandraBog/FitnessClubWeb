﻿using System;
using System.Linq;
using System.Threading.Tasks;
using FitnessClubWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FitnessClubWeb.Controllers
{
    [Produces("application/json")]

    public class AccountController : Controller
    {
        /// <summary>
        /// класс для входа и авторизиции пользователя
        /// </summary>
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        /// <summary>
        /// функция, для регистрации пользователя
        /// </summary>
        /// <param name="model">объект, который содержить в себе регистрационные данные пользователя</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/account/register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model) 
        {
            
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,                  
                };
                IdentityResult result = new IdentityResult();
                try
                {
                    result = await this.userManager.CreateAsync(user, model.Password);  // записывается результат регистрации пользователя
                }
                catch(Exception e)
                {
                    return BadRequest(e); // возвращает результат ошибку
                }

                if (result.Succeeded)
                {
                    await this.signInManager.SignInAsync(user, false);   // авторизует пользователя
                    await this.userManager.AddToRoleAsync(user, "user");

                    var message = new
                    {
                        message = "Добавлен новый пользователь: " + user.UserName
                    };
                    return Ok(message);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    var errorMsg = new
                    {
                        message = "Пользователь не добавлен.",
                        error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                    };
                    return Ok(errorMsg); // возвращает все возможные ошибки
                }
            }
            else
            {
                var errorMsg = new
                {
                    message = "Неверные входные данные.",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };

                return Ok(errorMsg);
            }
        }

        [HttpPost]
        [Route("api/account/login")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {             
            if (ModelState.IsValid)
            {
                var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var msg = new
                    {
                        message = "Выполнен вход пользователем: " + model.Email
                    };
                    return Ok(msg);
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    var errorMsg = new
                    {
                        message = "Вход не выполнен.",
                        error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                    };
                    return Ok(errorMsg);
                }
            }
            else
            {
                var errorMsg = new
                {
                    message = "Вход не выполнен.",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };
                return Ok(errorMsg);
            }
        }

        [HttpPost]
        [Route("api/account/logoff")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            // Удаление куки
            await this.signInManager.SignOutAsync();
            var msg = new
            {
                message = "Выполнен выход."
            };
            return Ok(msg);
        }

        [HttpPost]
        [Route("api/Account/isAuthenticated")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LogisAuthenticatedOff()
        {
            User usr = await GetCurrentUserAsync();
            var message = usr == null ? "Вы Гость. Пожалуйста, выполните вход." : "Вы вошли как: " + usr.UserName;

            var msg = new
            {
                message
            };

            return Ok(msg);
        }

        [HttpGet]
        [Route("api/Account/userRole")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> GetUserRole()
        {
            User user = await GetCurrentUserAsync();

            if(user == null)
            {
                return BadRequest();
            }

            var roles = await this.userManager.GetRolesAsync(user);

            return Ok(roles.FirstOrDefault());
        }

        private Task<User> GetCurrentUserAsync() => this.userManager.GetUserAsync(HttpContext.User);
    }
}