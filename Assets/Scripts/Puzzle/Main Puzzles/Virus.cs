using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]public enum thisColor
{
    None =0 ,
    Blue = 1 ,
    Red = 2 , 
    Yellow = 4, 

    BlueRed = Blue | Red , 
    YellowRed = Yellow | Red,
}

public enum DragDirection
{
    None ,
    Right , 
    Left , 
    Up ,
    Down
}

public class Virus : MonoBehaviour
{
    //Virus Status
    [Header("Virus State")]
    public thisColor thisVirusColor;
    [HideInInspector] public thisColor pointedSyringeColor;

    [HideInInspector] public bool virusHasTwoColor = false;

    //Set Drag Direction
    [HideInInspector] public DragDirection dragDirection;

    [HideInInspector] public bool bulletHasVelocity = false;

    WaitForEndOfFrame waitToEndOfFrame = new WaitForEndOfFrame();
    PuzzleManager puzzleManager;
    AudioSource audioSource;

    [SerializeField] GameObject bullet;

    [Header("Bullet Animation Config")]
    [SerializeField] float bulletOutDuration;
    [SerializeField] float bulletSpeed;
    public AnimationCurve bulletOutAniamtion;
    public AnimationCurve bulletScaleAnimation;

    [HideInInspector] public List<Syringe> aroundSyringes = new List<Syringe>();

    [Header("Part of virus that colored")]
    public GameObject VirusForColoring;
   


    private void Awake()
    {       
        virusHasTwoColor =  thisVirusColor == thisColor.BlueRed || thisVirusColor == thisColor.YellowRed;
    }

    private void Start()
    {
        puzzleManager = transform.parent.GetComponent<PuzzleManager>();
        audioSource = puzzleManager.audioSource;

        if (bullet)
        {
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        }
    }

    public Vector2 FindDragDirectionVector()
    {
        switch (dragDirection)
        {
            case DragDirection.None:
                {
                    return Vector2.zero;
                }

            case DragDirection.Right:
                {
                    return transform.right;
                }

            case DragDirection.Left:
                {
                    return -transform.right;
                }

            case DragDirection.Up:
                {
                    return transform.up;
                }

            case DragDirection.Down:
                {

                    return -transform.up;
                }
            default:
                return Vector2.zero;
        }
    }

    public IEnumerator FireBulletToDragDirection()
    {            
        if (virusHasTwoColor) { yield break; }

        Vector3 defaultBulletPos = bullet.transform.localPosition;

        AudioClip bulletClip = puzzleManager.bulletSound;
        audioSource.PlayOneShot(bulletClip,0.3f);

        switch (dragDirection)
        {
            case DragDirection.Left:
                bulletHasVelocity = true;
                bullet.transform.eulerAngles = Vector3.zero;
                for (float f = 0; f < bulletOutDuration; f += Time.deltaTime)
                {
                    bullet.transform.localPosition = new Vector3(bulletOutAniamtion.Evaluate(Mathf.Min(1, (f / bulletOutDuration) + 0.1f)), defaultBulletPos.y, defaultBulletPos.z);
                    bullet.transform.localScale = new Vector3(bullet.transform.localScale.x, bulletScaleAnimation.Evaluate(Mathf.Min(1, (f / bulletOutDuration) + 0.1f)), bullet.transform.localScale.z);
                    yield return waitToEndOfFrame;
                }
                bullet.GetComponent<Rigidbody2D>().velocity = Vector2.left * bulletSpeed;
                
                break;

            case DragDirection.Down:
                bulletHasVelocity = true;
                bullet.transform.eulerAngles = new Vector3(0, 0, 90);
                for (float f = 0; f < bulletOutDuration; f += Time.deltaTime)
                {
                    bullet.transform.localPosition = new Vector3(defaultBulletPos.x, bulletOutAniamtion.Evaluate(Mathf.Min(1, (f / bulletOutDuration) + 0.1f)), defaultBulletPos.z);
                    bullet.transform.localScale = new Vector3(bullet.transform.localScale.x, bulletScaleAnimation.Evaluate(Mathf.Min(1, (f / bulletOutDuration) + 0.1f)), bullet.transform.localScale.z);
                    yield return waitToEndOfFrame;
                }
                bullet.GetComponent<Rigidbody2D>().velocity = Vector2.down * bulletSpeed;
               
                break;

            case DragDirection.Right:
                bulletHasVelocity = true;
                bullet.transform.eulerAngles = new Vector3(0, 0, 180);
                for (float f = 0; f < bulletOutDuration; f += Time.deltaTime)
                {
                    bullet.transform.localPosition = new Vector3(-bulletOutAniamtion.Evaluate(Mathf.Min(1, (f / bulletOutDuration) + 0.1f)), defaultBulletPos.y, defaultBulletPos.z);
                    bullet.transform.localScale = new Vector3(bullet.transform.localScale.x, bulletScaleAnimation.Evaluate(Mathf.Min(1, (f / bulletOutDuration) + 0.1f)), bullet.transform.localScale.z);
                    yield return waitToEndOfFrame;
                }
                bullet.GetComponent<Rigidbody2D>().velocity = Vector2.right * bulletSpeed;
                break;

            case DragDirection.Up:
                bulletHasVelocity = true;
                bullet.transform.eulerAngles = new Vector3(0, 0, 270);
                for (float f = 0; f < bulletOutDuration; f += Time.deltaTime)
                {
                    bullet.transform.localPosition = new Vector3(defaultBulletPos.x, -bulletOutAniamtion.Evaluate(Mathf.Min(1, (f / bulletOutDuration) + 0.1f)), defaultBulletPos.z);
                    bullet.transform.localScale = new Vector3(bullet.transform.localScale.x, bulletScaleAnimation.Evaluate(Mathf.Min(1, (f / bulletOutDuration) + 0.1f)), bullet.transform.localScale.z);
                    yield return waitToEndOfFrame;
                }
                bullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * bulletSpeed;
                
                break;
        }
    }

    private bool IsAroundSyringesCompleteVirusColor()
    {
        if (!virusHasTwoColor) return false;

        pointedSyringeColor = thisColor.None;

        int syringeLayerMask = 1 << 8;

        RaycastHit2D leftSyringe = Physics2D.Raycast(transform.position, -transform.right, 100, syringeLayerMask);
        RaycastHit2D rightSyringe = Physics2D.Raycast(transform.position, transform.right, 100, syringeLayerMask);
        RaycastHit2D topSyringe = Physics2D.Raycast(transform.position, transform.up, 100, syringeLayerMask);
        RaycastHit2D botSyringe = Physics2D.Raycast(transform.position, -transform.up, 100, syringeLayerMask);


        aroundSyringes.Clear();

        if (leftSyringe)
        {
            pointedSyringeColor = pointedSyringeColor | leftSyringe.transform.GetComponent<Syringe>().thisSyringeColor;
            aroundSyringes.Add(leftSyringe.transform.GetComponent<Syringe>());
        }
        if (rightSyringe)
        {
            pointedSyringeColor = pointedSyringeColor | rightSyringe.transform.GetComponent<Syringe>().thisSyringeColor;
            aroundSyringes.Add(rightSyringe.transform.GetComponent<Syringe>());
        }
        if (topSyringe)
        {
            pointedSyringeColor = pointedSyringeColor | topSyringe.transform.GetComponent<Syringe>().thisSyringeColor;
            aroundSyringes.Add(topSyringe.transform.GetComponent<Syringe>());
        }
        if (botSyringe)
        {
            pointedSyringeColor = pointedSyringeColor | botSyringe.transform.GetComponent<Syringe>().thisSyringeColor;
            aroundSyringes.Add(botSyringe.transform.GetComponent<Syringe>());
        }

        if (pointedSyringeColor == thisVirusColor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckTwoColorViruses()
    {
        if (IsAroundSyringesCompleteVirusColor() == false) return;

        for (int i = 0; i < aroundSyringes.Count; i++)
        {
            Syringe pointedSyringe = aroundSyringes[i];
            if (pointedSyringe.state != SyringeState.HasTwoColor || !pointedSyringe.LastPointedVirus.Equals(this))
                return;
        }

        for (int i = 0; i < aroundSyringes.Count; i++)
        {
            aroundSyringes[i].state = SyringeState.Destroying;
            aroundSyringes[i].DoSyringeState();
        }
    }

    public void SetMainVirusColor()
    {
        if (bullet)
        {
            GetComponent<SpriteRenderer>().color = bullet.GetComponent<Bullet>().SetSpriteColor(thisVirusColor);
        }
    }

    private void OnDisable()
    {
        InstantiateVfxEffect();
        InvokeAroundSyringes();
    }

    private void InstantiateVfxEffect()
    {        
        if (/*!StoryManager.instance.isRestartingOrContinue*/ puzzleManager.isVfxInstansiate)
        {
            GameObject vfxPrefab = puzzleManager.vfxParticle;
            SpriteRenderer vfxSprite = vfxPrefab.GetComponent<SpriteRenderer>();
            if (virusHasTwoColor)
            {
                if (thisVirusColor == thisColor.BlueRed)
                {
                    vfxSprite.color = new Color(0.65f, 0.21f, 1, 1);
                }
                else
                {
                    vfxSprite.color = new Color(1, 0.38f, 0.18f, 1);
                }
            }
            else
            {
                vfxSprite.color = GetComponent<SpriteRenderer>().color;
            }
            Instantiate(vfxPrefab, transform.position, Quaternion.identity, puzzleManager.transform);
            vfxPrefab.transform.localScale = transform.localScale;
            audioSource.PlayOneShot(puzzleManager.virusDeathSound, 0.8f);

            puzzleManager.CheckForVirusNumbers();
        }
    }

    public void InvokeAroundSyringes()
    {
        List<Syringe> syringes = new List<Syringe>();

        int syringeLayerMask = 1 << 8;

        RaycastHit2D leftSyringe = Physics2D.Raycast(transform.position, -transform.right, 100, syringeLayerMask);
        RaycastHit2D rightSyringe = Physics2D.Raycast(transform.position, transform.right, 100, syringeLayerMask);
        RaycastHit2D topSyringe = Physics2D.Raycast(transform.position, transform.up, 100, syringeLayerMask);
        RaycastHit2D botSyringe = Physics2D.Raycast(transform.position, -transform.up, 100, syringeLayerMask);

        if (topSyringe)
        {
            syringes.Add(topSyringe.transform.GetComponent<Syringe>());
        }
        if (botSyringe)
        {
            syringes.Add(botSyringe.transform.GetComponent<Syringe>());
        }
        if (leftSyringe)
        {
            syringes.Add(leftSyringe.transform.GetComponent<Syringe>());
        }
        if (rightSyringe)
        {
            syringes.Add(rightSyringe.transform.GetComponent<Syringe>());
        }


        for (int i = 0; i < syringes.Count; i++)
        {
            if (syringes[i].state != SyringeState.Destroying)
            {
                syringes[i].CheckSyringeState();
            }
        }

    }
}
