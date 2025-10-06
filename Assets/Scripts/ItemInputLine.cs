using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInputLine : MonoBehaviour
{
    public TMP_InputField IDInput;
    public TMP_InputField NameInput;
    public TMP_InputField DescriptionInput;
    public Image ItemImage;
    public Button SelectImageButton;
    public Button RemoveButton;
    public Button AddButton;

    private CreateAdventure parent;
    
    public void Setup(CreateAdventure parentRef)
    {
        parent = parentRef;
        AddButton.onClick.AddListener(parent.AddItem);
        RemoveButton.onClick.AddListener(() => parent.RemoveItemLine(this));
        SelectImageButton.onClick.AddListener(SelectImage);
    }

    public void SelectImage()
    {
        parent.SelectImage((sprite) => {
            ItemImage.sprite = sprite;
        });
    }
}