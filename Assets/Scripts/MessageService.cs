namespace Mapbox.Unity.Ar
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using GameSparks.Core;
    using Mapbox.Unity.Utilities;

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
                        Console.Instance.Log("Message Saved To GameSparks...", "lightblue");
                    }
                    else
                    {
                        Console.Instance.Log("Error Saving Message Data...", "lightblue");
                    }
                });
        }

        public void LoadAllMessageNotSet(){
            Debug.Log("开始查询所有的位置消息");
            Console.Instance.Log("开始查询所有的位置消息","lightblue");

            List<GameObject> messageObjectList = new List<GameObject>();

            new GameSparks.Api.Requests.LogEventRequest().SetEventKey("LOAD_MESSAGE").Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Console.Instance.Log("Received Player Data From GameSparks...", "lightblue");
                    List<GSData> locations = response.ScriptData.GetGSDataList("all_Messages");
                    for (var e = locations.GetEnumerator(); e.MoveNext();)
                    {

                        GameObject MessageBubble = Instantiate(messagePrefabAR, mapRootTransform);
                        Message message = MessageBubble.GetComponent<Message>();
                        GSData data = e.Current.GetGSData("data");

                        message.latitude = double.Parse(data.GetString("LAT"));
                        message.longitude = double.Parse(data.GetString("LON"));
                        message.text = data.GetString("TEXT");

                        messageObjectList.Add(MessageBubble);
                        Debug.Log("查询到的经度:" + message.longitude + " 纬度: " + message.latitude + " text:" + message.text);
                        Console.Instance.Log("查询到的经度:" + message.longitude + " 纬度: " + message.latitude + " text:" + message.text, "lightblue");
                    }
                }
                else
                {
                    Debug.Log("发生错误");
                    Console.Instance.Log("Error Loading Message Data...", "lightblue");
                }
            });
        }

        public void LoadAllMessages()
        {
            Debug.Log("开始查询所有的位置消息");

            List<GameObject> messageObjectList = new List<GameObject>();

            new GameSparks.Api.Requests.LogEventRequest().SetEventKey("LOAD_MESSAGE").Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Console.Instance.Log("Received Player Data From GameSparks...", "lightblue");
                    List<GSData> locations = response.ScriptData.GetGSDataList("all_Messages");
                    for (var e = locations.GetEnumerator(); e.MoveNext();)
                    {

                        GameObject MessageBubble = Instantiate(messagePrefabAR, mapRootTransform);
                        Message message = MessageBubble.GetComponent<Message>();
                        GSData data = e.Current.GetGSData("data");

                        message.latitude = double.Parse(data.GetString("LAT"));
                        message.longitude = double.Parse(data.GetString("LON"));
                        message.text = data.GetString("TEXT");

                        messageObjectList.Add(MessageBubble);
                        Debug.Log("查询到的经度:" + message.longitude + " 纬度: " + message.latitude + " text:" + message.text);
                    }
                }
                else
                {
                    Debug.Log("发生错误");
                    Console.Instance.Log("Error Loading Message Data...", "lightblue");
                }
            });
            //pass list of objects to ARmessage provider so they can be placed
            ARMessageProvider.Instance.LoadARMessages(messageObjectList);
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
                        Console.Instance.Log("Message Saved To GameSparks...", "lightblue");
                        Debug.Log("Message Saved To GameSparks...");
                    }
                    else
                    {
                        Console.Instance.Log("Message Saved To GameSparks...,fail", "lightblue");

                    }
                });
        }
    }
}
