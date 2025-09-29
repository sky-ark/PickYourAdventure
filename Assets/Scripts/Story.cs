using System;
using System.Collections.Generic;

[Serializable]
public class Story
{
    // The named of the story displayed
    public string StoryName;
    // The name of the image representative of the story (banner-like)
    public string ImageName;
    // The starting id of the first thumbnail
    public string StartingThumbnailId;
    // The list containing all thumbnail of the story
    public List<Thumbnail> Thumbnails;
    // The list containing all items of the story
    public List<Item> Items;
}