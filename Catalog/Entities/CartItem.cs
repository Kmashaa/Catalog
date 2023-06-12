namespace Catalog.Entities
{
    public class CartItem
    {
        public Credit Item { get; set; }
        public int Count { get; set; }

        public CartItem(Credit item)
        {
            Item = item;
            Count = 1;
        }
    }
}
