using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public ThumbnailUI ThumbnailUI;
    public Story CurrentStory;

    private void Start() {
        Load();
    }

    [ContextMenu("Save")]
    private void Save() {
        string json = JsonUtility.ToJson(CurrentStory);
        File.WriteAllText(Application.persistentDataPath + "/Story.json", json);
    }
    
    [ContextMenu("Load")]
    private void Load() {
        string json = File.ReadAllText(Application.persistentDataPath + "/Story.json");
        CurrentStory = JsonUtility.FromJson<Story>(json);
        ThumbnailUI.Setup(CurrentStory);
    }
    
    [ContextMenu("Reset")]
    private void Reset() {
        CurrentStory = null;
    }
}