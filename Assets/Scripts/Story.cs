using System;
using System.Collections.Generic;

[Serializable]
public class Story
{
    public string StoryName;
    public string StartingThumbnailId;
    public List<Thumbnail> Thumbnails;
}

