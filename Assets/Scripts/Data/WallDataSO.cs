using UnityEngine;
using SubjectNerd.Utilities;



public enum WallActions
{
    None , fadeInSprite ,  InstantiateAnimationPrefab , ChangeSprite , TriggerAnimation ,
    ActiveContinueButton , DeactiveContinueButton , FadeOutSprite , DeleteGameObject , EnableIntractScene , DisableInteractScene
}


[CreateAssetMenu(menuName = "Wall Data" , fileName = "Wall Scene 1" , order = 1)]
public class WallDataSO : ScriptableObject
{
    //[Reorderable]
    public W_Actions[] actions;
}

[System.Serializable]
public class W_Actions
{
    public WallActions w_Action = WallActions.None;
    public Triggers[] w_Trigger;
    //public bool w_WaitToEnd;
    

    [Tooltip("if true this object will be saved and can be access later to alter")]
    public bool w_UseLater;
    [Tooltip("If false , This action has to finish to go next . if true , Can execute and then fill the wait field to wait for some of its time then continue chain")]
    public bool w_IsSimultaneously;
    public float w_WaitTime;

    public bool w_IsCamera;

    public Sprite w_sprite;
    public Sprite w_ReplaceSprite;
    public float w_FTime;
    public int w_SortingLayer;
    public AudioClip w_FSound;
    public float w_audioVolume;

    public bool w_SetChildDesk;

    public GameObject w_AnimPrefab;
    public string w_ObjName;
    public bool w_IsExistInScene;
    public string w_sortingLayerName;

    public Vector3 w_Position;
    public Quaternion w_Rotation;
    public Vector3 w_Scale;
}


[System.Serializable]
public class Triggers
{
    public string TriggerName;
    public float animSpeed;
    [Tooltip("Time To Continue Showing animtion One After Another")]
    public float animWait;
}
