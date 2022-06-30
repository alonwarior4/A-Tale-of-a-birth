using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SyringeState
{
    Standby , Destroying , Off , Swapped , HasTwoColor
}

public class Syringe : MonoBehaviour
{
    GameObject DraggableSyringe;

    //Syringe Total State
    [Header("Syringe State")]
    public thisColor thisSyringeColor;
    public SyringeState state = SyringeState.Standby;

    //Syringe Swap State
    [HideInInspector] public bool isOnAnotherSyringe = false;
    [HideInInspector] public GameObject otherSyringe;
    public bool IsCanDoAction;

    //Syringe Transform
    [HideInInspector] public Vector3 defaultPosition;
    [HideInInspector] public Quaternion defaultRotation;
    [HideInInspector] public Vector3 defaultScale;
    

    //syringe Lazer animation Curve
    [Header("Child Laser Config")]
    public GameObject laser;
    [SerializeField] SpriteRenderer syringeLight;
    [SerializeField] SpriteRenderer syringeTank;
    [SerializeField] AnimationCurve lazerCurve;
    public float lazerAnimDuration;


    //cash value
    WaitForEndOfFrame waitToEndOfFrame = new WaitForEndOfFrame();
    PuzzleManager puzzleManager;
    AudioSource audioSource;

    //refrence audio clips
    [Header("Audio Clips")]
    [SerializeField] AudioClip pourSyringe;
    [SerializeField] AudioClip laserBigSound;
    [SerializeField] AudioClip syringeSwap;

    //last time laser pos behind virus
    public Vector3 lastKnownLaserPos;
    //new laser pos
    public Vector3 newLaserPos;
    //is syringe vertical or horizontal
    public bool isSyringeVertical;
    //all front viruses
    [HideInInspector] public List<RaycastHit2D> frontViruses = new List<RaycastHit2D>();
    //Last Virus That laser reached
    public Virus LastPointedVirus = null;

    public Transform laserFinalPos;
    public Transform finalPosCash;


    private void Awake()
    {
        finalPosCash = laserFinalPos;
        defaultPosition = transform.localPosition;
        defaultRotation = transform.localRotation;
        defaultScale = transform.localScale * transform.GetChild(1).transform.localScale.x;

        IsCanDoAction = true;
    }

    private void Start()
    {
        puzzleManager = transform.parent.GetComponent<PuzzleManager>();
        audioSource = puzzleManager.audioSource;

        SetSyringeAngle();
        SetSyringeState();

        Laser childLaser = laser.GetComponent<Laser>();
        childLaser.DrawLaser(transform.position, newLaserPos);

        lastKnownLaserPos = newLaserPos;
    }


    private void SetSyringeAngle()
    {
        float zRotation = transform.localEulerAngles.z;
        isSyringeVertical = (Mathf.Abs(zRotation) == 180 || Mathf.Abs(zRotation) == 0) ? true : false;
    }

    public void SetSyringeState()
    {
        LastPointedVirus = null;
        frontViruses = null;


        if (state == SyringeState.Swapped)
        {
            lastKnownLaserPos = transform.position;
        }

        frontViruses = Physics2D.RaycastAll(transform.position, -transform.up, 100f, LayerMask.GetMask("Virus")).ToList();

        if (frontViruses.Count == 0)
        {
            state = SyringeState.Off;
            newLaserPos = lastKnownLaserPos;
            return;
        }

        for (int i = 0; i < frontViruses.Count; i++)
        {
            Virus virus = frontViruses[i].transform.GetComponent<Virus>();
            if (virus.thisVirusColor != thisSyringeColor)
            {
                if (virus.virusHasTwoColor)
                {
                    state = SyringeState.HasTwoColor;
                    frontViruses.RemoveRange(i + 1, frontViruses.Count - (i + 1));
                }
                else
                {
                    state = SyringeState.Standby;
                }

                LastPointedVirus = virus;
                newLaserPos = virus.transform.position;
                return;
            }
        }

        state = SyringeState.Destroying;
        newLaserPos = laserFinalPos.position;

        //Vector3 lastVirusPos = frontViruses[frontViruses.Count - 1].transform.position;
        //float laserEndoffset = 0.5f * puzzleManager.transform.localScale.x;
        LastPointedVirus = frontViruses[frontViruses.Count - 1].transform.GetComponent<Virus>();
        //newLaserPos = (isSyringeVertical) ? (lastVirusPos.y > transform.position.y) ? lastVirusPos + new Vector3(0, laserEndoffset, 0) : lastVirusPos - new Vector3(0, laserEndoffset, 0) : (lastVirusPos.x > transform.position.x) ? lastVirusPos + new Vector3(laserEndoffset, 0, 0) : lastVirusPos - new Vector3(laserEndoffset, 0, 0);
    }


    public void DoSyringeState()
    {
        StartCoroutine(DoSyringeStateCoroutine());
    }

    IEnumerator DoSyringeStateCoroutine()
    {
        SetSyringeAngle();

        switch (state)
        {
            case SyringeState.Standby:
                IsCanDoAction = false;

                if (newLaserPos == lastKnownLaserPos)
                {
                    IsCanDoAction = true;
                    yield break;
                }
                else
                {
                    float oldPosDistance = Vector3.Distance(lastKnownLaserPos, transform.position);
                    float newPosDistance = Vector3.Distance(newLaserPos, transform.position);


                    if (newPosDistance > oldPosDistance)
                    {
                        laser.GetComponent<Laser>().DrawLaser(lastKnownLaserPos, newLaserPos);
                        lastKnownLaserPos = newLaserPos;
                        yield return new WaitForSeconds(1.5f);
                    }
                    else
                    {
                        laser.transform.position = newLaserPos;
                        lastKnownLaserPos = newLaserPos;
                    }

                    IsCanDoAction = true;
                }
                break;


            case SyringeState.Destroying:
                IsCanDoAction = false;

                if (lastKnownLaserPos != newLaserPos)
                {
                    laser.GetComponent<Laser>().DrawLaser(lastKnownLaserPos, newLaserPos);
                    lastKnownLaserPos = newLaserPos;
                }

                DisableFrontVirusesColliders();
                if (IsCanDestroy())
                {
                    GetComponent<Collider2D>().enabled = false;
                    EmptySyringe();
                }
                DestroyFrontViruses();
                break;


            case SyringeState.HasTwoColor:
                if (lastKnownLaserPos != newLaserPos)
                {
                    laser.GetComponent<Laser>().DrawLaser(lastKnownLaserPos, newLaserPos);
                    lastKnownLaserPos = newLaserPos;
                }
                LastPointedVirus.CheckTwoColorViruses();
                break;


            case SyringeState.Off:
                GetComponent<Collider2D>().enabled = false;
                DisableSyringe();
                break;


            default:
                break;
        }
    }

    public void CheckSyringeState()
    {
        SetSyringeAngle();
        SetSyringeState();
        DoSyringeState();
    }

    public void DisableFrontVirusesColliders()
    {
        for (int i = 0; i < frontViruses.Count; i++)
        {
            frontViruses[i].transform.GetComponent<Collider2D>().enabled = false;
        }
    }

    public void DestroyFrontViruses()
    {
        StartCoroutine(DestroyFrontVirusesCoroutine());
    }


    public void EmptySyringe()
    {
        StartCoroutine(EmptySyringeCoroutine());
    }

    IEnumerator DestroyFrontVirusesCoroutine()
    {
        StartCoroutine(LaserAnimation());

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < frontViruses.Count; i++)
        {
            frontViruses[i].transform.gameObject.SetActive(false);
            //puzzleManager.CheckForVirusNumbers();
            yield return new WaitForSeconds(0.2f);
        }

        IsCanDoAction = true;
        CheckSyringeState();
    }

    IEnumerator EmptySyringeCoroutine()
    {
        GetComponent<Animator>().SetTrigger("Empty");
        yield return new WaitForSeconds(2.75f);

        DisableSyringe();
    }

    bool IsCanDestroy()
    {
        if (Physics2D.RaycastAll(transform.position, -transform.up, 100f, LayerMask.GetMask("Virus")).Length == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator LaserAnimation()
    {
        Vector3 lazerDefaultScale = laser.transform.localScale;

        audioSource.PlayOneShot(pourSyringe, 1f);
        audioSource.PlayOneShot(laserBigSound, 0.5f);

        for (float f = 0; f < lazerAnimDuration; f += Time.deltaTime)
        {
            laser.transform.localScale = new Vector3(lazerDefaultScale.x, lazerCurve.Evaluate(Mathf.Min(1, (f / lazerAnimDuration))), lazerDefaultScale.z);
            yield return waitToEndOfFrame;
        }
        laser.transform.localScale = lazerDefaultScale;
        yield return new WaitForSeconds(0.5f);
    }


    private void DisableSyringe()
    {
        enabled = false;

        laser.SetActive(false);
        syringeLight.color = new Color(0.58f, 0.61f, 0.64f, 1);
        syringeTank.color = new Color(0.58f, 0.61f, 0.64f, 1);
    }


    public void SwappingProcess()
    {
        if (isOnAnotherSyringe && otherSyringe && otherSyringe.GetComponent<Syringe>().IsCanDoAction)
        {
            Syringe other = otherSyringe.GetComponent<Syringe>();

            SwapTwoSyringes();

            //change state
            state = SyringeState.Swapped;
            other.state = SyringeState.Swapped;

            //check state
            CheckSyringeState();
            other.CheckSyringeState();
        }

        otherSyringe = null;
    }

    private void SwapTwoSyringes()
    {
        Syringe other = otherSyringe.GetComponent<Syringe>();

        // Swap Positions
        transform.localPosition = otherSyringe.transform.localPosition;
        otherSyringe.transform.localPosition = defaultPosition;
        defaultPosition = transform.localPosition;
        /*otherSyringe.GetComponent<Syringe>()*/other.defaultPosition = otherSyringe.transform.localPosition;

        // Swap Rotations 
        transform.localRotation = otherSyringe.transform.localRotation;
        otherSyringe.transform.localRotation = defaultRotation;
        defaultRotation = transform.localRotation;
        /*otherSyringe.GetComponent<Syringe>()*/other.defaultRotation = otherSyringe.transform.localRotation;

        //Swap laser final poses
        laserFinalPos = other.finalPosCash;
        other.laserFinalPos = finalPosCash;

        finalPosCash = laserFinalPos;
        other.finalPosCash = other.laserFinalPos;

        audioSource.PlayOneShot(syringeSwap, 0.2f);
        Physics2D.SyncTransforms();
    }

}
