using System;

[Serializable]
public class Item
{
    // The Id of the item for reference
    public string Id;
    // The name of the item (for label)
    public string ItemName;
    // The name of the icon representative of the item in the story folder (example : sword.png)
    public string IconName;
    // The description of the item (for label)
    public string Description;
}