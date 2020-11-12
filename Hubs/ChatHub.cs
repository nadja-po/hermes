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
            if (Context.UserIdentifier != user)
                await Clients.User(Context.UserIdentifier).SendAsync("ReceiveMessage", signin, message);
            await Clients.User(user).SendAsync("ReceiveMessage", signin, message);

        }
        //Connecting and disconnecting  
        //Notification function for incoming and outgoing users
        public override async Task OnConnectedAsync()
        {
                await Clients.All.SendAsync("Notify", $"{Context.UserIdentifier} entered the chat");
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
            await Clients.Group(groupName).SendAsync("Notify", $"{ Context.UserIdentifier} added to group");
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync(Context.UserIdentifier + " left group");
        }

        public async Task SendMessageGroup(string message, string groupName)
        {
            var signin = _signInManager.Context.User.Identity.Name;
            await Clients.Group(groupName).SendAsync("ReceiveMessage", signin, message).ConfigureAwait(true);
        }
        
    }
}
