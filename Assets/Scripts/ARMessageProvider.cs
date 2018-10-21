namespace Mapbox.Unity.Ar
{
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;
  using Mapbox.Unity.Utilities;
  using Mapbox.Utils;
  using Mapbox.Unity.Map;
  using Model;

  public class ARMessageProvider : MonoBehaviour
  {
    public static ARMessageProvider Instance { get; private set; }



    [SerializeField]
    private AbstractMap _map;

    [HideInInspector]
    private List<LocationMessage> locationMessages = new List<LocationMessage>();

    [HideInInspector]
    public List<GameObject> currentMessages = new List<GameObject>();
    [HideInInspector]
    public bool deviceAuthenticated;
    private bool gotInitialAlignment;

    public Mapbox.Unity.Location.DeviceLocationProvider deviceLocation;

    void Awake()
    {
      Instance = this;
    }

    private void _removeAllMessages()
    {
      foreach (LocationMessage locationMessage in locationMessages)
      {
        Destroy(locationMessage.Parent);
      }
      locationMessages.Clear();
    }


    private IEnumerator _showARMessages(List<LocationMessage> messages)
    {
      yield return new WaitForSeconds(2f);

      foreach (LocationMessage message in messages)
      {
        _showARMessage(message);
      }
    }

    private void _showARMessage(LocationMessage message)
    {
      Vector3 _targetPosition = _map.Root.TransformPoint(Conversions.GeoToWorldPosition(message.Latitude, message.Longitude, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
      Message msg = message.Parent.GetComponent<Message>();
      msg.SetText(message.Text);
      msg.transform.position = _targetPosition;
    }

    public void ShowAllMessages()
    {
      StartCoroutine(_showARMessages(locationMessages));
    }

    public void RemoveAllMessages()
    {
      _removeAllMessages();
    }

    public void SetMessages(List<LocationMessage> messages)
    {
      locationMessages = messages;
    }

    public void AddMessage(LocationMessage message)
    {
      locationMessages.Add(message);
      _showARMessage(message);
    }

    public void UpdateCurrentLocation(Vector2d currentLocation)
    {
      if (locationMessages.Count > 0)
      {
        foreach (LocationMessage locationMessage in locationMessages)
        {
          Vector3 _targetPosition = _map.Root.TransformPoint(Conversions.GeoToWorldPosition(locationMessage.Latitude, locationMessage.Longitude, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
          locationMessage.Parent.GetComponent<Message>().transform.position = _targetPosition;
        }
      }
    }

    /// <summary>
    /// Gots the alignment.
    /// </summary>
    public void GotAlignment()
    {
      XLogger.Info("对齐成功!");

      if (DeviceAuthentication.Instance.DeviceAuthenticated)
      {
        if (!gotInitialAlignment)
        {
          XLogger.Info("开始初始化UI和消息....");
          gotInitialAlignment = true;
          //set UI active once we are authenticated
          UIBehavior.Instance.ShowUI();
          //load first messages
          MessageService.Instance.LoadAllMessages();
        }
        else
        {
          XLogger.Info("更新信息位置....");
          UpdateCurrentLocation(deviceLocation.CurrentLocation.LatitudeLongitude);
        }
      }
      else
      {
        XLogger.Error("设备未授权");
      }
    }

    public void RemoveCurrentMessages()
    {
      foreach (GameObject messageObject in currentMessages)
      {
        Destroy(messageObject);
      }
      currentMessages.Clear();
    }
  }
}

