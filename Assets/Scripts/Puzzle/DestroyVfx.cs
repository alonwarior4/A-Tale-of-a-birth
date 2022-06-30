using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyVfx : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 0.6f);
    }
}
