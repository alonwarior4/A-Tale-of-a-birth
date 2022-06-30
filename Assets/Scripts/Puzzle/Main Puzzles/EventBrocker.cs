

public static class EventBrocker 
{

    //public static UnityEvent OnPuzzleCheck = new UnityEvent();        //any change in puzzle virus or syringes trigger this event

    public delegate void PuzzleCheck();
    public static event PuzzleCheck OnBulletHit;
    public static event PuzzleCheck OnSyringeSwap;
    public static event PuzzleCheck OnVirusDestroy;

    public static void CallOnBulletHit()
    {
        OnBulletHit?.Invoke();
    }

    public static void CallOnSyringeSwap()
    {
        OnSyringeSwap?.Invoke();
    }

    public static void CallOnVirusDestroy()
    {
        OnVirusDestroy?.Invoke();
    }
}
