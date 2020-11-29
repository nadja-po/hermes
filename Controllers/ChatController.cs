using Hermes_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Hermes_Services;
using Hermes_Services.Handler;
using System.Collections.Generic;

namespace Hermes_chat.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private GroupHandler _groupHandler = new GroupHandler();
        private UsersInGroupHandler _usersInGroupHandler = new UsersInGroupHandler();
        private readonly IHubContext<ChatHub> _hubContext;
        public ChatController(UserManager<AppUser> userManager, IHubContext<ChatHub> hubContext)
        {
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public IActionResult ChatUsers()
        {
            ViewBag.Groups = _groupHandler.GetAll().Where(g => g.ModeratorId != null);
            ViewBag.user = _userManager.GetUserName(User);
            return View(_userManager.Users.ToList());
        }

        [HttpGet]
        public IActionResult Users(int? id, string userName)
        {
            var groups = _groupHandler.GetAll();
            if (id.HasValue)
            {
                var activeGroup = groups.FirstOrDefault(g => g.Id == id);
                var creatorId = activeGroup.CreatorId;
                var creatorName = _userManager.Users.FirstOrDefault(g => g.Id == creatorId).UserName;
                ViewBag.creator = creatorName;
                ViewBag.user = userName;
                ViewBag.group = activeGroup.GroupName;
            }

            return View();
        }

        public IActionResult UserPrivateChat(string userName)
        {
            var name = userName;
            var creatorId = _userManager.GetUserId(User);
            string user = _userManager.GetUserName(User);
            string groupName = userName + user;
            Group group = new Group { CreatorId = creatorId, GroupName = groupName };
            if (ModelState.IsValid)
            {
                if (_groupHandler.GetByName(groupName) == null)
                {
                    int _id = _groupHandler.Create(group);
                    string url = "https://" + HttpContext.Request.Host + "/Chat/Users/" + _id.ToString() + "?userName=" + name;
                    _hubContext.Clients.User(userName).SendAsync("ReceiveMessage", user, "You were invited to a private chat: ");
                    _hubContext.Clients.User(userName).SendAsync("ReceiveMessageUser", url);
                    return RedirectToAction("Users", "Chat", new { id = _id, userName = name });
                }
                else
                {
                    var group1 = _groupHandler.GetByName(groupName);
                    int _id = group1.Id;
                    string url = "https://" + HttpContext.Request.Host + "/Chat/Users/" + _id.ToString() + "?userName=" + name;
                    _hubContext.Clients.User(userName).SendAsync("ReceiveMessageNotify", user, "You were invited to a private chat: ");
                    _hubContext.Clients.User(userName).SendAsync("ReceiveMessageUser", url);
                    return RedirectToAction("Users", "Chat", new { id = _id, userName = name });
                }
            }
            else
            {
                return RedirectToAction(nameof(ChatUsers));
            }
        }

        [HttpGet]
        public IActionResult CreateGroup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateGroup(string groupName)
        {
            var user = _userManager.GetUserAsync(User).Result;
            Group group = new Group { CreatorId = user.Id, GroupName = groupName, ModeratorId = user.Id};
            if (user.UsersInGroup == null)
            {
                user.UsersInGroup = new List<UsersInGroup>();
            }
            if (ModelState.IsValid)
            {
                if (_groupHandler.GetByName(groupName) == null)
                {
                    _userManager.UpdateAsync(user);
                    int _id = _usersInGroupHandler.CreateUserInGroup(group, user.Id);
                    return RedirectToAction("Groups", "Chat", new { id = _id });
                }
                else
                {
                    ModelState.AddModelError("GroupName", "This group name is already taken");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }


        public IActionResult Groups(int? id)
        {
            var groups = _groupHandler.GetAll();
            ViewBag.Groups = groups;
            if (id.HasValue)
            {
                var activeGroup = groups.FirstOrDefault(g => g.Id == id);
                var users = _usersInGroupHandler.GetUsersByGroup(id.Value);
                var numberUsers = _groupHandler.GetNumberUsersInGroup(id.Value);
                var user = _userManager.GetUserId(User);
                var userInGroup = _usersInGroupHandler.GetUserInGroup(activeGroup.Id, user);
                ViewBag.Group = activeGroup.GroupName;
                ViewBag.GroupId = activeGroup.Id;
                ViewBag.userName = _userManager.GetUserName(User);
                ViewBag.Users = users;
                ViewBag.numberUsers = numberUsers;
                //ViewBag.user = user;
                ViewBag.userInGroup = userInGroup;
            }

            return View(_userManager.Users.ToList());
        }

        public IActionResult JoinGroup(int id)
        {
            var group = _groupHandler.GetById(id);
            var user = _userManager.GetUserAsync(User).Result;
            if (user.UsersInGroup == null)
            {
                user.UsersInGroup = new List<UsersInGroup>();
            }
            if (_usersInGroupHandler.GetUserInGroup(id, user.Id) == null)
            {
                _usersInGroupHandler.AddUserInGroup(_groupHandler.GetById(id), user.Id);
                return RedirectToAction("Groups", "Chat", new { id });
            }
            else
            {
                ModelState.AddModelError("JoinGroup", "You already joined");
                return RedirectToAction("Groups", "Chat", new { id });
            }

        }
        public IActionResult LeaveGroup(int id)
        {
            var userId = _userManager.GetUserId(User);
            var oldModerator = _usersInGroupHandler.GetUserInGroup(id, userId).Id;
            var userName = _userManager.GetUserName(User);
            var groups = _groupHandler.GetAll();
            var activeGroup = groups.FirstOrDefault(g => g.Id == id);
            var groupName = activeGroup.GroupName;
            var moderatorId = activeGroup.ModeratorId;
            _usersInGroupHandler.DeleteUserIntoGroup(id, userId);
            _hubContext.Clients.Group(groupName).SendAsync("NotifyGroup", $"{userName} left the group");
            int numberUsers = _groupHandler.GetNumberUsersInGroup(id); 
            if(numberUsers == 0)
            {
                _groupHandler.Delete(_groupHandler.GetById(id));
            }
            else
            {
                if(userId == moderatorId)
                {
                    var nextUser = _usersInGroupHandler.GetUsersByGroup(id).Where(l => l.Id != oldModerator).Min(n => n.Id);
                    var nextUserId = _usersInGroupHandler.GetUserInGroupById(nextUser).UserId;
                    var nextUserName = _userManager.Users.FirstOrDefault(g => g.Id == nextUserId).UserName;
                    _groupHandler.ChangeModeratorInGroup(id, nextUserId);

                    _hubContext.Clients.User(nextUserName).SendAsync("ReceiveMessageNotify", userName, "You have become a moderator of the group: " + groupName);
                }
            }
            return RedirectToAction(nameof(ChatUsers));
        }
    }
}
