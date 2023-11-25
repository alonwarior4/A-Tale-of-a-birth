using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
using TapsellPlusSDK;

public class StoryManager : MonoBehaviour
{
    //Main Wall and Desk Transform for other objects that create in runtime to be child of
    [Header("Desk and Wall Parent Transforms")]
    [SerializeField] Transform wallTransform;
    [SerializeField] Transform deskTransform;

    //singleton
    public static StoryManager instance;

    //for load and save purpos
    [HideInInspector] public bool isFirstTime;

    //Scene Objects Saved in Dictionary
    public Dictionary<string, GameObject> hireArchyCreatedObjects = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> hireArchyExistedObj = new Dictionary<string, GameObject>();
    [Header("Animation Static Sprite Atlas")]
    [SerializeField] SpriteAtlas animSprites;

    //desk job index when puzzle break it
    [Header("Index of Desk Break Chain")]
    public int breakedDeskJobIndex = 0;

    //current scene numbers to save and load operations
    [Header("Number Of Wall And Desk need to be draw")]
    public int currentWallNumber = 1;
    public int currentDeskNumber = 1;
  
    //main camera cash
    Camera mainCamera;

    //Cash Values
    Color transparent = new Color(1, 1, 1, 0);
    Color adjective = new Color(1, 1, 1, 1);
    WaitForEndOfFrame waitToEndFrame = new WaitForEndOfFrame();
    AudioSource audioSource;

    //animator for change scene
    [Header("Change Scene Animator")]
    [SerializeField] Animator changeSceneAnimator;

    [Header("UI MANAGER PART")]
    [SerializeField] CanvasRenderer continueBtn;
    [SerializeField] CanvasRenderer playBtn;
    [SerializeField] CanvasRenderer aboutUsBtn;
    [SerializeField] CanvasRenderer okResetBtn;
    [SerializeField] CanvasRenderer cancelResetBtn;
    [SerializeField] CanvasRenderer newGameBtn;
    bool isShowCredit = false;

    // to specifiy when play destroy virus vfx
    [Header("Check if restarting ro continue")]
    ///*[HideInInspector] */public bool isRestartingOrContinue = false;

    [Header("Ui Button")]
    public Button resetPuzzleMicroBtn;
    public Button resetPuzzleNotebookBtn;

    [Header("Ui Images")]
    [SerializeField] Image creditImage;

    [Header("General Ui Ref")]
    [SerializeField] Canvas mainUiCanvas;
    [SerializeField] Canvas endGameCanvas;

    [Header("UI ANIMATORS")]
    [SerializeField] Animator PauseBtnAnimator;
    [SerializeField] Animator resetBtnAnimator;
    [SerializeField] Animator microMaskAnim;
    bool pauseAnimBool = false;
    bool resetAnimBool = false;

    [Header("Desk UI For NoteBook and Micro")]
    [SerializeField] GameObject notebookCanvas;
    [SerializeField] GameObject microCanvas;
    [SerializeField] AudioClip microTurnOn;

    //time delay offset
    [Header("time for fix wait duration bugs")]
    [SerializeField] float timeDelayOffset = 0.25f;

    //music controller refrence
    [Header("BackGround Music Controller")]
    [SerializeField] BGMusic_Controller musicController;

    //Async Data 
    W_Actions[] w_actionArray;
    D_Actions[] d_actionDataArray;

    //TODO : delete after test
    public bool DeleteAllPlayerPrefKeys;

    //refrence to puzzlePref for delete and restart purpose
    GameObject puzzlePref;

    //maximum wall numbers for wall and desk
    int maxWallNum = 14;
    int maxDeskNum = 13;

    [Header("Clock Audio source Refrence")]
    [SerializeField] AudioSource clockAS;

    //TODO : for tapsell advertisement uncomment below line
    //const string TAPSELL_KEY = "jmeqimfjoftlmrdcciqtjqkjfnnodgtggiplidtgilnkcqookimrhlgkirgcgsookpqcrp";
    //const string ZONE_ID = "5f796cf2986c27000137075e";
    //TapsellPlusAdModel tapsellLoadedAd = null;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        AddExistedHireArchyToDictionary();
        mainUiCanvas.gameObject.SetActive(true);

        currentWallNumber = 11;
        currentDeskNumber = 11;
        isFirstTime = true;

        if (DeleteAllPlayerPrefKeys)
        {
            PlayerPrefs.DeleteAll();
        }
        else
        {
            CheckIfFirstTime();
        }



        LoadGameProgress();
        


        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();

        if(currentDeskNumber <= maxDeskNum)
        {
            d_actionDataArray = Resources.Load<DeskDataSO>("D_Data/Ds" + (currentDeskNumber)).deskActions;
        }

        if (currentWallNumber <= maxWallNum)
        {
            w_actionArray = Resources.Load<WallDataSO>("W_Data/Ws" + (currentWallNumber)).actions;
        }
    }

    private void Start()
    {
        //TODO : for tapsel ad uncomment below line
        //Tapsell.Initialize(TAPSELL_KEY);

        clockAS.enabled = false;
        continueBtn.gameObject.SetActive(false);
        aboutUsBtn.gameObject.SetActive(true);
        playBtn.gameObject.SetActive(true);

        //if (TapsellAdManager._Instance.endGameTapsellAd == null)
        //{
        //    TapsellAdManager._Instance.RequestForEndTapsellAd();
        //}
    }



    //IEnumerator SendRequestForAdvertisement()
    //{
    //    RequestForAd();
    //    yield return new WaitForSeconds(5.5f);
    //    if (tapsellLoadedAd == null)
    //    {
    //        RequestForAd();
    //    }
    //}

    
    //private void RequestForAd()
    //{
    //    Tapsell.RequestAd(ZONE_ID, true, (TapsellAd resault) => { tapsellLoadedAd = resault; }, (string noAd) => { Debug.Log("no ad available"); },
    //        (TapsellError error) => { Debug.Log(error.message); }, (string noNetwork) => { Debug.Log("no network available"); }, (TapsellAd expiredAd) => { tapsellLoadedAd = null; });
    //}

    private void OnDestroy()
    {
        Destroy(instance);
    }

    private void CheckIfFirstTime()
    {
        if (PlayerPrefs.HasKey("FirstTimePassed"))
        {
            isFirstTime = false;           
        }
        else
        {
            PlayerPrefs.SetInt("FirstTimePassed", 1);
            currentWallNumber = 1;
            currentDeskNumber = 1;
            SaveSystem.SaveGameData(this);
        }
    }

    private void AddExistedHireArchyToDictionary()
    {
        GameObject[] existedObjects = GameObject.FindGameObjectsWithTag("Existed");
        for(int i=0; i<existedObjects.Length; i++)
        {
            hireArchyExistedObj.Add(existedObjects[i].name, existedObjects[i]);
        }
    }

    private void LoadGameProgress()
    {
        ////Read from Hdd Path
        ManagerData m_Data = SaveSystem.LoadGameData();
        currentWallNumber = m_Data.currentWallNumebrData;
        currentDeskNumber = m_Data.currentDeskNumberData;

        //load scenes in order
        for (int i = 1; i < currentWallNumber; i++)
        {
            LoadWall(i);
            if (i <= currentDeskNumber - 1)
            {
                LoadDesk(i);
            }
        }
    }

    private void ContinueButtonAndGameState()
    {
        if(currentDeskNumber < currentWallNumber)
        {
            continueBtn.gameObject.SetActive(true);
        }
        else
        {
            DoAllWallActions();
        }
    }


    #region Wall

    public void DoAllWallActions()
    {
        StartCoroutine(SequentialWallActions());
    }  

    IEnumerator SequentialWallActions()
    {
        ResourceRequest nextWallData = new ResourceRequest();

        //if(TapsellAdManager._Instance.endGameTapsellAd == null)
        //{
        //    TapsellAdManager._Instance.RequestForEndTapsellAd();
        //}

        if (currentWallNumber < maxWallNum)
        {
            nextWallData = Resources.LoadAsync<WallDataSO>("W_Data/Ws" + (currentWallNumber + 1));
        }

        MainGameBlendMusic(currentWallNumber);

        for (int i = 0; i < w_actionArray.Length; i++)
        {
            if (w_actionArray[i].w_IsSimultaneously)
            {
                StartCoroutine(DoWallAction(i));
                if (w_actionArray[i].w_WaitTime > 0)
                {
                    yield return new WaitForSeconds(w_actionArray[i].w_WaitTime + timeDelayOffset);
                }
            }
            else
            {
                yield return StartCoroutine(DoWallAction(i));
            }
        }

        if(currentWallNumber < maxWallNum)
        {
            currentWallNumber++;
            SaveSystem.SaveGameData(this);
            w_actionArray = null;
            Resources.UnloadUnusedAssets();
            yield return new WaitUntil(() => nextWallData.isDone);
            WallDataSO wallData = nextWallData.asset as WallDataSO;
            w_actionArray = wallData.actions;
        }
        else
        {
            EndTheGame();
        }  
    }

    IEnumerator DoWallAction(int index)
    {
        W_Actions actionData = w_actionArray[index];

        switch (actionData.w_Action)
        {
            case WallActions.None:
                break;


            case WallActions.fadeInSprite:
                //Make Game Object At default transform
                GameObject w_Obj = new GameObject(actionData.w_sprite.name);

                //Make this child of wall
                if (actionData.w_SetChildDesk)
                {
                    w_Obj.transform.parent = deskTransform;
                }
                else
                {
                    w_Obj.transform.parent = wallTransform;
                }

                // save to dictionary to change later
                if (actionData.w_UseLater)
                {
                    hireArchyCreatedObjects.Add(actionData.w_sprite.name, w_Obj);
                }

                //Set Transform
                w_Obj.transform.localPosition = actionData.w_Position;
                w_Obj.transform.localRotation = actionData.w_Rotation;
                w_Obj.transform.localScale = actionData.w_Scale;

                //add sprite renderer and set the sprite
                SpriteRenderer w_ObjSprite = w_Obj.AddComponent<SpriteRenderer>();
                w_ObjSprite.sortingOrder = actionData.w_SortingLayer;
                w_ObjSprite.sortingLayerName = actionData.w_sortingLayerName;
                w_ObjSprite.color = transparent;
                w_ObjSprite.sprite = actionData.w_sprite;

                // play sound if exist
                if (actionData.w_FSound)
                {
                    audioSource.PlayOneShot(actionData.w_FSound, actionData.w_audioVolume);
                }

                //fade in sprite
                for (float f = 0; f < actionData.w_FTime; f += Time.deltaTime)
                {
                    w_ObjSprite.color = Color.Lerp(transparent, adjective, Mathf.Min(1, f / actionData.w_FTime));
                    yield return waitToEndFrame;
                }
                break;


            case WallActions.InstantiateAnimationPrefab:
                //instanciate from prefab and make this child of wall
                GameObject animObj = Instantiate(actionData.w_AnimPrefab, actionData.w_Position, Quaternion.identity, (actionData.w_SetChildDesk) ? deskTransform : wallTransform) as GameObject;
                animObj.transform.localPosition = actionData.w_Position;

                string fixedObjName = animObj.name.Replace("(Clone)", "");
                animObj.name = fixedObjName;

                //can change to one sprite
                bool canReplace = false;

                if (fixedObjName != "Anim_frame1")
                {
                    canReplace = true;
                }

                //can save to dictionary to use later
                if (actionData.w_UseLater)
                {
                    hireArchyCreatedObjects.Add(fixedObjName, animObj);
                }

                //fade in
                SpriteRenderer animSprite = animObj.GetComponent<SpriteRenderer>();
                animSprite.color = transparent;

                for (float f = 0; f < actionData.w_FTime; f += Time.deltaTime)
                {
                    animSprite.color = Color.Lerp(transparent, adjective, Mathf.Min(1, f / actionData.w_FTime));
                    yield return waitToEndFrame;
                }

                //trigger story animation
                Animator w_animator = animObj.GetComponent<Animator>();
                Triggers[] w_animTriggers = actionData.w_Trigger;

                for (int i = 0; i < w_animTriggers.Length; i++)
                {
                    w_animator.SetTrigger(w_animTriggers[i].TriggerName);
                    w_animator.SetFloat("Speed", w_animTriggers[i].animSpeed);

                    if (actionData.w_IsSimultaneously && w_animTriggers[i].animWait > 0)
                    {
                        yield return new WaitForSeconds(w_animTriggers[i].animWait + timeDelayOffset);
                    }
                    else 
                    {
                        yield return waitToEndFrame;
                        yield return new WaitForSeconds(w_animator.GetCurrentAnimatorStateInfo(0).length + timeDelayOffset);
                    }
                }

                if (canReplace && !actionData.w_UseLater)
                {
                    StartCoroutine(SetLastFrame(w_animator , fixedObjName , animSprite));
                }

                break;


            case WallActions.ChangeSprite:

                //change an object sprite
                if (actionData.w_IsExistInScene)
                {
                    if (hireArchyExistedObj.ContainsKey(actionData.w_ObjName))
                    {
                        SpriteRenderer renderer = hireArchyExistedObj[actionData.w_ObjName].GetComponent<SpriteRenderer>();
                        renderer.sprite = actionData.w_ReplaceSprite;
                    }
                    else
                    {
                        Debug.LogError("There Is No Game Object With name " + actionData.w_ObjName + " in existed hirearchy objects please check the name");
                    }
                }
                else
                {
                    if (hireArchyCreatedObjects.ContainsKey(actionData.w_sprite.name))
                    {
                        SpriteRenderer renderer = hireArchyCreatedObjects[actionData.w_sprite.name].GetComponent<SpriteRenderer>();
                        renderer.sprite = actionData.w_ReplaceSprite;
                    }
                    else
                    {
                        Debug.LogError("There Is No Game Object With name" + actionData.w_ObjName + " in created runtime objects please check the name");
                    }

                }
                break;


            case WallActions.TriggerAnimation:

                //trigger camera and wait to end of animation
                Animator objectAnimator;
                if (actionData.w_IsCamera)
                {
                    objectAnimator = mainCamera.GetComponent<Animator>();
                }
                else if (actionData.w_IsExistInScene)
                {
                    objectAnimator = hireArchyExistedObj[actionData.w_ObjName].GetComponent<Animator>();
                }
                else
                {
                    objectAnimator = hireArchyCreatedObjects[actionData.w_ObjName].GetComponent<Animator>();
                }

                Triggers[] w_Triggers = actionData.w_Trigger;
                for (int i = 0; i < w_Triggers.Length; i++)
                {
                    objectAnimator.SetTrigger(w_Triggers[i].TriggerName);
                    objectAnimator.SetFloat("Speed", w_Triggers[i].animSpeed);

                    if (actionData.w_IsSimultaneously == false)
                    {
                        yield return waitToEndFrame;
                        yield return new WaitForSeconds(objectAnimator.GetCurrentAnimatorStateInfo(0).length + timeDelayOffset);
                    }
                    else
                    {
                        if (w_Triggers[i].animWait > 0)
                        {
                            yield return new WaitForSeconds(w_Triggers[i].animWait + timeDelayOffset);
                        }
                    }

                }
                break;


            case WallActions.FadeOutSprite:
                //fade out and delete an object created in the scene
                if (actionData.w_IsExistInScene)
                {
                    string spriteName = actionData.w_ObjName;

                    if (hireArchyExistedObj.ContainsKey(spriteName))
                    {
                        SpriteRenderer sceneSR = hireArchyExistedObj[spriteName].GetComponent<SpriteRenderer>();

                        for (float f = 0; f < actionData.w_FTime; f += Time.deltaTime)
                        {
                            sceneSR.color = Color.Lerp(adjective, transparent, Mathf.Min(1, f / actionData.w_FTime));
                            yield return waitToEndFrame;
                        }

                        GameObject forDelete = hireArchyExistedObj[spriteName];
                        hireArchyExistedObj.Remove(spriteName);
                        Destroy(forDelete);
                    }
                    else
                    {
                        Debug.LogError("Game Object with Name " + spriteName + " Not Found In Hirearchy");
                    }
                }
                else
                {
                    string spriteName = actionData.w_sprite.name;

                    if (hireArchyCreatedObjects.ContainsKey(spriteName))
                    {
                        SpriteRenderer sceneSR = hireArchyCreatedObjects[spriteName].GetComponent<SpriteRenderer>();

                        for (float f = 0; f < actionData.w_FTime; f += Time.deltaTime)
                        {
                            sceneSR.color = Color.Lerp(adjective, transparent, Mathf.Min(1, f / actionData.w_FTime));
                            yield return waitToEndFrame;
                        }

                        GameObject forDelete = hireArchyCreatedObjects[spriteName];
                        hireArchyCreatedObjects.Remove(spriteName);
                        Destroy(forDelete);
                    }
                    else
                    {
                        Debug.LogError("Game Object with Name " + spriteName + " Not Found In runtime objects");
                    }
                }
                break;


            case WallActions.DeleteGameObject:
                string w_objName;
                if (actionData.w_IsExistInScene)
                {
                    w_objName = actionData.w_ObjName;
                    GameObject objForDelete = hireArchyExistedObj[w_objName];
                    hireArchyExistedObj.Remove(w_objName);
                    Destroy(objForDelete);
                }
                else
                {
                    w_objName = actionData.w_sprite.name;
                    GameObject objForDelete = hireArchyCreatedObjects[w_objName];
                    hireArchyCreatedObjects.Remove(w_objName);
                    Destroy(objForDelete);
                }
                break;


            case WallActions.DeactiveContinueButton:
                if (continueBtn)
                {
                    continueBtn.gameObject.SetActive(false);
                }
                break;


            case WallActions.ActiveContinueButton:
                if (continueBtn)
                {
                    continueBtn.gameObject.SetActive( true);
                }
                break;


            default:
                break;
        }
    }

    #endregion

    #region Desk

    public void DoAllDeskActions()
    {
        StartCoroutine(SequentialDeskAction());
    }

    IEnumerator SequentialDeskAction()
    {
        ResourceRequest nextDeskData = new ResourceRequest();

        if(currentDeskNumber < maxDeskNum)
        {
            nextDeskData = Resources.LoadAsync<DeskDataSO>("D_Data/Ds" + (currentDeskNumber + 1));
        }
              
        for (int i = breakedDeskJobIndex; i < d_actionDataArray.Length; i++)
        {
            if (d_actionDataArray[i].d_DeskJob == DeskJobs.InstantiatePuzzlePrefab)
            {
                if (d_actionDataArray[i].d_PuzzlePrefab)
                {
                    breakedDeskJobIndex = i + 1;
                    yield return StartCoroutine(DoDeskAction(i));
                    yield break;
                }
            }
            else
            {
                if (d_actionDataArray[i].d_IsSimultaneously)
                {
                    StartCoroutine(DoDeskAction(i));
                    if (d_actionDataArray[i].d_WaitTime > 0)
                    {
                        yield return new WaitForSeconds(d_actionDataArray[i].d_WaitTime + timeDelayOffset);
                    }
                }
                else
                {
                    yield return StartCoroutine(DoDeskAction(i));
                }
            }


        }
        breakedDeskJobIndex = 0;
        if(currentDeskNumber < maxDeskNum)
        {
            currentDeskNumber++;
            SaveSystem.SaveGameData(this);

            d_actionDataArray = null;
            Resources.UnloadUnusedAssets();
            yield return new WaitUntil(() => nextDeskData.isDone);
            DeskDataSO deskData = nextDeskData.asset as DeskDataSO;
            d_actionDataArray = deskData.deskActions;

            StartCoroutine(ChangeToWallView());
        }
        else
        {
            StartCoroutine(ChangeToWallView());
        }
        
    }

    public IEnumerator DoDeskAction(int index)
    {
        //isRestartingOrContinue = false;
        //resetPuzzleNotebookBtn.interactable = false;
        //resetPuzzleMicroBtn.interactable = false;
        DisableResetButtons();

        D_Actions d_actionData = d_actionDataArray[index];

        switch (d_actionData.d_DeskJob)
        {
            case DeskJobs.DeactiveContinueButton:
                if (continueBtn)
                {
                    continueBtn.gameObject.SetActive(false);
                }
                break;


            case DeskJobs.ActivateContinueButton:
                if (continueBtn)
                {
                    continueBtn.gameObject.SetActive(true);
                }
                break;


            case DeskJobs.InstantiatePuzzlePrefab:
                // instanciate puzzle from prefab asset in scriptable object data and make this child of desk                            
                GameObject puzzleObj = Instantiate(d_actionData.d_PuzzlePrefab, d_actionData.d_Position, d_actionData.d_Rotation, deskTransform) as GameObject;
                puzzleObj.transform.localPosition = d_actionData.d_Position;
                if (microCanvas.activeInHierarchy)
                {
                    microMaskAnim.SetTrigger("On");
                    audioSource.PlayOneShot(microTurnOn);
                }            
                puzzlePref = puzzleObj;
                break;


            case DeskJobs.FadeInSprite:
                //Make Game Object At default transform
                GameObject D_spriteObj = new GameObject(d_actionData.d_Sprite.name);

                //make this child of desk
                if (d_actionData.d_SetWallChild)
                {
                    D_spriteObj.transform.parent = wallTransform;
                }
                else
                {
                    D_spriteObj.transform.parent = deskTransform;
                }

                if (d_actionData.d_UseLater)
                {
                    hireArchyCreatedObjects.Add(d_actionData.d_Sprite.name, D_spriteObj);
                }

                D_spriteObj.transform.localPosition = d_actionData.d_Position;
                D_spriteObj.transform.localRotation = d_actionData.d_Rotation;
                D_spriteObj.transform.localScale = d_actionData.d_Scale;

                //add sprite renderer and set the sprite
                SpriteRenderer spriteObj_SR = D_spriteObj.AddComponent<SpriteRenderer>();
                spriteObj_SR.color = transparent;
                spriteObj_SR.sortingOrder = d_actionData.d_SortingOrder;
                spriteObj_SR.sortingLayerName = d_actionData.d_SortingLayerName;
                spriteObj_SR.sprite = d_actionData.d_Sprite;

                //play sound if exist
                if (d_actionData.d_Sound)
                {
                    audioSource.PlayOneShot(d_actionData.d_Sound, d_actionData.d_Volume);
                }

                //fade in sprite
                for (float f = 0; f < d_actionData.d_Ftime; f += Time.deltaTime)
                {
                    spriteObj_SR.color = Color.Lerp(transparent, adjective, Mathf.Min(1, f / d_actionData.d_Ftime));
                    yield return waitToEndFrame;
                }
                break;


            case DeskJobs.ChangeSprite:

                //change an object sprite
                if (d_actionData.d_isExistInScene)
                {
                    if (hireArchyExistedObj.ContainsKey(d_actionData.d_ObjName))
                    {
                        SpriteRenderer renderer = hireArchyExistedObj[d_actionData.d_ObjName].GetComponent<SpriteRenderer>();
                        renderer.sprite = d_actionData.d_ReplaceSprite;
                    }
                    else
                    {
                        Debug.LogError("There Is No Game Object With name " + d_actionData.d_ObjName + " in existed hirearchy objects please check the name");
                    }
                }
                else
                {
                    if (hireArchyCreatedObjects.ContainsKey(d_actionData.d_Sprite.name))
                    {
                        SpriteRenderer renderer = hireArchyCreatedObjects[d_actionData.d_Sprite.name].GetComponent<SpriteRenderer>();
                        renderer.sprite = d_actionData.d_ReplaceSprite;
                    }
                    else
                    {
                        Debug.LogError("There Is No Game Object With name " + d_actionData.d_ObjName + " in created runtime objects please check the name");
                    }
                }
                break;


            case DeskJobs.TriggerAnimation:

                Animator d_ObjectAnimator = new Animator();

                if (d_actionData.d_IsCamera)
                {
                    d_ObjectAnimator = mainCamera.GetComponent<Animator>();
                }
                else if (d_actionData.d_isExistInScene)
                {
                    d_ObjectAnimator = hireArchyExistedObj[d_actionData.d_ObjName].GetComponent<Animator>();
                }
                else
                {
                    d_ObjectAnimator = hireArchyCreatedObjects[d_actionData.d_ObjName].GetComponent<Animator>();
                }

                d_ObjectAnimator.SetTrigger(d_actionData.d_triggerName);
                d_ObjectAnimator.SetFloat("Speed", d_actionData.d_animSpeed);

                if (d_actionData.d_IsSimultaneously == false)
                {
                    yield return waitToEndFrame;
                    yield return new WaitForSeconds(d_ObjectAnimator.GetCurrentAnimatorStateInfo(0).length + timeDelayOffset);
                }
                else
                {
                    if (d_actionData.d_WaitTime > 0)
                    {
                        yield return new WaitForSeconds(d_actionData.d_WaitTime + timeDelayOffset);
                    }
                }
                break;


            case DeskJobs.fadeOutsprite:

                //fade out and delete an object created on the desk
                if (d_actionData.d_isExistInScene)
                {
                    string spriteName = d_actionData.d_ObjName;

                    if (hireArchyExistedObj.ContainsKey(spriteName))
                    {
                        SpriteRenderer sceneSR = hireArchyExistedObj[spriteName].GetComponent<SpriteRenderer>();

                        for (float f = 0; f < d_actionData.d_Ftime; f += Time.deltaTime)
                        {
                            sceneSR.color = Color.Lerp(adjective, transparent, Mathf.Min(1, f / d_actionData.d_Ftime));
                            yield return waitToEndFrame;
                        }

                        GameObject forDelete = hireArchyExistedObj[spriteName];
                        hireArchyExistedObj.Remove(spriteName);
                        Destroy(forDelete);
                    }
                    else
                    {
                        Debug.LogError("Game Object with Name " + spriteName + " Not Found In Hirearchy");
                    }
                }
                else
                {
                    string spriteName = d_actionData.d_Sprite.name;

                    if (hireArchyCreatedObjects.ContainsKey(spriteName))
                    {
                        SpriteRenderer sceneSR = hireArchyCreatedObjects[spriteName].GetComponent<SpriteRenderer>();

                        for (float f = 0; f < d_actionData.d_Ftime; f += Time.deltaTime)
                        {
                            sceneSR.color = Color.Lerp(adjective, transparent, Mathf.Min(1, f / d_actionData.d_Ftime));
                            yield return waitToEndFrame;
                        }

                        GameObject forDelete = hireArchyCreatedObjects[spriteName];
                        hireArchyCreatedObjects.Remove(spriteName);
                        Destroy(forDelete);
                    }
                    else
                    {
                        Debug.LogError("Game Object with Name " + spriteName + " Not Found In runtime objects");
                    }
                }
                break;


            case DeskJobs.DeleteGameObject:
                string d_ObjName;
                if (d_actionData.d_isExistInScene)
                {
                    d_ObjName = d_actionData.d_ObjName;
                    GameObject objForDelete = hireArchyExistedObj[d_ObjName];
                    hireArchyExistedObj.Remove(d_ObjName);
                    Destroy(objForDelete);
                }
                else
                {
                    d_ObjName = d_actionData.d_Sprite.name;
                    GameObject objForDelete = hireArchyCreatedObjects[d_ObjName];
                    hireArchyCreatedObjects.Remove(d_ObjName);
                    Destroy(objForDelete);
                }
                break;


            default:
                break;
        }
    }

    #endregion


    private void MainGameBlendMusic(int sceneNumber)
    {
        if (sceneNumber == 6)
        {
            musicController.BlendSectionBGs(0 , 1 , 2);
        }
        else if (sceneNumber == 9)
        {
            musicController.BlendSectionBGs(1 , 2 , 2.5f);
        }
        else if (sceneNumber == 11)
        {
            musicController.BlendSectionBGs(2 , 3 , 2f);
        }
        else if(sceneNumber == 14)
        {
            musicController.BlendSectionBGs(3 , 4, 2.2f);
        }
    }

    private void LoadGameBlendMusic(int sceneNumber)
    {
        if (sceneNumber < 6)
        {
            musicController.BlendSectionBGs(0, 0, 1);
        }
        else if (sceneNumber >= 6 && sceneNumber <= 8)
        {
            musicController.BlendSectionBGs(0,1, 2);
        }
        else if (sceneNumber >= 9 && sceneNumber <= 10)
        {
            musicController.BlendSectionBGs(1, 2, 2.2f);
        }
        else if (sceneNumber >= 11 && sceneNumber <= 12)
        {
            musicController.BlendSectionBGs(2, 3, 2);
        }
        else if(sceneNumber >= 13 && sceneNumber <= 14)
        {
            float[] blends = { 0, 1 };
            musicController.masterMix.TransitionToSnapshots(musicController.masterSnapShots, blends, 0.1f);
            musicController.BlendSectionBGs(2, 3, 2f);
        }
    }

    private void EndTheGame()
    {
        mainUiCanvas.gameObject.SetActive(false);
        endGameCanvas.gameObject.SetActive(true);
        wallTransform.gameObject.SetActive(false);
        deskTransform.gameObject.SetActive(false);
    }

    IEnumerator SetLastFrame(Animator animator, string s_Name, SpriteRenderer spriteR)
    {
        yield return new WaitForSeconds(5.5f);

        animator.enabled = false;
        Destroy(animator);
        Resources.UnloadUnusedAssets();

        spriteR.sprite = animSprites.GetSprite(s_Name);
    }

    //TODO : for Tapsel Ad
    public void ShowTapsellLoadedAd()
    {
        //if (tapsellLoadedAd != null)
        //{
        //    Tapsell.ShowAd(tapsellLoadedAd, new TapsellShowOptions());
        //}
        TapsellAdManager._Instance.RequestAndShowAd();
    }

    public void DisableResetButtons()
    {
        resetPuzzleMicroBtn.interactable = false;
        resetPuzzleNotebookBtn.interactable = false;
    }

    

    #region Load Part
    private void LoadWall(int wallNumber)
    {
        W_Actions[] actionDataArray = Resources.Load<WallDataSO>("W_Data/Ws" + (wallNumber)).actions;

        //BlendMusic(wallNumber);

        for (int j = 0; j < actionDataArray.Length; j++)
        {
            W_Actions actionData = actionDataArray[j];

            switch (actionData.w_Action)
            {

                case WallActions.fadeInSprite:
                    //Make Game Object At default transform
                    GameObject spriteObj = new GameObject(actionData.w_sprite.name);

                    //make this child of wall
                    if (actionData.w_SetChildDesk)
                    {
                        spriteObj.transform.parent = deskTransform;
                    }
                    else
                    {
                        spriteObj.transform.parent = wallTransform;
                    }

                    //save for later
                    if (actionData.w_UseLater)
                    {
                        hireArchyCreatedObjects.Add(actionData.w_sprite.name, spriteObj);
                    }

                    //Set Transform
                    spriteObj.transform.localPosition = actionData.w_Position;
                    spriteObj.transform.localRotation = actionData.w_Rotation;
                    spriteObj.transform.localScale = actionData.w_Scale;

                    //add sprite renderer and set the sprite
                    SpriteRenderer spriteObj_SR = spriteObj.AddComponent<SpriteRenderer>();
                    spriteObj_SR.sortingOrder = actionData.w_SortingLayer;
                    spriteObj_SR.sortingLayerName = actionData.w_sortingLayerName;
                    spriteObj_SR.sprite = actionData.w_sprite;
                    break;


                case WallActions.InstantiateAnimationPrefab:
                    //instanciate from prefab and make this child of wall
                    GameObject animObj = Instantiate(actionData.w_AnimPrefab, actionData.w_Position, Quaternion.identity, (actionData.w_SetChildDesk) ? deskTransform : wallTransform) as GameObject;
                    animObj.transform.localPosition = actionData.w_Position;

                    string fixedObjName = animObj.name.Replace("(Clone)", "");
                    animObj.name = fixedObjName;

                    //save for later
                    if (actionData.w_UseLater)
                    {
                        hireArchyCreatedObjects.Add(animObj.name, animObj);
                    }

                    //trigger story animation
                    if (fixedObjName == "Anim_frame1")
                    {
                        Animator anim = animObj.GetComponent<Animator>();
                        for (int k = 0; k < actionData.w_Trigger.Length; k++)
                        {
                            anim.SetTrigger("L_" +  actionData.w_Trigger[k].TriggerName);
                        }


                        if (actionData.w_Trigger.Length > 0)
                        {
                            anim.SetFloat("Speed", actionData.w_Trigger[actionData.w_Trigger.Length - 1].animSpeed);
                        }
                    }
                    else
                    {
                        if (actionData.w_UseLater) break;
                        Animator objAnim = animObj.GetComponent<Animator>();
                        objAnim.enabled = false;
                        Destroy(objAnim);
                        
                        animObj.GetComponent<SpriteRenderer>().sprite = animSprites.GetSprite(fixedObjName);
                    }
                    break;


                case WallActions.ChangeSprite:
                    //change an object sprite
                    if (actionData.w_IsExistInScene)
                    {
                        if (hireArchyExistedObj.ContainsKey(actionData.w_ObjName))
                        {
                            SpriteRenderer renderer = hireArchyExistedObj[actionData.w_ObjName].GetComponent<SpriteRenderer>();
                            renderer.sprite = actionData.w_ReplaceSprite;
                        }
                        else
                        {
                            Debug.LogError("There Is No Game Object With name " + actionData.w_ObjName + " in existed hirearchy objects please check the name");
                        }

                    }
                    else
                    {
                        if (hireArchyCreatedObjects.ContainsKey(actionData.w_sprite.name))
                        {
                            SpriteRenderer renderer = hireArchyCreatedObjects[actionData.w_sprite.name].GetComponent<SpriteRenderer>();
                            renderer.sprite = actionData.w_ReplaceSprite;
                        }
                        else
                        {
                            Debug.LogError("There Is No Game Object With name " + actionData.w_ObjName + " in created runtime objects please check the name");
                        }

                    }
                    break;


                case WallActions.TriggerAnimation:
                    Animator objectAnimator;
                    if (actionData.w_IsCamera)
                    {
                        break;
                    }
                    else if (actionData.w_IsExistInScene)
                    {
                        objectAnimator = hireArchyExistedObj[actionData.w_ObjName].GetComponent<Animator>();
                    }
                    else
                    {
                        objectAnimator = hireArchyCreatedObjects[actionData.w_ObjName].GetComponent<Animator>();
                    }

                    Triggers[] w_Triggers = actionData.w_Trigger;

                    if(actionData.w_ObjName.Substring(0 , 2) == "PP")
                    {
                        for (int k = 0; k < w_Triggers.Length; k++)
                        {
                            objectAnimator.SetTrigger("L_" +  w_Triggers[k].TriggerName);
                            objectAnimator.SetFloat("Speed", w_Triggers[w_Triggers.Length - 1].animSpeed);
                        }
                    }
                    else
                    {
                        for (int k = 0; k < w_Triggers.Length; k++)
                        {
                            objectAnimator.SetTrigger(w_Triggers[k].TriggerName);
                            objectAnimator.SetFloat("Speed", w_Triggers[w_Triggers.Length - 1].animSpeed);
                        }
                    }                              
                    break;


                case WallActions.FadeOutSprite:
                    if (actionData.w_IsExistInScene)
                    {
                        string spriteName = actionData.w_ObjName;

                        if (hireArchyExistedObj.ContainsKey(spriteName))
                        {
                            GameObject forDelete = hireArchyExistedObj[spriteName];
                            hireArchyExistedObj.Remove(spriteName);
                            Destroy(forDelete);
                        }
                        else
                        {
                            Debug.LogError("Game Object with Name " + spriteName + " Not Found In existed Hirearchy objects");
                        }
                    }
                    else
                    {
                        string spriteName = actionData.w_sprite.name;

                        if (hireArchyCreatedObjects.ContainsKey(spriteName))
                        {
                            GameObject forDelete = hireArchyCreatedObjects[spriteName];
                            hireArchyCreatedObjects.Remove(spriteName);
                            Destroy(forDelete);
                        }
                        else
                        {
                            Debug.LogError("Game Object with Name " + spriteName + " Not Found In runtime created objects");
                        }
                    }
                    break;


                case WallActions.DeleteGameObject:
                    string w_objName;
                    if (actionData.w_IsExistInScene)
                    {
                        w_objName = actionData.w_ObjName;
                        GameObject objForDelete = hireArchyExistedObj[w_objName];
                        hireArchyExistedObj.Remove(w_objName);
                        Destroy(objForDelete);
                    }
                    else
                    {
                        w_objName = actionData.w_sprite.name;
                        GameObject objForDelete = hireArchyCreatedObjects[w_objName];
                        hireArchyCreatedObjects.Remove(w_objName);
                        Destroy(objForDelete);
                    }
                    break;


                default:
                    break;
            }
        }
        actionDataArray = null;
        Resources.UnloadUnusedAssets();
    }

    public void LoadDesk(int deskNumber)
    {             
            D_Actions[] d_actionDataArray = Resources.Load<DeskDataSO>("D_Data/Ds" + (deskNumber)).deskActions;

            for (int j = 0; j < d_actionDataArray.Length; j++)
            {
                D_Actions d_actionData = d_actionDataArray[j];

                switch (d_actionData.d_DeskJob)
                {

                    case DeskJobs.FadeInSprite:
                        //Make Game Object At default transform
                        GameObject D_spriteObj = new GameObject(d_actionData.d_Sprite.name);
                        
                        //make this child of desk
                        if (d_actionData.d_SetWallChild)
                        {
                            D_spriteObj.transform.parent = wallTransform;
                        }
                        else
                        {
                            D_spriteObj.transform.parent = deskTransform;
                        }

                        //save for later
                        if (d_actionData.d_UseLater)
                        {
                            hireArchyCreatedObjects.Add(d_actionData.d_Sprite.name, D_spriteObj);
                        }

                        //Set Transform
                        D_spriteObj.transform.localPosition = d_actionData.d_Position;
                        D_spriteObj.transform.localRotation = d_actionData.d_Rotation;
                        D_spriteObj.transform.localScale = d_actionData.d_Scale;

                        //add sprite renderer and set the sprite
                        SpriteRenderer spriteObj_SR = D_spriteObj.AddComponent<SpriteRenderer>();
                        spriteObj_SR.sortingOrder = d_actionData.d_SortingOrder;
                        spriteObj_SR.sortingLayerName = d_actionData.d_SortingLayerName;
                        spriteObj_SR.sprite = d_actionData.d_Sprite;
                        break;


                    case DeskJobs.ChangeSprite:
                        //change an object sprite
                        if (d_actionData.d_isExistInScene)
                        {
                            if (hireArchyExistedObj.ContainsKey(d_actionData.d_ObjName))
                            {
                                SpriteRenderer renderer = hireArchyExistedObj[d_actionData.d_ObjName].GetComponent<SpriteRenderer>();
                                renderer.sprite = d_actionData.d_ReplaceSprite;
                            }
                            else
                            {
                                Debug.LogError("There Is No Game Object With name " + d_actionData.d_ObjName + " in existed hirearchy objects please check the name");
                            }

                        }
                        else
                        {
                            if (hireArchyCreatedObjects.ContainsKey(d_actionData.d_Sprite.name))
                            {
                                SpriteRenderer renderer = hireArchyCreatedObjects[d_actionData.d_Sprite.name].GetComponent<SpriteRenderer>();
                                renderer.sprite = d_actionData.d_ReplaceSprite;
                            }
                            else
                            {
                                Debug.LogError("There Is No Game Object With name " + d_actionData.d_ObjName + " in created runtime objects please check the name");
                            }

                        }
                        break;


                case DeskJobs.TriggerAnimation:
                    Animator d_ObjAnimator;
                    if (d_actionData.d_IsCamera)
                    {
                        break;
                    }
                    else if (d_actionData.d_isExistInScene)
                    {
                        d_ObjAnimator = hireArchyExistedObj[d_actionData.d_ObjName].GetComponent<Animator>();
                    }
                    else
                    {
                        d_ObjAnimator = hireArchyCreatedObjects[d_actionData.d_ObjName].GetComponent<Animator>();
                    }

                    //if()
                    //d_ObjAnimator.SetTrigger(d_actionData.d_triggerName);
                    //d_ObjAnimator.SetFloat("Speed", d_actionData.d_animSpeed);
                    string triggerName = d_actionData.d_triggerName;
                    if (d_actionData.d_ObjName.Substring(0, 2) == "PP")
                    {
                        d_ObjAnimator.SetTrigger("L_" + triggerName);
                        d_ObjAnimator.SetFloat("Speed", d_actionData.d_animSpeed);
                    }
                    else
                    {

                        d_ObjAnimator.SetTrigger(triggerName);
                        d_ObjAnimator.SetFloat("Speed", d_actionData.d_animSpeed);
                    }
                    break;


                case DeskJobs.fadeOutsprite:
                        if (d_actionData.d_isExistInScene)
                        {
                            string spriteName = d_actionData.d_ObjName;

                            if (hireArchyExistedObj.ContainsKey(spriteName))
                            {
                                GameObject forDelete = hireArchyExistedObj[spriteName];
                                hireArchyExistedObj.Remove(spriteName);
                                Destroy(forDelete);
                            }
                            else
                            {
                                Debug.LogError("Game Object with Name " + spriteName + " Not Found In existed Hirearchy objects");
                            }
                        }
                        else
                        {
                            string spriteName = d_actionData.d_Sprite.name;

                            if (hireArchyCreatedObjects.ContainsKey(spriteName))
                            {
                                GameObject forDelete = hireArchyCreatedObjects[spriteName];
                                hireArchyCreatedObjects.Remove(spriteName);
                                Destroy(forDelete);
                            }
                            else
                            {
                                Debug.LogError("Game Object with Name " + spriteName + " Not Found In runtime created objects");
                            }
                        }
                        break;


                    case DeskJobs.DeleteGameObject:
                        string d_ObjName;
                        if (d_actionData.d_isExistInScene)
                        {
                            d_ObjName = d_actionData.d_ObjName;
                            GameObject objForDelete = hireArchyExistedObj[d_ObjName];
                            hireArchyExistedObj.Remove(d_ObjName);
                            Destroy(objForDelete);
                        }
                        else
                        {
                            d_ObjName = d_actionData.d_Sprite.name;
                            GameObject objForDelete = hireArchyCreatedObjects[d_ObjName];
                            hireArchyCreatedObjects.Remove(d_ObjName);
                            Destroy(objForDelete);
                        }
                        break;


                    default:
                        break;
                }
            }
        
    }

    #endregion
    
   
    // **************************************************** UI MANAGEMENT ****************************************************** //

    #region UI Part

    public void ContinueButton()
    {
        StartCoroutine(ChangeToDeskView());
    }

    public void PlayButton()
    {
        StartCoroutine(PlayButtonCoroutine());
    }

    public void PauseButton()
    {
        pauseAnimBool = !pauseAnimBool;
        if (resetAnimBool)
        {
            StartCoroutine(ClosePauseCoroutine());
        }
        else
        {
            PauseBtnAnimator.SetBool("ShowPause", pauseAnimBool);
        }
    }

    public void RestartPuzzle()
    {
        StartCoroutine(RestartPuzzleCoroutine());
    }

    public void ContinueStoryGameplay()
    {
        StartCoroutine(ContinueStoryGameplayCoroutine());
    }

    public void GoToMainMenu()
    {
        resetAnimBool = !resetAnimBool;
        resetBtnAnimator.SetBool("Show", resetAnimBool);
    }

    public void RestartPuzzleNoteBook()
    {
        //isRestartingOrContinue = true;
        //resetPuzzleNotebookBtn.interactable = false;
        DisableResetButtons();
        Destroy(puzzlePref);
        StartCoroutine(DoDeskAction(breakedDeskJobIndex - 1));
    }

    public void OKToMainMenu()
    {      
        SceneManager.LoadScene("Base Scene");
    }

    public void CancelToMainMenu()
    {
        resetAnimBool = false;
        resetBtnAnimator.SetBool("Show", resetAnimBool);
    }

    public void StartNewGame()
    {
        mainUiCanvas.GetComponent<Animator>().SetTrigger("Come");
    }

    public void OkToNewGame()
    {
        currentDeskNumber = 1;
        currentWallNumber = 1;
        SaveSystem.SaveGameData(this);
        SceneManager.LoadScene("Base Scene");
    }

    public void CancelToNewGame()
    {
        mainUiCanvas.GetComponent<Animator>().SetTrigger("Go");
    }

    public void ShowCredit()
    {
        isShowCredit = !isShowCredit;
        if (isShowCredit)
        {
            playBtn.gameObject.SetActive(false);
            newGameBtn.gameObject.SetActive(false);
            creditImage.gameObject.SetActive(true);
        }
        else
        {
            playBtn.gameObject.SetActive(true);
            newGameBtn.gameObject.SetActive(true);
            creditImage.gameObject.SetActive(false);
        }       
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator PlayButtonCoroutine()
    {
        //TODO : for tapsel ad
        //StartCoroutine(SendRequestForAdvertisement());


        playBtn.gameObject.SetActive(false);
        aboutUsBtn.gameObject.SetActive(false);
        newGameBtn.gameObject.SetActive(false);
        okResetBtn.gameObject.SetActive(false);
        cancelResetBtn.gameObject.SetActive(false);

        ContinueButtonAndGameState();
        yield return waitToEndFrame;
        LoadGameBlendMusic(currentWallNumber);
        clockAS.enabled = true;
    }

    IEnumerator ClosePauseCoroutine()
    {
        resetAnimBool = false;
        resetBtnAnimator.SetBool("Show", resetAnimBool);
        yield return new WaitForSeconds(1.25f);
        PauseBtnAnimator.SetBool("ShowPause", pauseAnimBool);
    }

    IEnumerator ChangeToDeskView()
    {
        continueBtn.gameObject.SetActive( false);
        changeSceneAnimator.SetTrigger("DeskView");
        mainCamera.farClipPlane = 4;
        deskTransform.position = new Vector3(deskTransform.position.x, deskTransform.position.y, 2);


        if (currentDeskNumber >= 2)
        {
            microCanvas.SetActive(true);
            resetPuzzleMicroBtn.interactable = false;
            microMaskAnim.gameObject.SetActive(true);
        }
        else
        {
            notebookCanvas.SetActive(true);
            resetPuzzleNotebookBtn.interactable = false;
        }

        yield return waitToEndFrame;
        yield return new WaitForSeconds(changeSceneAnimator.GetCurrentAnimatorStateInfo(0).length + 0.25f);

        DoAllDeskActions();
    }

    IEnumerator ChangeToWallView()
    {
        continueBtn.gameObject.SetActive( false);
        yield return new WaitForSeconds(2f);
        changeSceneAnimator.SetTrigger("WallView");
        mainCamera.farClipPlane = 6;
        deskTransform.position = new Vector3(deskTransform.position.x, deskTransform.position.y, -10);

        microCanvas.SetActive(false);
        microMaskAnim.gameObject.SetActive(false);
        notebookCanvas.SetActive(false);

        yield return waitToEndFrame;
        yield return new WaitForSeconds(changeSceneAnimator.GetCurrentAnimatorStateInfo(0).length + 0.25f);
          
        DoAllWallActions();
    }

    IEnumerator RestartPuzzleCoroutine()
    {
        //isRestartingOrContinue = true;
        resetPuzzleMicroBtn.interactable = false;
        microMaskAnim.SetTrigger("Off");
        yield return new WaitForSeconds(1.5f);
        if (puzzlePref.GetComponent<PuzzleManager>())
        {
            puzzlePref.GetComponent<PuzzleManager>().isVfxInstansiate = false;
        }
        Destroy(puzzlePref);
        StartCoroutine(DoDeskAction(breakedDeskJobIndex - 1));
    }

    IEnumerator ContinueStoryGameplayCoroutine()
    {
        yield return new WaitForSeconds(1f);
        microMaskAnim.SetTrigger("Off");
        yield return new WaitForSeconds(1.5f);
        //isRestartingOrContinue = true;
        if (puzzlePref.GetComponent<PuzzleManager>())
        {
            puzzlePref.GetComponent<PuzzleManager>().isVfxInstansiate = false;
        }
        Destroy(puzzlePref);
        DoAllDeskActions();
    }

    #endregion
}
