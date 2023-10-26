using UnityEngine;
using UnityEditor;
using System.IO;
using Studio23.SS2.InventorySystem.Data;

public class InventoryEditorWindow : EditorWindow
{
    private string newItemName = "";
    private string newItemDescription = "";
    private Sprite newItemIcon = null;
    private string csvPath;

    [MenuItem("Studio-23/Inventory System/Backpack")]
    public static void ShowWindow()
    {
        GetWindow(typeof(InventoryEditorWindow), false, "Inventory Editor");
    }
    private void OnGUI()
    {
        GUILayout.Label("Create New Item", EditorStyles.boldLabel);

        newItemName = EditorGUILayout.TextField("Name", newItemName);
        newItemDescription = EditorGUILayout.TextField("Description", newItemDescription);
        newItemIcon = (Sprite)EditorGUILayout.ObjectField("Icon", newItemIcon, typeof(Sprite), false);

        if(newItemIcon != null && !string.IsNullOrEmpty(newItemName) && !string.IsNullOrEmpty(newItemDescription)){
            if (GUILayout.Button("Create Item"))
            {
                CreateNewItem(newItemName, newItemDescription, newItemIcon);
            }
        }


        GUILayout.Label("Generate Items in Bulk", EditorStyles.boldLabel);

        if (GUILayout.Button("Browse CSV File"))
        {
           csvPath= EditorUtility.OpenFilePanel("Import Bulk Items", "", "*.csv");
        }

        if (!string.IsNullOrEmpty(csvPath) && GUILayout.Button("Generate Items from CSV") )
        {
            GenerateItemsFromCSV();
        }

        if (GUILayout.Button("Get CSV Template"))
        {
            string templatePath = EditorUtility.SaveFilePanel("Save CSV template","","Items Template","*.csv");
        }
    }

    private void CreateNewItem(string name, string description, Sprite icon)
    {
        // Create a new Item scriptable object and set its properties.
        Item newItem = ScriptableObject.CreateInstance<Item>();
        newItem.Name = name;
        newItem.Description = description;
        newItem.Icon = icon;

        // Save the item to a directory (Resources > Inventory System > Backpack).
        string itemPath = "Assets/Resources/Inventory System/Backpack";
        string itemName = $"{name}.asset";
        AssetDatabase.CreateAsset(newItem, Path.Combine(itemPath, itemName));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Clear the form fields.
        newItemName = "";
        newItemDescription = "";
        newItemIcon = null;

        Debug.Log("Item created and saved.");
    }

    private void GenerateItemsFromCSV()
    {
        // Implement CSV parsing and item creation from the CSV file.
        // You'll need to handle reading the CSV and creating items here.
        // This code is a placeholder.

        //Debug.Log("Items generated from CSV: " + csvFile.name);
    }

    private void GetCSVTemplate()
    {
        // Implement creating and providing a CSV template.
        // You can create a sample CSV file with headers for name, description, etc.

        Debug.Log("CSV template retrieved.");
    }
}
