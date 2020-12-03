using Hermes_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Hermes_Services;
using Hermes_Services.Handler;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            ViewBag.users = _userManager.Users.ToList().OrderByDescending(i => i.IsConnected);
            return View();
        }

        [HttpGet]
        public IActionResult Users(int? id, string userName)
        {
            var groups = _groupHandler.GetAll();
            if (id.HasValue && _groupHandler.GetAll().Any(x => x.Id == id) && _userManager.Users.Any(x => x.UserName == userName))
            {
                var activeGroup = groups.FirstOrDefault(g => g.Id == id);
                var creatorId = activeGroup.CreatorId;
                var creatorName = _userManager.Users.FirstOrDefault(g => g.Id == creatorId).UserName;
                ViewBag.creator = creatorName;
                ViewBag.user = userName;
                ViewBag.group = activeGroup.GroupName;
            }
            else
            {
                ModelState.AddModelError("error", "User not found!");
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
                    _hubContext.Clients.User(userName).SendAsync("ReceiveMessageNotify", user, "You were invited to a private chat: ");
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
            Group group = new Group { CreatorId = user.Id, GroupName = groupName, ModeratorId = user.Id };
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
            if (id.HasValue)/* in brackets && groups.Any(x=>x.Id == id)*/
            {
                var activeGroup = groups.FirstOrDefault(g => g.Id == id);
                var users = _usersInGroupHandler.GetUsersByGroup(id.Value);
                var numberUsers = _groupHandler.GetNumberUsersInGroup(id.Value);
                var user = _userManager.GetUserId(User);
                var userName = _userManager.GetUserName(User);
                List<AppUser> usersInGroup = new List<AppUser>();
                foreach (var u in users)
                {
                    var userInGroupId = _usersInGroupHandler.GetId(u);
                    var user1 = _userManager.Users.FirstOrDefault(k => k.Id == userInGroupId);
                    usersInGroup.Add(user1);
                }
                var userInGroup = _usersInGroupHandler.GetUserInGroup(activeGroup.Id, user);
                ViewBag.Group = activeGroup.GroupName;
                ViewBag.GroupId = activeGroup.Id;
                ViewBag.userName = _userManager.GetUserName(User);
                ViewBag.numberUsers = numberUsers;
                ViewBag.usersInGroup = usersInGroup.OrderByDescending(i => i.IsConnected);
                ViewBag.userInGroup = userInGroup;
            }

            else
            {
                ModelState.AddModelError("error", "Group not found!");

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
                _usersInGroupHandler.AddUserInGroup(group, user.Id);
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
            var user = _userManager.GetUserAsync(User).Result;
            var oldModerator = user.Id;
            var activeGroup = _groupHandler.GetById(id);
            var users = _usersInGroupHandler.GetUsersByGroup(id);
            _usersInGroupHandler.DeleteUserIntoGroup(id, user.Id);
            _hubContext.Clients.Group(activeGroup.GroupName).SendAsync("NotifyGroup", $"{user.UserName} left the group");
            if (_usersInGroupHandler.GetUsersByGroup(id).Count < 1)
            {
                _groupHandler.Delete(activeGroup);
            }
            else
            {
                if (user.Id == activeGroup.ModeratorId)
                {
                    var nextUser = _usersInGroupHandler.GetUsersByGroup(id).FirstOrDefault(l => l.UserId != oldModerator);
                    _groupHandler.ChangeModeratorInGroup(id, nextUser.UserId);

                    _hubContext.Clients.User(_userManager.FindByIdAsync(nextUser.UserId).Result.UserName).SendAsync("ReceiveMessageNotify", user.UserName, "You have become a moderator of the group: " + activeGroup.GroupName);
                }
            }
            return RedirectToAction(nameof(ChatUsers));
        }

        public IActionResult BanList(int? groupId)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var activeGroup = _groupHandler.GetById(groupId.Value);
            var moderatorId = activeGroup.ModeratorId;
            var moderator = _userManager.Users.FirstOrDefault(k => k.Id == moderatorId);
            var users = _usersInGroupHandler.GetUsersByGroup(groupId.Value);
            var banList = _usersInGroupHandler.GetAllBanned(groupId.Value);
            ViewBag.Moderator = moderator.UserName;
            ViewBag.Group = activeGroup.GroupName;
            ViewBag.GroupId = activeGroup.Id;
            ViewBag.Banlist = banList;
            return View();
        }
        public IActionResult Banned(int? groupId, string id)
        {
            _usersInGroupHandler.Banned(groupId.Value, id);
            return View(nameof(Groups));
        }
        public IActionResult Return(int? groupId, string id)
        {
            _usersInGroupHandler.Return(groupId.Value, id);
            return View(nameof(Ban));
        }
    }
}
