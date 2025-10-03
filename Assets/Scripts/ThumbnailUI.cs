using System;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ThumbnailUI : MonoBehaviour {

    public GameObject ButtonChoicePrefab;
    
    [Header("References")]
    public Image Image;
    public TMP_Text Description;
    public GameObject StoryPanel;
    public Transform ChoiceContent;
    public Button SaveButton;
    public GameObject MainMenuPanel;

    private Story _story;
    private String _currentThumbnailId;

    private void Start()
    {
        SaveButton.onClick.AddListener(() =>
        {
            if (_story == null)
            {
                Debug.LogError("No story loaded to save progress.");
                return;
            }
            StoryProgressionManager.SaveProgress(_story.StoryName, _currentThumbnailId, InventoryManager.Instance._items.ToList());
            Debug.Log("Game progress saved.");
        });
    }

    public void Setup(Story story) {
        _story = story;
        Thumbnail firstThumbnail = story.Thumbnails.Find(t => t.Id == story.StartingThumbnailId);
        LoadThumbnail(firstThumbnail);
    }

    public void LoadThumbnail(Thumbnail thumbnail)
    {
        _currentThumbnailId = thumbnail.Id;
        if (!string.IsNullOrEmpty(thumbnail.ImageName))
        {
            Sprite sprite = LoadSprite(thumbnail.ImageName);
            Image.sprite = sprite;
            Image.enabled = true;
            Image.preserveAspect = true;
        }
        else
        {
            Image.sprite = null;
            Image.enabled = false;
            Debug.LogWarning($"ThumbnailUI: Image is null for thumbnail {thumbnail.Id}");
        }
        if(Description != null) Description.text = thumbnail.Description;
        ClearChoices();
        foreach (Choice choice in thumbnail.Choices)
        {
            GameObject instantiate = Instantiate(ButtonChoicePrefab, ChoiceContent);
            instantiate.GetComponentInChildren<TMP_Text>().text = choice.Description;
            bool accessible = InventoryManager.Instance.HasItem(choice.NeededItemsId);
            Button btn = instantiate.GetComponent<Button>();
            btn.interactable = accessible;

            if (accessible)
            {


                btn.onClick.AddListener(() =>
                {
                    if (string.IsNullOrEmpty(choice.ThumbnailLinkId))
                    {
                        Debug.Log("End of the story");
                        MainMenuPanel.SetActive(true);
                        StoryPanel.SetActive(false);
                        return;
                    }

                    Thumbnail linkedThumbnail = _story.Thumbnails.Find(t => t.Id == choice.ThumbnailLinkId);
                    InventoryManager.Instance.AddItem(choice.GivenItemsId);
                    InventoryManager.Instance.RemoveItem(choice.TakenItemsId);
                    LoadThumbnail(linkedThumbnail);
                });
            }
            else
            {
                TMP_Text btnText = instantiate.GetComponentInChildren<TMP_Text>();
                if (btnText != null)
                {
                    btnText.text += " (Locked)";
                    btnText.color = Color.gray;
                }
                
            }
        }
    }

    public Sprite LoadSprite(string imageName)
    {
        string imagePath = Path.Combine(Application.persistentDataPath, _story.StoryName, imageName + ".png");
        if (!File.Exists(imagePath))
        {
            Debug.Log("Image file not found at " + imagePath);
            return null;
        }
        byte[] imageBytes = File.ReadAllBytes(imagePath);
        Texture2D texture = new Texture2D(2, 2);
        if(!texture.LoadImage(imageBytes)) return null;
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        return Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
    }
    
    private void ClearChoices() {
        foreach (Transform child in ChoiceContent) {
            Destroy(child.gameObject);
        }
    }

    public void BackToMenu()
    {
        MainMenuPanel.SetActive(true);
        this.gameObject.SetActive(false);
        
    }
}