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
        urlInput.text = "https://api.github.com/repos/sky-ark/PickYourAdventureStories/contents/"; // Remplacez par l'URL de votre API GitHub
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
            infoText.text += $"Error while getting stories's list: {www.error}";
            yield break;
        }

        var rootJson = JSON.Parse(www.downloadHandler.text);
        // Correction : S'assurer que rootJson est bien un array
        if (rootJson == null || !rootJson.IsArray)
        {
            infoText.text += "API response is not a JSON array. Please check the URL provided.";
            yield break;
        }

        foreach (var item in rootJson)
        {
            var storyObj = item.Value;
            string type = storyObj["type"];
            if (type == "dir") // Si c'est un dossier (une story)
            {
                string storyName = storyObj["name"];
                string storyApiUrl = storyObj["url"]; // URL API GitHub du dossier

                UnityWebRequest storyRequest = UnityWebRequest.Get(storyApiUrl);
                yield return storyRequest.SendWebRequest();

                if (storyRequest.result != UnityWebRequest.Result.Success)
                {
                    infoText.text += $"Error while getting the story {storyName} : {storyRequest.error}";
                    continue;
                }

                var storyJson = JSON.Parse(storyRequest.downloadHandler.text);
                if (storyJson == null || !storyJson.IsArray)
                {
                    infoText.text += ($"API response for story {storyName} is not a JSON array. Skipping...");
                    continue;
                }

                foreach (var file in storyJson)
                {
                    var fileObj = file.Value;
                    string fileName = fileObj["name"];
                    string downloadUrl = fileObj["download_url"];
                    if (string.IsNullOrEmpty(downloadUrl)) continue;

                    UnityWebRequest fileRequest = UnityWebRequest.Get(downloadUrl);
                    yield return fileRequest.SendWebRequest();

                    if (fileRequest.result == UnityWebRequest.Result.Success)
                    {
                        string filePath = Path.Combine(Application.persistentDataPath, storyName, fileName);
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                        File.WriteAllBytes(filePath, fileRequest.downloadHandler.data);
                        infoText.text += $"Downloaded and saved: {filePath}";
                    }
                    else
                    {
                        infoText.text +=
                            $"Error downloading file {fileName} from story {storyName}: {fileRequest.error}";
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