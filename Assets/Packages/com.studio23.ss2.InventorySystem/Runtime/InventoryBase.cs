using System.Collections.Generic;
using System.IO;
using UnityEngine;
using com.studio23.ss2.inventorysystem.data;
using Newtonsoft.Json;

public class InventoryBase<T> where T : ItemBase
{
    [SerializeField] private List<T> _items;
    [SerializeField] private List<Blueprint> _blueprints;
    public readonly string SaveDirectory = "SaveData/Inventory";
    public readonly string ResourcesPath;

    public InventoryBase(string inventoryName)
    {
        ResourcesPath = inventoryName;
        _items = new List<T>();
        _blueprints = new List<Blueprint>();
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

    [ContextMenu("Save")]
    public void SaveInventory()
    {
        string path = Path.Combine(Application.persistentDataPath, SaveDirectory);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path = Path.Combine(path, $"{ResourcesPath}.json");

        List<string> itemNames = new List<string>();

        foreach (var item in _items)
        {
            itemNames.Add(item.Name);
        }
        string json = JsonConvert.SerializeObject(itemNames, Formatting.Indented);

        File.WriteAllText(path, json);
    }

    [ContextMenu("Load")]
    public void LoadInventory()
    {
        string path = Path.Combine(Application.persistentDataPath, SaveDirectory, $"{ResourcesPath}.json");

        if (!File.Exists(path)) return;
        string json = File.ReadAllText(path);
        List<string> itemNames = JsonConvert.DeserializeObject<List<string>>(json);

        _items.Clear();
        foreach (var itemName in itemNames)
        {
            // Load the item with matching ID from resources
            T item = Resources.Load<T>($"{ResourcesPath}/{itemName}");
            _items.Add(item);
        }
    }
}