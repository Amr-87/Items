using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task.Models.Database;
using Task.Models.Entities;
using Task.ViewModels;
using Task.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task.Controllers
{
    public class StoresManagementController : Controller
    {
        private readonly AppDBContext db;
        private readonly ILogger<StoresManagementController> logger;

        public StoresManagementController(AppDBContext db, ILogger<StoresManagementController> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var allStores = await db.Stores.Include(a => a.StoreItems).ThenInclude(si => si.Item).ToListAsync();
                var allItems = await db.Items.Include(a => a.StoreItems).ToListAsync();

                StoresAndItems model = new() { Items = allItems, Stores = allStores };

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while loading stores and items.");
                return View("Error", new ErrorViewModel { Message = "An error occurred while loading stores and items." });
            }
        }
        [HttpGet]
        public async Task<IActionResult> ShowStore(int id)
        {
            try
            {
                var allStores = await db.Stores.Where(a => a.Id == id).Include(a => a.StoreItems).ThenInclude(si => si.Item).ToListAsync();
                var allItems = await db.Items.Include(a => a.StoreItems).ToListAsync();

                StoresAndItems model = new() { Items = allItems, Stores = allStores };

                return View("Index",model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while loading stores and items.");
                return View("Error", new ErrorViewModel { Message = "An error occurred while loading stores and items." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddItems()
        {
            try
            {
                var allStores = await db.Stores.ToListAsync();
                var allItems = await db.Items.ToListAsync();

                StoresAndItems model = new() { Items = allItems, Stores = allStores };

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while loading stores and items for adding items.");
                return View("Error", new ErrorViewModel { Message = "An error occurred while loading stores and items for adding items." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddItems(int storeId, Dictionary<int, int> itemQuantity)
        {
            try
            {
                var store = await db.Stores.Include(s => s.StoreItems).FirstOrDefaultAsync(s => s.Id == storeId);
                if (store == null)
                {
                    logger.LogWarning("Store with ID {StoreId} not found.", storeId);
                    return NotFound("Store not found.");
                }

                foreach (var item in itemQuantity)
                {
                    if (item.Value > 0)
                    {
                        var current = store.StoreItems.FirstOrDefault(si => si.ItemId == item.Key);
                        if (current != null)
                        {
                            current.Quantity = item.Value;
                            db.StoreItems.Update(current);
                        }
                        else
                        {
                            StoreItem newStoreItem = new() { StoreId = storeId, ItemId = item.Key, Quantity = item.Value };
                            await db.StoreItems.AddAsync(newStoreItem);
                        }
                    }
                    else if (item.Value == 0)
                    {
                        var current = store.StoreItems.FirstOrDefault(si => si.ItemId == item.Key);
                        if (current != null)
                        {

                            db.StoreItems.Remove(current);
                        }
                    }
                }

                await db.SaveChangesAsync();
                logger.LogInformation("Items successfully updated for store ID {StoreId}.", storeId);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding items to store ID {StoreId}.", storeId);
                return View("Error", new ErrorViewModel { Message = "An error occurred while updating items for the store." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddItemsToStore(int storeId)
        {
            try
            {
                // 1. Efficiently retrieve store and items:
                var store = await db.Stores
                                    .Include(s => s.StoreItems)
                                    .ThenInclude(si => si.Item) // Ensure related items are included
                                    .FirstOrDefaultAsync(s => s.Id == storeId);
                if (store == null)
                {
                    return NotFound(); // Handle store not found scenario
                }

                var allItems = await db.Items.ToListAsync();

                // 2. Optimized logic to get items not in store:
                var itemsNotInStore = allItems.Except(store.StoreItems.Select(si => si.Item)).ToList();

                // 3. Create and return view model:
                StoreAndItems model = new StoreAndItems { Store = store, ItemsNotInStore = itemsNotInStore };
                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding items to store.");
                return View("Error", new ErrorViewModel { Message = "An error occurred while adding items to store." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddItemsToStore(int storeId, Dictionary<int, int>? itemQuantity, Dictionary<int, int>? newItemQuantity)
        {
            try
            {
                // Retrieve the store
                var store = await db.Stores
                                    .Include(s => s.StoreItems)
                                    .FirstOrDefaultAsync(s => s.Id == storeId);
                if (store == null)
                {
                    logger.LogWarning("Store with ID {StoreId} not found.", storeId);
                    return NotFound("Store not found.");
                }

                // Process existing items
                if (itemQuantity != null)
                {
                    foreach (var item in itemQuantity)
                    {
                        var current = store.StoreItems.FirstOrDefault(si => si.ItemId == item.Key);
                        if (item.Value > 0)
                        {
                            if (current != null)
                            {
                                current.Quantity = item.Value;
                                db.StoreItems.Update(current);
                            }
                        }
                        else if (item.Value == 0 && current != null)
                        {
                            db.StoreItems.Remove(current);
                        }
                    }
                }

                // Process new items
                if (newItemQuantity != null)
                {
                    foreach (var item in newItemQuantity)
                    {
                        var current = store.StoreItems.FirstOrDefault(si => si.ItemId == item.Key);
                        if (item.Value > 0)
                        {
                            if (current != null)
                            {
                                current.Quantity = item.Value;
                                db.StoreItems.Update(current);
                            }
                            else
                            {
                                StoreItem newStoreItem = new() { StoreId = storeId, ItemId = item.Key, Quantity = item.Value };
                                await db.StoreItems.AddAsync(newStoreItem);
                            }
                        }
                    }
                }

                // Save changes to the database
                await db.SaveChangesAsync();
                logger.LogInformation("Items successfully updated for store ID {StoreId}.", storeId);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding items to store ID {StoreId}.", storeId);
                return View("Error", new ErrorViewModel { Message = "An error occurred while updating items for the store." });
            }
        }

    }
}
