using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VirtualDice.Hubs;
using VirtualDice.Models;

namespace VirtualDice.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Dashboard(NickName nickName)
        {
            bool isUserNameFree = UsersHandler.Users.FirstOrDefault(x => x.Name == nickName.Name) == null;

            if (!isUserNameFree)
            {
                ModelState.AddModelError("NickNameTaken", "This nickname is already in use!");
            }

            if (!ModelState.IsValid)
            {
                return this.View("Index", nickName);
            }

            nickName.Score = null;

            if (UsersHandler.Users.Count == 0)
            {
                nickName.IsAdmin = true;
            }

            UsersHandler.Users.Add(nickName);

            return View(nickName);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
