using UnityEngine;
using UnityEditor;
using System.IO;
using Debug = UnityEngine.Debug;
using Studio23.SS2.InventorySystem.Data;

namespace Studio23.SS2.InventorySystem.Editor
{
    public class BackpackEditorWindow
    {
        private string newItemID = "";
        private string newItemName = "";
        private string newItemDescription = "";
        private Sprite newItemIcon = null;
        private string csvPath;


        private bool isProcessing;
        private float progress;
        private string debugLog = "";
        private Vector2 debugLogScrollPosition = Vector2.zero;

        private static string _resourceFolderPath => $"Assets/Resources";
        private static string _inventorySystemFolder => $"Inventory System";

        private string _itemsDirectory = $"{_resourceFolderPath}/{_inventorySystemFolder}/Backpack";
        private string _iconPath = $"{_inventorySystemFolder}/Icons";


        public void ShowWindow()
        {
            DrawGUI();
        }
        private void DrawGUI()
        {
            GUILayout.Label("Create New Item", EditorStyles.whiteLargeLabel);

            newItemID = EditorGUILayout.TextField("ID", newItemID);
            newItemName = EditorGUILayout.TextField("Name", newItemName);
            newItemDescription = EditorGUILayout.TextField("Description", newItemDescription);
            newItemIcon = (Sprite)EditorGUILayout.ObjectField("Icon", newItemIcon, typeof(Sprite), false);

            if (newItemIcon != null && !string.IsNullOrEmpty(newItemName) && !string.IsNullOrEmpty(newItemDescription))
            {
                if (GUILayout.Button("Create Item"))
                {
                    CreateNewItem(newItemID,newItemName, newItemDescription, newItemIcon);
                }
            }

            GUILayout.Space(20);
         
            GUILayout.Label("Generate Items in Bulk", EditorStyles.whiteLargeLabel);

            if (GUILayout.Button("Browse CSV File"))
            {
                csvPath = EditorUtility.OpenFilePanel("Import Bulk Items", "", "csv");
            }

            if (!string.IsNullOrEmpty(csvPath) && GUILayout.Button("Generate Items from CSV"))
            {
                GenerateItemsFromCSV();
            }

            if (!Directory.Exists($"{_resourceFolderPath}/{_iconPath}"))
            {
                if (GUILayout.Button("Create Icon Folder"))
                {
                    Directory.CreateDirectory($"{_resourceFolderPath}/{_iconPath}");
                }
            }

            


            if (GUILayout.Button("Get CSV Template"))
            {
                string templatePath = EditorUtility.SaveFilePanel("Save CSV template", "", "Items Template", "csv");

                using (StreamWriter writer = new StreamWriter(templatePath))
                {
                    writer.WriteLine("ID,Name,Description,IconName");
                }

            }


            if (isProcessing)
            {
                GUILayout.Space(10);
                GUILayout.Label("Progress", EditorStyles.boldLabel);

                Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
                EditorGUI.ProgressBar(rect, progress, $"{(progress * 100):F2}%");

                GUILayout.Space(20);
                GUILayout.Label("Creation Log:", EditorStyles.boldLabel);
                debugLogScrollPosition = GUILayout.BeginScrollView(debugLogScrollPosition, GUILayout.Height(100));
                GUIStyle style = new GUIStyle
                {
                    richText = true
                };
                GUILayout.Label(debugLog,style,GUILayout.ExpandHeight(true));
                GUILayout.EndScrollView();
            }




        }

        private void CreateNewItem(string id,string name, string description, Sprite icon)
        {
            // Create a new Item scriptable object and set its properties.
            Item newItem = ScriptableObject.CreateInstance<Item>();
            newItem.Id = id;
            newItem.Name = name;
            newItem.Name = name;
            newItem.Description = description;
            newItem.Icon = icon;


            string itemName = $"{name}.asset";
            AssetDatabase.CreateAsset(newItem, Path.Combine(_itemsDirectory, itemName));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // Clear the form fields.
            newItemName = "";
            newItemDescription = "";
            newItemIcon = null;

        }

        private void GenerateItemsFromCSV()
        {
            debugLog=string.Empty;
            isProcessing = true;
            // Load the text content of the CSV file.
            string csvText = File.ReadAllText(csvPath);

            // Split the CSV content into rows.
            string[] rows = csvText.Split('\n');

            // Define the target directory path.


            // Ensure the target directory exists; create it if it doesn't.
            if (!Directory.Exists(_itemsDirectory))
            {
                Directory.CreateDirectory(_itemsDirectory);
            }

            // Iterate through CSV rows, skipping the header.
            for (int i = 1; i < rows.Length; i++)
            {
                progress = (float)i / rows.Length;
                string row = rows[i].Trim();
                if (string.IsNullOrEmpty(row))
                    continue;

                string[] columns = row.Split(',');

                if (columns.Length != 4)
                {
                    Debug.LogWarning("Skipping invalid CSV row: " + row);
                    continue;
                }

                string itemId = columns[0];
                string itemName = columns[1];
                string itemDescription = columns[2];
                string iconName = columns[3].Trim();

                // Load the sprite from the Resources folder using the iconName.
                
                Sprite icon = Resources.Load<Sprite>($"{_iconPath}/{iconName}");

                if (icon == null)
                {
                    Debug.LogWarning("Icon not found for item: " + itemName);
                }

                Item newItem = ScriptableObject.CreateInstance<Item>();
                newItem.Id = itemId;
                newItem.Name = itemName;
                newItem.Description = itemDescription;
                newItem.Icon = icon;

         
                string itemPath = Path.Combine(_itemsDirectory, $"{itemName}.asset");

               
                AssetDatabase.CreateAsset(newItem, itemPath);
               
                debugLog += $"{i})<color=green> <b>[Added]</b> </color> {itemPath} \n";

                

            }

            progress = rows.Length / rows.Length;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Items generated from CSV");
        }

    }
}