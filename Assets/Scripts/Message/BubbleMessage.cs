using UnityEngine;
using System.Collections;
using Model;

/// <summary>
/// 气泡框消息
/// </summary>
public class BubbleMessage : MonoBehaviour
{
  public Transform mapRootTransform;
  public GameObject messagePrefabAR;

  /// <summary>
  /// 气泡框的位置消息
  /// </summary>
  [HideInInspector]
  private LocationMessage _locationMessage;


  /// <summary>
  /// 创建一条消息
  /// </summary>
  /// <param name="latitude">纬度</param>
  /// <param name="longitude">经度</param>
  /// <param name="text">文本</param>
  /// <returns></returns>
  public LocationMessage CreateMessage(double latitude, double longitude, string text)
  {
    GameObject MessageBubble = Instantiate(messagePrefabAR, mapRootTransform);
    LocationMessage locationMessage = new LocationMessage(latitude, longitude, text);
    locationMessage.Parent = MessageBubble;
    return locationMessage;
  }

}
