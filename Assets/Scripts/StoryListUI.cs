using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StoryListUI : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject storyButtonPrefab;
    public Transform storiesPanel;

    public GameObject StoryPanel;

    void Start()
    {
        gameManager.LoadAllStories();
        foreach (var story in gameManager.stories)
        {
            GameObject storyButton = Instantiate(storyButtonPrefab, storiesPanel);
            storyButton.GetComponentInChildren<TMP_Text>().text = story.StoryName;
            storyButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                gameManager.ThumbnailUI.Setup(story);
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