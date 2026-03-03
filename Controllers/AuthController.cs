using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplicationBlogPost.Models.ViewModels;

namespace WebApplicationBlogPost.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManger;
        private RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        //Registr
        //Login
        //Logut
        public AuthController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, 
            SignInManager<IdentityUser> signInManager )
        {
            _userManger = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            
            return View();
        }
        // this code for to Register New User
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            // Check for Validation
            if (ModelState.IsValid)
            {
                // Create Identity User Object
                var user = new IdentityUser
                {
                    UserName = registerViewModel.Email,
                    Email = registerViewModel.Email
                };
                // User Create
              var result= await _userManger.CreateAsync(user, registerViewModel.Password);
                // if User Create Successfuly
                if (result.Succeeded)
                {
                    // if the user role exit in the Data base
                   if( !await _roleManager.RoleExistsAsync("User"))
                    {
                       await _roleManager.CreateAsync(new IdentityRole("User"));
                    }
                   await _userManger.AddToRoleAsync(user, "User");
                   await  _signInManager.SignInAsync(user, true);

                    return RedirectToAction("Index","Post");
                }
            }
            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        //This code for to Check if the user exisset in Database
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
              var user = await _userManger.FindByEmailAsync(loginViewModel.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "The Email Or Password is Incorrect");
                    return View(loginViewModel);
                }
              var signInResult = await _signInManager.PasswordSignInAsync(user,loginViewModel.Password,false,false);
                if (!signInResult.Succeeded)
                {
                    ModelState.AddModelError("", "The Email Or Password is Incorrect");
                    return View(loginViewModel);
                }
                return RedirectToAction("Index", "Post");
            }
            return View(loginViewModel);
        }

        // this code for to Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await  _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Post");
        }
        // this code for AccessDenied for Admin
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
