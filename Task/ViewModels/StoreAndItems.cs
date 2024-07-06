using Task.Models.Entities;

namespace Task.ViewModels
{
    public class StoreAndItems
    {
        public Store Store { get; set; }
        public ICollection<Item> ItemsNotInStore { get; set; } = new HashSet<Item>();
    }
}
