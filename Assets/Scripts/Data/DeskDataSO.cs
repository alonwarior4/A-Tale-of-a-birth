using UnityEngine;


public enum DeskJobs
{
    None , DeactiveContinueButton , ActivateContinueButton , InstantiatePuzzlePrefab , FadeInSprite , ChangeSprite , TriggerAnimation , fadeOutsprite 
        , DeleteGameObject 
}



[CreateAssetMenu(menuName = "Desk Data" , fileName = "Desk Scene 1" , order = 2)]
public class DeskDataSO : ScriptableObject
{
    //[Reorderable]
    public D_Actions[] deskActions;
}


[System.Serializable]
public class D_Actions
{
    public DeskJobs d_DeskJob = DeskJobs.None;
    public GameObject d_PuzzlePrefab = null;
    //[Tooltip("If check , puzzle cannot be solved and after some time game continue automatically")]
    //public bool d_CantResolve = false;
    [Tooltip("for unresolve puzzles , game automatically continue after")]
    public float d_PuzzleTryTime = 0;

    [Tooltip("Check if this action will not be wait till end to continue story chain")]
    public bool d_IsSimultaneously = false;
    public float d_WaitTime = 0;

    public bool d_IsCamera = false;

    public Sprite d_Sprite = null;
    public Sprite d_ReplaceSprite = null;
    public float d_Ftime = 0;
    public int d_SortingOrder = 0;
    public string d_SortingLayerName = "";
    public AudioClip d_Sound = null;
    public float d_Volume = 0;

    public bool d_SetWallChild = false;

    public bool d_UseLater = false;
    public bool d_isExistInScene = false;

    public string d_ObjName = "";
    public string d_triggerName = "";
    public float d_animSpeed = 0;

    public Vector3 d_Position;
    public Quaternion d_Rotation;
    public Vector3 d_Scale;
}
