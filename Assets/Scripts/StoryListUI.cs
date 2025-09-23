using System.Collections;
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
        }
    }
}