using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;  
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;  
using Microsoft.AspNetCore.Authorization;  
using TaskManager.Models;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Dapper.Contrib;
using TaskList.ViewModels;
using Microsoft.Extensions.Configuration;
using Dapper.Contrib.Extensions;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;  
        private readonly SignInManager<IdentityUser> _signInManager;  
        private readonly IConfiguration configuration;  

        private readonly DbContextOptions _options;

        public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager,  SignInManager<IdentityUser> signInManager,IConfiguration iConfig, DbContextOptions options)
        {
            
            _logger = logger;
            _userManager = userManager;  
            _signInManager = signInManager; 
            configuration = iConfig;
            _options= options;
        }

        public IActionResult Index()
        {
            string dbConn = configuration.GetValue<string>("ConnectionStrings:MyConn");  
            TaskListViewModel viewModel = new TaskListViewModel();  
            return View("Index", viewModel);

        }
    /*    
     public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            string dbConn = configuration.GetValue<string>("ConnectionStrings:MyConn");  
            var context = new AppDbContext(_options);
            var Tasks = from m in context.TaskListItems
                        select m;       
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            

            if (!String.IsNullOrEmpty(searchString))
            {
                Tasks = Tasks.Where(s => s.Description.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Date":
                    Tasks = Tasks.OrderBy(v => v.DueDate);
                    break;
                case "date_desc":
                    Tasks = Tasks.OrderByDescending(v => v.DueDate);
                    break;
            }
           // await View("Index", viewModel);
        } 
    
*/       
        public IActionResult Edit(int id)
        {
            string dbConn = configuration.GetValue<string>("ConnectionStrings:MyConn");  
            TaskListViewModel viewModel = new TaskListViewModel();
            viewModel.EditableItem = viewModel.TaskItems.FirstOrDefault(x => x.Id == id);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            
            string dbConn = configuration.GetValue<string>("ConnectionStrings:MyConn");  
            using(var db = new SqlConnection(dbConn))
            {
            TaskListItem item = db.Get<TaskListItem>(id);
            if(item != null)
                db.Delete(item);
            return RedirectToAction("Index");
            }
        }

        public IActionResult CreateUpdate(TaskListViewModel viewModel)
        {
            string dbConn = configuration.GetValue<string>("ConnectionStrings:MyConn");  
            if(ModelState.IsValid)
            {
            using(var db = new SqlConnection(dbConn))
            {
                if(viewModel.EditableItem.Id <= 0)
                {
                    db.Insert<TaskListItem>(viewModel.EditableItem);
                }
                else
                {
                TaskListItem dbItem = db.Get<TaskListItem>(viewModel.EditableItem.Id);
                TryUpdateModelAsync<TaskListItem>(dbItem, "EditableItem");
                db.Update<TaskListItem>(dbItem);
                }
                return RedirectToAction("Index");
            }
            }
            else
                {
                    TaskListViewModel taskListViewModel = new TaskListViewModel();
                    return View("Index", taskListViewModel);
                }
               
        }

        public IActionResult ToggleComlpeted(int id)
        {
        string dbConn = configuration.GetValue<string>("ConnectionStrings:MyConn");  
        using(var db = new SqlConnection(dbConn))
            {
            TaskListItem item = db.Get<TaskListItem>(id);
            if(item != null)
            {
                item.Completed = !item.Completed;
                db.Update<TaskListItem>(item);
            }
            TaskListViewModel taskListViewModel = new TaskListViewModel();
            return View("Index", taskListViewModel);
            }
        }
    
        public IActionResult Privacy()
        {
            
            return View();
        }
       
      
        [HttpGet]  
        public IActionResult Login()  
        {  
            return View();  
        }  
  
        [HttpPost]  
        public async Task<IActionResult> Login(LoginViewModel appUser)  
        {
           
            if (appUser is null)
            {
                throw new ArgumentNullException(nameof(appUser));
            }

            //login functionality  
            var user = await _userManager.FindByNameAsync(appUser.Email);  
  
            if (user != null)  
            {  
                //sign in  
                if (appUser.Password != null)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(appUser.Email, appUser.Password, false, false);  
                    if (signInResult.Succeeded)  
                    {  
                        return RedirectToAction("Index");  
                    }
                }  
            }  
  
            return RedirectToAction("Register");  
        }  
  
        public IActionResult Register()  
        {  
            return View();  
        }  
  
        [HttpPost]  
        public async Task<IActionResult> Register(RegisterViewModel model)  
        {  
            //register functionality  
            var user = new IdentityUser  
            {  
                Email = model.Email,  
                UserName = model.Email
            };  
  
            var result = await _userManager.CreateAsync(user, model.Password);  
            
  
            if (result.Succeeded)  
            {  
                // User sign  
                // sign in   
                var signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);  
  
                if (signInResult.Succeeded)  
                {  
                    return RedirectToAction("Index");  
                }  
            }  
  
            return View();  
        }  
  
        public async Task<IActionResult> LogOut(string username, string password)  
        {  
            await _signInManager.SignOutAsync();  
            return RedirectToAction("Index");  
        }  

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
    }
}