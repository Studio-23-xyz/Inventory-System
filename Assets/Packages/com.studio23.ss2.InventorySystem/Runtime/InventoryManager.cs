using com.studio23.ss2.inventorysystem.data;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public InventoryBase<Item> Backpack;
    // public InventoryBase<Item> Journal;
    // public InventoryBase<Item> Hints;

    void Awake()
    {
        Instance = this;
        Initialize();
    }

    private void Initialize()
    {
        Backpack = new InventoryBase<Item>("Backpack");
    }
}
