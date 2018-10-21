using UnityEngine;
using System.Collections;
using Mapbox.Unity.Utilities;
/// <summary>
/// 日志工具类
/// </summary>
public static class XLogger
{
    /// <summary>
    /// Error the specified msg.
    /// </summary>
    /// <param name="msg">Message.</param>
    public static void Error(string msg)
    {
        Console.Instance.Log(msg, "lightred");
        UnityEngine.Debug.Log("error: " + msg);
    }

    /// <summary>
    /// Info the specified msg.
    /// </summary>
    /// <param name="msg">Message.</param>
    public static void Info(string msg)
    {
        Console.Instance.Log(msg, "lightgreen");
        UnityEngine.Debug.Log("info: " + msg);

    }
    /// <summary>
    /// Debug the specified msg.
    /// </summary>
    /// <param name="msg">Message.</param>
    public static void Debug(string msg)
    {
        Console.Instance.Log(msg, "lightblue");
        UnityEngine.Debug.Log("debug: " + msg);

    }

}
