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
        public readonly ApplicationDbContext _context;

        public readonly UserManager<AppUser> _userManager;

    public ChatController(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;

    }
    public IActionResult ChatUsers()
        {
            return View();
        }

        public async Task<IActionResult> TestChat()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserName = currentUser.UserName;
            var messages = await _context.Messages.ToListAsync();
            return View();
        }
    }
}
