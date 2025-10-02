using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StoryProgressionManager : MonoBehaviour
{
    public static void SaveProgress(string storyName, string currentThumbnailId, List<string> inventoryItems)
    {
        StorySaveData saveData = new StorySaveData()
        {
            StoryName = storyName,
            CurrentThumbnailId = currentThumbnailId,
            InventoryItems = inventoryItems
        };
        
        string json = JsonUtility.ToJson(saveData, true);
        string saveDirectory = Path.Combine(Application.persistentDataPath, "SaveFolder");
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }
        string path = Path.Combine(saveDirectory, storyName + "_save.json");
        File.WriteAllText(path, json);
    }

    public static StorySaveData LoadProgress(string storyName)
    {
        string path = Path.Combine(Application.persistentDataPath, "SaveFolder" , storyName);
        Debug.Log($"Loading save file from {path}");
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Save file not found at {path}");
            return null;
        }
            string json = File.ReadAllText(path);
            StorySaveData saveData = JsonUtility.FromJson<StorySaveData>(json);
            return saveData;
    }
}
