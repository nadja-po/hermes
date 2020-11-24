using Hermes_Services.Handlers;
using Hermes_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
                if (groupManager.GetByName(model.GroupName) == null)
                {
                    int _id = groupManager.CreateGroup(model);
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
                groupManager.Delete(groupManager.Get(id));
            }
            return RedirectToAction(nameof(ChatUsers));
        }
    }
}
