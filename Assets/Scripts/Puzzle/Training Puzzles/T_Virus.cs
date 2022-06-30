using UnityEngine;

public class T_Virus : MonoBehaviour
{
    [SerializeField] GameObject destroyVfx;
    [SerializeField] float puzzleScale;
    private void OnDisable()
    {
        //check if disable for restart or kill virus
        GameObject vfxExplode =  Instantiate(destroyVfx, transform.position, Quaternion.identity) as GameObject;
        vfxExplode.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;

        vfxExplode.transform.localScale = Vector3.one * puzzleScale;
    }
}
