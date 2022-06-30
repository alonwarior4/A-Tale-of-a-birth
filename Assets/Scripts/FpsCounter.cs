using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fpsText;

    private void Start()
    {
        Application.targetFrameRate = 30;
        StartCoroutine(ShowFramePerSecond());
    }

    IEnumerator ShowFramePerSecond()
    {
        while (true)
        {
            float fps = (int)(1 / Time.unscaledDeltaTime);
            fpsText.text = fps.ToString();
            yield return new WaitForSeconds(0.25f);
        }
        
    }

}
