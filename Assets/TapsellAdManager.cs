using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapsellPlusSDK;

public class TapsellAdManager : MonoBehaviour
{

    public static TapsellAdManager _Instance;

    const string TAPSELL_KEY = "jmeqimfjoftlmrdcciqtjqkjfnnodgtggiplidtgilnkcqookimrhlgkirgcgsookpqcrp";
    const string ZONE_ID = "5f796cf2986c27000137075e";

    private bool isTapsellInitialized = false;

    //TapsellAd tapsellLoadedAd = null;
    //public TapsellAd firstGameTapselAd = null;
    //public TapsellAd endGameTapsellAd = null;

    private void Awake()
    {
        if (!_Instance)
        {
            _Instance = this;
        }
    }

    void Start()
    {
        TryInitializeTapsellPlus();
        //TapsellPlus.Initialize(TAPSELL_KEY , (message) => { isTapsellInitialized = true; } , null);
        //if (PlayerPrefs.HasKey("FirstTimePassed"))
        //{
        //    RequestForFirstTapsellAd();
        //}

        //RequestForEndTapsellAd();
    }

    private void TryInitializeTapsellPlus()
    {
        if (isTapsellInitialized) return;

        TapsellPlus.Initialize(TAPSELL_KEY, (message) => { isTapsellInitialized = true; }, null);
    }

    public void RequestAndShowAd()
    {
        TryInitializeTapsellPlus();

        TapsellPlus.RequestInterstitialAd(ZONE_ID,
            (adModel) => 
            {
                TapsellPlus.ShowInterstitialAd(adModel.responseId, null, null, null);
            },
            null);
    }


    //public void RequestForEndTapsellAd()
    //{
    //    StartCoroutine(RequestForEndAdCoroutine());
    //}


    //public void RequestForFirstTapsellAd()
    //{
    //    StartCoroutine(RequestForFirstAdCoroutine());
    //}

    //IEnumerator RequestForFirstAdCoroutine()
    //{

    //    Tapsell.RequestAd(ZONE_ID, true, (TapsellAd resault) => { firstGameTapselAd = resault; }, (string noAd) => { Debug.Log("no ad available"); },
    //        (TapsellError error) => { Debug.Log(error.message); }, (string noNetwork) => { Debug.Log("no network available"); }, (TapsellAd expiredAd) => { firstGameTapselAd = null; });
       
    //    yield return new WaitForSeconds(5);

    //    if(firstGameTapselAd == null)
    //    {
    //        Tapsell.RequestAd(ZONE_ID, true, (TapsellAd resault) => { firstGameTapselAd = resault; }, (string noAd) => { Debug.Log("no ad available"); },
    //        (TapsellError error) => { Debug.Log(error.message); }, (string noNetwork) => { Debug.Log("no network available"); }, (TapsellAd expiredAd) => { firstGameTapselAd = null; });
    //    }
    //}

    //IEnumerator RequestForEndAdCoroutine()
    //{
    //    Tapsell.RequestAd(ZONE_ID, true, (TapsellAd resault) => { endGameTapsellAd = resault; }, (string noAd) => { Debug.Log("no ad available"); },
    //        (TapsellError error) => { Debug.Log(error.message); }, (string noNetwork) => { Debug.Log("no network available"); }, (TapsellAd expiredAd) => { endGameTapsellAd = null; });

    //    yield return new WaitForSeconds(5);

    //    if (endGameTapsellAd == null)
    //    {
    //        Tapsell.RequestAd(ZONE_ID, true, (TapsellAd resault) => { endGameTapsellAd = resault; }, (string noAd) => { Debug.Log("no ad available"); },
    //        (TapsellError error) => { Debug.Log(error.message); }, (string noNetwork) => { Debug.Log("no network available"); }, (TapsellAd expiredAd) => { endGameTapsellAd = null; });
    //    }
    //}

    //public void ShowLastTapsellAd()
    //{
    //    if(endGameTapsellAd != null)
    //    {
    //        Tapsell.ShowAd(endGameTapsellAd, new TapsellShowOptions());
    //    }
    //}

    //public void ShowFirstTapsellAd()
    //{
    //    if(firstGameTapselAd != null)
    //    {
    //        Tapsell.ShowAd(firstGameTapselAd, new TapsellShowOptions());
    //    }
    //}

    private void OnDestroy()
    {
        //Destroy(this);
        _Instance = null;
    }

}
