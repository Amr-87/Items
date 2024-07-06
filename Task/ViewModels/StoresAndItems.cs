using Task.Models.Entities;

namespace Task.ViewModels
{
    public class StoresAndItems
    {
        public ICollection<Store> Stores { get; set; } = new HashSet<Store>();
        public ICollection<Item> Items { get; set; } = new HashSet<Item>();
    }
}
