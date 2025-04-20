using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public void ResetAll()
    {
        foreach (var mono in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (mono is Resettable resettable)
            {
                resettable.DoReset();
            }
        }
    }
}