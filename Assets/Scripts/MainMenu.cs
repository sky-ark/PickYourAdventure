using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject StoryPanel;
    public GameObject CreateAdventurePanel;
    public void OnPlayButtonClicked()
    {
        MainMenuPanel.SetActive(false);
        StoryPanel.SetActive(true);
    }

    public void OnCreateAdventureClicked()
    {
        MainMenuPanel.SetActive(false);
        CreateAdventurePanel.SetActive(true);
    }
    public void OnQuitButtonClicked()
    {
        Debug.Log("Quit the game");
        Application.Quit();
    }
}
