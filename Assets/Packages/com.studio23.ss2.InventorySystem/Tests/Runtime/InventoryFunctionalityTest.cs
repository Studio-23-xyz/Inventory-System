using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.studio23.ss2.inventorysystem.data;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class InventoryFunctionalityTest
{
    private List<Item> AllItems;
    private InventoryManager _inventoryManager;

    [SetUp]
    public void Init()
    {
        GameObject gameObject = new GameObject();
        _inventoryManager = gameObject.AddComponent<InventoryManager>();

        AllItems = Resources.LoadAll<Item>("Items").ToList();
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
    public IEnumerator AddItem()
    {
        //Arrange
        Item item = new RandomListGenerator().GetRandomSublist(AllItems, 1, 1)[0];
        
        //Act
        Assert.IsTrue(InventoryManager.Instance.AddItem(item));

        //Assert
        Assert.IsTrue(InventoryManager.Instance.HasItem(item));

        Debug.Log(item.ItemName);
        yield return null;
    }

    [UnityTest]
    [Repeat(5)]
    public IEnumerator RemoveItem()
    {
        //Arrange
        Item item = new RandomListGenerator().GetRandomSublist(AllItems, 1, 1)[0];

        //Act
        if (InventoryManager.Instance.HasItem(item))
        {
            Assert.IsTrue(InventoryManager.Instance.RemoveItem(item));
        }

        //Assert
        Assert.IsFalse(InventoryManager.Instance.HasItem(item));

        Debug.Log(item.ItemName);
        yield return null;
    }
}
