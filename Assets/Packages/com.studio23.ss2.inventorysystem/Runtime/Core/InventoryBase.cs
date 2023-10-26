using Cysharp.Threading.Tasks;
using Studio23.SS2.InventorySystem.Data;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("com.studio23.ss2.inventorysystem.Tests")]
namespace Studio23.SS2.InventorySystem.Core
{
    public class InventoryBase<T> where T : ItemBase
    {
        [SerializeField] private List<T> _items;
        public string SaveDirectory = "Inventory";
        public readonly string Inventoryname;
        internal string ItemsDirectory => Path.Combine(Application.persistentDataPath, SaveDirectory);

        public InventoryBase(string inventoryName)
        {
            Inventoryname = inventoryName;
            _items = new List<T>();
        }

        public bool AddItem(T item)
        {
            _items.Add(item);
            return true;
        }

        public bool RemoveItem(T item)
        {
            if (!HasItem(item)) return false;
            _items.Remove(item);
            return true;
        }

        public bool HasItem(T item)
        {
            return _items.Contains(item);
        }

        public List<T> GetAll() => _items;


        public async UniTask SaveInventory(bool enableEncryption = true, string encryptionKey = "1234567812345678", string encryptionIV = "1234567876543218")
        {

            List<string> itemNames = new List<string>();

            foreach (var item in _items)
            {
                itemNames.Add(item.Name);
            }

            await SaveSystem.Core.SaveSystem.Instance.SaveData(itemNames, Inventoryname, ItemsDirectory, ".tm", enableEncryption, encryptionKey, encryptionIV);



        }

        public async UniTask LoadInventory(bool enableEncryption = true, string encryptionKey = "1234567812345678", string encryptionIV = "1234567876543218")
        {

            List<string> itemNames = await SaveSystem.Core.SaveSystem.Instance.LoadData<List<string>>(Inventoryname, ItemsDirectory, ".tm", enableEncryption, encryptionKey, encryptionIV);

            _items.Clear();
            foreach (var itemName in itemNames)
            {
                T item = Resources.Load<T>($"{Inventoryname}/{itemName}");
                _items.Add(item);
            }
        }
    }
}