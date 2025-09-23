using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject StoryPanel;

    public void OnPlayButtonClicked()
    {
        MainMenuPanel.SetActive(false);
        StoryPanel.SetActive(true);
    }

    public void OnQuitButtonClicked()
    {
        Debug.Log("Quit the game");
        Application.Quit();
    }
}
