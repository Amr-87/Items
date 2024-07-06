namespace Task.Models.Entities
{
    public class StoreItem
    {
        public int StoreId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }

        public Store Store { get; set; }
        public Item Item { get; set; }
    }
}
