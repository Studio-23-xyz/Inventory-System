using System;
using UnityEngine;

namespace Studio23.SS2.InventorySystem.Data
{
	[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
	[Serializable]
	public class Item : ItemBase
	{
		public Sprite Icon;
	}
}
