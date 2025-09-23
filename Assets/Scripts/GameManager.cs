using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]  public List<Story> stories;
    public ThumbnailUI ThumbnailUI;
   // public Story[] Stories;
    //public Story CurrentStory;
    

    private void Start() {
        //Load();
    }

    // [ContextMenu("Save Current Story")]
    // private void Save() {
    //     string json = JsonUtility.ToJson(CurrentStory);
    //     string fileName = CurrentStory.StoryName + ".json";
    //     File.WriteAllText(Path.Combine(Application.persistentDataPath , fileName), json);
    // }
    
    [ContextMenu("Save All Stories")]
    public void SaveAllStories(){
        foreach (var story in stories) 
        {
        string storyFolder = Path.Combine(Application.persistentDataPath, story.StoryName); 
        if (!Directory.Exists(storyFolder)) 
        {
            Directory.CreateDirectory(storyFolder);
        }
        string json = JsonUtility.ToJson(story);
        string fileName = story.StoryName + ".json";
        string filePath = Path.Combine(storyFolder, fileName);
        File.WriteAllText(Path.Combine(filePath), json);
        foreach (var thumbnail in  story.Thumbnails)
        {
            if (thumbnail.Image != null)
            {
                Texture2D source = thumbnail.Image.texture;
                // Create a readable copy of the texture if it's not readable
                Texture2D readableTex = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
                RenderTexture rt = RenderTexture.GetTemporary(source.width, source.height);
                Graphics.Blit(source, rt);
                RenderTexture.active = rt;
                readableTex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                readableTex.Apply();
                RenderTexture.active = null;
                RenderTexture.ReleaseTemporary(rt);

                byte[] imageBytes = readableTex.EncodeToPNG();
                string imagePath = Path.Combine(storyFolder, thumbnail.Id + ".png");
                File.WriteAllBytes(imagePath, imageBytes);
                Object.DestroyImmediate(readableTex);
            }
        }
        }
    }
    
    // [ContextMenu("Load")]
    // private void Load() {
    //     string json = File.ReadAllText(Application.persistentDataPath + "/Story.json");
    //     CurrentStory = JsonUtility.FromJson<Story>(json);
    //     ThumbnailUI.Setup(CurrentStory);
    // }
    
    // [ContextMenu("Reset")]
    // private void Reset() {
    //     CurrentStory = null;
    // }

   [ContextMenu("Load All Stories")]
    public void LoadAllStories()
    {
        stories.Clear();
        string roothPath = Application.persistentDataPath;
        string[] storyFolders = Directory.GetDirectories(roothPath);
        foreach (var storyFolder in storyFolders)
        {
            string folderName = Path.GetFileName(storyFolder);
            string jsonPath = Path.Combine(storyFolder, folderName + ".json");
            if (File.Exists(jsonPath))
            {
                string json = File.ReadAllText(jsonPath);
                Story story = JsonUtility.FromJson<Story>(json);
                foreach (var thumbnail in story.Thumbnails)
                {
                    string imagePath = Path.Combine(storyFolder, thumbnail.Id + ".png");
                    if (File.Exists(imagePath))
                    {
                        byte[] imageBytes = File.ReadAllBytes(imagePath);
                        Texture2D texture = new Texture2D(2, 2);
                        texture.LoadImage(imageBytes);
                        Rect rect = new Rect(0, 0, texture.width, texture.height);
                        thumbnail.Image = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
                    }
                    else
                    {
                        Debug.LogWarning($"Image file not found: {imagePath}");
                    }
                }
                stories.Add(story);
                Debug.Log($"Loaded story: {story.StoryName} from {jsonPath}");
            }
            else
            {
                Debug.LogWarning($"No JSON file found for story in folder: {storyFolder}");
            }
        }
        
    }
}