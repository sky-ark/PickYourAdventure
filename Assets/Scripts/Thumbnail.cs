using System;
using System.Collections.Generic;

[Serializable]
public class Thumbnail {
    // The Id of the thumbnail for reference
    public string Id;
    // The name of the image of the thumbnail inside the folder with the extension
    // For example : "toto.png"
    public string ImageName;
    // The name of the sfx of the thumbnail inside the folder with the extension
    // For example : "toto.flac"
    public string SfxName;
    // The name of the music of the thumbnail inside the folder with the extension
    // For example : "toto.ogg"
    public string MusicName;
    // The description of the thumbnail, scenery description
    public string Description;
    // The list of the choices for this thumbnail
    public List<Choice> Choices;
}