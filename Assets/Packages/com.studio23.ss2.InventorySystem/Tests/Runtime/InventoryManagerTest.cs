using com.studio23.ss2.inventorysystem.data;
using UnityEngine;

public class InventoryManagerTest : MonoBehaviour
{
    public static InventoryManagerTest Instance;

    public InventoryBase<Item> Backpack;


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
