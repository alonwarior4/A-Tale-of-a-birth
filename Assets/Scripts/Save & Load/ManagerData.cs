using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ManagerData
{
    public int currentDeskNumberData;
    public int currentWallNumebrData;

    public ManagerData(StoryManager storyManager)
    {
        currentDeskNumberData = storyManager.currentDeskNumber;
        currentWallNumebrData = storyManager.currentWallNumber;
    }
    
}
