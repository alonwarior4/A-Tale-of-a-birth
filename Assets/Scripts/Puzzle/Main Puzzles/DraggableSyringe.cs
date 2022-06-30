using UnityEngine;

public class DraggableSyringe : MonoBehaviour
{
    public Syringe target;

    public void SetTargetConfigs()
    {      
        transform.position = target.transform.position;
        transform.localRotation = target.transform.localRotation;
        transform.localScale = target.defaultScale;


        transform.GetChild(0).GetComponent<SpriteRenderer>().color = SetColor(target.thisSyringeColor);
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = SetColor(target.thisSyringeColor);
    }

    private Color SetColor(thisColor targetColor)
    {
        switch (targetColor)
        {
            //case thisColor.Blue:
            //    break;
            case thisColor.Red:
                return new Color(0.93f, 0.17f, 0.29f, 1);
            case thisColor.Yellow:
                return new Color(1, 0.84f, 0.063f, 1);
            case thisColor.Blue:
                return new Color(0.023f, 0.64f, 0.91f);
            default:
                return Color.white;
        }
    }


    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.GetComponent<Syringe>())
        {
            target.isOnAnotherSyringe = true;
            target.otherSyringe = otherCollider.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.GetComponent<Syringe>())
        {
            target.isOnAnotherSyringe = false;
            target.otherSyringe = null;
        }
    }
}
