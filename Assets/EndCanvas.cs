using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCanvas : MonoBehaviour
{
    public void ShowRequestedTapsellAd()
    {
        StoryManager.instance.ShowTapsellLoadedAd();
        //TapsellAdManager._Instance.ShowLastTapsellAd();
    }
}
