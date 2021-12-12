using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Character
{
    None  = 0,
    pilot = 1,
    milit = 2,
    mecan = 3,
    alien = 4,
}

[Serializable]
public class Conversation
{
    public Action actionToPerformAtEnd;
    public List<Sentence> sentences;

    [Header("Reading it")]
    public int currentIndex = 0;
}

[Serializable]
public class Sentence 
{
    //maybe add "play music" or "wait for X second" or "play animation" to add more juice
    public Character character = Character.None;
    public int indexFace = 0;
    public Color bgColor = new Color(149,105,105,179);
    [TextArea]
    public string textDisplay = "";
    public float speed = 1f; 

    public Sentence()
    {
        character = Character.None;
        indexFace = 0;
        bgColor = new Color(149, 105, 105, 179);

        textDisplay = "";
        speed = 1f;
    }
}