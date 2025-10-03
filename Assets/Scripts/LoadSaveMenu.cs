using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadSaveMenu : MonoBehaviour
{
    public GameObject LoadButtonPrefab;
    public Transform LoadButtonContainer;
    public ThumbnailUI thumbnailUI;
    public GameObject StoryPanel;
    public GameObject MainMenuPanel;
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
            });
            Button deleteButton = loadButtonObj.transform.Find("DeleteSaveButton").GetComponent<Button>();
            deleteButton.onClick.AddListener((() =>
            {
              File.Delete(file);
              Destroy(loadButtonObj);
            }));
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
            StoryPanel.SetActive(true);
            thumbnailUI.Setup(story);
            Thumbnail startingThumbnail = story.Thumbnails.Find(t => t.Id == save.CurrentThumbnailId);
            if(startingThumbnail == null) Debug.Log("No thumbnail found for " + story.StoryName);
            thumbnailUI.LoadThumbnail(startingThumbnail);
            this.gameObject.SetActive(false);
        }
        else {
            Debug.LogError("Failed to load story progress from " + storySavedName);
        }
    }
    
    public void BackToMainMenu()
    {
        MainMenuPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
