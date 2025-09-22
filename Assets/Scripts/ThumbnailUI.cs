using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThumbnailUI : MonoBehaviour {

    public GameObject ButtonChoicePrefab;
    
    [Header("References")]
    public Image Image;
    public TMP_Text Description;
    public Transform ChoiceContent;

    private Story _story;

    public void Setup(Story story) {
        _story = story;
        Thumbnail firstThumbnail = story.Thumbnails.Find(t => t.Id == story.StartingThumbnailId);
        LoadThumbnail(firstThumbnail);
    }

    private void LoadThumbnail(Thumbnail thumbnail) {
        Image.sprite = thumbnail.Image;
        Description.text = thumbnail.Description;
        ClearChoices();
        foreach (Choice choice in thumbnail.Choices) {
            GameObject instantiate = Instantiate(ButtonChoicePrefab, ChoiceContent);
            instantiate.GetComponentInChildren<TMP_Text>().text = choice.Description;
            instantiate.GetComponent<Button>().onClick.AddListener(() => {
                Thumbnail linkedThumbnail = _story.Thumbnails.Find(t => t.Id == choice.ThumbnailLinkId);
                LoadThumbnail(linkedThumbnail);
            });
        }
    }

    private void ClearChoices() {
        foreach (Transform child in ChoiceContent) {
            Destroy(child.gameObject);
        }
    }
}