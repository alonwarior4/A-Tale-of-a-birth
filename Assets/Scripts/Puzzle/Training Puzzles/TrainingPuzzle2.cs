using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class TrainingPuzzle2 : MonoBehaviour
{
    [Header("Syringes")]
    [SerializeField] GameObject redSyringe;
    [SerializeField] GameObject yellowSyringe;

    [Header("Tip Animation")]
    [SerializeField] Animator pointerAnim;

    [Header("Sounds")]
    [SerializeField] AudioClip DestroyVirus;
    [SerializeField] AudioClip laserKoloft;
    [SerializeField] AudioClip pourSyringe;

    WaitForEndOfFrame waitToEndFrame = new WaitForEndOfFrame();

    AudioSource audioSource;

    GameObject touchedObject;

    Vector3 redSyringDefaultPos;
    Vector3 yellowSyringeDefaultPos;

    Camera mainCamera;

    Animator animator;

    private void Awake()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        animator.enabled = false;
    }

    private void Start()
    {     
        pointerAnim.SetTrigger("OnlyTip");

        redSyringDefaultPos = redSyringe.transform.localPosition;
        yellowSyringeDefaultPos = yellowSyringe.transform.localPosition;

        StartCoroutine(TrainPuuzzle());
    }

    IEnumerator TrainPuuzzle()
    {
        while (true)
        {
            if (Input.touchCount > 0)
            {
                Touch firstTouch = Input.GetTouch(0);

                if (firstTouch.phase == TouchPhase.Began)
                {
                    Vector2 touchWorldPos = mainCamera.ScreenToWorldPoint(firstTouch.position);
                    RaycastHit2D hit;
                    hit = Physics2D.CircleCast(touchWorldPos, 0.1f, transform.forward, 100f);

                    if (hit)
                    {
                        touchedObject = hit.transform.gameObject;
                        touchedObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                    }
                }

                if (touchedObject && firstTouch.phase == TouchPhase.Moved)
                {
                    Vector2 touchPos = mainCamera.ScreenToWorldPoint(firstTouch.position);
                    touchedObject.transform.position = new Vector3(touchPos.x, touchPos.y, touchedObject.transform.position.z);
                }


                if (touchedObject && firstTouch.phase == TouchPhase.Ended)
                {
                    touchedObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;

                    bool is2SyringeCollide = yellowSyringe.GetComponent<Collider2D>().bounds.Intersects(redSyringe.GetComponent<Collider2D>().bounds);

                    if (is2SyringeCollide)
                    {
                        yellowSyringe.transform.localPosition = redSyringDefaultPos;
                        redSyringe.transform.localPosition = yellowSyringeDefaultPos;
                        animator.enabled = true;
                        pointerAnim.SetTrigger("End");
                        GetComponent<Animator>().SetTrigger("VirusKill");
                        yield return new WaitForSeconds(4f);
                        StoryManager.instance.ContinueStoryGameplay();
                        break;
                    }
                    else
                    {
                        if (touchedObject.Equals(redSyringe))
                        {
                            redSyringe.transform.localPosition = redSyringDefaultPos;
                            touchedObject = null;
                        }
                        else
                        {
                            yellowSyringe.transform.localPosition = yellowSyringeDefaultPos;
                            touchedObject = null;
                        }

                    }
                }
            }
            yield return waitToEndFrame;
        }
    }

    public void PlayDestroyVirusSound()
    {
        if (DestroyVirus)
        {
            audioSource.PlayOneShot(DestroyVirus);
        }
    }

    public void PlayLaserKoloftSound()
    {
        if (laserKoloft)
        {
            audioSource.PlayOneShot(laserKoloft);
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
