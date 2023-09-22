using System.Collections.Generic;
using UnityEngine;
using System.IO;
using com.studio23.ss2.inventorysystem.data;
using Newtonsoft.Json;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private List<Item> _items;
    private readonly string _saveDirectory = "SaveData"; 
    private readonly string _resourcesPath = "Items";

    void Awake()
    {
        Instance = this;
        _items = new List<Item>();
    }

    public bool AddItem(Item item)
    {
        _items.Add(item);
        return true;
    }

    public bool RemoveItem(Item item)
    {
        if (!HasItem(item)) return false;
        _items.Remove(item);
        return true;
    }

    public bool HasItem(Item item)
    {
        return _items.Contains(item);
    }

    [ContextMenu("Save")]
    public void SaveInventory()
    {
        string path = Path.Combine(Application.persistentDataPath, _saveDirectory);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path = Path.Combine(path, "inventory.json");

        List<string> itemNames = new List<string>();

        foreach (var item in _items)
        {
            itemNames.Add(item.ItemName);
        }
        string json = JsonConvert.SerializeObject(itemNames, Formatting.Indented);

        File.WriteAllText(path, json);
    }

    [ContextMenu("Load")]
    public void LoadInventory()
    {
        string path = Path.Combine(Application.persistentDataPath, _saveDirectory, "inventory.json");

        if (!File.Exists(path)) return;
        string json = File.ReadAllText(path);
        List<string> itemNames = JsonConvert.DeserializeObject<List<string>>(json);

        _items.Clear();
        foreach (var itemName in itemNames)
        {
            // Load the item with matching ID from resources
            Item item = Resources.Load<Item>($"{_resourcesPath}/{itemName}");

            _items.Add(item);
        }
    }
}