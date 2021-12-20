using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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

[Serializable][ExecuteAlways]
public class Sentence 
{
    //maybe add "play music" or "wait for X second" or "play animation" to add more juice
    [OnValueChanged("ColorTest")]
    public Character character = Character.None;
    public int indexFace = 0;
    public Color bgColor = Color.white;
    [TextArea]
    public string textDisplay = "";
    public float speed = 1f;
    public AnimName animation = AnimName.none;

    public enum AnimName
    {
        none = 0,
        Squish,
        Shake,
        Jump,
        QuickRot,
        Forward = 5,
        ForwardRot,
    }
    
    [Button]
    public void Test()
    {
        GameObject.FindObjectOfType<UI_Conversation>().DisplaySentence(this);
    }

    public void ColorTest()
    {
        //if (bgColor == Color.white || bgColor == new Color(191f / 255f, 135f / 255f, 135f / 255f, 255f / 255f))
        {
            switch (character)
            {
                case Character.pilot:
                    bgColor = new Color(255f / 255f, 241f / 255f, 134f / 255f, 146f / 255f);
                    break;
                case Character.milit:
                    bgColor = new Color(255f / 255f, 102f / 255f,  81f / 255f, 146f / 255f);
                    break;
                case Character.mecan:
                    bgColor = new Color(0f   / 255f, 225f / 255f, 255f / 255f, 146f / 255f);
                    break;
            }
        }
    }

    public Sentence()
    {
        character = Character.None;
        indexFace = 0;
        bgColor = new Color(149, 105, 105, 179);

        textDisplay = "";
        speed = 1f;
    }


}