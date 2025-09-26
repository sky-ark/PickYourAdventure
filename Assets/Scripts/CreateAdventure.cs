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
    public GameObject StoryNamePanel;
    public GameObject ThumbnailsPanel;
    public GameObject StorySavedPanel;
    public TMP_InputField StoryNameInputField;
    public Transform ThumbnailsContainer;
    public GameObject ThumbnailInputPrefab;

    [Header("Actions")]
    public Button ValidateNameButton;
    public Button SaveTheStoryButton;
    public Button QuitStoryCreationButton;
    
    private List<ThumbnailInputLine> thumbnailLines = new List<ThumbnailInputLine>();

    void Start()
    {
        ValidateNameButton.onClick.AddListener(OnNameValidationClicked);
        SaveTheStoryButton.onClick.AddListener(OnSaveTheStoryClicked);
        QuitStoryCreationButton.onClick.AddListener(OnQuitStoryCreationClicked);
    }

    public void OnNameValidationClicked()
    {
        StoryNamePanel.SetActive(false);
        OnAddThumbnailClicked();
    }

    public void OnAddThumbnailClicked()
    {
        GameObject obj = Instantiate(ThumbnailInputPrefab, ThumbnailsContainer);
        ThumbnailInputLine til = obj.GetComponent<ThumbnailInputLine>();
        til.Setup(this);
        thumbnailLines.Add(til);
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
        story.Thumbnails = new List<Thumbnail>();

        string storyFolder = Path.Combine(Application.persistentDataPath, story.StoryName);
        if (!Directory.Exists(storyFolder))
            Directory.CreateDirectory(storyFolder);

        // Préparation des thumbnails
        foreach (var til in thumbnailLines)
        {
            Thumbnail thumbnail = new Thumbnail();
            thumbnail.Id = til.IdInput.text;
            thumbnail.Description = til.DescriptionInput.text;
            thumbnail.Choices = til.GetChoices();

            // Gestion image (sauvegarde PNG)
            if (til.ThumbnailImage.sprite != null)
            {
                thumbnail.Image = til.ThumbnailImage.sprite;
                // Sauver l'image
                Texture2D tex = til.ThumbnailImage.sprite.texture;
                Texture2D readableTex = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);
                RenderTexture rt = RenderTexture.GetTemporary(tex.width, tex.height);
                Graphics.Blit(tex, rt);
                RenderTexture.active = rt;
                readableTex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                readableTex.Apply();
                RenderTexture.active = null;
                RenderTexture.ReleaseTemporary(rt);

                byte[] imageBytes = readableTex.EncodeToPNG();
                string imagePath = Path.Combine(storyFolder, thumbnail.Id + ".png");
                File.WriteAllBytes(imagePath, imageBytes);
                DestroyImmediate(readableTex);
            }

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
        ThumbnailsPanel.SetActive(false);
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
}