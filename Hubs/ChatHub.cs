﻿using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Hermes_chat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        public ChatHub(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }
        
        public async Task SendMessage(string message)
        {
            var signin = _signInManager.Context.User.Identity.Name;
            await Clients.All.SendAsync("ReceiveMessage", signin, message);
        }

        
        //CODE BELOW MIGHT BE USEFUL PRIVATE CHAT
        //public async Task SendMessageUser(string user, string message)
        //{
        //    var signin = _signInManager.Context.User.Identity.Name;
        //    await Clients.User(user).SendAsync("ReceiveMessage", signin, message);
        //}
    }
}
