using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThumbnailUI : MonoBehaviour {

    public GameObject ButtonChoicePrefab;
    
    [Header("References")]
    public Image Image;
    public TMP_Text Description;
    public GameObject StoryPanel;
    public Transform ChoiceContent;

    private Story _story;

    public void Setup(Story story) {
        _story = story;
        Thumbnail firstThumbnail = story.Thumbnails.Find(t => t.Id == story.StartingThumbnailId);
        LoadThumbnail(firstThumbnail);
    }

    private void LoadThumbnail(Thumbnail thumbnail) {
        if (!string.IsNullOrEmpty(thumbnail.ImageName))
        {
            Sprite sprite = LoadSprite(thumbnail.ImageName);
            Image.sprite = sprite;
            Image.enabled = true;
            Debug.Log($"ThumbnailUI: Affiche image {thumbnail.Id}");
        }
        else
        {
            Image.sprite = null;
            Image.enabled = false;
            Debug.LogWarning($"ThumbnailUI: Image is null for thumbnail {thumbnail.Id}");
        }
        if(Description != null) Description.text = thumbnail.Description;
        ClearChoices();
        foreach (Choice choice in thumbnail.Choices) {
            GameObject instantiate = Instantiate(ButtonChoicePrefab, ChoiceContent);
            instantiate.GetComponentInChildren<TMP_Text>().text = choice.Description;
            instantiate.GetComponent<Button>().onClick.AddListener(() => {
                if (string.IsNullOrEmpty(choice.ThumbnailLinkId))
                {
                    Debug.Log("End of the story");
                    StoryPanel.SetActive(false);
                    return;
                }
                Thumbnail linkedThumbnail = _story.Thumbnails.Find(t => t.Id == choice.ThumbnailLinkId);
                LoadThumbnail(linkedThumbnail);
            });
        }
    }

    public Sprite LoadSprite(string imageName)
    {
        string imagePath = Path.Combine(Application.persistentDataPath, imageName + ".png");
        if (!File.Exists(imagePath)) return null;
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
}