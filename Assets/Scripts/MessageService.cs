namespace Mapbox.Unity.Ar
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using GameSparks.Core;
  using Mapbox.Unity.Utilities;
  using Model;

  public class MessageService : MonoBehaviour
  {

    /// <summary>
    /// This class handles communication with gamesparks for
    /// removing, loading, and writing new messages. New Message
    /// objects are instantiated here.
    /// </summary>
    private static MessageService _instance;
    public static MessageService Instance { get { return _instance; } }

    public Transform mapRootTransform;

    public GameObject messagePrefabAR;

    void Awake()
    {
      _instance = this;
    }

    public void RemoveAllMessages()
    {
      new GameSparks.Api.Requests.LogEventRequest()
          .SetEventKey("REMOVE_MESSAGES")
          .Send((response) =>
          {
            if (!response.HasErrors)
            {
              XLogger.Error("移除所有消息成功");
            }
            else
            {
              XLogger.Error("移除所有消息失败");
            }
          });
    }

    public void LoadAllMessageNotShow()
    {
      XLogger.Info("开始查询所有的位置信息");
      List<LocationMessage> messages = new List<LocationMessage>();

      new GameSparks.Api.Requests.LogEventRequest().SetEventKey("LOAD_MESSAGE").Send((response) =>
      {
        if (!response.HasErrors)
        {
          List<GSData> locations = response.ScriptData.GetGSDataList("all_Messages");
          for (var e = locations.GetEnumerator(); e.MoveNext();)
          {

            GameObject MessageBubble = Instantiate(messagePrefabAR, mapRootTransform);
            GSData data = e.Current.GetGSData("data");
            LocationMessage locationMessage = new LocationMessage(double.Parse(data.GetString("LAT")),
                                                                       double.Parse(data.GetString("LON")),
                                                                        data.GetString("TEXT"));
            messages.Add(locationMessage);
            XLogger.Info(locationMessage.ToString());
          }
        }
        else
        {
          XLogger.Error("加载位置数据失败");
        }
      });
    }

    public void LoadAllMessages()
    {
      XLogger.Info("开始查询所有的位置信息");
      List<LocationMessage> messages = new List<LocationMessage>();

      new GameSparks.Api.Requests.LogEventRequest().SetEventKey("LOAD_MESSAGE").Send((response) =>
      {
        if (!response.HasErrors)
        {
          List<GSData> locations = response.ScriptData.GetGSDataList("all_Messages");
          for (var e = locations.GetEnumerator(); e.MoveNext();)
          {
            GSData data = e.Current.GetGSData("data");
            LocationMessage locationMessage = GetComponent<BubbleMessage>()
                  .CreateMessage(double.Parse(data.GetString("LAT")), double.Parse(data.GetString("LON")), data.GetString("TEXT"));
            messages.Add(locationMessage);
            XLogger.Info(locationMessage.ToString());
          }
        }
        else
        {
          XLogger.Error("查询位置数据失败");
        }
      });
      //pass list of objects to ARmessage provider so they can be placed
      ARMessageProvider.Instance.SetMessages(messages);
      ARMessageProvider.Instance.ShowAllMessages();
    }

    public void SaveMessage(double lat, double lon, string text)
    {
      new GameSparks.Api.Requests.LogEventRequest()

          .SetEventKey("SAVE_GEO_MESSAGE")
          .SetEventAttribute("LAT", lat.ToString())
          .SetEventAttribute("LON", lon.ToString())
          .SetEventAttribute("TEXT", text)
          .Send((response) =>
          {

            if (!response.HasErrors)
            {
              XLogger.Info("成功,消息已保存到了GameSparks");
              LocationMessage locationMessage = GetComponent<BubbleMessage>().CreateMessage(lat, lon, text);
              ARMessageProvider.Instance.AddMessage(locationMessage);
            }
            else
            {
              XLogger.Error("失败,消息未保存到了GameSparks");
            }
          });
    }
  }
}
