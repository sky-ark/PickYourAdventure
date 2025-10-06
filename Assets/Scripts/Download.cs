using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using TMPro;
using System.IO;

public class DownloadAllStories : MonoBehaviour
{
    public TMP_InputField urlInput;
    public TMP_Text infoText;
    public GameObject MainMenuPanel;

    private void OnEnable()
    {
        infoText.text = "";
        urlInput.text = "https://api.github.com/repos/sky-ark/PickYourAdventureStories/contents/"; // Replace with your GitHub API URL if needed
    }

    public void OnDownload()
    {
        string apiUrl = urlInput.text;
        infoText.text = "Downloading stories...\n";
        StartCoroutine(DownloadAllStoryFolders(apiUrl));
    }

    IEnumerator DownloadAllStoryFolders(string apiUrl)
    {
        UnityWebRequest www = UnityWebRequest.Get(apiUrl);
        yield return www.SendWebRequest();

        List<string> downloadedStories = new List<string>();
        if (www.result != UnityWebRequest.Result.Success)
        {
            infoText.text += $"Error while retrieving the list of stories: {www.error}";
            yield break;
        }

        var rootJson = JSON.Parse(www.downloadHandler.text);
        if (rootJson == null || !rootJson.IsArray)
        {
            infoText.text += "API response is not a JSON array. Please check the provided URL.";
            yield break;
        }

        foreach (var item in rootJson)
        {
            var storyObj = item.Value;
            string type = storyObj["type"];
            if (type == "dir") // If it's a directory (a story)
            {
                string storyName = storyObj["name"];
                string storyApiUrl = storyObj["url"]; // GitHub API URL of the directory

                UnityWebRequest storyRequest = UnityWebRequest.Get(storyApiUrl);
                yield return storyRequest.SendWebRequest();

                if (storyRequest.result != UnityWebRequest.Result.Success)
                {
                    infoText.text += $"Error while retrieving the story {storyName}: {storyRequest.error}\n";
                    continue;
                }

                var storyJson = JSON.Parse(storyRequest.downloadHandler.text);
                if (storyJson == null || !storyJson.IsArray)
                {
                    infoText.text += $"API response for story {storyName} is not a JSON array. Skipping...\n";
                    continue;
                }

                foreach (var file in storyJson)
                {
                    var fileObj = file.Value;
                    string fileName = fileObj["name"];
                    string downloadUrl = fileObj["download_url"];

                    // LOG each file seen
                    infoText.text += $"Detected: {fileName} (url: {(string.IsNullOrEmpty(downloadUrl) ? "NONE" : downloadUrl)})\n";

                    if (string.IsNullOrEmpty(downloadUrl))
                    {
                        infoText.text += $"File ignored (no download_url): {fileName}\n";
                        continue;
                    }

                    infoText.text += $"Attempting to download: {fileName}...\n";
                    UnityWebRequest fileRequest = UnityWebRequest.Get(downloadUrl);
                    yield return fileRequest.SendWebRequest();

                    if (fileRequest.result == UnityWebRequest.Result.Success)
                    {
                        string filePath = Path.Combine(Application.persistentDataPath, storyName, fileName);
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        File.WriteAllBytes(filePath, fileRequest.downloadHandler.data);
                        infoText.text += $"Downloaded and saved: {filePath}\n";
                    }
                    else
                    {
                        infoText.text += $"Error downloading file {fileName} from story {storyName}: {fileRequest.error}\n";
                    }
                }
            }
        }
    }
    
    public void BackToMenu()
    {
        this.gameObject.SetActive(false);
        MainMenuPanel.SetActive(true);
    }
}