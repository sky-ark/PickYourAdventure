using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class SoundManager : MonoBehaviour
{
    
    public AudioSource MusicSource;
    public AudioSource SFXSource;
    
    public IEnumerator PlayThumbnailMusic(string storyName, string musicName)
    {
        string musicPath = Path.Combine(Application.persistentDataPath, storyName, musicName);
        if (!File.Exists(musicPath))
        {
            Debug.Log("Music file not found at " + musicPath);
            yield break;
        }
        string url = "file://" + musicPath;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                MusicSource.clip = clip;
                MusicSource.loop = true;
                MusicSource.Play();
            }
        }
    }

    public IEnumerator PlayThumbnailSFX(string storyName, string musicName)
    {
        string SFXPath = Path.Combine(Application.persistentDataPath, storyName, musicName);
        if (!File.Exists(SFXPath))
        {
            Debug.Log("Music file not found at " + SFXPath);
            yield break;
        }
        string url = "file://" + SFXPath;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                SFXSource.clip = clip;
                SFXSource.PlayOneShot(clip);
            }
        }
    }
    public void StopSound()
    {
        MusicSource.Stop();
        SFXSource.Stop();
    }
}
