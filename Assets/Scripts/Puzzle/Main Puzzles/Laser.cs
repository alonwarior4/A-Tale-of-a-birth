using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //Cache Section
    private Syringe parentSyringe;

    WaitForEndOfFrame waitForEndFrame = new WaitForEndOfFrame();

    public float moveSpeed;

    private void Awake()
    {
        parentSyringe = transform.parent.GetComponent<Syringe>();
    }


    public void DrawLaser(Vector3 firstPos, Vector3 LastPos)
    {
        StartCoroutine(DrawLaserCoroutine(firstPos, LastPos));
    }

    private IEnumerator DrawLaserCoroutine(Vector3 firstLaserPos, Vector3 LastLaserPos)
    {
        transform.position = firstLaserPos;

        if (parentSyringe.isSyringeVertical)
        {
            while (Mathf.Abs(transform.position.y - LastLaserPos.y) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, LastLaserPos, moveSpeed * Time.deltaTime);
                yield return waitForEndFrame;
            }

            transform.position = LastLaserPos;
        }
        else
        {
            while (Mathf.Abs(transform.position.x - LastLaserPos.x) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, LastLaserPos, moveSpeed * Time.deltaTime);
                yield return waitForEndFrame;
            }

            transform.position = LastLaserPos;
        }
    }


}
