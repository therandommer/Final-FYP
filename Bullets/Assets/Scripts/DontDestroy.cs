using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
	public static DontDestroy thisInstance = null;
	void Awake()
	{
		if (thisInstance != null)
		{
			DestroyImmediate(gameObject);
		}

		else
		{
			DontDestroyOnLoad(gameObject);
			thisInstance = this;
		}
	}
}
