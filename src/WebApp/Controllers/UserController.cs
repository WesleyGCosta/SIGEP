﻿using Domain.Entities;
using Domain.Notifications.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Factories;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            INotifier notifier) : base(notifier)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Management()
        {
            return View(await GetAsyncListUsers());
        }

        private async Task<List<User>> GetAsyncListUsers()
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync();
            foreach (var user in users)
            {
                user.Role = new List<string>(await _userManager.GetRolesAsync(user));
            }

            return users;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            await ViewBagRoles();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserFactory.ToIdentityUser(model);

                var resultCreate = await _userManager.CreateAsync(user, model.Password);            

                if (resultCreate.Succeeded)
                {
                    var resultAddRole = await _userManager.AddToRoleAsync(user, model.Role);
                    if (resultAddRole.Succeeded)
                        return RedirectToAction(nameof(Management));
                }

                foreach (var error in resultCreate.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            await ViewBagRoles();
            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = "Admin, Gerente")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Warning"] = "Usuário não encontrado";
                return NotFound();
            }
            await ViewBagRoles();
            return PartialView("_FormEditUserModal", user);
        }

        private async Task ViewBagRoles()
        {
            ViewBag.Roles = new SelectList(await _roleManager.Roles.AsNoTracking().OrderBy(r => r.Name).ToListAsync(), "Name", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(User user)
        {
            var userConsult = await _userManager.FindByIdAsync(user.Id);
            if (userConsult != null)
            {
                var result = await _userManager.UpdateAsync(user);

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return PartialView("_ListUser", await GetAsyncListUsers());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            TempData["Success"] = "Usuário excluído com sucesso";

            return PartialView("_ListUser", await GetAsyncListUsers());
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logar(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.Lembrar, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Login Inválido");

            }
            return View("Login");
        }

        public IActionResult RestrictedAcess()
        {
            return StatusCode(403);
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}
