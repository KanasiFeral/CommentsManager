using System.Linq;

namespace CommentManager.Classes
{
    /// <summary>
    /// The class responsible for working with comments
    /// </summary>
    public class CommentsManager
    {
        private static readonly DBContext _context = new DBContext();

        /// <summary>
        /// Property storing comments
        /// </summary>
        public static string ReplyBranches { get; set; }

        /// <summary>
        /// Clearing method of global variable storing comment thread
        /// </summary>
        /// <returns>Return null</returns>
        public static string CleanReplyBranches() => ReplyBranches = null;

        /// <summary>
        /// Getting the second line of comments from the database, generating html code
        /// </summary>
        /// <param name="commentId">Comment id</param>
        /// <returns>Generated HTML line with internal comment thread</returns>
        public static string GetSecondLevelComments(int commentId)
        {
            var replyComments = _context.Comment.Where(x => x.Comment_Id == commentId).ToList();

            foreach (var item in replyComments)
            {
                ReplyBranches += "" +
                "<ol class='children' style='margin-top: 10px;'>" +
                "<li class='list-group-item'>" +
                "<div class='comment-content d-flex'>" +
                "<div class='comment-meta'>" +

                $"<p>Date: {item.CommentDate}</p>" +
                $"<p>Time: {item.CommentTime}</p>" +
                $"<p>Name: {item.UserName}</p>" +
                $"<p>Data: {item.Message}</p>" +

                "<div class='d-flex align-items-center'>" +
                $"<a href='javascript: void(0)' class='btn btn-primary replySecondLvl' name='{item.UserName}'>Reply</a>" +
                "</div>" +
                "</div>" +
                "</div></li></ol>";

                // Generate reply branches using recursion
                if (item.Id != 0)
                {
                    if (_context.Comment.Where(x => x.Comment_Id == item.Id).ToList().Count() != 0)
                    {
                        GetSecondLevelComments(item.Id);
                    }
                }
            }

            return ReplyBranches;
        }

        /// <summary>
        /// Getting the status of whether an internal comment thread exists
        /// </summary>
        /// <param name="table">Table name</param>
        /// <param name="commentId">Comment id</param>
        /// <returns>Status of whether an internal comment thread exists</returns>
        public static bool GetReplyBranchesStatus(int commentId) => _context.Comment.Where(x => x.Comment_Id == commentId).ToList().Count() < 0 ? false : true;
    }
}