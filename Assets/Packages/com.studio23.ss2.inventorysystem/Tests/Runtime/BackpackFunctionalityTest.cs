using NUnit.Framework;
using Studio23.SS2.InventorySystem.Core;
using Studio23.SS2.InventorySystem.Data;
using System.Collections;
using System.Collections.Generic;

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
        Backpack = new InventoryBase<Item>("Backpack");

        _allItems = Resources.LoadAll<Item>("Inventory System/Backpack").ToList();
        _randomSubList = new RandomListGenerator().GetRandomSublist(_allItems, 1, _allItems.Count).ToList();

        foreach (var item in _randomSubList)
        {
            Backpack.AddItem(item);
        }
    }

  

    [UnityTest, Order(1), Repeat(3)]
    public IEnumerator HasItem()
    {
        // Arrange
        Item item = new RandomListGenerator().GetRandomSublist(_randomSubList, 1, 1)[0];
        Debug.Log(item.Name);

        // Assert
        Assert.IsTrue(Backpack.HasItem(item));
        yield return null;
    }



   

    [UnityTest, Order(10), Repeat(3)]
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




}
