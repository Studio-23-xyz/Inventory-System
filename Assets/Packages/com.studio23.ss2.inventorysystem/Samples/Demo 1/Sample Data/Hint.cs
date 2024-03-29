using Studio23.SS2.InventorySystem.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hint", menuName = "Studio-23/Inventory System/New Hint")]
public class Hint : ItemBase
{
    public string Summary;
    public override void AssignSerializedData(string data)
    {
        Summary = data;
    }

    public override string GetSerializedData()
    {
        return Summary;
    }
}
