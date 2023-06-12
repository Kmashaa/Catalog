namespace Catalog.Entities
{
    public class Credit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Percents { get; set; }
        public int Months { get; set; }
        public string? GeneralNote { get; set; }
        public string? SpecialNote { get; set; }
        public int CategoryId { get; set; }
    }
}
