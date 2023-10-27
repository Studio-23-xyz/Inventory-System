using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Studio23.SS2.InventorySystem.Core;
using Studio23.SS2.InventorySystem.Data;
using Studio23.SS2.SaveSystem.Core;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class BackpackFunctionalityTest
{
  
    private InventoryBase<Item> Backpack;

    private List<Item> _allItems;
    private List<Item> _randomSubList;

    [OneTimeSetUp]
    public void Init()
    {
        GameObject testObject = new GameObject();
        testObject.AddComponent<SaveSystem>();
        Backpack = new InventoryBase<Item>("Backpack");

        _allItems = Resources.LoadAll<Item>("Inventory System/Backpack").ToList();
        _randomSubList = new RandomListGenerator().GetRandomSublist(_allItems, 1, _allItems.Count).ToList();

        foreach (var item in _randomSubList)
        {
            Backpack.AddItem(item);
        }
    }

   

    [UnityTest, Order(10), Repeat(5)]
    public IEnumerator RemoveItem()
    {
        // Arrange
        Item item = new RandomListGenerator().GetRandomSublist(_allItems, 1, 1)[0];
        Debug.Log(item.Name);

        // Act
        if (Backpack.HasItem(item))
        {
            Assert.IsTrue(Backpack.RemoveItem(item));
        }

        // Assert
        Assert.IsFalse(Backpack.HasItem(item));

        yield return null;
    }

    [UnityTest, Order(2), Repeat(5)]
    public IEnumerator HasItem()
    {
        // Arrange
        Item item = new RandomListGenerator().GetRandomSublist(_randomSubList, 1, 1)[0];
        Debug.Log(item.Name);

        // Assert
        Assert.IsTrue(Backpack.HasItem(item));
        yield return null;
    }

    [UnityTest, Order(3)]
    public IEnumerator SaveInventory_NO_Encryption() => UniTask.ToCoroutine(async () =>
    {

        // Arrange
        await Backpack.SaveInventory(false);

        // Act
        List<string> itemNames = new List<string>();
        foreach (var item in _randomSubList)
        {
            itemNames.Add(item.Name);
        }
        string itemListToBeSaved = JsonConvert.SerializeObject(itemNames, Formatting.Indented);

        string path = Path.Combine(Backpack.ItemsDirectory, $"{Backpack.Inventoryname}.tm");

        string itemListFromFile = await File.ReadAllTextAsync(path);

        // Assert
        Assert.IsTrue(itemListToBeSaved.Equals(itemListFromFile));


    });


    [UnityTest, Order(4)]
    public IEnumerator LoadInventory_NO_Encryption() => UniTask.ToCoroutine(async () =>
    {

        // Arrange
        await Backpack.LoadInventory(false);

        // Act
        List<Item> allItems = Backpack.GetAll();

        // Assert
        CollectionAssert.AreEqual(_randomSubList, allItems);

    });


    [UnityTest, Order(5)]
    public IEnumerator Encrypted_Save_Load() => UniTask.ToCoroutine(async () =>
    {

        // Arrange
        await Backpack.SaveInventory();
        await Backpack.LoadInventory();

        // Act
        List<Item> allItems = Backpack.GetAll();

        // Assert
        CollectionAssert.AreEqual(_randomSubList, allItems);

    });



    [OneTimeTearDown]
    public void TearDown()
    {
        string ItemsDirectory = Backpack.ItemsDirectory;
        Directory.Delete(ItemsDirectory, true);
    }


}
