using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFindCamera : MonoBehaviour
{
    Canvas thisCanvas;
    void Start()
    {
        thisCanvas = GetComponent<Canvas>();
    }
    void Update()
    {
        if(!thisCanvas.worldCamera)
		{
            thisCanvas.worldCamera = FindObjectOfType<Camera>();
		}
    }
}
