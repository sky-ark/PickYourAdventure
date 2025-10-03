using System.IO;
using UnityEngine;

public static class ImageLoader
{
    public static Sprite LoadSprite(string storyName,string imageName)
    {
        if(string.IsNullOrEmpty(storyName) || string.IsNullOrEmpty(imageName)) return null;
        string imagePath = Path.Combine(Application.persistentDataPath, storyName, imageName);
        if (!File.Exists(imagePath))
        {
            Debug.Log("Image file not found at " + imagePath);
            return null;
        }
        byte[] imageBytes = File.ReadAllBytes(imagePath);
        Texture2D tex = new Texture2D(2, 2);
        if(!tex.LoadImage(imageBytes)) return null;
        Rect rect = new Rect(0, 0, tex.width, tex.height);
        Debug.Log("Loaded image from " + imagePath);
        return Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f));
    }
}