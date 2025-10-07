using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceInputLine : MonoBehaviour
{
  public TMP_InputField ChoiceDescriptionInput;
  public TMP_InputField ChoiceThumbnailLinkIdInput;
  public TMP_InputField ChoiceNeededItemInput;
  public TMP_InputField ChoiceGivenItemInput;
  public TMP_InputField ChoiceTakenItemInput;
  public Button RemoveChoiceButton;
  
  private ThumbnailInputLine parent;

  public void Setup(ThumbnailInputLine parentRef)
  {
    parent = parentRef;
    RemoveChoiceButton.onClick.AddListener(() => parent.RemoveChoiceLine(this));
  }
}
