using CommentManager.Classes;
using CommentManager.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using X.PagedList;

namespace CommentManager.Controllers
{
    /// <summary>
    /// Main controller
    /// </summary>
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

        /// <summary>
        /// Main page
        /// </summary>
        /// <returns>Main page</returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Action adding a comment to the database
        /// </summary>
        /// <param name="userName">User name from the page</param>
        /// <param name="message">User message  from the page</param>
        /// <param name="page">Number of current page</param>
        /// <param name="replyCommentId">Id of the comment of the second comment thread</param>
        /// <param name="divId">Current HTML block to work with</param>
        /// <param name="btnId">Current HTML button to work with</param>
        /// <param name="secondLvl">Was the second comment thread selected?</param>
        /// <param name="divIdInsert">Where to insert the result</param>
        /// <returns>Json with results</returns>
        [HttpPost]
        public IActionResult LeaveComment(string userName, string message, int? page, int? replyCommentId, string divId, string btnId, bool secondLvl, string divIdInsert)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(message))
            {
                return Json(new
                {
                    divId,
                    btnId,
                    message = "All fields are required!",
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

            if (replyCommentId.HasValue)
            {
                currentComment.Comment_Id = replyCommentId.Value;
            }

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

        /// <summary>
        /// Displaying all comments
        /// </summary>
        /// <param name="page">Number of current page</param>
        /// <returns>Partial view with comments</returns>
        [HttpPost]
        public IActionResult Comments(int? page)
        {
            var comments = _context.Comment.Where(x => x.Comment_Id == null).ToList();

            ViewBag.Page = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.CommentsList = comments.ToPagedList(page.HasValue ? Convert.ToInt32(page) : 1, 5);

            return PartialView("~/Views/Home/PartialViews/Comments.cshtml");
        }

        /// <summary>
        /// Find a comment from an internal comment thread
        /// </summary>
        /// <param name="replyCommentId">Internal comment id</param>
        /// <returns>Partial view with internal comments</returns>
        [HttpPost]
        public IActionResult SingleCommentLvl2(int replyCommentId)
        {
            var model = _context.Comment.Where(x => x.Comment_Id == replyCommentId).OrderByDescending(o => o.Id).FirstOrDefault();

            return PartialView("~/Views/Home/PartialViews/SingleCommentLvl2.cshtml", model);
        }

        /// <summary>
        /// Check if the main comment has an internal comment thread
        /// </summary>
        /// <param name="divId">Main brach comment id</param>
        /// <returns>Result</returns>
        [HttpPost]
        public IActionResult CheckSecondBranch(string divId)
        {
            var commentId = divId.Split("-").LastOrDefault();

            var model = _context.Comment.Where(x => x.Comment_Id == int.Parse(commentId)).ToList();

            if (model == null || model.Count() == 0)
            {
                return Json(new
                {
                    status = false
                });
            }
            else
            {
                return Json(new
                {
                    status = true
                });
            }
        }

        /// <summary>
        /// Base action for errors
        /// </summary>
        /// <returns>View with error data</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion
    }
}
