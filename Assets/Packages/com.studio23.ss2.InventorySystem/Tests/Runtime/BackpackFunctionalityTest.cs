using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using com.studio23.ss2.inventorysystem.data;
using NUnit.Framework;
using Newtonsoft.Json;

[TestFixture]
public class BackpackFunctionalityTest
{
    private InventoryManager _inventoryManager;

    private List<Item> _allItems;
    private List<Item> _randomSubList;

    [OneTimeSetUp]
    public void Init()
    {
        GameObject gameObject = new GameObject();
        _inventoryManager = gameObject.AddComponent<InventoryManager>();

        _allItems = Resources.LoadAll<Item>("Backpack").ToList();
        _randomSubList = new RandomListGenerator().GetRandomSublist(_allItems, 1, _allItems.Count).ToList();

        foreach (var item in _randomSubList)
        {
            InventoryManager.Instance.Backpack.AddItem(item);
        }
    }

    [UnityTest, Order(1)]
    public IEnumerator InstanceCheck()
    {
        yield return new WaitForFixedUpdate();
        Assert.NotNull(_inventoryManager);
    }

    [UnityTest, Order(5), Repeat(5)]
    public IEnumerator RemoveItem()
    {
        // Arrange
        Item item = new RandomListGenerator().GetRandomSublist(_allItems, 1, 1)[0];
        Debug.Log(item.Name);

        // Act
        if (InventoryManager.Instance.Backpack.HasItem(item))
        {
            Assert.IsTrue(InventoryManager.Instance.Backpack.RemoveItem(item));
        }

        // Assert
        Assert.IsFalse(InventoryManager.Instance.Backpack.HasItem(item));

        yield return null;
    }

    [UnityTest, Order(2), Repeat(5)]
    public IEnumerator HasItem()
    {
        // Arrange
        Item item = new RandomListGenerator().GetRandomSublist(_randomSubList, 1, 1)[0];
        Debug.Log(item.Name);

        // Assert
        Assert.IsTrue(InventoryManager.Instance.Backpack.HasItem(item));
        yield return null;
    }

    [UnityTest, Order(3)]
    public IEnumerator SaveInventory()
    {
        // Arrange
        InventoryManager.Instance.Backpack.SaveInventory();

        // Act
        List<string> itemNames = new List<string>();
        foreach (var item in _randomSubList)
        {
            itemNames.Add(item.Name);
        }
        string itemListToBeSaved = JsonConvert.SerializeObject(itemNames, Formatting.Indented);

        string path = Path.Combine(Application.persistentDataPath, InventoryManager.Instance.Backpack.SaveDirectory, $"{InventoryManager.Instance.Backpack.ResourcesPath}.json");
        if (!File.Exists(path)) yield return null;
        string itemListFromFile = File.ReadAllText(path);
        
        // Assert
        Assert.IsTrue(itemListToBeSaved.Equals(itemListFromFile));
    }

    [UnityTest, Order(4)]
    public IEnumerator LoadInventory()
    {
        // Arrange
        InventoryManager.Instance.Backpack.LoadInventory();

        // Act
        List<Item> allItems = InventoryManager.Instance.Backpack.GetAll();

        // Assert
        CollectionAssert.AreEqual(_randomSubList, allItems);
        yield return null;
    }
}
