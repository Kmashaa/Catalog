namespace Catalog.Models
{
    public class Logg
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string? Operation { get; set; } //insert, update, delete, select
        public string? Table { get; set; }
        public string? Date { get; set; }
    }
}
