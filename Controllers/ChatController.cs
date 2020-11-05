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

        public IActionResult ChatUsers()
        {
            return View();
        }

        public IActionResult TestChat()
        {
            return View();
        }
    }
}
