using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    //Virus Parent Cash
    Virus virusParent;

    //Bullet Config
    public thisColor thisBulletColor;
    Vector3 bulletDefaultScale;
    Vector3 bulletdefaultPos;

    PuzzleManager puzzleManager;

    private void Awake()
    {
        virusParent = transform.parent.GetComponent<Virus>();
        puzzleManager = virusParent.transform.parent.GetComponentInParent<PuzzleManager>();
        thisBulletColor = virusParent.thisVirusColor;
        GetComponent<SpriteRenderer>().color = SetSpriteColor(virusParent.thisVirusColor);

        bulletDefaultScale = transform.localScale;
        bulletdefaultPos = transform.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.GetComponent<Virus>())
        {
            //check if target has two color
            Virus otherVirus = otherCollider.GetComponent<Virus>();
            bool otherVirusHasTwoColor = otherVirus.thisVirusColor == thisColor.BlueRed || otherVirus.thisVirusColor == thisColor.YellowRed;
            bool areColorSame = otherVirus.thisVirusColor == virusParent.thisVirusColor;

            if (otherVirusHasTwoColor || areColorSame)
            {
                PutBulletBack();
            }
            else
            {
                ChangeTargetVirusColor(otherCollider, otherVirus);
            }
        }
        else
        {
            PutBulletBack();
        }
    }

    private void ChangeTargetVirusColor(Collider2D otherCollider, Virus targetVirus)
    {
        PutBulletBack();

        //change target virus 
        targetVirus.thisVirusColor = thisBulletColor;
        GameObject virusColor = otherCollider.GetComponent<Virus>().VirusForColoring;
        virusColor.GetComponent<SpriteRenderer>().color = SetSpriteColor(thisBulletColor);
        targetVirus.GetComponent<Animator>().SetTrigger("GoColor");

        //change target bullet
        Bullet otherVirusBullet = targetVirus.transform.GetChild(0).GetComponent<Bullet>();
        otherVirusBullet.GetComponent<SpriteRenderer>().color = SetSpriteColor(thisBulletColor);
        otherVirusBullet.thisBulletColor = thisBulletColor;

        puzzleManager.audioSource.PlayOneShot(puzzleManager.virusChangeColor , 0.5f);
        targetVirus.InvokeAroundSyringes();
    }

    private void PutBulletBack()
    {        
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        virusParent.bulletHasVelocity = false;
        transform.localPosition = bulletdefaultPos;
        transform.localScale = bulletDefaultScale;      
    }

    public Color SetSpriteColor(thisColor color)
    {
        switch (color)
        {
            case thisColor.None:
                return Color.black;
            case thisColor.Blue:
                return new Color(0.02f, 0.63f, 0.90f);
            case thisColor.Red:
                return new Color(0.93f, 0.17f, 0.28f);
            case thisColor.Yellow:
                return new Color(1, 0.83f, 0.06f);
            case thisColor.BlueRed:
                return new Color(0.63f, 0, 1);
            case thisColor.YellowRed:
                return new Color(1, 0.47f, 0);
            default:
                return Color.black;
        }
    }



}
