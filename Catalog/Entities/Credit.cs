namespace Catalog.Entities
{
    public class Credit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Percents { get; set; }
        public int CategoryId { get; set; }
    }
}
