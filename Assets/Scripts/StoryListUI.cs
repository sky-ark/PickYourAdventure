using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Object = System.Object;

public class StoryListUI : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject storyButtonPrefab;
    public Transform storiesPanel;

    public GameObject StoryPanel;
    public GameObject MainMenuPanel;
    public Button BackToMenuButton;

    void Start()
    {
        BackToMenuButton.onClick.AddListener(() =>
        {
            StoryPanel.SetActive(false);
            MainMenuPanel.SetActive(true);
            this.gameObject.SetActive(false);
        });
        
    }

    private void OnEnable()
    {
        // Delete previous buttons
        foreach (Transform child in storiesPanel)
        {
            Destroy(child.gameObject);
        }
        gameManager.LoadAllStories();
        foreach (var story in gameManager.stories)
        {
            GameObject storyButton = Instantiate(storyButtonPrefab, storiesPanel);
            // Load Story's image if exists
            if (!string.IsNullOrEmpty(story.ImageName))
            {
                Sprite storySprite = ImageLoader.LoadSprite(story.StoryName, story.ImageName);  
                Transform imageTransform = storyButton.transform.Find("StoryImage");
                Image storyImage = imageTransform.GetComponent<Image>();
                storyImage.sprite = storySprite;
                storyImage.preserveAspect = true;
            }
            // Set Story's name on the button
            storyButton.GetComponentInChildren<TMP_Text>().text = story.StoryName;
            storyButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                InventoryManager.Instance.ClearInventory();
                gameManager.ThumbnailUI.Setup(story);
                gameManager.ThumbnailUI.RefreshInventory();
                StoryPanel.SetActive(true);
            });
            Button deleteButton = storyButton.transform.Find("DeleteButton").GetComponent<Button>();
            if (deleteButton != null)
            {
                deleteButton.onClick.AddListener(() =>
                {
                    DeleteStory(story);
                    Destroy(storyButton);
                });
            }
        }
    }

    void DeleteStory(Story story)
    {
        string storyFolder = Path.Combine(Application.persistentDataPath, story.StoryName);
        if (Directory.Exists(storyFolder))
        {
            Directory.Delete(storyFolder, true);
        }
        gameManager.stories.Remove(story);
    }
}