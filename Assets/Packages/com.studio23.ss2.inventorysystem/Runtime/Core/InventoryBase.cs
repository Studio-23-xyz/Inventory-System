using Cysharp.Threading.Tasks;
using Studio23.SS2.InventorySystem.Data;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("com.studio23.ss2.inventorysystem.playmodetests")]
namespace Studio23.SS2.InventorySystem.Core
{
    /// <summary>
    /// Create New inventories by passing new types of objects. This is the base class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InventoryBase<T> where T : ItemBase
    {
        [SerializeField] private List<T> _items;
        public string SaveDirectory = "Inventory";
        public readonly string InventoryName;
        internal string ItemsDirectory => Path.Combine(Application.persistentDataPath, SaveDirectory);

        public delegate void InventoryItemEvent(T item);

        public InventoryItemEvent OnItemAdded;
        public InventoryItemEvent OnItemRemoved;

        public InventoryBase(string inventoryName)
        {
            InventoryName = inventoryName;
            _items = new List<T>();
        }

        public bool AddItem(T item)
        {
            _items.Add(item);
            OnItemAdded?.Invoke(item);
            return true;
        }

        public bool RemoveItem(T item)
        {
            if (!HasItem(item)) return false;
            _items.Remove(item);
            OnItemRemoved?.Invoke(item);
            return true;
        }

        public bool AddItemUnique(T item)
        {
            if(_items.Contains(item)) 
                return false;

            _items.Add(item);
            OnItemAdded?.Invoke(item);
            return true;
        }

        public bool HasItem(T item)
        {
            return _items.Contains(item);
        }

        public List<T> GetAll() => _items;

        /// <summary>
        /// Saves inventory data using savesystem library
        /// </summary>
        /// <param name="enableEncryption"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <returns></returns>
        public virtual async UniTask SaveInventory(bool enableEncryption = true, string encryptionKey = "1234567812345678", string encryptionIV = "1234567876543218")
        {
            List<ItemSaveData> savedItemsData = CreateSaveData();

            await SaveSystem.Core.SaveSystem.Instance.SaveData(savedItemsData, InventoryName, ItemsDirectory, ".tm", enableEncryption, encryptionKey, encryptionIV);
        }

        internal List<ItemSaveData> CreateSaveData()
        {
            List<ItemSaveData> savedItemsData = new List<ItemSaveData>();

            foreach (var item in _items)
            {
                var itemSaveData = new ItemSaveData(item.name, item.GetSerializedData());
                savedItemsData.Add(itemSaveData);
            }

            return savedItemsData;
        }


        /// <summary>
        /// Loads the saved inventory data using savesystem library
        /// </summary>
        /// <param name="enableEncryption"></param>
        /// <param name="encryptionKey"></param>
        /// <param name="encryptionIV"></param>
        /// <returns></returns>
        public virtual async UniTask LoadInventory(bool enableEncryption = true, string encryptionKey = "1234567812345678", string encryptionIV = "1234567876543218")
        {
            List<ItemSaveData> loadedItemDatas = await SaveSystem.Core.SaveSystem.Instance.LoadData<List<ItemSaveData>>(
                    InventoryName, ItemsDirectory,
                    ".tm",
                    enableEncryption, encryptionKey, encryptionIV
                );

            _items.Clear();

            if(loadedItemDatas == null )
            {
                Debug.LogWarning($"No inventory file found for {InventoryName}, Save Inventory First");
                return;
            }

            foreach (var loadedItemData in loadedItemDatas)
            {
                T item = Resources.Load<T>($"Inventory System/{InventoryName}/{loadedItemData.SOName}");
                item.AssignSerializedData(loadedItemData.ItemData);
                _items.Add(item);
            }
        }
    }
}