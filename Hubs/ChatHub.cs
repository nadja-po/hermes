using Microsoft.AspNetCore.SignalR;
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


        //PRIVATE MESSAGE
        public async Task SendMessageUser(string user, string message)
        {
            var signin = _signInManager.Context.User.Identity.Name;
            await Clients.User(user).SendAsync("ReceiveMessage", signin, message);

        }


        //public async Task SendMessageUser(string message, string user)
        //{
        //    var userName = Context.User.Identity.Name;

        //    if (Context.UserIdentifier != user) // если получатель и текущий пользователь не совпадают
        //        await Clients.User(Context.UserIdentifier).SendAsync("ReceiveMessage", userName, message);
        //    await Clients.User(user).SendAsync("ReceiveMessage", userName, message);
        //}
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify", $"{Context.UserIdentifier} entered the chat");
            await base.OnConnectedAsync();
        }
    }
}
