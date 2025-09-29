using System;
using System.Collections.Generic;

[Serializable]
public class Choice {
    // The content of the choice (clicked label)
    public string Description;
    // The id of the thumbnail this choice send
    public string ThumbnailLinkId;
    // The list of item need to be able to select this choice
    public List<string> NeededItemsId;
    // The list of item given when selecting this choice
    public List<string> GivenItemsId;
    // The list of item remove form the inventory when selecting this choice
    public List<string> TakenItemsId;
}