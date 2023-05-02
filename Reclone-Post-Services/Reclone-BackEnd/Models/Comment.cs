namespace Reclone_BackEnd.Models
{
    public class Comment
    {
        public long Id { get;  set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string? Content { get; set; }

    }
}
