using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Thumbnail {
    public string Id;
    public Sprite Image;
    public string Description;
    public List<Choice> Choices;
}

