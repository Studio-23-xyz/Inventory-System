using Studio23.SS2.InventorySystem.Data;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

[assembly: InternalsVisibleTo("com.studio23.ss2.inventorysystem.playmodetests")]
namespace Studio23.SS2.InventorySystem.Core
{
    /// <summary>
    /// Create New inventories by passing new types of objects. This is the base class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InventoryBase<T> where T : ItemBase
    {
        //#TODO refactor item fetching logic into a provider class
        private bool _loadFromAddressable;
        [SerializeField] private List<T> _items;
        public readonly string InventoryName;

        public delegate void InventoryItemEvent(T item);

        public InventoryItemEvent OnItemAdded;
        public InventoryItemEvent OnItemRemoved;

        public InventoryBase(string inventoryName, bool loadFromAddressable = false)
        {
            InventoryName = inventoryName;
            _items = new List<T>();
            _loadFromAddressable = loadFromAddressable;
        }

        public bool AddItem(T item)
        {
            _items.Add(item);
            OnItemAdded?.Invoke(item);
            Debug.Log($"{item},Was Added to the Inventory.", item);
            return true;
        }

        public bool RemoveItem(T item)
        {
            if (!HasItem(item)) return false;
            _items.Remove(item);
            OnItemRemoved?.Invoke(item);
            Debug.Log($"{item},Was Removed from the Inventory.", item);
            return true;
        }

        public bool AddItemUnique(T item)
        {
            if (_items.Contains(item))
                return false;

            AddItem(item);
            return true;
        }

        public bool HasItem(T item)
        {
            return _items.Contains(item);
        }

        public List<T> GetAll() => _items;


        public virtual List<ItemSaveData> GetInventorySaveData()
        {
            List<ItemSaveData> savedItemsData = new List<ItemSaveData>();

            foreach (var item in _items)
            {
                ItemSaveData itemSaveData = CreateSaveDataForItem(item);
                savedItemsData.Add(itemSaveData);
            }

            return savedItemsData;
        }

        protected virtual ItemSaveData CreateSaveDataForItem(T item)
        {
            var itemSaveData = new ItemSaveData(item.name, item.GetSerializedData());
            return itemSaveData;
        }

        public virtual async UniTask LoadInventoryDataAsync(List<ItemSaveData> loadedItemDatas)
        {
            _items.Clear();

            if (loadedItemDatas == null)
            {
                Debug.LogWarning($"No inventory file found for {InventoryName}, Save Inventory First");
                return;
            }

            foreach (var loadedItemData in loadedItemDatas)
            {
                T item = await LoadItemAssetFromItemDataAsync(loadedItemData);
                if (item == null)
                {
                    Debug.LogWarning($"{loadedItemData.SOName} was not found in resources. Was perhaps deleted?");
                    continue;
                }
                item.AssignSerializedData(loadedItemData.ItemData);
                _items.Add(item);
            }
        }
        
        public virtual void LoadInventoryData(List<ItemSaveData> loadedItemDatas)
        {
            _items.Clear();

            if (loadedItemDatas == null)
            {
                Debug.LogWarning($"No inventory file found for {InventoryName}, Save Inventory First");
                return;
            }

            foreach (var loadedItemData in loadedItemDatas)
            {
                T item = LoadItemAssetFromItemData(loadedItemData);
                if (item == null)
                {
                    Debug.LogWarning($"{loadedItemData.SOName} was not found in resources. Was perhaps deleted?");
                    continue;
                }
                item.AssignSerializedData(loadedItemData.ItemData);
                _items.Add(item);
            }
        }

        private T LoadItemAssetFromItemData(ItemSaveData loadedItemData)
        {
            string path = $"Inventory System/{InventoryName}/{loadedItemData.SOName}";
            Debug.LogWarning($"_loadFromAddressable {_loadFromAddressable} " + path);

            if (_loadFromAddressable)
            {
                var handle = Addressables.LoadAssetAsync<T>(
                    path);
                handle.WaitForCompletion();
                Debug.LogWarning($"path equal {path}  ({(path == "Inventory System/Abilities/Ability_WriterVision")}):  {handle.Result}", handle.Result);

                return handle.Result;
            }
            else
            {
                return Resources.Load<T>(path);
            }
        }
        private async UniTask<T> LoadItemAssetFromItemDataAsync(ItemSaveData loadedItemData)
        {
            if (_loadFromAddressable)
            {
                var path = $"Inventory System/{InventoryName}/{loadedItemData.SOName}";
                var handle = Addressables.LoadAssetAsync<T>(
                    path);
                await handle;
                Debug.LogWarning($"path {path} equal ({(path == "Inventory System/Abilities/Ability_WriterVision")}):  {handle.Result}", handle.Result);
                return handle.Result;
            }
            else
            {
                ResourceRequest resourceRequest = Resources.LoadAsync<T>($"Inventory System/{InventoryName}/{loadedItemData.SOName}");
                await resourceRequest;
                return resourceRequest.asset as T;
            }

        }
    }
}