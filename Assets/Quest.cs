using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Kingdoms and Heroes/Quest", order = 1)]
public class Quest : ScriptableObject
{
    public int ID;
    public bool Active = false;
    public string Title;
    public string Description;
    // Does the quest have a specified duration?
    public int Duration = 0;
    // Quests those have to be finished before this one
    public int[] PrerequisteQuests;
    public int RewardXP = 100;
    public int RewardGold = 0;
    public int RewardResource1 = 0;
    public int RewardResource2 = 0;
    public int RewardResource3 = 0;
    public int RewardResource4 = 0;
    public int RewardResource5 = 0;
    public int RewardResource6 = 0;
    public int[] UnlocksQuests;
    
    DateTime StartedAt = DateTime.MinValue;
    DateTime CompletedAt = DateTime.MinValue;

    public void StartWorkingOnIt()
    {
        if(Active || CompletedAt != DateTime.MinValue)
        {
            return;
        }

        Active = true;
        StartedAt = DateTime.Now;

        Save();
    }

    public void FinishIt()
    {
        Active = false;
        CompletedAt = DateTime.Now;

        Save();
    }

    void Save()
    {
        // TODO
    }
}
