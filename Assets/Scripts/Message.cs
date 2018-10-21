﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;

public class Message : MonoBehaviour
{

    public TextMesh messageText;

    public void SetText(string text)
    {
        //TODO: here we would need to size the text and
        //mesage bubble according to the length of text.
        //right now this only replaces spaces with a new
        //line character
        string newText = text.Replace(" ", "\n");
        messageText.text = newText;
    }

    void Start()
    {
        //set the camera in the text's Canvas component, I needed to add this
        //so the layer of the text always shows up over the message sprite.
        messageText.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    void Update()
    {
        //make sure the bubble is always facing the camera
        transform.LookAt(Camera.main.transform);
    }

    public static Message Factory(LocationMessage message)
    {
        Message msg = message.Parent.GetComponent<Message>();
        return msg;
    }
}
