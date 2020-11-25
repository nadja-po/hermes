using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Hermes_Services.Data;
using Microsoft.AspNetCore.Authorization;

namespace Hermes_Services
{
    [Authorize]
    public class ChatHub : Hub
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext db;

        public ChatHub(SignInManager<IdentityUser> signInManager, ApplicationDbContext db)
        {
            _signInManager = signInManager;
            this.db = db;
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
            if (Context.UserIdentifier != user)
            await Clients.User(Context.UserIdentifier).SendAsync("ReceiveMessage", signin, message);
            await Clients.User(user).SendAsync("ReceiveMessage", signin, message);

        }
        //Connecting and disconnecting  
        //Notification function for incoming and outgoing users
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify", $"{Context.UserIdentifier} joined the chat");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("Notify", $"{Context.UserIdentifier} left the chat");
            await base.OnDisconnectedAsync(exception);
        }

        //CHAT ROOMS
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("Notify", $"{ Context.UserIdentifier} joined the group");
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync(Context.UserIdentifier + " left the group");
        }

        public async Task SendMessageGroup(string message, string groupName)
        {
            var signin = _signInManager.Context.User.Identity.Name;
            await Clients.Group(groupName).SendAsync("ReceiveMessageGroup", signin, message).ConfigureAwait(true);
        }
        
    }
}
