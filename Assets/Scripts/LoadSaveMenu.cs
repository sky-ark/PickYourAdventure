using System;
using System.IO;
using UnityEngine;

public class LoadSaveMenu : MonoBehaviour
{
    public GameObject LoadButtonPrefab;
    public Transform LoadButtonContainer;
    public ThumbnailUI thumbnailUI;

    private void OnEnable()
    {
        PopulateSaveButtons();
    }

    private void PopulateSaveButtons()
    {
        //Clear previous buttons
        foreach (Transform child in LoadButtonContainer)
        {
            Destroy(child.gameObject);
        }
        
        string saveFolder = Path.Combine(Application.persistentDataPath, "SaveFolder");
        string[] files = Directory.GetFiles(saveFolder, "*_save.json");

        foreach (var file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string storyName = fileName.Replace("_save", "");
            GameObject loadButtonObj = Instantiate(LoadButtonPrefab, LoadButtonContainer);
            loadButtonObj.GetComponentInChildren<TMPro.TMP_Text>().text = storyName;
            loadButtonObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            {
                LoadStory(file);
                this.gameObject.SetActive(false);
            });
        }
    }

    private void LoadStory(string storySavedName)
    {
        var save = StoryProgressionManager.LoadProgress(storySavedName);
        if (save != null)
        {
            InventoryManager.Instance.ClearInventory();
            InventoryManager.Instance.AddItem(save.InventoryItems);
            string storyFolder = Path.Combine(Application.persistentDataPath, save.StoryName);
            string storyPath = Path.Combine(storyFolder, save.StoryName + ".json");
            string json = File.ReadAllText(storyPath);
            Story story = JsonUtility.FromJson<Story>(json);
            thumbnailUI.Setup(story);
            Thumbnail startingThumbnail = story.Thumbnails.Find(t => t.Id == save.CurrentThumbnailId);
            thumbnailUI.LoadThumbnail(startingThumbnail);
        }
    }
    
}
