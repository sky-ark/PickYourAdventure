using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class CreateAdventure : MonoBehaviour
{
    [Header("Panels & Containers")]
    public GameObject CreateAdventurePanel;
    public GameObject MainMenuPanel;
    public GameObject StorySavedPanel;
    public Transform ThumbnailsContainer;
    public GameObject ItemInputPrefab;
    public GameObject ThumbnailInputPrefab;

    [Header("Story Info")]
    public TMP_InputField StoryNameInputField;
    public Image StoryImage;

    [Header("Actions")]
    public Button SelectStoryImageButton;
    public Button AddItemButton;
    public Button AddThumbnailButton;
    public Button SaveTheStoryButton;
    public Button QuitStoryCreationButton;

    private List<ThumbnailInputLine> thumbnailLines = new List<ThumbnailInputLine>();
    private List<ItemInputLine> itemLines = new List<ItemInputLine>();

    void Start()
    {
        SaveTheStoryButton.onClick.AddListener(OnSaveTheStoryClicked);
        QuitStoryCreationButton.onClick.AddListener(OnQuitStoryCreationClicked);
        SelectStoryImageButton.onClick.AddListener(OnSelectStoryImage);
        AddThumbnailButton.onClick.AddListener(OnAddThumbnailClicked);
        AddItemButton.onClick.AddListener(AddItem);
    }
    
    public void OnSelectStoryImage()
    {
        SelectImage((sprite) =>
        {
            if (sprite != null)
            {
                StoryImage.sprite = sprite;
                StoryImage.preserveAspect = true;
            }
        });
    }

    public void OnAddThumbnailClicked()
    {
        GameObject obj = Instantiate(ThumbnailInputPrefab, ThumbnailsContainer);
        ThumbnailInputLine til = obj.GetComponent<ThumbnailInputLine>();
        til.Setup(this);
        thumbnailLines.Add(til);
    }

    public void AddItem()
    {
        GameObject obj = Instantiate(ItemInputPrefab, ThumbnailsContainer);
        ItemInputLine iil = obj.GetComponent<ItemInputLine>();
        iil.Setup(this);
        itemLines.Add(iil);
    }
    public void RemoveItemLine(ItemInputLine line)
    {
        itemLines.Remove(line);
        Destroy(line.gameObject);
    }

    public void RemoveThumbnailLine(ThumbnailInputLine line)
    {
        thumbnailLines.Remove(line);
        Destroy(line.gameObject);
    }

    public void OnSaveTheStoryClicked()
    {
        Story story = new Story();
        story.StoryName = StoryNameInputField.text;

        string storyFolder = Path.Combine(Application.persistentDataPath, story.StoryName);
        if (!Directory.Exists(storyFolder))
            Directory.CreateDirectory(storyFolder);

        // Sauver l'image principale
        story.ImageName = SaveSpriteAsPng(StoryImage.sprite, storyFolder, story.StoryName);

        // Items preparation
        story.Items = new List<Item>();
        foreach (var iil in itemLines)
        {
            Item item = new Item();
            item.Id = iil.IDInput.text;
            item.ItemName = iil.NameInput.text;
            item.Description = iil.DescriptionInput.text;

            // Gestion de l'image de l'item
            item.IconName = SaveSpriteAsPng(iil.ItemImage.sprite, storyFolder, item.Id);

            story.Items.Add(item);
        }

        story.Thumbnails = new List<Thumbnail>();

        // Thumbnails preparation
        foreach (var til in thumbnailLines)
        {
            Thumbnail thumbnail = new Thumbnail();
            thumbnail.Id = til.IdInput.text;
            thumbnail.Description = til.DescriptionInput.text;
            thumbnail.Choices = til.GetChoices();

            // Gestion image (sauvegarde PNG)
            thumbnail.ImageName = SaveSpriteAsPng(til.ThumbnailImage.sprite, storyFolder, thumbnail.Id);

            story.Thumbnails.Add(thumbnail);
        }
        if (story.Thumbnails.Count > 0)
            story.StartingThumbnailId = story.Thumbnails[0].Id;

        // Sauvegarde JSON
        string json = JsonUtility.ToJson(story, true);
        string fileName = story.StoryName + ".json";
        string jsonPath = Path.Combine(storyFolder, fileName);
        File.WriteAllText(jsonPath, json);

        Debug.Log($"Story '{story.StoryName}' saved with {story.Thumbnails.Count} thumbnails.");
        StorySavedPanel.SetActive(true);
    }

    public void OnQuitStoryCreationClicked()
    {
        StorySavedPanel.SetActive(false);
        foreach (var line in thumbnailLines)
        {
            Destroy(line.gameObject);
        }
        thumbnailLines.Clear();
        StoryNameInputField.text = "";
        MainMenuPanel.SetActive(true);
        CreateAdventurePanel.SetActive(false);
    }

    private string SaveSpriteAsPng(Sprite sprite, string folderPath, string fileNameWithoutExtension)
    {
        if (sprite == null) return null;

        Texture2D tex = sprite.texture;
        Texture2D readableTex = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);
        RenderTexture rt = RenderTexture.GetTemporary(tex.width, tex.height);
        Graphics.Blit(tex, rt);
        RenderTexture.active = rt;
        readableTex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readableTex.Apply();
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        byte[] imageBytes = readableTex.EncodeToPNG();
        string imageFileName = fileNameWithoutExtension + ".png";
        string imagePath = Path.Combine(folderPath, imageFileName);
        File.WriteAllBytes(imagePath, imageBytes);
        DestroyImmediate(readableTex);

        return imageFileName;
    }
    public void SelectImage(System.Action<Sprite> onImageSelected)
    {
#if UNITY_ANDROID || UNITY_IOS
        NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, 512);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                Vector2 pivot = new Vector2(0.5f, 0.5f);
                Sprite sprite = Sprite.Create(texture, rect, pivot);
                onImageSelected?.Invoke(sprite);
            }
        }, "Select a PNG image", "image/png");
#endif
    }
    
    
}