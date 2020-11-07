using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes_chat.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hermes_chat.Models;

namespace Hermes_chat.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public ChatController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult ChatUsers()
        {
            return View(_userManager.Users.ToList());
        }

        public IActionResult TestChat()
        {
            return View();
        }

        public IActionResult Users(string userName)  
        {
            //var name = _userManager.FindByNameAsync(userName);
            ViewBag.name = userName;
            return View();
        }
    }
}
