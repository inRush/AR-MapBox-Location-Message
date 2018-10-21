using System;
using UnityEngine;

namespace Model
{
  public class LocationMessage : MonoBehaviour
  {
    public Transform mapRootTransform;
    public GameObject messagePrefabAR;

    public double Latitude
    {
      get; set;
    }
    public double Longitude
    {
      get; set;
    }
    public string Text
    {
      get; set;
    }
    public GameObject Parent
    {
      get; set;
    }

    public LocationMessage(double latitude, double longitude, string text)
    {
      Latitude = latitude;
      Longitude = longitude;
      Text = text;

    }

    public override string ToString()
    {
      return "经度: " + Longitude + " 纬度: " + Latitude + " text: " + Text;
    }
  }
}
