using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication1.Data;

namespace WebApplication1.Controllers;

[Authorize(Roles = "Admin")]
public class RoleController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;


    public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return View(roles);
    }

    public async Task<IActionResult> AddRole(string userRole)
    {
        string msg = "";
        if (!string.IsNullOrEmpty(userRole))
        {
            if (await _roleManager.RoleExistsAsync(userRole))
            {
                msg = $"Role {userRole} Already exists!";
            }
            else
            {
                IdentityRole role = new IdentityRole(userRole);
                await _roleManager.CreateAsync(role);
                msg = $"Role {userRole} Created Successfully!";
            }
        }
        else
        {
            msg = "Please Insert a Valid Role";
        }
        ViewBag.Msg = msg;
        var roles = await _roleManager.Roles.ToListAsync();
        return View("Index", roles);
    }

    public IActionResult AssignRole()
    {
        ViewBag.users = _userManager.Users;
        ViewBag.roles = _roleManager.Roles;
        ViewBag.msg = TempData["msg"];
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AssignRole(string userData, string roleData)
    {
        string msg = "";
        if (!string.IsNullOrEmpty(userData) && !string.IsNullOrEmpty(roleData))
        {
            AppUser u = await _userManager.FindByEmailAsync(userData);
            if (u != null)
            {
                if (await _roleManager.RoleExistsAsync(roleData))
                {
                    await _userManager.AddToRoleAsync(u, roleData);
                    msg = "Role Has been Assigned";

                }
                else
                {
                    msg = "Role does not exist";
                }
            }
            else
            {
                msg = "User not found";
            }

        }
        else
        {
            msg = "Please Select a valid user & role";

        }
        TempData["msg"] = msg;
        return RedirectToAction("AssignRole");
    }
}

