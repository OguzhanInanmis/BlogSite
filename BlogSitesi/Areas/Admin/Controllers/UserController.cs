using AutoMapper;
using Blog.Entity.Entity;
using Blog.Entity.ViewModels.Users;
using Blog.Services.Extensions;
using Blog.Web.ResultMessages;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly IMapper mapper;
        private readonly IToastNotification toast;
        private readonly IValidator<AppUser> validator;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMapper mapper, IToastNotification toast, IValidator<AppUser> validator)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.toast = toast;
            this.validator = validator;
        }
        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();
            var map = mapper.Map<List<UsersVM>>(users);
            foreach (var user in map)
            {
                var findUser = await userManager.FindByIdAsync(user.Id.ToString());
                //birden fazla rol olursa seperatora değer verilip hepsi tek stringte alınabilir.
                var role = string.Join("", await userManager.GetRolesAsync(findUser));

                user.Role = role;
            }
            return View(map);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var roles = await roleManager.Roles.ToListAsync();
            return View(new UserAddVM { Roles = roles });
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserAddVM userAddVM)
        {
            var map = mapper.Map<AppUser>(userAddVM);
            var validation = await validator.ValidateAsync(map);
            var roles = await roleManager.Roles.ToListAsync();
            if (ModelState.IsValid)
            {
                map.UserName = userAddVM.Email;
                var result = await userManager.CreateAsync(map, string.IsNullOrEmpty(userAddVM.Password) ? "" : userAddVM.Password);
                if (result.Succeeded)
                {
                    var findRole = await roleManager.FindByIdAsync(userAddVM.RoleId.ToString());
                    await userManager.AddToRoleAsync(map, findRole.ToString());
                    toast.AddSuccessToastMessage(Messages.User.Add(userAddVM.Email), new ToastrOptions { Title = "Başarılı" });
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }
                else
                {
                    result.AddToIdentityModelState(this.ModelState);
                    validation.AddToModelState(this.ModelState);
                    return View(new UserAddVM { Roles = roles });
                }
            }
            return View(new UserAddVM { Roles = roles });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            var roles = await roleManager.Roles.ToListAsync();

            var map = mapper.Map<UserUpdateVM>(user);
            map.Roles = roles;
            return View(map);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateVM userUpdateVM)
        {
            var user = await userManager.FindByIdAsync(userUpdateVM.Id.ToString());
            if (user != null)
            {
                var userRole = string.Join("", await userManager.GetRolesAsync(user));
                var roles = await roleManager.Roles.ToListAsync();
                if (ModelState.IsValid)
                {
                    var map = mapper.Map(userUpdateVM, user);
                    var validation = await validator.ValidateAsync(map);
                    if (validation.IsValid)
                    {
                        user.UserName = userUpdateVM.Email;
                        user.SecurityStamp = Guid.NewGuid().ToString();
                        var result = await userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            await userManager.RemoveFromRoleAsync(user, userRole);
                            var findRole = await roleManager.FindByIdAsync(userUpdateVM.RoleId.ToString());
                            await userManager.AddToRoleAsync(user, findRole.Name);
                            toast.AddSuccessToastMessage(Messages.User.Update(userUpdateVM.Email), new ToastrOptions { Title = "Başarılı" });
                            return RedirectToAction("Index", "User", new { Area = "Admin" });
                        }
                        else
                        {
                            result.AddToIdentityModelState(this.ModelState);
                            return View(new UserUpdateVM { Roles = roles });
                        }
                    }
                    else
                    {
                        validation.AddToModelState(this.ModelState);
                        return View(new UserUpdateVM { Roles = roles });
                    }
                }
            }
            return NotFound();
        }
        public async Task<IActionResult> Delete(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    toast.AddSuccessToastMessage(Messages.User.Update(user.Email), new ToastrOptions { Title = "Başarılı" });
                    return RedirectToAction("Index", "User", new { Area = "Admin" });
                }
                else
                {
                    result.AddToIdentityModelState(this.ModelState);
                }
            }
            return NotFound();
        }
    }
}
