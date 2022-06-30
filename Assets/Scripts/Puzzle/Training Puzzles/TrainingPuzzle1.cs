using System.Collections;
using UnityEngine;


public enum MoveDirection
{
    None , Up , Down , Right , Left
}
public class TrainingPuzzle1 : MonoBehaviour
{
    [Header("Viruses")]
    [SerializeField] GameObject yellowVirus1;
    [SerializeField] GameObject redVirus1;
    [SerializeField] GameObject yellowVirus2;

    [Header("Pointer Animtor")]
    [SerializeField] Animator pointerAnim;

    //Cashed Variables
    GameObject touchedObject;
    MoveDirection direction;
    Animator thisAnim;

    WaitForEndOfFrame waitToEndFrame = new WaitForEndOfFrame();

    Vector3 firstTouchPos;
    Vector3 lastTouchPos;

    Camera mainCamera;

    [Header("Sounds")]
    [SerializeField] AudioClip DestroyVirus;
    [SerializeField] AudioClip LaserKoloft;
    [SerializeField] AudioClip fireBullet;
    [SerializeField] AudioClip changeColorVirus;
    [SerializeField] AudioClip pourSyringe;

    AudioSource audioSource;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        touchedObject = redVirus1;
        thisAnim = GetComponent<Animator>();

        StartCoroutine(DoFirstTip());
        pointerAnim.SetTrigger("FirstTip");
    }

    IEnumerator DoFirstTip()
    {
        while (true)
        {
            if (Input.touchCount > 0)
            {
                Touch firstTouch = Input.GetTouch(0);
                if (firstTouch.phase == TouchPhase.Began)
                {
                    Vector2 touchWorldPos = mainCamera.ScreenToWorldPoint(firstTouch.position);
                    firstTouchPos = touchWorldPos;

                    RaycastHit2D hit;
                    hit = Physics2D.CircleCast(touchWorldPos , 0.1f , mainCamera.transform.forward , 100f);

                    if (hit)
                    {
                        touchedObject = hit.transform.gameObject;
                    }
                }

                if (firstTouch.phase == TouchPhase.Ended && touchedObject.Equals(yellowVirus2))
                {
                    lastTouchPos = mainCamera.ScreenToWorldPoint(firstTouch.position);

                    float deltaX = lastTouchPos.x - firstTouchPos.x;
                    float deltaY = lastTouchPos.y - firstTouchPos.y;
                    float absoluteX = Mathf.Abs(deltaX);
                    float absoluteY = Mathf.Abs(deltaY);

                    if (absoluteX > absoluteY)
                    {
                        direction = (deltaX > 0) ? MoveDirection.Right : MoveDirection.Left;
                    }
                    else
                    {
                        direction = (deltaY > 0) ? MoveDirection.Up : MoveDirection.Down;
                    }

                    if (direction == MoveDirection.Left)
                    {
                        thisAnim.SetTrigger("LeftFire");
                        pointerAnim.SetTrigger("Default");
                        yield return waitToEndFrame;
                        yield return new WaitForSeconds(thisAnim.GetCurrentAnimatorStateInfo(0).length + 1);                       
                        break;
                    }
                }
            }
            yield return waitToEndFrame;
        }
        StartCoroutine(DoSecondTip());
        pointerAnim.SetTrigger("SecondTip");
    }

    IEnumerator DoSecondTip()
    {
        while (true)
        {
            if (Input.touchCount > 0)
            {
                Touch firstTouch = Input.GetTouch(0);
                if (firstTouch.phase == TouchPhase.Began)
                {
                    Vector2 touchWorldPos = mainCamera.ScreenToWorldPoint(firstTouch.position);
                    firstTouchPos = touchWorldPos;

                    RaycastHit2D hit;
                    hit = Physics2D.CircleCast(touchWorldPos,0.1f ,  transform.forward , 100f);

                    if (hit)
                    {
                        touchedObject = hit.transform.gameObject;
                    }
                }

                if (firstTouch.phase == TouchPhase.Ended && touchedObject.Equals(yellowVirus1))
                {
                    lastTouchPos = mainCamera.ScreenToWorldPoint(firstTouch.position);
                    float deltaX = lastTouchPos.x - firstTouchPos.x;
                    float deltaY = lastTouchPos.y - firstTouchPos.y;
                    float absoluteX = Mathf.Abs(deltaX);
                    float absoluteY = Mathf.Abs(deltaY);

                    if (absoluteX > absoluteY)
                    {
                        direction = (deltaX > 0) ? MoveDirection.Right : MoveDirection.Left;
                    }
                    else
                    {
                        direction = (deltaY > 0) ? MoveDirection.Up : MoveDirection.Down;
                    }

                    if (direction == MoveDirection.Down)
                    {
                        thisAnim.SetTrigger("TopFire");
                        pointerAnim.SetTrigger("Default");
                        yield return waitToEndFrame;
                        yield return new WaitForSeconds(thisAnim.GetCurrentAnimatorStateInfo(0).length + 1);
                        StoryManager.instance.ContinueStoryGameplay();
                        break;
                    }
                }
            }
            yield return waitToEndFrame;
        }
    }

    public void PlayDestryVirusSound()
    {
        if (DestroyVirus)
        {
            audioSource.PlayOneShot(DestroyVirus);
        }
    }

    public void PlayLaserEffectSound()
    {
        if (LaserKoloft)
        {
            audioSource.PlayOneShot(LaserKoloft);
        }
    }

    public void PlayFireBulletSound()
    {
        if (fireBullet)
        {
            audioSource.PlayOneShot(fireBullet);
        }
    }

    public void PlayChangeColorVirus()
    {
        if (changeColorVirus)
        {
            audioSource.PlayOneShot(changeColorVirus);
        }
    }
    
    public void PlayPourSyringe()
    {
        if (pourSyringe)
        {
            audioSource.PlayOneShot(pourSyringe);
        }
    }
}
