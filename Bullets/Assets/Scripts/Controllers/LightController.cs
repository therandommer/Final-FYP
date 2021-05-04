using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LightController : MonoBehaviour
{
    public Light2D thisLight;
    public void EnableLight()
	{
        thisLight.enabled = true;
	}
    public void DisableLight()
	{
        thisLight.enabled = false;
	}
}
