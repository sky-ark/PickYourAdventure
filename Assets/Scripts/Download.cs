using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using TMPro;

public class Download : MonoBehaviour
{
    public TMP_InputField urlInput;

    public void OnDownload()
    {
        string url = urlInput.text;
        StartCoroutine(DownloadFromURL(url));
    }

    IEnumerator DownloadFromURL(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            var json = JSON.Parse(www.downloadHandler.text);
            List<string> storyURLs = new List<string>();
            foreach (var file in json.AsArray)
            {
                string name = file.Value["name"];
                string downloardURL = file.Value["download_url"];
                if (name.EndsWith(".json")) 
                {
                    storyURLs.Add(downloardURL);
                }
            }

            foreach (var storyURL in storyURLs)
            {
                UnityWebRequest fileRequest = UnityWebRequest.Get(storyURL);
                yield return fileRequest.SendWebRequest();
                if (fileRequest.result == UnityWebRequest.Result.Success)
                {
                    string storyJson = fileRequest.downloadHandler.text;
                    string fileName = storyURL.Substring(storyURL.LastIndexOf('/') + 1);
                    System.IO.File.WriteAllText(System.IO.Path.Combine(Application.persistentDataPath, fileName), storyJson);
                    Debug.Log($"Downloaded and saved: {fileName}");
                }
                else
                {
                    Debug.LogError($"Failed to download file from {storyURL}: {fileRequest.error}");
                }
            }
        }
        else
        {
            Debug.LogError($"Failed to download file from {url}: {www.error}");
        }
    }
}
