using Newtonsoft.Json;
using Studio23.SS2.InventorySystem.Core;
using Studio23.SS2.InventorySystem.Data;
using Studio23.SS2.SaveSystem.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Studio23.SS2.InventorySystem.Samples
{
    public class HintManager : MonoBehaviour, ISaveable
    {
        public InventoryBase<Hint> Hints;

        public List<Hint> HintList;

        void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Hints = new InventoryBase<Hint>("Hints");

            foreach (var hint in HintList)
            {
                Hints.AddItem(hint);
            }

        }

        [ContextMenu("Reset List")]
        public void ReInitList()
        {
            HintList = new List<Hint>();
            foreach (var hint in Hints.GetAll())
            {
                HintList.Add(hint);
            }
        }

        public string GetUniqueID()
        {
            return Hints.InventoryName;
        }

        public void AssignSerializedData(string data)
        {
            List<ItemSaveData> items = JsonConvert.DeserializeObject<List<ItemSaveData>> (data);
            Hints.LoadInventoryDataAsync(items);
        }

        public string GetSerializedData()
        {
            List<ItemSaveData> data = Hints.GetInventorySaveData();
            return JsonConvert.SerializeObject(data);
        }

        

        
    }

}

