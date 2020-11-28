using Hermes_Services.Repositories;
using Hermes_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Hermes_Services;
using Hermes_Services.Data;

namespace Hermes_chat.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private GroupManager groupManager = new GroupManager();
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ApplicationDbContext db;
        public ChatController(UserManager<IdentityUser> userManager, IHubContext<ChatHub> hubContext, ApplicationDbContext db)
        {
            _userManager = userManager;
            _hubContext = hubContext;
            this.db = db;
        }

        public IActionResult ChatUsers()
        {
            ViewBag.Groups = groupManager.GetAllGroups().Where(g => g.ModeratorId != null);
            ViewBag.user = _userManager.GetUserName(User);
            return View(_userManager.Users.ToList());
        }

        [HttpGet]
        public IActionResult Users(int? id, string userName)
        {
            var groups = groupManager.GetAllGroups();
            var usersDB = db.Users.Select(x => x.UserName);
            var groupDB = db.Group.Select(x => x.Id).ToList();

            if (id.HasValue && groupDB.Contains((int)id) && usersDB.Contains(userName))
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
                if (groupManager.GetByName(groupName) == null)
                {
                    int _id = groupManager.CreateGroup(group.ToData());
                    string url = "https://" + HttpContext.Request.Host + "/Chat/Users/" + _id.ToString() + "?userName=" + name;
                    _hubContext.Clients.User(userName).SendAsync("ReceiveMessage", user, "You were invited to a private chat: ");
                    _hubContext.Clients.User(userName).SendAsync("ReceiveMessageUser", url);
                    return RedirectToAction("Users", "Chat", new { id = _id, userName = name });
                }
                else
                {
                    var group1 = groupManager.GetByName(groupName);
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
            var creatorId = _userManager.GetUserId(User);
            Group group = new Group { CreatorId = creatorId, GroupName = groupName, ModeratorId = creatorId };
            if (ModelState.IsValid)
            {
                if (groupManager.GetByName(groupName) == null)
                {
                    int _id = groupManager.CreateGroup(group.ToData());
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
            var groups = groupManager.GetAllGroups();
            ViewBag.Groups = groups;
            var groupDB = db.Group.Select(x => x.Id).ToList();

            if (id.HasValue && groupDB.Contains((int)id))
            {
                var activeGroup = groups.FirstOrDefault(g => g.Id == id);
                var users = groupManager.GetUsersByGroup(id.Value);
                var numberUsers = groupManager.GetNumberUsersInGroup(id.Value);
                var user = _userManager.GetUserId(User);
                var userInGroup = groupManager.GetUserInGroup(activeGroup.Id, user);
                ViewBag.Group = activeGroup.GroupName;
                ViewBag.GroupId = activeGroup.Id;
                ViewBag.userName = _userManager.GetUserName(User);
                ViewBag.Users = users;
                ViewBag.numberUsers = numberUsers;
                //ViewBag.user = user;
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

            var user = _userManager.GetUserId(User);
            if (groupManager.GetUserInGroup(id, user) == null)
            {
                groupManager.AddUserInGroup(id, user);
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
            var oldModerator = groupManager.GetUserInGroup(id, userId).Id;
            var userName = _userManager.GetUserName(User);
            var groups = groupManager.GetAllGroups();
            var activeGroup = groups.FirstOrDefault(g => g.Id == id);
            var groupName = activeGroup.GroupName;
            var moderatorId = activeGroup.ModeratorId;
            groupManager.DeleteUserIntoGroup(id, userId);
            _hubContext.Clients.Group(groupName).SendAsync("NotifyGroup", $"{userName} left the group");
            int numberUsers = groupManager.GetNumberUsersInGroup(id); 
            if(numberUsers == 0)
            {
                groupManager.DeleteGroup(id);
            }
            else
            {
                if(userId == moderatorId)
                {
                    var nextUser = groupManager.GetUsersByGroup(id).Where(l => l.Id != oldModerator).Min(n => n.Id);
                    var nextUserId = groupManager.GetUserInGroupBiId(nextUser).UserId;
                    var nextUserName = _userManager.Users.FirstOrDefault(g => g.Id == nextUserId).UserName;
                    groupManager.ChangeModeratorInGroup(id, nextUserId);

                    _hubContext.Clients.User(nextUserName).SendAsync("ReceiveMessageNotify", userName, "You have become a moderator of the group: " + groupName);
                }
            }
            return RedirectToAction(nameof(ChatUsers));
        }
    }
}
