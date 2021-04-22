using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
	[SerializeField]
	Renderer rend;
	[SerializeField]
	private float scrollSpeed = 2;

	void Start()
	{
		rend = GetComponent<Renderer>();
	}
	void Update()
	{
			float offset = Time.deltaTime * scrollSpeed * Time.timeScale;
			//rend.material.SetTextureOffset(0, new Vector2(0, offset));
			rend.material.mainTextureOffset = new Vector2(0, Time.time * scrollSpeed);
		
	}
}
