using Studio23.SS2.InventorySystem.Data;
using UnityEngine;

namespace Studio23.SS2.InventorySystem.Core
{
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
}