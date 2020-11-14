﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes_chat.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hermes_chat.Models;
using Hermes_chat.Managers;
using System.Security.Claims;

namespace Hermes_chat.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private GroupManager groupManager = new GroupManager();
        private readonly SignInManager<IdentityUser> _signInManager;
        public ChatController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult ChatUsers()
        {
            ViewBag.Groups = groupManager.GetAllGroups();

            return View(_userManager.Users.ToList());
        }

        public IActionResult Users(string userName)  
        {
            ViewBag.name = userName;
            return View();
        }

        [HttpGet]
        public IActionResult CreateGroup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateGroup(Group model)
        {
            ViewBag.CreatorId = _userManager.GetUserId(User);
            ViewBag.userName = _userManager.GetUserName(User);
            if (ModelState.IsValid)
            {
                int _id = groupManager.CreateGroup(model.ToData());
                return RedirectToAction("Groups", "Chat", new {id = _id});
                
            }
            else
            {
                return View();
            }
        }


        public IActionResult Groups(int? id)
        {
            var groups = groupManager.GetAllGroups();
            ViewBag.Groups = groups;
            if (id.HasValue)
            {
                var activeGroup = groups.FirstOrDefault(g => g.Id == id);
                //var users = groupManager.GetUsersByGroup(id.Value);
                ViewBag.Group = activeGroup.GroupName;
                ViewBag.userName = _userManager.GetUserName(User);
                //ViewBag.Users = users;
            }
            
            return View(_userManager.Users.ToList());
        }

        //public IActionResult TestChat()
        //{
        //    return View();
        //}

    }
}
