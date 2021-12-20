using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversations", menuName = "CustomScriptableObject/Conversation", order = 1)]
public class ConvData : ScriptableObject
{
    public Conversation entranceConv;
    public Conversation afterTutoConv;


    public List<Conversation> landingConv = new List<Conversation>();

    public Conversation firstLandingConv;
    public List<Conversation> defaultLandingConv = new List<Conversation>();
    public List<Conversation> land_FriendMilit = new List<Conversation>();
    public List<Conversation> land_FriendPilot = new List<Conversation>();
    public List<Conversation> land_FriendMecan = new List<Conversation>();
    public List<Conversation> land_FriendMilitPilot = new List<Conversation>();
    public List<Conversation> land_FriendPilotMecan = new List<Conversation>();
    public List<Conversation> land_FriendMecanMilit = new List<Conversation>();
    public List<Conversation> land_FriendMilitPilotMecan = new List<Conversation>();

    public void Initialize()
    {
        landingConv = new List<Conversation>();
        landingConv.Add(firstLandingConv);
        firstLandingConv.actionToPerformAtEnd += FirstLandDone;
    }

    public void FirstLandDone()
    {
        firstLandingConv.actionToPerformAtEnd = null;

        landingConv.RemoveAt(0);
        foreach(Conversation c in defaultLandingConv)
        {
            landingConv.Add(c);
        }
    }

    public void UpdateLandConvList()
    {

    }


}
