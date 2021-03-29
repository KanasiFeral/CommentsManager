using CommentManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CommentManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static List<Comment> _comments;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(string userName, string message)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(message))
            {
                return Json(new
                {
                    divId = "sendMessageBlock",
                    message = "All fields are required!",
                    btnId = "sendMessageBtn",
                    time = 5000
                });
            }

            _comments.Add(new Comment()
            {
                UserName = userName,
                Message = message                
            });

            return Json(new { divId = "sendMessageBlock", message = "Comment has been added!", btnId = "sendFeedbackBtn", time = 5000 });
        }

        [HttpPost]
        public IActionResult Comments(int? page)
        {
            ViewBag.Page = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CommentsList = _comments;

            return PartialView("~/Views/Home/PartialViews/Comments.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
