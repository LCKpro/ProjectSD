using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameUtils : MonoBehaviour
{
    public static void Log(string l)
    {
        Debug.Log(l);
    }

    public static void Log(string l, string i)
    {
        ConsoleProDebug.LogToFilter(l, i);
    }
}
