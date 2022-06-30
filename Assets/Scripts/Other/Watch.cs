using UnityEngine;

public class Watch : MonoBehaviour
{
    [SerializeField] GameObject bigClockHand;
    [SerializeField] GameObject smallClockHand;

    [SerializeField] float rotateSpeed;
    private void Start()
    {
        bigClockHand.transform.eulerAngles = new Vector3(0 ,0 ,UnityEngine.Random.Range(0 , 359));
        smallClockHand.transform.eulerAngles = new Vector3(0 ,0 ,UnityEngine.Random.Range(0 , 359));
    }

    private void Update()
    {
        RotateHandles();
    }

    private void RotateHandles()
    {      
        smallClockHand.transform.Rotate(Vector3.back * rotateSpeed * Time.deltaTime);
        bigClockHand.transform.Rotate(Vector3.back * rotateSpeed * 12 * Time.deltaTime);
    }

}
