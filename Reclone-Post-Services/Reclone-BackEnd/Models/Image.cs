namespace Reclone_BackEnd.Models
{
    public class Image
    {
        public long Id { get; set; }
        public string PublicId { get; set; }
        public string UserId { get; set; }
        public string? Caption { get; set; }
        public string? URL { get; set; }

        public string? Tag { get; set; }

        public int? likes { get; set; } = 0;
    }
}
