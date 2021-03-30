namespace CommentManager.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int? Comment_Id { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public string CommentDate { get; set; }

        public string CommentTime { get; set; }
    }
}