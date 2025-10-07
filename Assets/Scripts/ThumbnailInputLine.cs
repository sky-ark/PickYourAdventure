using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThumbnailInputLine : MonoBehaviour
{
    public TMP_InputField IdInput;
    public TMP_InputField DescriptionInput;
    public Image ThumbnailImage;
    public Button SelectImageButton;
    public Button RemoveButton;
    public Transform ChoicesContainer;
    public GameObject ChoiceInputPrefab;
    public Button AddThumbnailButton;
    public Button AddChoiceButton;

    private List<ChoiceInputLine> choiceLines = new List<ChoiceInputLine>();
    private CreateAdventure parent;

    public void Setup(CreateAdventure parentRef)
    {
        parent = parentRef;
        RemoveButton.onClick.AddListener(() => parent.RemoveThumbnailLine(this));
        AddThumbnailButton.onClick.AddListener(parent.OnAddThumbnailClicked);
        AddChoiceButton.onClick.AddListener(AddChoice);
        SelectImageButton.onClick.AddListener(SelectImage);
    }

    public void AddChoice()
    {
       GameObject obj = Instantiate(ChoiceInputPrefab, ChoicesContainer);
       ChoiceInputLine cil = obj.GetComponent<ChoiceInputLine>();
       cil.Setup(this);
       choiceLines.Add(cil);
    }

    public void RemoveChoiceLine(ChoiceInputLine line)
    {
        choiceLines.Remove(line);
        Destroy(line.gameObject);
    }
    
    public List<Choice> GetChoices()
    {
        List<Choice> result = new List<Choice>();
        foreach (var line in choiceLines)
        {
            Choice choice = new Choice
            {
                Description = line.ChoiceDescriptionInput.text,
                ThumbnailLinkId = line.ChoiceThumbnailLinkIdInput.text,
                NeededItemsId = ParseInputToList(line.ChoiceNeededItemInput.text),
                GivenItemsId = ParseInputToList(line.ChoiceGivenItemInput.text),
                TakenItemsId = ParseInputToList(line.ChoiceTakenItemInput.text),
            };
            result.Add(choice);
        }

        return result;
    }
    
    private List<string> ParseInputToList(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new List<string>();
        // Seperate by comma and trim spaces
        return new List<string>(
            input.Split(',')
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
        );
    }
    public void SelectImage()
    {
        parent.SelectImage((sprite => ThumbnailImage.sprite = sprite));
    }
}
