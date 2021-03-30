using CommentManager.Classes;
using CommentManager.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using X.PagedList;

namespace CommentManager.Controllers
{
    public class HomeController : Controller
    {
        #region Init data

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<HomeController> _logger;
        private readonly DBContext _context = new DBContext();

        public HomeController(IWebHostEnvironment hostingEnvironment, ILogger<HomeController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        #endregion

        #region Actions

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(string userName, string message, int? page, int? replyCommentId, string divId, string btnId, bool secondLvl, string divIdInsert)
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

            // Add data
            Comment currentComment = new Comment()
            {
                UserName = userName,
                Message = message,
                CommentDate = DateTime.Now.ToString("dd.MM.yyyy"),
                CommentTime = DateTime.Now.ToString("hh.mm.ss")
            };

            _context.Comment.Add(currentComment);
            _context.SaveChanges();

            if (secondLvl == true)
            {
                return Json(new 
                { 
                    secondLvl,
                    divIdInsert,
                    replyCommentId,
                    currentComment.CommentDate,
                    currentComment.CommentTime,
                    message
                });
            }
            else
            {
                return Json(new 
                {
                    secondLvl,
                    page
                });
            }
        }

        [HttpPost]
        public IActionResult Comments(int? page)
        {
            var comments = _context.Comment.ToList();

            ViewBag.Page = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CommentsList = comments.ToPagedList(page.HasValue ? Convert.ToInt32(page) : 1, 5);

            return PartialView("~/Views/Home/PartialViews/Comments.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion

        #region Private methods



        #endregion
    }
}
