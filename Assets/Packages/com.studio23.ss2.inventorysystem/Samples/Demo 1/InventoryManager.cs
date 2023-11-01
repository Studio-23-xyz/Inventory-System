using Studio23.SS2.InventorySystem.Core;
using Studio23.SS2.InventorySystem.Data;
using UnityEngine;


    public class InventoryManager : MonoBehaviour
    {


        public InventoryBase<Item> Backpack;
        public InventoryBase<Hint> Hints;

        void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Backpack = new InventoryBase<Item>("Backpack");
            Hints = new InventoryBase<Hint>("Hints");
        }
    }
