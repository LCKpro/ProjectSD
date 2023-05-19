using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameUtils : MonoBehaviour
{
    private static System.Random _sysRand;

    public static System.Random SysRand
    {
        get
        {
            if (_sysRand == null)
                _sysRand = new System.Random();
            return _sysRand;
        }
    }

    public static void Log(string l)
    {
        Debug.Log(l);
    }

    public static void Log(string l, string i)
    {
        ConsoleProDebug.LogToFilter(l, i);
    }

    public static void Error(string l)
    {
        Debug.LogError(l);
    }

    public static void LogException(Exception ex)
    {
        Debug.LogError($"{ex.GetType().FullName}\n{ex.Message}\n{ex.Source}\n{ex.StackTrace}");
    }

    public static int RandomWeightedList(IEnumerable<int> list)
    {
        int rand = RandomRange(0, list.Sum());
        int v = 0;
        for (int i = 0; i < list.Count(); i++)
        {
            v += list.ElementAt(i);
            if (v > rand) return i;
        }

        return list.Count() - 1;
    }

    /// <summary>
    /// ratio가 0.1이면 true일 확률이 10%
    /// </summary>
    public static bool RandomBoolFromPercent(float ratio)
    {
        var r = SysRand.NextDouble();
        return r <= ratio;
    }

    public static bool RandomBool()
    {
        return RandomBoolFromPercent(0.5f);
    }

    public static T RandomItem<T>(IEnumerable<T> list)
    {
        if (!list.Any())
            return default(T);
        int index = SysRand.Next(0, list.Count());
        return list.ElementAt(index);
    }

    public static int RandomRange(int min, int max)
    {
        double val = SysRand.NextDouble();
        int item = 0;
        val *= max - min;
        item = ((int)val + min);
        return item;
    }

    public static float RandomRange(float min, float max)
    {
        double val = SysRand.NextDouble();
        float item = 0f;
        val *= max - min;
        item = (float)(val + min);
        return item;
    }

    public static float ConvertToSingle(string value)
    {
        return float.Parse(value, CultureInfo.InvariantCulture);
    }
}
