using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Hermes_Services.Data;
using Microsoft.AspNetCore.Authorization;
using Hermes_Models;
using System.Linq;

namespace Hermes_Services
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext db;

        public ChatHub(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext db)
        {
            _userManager = userManager;
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
            await Clients.All.SendAsync("Notify", /*$"{Context.UserIdentifier} joined the chat", */Context.UserIdentifier);
            await base.OnConnectedAsync();
            AppUser user = this.db.AppUsers.FirstOrDefault(t => t.UserName == Context.User.Identity.Name);
            user.IsConnected = true;
            await _userManager.UpdateAsync(user);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("NotifyD", /*$"{Context.UserIdentifier} left the chat"*/Context.UserIdentifier);
            await base.OnDisconnectedAsync(exception);
            AppUser user = this.db.AppUsers.FirstOrDefault(t => t.UserName == Context.User.Identity.Name);
            user.IsConnected = false;
            await _userManager.UpdateAsync(user);
        }

        //CHAT ROOMS
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("NotifyGroup", $"{ Context.UserIdentifier} joined the group");
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("NotifyGroup", $"{ Context.UserIdentifier} left the group");
        }

        public async Task SendMessageGroup(string message, string groupName)
        {
            var signin = _signInManager.Context.User.Identity.Name;
            await Clients.Group(groupName).SendAsync("ReceiveMessageGroup", signin, message).ConfigureAwait(true);
        }
        
    }
}
