using Hermes_Services.Repositories;
using Hermes_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Hermes_Services;

namespace Hermes_chat.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private GroupManager groupManager = new GroupManager();
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IHubContext<ChatHub> _hubContext;
        public ChatController(UserManager<IdentityUser> userManager, IHubContext<ChatHub> hubContext)
        {
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public IActionResult ChatUsers()
        {
            ViewBag.Groups = groupManager.GetAllGroups();
            ViewBag.user = _userManager.GetUserName(User);
            return View(_userManager.Users.ToList());
        }

        [HttpGet]
        public IActionResult Users(int? id, string userName)
        {
            var groups = groupManager.GetAllGroups();
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
                    _hubContext.Clients.User(userName).SendAsync("ReceiveMessage", user, "You were invited to a private chat: ");
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
            Group group = new Group { CreatorId = creatorId, GroupName = groupName };
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
            if (id.HasValue)
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
            groupManager.DeleteUserIntoGroup(id, userId);
            int numberUsers = groupManager.GetNumberUsersInGroup(id); 
            if(numberUsers == 0)
            {
                groupManager.DeleteGroup(id);
            }
            return RedirectToAction(nameof(ChatUsers));
        }
    }
}
