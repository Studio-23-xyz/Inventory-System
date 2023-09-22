using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using com.studio23.ss2.inventorysystem.data;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class InventoryFunctionalityTest
{
    private List<Item> AllItems;
    private List<Item> RandomSubList;

    private InventoryManager _inventoryManager;

    private readonly string _saveDirectory = "SaveData";
    private readonly string _resourcesPath = "Items";

    [SetUp]
    public void Init()
    {
        GameObject gameObject = new GameObject();
        _inventoryManager = gameObject.AddComponent<InventoryManager>();

        AllItems = Resources.LoadAll<Item>("Items").ToList();
        RandomSubList = new RandomListGenerator().GetRandomSublist(AllItems, 1, AllItems.Count).ToList();

        foreach (var item in RandomSubList)
        {
            InventoryManager.Instance.AddItem(item);
        }
    }

    [UnityTest]
    public IEnumerator InstanceCheck()
    {
        yield return new WaitForFixedUpdate();
        Assert.NotNull(_inventoryManager);
    }

    //public static IEnumerable<TestCaseData> GetTestCases() => Enumerable.Repeat(new TestCaseData(), _repetition);
    //[TestCaseSource(nameof(GetTestCases))]

    [UnityTest]
    [Repeat(5)]
    public IEnumerator HasItem()
    {
        // Arrange
        Item item = new RandomListGenerator().GetRandomSublist(RandomSubList, 1, 1)[0];
        Debug.Log(item.ItemName);

        // Assert
        Assert.IsTrue(InventoryManager.Instance.HasItem(item));
        yield return null;
    }

    [UnityTest]
    [Repeat(5)]
    public IEnumerator RemoveItem()
    {
        // Arrange
        Item item = new RandomListGenerator().GetRandomSublist(AllItems, 1, 1)[0];
        Debug.Log(item.ItemName);

        // Act
        if (InventoryManager.Instance.HasItem(item))
        {
            Assert.IsTrue(InventoryManager.Instance.RemoveItem(item));
        }

        // Assert
        Assert.IsFalse(InventoryManager.Instance.HasItem(item));

        yield return null;
    }

    [UnityTest]
    [Repeat(5)]
    public IEnumerator SaveInventory()
    {
        // Arrange
        InventoryManager.Instance.SaveInventory();

        // Act
        //string itemListToBeSaved = JsonConvert.SerializeObject(RandomSubList, Formatting.Indented);

        string path = Path.Combine(Application.persistentDataPath, _saveDirectory, "inventory.json");
        if (!File.Exists(path)) yield return null;
        string itemListFromFile = File.ReadAllText(path);

        // Assert

    }
}
