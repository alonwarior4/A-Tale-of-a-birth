using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    #region Audio Part
    public AudioClip bulletSound;
    public AudioClip virusDeathSound;
    public AudioClip virusChangeColor;
    [HideInInspector] public AudioSource audioSource;
    #endregion


    #region Variables
    [SerializeField] GameObject draggableSyringe;

    Camera mainCamera;
    Syringe hittedSyringe = null;
    Virus hittedVirus = null;


    Vector2 touchFirstPos;
    Vector2 touchLastPos;


    [Header("Puzzle Configs")]
    [SerializeField] int allPuzzleViruses;
    [SerializeField] float puzzleTime;
    [SerializeField] bool CanResolve;
    public GameObject vfxParticle;

    [Header("If Player Can Do Action With Puzzle Elements")]
    [SerializeField] bool isCanMoveSyringe;
    [SerializeField] bool isCanVirusFire;

    [Header("Noise Animation")]
    [SerializeField] Animator Noise;


    public bool isVfxInstansiate = true;

    #endregion



    private void Awake()
    {
        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();
        draggableSyringe.SetActive(false);
    }

    private void OnEnable()
    {
        if (CanResolve)
        {
            StoryManager.instance.resetPuzzleNotebookBtn.interactable = true;
            StoryManager.instance.resetPuzzleMicroBtn.interactable = true;
        }
        else
        {
            StoryManager.instance.resetPuzzleNotebookBtn.interactable = false;
            StoryManager.instance.resetPuzzleMicroBtn.interactable = false;
        }
    }

    private void Start()
    {
        if (Noise)
        {
            Noise.speed *= 10 / puzzleTime;
        }

        if(puzzleTime > 0.5f)
        {
            StartCoroutine(CheckForPuzzleEnd());
        }      
    }

    private void Update()
    {
        TouchRealiser();
    }

    private void TouchRealiser()
    {      
        if (Input.touchCount > 0)
        {
            Touch firstTouch = Input.GetTouch(0);
            Vector3 firstTouchWorldPos = mainCamera.ScreenToWorldPoint(firstTouch.position); 

            if (firstTouch.phase == TouchPhase.Began)
            {
                //reset hitted cash
                hittedVirus = null;
                hittedSyringe = null;

                touchFirstPos = firstTouchWorldPos;

                //layers to limit raycast hits
                int syringeLayer = LayerMask.GetMask("Syringe");
                int virusLayer = LayerMask.GetMask("Virus");

                RaycastHit2D hittedObj = Physics2D.CircleCast(firstTouchWorldPos , 0.1f , mainCamera.transform.forward , 100 , virusLayer | syringeLayer);
                if (!hittedObj)
                    return;

                SetHittedObject(hittedObj);
                if (hittedSyringe)
                {
                    draggableSyringe.SetActive(true);
                    DraggableSyringe D_Syringe = draggableSyringe.GetComponent<DraggableSyringe>();
                    D_Syringe.target = hittedSyringe;
                    D_Syringe.SetTargetConfigs();

                    Physics2D.IgnoreCollision(hittedSyringe.GetComponent<Collider2D>(), draggableSyringe.GetComponent<Collider2D>(), true);

                    foreach (Transform child in hittedSyringe.transform)
                    {
                        child.GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
            }

            if (hittedSyringe &&  firstTouch.phase == TouchPhase.Moved )
            {
                draggableSyringe.transform.position = new Vector3(firstTouchWorldPos.x, firstTouchWorldPos.y, hittedSyringe.transform.position.z);            
            }

            if (firstTouch.phase == TouchPhase.Ended)
            {
                touchLastPos = mainCamera.ScreenToWorldPoint(firstTouch.position);
                Physics2D.SyncTransforms();

                if (hittedVirus && isCanVirusFire)
                {
                    CalculateDragDirectionVector();
                }
                else if (hittedSyringe)
                {
                    hittedSyringe.SwappingProcess();

                    Physics2D.IgnoreCollision(hittedSyringe.GetComponent<Collider2D>(), draggableSyringe.GetComponent<Collider2D>(), false);
                    draggableSyringe.SetActive(false);
                    foreach (Transform child in hittedSyringe.transform)
                    {
                        child.GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
            }
        }
    }

    private void SetHittedObject(RaycastHit2D hit)
    {
        Virus virusTemp = hit.transform.GetComponent<Virus>();
        Syringe syringeTemp = hit.transform.GetComponent<Syringe>();

        if (virusTemp)
        {
            hittedVirus = virusTemp;
            hittedSyringe = null;
        }
        else if (isCanMoveSyringe && syringeTemp && syringeTemp.IsCanDoAction)
        {
            hittedSyringe = syringeTemp;
            hittedVirus = null;
        }
    }

    private void CalculateDragDirectionVector()
    {
        hittedVirus.dragDirection = DragDirection.None;

        float deltaX = touchLastPos.x - touchFirstPos.x;
        float deltaY = touchLastPos.y - touchFirstPos.y;

        float absoluteX = Mathf.Abs(deltaX);
        float absoluteY = Mathf.Abs(deltaY);

        if(absoluteX > absoluteY)
        {
            if (deltaX > 0.1f)
                hittedVirus.dragDirection = DragDirection.Right;
            if (deltaX < -0.1f)
                hittedVirus.dragDirection = DragDirection.Left;
        }
        else
        {
            if (deltaY > 0.1f)
                hittedVirus.dragDirection = DragDirection.Up;
            else if (deltaY < -0.1f)
                hittedVirus.dragDirection = DragDirection.Down;
        }

        if (hittedVirus.bulletHasVelocity || !IsAnyVirusExist()) return;         

        StartCoroutine(hittedVirus.FireBulletToDragDirection());
    }

    private bool IsAnyVirusExist()
    {
        RaycastHit2D[] objectAhead = Physics2D.RaycastAll(hittedVirus.transform.position, hittedVirus.FindDragDirectionVector(), 100f, LayerMask.GetMask("Virus") | LayerMask.GetMask("Blocker"));
        if (objectAhead.Length > 1)
        {
            if (objectAhead[1].transform.GetComponent<Virus>())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void CheckForVirusNumbers()
    {
        allPuzzleViruses -= 1;
        if (allPuzzleViruses == 0)
        {
            StoryManager.instance.DisableResetButtons();
            StoryManager.instance.ContinueStoryGameplay();
        }
    }

    IEnumerator CheckForPuzzleEnd()
    {
        WaitForEndOfFrame waitToEndOfFrame = new WaitForEndOfFrame();

        while(puzzleTime > 0.5f)
        {
            puzzleTime -= Time.deltaTime;
            yield return waitToEndOfFrame;
        }

        if (!CanResolve)
        {
            StoryManager.instance.ContinueStoryGameplay();
        }
        else
        {
            isCanMoveSyringe = false;
            isCanVirusFire = false;
        }
    }

}
